using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    /// <summary>
    ///     订单查询响应参数
    /// </summary>
    /// <seealso cref="Jinyinmao.BankGateway.WebApi.Models.Response.FrontResponseBase" />
    public class OrderSearchResponse
    {
        /// <summary>
        ///     金额(单位分)
        /// </summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        ///     业务类型(1000-充值,2000-提现,3003-取消投资,3010-取消预约冻结,3011-预约冻结,3100-投资,5000-放款,7000-还款,8000-返利)
        /// </summary>
        [JsonProperty("bizType")]
        public string BizType { get; set; }

        /// <summary>
        ///     币种
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        ///     商户编号
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        ///     交易流水号
        /// </summary>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     出款方
        /// </summary>
        [JsonProperty("payUserId")]
        public string PayUserId { get; set; }

        /// <summary>
        ///     收款方
        /// </summary>
        [JsonProperty("receiveUserId")]
        public string ReceiveUserId { get; set; }

        /// <summary>
        ///     状态码(1-成功,0-处理中,-1-失败)
        /// </summary>
        [JsonProperty("respCode")]
        public int RespCode { get; set; }

        /// <summary>
        ///     返回处理信息
        /// </summary>
        [JsonProperty("respMsg")]
        public string RespMsg { get; set; }

        /// <summary>
        ///     返回码
        /// </summary>
        [JsonProperty("respSubCode")]
        public string RespSubCode { get; set; }

        /// <summary>
        ///     状态(I-处理中,S-成功,F-失败)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}