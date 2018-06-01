using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    public class WebResponseBase
    {
        /// <summary>
        ///     加密后的业务数据
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }

        /// <summary>
        ///     商户编码
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        ///     密钥
        /// </summary>
        [JsonProperty("tm")]
        public string Tm { get; set; }
    }
}