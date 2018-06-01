using System.Collections.Generic;
using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    /// <summary>
    ///     Class BatchCreditCreateInputRequest.
    /// </summary>
    public class BatchCreditCreateInputRequest
    {
        /// <summary>
        ///     失败原因
        /// </summary>
        /// <value>The fail reason.</value>
        [JsonProperty("failReason")]
        public string FailReason { get; set; }

        /// <summary>
        ///     S = 成功
        ///     F = 失败
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        ///     请求流水号
        /// </summary>
        /// <value>The sub order identifier.</value>
        [JsonProperty("subOrderId")]
        public string SubOrderId { get; set; }
    }

    public class NotifyBatchCreditRequest
    {
        /// <summary>
        ///     商户编号
        /// </summary>
        /// <value>The merchant identifier.</value>
        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        ///     由网贷平台生成的唯一的交易流水号
        /// </summary>
        /// <value>The order identifier.</value>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        /// <value>The remark.</value>
        [JsonProperty("remark")]
        public string Remark { get; set; }

        /// <summary>
        ///     处理子单结果集合
        /// </summary>
        /// <value>The result list.</value>
        [JsonProperty("resultList")]
        public List<BatchCreditCreateInputRequest> ResultList { get; set; }

        /// <summary>
        ///     签名，附加说明
        /// </summary>
        /// <value>The signature.</value>
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}