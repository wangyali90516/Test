using Microsoft.WindowsAzure.Storage.Table;

namespace RedisToTableTool.Model
{
    public class CancelBookModel : TableEntity
    {
        /// <summary>
        ///     解冻总金额，包含平台收费金额，单位（分）
        /// </summary>
        public long Amount { get; set; }

        public string DebtToTransfer { get; set; }

        /// <summary>
        ///     支持平台向用户收费
        /// </summary>
        public long FeeAmount { get; set; }

        /// <summary>
        ///     仅支持：P2p 平台账户或者网贷平台收费账户
        /// </summary>
        public string FeeUserId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is success.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        ///     由网贷平台生成的唯一的交易流水号
        /// </summary>
        public string OrderId { get; set; }

        public string RedeemOrderId { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     冻结类型 01-预约投资
        /// </summary>
        public string UnFreezeType { get; set; }

        /// <summary>
        ///     网贷平台唯一的用户编码
        /// </summary>
        public string UserId { get; set; }
    }
}