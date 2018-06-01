using System.Collections.Generic;
using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    public class BookFrezzeModel
    {
        /// <summary>
        ///     用预约投资冻结账户金额 ,
        /// </summary>
        [JsonProperty("freezeAccountAmount")]
        public double? FreezeAccountAmount { get; set; }

        /// <summary>
        ///     预约冻结总金额
        /// </summary>
        [JsonProperty("freezeSumAmount")]
        public double? FreezeSumAmount { get; set; }

        /// <summary>
        ///     01(目前只支持预约投资)
        /// </summary>
        [JsonProperty("freezeType")]
        public string FreezeType { get; set; }

        /// <summary>
        ///     由网贷平台生成的唯一的交易流水号
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        ///     前台回调地址
        /// </summary>
        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     集合
        /// </summary>
        [JsonProperty("rpOrderList")]
        public List<RpOrder> RpOrderList { get; set; }

        /// <summary>
        ///     网贷平台唯一的用户编码
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}