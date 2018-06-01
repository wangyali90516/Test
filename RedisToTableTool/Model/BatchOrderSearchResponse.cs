using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    public class BatchOrderSearchResponse
    {
        /// <summary>
        ///     金额(单位:分)
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        ///     业务类型(3005-流标,5000-放款,7000-还款,8000-返利)
        /// </summary>
        [JsonProperty("bizType")]
        public string BizType { get; set; }

        /// <summary>
        ///     币种
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        ///     失败原因
        /// </summary>
        [JsonProperty("failReason")]
        public string FailReason { get; set; }

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
        ///     备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }

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
        ///     订单状态(I-处理中,S-成功,F-失败,C-处理完成【返利接口处理完成（部分成功情况）】)
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}