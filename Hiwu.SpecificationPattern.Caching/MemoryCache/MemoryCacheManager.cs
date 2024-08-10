using Hiwu.SpecificationPattern.Application.Interfaces.Caching;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Hiwu.SpecificationPattern.Caching.MemoryCache
{
    public class MemoryCacheManager : IMemoryCacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Retrieves the cached string value for the specified cache key.
        /// </summary>
        /// <param name="cacheKey">The key of the cache item.</param>
        /// <returns>The cached string value, or null if not found.</returns>
        public async Task<string> GetAsync(string cacheKey)
        {
            // Retrieve the cached value as a string
            return await Task.FromResult(_memoryCache.TryGetValue(cacheKey, out string cachedValue) ? cachedValue : null);
        }

        /// <summary>
        /// Stores the specified data in the cache with a given time-to-live.
        /// </summary>
        /// <param name="cacheKey">The key to identify the cache item.</param>
        /// <param name="data">The data to cache.</param>
        /// <param name="cacheTime">The time-to-live of the cached item in seconds.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SetAsync(string cacheKey, object data, int cacheTime)
        {
            if (data == null) return;

            // Serialize the data to JSON
            var serializedItem = JsonConvert.SerializeObject(data);

            // Set the data in the cache with the specified expiration time
            _memoryCache.Set(cacheKey, serializedItem, TimeSpan.FromSeconds(cacheTime));

            await Task.CompletedTask;
        }

        /// <summary>
        /// Retrieves the cached value for the specified cache key and deserializes it to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the cached value into.</typeparam>
        /// <param name="cacheKey">The key of the cache item.</param>
        /// <returns>The deserialized cached value of type T, or the default value if not found.</returns>
        public async Task<T> GetAsync<T>(string cacheKey)
        {
            // Retrieve the cached value as JSON
            var cachedValue = await GetAsync(cacheKey);
            if (cachedValue == null) return default;

            // Deserialize the JSON value to the specified type
            return JsonConvert.DeserializeObject<T>(cachedValue);
        }

        /// <summary>
        /// Stores the specified data in the cache with a given time-to-live, and serializes it to JSON.
        /// </summary>
        /// <typeparam name="T">The type of the data to cache.</typeparam>
        /// <param name="cacheKey">The key to identify the cache item.</param>
        /// <param name="data">The data to cache.</param>
        /// <param name="cacheTime">The time-to-live of the cached item in seconds.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SetAsync<T>(string cacheKey, T data, int cacheTime)
        {
            // Serialize the data to JSON
            var serializedItem = JsonConvert.SerializeObject(data);

            // Set the data in the cache with the specified expiration time
            _memoryCache.Set(cacheKey, serializedItem, TimeSpan.FromSeconds(cacheTime));

            await Task.CompletedTask;
        }

        /// <summary>
        /// Checks if an item with the specified key is present in the cache.
        /// </summary>
        /// <param name="cacheKey">The key of the cache item.</param>
        /// <returns>True if the item exists in the cache; otherwise, false.</returns>
        public bool IsSet(string cacheKey)
        {
            return _memoryCache.TryGetValue(cacheKey, out _);
        }

        /// <summary>
        /// Removes the item with the specified key from the cache.
        /// </summary>
        /// <param name="cacheKey">The key of the cache item to remove.</param>
        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
    }
}
