using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsFormsMigrationData;
using RedisToTableTool.Model;
using StackExchange.Redis;

namespace UserToAssetTool
{
    public static class RedisHelper
    {
        private static readonly IDatabase database = ConfigsManager.GetBizRedisClient();

        /// <summary>
        ///     检查是否包含没有删除的资产用户比例
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public static async Task<bool> CheckRedisAssetUserRatioAsync(IEnumerable<UserAssetRatio> userAssetRatios)
        {
            try
            {
                foreach (UserAssetRatio subItem in userAssetRatios)
                {
                    UserAssetRatio assetUserItem = await database.HashGetFromCacheAsync<UserAssetRatio>("AssetUserRelation", subItem.AssetId, subItem.UserAssetRatioId);
                    if (assetUserItem != null)
                    {
                        return false;
                    }
                }
                return true;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex)
            {
                Logger.Logw(ex.Message + "------CheckRedisAssetUserRatioAsync", 3);
                return false;
            }
        }

        /// <summary>
        ///     检查是否包含没有删除的用户资产比例
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public static async Task<bool> CheckRedisUserAssetRatioAsync(IEnumerable<UserAssetRatio> userAssetRatios)
        {
            try
            {
                foreach (UserAssetRatio subItem in userAssetRatios)
                {
                    UserAssetRatio userAssetItem = await database.HashGetFromCacheAsync<UserAssetRatio>("UserAssetRelation", subItem.UserId, subItem.UserAssetRatioId);
                    if (userAssetItem != null)
                    {
                        return false;
                    }
                }
                return true;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex)
            {
                Logger.Logw(ex.Message + "------CheckRedisUserAssetRatioAsync", 3);
                return false;
            }
        }

        /// <summary>
        ///     获取单个资产信息
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public static async Task<OnSellAssetToRedis> GetRedisAssetIdInfoAsync(string assetId)
        {
            return await database.HashGetFromCacheAsync<OnSellAssetToRedis>("OnSellAssetToRedis", "List", assetId);
        }

        /// <summary>
        ///     获取所有的资产
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public static List<OnSellAssetToRedis> GetRedisAssetInfos()
        {
            return database.HashGetAllFromCache<OnSellAssetToRedis>("OnSellAssetToRedis", "List").ToList();
        }

        /// <summary>
        ///     get redis message manager as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public static List<UserAssetRatio> GetRedisAssetUserRatiosAsync(string redishashKey)
        {
            //根据Key获取指定的值
            return database.HashGetAllFromCache<UserAssetRatio>("AssetUserRelation", redishashKey).ToList();
        }

        /// <summary>
        ///     get redis message manager as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public static List<UserAssetRatio> GetRedisUserAssetRatiosAsync(string redishashKey)
        {
            //根据Key获取指定的值
            return database.HashGetAllFromCache<UserAssetRatio>("UserAssetRelation", redishashKey).ToList();
        }

        /// <summary>
        ///     获取单个用户信息
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public static async Task<UserInfoRedis> GetRedisUserInfoAsync(string userId)
        {
            return await database.HashGetFromCacheAsync<UserInfoRedis>("UserInfoToRedis", "List", userId);
        }

        /// <summary>
        ///     获取所有用户
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public static List<UserInfoRedis> GetRedisUserInfos()
        {
            return database.HashGetAllFromCache<UserInfoRedis>("UserInfoToRedis", "List").ToList();
        }

        /// <summary>
        ///     获取回滚数据    //  ErrorYemUserPurchase ErrorOnSellAsset ErrorAssetUserRelation
        /// </summary>
        /// <returns></returns>
        public static List<OnSellAssetDto> GetRollbackAssetsData()
        {
            return database.HashGetAllFromCache<OnSellAssetDto>("ErrorOnSellAsset", "List").ToList();
        }

