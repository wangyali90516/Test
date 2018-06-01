using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindowsFormsMigrationData;
using J.Base.Lib;
using JinYinMao.Business.Assets.Request;
using RedisToTableTool.Model;
using StackExchange.Redis;
using UserToAssetTool;

namespace RedisToTableTool
{
    /// <summary>
    ///     redishelper
    /// </summary>
    public class RedisHelperSpecial
    {
        private readonly IDatabase database;

        public RedisHelperSpecial(int number)
        {
            this.database = ConfigsManager.GetBizRedisClientHelper(number);
        }

        /// <summary>
        ///     检查是否包含没有删除的资产用户比例
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public async Task<bool> CheckRedisAssetUserRatioAsync(IEnumerable<UserAssetRatio> userAssetRatios)
        {
            try
            {
                foreach (UserAssetRatio subItem in userAssetRatios)
                {
                    UserAssetRatio assetUserItem = await this.database.HashGetFromCacheAsync<UserAssetRatio>("AssetUserRelation", subItem.AssetId, subItem.UserAssetRatioId);
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
        public async Task<bool> CheckRedisUserAssetRatioAsync(IEnumerable<UserAssetRatio> userAssetRatios)
        {
            try
            {
                foreach (UserAssetRatio subItem in userAssetRatios)
                {
                    UserAssetRatio userAssetItem = await this.database.HashGetFromCacheAsync<UserAssetRatio>("UserAssetRelation", subItem.UserId, subItem.UserAssetRatioId);
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
        ///     查询所有的标的id下错误的数据
        /// </summary>
        /// <returns></returns>
        public List<string> GetCgbankYemSuccessListData(string bidId)
        {
            return this.database.HashGetAllFromCache<string>("CgBankOrderSuccessList", bidId).ToList();
        }

        /// <summary>
        ///     获取单个资产信息
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public async Task<OnSellAssetToRedis> GetRedisAssetIdInfoAsync(string assetId)
        {
            return await this.database.HashGetFromCacheAsync<OnSellAssetToRedis>("OnSellAssetToRedis", "List", assetId);
        }

        /// <summary>
        ///     获取所有的资产
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public List<OnSellAssetToRedis> GetRedisAssetInfos()
        {
            return this.database.HashGetAllFromCache<OnSellAssetToRedis>("OnSellAssetToRedis", "List").ToList();
        }

        /// <summary>
        ///     get redis message manager as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public List<UserAssetRatio> GetRedisAssetUserRatiosAsync(string redishashKey)
        {
            //根据Key获取指定的值
            return this.database.HashGetAllFromCache<UserAssetRatio>("AssetUserRelation", redishashKey).ToList();
        }

        /// <summary>
        ///     检查是否包含没有删除的资产用户比例
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public async Task<string> GetRedisContentAsync(string keyName)
        {
            return await this.database.StringGetAsync(keyName);
        }

        public async Task<List<int>> GetRedisNotifyBizStatusAsnc(string orderId)
        {
            string status = await this.database.StringGetAsync($"NotifyBizStatus:{orderId}");
            return status.FromJson<List<int>>();
        }

        /// <summary>
        ///     查询成功的userassetRatioIds
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<string> GetRedisSuccessInsertUserAssetRatioIdsAsync(string userid)
        {
            return this.database.HashGetAllFromCache<string>("SuccessInsertUserAssetRatioIds", userid).ToList();
        }

        //查出所有的用户关系比例
        public List<UserAssetRatio> GetRedisUserAssetRatioBeforeUpdateAsync(string userId)
        {
            return this.database.HashGetAllFromCache<UserAssetRatio>("UdpateUserAssetRetioInfos", userId).ToList();
        }

        /// <summary>
        ///     get redis message manager as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public List<UserAssetRatio> GetRedisUserAssetRatiosAsync(string redishashKey)
        {
            //根据Key获取指定的值
            return this.database.HashGetAllFromCache<UserAssetRatio>("UserAssetRelation", redishashKey).ToList();
        }

        /// <summary>
        ///     获取单个用户信息
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public async Task<UserInfoRedis> GetRedisUserInfoAsync(string userId)
        {
            return await this.database.HashGetFromCacheAsync<UserInfoRedis>("UserInfoToRedis", "List", userId);
        }

        /// <summary>
        ///     获取所有用户
        /// </summary>
        /// <returns>Task&lt;List&lt;MessageTemplate&gt;&gt;.</returns>
        public List<UserInfoRedis> GetRedisUserInfos()
        {
            return this.database.HashGetAllFromCache<UserInfoRedis>("UserInfoToRedis", "List").ToList();
        }

        /// <summary>
        ///     获取回滚数据    //  ErrorYemUserPurchase ErrorOnSellAsset ErrorAssetUserRelation
        /// </summary>
        /// <returns></returns>
        public List<OnSellAssetDto> GetRollbackAssetsData()
        {
            return this.database.HashGetAllFromCache<OnSellAssetDto>("ErrorOnSellAsset", "List").ToList();
        }

        /// <summary>
        ///     获取回滚数据    //  ErrorYemUserPurchase ErrorOnSellAsset ErrorAssetUserRelation
        /// </summary>
        /// <returns></returns>
        public List<UserAssetRatio> GetRollbackAssetUserData()
        {
            return this.database.HashGetAllFromCache<UserAssetRatio>("ErrorAssetUserRelation", "List").ToList();
        }

        /// <summary>
        ///     获取回滚数据    //  ErrorYemUserPurchase ErrorOnSellAsset ErrorAssetUserRelation
        /// </summary>
        /// <returns></returns>
        public List<UserAssetRatio> GetRollbackUserAssetData()
        {
            return this.database.HashGetAllFromCache<UserAssetRatio>("ErrorUserAssetRelation", "List").ToList();
        }

        /// <summary>
        ///     获取回滚数据    //  ErrorYemUserPurchase ErrorOnSellAsset ErrorAssetUserRelation
        /// </summary>
        /// <returns></returns>
        public List<RollBackDataModel> GetRollbackYemUserPurchaseData()
        {
            return this.database.HashGetAllFromCache<RollBackDataModel>("ErrorYemUserPurchase", "List").ToList();
        }

        /// <summary>
        ///     资产用户比例关系
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public async Task<bool> RemoveRedisAssetUserRatioAsync(List<UserAssetRatio> userAssetRatios)
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
                            await this.database.HashDeleteAsync($"AssetUserRelation:{item}", subItem.UserAssetRatioId);
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
        public async Task<bool> RemoveRedisUserAssetRatioAsync(IEnumerable<UserAssetRatio> userAssetRatios)
        {
            try
            {
                foreach (UserAssetRatio subItem in userAssetRatios)
                {
                    await this.database.HashDeleteAsync($"UserAssetRelation:{subItem.UserId}", subItem.UserAssetRatioId);
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

        /// <summary>
        ///     记录每个rowkey下面的所有batchInfo
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisAdvanceDebtInputDtosAsync(IEnumerable<AdvanceDebtInputDto> listadvAdvanceDebtInputDtos, string rowKey)
        {
            foreach (var subItem in listadvAdvanceDebtInputDtos)
            {
                await this.database.HashSetToCacheAsync("AdvanceDebtInputDtos", rowKey, subItem.DebtToTransferId, subItem, TimeSpan.MaxValue);
            }
        }

        /// <summary>
        ///     设置每个rowkey的AdvanceDebtRequest
        /// </summary>
        /// <param name="advanceDebtRequests"></param>
        /// <param name="rowKey"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        public async Task SetRedisAdvanceDebtRequestModelAsync(IEnumerable<AdvanceDebtRequest> advanceDebtRequests, string rowKey)
        {
            foreach (AdvanceDebtRequest advanceDebtRequest in advanceDebtRequests)
            {
                await this.database.HashSetToCacheAsync("AdvanceDebtRequest", rowKey, advanceDebtRequest.DebtToTransferId, advanceDebtRequest, TimeSpan.MaxValue);
            }
        }

        /// <summary>
        ///     记错误的资产用户信息
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisAssetUserRatioByOldAsync(List<UserAssetRatio> assetuserRatios)
        {
            foreach (var subItem in assetuserRatios)
            {
                await this.database.HashSetToCacheAsync("ErrorAssetUserRelation", "List", subItem.UserAssetRatioId, subItem, TimeSpan.MaxValue);
            }
        }

        /// <summary>
        ///     记录每个rowkey下面的所有batchInfo
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisBatchCreditInfoAsync(BatchBookCreditInputDto requBatchBookCreditInputDto, string rowKey)
        {
            await this.database.SetDataToCacheAsync("BatchBookCreditInputInfo", rowKey, requBatchBookCreditInputDto, TimeSpan.MaxValue);
        }

        /// <summary>
        ///     记录未插入的orderid
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisCgBankOrderListSuccessAsync(string bidId, string orderId)
        {
            await this.database.HashSetToCacheAsync("CgBankOrderSuccessList", bidId, orderId, orderId, TimeSpan.MaxValue);
        }

        /// <summary>
        ///     设置每个rowkey的Notifybatchrequest
        /// </summary>
        /// <param name="batchCreditRequest"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public async Task SetRedisNotifyBatchCreditRequestAsync(NotifyBatchCreditRequest batchCreditRequest, string rowKey)
        {
            await this.database.HashSetToCacheAsync("NotifyBatchCreditRequest", "List", rowKey, batchCreditRequest, TimeSpan.MaxValue);
        }

        /// <summary>
        ///     通知交易
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisNotifyBizStatusAsync(List<int> status, string orderId)
        {
            await this.database.SetDataToCacheAsync("NotifyBizStatus", orderId, status.ToJson(), TimeSpan.MaxValue);
        }

        /// <summary>
        ///     通知交易购买
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisNotifyPurchaseAsync(TifisfalPurchaseRequestModel model, string deptId)
        {
            await this.database.HashSetToCacheAsync("TifisfalPurchaseRequestModels", "List", deptId, model, TimeSpan.MaxValue);
            //await this.database.SetDataToCacheAsync("NotifyPurchaseRequest", orderId, status.ToJson(), TimeSpan.MaxValue);
        }

        /// <summary>
        ///     记错误的资产信息
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisOnSellAssetByOldAsync(List<OnSellAssetDto> onSellAssetDtos)
        {
            foreach (var subItem in onSellAssetDtos)
            {
                await this.database.HashSetToCacheAsync("ErrorOnSellAsset", "List", subItem.OnSellAssetId, subItem, TimeSpan.MaxValue);
            }
        }

        /// <summary>
        ///     设置每个rowkey的SendDebtForBankModel
        /// </summary>
        /// <param name="sendDebtForBankModel"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public async Task SetRedisSendDebtForBankModelAsync(SendDebtForBankModel sendDebtForBankModel, string rowKey)
        {
            await this.database.HashSetToCacheAsync("SendDebtForBankModel", "List", rowKey, sendDebtForBankModel, TimeSpan.MaxValue);
        }

        /// <summary>
        ///     记录每个rowkey下面的所有suborderIds
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisSubOrderListInfosAsync(IEnumerable<SubInvestOrderInputDto> suborderIds, string rowKey)
        {
            try
            {
                foreach (SubInvestOrderInputDto subItem in suborderIds)
                {
                    await this.database.HashSetToCacheAsync("SubOrderIdInfos", rowKey, subItem.SubOrderId, subItem, TimeSpan.MaxValue);
                }
            }
            catch (Exception e)
            {
                //
            }
        }

        /// <summary>
        ///     记录成功的userassetRatioIds
        /// </summary>
        /// <param name="userAssetRatioId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task SetRedisSuccessInsertUserAssetRatioIdsAsync(string userAssetRatioId, string userid)
        {
            await this.database.HashSetToCacheAsync("SuccessInsertUserAssetRatioIds", userid, Guid.NewGuid().ToGuidString(), userAssetRatioId, TimeSpan.MaxValue);
        }

        /// <summary>
        ///     通知赎回购买
        /// </summary>
        /// <returns></returns>
        public async Task SetRedistirisfalUserRedemptionInfoModelAsync(TirisfalUserRedemptionInfoModel model, string deptId)
        {
            await this.database.HashSetToCacheAsync("TirisfalUserRedemptionInfoModels", "List", deptId, model, TimeSpan.MaxValue);
        }

        //记录所有的
        /// <summary>
        ///     用户资产比例
        /// </summary>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        public async Task SetRedisUserAssetRatioAsync(List<UserAssetRatio> userAssetRatios)
        {
            foreach (var subItem in userAssetRatios)
            {
                await this.database.HashSetToCacheAsync("UserAssetRelation", subItem.UserId, subItem.UserAssetRatioId, subItem, TimeSpan.MaxValue);
            }
        }

        /// <summary>
        ///     修改前的用户比例关系
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisUserAssetRatioBeforeUpdateAsync(string userId, IEnumerable<UserAssetRatio> userAssetRatios)
        {
            foreach (UserAssetRatio item in userAssetRatios)
            {
                await this.database.HashSetToCacheAsync("UdpateUserAssetRetioInfos", userId, item.UserAssetRatioId, item, TimeSpan.MaxValue);
            }
        }

        /// <summary>
        ///     记错误的用户资产信息
        /// </summary>
        /// <returns></returns>
        public async Task SetRedisUserAssetRatioByOldAsync(List<UserAssetRatio> userAssetRatios)
        {
            foreach (var subItem in userAssetRatios)
            {
                await this.database.HashSetToCacheAsync("ErrorUserAssetRelation", "List", subItem.UserAssetRatioId, subItem, TimeSpan.MaxValue);
            }
        }

        //{
        //public static async Task SetRedisUserAssetRatioAsync(string userId, IEnumerable<UserAssetRatio> userAssetRatios)

        ///// <param name="userAssetRatios"></param>

        ///// <param name="userId"></param>
        ///// </summary>
        /////     用户资产比例

        ///// <summary>

        ///// <summary>
        /////     记错误的订单信息
        ///// </summary>
        ///// <returns></returns>
        //public  async Task SetRedisYemUserPurchaseByOldAsync(ResetYemUserPurchase yemUserProductDto)
        //{
        //    await this.database.HashSetToCacheAsync("ErrorYemUserPurchase", "List", yemUserProductDto.OrderId, yemUserProductDto, TimeSpan.MaxValue);
        //}
        //                await redisDatabase.HashSetToCacheAsync("AssetUserRelation", item, subItem.UserAssetRatioId, subItem.ToJson(), TimeSpan.MaxValue);

        //            {
        //            foreach (var subItem in currentUserAsset)
        //        {
        //    {
        //    foreach (var item in assetIds)

        //    var assetIds = userAssetRatios.Select(p => p.AssetId).ToList();
        //    IDatabase redisDatabase = ConfigsManager.GetBizRedisClient();
        //{
        ///// </summary>
        /////     资产用户比例关系

        ///// <summary>
        ///// <param name="userAssetRatios"></param>

        ///// <returns></returns>

        //public static async Task SetRedisAssetUserRatioAsync(List<UserAssetRatio> userAssetRatios)

        //        var currentUserAsset = userAssetRatios.Where(p => p.AssetId == item).ToList();

        //        if (currentUserAsset.Any())
        //            }

        //        }
        //    }

        //}

        ///// <returns></returns>
        //    IDatabase redisDatabase = ConfigsManager.GetBizRedisClient();
        //    foreach (var subItem in userAssetRatios)
        //    {
        //        await redisDatabase.HashSetToCacheAsync("UserAssetRelation", userId, subItem.UserAssetRatioId, subItem.ToJson(), TimeSpan.MaxValue);
        //    }
        //}
    }
}