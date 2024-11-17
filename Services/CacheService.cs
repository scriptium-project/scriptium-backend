using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;
using static writings_backend_dotnet.Utility.Utility;

namespace writings_backend_dotnet.Services
{
    public interface ICacheService
    {
        Task<T?> GetCachedDataAsync<T>(string key);
        Task SetCacheDataAsync<T>(string key, T data);
    }

    public class CacheService(ApplicationDBContext db) : ICacheService
    {
        private readonly ApplicationDBContext _db = db;

        public async Task<T?> GetCachedDataAsync<T>(string key)
        {
            var cache = await _db.Cache
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


       public async Task SetCacheDataAsync<T>(string key, T data)
        {
            string jsonData = JsonSerializer.Serialize(data);

            var existingCacheEntry = await _db.Cache.FirstOrDefaultAsync(c => c.Key == key);

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
                    ExpirationDate = DateTime.UtcNow + TimeSpan.FromDays(1)
                };

                _db.Cache.Add(cacheEntry);

                var cacheR = new CacheR
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