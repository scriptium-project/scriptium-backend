using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using scriptium_backend_dotnet.DB;
using scriptium_backend_dotnet.Models;

namespace scriptium_backend_dotnet.Services
{
    public interface ICacheService
    {
        Task<T?> GetCachedDataAsync<T>(string key);
        Task SetCacheDataAsync<T>(string key, T data, double? ExpirationDayCount = 10);
    }

    public class CacheService(ApplicationDBContext db) : ICacheService
    {
        private readonly ApplicationDBContext _db = db ?? throw new ArgumentNullException(nameof(db));

        public async Task<T?> GetCachedDataAsync<T>(string key)
        {
            Cache? cache = await _db.Cache
                .Where(c => c.Key == key && c.ExpirationDate > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (cache == null) return default;

            try
            {
                T data = JsonSerializer.Deserialize<T>(cache.Data)!;

                var cacheR = new CacheR
                {
                    CacheId = cache.Id,
                    FetchedAt = DateTime.UtcNow
                };

                _db.CacheR.Add(cacheR);

                await _db.SaveChangesAsync();

                return data;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Deserialization error: {ex.Message}");
                return default;
            }
        }


        public async Task SetCacheDataAsync<T>(string key, T data, double? ExpirationDayCount)
        {
            string jsonData = JsonSerializer.Serialize(data);

            Cache? existingCacheEntry = await _db.Cache.FirstOrDefaultAsync(c => c.Key == key);

            if (existingCacheEntry != null)
            {
                existingCacheEntry.Data = jsonData;
                _db.Cache.Update(existingCacheEntry);
            }
            else
            {
                var cacheEntry = new Cache
                {
                    Key = key,
                    Data = jsonData,
                    ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(ExpirationDayCount ?? 10)
                };

                _db.Cache.Add(cacheEntry);

                CacheR cacheR = new()
                {
                    Cache = cacheEntry,
                    FetchedAt = DateTime.UtcNow
                };
                _db.CacheR.Add(cacheR);
            }

            await _db.SaveChangesAsync();
        }


    }

}