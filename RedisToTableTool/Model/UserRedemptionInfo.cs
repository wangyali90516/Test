using System;
using System.ComponentModel;
using UserToAssetTool;

namespace JYM.Infrastructure.Models
{
    /// <summary>
    ///     余额猫赎回订单
    /// </summary>
    [Description("余额猫赎回订单")]
    public class UserRedemptionInfo : AssetBaseDto
    {
        /// <summary>
        ///     已经处理金额
        /// </summary>
        [Description("已经处理金额")]
        public long AlreadyDealtAmount { get; set; }

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
        ///     处理时间
        /// </summary>
        [Description("处理时间")]
        public DateTime HandlingTime { get; set; }

        /// <summary>
        ///     是否全部赎回 true:全部赎回 false:非全部赎回
        /// </summary>
        [Description("是否全部赎回 true:全部赎回 false:非全部赎回")]
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
        [Description("是否发送给交易系统")]
        public bool IsSendTrade { get; set; }

        /// <summary>
        ///     剩余可赎回金额
        /// </summary>
        [Description("剩余可赎回金额")]
        public long JBYLeftAmount { get; set; }

        /// <summary>
        ///     赎回订单号(赎回流水信息)
        /// </summary>
        [Description("赎回订单号(赎回流水信息)")]
        public string OrderId { get; set; }

        /// <summary>
        ///     订单赎回时间
        /// </summary>
        [Description("订单赎回时间")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     应该处理金额
        /// </summary>
        [Description("应该处理金额")]
        public long OughtAmount { get; set; }

        /// <summary>
        ///     资产配置系统的产品Id
        /// </summary>
        [Description("资产配置系统的产品Id")]
        public string ProductId { get; set; }

        /// <summary>
        ///     交易系统的产品Id
        /// </summary>
        [Description("交易系统的产品Id")]
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     赎回的金额
        /// </summary>
        [Description("赎回的金额")]
        public long RedemptionAmount { get; set; }

        /// <summary>
        ///     赎回订单所属分类
        /// </summary>
        /// <value>The redemption order category.</value>
        [Description("赎回订单所属分类")]
        public int RedemptionOrderCategory { get; set; }

        /// <summary>
        ///     待处理金额
        /// </summary>
        [Description("待处理金额")]
        public long RemainingAmount { get; set; }

        /// <summary>
        ///     赎回流水号(赎回流水信息)
        /// </summary>
        [Description("赎回流水号(赎回流水信息)")]
        public string SequenceNo { get; set; }

        /// <summary>
        ///     赎回手续费
        /// </summary>
        /// <value>The service charge.</value>
        [Description("赎回手续费")]
        public long ServiceCharge { get; set; }

        /// <summary>
        ///     赎回订单状态
        /// </summary>
        [Description("赎回订单状态")]
        public int Status { get; set; }

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
        ///     等待分配的预约冻结信息
        /// </summary>
        /// <value>The wait forallot list.</value>
        public string WaitForallotList { get; set; }

        /// <summary>
        ///     赎回中的订单信息
        /// </summary>
        /// <value>The wait forallot list.</value>
        public string WaitForRedeemList { get; set; }
    }
}

namespace UserToAssetTool
{
    /// <summary>
    ///     余额猫赎回订单
    /// </summary>
    [Description("余额猫赎回订单")]
    public class UserRedemptionInfo : AssetBaseDto
    {
        /// <summary>
        ///     已经处理金额
        /// </summary>
        [Description("已经处理金额")]
        public long AlreadyDealtAmount { get; set; }

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
        ///     处理时间
        /// </summary>
        [Description("处理时间")]
        public DateTime HandlingTime { get; set; }

        /// <summary>
        ///     是否全部赎回 true:全部赎回 false:非全部赎回
        /// </summary>
        [Description("是否全部赎回 true:全部赎回 false:非全部赎回")]
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
        [Description("是否发送给交易系统")]
        public bool IsSendTrade { get; set; }

        /// <summary>
        ///     剩余可赎回金额
        /// </summary>
        [Description("剩余可赎回金额")]
        public long JBYLeftAmount { get; set; }

        /// <summary>
        ///     赎回订单号(赎回流水信息)
        /// </summary>
        [Description("赎回订单号(赎回流水信息)")]
        public string OrderId { get; set; }

        /// <summary>
        ///     订单赎回时间
        /// </summary>
        [Description("订单赎回时间")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     应该处理金额
        /// </summary>
        [Description("应该处理金额")]
        public long OughtAmount { get; set; }

        /// <summary>
        ///     资产配置系统的产品Id
        /// </summary>
        [Description("资产配置系统的产品Id")]
        public string ProductId { get; set; }

        /// <summary>
        ///     交易系统的产品Id
        /// </summary>
        [Description("交易系统的产品Id")]
        public string ProductIdentifier { get; set; }

        /// <summary>
        ///     赎回的金额
        /// </summary>
        [Description("赎回的金额")]
        public long RedemptionAmount { get; set; }

        /// <summary>
        ///     赎回订单所属分类
        /// </summary>
        /// <value>The redemption order category.</value>
        [Description("赎回订单所属分类")]
        public int RedemptionOrderCategory { get; set; }

        /// <summary>
        ///     待处理金额
        /// </summary>
        [Description("待处理金额")]
        public long RemainingAmount { get; set; }

        /// <summary>
        ///     赎回流水号(赎回流水信息)
        /// </summary>
        [Description("赎回流水号(赎回流水信息)")]
        public string SequenceNo { get; set; }

        /// <summary>
        ///     赎回手续费
        /// </summary>
        /// <value>The service charge.</value>
        [Description("赎回手续费")]
        public long ServiceCharge { get; set; }

        /// <summary>
        ///     赎回订单状态
        /// </summary>
        [Description("赎回订单状态")]
        public int Status { get; set; }

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
        ///     等待分配的预约冻结信息
        /// </summary>
        /// <value>The wait forallot list.</value>
        public string WaitForallotList { get; set; }

        /// <summary>
        ///     赎回中的订单信息
        /// </summary>
        /// <value>The wait forallot list.</value>
        public string WaitForRedeemList { get; set; }
    }
}