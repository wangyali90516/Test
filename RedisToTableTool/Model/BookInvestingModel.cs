using System;
using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    public class BookInvestingModel
    {
        /// <summary>
        ///     手机号码
        /// </summary>
        [JsonProperty("cellphone")]
        public string Cellphone { get; set; }

        /// <summary>
        ///     身份证号码
        /// </summary>
        [JsonProperty("credentialNo")]
        public string CredentialNo { get; set; }

        /// <summary>
        ///     证件类型
        ///     证件类型。10 =&gt; 身份证， 20 =&gt; 护照，30 =&gt; 台湾， 40 =&gt; 军官证
        /// </summary>

        [JsonProperty("credentialType")]
        public string CredentialType { get; set; }

        /// <summary>
        ///     主订单Id
        /// </summary>

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     购买金额
        /// </summary>
        [JsonProperty("purchaseAmount")]
        public long PurchaseAmount { get; set; }

        /// <summary>
        ///     申购日期
        /// </summary>
        [JsonProperty("purchaseStartDateTime")]
        public DateTime PurchaseStartDateTime { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        [JsonProperty("realName")]
        public string RealName { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}