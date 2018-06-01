using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserToAssetTool
{
    public class UserAssetRatio : AssetBaseDto
    { /// <summary>
      ///     资产类型
      /// </summary>
        [Description("资产类型")]
        public int AssetCategoryCode { get; set; }

        /// <summary>
        ///     资产Id
        /// </summary>
        [Description("资产Id")]
        public string AssetId { get; set; }

        /// <summary>
        ///     到期日
        /// </summary>
        [Description("到期日")]
        public DateTime BillDueDate { get; set; }

        /// <summary>
        ///     本金
        /// </summary>
        [Description("本金")]
        public long Capital { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        [Description("手机号")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     身份证号
        /// </summary>
        [Description("身份证号")]
        public string CredentialNo { get; set; }

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
        ///     通知交易系统响应信息
        /// </summary>
        [Description("通知交易系统响应信息")]
        public string NotifyTradingRespInfo { get; set; }

        /// <summary>
        ///     通知交易系统时间
        /// </summary>
        [Description("通知交易系统时间")]
        public DateTime? NotifyTradingTime { get; set; }

        /// <summary>
        ///     分子
        /// </summary>
        [Description("分子")]
        public long Numerator { get; set; }

        /// <summary>
        ///     订单号
        /// </summary>
        [Description("订单号")]
        public string OrderId { get; set; }

        /// <summary>
        ///     下单时间
        /// </summary>
        [Description("下单时间")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     原始单号
        /// </summary>
        /// <value>The original user asset ratio identifier.</value>
        [Description("原始单号")]
        public string OriginalUserAssetRatioId { get; set; }

        /// <summary>
        ///     订单金额
        /// </summary>
        [Description("订单金额")]
        public long PurchaseMoney { get; set; }

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
        ///     业务主键
        /// </summary>
        [Description("业务主键")]
        public string UserAssetRatioId { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        [Description("用户Id")]
        public string UserId { get; set; }

        /// <summary>
        ///     用户名称
        /// </summary>
        [Description("用户名称")]
        public string UserName { get; set; }

        /// <summary>
        ///     当前价值
        /// </summary>
        [Description("当前价值")]
        public long UserPresentValue { get; set; }
    }
}