using UserToAssetTool;

namespace RedisToTableTool.Model
{
    public class RollBackDataModel
    {
        /// <summary>
        ///     本次处理金额
        /// </summary>
        public long CurrentDealPurchaseAmount { get; set; }

        /// <summary>
        ///     订单号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        ///     订单
        /// </summary>
        public YemUserProductDto YemUserProductDto { get; set; }
    }
}