using System;

namespace RedisToTableTool.Model
{
    /// <summary>
    ///     资产相关信息
    /// </summary>
    public class OnSellAssetToRedis
    {
        public int AssetCategoryCode { get; set; }
        public DateTime BillDueDate { get; set; }
        public long Denominator { get; set; }
        public string OnSellAssetId { get; set; }
    }

    /// <summary>
    ///     用户信息
    /// </summary>
    public class UserInfoRedis
    {
        public string Cellphone { get; set; }
        public string CredentialNo { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public long PurchaseMoney { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}