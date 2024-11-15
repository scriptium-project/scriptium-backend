using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using writings_backend_dotnet.DB;
using writings_backend_dotnet.Models;

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
            string? jsonString = await _db.Cache
                .Where(c => c.Key == key && c.ExpirationDate < DateTime.Now + TimeSpan.FromSeconds(10))
                .Select(c => c.Data)
                .FirstOrDefaultAsync();

            if (jsonString == null) return default;

            try
            {
                T data = JsonSerializer.Deserialize<T>(jsonString)!;
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
                    Data = jsonData
                };

                await _db.Cache.AddAsync(cacheEntry);
            }

            await _db.SaveChangesAsync();
        }

    }

}