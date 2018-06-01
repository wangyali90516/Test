using System;
using System.Collections.Generic;

namespace RedisToTableTool.Model
{
    public class SendDebtForBankModel
    {
        /// <summary>
        ///     币种(默认:CNY)
        /// </summary>
        /// <value>The currency.</value>
        public string Currency { get; set; }

        /// <summary>
        ///     由网贷平台生成的唯一的交易流水号 stringReg. Exp.:^[A-Z0-9]{32}|[a-z0-9]{32}$,
        /// </summary>
        /// <value>The order identifier.</value>
        public string OrderId { get; set; }

        /// <summary>
        ///     备注 stringMax. Length:256Reg
        /// </summary>
        /// <value>The remark.</value>
        public string Remark { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }

        /// <summary>
        ///     批量投资债转集合
        /// </summary>
        /// <value>The sub invest order list.</value>
        public List<SubInvestOrderInputDto> SubInvestOrderList { get; set; }

        /// <summary>
        ///     投资总金额(单位:分)
        /// </summary>
        /// <value>The total amount.</value>
        public long TotalAmount { get; set; }

        public DateTime? UpdatedTime { get; set; }

        /// <summary>
        ///     投资人网贷平台唯一的用户编码
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserId { get; set; }
    }

    /// <summary>
    ///     Class SubInvestOrderInputDto.
    /// </summary>
    public class SubInvestOrderInputDto1
    {
        /// <summary>
        ///     转让本金,必须大于0
        /// </summary>
        /// <value>The amount.</value>
        public long Amount { get; set; }

        /// <summary>
        ///     出让人（原始投资人），网贷平台唯一的用户编码 stringReg. Exp.:^[A-Z0-9]{32}|[a-z0-9]{32}$,
        /// </summary>
        /// <value>The assignor user identifier.</value>
        public string AssignorUserId { get; set; }

        /// <summary>
        ///     单次债转主键
        /// </summary>
        /// <value>The debt to transfer identifier.</value>
        public string DebtToTransferId { get; set; }

        /// <summary>
        ///     折让金额
        /// </summary>
        /// <value>The discount.</value>
        public long Discount { get; set; }

        /// <summary>
        ///     转让利息，可以填0
        /// </summary>
        /// <value>The interest.</value>
        public long Interest { get; set; }

        /// <summary>
        ///     原投资交易流水号（出让人原投资单号） stringReg. Exp.:^[A-Z0-9]{32}|[a-z0-9]{32}$
        /// </summary>
        /// <value>The invest order identifier.</value>
        public string InvestOrderId { get; set; }

        /// <summary>
        ///     赎回订单Id
        /// </summary>
        /// <value>The redemption order identifier.</value>
        public string RedemptionOrderId { get; set; }

        /// <summary>
        ///     Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int Status { get; set; }

        /// <summary>
        ///     外部子订单单号
        /// </summary>
        /// <value>The sub order identifier.</value>
        public string SubOrderId { get; set; }
    }
}