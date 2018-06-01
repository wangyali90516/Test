using System.ComponentModel;

namespace RedisToTableTool.Model
{
    public class YemUserProductInfo
    {
        public long Allocated { get; set; }

        /// <summary>
        ///     订单ID.
        /// </summary>
        [Description("订单ID")]
        public string OrderId { get; set; }

        /// <summary>
        ///     购买金额.
        /// </summary>
        [Description("购买金额")]
        public long PurchaseMoney { get; set; }

        public long RemainingAmount { get; set; }
        public int Status { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        [Description("用户编号")]
        public string UserId { get; set; }
    }
}