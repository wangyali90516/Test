using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JinYinMao.Business.Assets.Request
{
    /// <summary>
    ///     Class AdvanceDebtRequest.
    /// </summary>
    public class AdvanceDebtRequest
    {
        /// <summary>
        ///     币种 CNY -人民币
        /// </summary>
        /// <value>The currency.</value>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        ///     批量债转通知银行流水号
        /// </summary>
        /// <value>The debt identifier.</value>
        [JsonProperty("debtId")]
        public string DebtId { get; set; }

        /// <summary>
        ///     债转关系主键
        /// </summary>
        /// <value>The debt to transfer identifier.</value>
        [JsonProperty("debtToTransferId")]
        public string DebtToTransferId { get; set; }

        /// <summary>
        ///     原标的投资单号
        /// </summary>
        /// <value>The invest order identifier.</value>
        [JsonProperty("investOrderId")]
        public string InvestOrderId { get; set; }

        /// <summary>
        ///     由网贷平台生成的唯一的交易流水号
        /// </summary>
        /// <value>The order identifier.</value>
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        /// <summary>
        ///     赎回Id
        /// </summary>
        /// <value>The redemption order identifier.</value>
        [JsonProperty("redemptionOrderId")]
        public string RedemptionOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        ///     债权投资列表
        /// </summary>
        /// <value>The sub order list.</value>
        [JsonProperty("subOrderList")]
        public List<CreditAssignmentOrderModel> SubOrderList { get; set; }

        /// <summary>
        ///     放款总金额，单位（分）放款总金额=债转投资订单总金额，系统将进行校验，金额不一致放款失败
        /// </summary>
        /// <value>The total amount.</value>
        [JsonProperty("totalAmount")]
        public long TotalAmount { get; set; }

        /// <summary>
        ///     手续费总金额，单位（分），无费用，请填写 0
        /// </summary>
        /// <value>The total fee.</value>
        [JsonProperty("totalFee")]
        public long TotalFee { get; set; }

        /// <summary>
        ///     放款总笔数
        /// </summary>
        /// <value>The total number.</value>
        [JsonProperty("totalNum")]
        public long TotalNum { get; set; }

        /// <summary>
        ///     Gets or sets the updated time.
        /// </summary>
        /// <value>The updated time.</value>
        [JsonProperty("updatedTime")]
        public DateTime? UpdatedTime { get; set; }

        /// <summary>
        ///     出让人，网贷平台唯一的用户编码
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }

    /// <summary>
    ///     Class CreditAssignmentOrderRequest.
    /// </summary>
    public class CreditAssignmentOrderModel
    {
        /// <summary>
        ///     债转订单号（可为多条） (赎回订单号)
        /// </summary>
        /// <value>The credit assign order identifier.</value>
        [JsonProperty("creditAssignOrderId")]
        public string CreditAssignOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the fee.
        /// </summary>
        /// <value>The fee.</value>
        [JsonProperty("fee")]
        public long Fee { get; set; }

        /// <summary>
        ///     收款方，网贷平台唯一的用户编码
        /// </summary>
        /// <value>The receive user identifier.</value>
        [JsonProperty("receiveUserId")]
        public string ReceiveUserId { get; set; }

        /// <summary>
        ///     收费子订单号
        /// </summary>
        /// <value>The sub order identifier.</value>
        [JsonProperty("subOrderId")]
        public string SubOrderId { get; set; }
    }
}