using System;

namespace RedisToTableTool.Model
{
    public class RefundMoneyRequest
    {
        /// <summary>
        ///     用户回款Id
        /// </summary>
        /// <value>The returned money financiers detail identifier.</value>

        public string AssetId { get; set; }

        /// <summary>
        ///     平台运营可用金额
        /// </summary>
        /// <value>The platform avail amount.</value>
        public long PlatformAvailAmount { get; set; }

        /// <summary>
        ///     平台佣金
        /// </summary>
        /// <value>The platform brokerage.</value>
        public long PlatformBrokerage { get; set; }

        /// <summary>
        ///     资产Id
        /// </summary>
        /// <value>The asset identifier.</value>
        /// <summary>
        ///     平台Id
        /// </summary>
        /// <value>The platform identifier.</value>
        public string PlatformId { get; set; }

        /// <summary>
        ///     平台名称
        /// </summary>
        /// <value>The name of the platform.</value>
        public string PlatformName { get; set; }

        /// <summary>
        ///     本金利息和
        /// </summary>
        /// <value>The repayment avail amount.</value>
        public long PrincipalAndInterest { get; set; }

        /// <summary>
        ///     其他金额
        /// </summary>
        /// <value>The repayment avail amount.</value>
        public long QitaAmount { get; set; }

        /// <summary>
        ///     实际回款总额
        /// </summary>
        /// <value>The repayment avail amount.</value>
        public long RealReturnedAmount { get; set; }

        /// <summary>
        ///     还款企业可用金额
        /// </summary>
        /// <value>The repayment avail amount.</value>
        public long RepaymentAvailAmount { get; set; }

        /// <summary>
        ///     还款企业Id
        /// </summary>
        /// <value>The repayment identifier.</value>
        public string RepaymentId { get; set; }

        /// <summary>
        ///     还款企业名称
        /// </summary>
        /// <value>The name of the repayment.</value>
        public string RepaymentName { get; set; }

        /// <summary>
        ///     更新人
        /// </summary>
        /// <value>The updated by.</value>
        public string UpdatedBy { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        /// <value>The updated time.</value>
        public DateTime UpdatedTime { get; set; }

        public string UserReturnedMoneyId { get; set; }
    }
}