        /// <summary>
        ///     获取回滚数据    //  ErrorYemUserPurchase ErrorOnSellAsset ErrorAssetUserRelation
        /// </summary>
        /// <returns></returns>
        public static List<UserAssetRatio> GetRollbackAssetUserData()
        {
            return database.HashGetAllFromCache<UserAssetRatio>("ErrorAssetUserRelation", "List").ToList();
        }

        /// <summary>
        ///     获取回滚数据    //  ErrorYemUserPurchase ErrorOnSellAsset ErrorAssetUserRelation
        /// </summary>
        /// <returns></returns>
        public static List<UserAssetRatio> GetRollbackUserAssetData()
        {
            return database.HashGetAllFromCache<UserAssetRatio>("ErrorUserAssetRelation", "List").ToList();
        }

        /// <summary>
        ///     获取回滚数据    //  ErrorYemUserPurchase ErrorOnSellAsset ErrorAssetUserRelation
        /// </summary>
        /// <returns></returns>
        public static List<RollBackDataModel> GetRollbackYemUserPurchaseData()
        {
            return database.HashGetAllFromCache<RollBackDataModel>("ErrorYemUserPurchase", "List").ToList();
        }

        /// <summary>
        ///     资产用户比例关系
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public static async Task<bool> RemoveRedisAssetUserRatioAsync(List<UserAssetRatio> userAssetRatios)
        {
            try
            {
                List<string> assetIds = userAssetRatios.Select(p => p.AssetId).ToList();
                foreach (var item in assetIds)
                {
                    List<UserAssetRatio> currentUserAsset = userAssetRatios.Where(p => p.AssetId == item).ToList();
                    if (currentUserAsset.Any())
                    {
                        foreach (UserAssetRatio subItem in currentUserAsset)
                        {
                            await database.HashDeleteAsync($"AssetUserRelation:{item}", subItem.UserAssetRatioId);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Logw(ex.Message + "------RemoveRedisAssetUserRatioAsync", 3);
                return false;
            }
        }

        /// <summary>
        ///     用户资产比例,设置比例为无效
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public static async Task<bool> RemoveRedisUserAssetRatioAsync(IEnumerable<UserAssetRatio> userAssetRatios)
        {
            try
            {
                foreach (UserAssetRatio subItem in userAssetRatios)
                {
                    await database.HashDeleteAsync($"UserAssetRelation:{subItem.UserId}", subItem.UserAssetRatioId);
                }
                return true;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex)
            {
                Logger.Logw(ex.Message + "------RemoveRedisUserAssetRatioAsync", 3);
                return false;
            }
        }

        //        if (currentUserAsset.Any())

        //        var currentUserAsset = userAssetRatios.Where(p => p.AssetId == item).ToList();

        //public static async Task SetRedisAssetUserRatioAsync(List<UserAssetRatio> userAssetRatios)
        ///// <returns></returns>
        ///// <param name="userAssetRatios"></param>

        ///// <summary>
        /////     资产用户比例关系
        ///// </summary>
        //{
        //    IDatabase redisDatabase = ConfigsManager.GetBizRedisClient();

        //    var assetIds = userAssetRatios.Select(p => p.AssetId).ToList();
        //    foreach (var item in assetIds)
        //    {
        //        {
        //            foreach (var subItem in currentUserAsset)

        //            {
        //                await redisDatabase.HashSetToCacheAsync("AssetUserRelation", item, subItem.UserAssetRatioId, subItem.ToJson(), TimeSpan.MaxValue);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        /////     用户资产比例
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="userAssetRatios"></param>
        ///// <returns></returns>
        //public static async Task SetRedisUserAssetRatioAsync(string userId, IEnumerable<UserAssetRatio> userAssetRatios)
        //{
        //    IDatabase redisDatabase = ConfigsManager.GetBizRedisClient();
        //    foreach (var subItem in userAssetRatios)
        //    {
        //        await redisDatabase.HashSetToCacheAsync("UserAssetRelation", userId, subItem.UserAssetRatioId, subItem.ToJson(), TimeSpan.MaxValue);
        //    }
        //}
    }
}