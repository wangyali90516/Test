using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    public class RpOrder
    {
        /// <summary>
        ///     红包金额(单位:分)
        /// </summary>
        [JsonProperty("rpAmount")]
        public double? RpAmount { get; set; }

        /// <summary>
        ///     红包单编号
        /// </summary>
        [JsonProperty("rpSubOrderId")]
        public string RpSubOrderId { get; set; }

        /// <summary>
        ///     网贷平台红包账户(或营销账户)网贷平台唯一的用户编码
        /// </summary>

        [JsonProperty("rpUserId")]
        public string RpUserId { get; set; }
    }
}