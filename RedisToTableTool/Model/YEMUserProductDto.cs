using System;
using System.ComponentModel;

namespace UserToAssetTool
{
    /// <summary>
    ///     预申购订单
    /// </summary>
    public class YemUserProductDto : AssetBaseDto
    {
        /// <summary>
        ///     已经分配的金额
        /// </summary>
        [Description("已经分配的金额")]
        public long Allocated { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        /// <value>The cell phone.</value>
        [Description("手机号")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     身份证号
        /// </summary>
        /// <value>The credential no.</value>
        [Description("身份证号")]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     投资成功的金额
        /// </summary>
        [Description("投资成功的金额")]
        public long InvestSuccessAmount { get; set; }

        /// <summary>
        ///     是否发送给交易系统
        /// </summary>
        [Description("是否发送给交易系统")]
        public bool IsSendTrade { get; set; }

        /// <summary>
        ///     预约批量投资详情
        /// </summary>
        [Description("预约批量投资详情")]
        public string OrderBatchBookInvestInfos { get; set; }

        /// <summary>
        ///     订单ID.
        /// </summary>
        [Description("订单ID")]
        public string OrderId { get; set; }

        /// <summary>
        ///     订单投资状态：-1-未投资成功，0-部分投资成功，1-全部投资成功
        /// </summary>
        [Description("订单投资状态：-1-未投资成功，0-部分投资成功，1-全部投资成功")]
        public int OrderInvestStatus { get; set; }

        /// <summary>
        ///     下单时间.
        /// </summary>
        [Description("下单时间")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     用户产品Id
        /// </summary>
        [Description("用户产品Id")]
        public string ProductId { get; set; }

        /// <summary>
        ///     交易系统产品ID.
        /// </summary>
        [Description("交易系统产品ID")]
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     购买金额.
        /// </summary>
        [Description("购买金额")]
        public long PurchaseMoney { get; set; }

        /// <summary>
        ///     剩余金额
        /// </summary>
        [Description("剩余金额")]
        public long RemainingAmount { get; set; }

        /// <summary>
        ///     流水号
        /// </summary>
        [Description("流水号")]
        public string SequenceNo { get; set; }

        /// <summary>
        ///     订单状态
        /// </summary>
        [Description("订单状态")]
        public int Status { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        [Description("用户编号")]
        public string UserId { get; set; }

        /// <summary>
        ///     用户名称.
        /// </summary>
        [Description("用户名称")]
        public string UserName { get; set; }

        /// <summary>
        ///     分配在售资产池等待银行返回的金额.
        /// </summary>
        [Description("分配在售资产池等待银行返回的金额")]
        public long WaitingBankBackAmount { get; set; }
    }
}