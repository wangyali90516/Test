using System.ComponentModel;
using UserToAssetTool;

namespace RedisToTableTool.Model
{
    public class DeptModel : AssetBaseDto
    {
        /// <summary>
        ///     本金金额
        /// </summary>
        [Description("本金金额")]
        public long Amount { get; set; }

        /// <summary>
        ///     资产Id
        /// </summary>
        [Description("资产Id")]
        public string AssetId { get; set; }

        /// <summary>
        ///     购买订单Id
        /// </summary>
        [Description("购买订单Id")]
        public string BuyOrderId { get; set; }

        /// <summary>
        ///     债转关系主键
        /// </summary>
        [Description("债转关系主键")]
        public string DebtToTransferId { get; set; }

        /// <summary>
        ///     利息
        /// </summary>
        /// <value>The interest.</value>
        [Description("利息")]
        public long Interest { get; set; }

        /// <summary>
        ///     新用户-资产标识
        /// </summary>
        [Description("新用户-资产标识")]
        public string NewUserAssetRatioId { get; set; }

        /// <summary>
        ///     购买用户
        /// </summary>
        [Description("购买用户")]
        public string NewUserId { get; set; }

        /// <summary>
        ///     老用户-资产标识
        /// </summary>
        [Description("老用户-资产标识")]
        public string OldUserAssetRatioId { get; set; }

        /// <summary>
        ///     老买用户
        /// </summary>
        [Description("老买用户")]
        public string OldUserId { get; set; }

        /// <summary>
        ///     赎回订单Id
        /// </summary>
        [Description("赎回订单Id")]
        public string RansomOrderId { get; set; }

        /// <summary>
        ///     (0--表示还未被使用等待银行返回结果，1--表示银行已经返回成功，2--银行返回失败)
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }
    }
}