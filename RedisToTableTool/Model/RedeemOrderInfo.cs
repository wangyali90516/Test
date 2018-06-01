using System;
using System.ComponentModel;
using UserToAssetTool;

namespace RedisToTableTool.Model
{
    public class RedeemOrderInfo : AssetBaseDto
    {
        /// <summary>
        ///     已经处理金额
        /// </summary>
        /// <value>The already dealt amount.</value>
        [Description("已经处理金额")]
        public long AlreadyDealtAmount { get; set; }

        /// <summary>
        ///     Gets or sets the cell phone.
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
        ///     处理时间
        /// </summary>
        /// <value>The handling time.</value>
        [Description("处理时间")]
        public DateTime HandlingTime { get; set; }

        /// <summary>
        ///     是否全部赎回 true:全部赎回 false:非全部赎回
        /// </summary>
        /// <value>The is full redeem.</value>
        public bool IsFullRedeem { get; set; }

        /// <summary>
        ///     是否被锁定
        /// </summary>
        /// <value><c>true</c> if this instance is lock; otherwise, <c>false</c>.</value>
        [Description("是否被锁定")]
        public bool IsLock { get; set; }

        /// <summary>
        ///     是否发送给交易系统
        /// </summary>
        /// <value><c>true</c> if this instance is send trade; otherwise, <c>false</c>.</value>
        [Description("是否发送给交易系统")]
        public bool IsSendTrade { get; set; }

        /// <summary>
        ///     剩余可赎回金额
        /// </summary>
        /// <value>The jby left amount.</value>
        public long JbyLeftAmount { get; set; }

        /// <summary>
        ///     赎回订单号(赎回流水信息)
        /// </summary>
        /// <value>The order identifier.</value>
        [Description("赎回订单号(赎回流水信息)")]
        public string OrderId { get; set; }

        /// <summary>
        ///     订单赎回时间
        /// </summary>
        /// <value>The order time.</value>
        [Description("订单赎回时间")]
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
        ///     待处理金额
        /// </summary>
        /// <value>The already dealt amount.</value>
        public long RemainingAmount { get; set; }

        /// <summary>
        ///     赎回订单号(赎回流水信息)
        /// </summary>
        /// <value>The order identifier.</value>
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
        ///     用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        [Description("用户名称")]
        public string UserName { get; set; }

        /// <summary>
        ///     等待分配的预约冻结信息
        /// </summary>
        /// <value>The wait for allot list.</value>
        public string WaitForallotList { get; set; }

        /// <summary>
        ///     赎回中的订单信息
        /// </summary>
        /// <value>The wait forallot list.</value>
        public string WaitForRedeemList { get; set; }
    }
}