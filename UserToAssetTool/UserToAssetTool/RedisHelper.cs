using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace UserToAssetTool
{
    internal static class RedisHelper
    {
        private static readonly IDatabase redisDatabase = ConfigsManager.GetBizRedisClient();

        /// <summary>
        ///     资产用户比例关系
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public static async Task RemoveRedisAssetUserRatioAsync(List<UserAssetRatio> userAssetRatios)
        {
            var assetIds = userAssetRatios.Select(p => p.AssetId).ToList();
            foreach (var item in assetIds)
            {
                var currentUserAsset = userAssetRatios.Where(p => p.AssetId == item).ToList();
                if (currentUserAsset.Any())
                {
                    foreach (var subItem in currentUserAsset)
                    {
                        try
                        {
                            await redisDatabase.HashDeleteAsync($"AssetUserRelation:{item}", subItem.UserAssetRatioId);
                        }
                        // ReSharper disable once EmptyGeneralCatchClause
                        catch (Exception)
                        {

                        }

                    }
                }
            }
        }

        /// <summary>
        ///     用户资产比例,设置比例为无效
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public static async Task RemoveRedisUserAssetRatioAsync(List<UserAssetRatio> userAssetRatios)
        {
            foreach (var subItem in userAssetRatios)
            {
                try
                {
                    await redisDatabase.HashDeleteAsync($"UserAssetRelation:{subItem.UserId}", subItem.UserAssetRatioId);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        ///     资产用户比例关系
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public static async Task SetRedisAssetUserRatioAsync(List<UserAssetRatio> userAssetRatios)
        {
            var assetIds = userAssetRatios.Select(p => p.AssetId).ToList();
            foreach (var item in assetIds)
            {
                var currentUserAsset = userAssetRatios.Where(p => p.AssetId == item).ToList();
                if (currentUserAsset.Any())
                {
                    foreach (var subItem in currentUserAsset)
                    {
                        await redisDatabase.HashSetToCacheAsync("AssetUserRelation", item, subItem.UserAssetRatioId, subItem, TimeSpan.MaxValue);
                    }
                }
            }
        }

        /// <summary>
        ///     用户资产比例
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public static async Task SetRedisUserAssetRatioAsync(List<UserAssetRatio> userAssetRatios)
        {
            foreach (var subItem in userAssetRatios)
            {
                await redisDatabase.HashSetToCacheAsync("UserAssetRelation", subItem.UserId, subItem.UserAssetRatioId, subItem, TimeSpan.MaxValue);
            }
        }
        /// <summary>
        ///     记错误的资产信息
        /// </summary>
        /// <returns></returns>
        public static async Task SetRedisOnSellAssetByOldAsync(List<OnSellAssetDto> onSellAssetDtos)
        {
            foreach (var subItem in onSellAssetDtos)
            {
                await redisDatabase.HashSetToCacheAsync("ErrorOnSellAsset", "List", subItem.OnSellAssetId, subItem, TimeSpan.MaxValue);
            }
        }
        /// <summary>
        ///     记错误的用户资产信息
        /// </summary>
        /// <returns></returns>
        public static async Task SetRedisUserAssetRatioByOldAsync(List<UserAssetRatio> userAssetRatios)
        {
            foreach (var subItem in userAssetRatios)
            {
                await redisDatabase.HashSetToCacheAsync("ErrorUserAssetRelation", "List", subItem.UserAssetRatioId, subItem, TimeSpan.MaxValue);
            }
        }

        /// <summary>
        ///     记错误的资产用户信息
        /// </summary>
        /// <returns></returns>
        public static async Task SetRedisAssetUserRatioByOldAsync(List<UserAssetRatio> assetuserRatios)
        {
            foreach (var subItem in assetuserRatios)
            {
                await redisDatabase.HashSetToCacheAsync("ErrorAssetUserRelation", "List", subItem.UserAssetRatioId, subItem, TimeSpan.MaxValue);
            }
        }
        /// <summary>
        ///     记错误的订单信息
        /// </summary>
        /// <returns></returns>
        public static async Task SetRedisYemUserPurchaseByOldAsync(ResetYemUserPurchase yemUserProductDto)
        {
            await redisDatabase.HashSetToCacheAsync("ErrorYemUserPurchase", "List", yemUserProductDto.OrderId, yemUserProductDto, TimeSpan.MaxValue);
        }
    }
}