using System.ComponentModel;

namespace JYM.Web.Requests
{
    /// <summary>
    ///     修改比例
    /// </summary>
    public class ModifyUserAssetRatioRequest
    {
        /// <summary>
        ///     资产Id
        /// </summary>
        [Description("资产Id")]
        public string AssetId { get; set; }

        /// <summary>
        ///     本金
        /// </summary>
        [Description("本金")]
        public long Capital { get; set; }

        /// <summary>
        ///     分母
        /// </summary>
        [Description("分母")]
        public long Denominator { get; set; }

        /// <summary>
        ///     是否投资成功: true-投资成功, false-未投资成功
        /// </summary>
        [Description("是否投资成功: true-投资成功, false-未投资成功")]
        public bool IsInvestSuccess { get; set; }

        /// <summary>
        ///     是否通知交易系统成功: true-通知成功, false-通知失败
        /// </summary>
        [Description("是否通知交易系统成功: true-通知成功, false-通知失败")]
        public bool IsNotifyTradingSuccess { get; set; }

        /// <summary>
        ///     是否已回款
        /// </summary>
        [Description("是否已回款")]
        public bool IsReturned { get; set; }

        /// <summary>
        ///     分子
        /// </summary>
        [Description("分子")]
        public long Numerator { get; set; }

        /// <summary>
        ///     分配完成标识(0--未打款；1--已经打款)
        /// </summary>
        [Description("分配完成标识(0--未打款；1--已经打款)")]
        public string Reserve { get; set; }

        /// <summary>
        ///     用户-资产比例关系的状态
        /// </summary>
        [Description("用户-资产比例关系的状态")]
        public int Status { get; set; }

        /// <summary>
        ///     用户资产比例ID
        /// </summary>
        [Description("用户资产比例ID")]
        public string UserAssetRatioId { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        [Description("用户Id")]
        public string UserId { get; set; }
        /// <summary>
        ///     是否无效
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}