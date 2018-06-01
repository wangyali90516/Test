using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J.Base.Lib;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace UserToAssetTool
{
    public static class RedisCacheHelper
    {
        /// <summary>
        ///     Clears the data from cache.
        /// </summary>
        /// <param name="redisDatabase">The redis client.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static Task<bool> ClearDataFromCacheAsync(this IDatabase redisDatabase, string cacheName, string cacheId)
        {
            string cacheKey = $"{cacheName}:{cacheId}";
            return redisDatabase.KeyDeleteAsync(cacheKey);
        }

        /// <summary>
        ///     Reads the data from cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisDatabase">The redis client.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <returns>T.</returns>
        public static async Task<T> ReadDataFromCacheAsync<T>(this IDatabase redisDatabase, string cacheName, string cacheId)
        {
            string cacheKey = $"{cacheName}:{cacheId}";

            try
            {
                string data = await redisDatabase.StringGetAsync(cacheKey);
                return data.IsNotNullOrWhiteSpace() ? JsonConvert.DeserializeObject<T>(data) : default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        ///     Sets the data to cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisDatabase">The redis client.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <param name="cacheId">The cache identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns>Task.</returns>
        public static Task<bool> SetDataToCacheAsync<T>(this IDatabase redisDatabase, string cacheName, string cacheId, T data, TimeSpan cacheTime)
        {
            string cacheKey = $"{cacheName}:{cacheId}";
            string dataJson = JsonConvert.SerializeObject(data);
            return redisDatabase.StringSetAsync(cacheKey, dataJson, cacheTime);
        }

        #region Hash存储

        public static IList<T> HashGetAllFromCache<T>(this IDatabase redisDatabase, string cacheName, string cacheId)
        {
            string cacheKey = $"{cacheName}:{cacheId}";

            try
            {
                var data = redisDatabase.HashGetAll(cacheKey);
                return data.Select(item => JsonConvert.DeserializeObject<T>(item.Value)).ToList();
            }
            catch (Exception)
            {
                return default(IList<T>);
            }
        }

        public static async Task<T> HashGetFromCacheAsync<T>(this IDatabase redisDatabase, string cacheName, string cacheId, string field)
        {
            string cacheKey = $"{cacheName}:{cacheId}";
            try
            {
                string data = await redisDatabase.HashGetAsync(cacheKey, field);
                return data.IsNotNullOrWhiteSpace() ? JsonConvert.DeserializeObject<T>(data) : default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static async Task<bool> HashSetToCacheAsync<T>(this IDatabase redisDatabase, string cacheName, string cacheId, string field, T data, TimeSpan cacheTime)
        {
            try
            {
                string cacheKey = $"{cacheName}:{cacheId}";
                string dataJson = JsonConvert.SerializeObject(data);
                bool exists = await redisDatabase.KeyExistsAsync(cacheKey);
                bool result = await redisDatabase.HashSetAsync(cacheKey, field, dataJson);
                if (!exists)
                {
                    await redisDatabase.KeyExpireAsync(cacheKey, cacheTime);
                }
                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion Hash存储
    }
}