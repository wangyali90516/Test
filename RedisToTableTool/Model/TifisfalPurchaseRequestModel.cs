using System;
using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    public class TifisfalPurchaseRequestModel
    {
        [JsonProperty("allotAmount")]
        public long AllotAmount { get; set; }

        [JsonProperty("isReturnToAccount")]
        public bool IsReturnToAccount { get; set; }

        [JsonProperty("resultTime")]
        public DateTime? ResultTime { get; set; }

        [JsonProperty("userIdentifier")]
        public string UserIdentifier { get; set; }

        [JsonProperty("yemAssetRecordIdentifier")]
        public string YemAssetRecordIdentifier { get; set; }

        [JsonProperty("yemOrderIdentifier")]
        public string YemOrderIdentifier { get; set; }
    }
}