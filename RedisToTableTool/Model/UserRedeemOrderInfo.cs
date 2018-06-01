using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RedisToTableTool.Model
{
    /// <summary>
    ///     赎回订单请求参数
    /// </summary>
    public class UserRedemptionOrderInputDto
    {
        /// <summary>
        ///     已经处理金额
        /// </summary>
        /// <value>The already dealt amount.</value>
        public long AlreadyDealtAmount { get; set; }

        /// <summary>
        ///     手机号
        /// </summary>
        /// <value>The cellphone.</value>
        public string Cellphone { get; set; }

        /// <summary>
        ///     身份证号
        /// </summary>
        /// <value>The credential no.</value>
        public string CredentialNo { get; set; }

        /// <summary>
        ///     处理时间
        /// </summary>
        /// <value>The handling time.</value>
        public DateTime HandlingTime { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     是否全部赎回 true:全部赎回 false:非全部赎回
        /// </summary>
        /// <value>The is full redeem.</value>
        public bool IsFullRedeem { get; set; }

        /// <summary>
        ///     是否通知交易系统
        /// </summary>
        /// <value><c>true</c> if this instance is send trade; otherwise, <c>false</c>.</value>
        public bool IsSendTrade { get; set; }

        /// <summary>
        ///     剩余可赎回金额（账户剩余在投金额）
        /// </summary>
        /// <value>The jby left amount.</value>
        public long JbyLeftAmount { get; set; }

        /// <summary>
        ///     订单赎回时间
        /// </summary>
        /// <value>The order time.</value>
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     应该处理金额
        /// </summary>
        /// <value>The ought amount.</value>
        public long OughtAmount { get; set; }

        /// <summary>
        ///     资产配置系统的产品Id
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductId { get; set; }

        /// <summary>
        ///     交易系统的产品Id
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     赎回的金额
        /// </summary>
        public long RedemptionAmount { get; set; }

        /// <summary>
        ///     赎回订单所属分类
        /// </summary>
        /// <value>The redemption order category.</value>
        public int RedemptionOrderCategory { get; set; }

        /// <summary>
        ///     剩余金额
        /// </summary>
        /// <value>The remaining amount.</value>
        public long RemainingAmount { get; set; }

        /// <summary>
        ///     赎回流水号
        /// </summary>
        /// <value>The redeem sequence no.</value>
        public string SequenceNo { get; set; }

        /// <summary>
        ///     赎回手续费
        /// </summary>
        /// <value>The service charge.</value>
        [Description("赎回手续费")]
        public long ServiceCharge { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }

        /// <summary>
        ///     为了转换成请求参数用
        /// </summary>
        /// <value>The order identifier.</value>
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }

        /// <summary>
        ///     用户名称
        /// </summary>
        /// <value>The name of the user.</value>
        [Description("用户名称")]
        public string UserName { get; set; }

        /// <summary>
        ///     等待分配的预约冻结信息
        /// </summary>
        /// <value>The wait forallot list.</value>
        public List<WaitForAllotBuyOrderListInputDto> WaitForallotList { get; set; }

        /// <summary>
        ///     赎回中的订单信息
        /// </summary>
        /// <value>The wait forallot list.</value>
        public List<WaitForAllotBuyOrderListInputDto> WaitForRedeemList { get; set; }
    }

    /// <summary>
    ///     赎回时传输等待处理的用户购买订单信息
    /// </summary>
    public class WaitForAllotBuyOrderListInputDto
    {
        /// <summary>
        ///     1:用户下单,2:系统下单,-1:预留值
        /// </summary>
        /// <value>The type of the booking.</value>
        public int BookingType { get; set; }

        /// <summary>
        ///     购买订单号
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }

        /// <summary>
        ///     订单待处理的金额
        /// </summary>
        /// <value>The wait amount.</value>
        public long WaitAmount { get; set; }
    }
}