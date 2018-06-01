using System;
using System.ComponentModel;

namespace RedisToTableTool.Model
{
    public class YemUserOrderInfo
    {
        //UserId,UserName,CellPhone,PurchaseMoney,OrderId,OrderTime,SequenceNo

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
        ///     订单ID.
        /// </summary>
        [Description("订单ID")]
        public string OrderId { get; set; }

        /// <summary>
        ///     下单时间.
        /// </summary>
        [Description("下单时间")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        ///     购买金额.
        /// </summary>
        [Description("购买金额")]
        public long PurchaseMoney { get; set; }

        /// <summary>
        ///     流水号
        /// </summary>
        [Description("流水号")]
        public string SequenceNo { get; set; }

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
    }
}