using System;

namespace WindowsFormsApp1
{
    public class AssetModel
    {
        #region Nested type: OnSellAssetAddRequest

        /// <summary>
        /// </summary>
        public class OnSellAssetAddRequest
        {
            /// <summary>
            ///     [Description("银票")]
            ///     DraftBill = 10000,
            ///     [Description("商票")]
            ///     MerchantBill = 20000,
            ///     [Description("个贷")]
            ///     PeerToPeer = 30000,
            ///     [Description("企贷资产")]
            ///     Enterprise = 40000
            /// </summary>

            public int AssetCategoryCode { get; set; }

            /// <summary>
            ///     资产编号
            /// </summary>

            public string AssetCode { get; set; }

            /// <summary>
            ///     资产Id
            /// </summary>

            public string AssetId { get; set; }

            /// <summary>
            ///     资产名称
            /// </summary>

            public string AssetName { get; set; }

            /// <summary>
            ///     计息开始时间
            /// </summary>
            /// <value>The bill due date.</value>
            public DateTime? BillAccrualDate { get; set; }

            /// <summary>
            ///     收票利率
            /// </summary>
            public long BillCost { get; set; }

            /// <summary>
            ///     到期日
            /// </summary>
            /// <value>The bill due date.</value>
            public DateTime BillDueDate { get; set; }

            /// <summary>
            ///     票面金额
            /// </summary>
            public long BillMoney { get; set; }

            /// <summary>
            ///     票号
            /// </summary>
            public string BillNo { get; set; }

            /// <summary>
            ///     已经计算过后的融资金额,需要计算
            /// </summary>
            public long CalculatedAmount { get; set; }

            /// <summary>
            ///     企业利率
            /// </summary>
            /// <value>The enterprise rate.</value>
            public long EnterpriseRate { get; set; }

            /// <summary>
            ///     融资方Id
            /// </summary>
            public string FinancierId { get; set; }

            /// <summary>
            ///     融资方
            /// </summary>
            public string FinancierName { get; set; }

            /// <summary>
            ///     资产第一次增值时间
            /// </summary>
            public DateTime? GrowthTime { get; set; }

            /// <summary>
            ///     固定周期天数
            /// </summary>
            public long PeriodDays { get; set; }

            /// <summary>
            ///     周期类型：0-到期日, 1-固定周期
            /// </summary>
            public int PeriodType { get; set; }

            /// <summary>
            ///     资产当前价值
            /// </summary>
            public long PresentValue { get; set; }

            /// <summary>
            ///     报备失败原因
            /// </summary>
            public string ReportedFailureReason { get; set; }
        }

        #endregion Nested type: OnSellAssetAddRequest
    }
}