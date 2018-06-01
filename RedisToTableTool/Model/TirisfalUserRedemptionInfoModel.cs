using System;
using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    public class TirisfalUserRedemptionInfoModel
    {
        /// <summary>
        ///     赎回确认金额
        /// </summary>
        /// <value>The result time.</value>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        ///     1:债转;2:取消预约冻结
        /// </summary>
        [JsonProperty("confirmType")]
        public int ConfirmType { get; set; }

        /// <summary>
        ///     处理时间
        /// </summary>
        [JsonProperty("resultTime")]
        public DateTime? ResultTime { get; set; }

        /// <summary>
        ///     系统替换订单唯一标识
        /// </summary>
        [JsonProperty("systemAssetRecordIdentifier")]
        public string SystemAssetRecordIdentifier { get; set; }

        /// <summary>
        ///     用户唯一标识
        /// </summary>
        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     赎回订单债转记录唯一标识
        /// </summary>
        [JsonProperty("yemAssetRecordIdentifier")]
        public string YemAssetRecordIdentifier { get; set; }

        /// <summary>
        ///     赎回订单唯一标识
        /// </summary>
        [JsonProperty("yemOrderIdentifier")]
        public string YemOrderIdentifier { get; set; }
    }
}