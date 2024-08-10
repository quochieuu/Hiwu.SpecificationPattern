using Hiwu.SpecificationPattern.Application.Enums;
using Hiwu.SpecificationPattern.Application.Interfaces.Caching;
using Hiwu.SpecificationPattern.Domain.Settings;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net;

namespace Hiwu.SpecificationPattern.Caching.Redis
{
    /// <summary>
    /// Manages caching using Redis as the backend store.
    /// </summary>
    public partial class RedisCacheManager : ICacheManager, IDisposable
    {
        private readonly IRedisConnectionWrapper _redisConnectionWrapper;
        private readonly IDatabase _database;
        private readonly RedisSettings _redisSettings;

        public RedisCacheManager(IRedisConnectionWrapper redisConnectionWrapper, RedisSettings redisSettings)
        {
            _redisSettings = redisSettings;

            if (string.IsNullOrEmpty(_redisSettings.RedisConnectionString))
                throw new ArgumentException("Redis connection string is empty", nameof(_redisSettings.RedisConnectionString));

            _redisConnectionWrapper = redisConnectionWrapper;
            _database = _redisConnectionWrapper.GetDatabase(_redisSettings.RedisDatabaseId ?? (int)RedisDatabaseNumber.Cache);
        }

        /// <summary>
        /// Retrieves all keys matching the specified prefix.
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        protected virtual IEnumerable<RedisKey> GetKeys(EndPoint endPoint, string prefix = null)
        {
            var server = _redisConnectionWrapper.GetServer(endPoint);

            var keys = server.Keys(_database.Database, string.IsNullOrEmpty(prefix) ? null : $"{prefix}*")
                        .Where(key => !key.ToString().Equals(_redisSettings.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase));

            return keys;
        }

        /// <summary>
        /// Retrieves the cached item asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual async Task<T> GetAsync<T>(string key)
        {
            var serializedItem = await _database.StringGetAsync(key);
            return !serializedItem.HasValue ? default : JsonConvert.DeserializeObject<T>(serializedItem);
        }

        /// <summary>
        /// Checks whether the specified cache key exists asynchronously.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual async Task<bool> IsSetAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        /// <summary>
        /// Retrieves the cached string asynchronously.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string cacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);

            if (cachedResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cachedResponse;
        }

        /// <summary>
        /// Retrieves the cached item asynchronously, or adds it if it does not exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="acquire"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire, int? cacheTime = null)
        {
            //item already is in cache, so return it
            if (await IsSetAsync(key))
                return await GetAsync<T>(key);

            //or create it using passed function
            var result = await acquire();

            //and set in cache (if cache time is defined)
            if ((cacheTime ?? _redisSettings.CacheTime) > 0)
                await SetAsync(key, result, cacheTime ?? _redisSettings.CacheTime);

            return result;
        }

        /// <summary>
        /// Retrieves the cached item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            var serializedItem = _database.StringGet(key);
            return !serializedItem.HasValue ? default : JsonConvert.DeserializeObject<T>(serializedItem);
        }

        /// <summary>
        /// Retrieves the cached item, or adds it if it does not exist.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="acquire"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key, Func<T> acquire, int? cacheTime = null)
        {
            if (IsSet(key)) return Get<T>(key);

            var result = acquire();

            if ((cacheTime ?? _redisSettings.CacheTime) > 0)
                Set(key, result, cacheTime ?? _redisSettings.CacheTime);

            return result;
        }

        /// <summary>
        /// Sets the specified item in the cache asynchronously.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        public async Task SetAsync(string key, object data, int cacheTime)
        {
            if (data == null) return;

            var expiresIn = TimeSpan.FromMinutes(cacheTime);
            var serializedItem = JsonConvert.SerializeObject(data);
            await _database.StringSetAsync(key, serializedItem, expiresIn);
        }

        /// <summary>
        /// Sets the specified item in the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null) return;

            var expiresIn = TimeSpan.FromMinutes(cacheTime);
            var serializedItem = JsonConvert.SerializeObject(data);
            _database.StringSet(key, serializedItem, expiresIn);
        }

        /// <summary>
        /// Checks whether the specified cache key exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return _database.KeyExists(key);
        }

        /// <summary>
        /// Removes the specified item from the cache.
        /// </summary>
        /// <param name="key"></param>
        public virtual void Remove(string key)
        {
            if (!key.Equals(_redisSettings.RedisDataProtectionKey, StringComparison.OrdinalIgnoreCase))
                _database.KeyDelete(key);
        }

        /// <summary>
        /// Removes items from the cache that match the specified prefix.
        /// </summary>
        /// <param name="prefix"></param>
        public virtual void RemoveByPrefix(string prefix)
        {
            foreach (var endPoint in _redisConnectionWrapper.GetEndPoints())
            {
                var keys = GetKeys(endPoint, prefix);
                _database.KeyDelete(keys.ToArray());
            }
        }

        /// <summary>
        /// Clears all cached items.
        /// </summary>
        public virtual void Clear()
        {
            foreach (var endPoint in _redisConnectionWrapper.GetEndPoints())
            {
                var keys = GetKeys(endPoint).ToArray();
                _database.KeyDelete(keys);
            }
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="RedisCacheManager"/>.
        /// </summary>
        public void Dispose()
        {
            _redisConnectionWrapper?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
