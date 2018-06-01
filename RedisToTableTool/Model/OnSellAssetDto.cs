using System;
using System.ComponentModel;
using Microsoft.WindowsAzure.Storage.Table;

namespace UserToAssetTool
{
    public class OnSellAssetDto : AssetBaseDto
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
        [Description("资产类型")]
        public int AssetCategoryCode { get; set; }

        /// <summary>
        ///     资产编号
        /// </summary>
        [Description("资产编号")]
        public string AssetCode { get; set; }

        /// <summary>
        ///     资产名称
        /// </summary>
        [Description("资产名称")]
        public string AssetName { get; set; }

        /// <summary>
        ///     资产所属产品类别
        /// </summary>
        [Description("资产所属产品类别")]
        public long AssetProductCategory { get; set; }

        /// <summary>
        ///     计息开始时间
        /// </summary>
        /// <value>The bill due date.</value>
        [Description("计息开始时间")]
        public DateTime? BillAccrualDate { get; set; }

        /// <summary>
        ///     到期日
        /// </summary>
        /// <value>The bill due date.</value>
        [Description("到期日")]
        public DateTime BillDueDate { get; set; }

        /// <summary>
        ///     票面金额
        /// </summary>
        [Description("票面金额")]
        public long BillMoney { get; set; }

        /// <summary>
        ///     票号
        /// </summary>
        [Description("票号")]
        public string BillNo { get; set; }

        /// <summary>
        ///     售票利率
        /// </summary>
        [Description("售票利率")]
        public long BillRate { get; set; }

        /// <summary>
        ///     已经计算过后的融资金额
        /// </summary>
        [Description("已经计算过后的融资金额")]
        public long CalculatedAmount { get; set; }

        /// <summary>
        ///     有效价值
        /// </summary>
        /// <value>The effective value.</value>
        public long EffectiveValue { get; set; }

        /// <summary>
        ///     企业利率
        /// </summary>
        /// <value>The enterprise rate.</value>
        public long EnterpriseRate { get; set; }

        /// <summary>
        ///     融资方Id
        /// </summary>
        [Description("融资方Id")]
        public string FinancierId { get; set; }

        /// <summary>
        ///     融资方
        /// </summary>
        [Description("融资方")]
        public string FinancierName { get; set; }

        /// <summary>
        ///     资产增值开始时间
        /// </summary>
        /// <value>The growth time.</value>
        [Description("资产增值开始时间")]
        public DateTime? GrowthTime { get; set; }

        /// <summary>
        ///     是否被锁定
        /// </summary>
        /// <value><c>true</c> if this instance is lock; otherwise, <c>false</c>.</value>
        [Description("是否被锁定")]
        public bool IsLock { get; set; }

        /// <summary>
        ///     表示资产是否为新的
        /// </summary>
        [Description("表示资产是否为新的")]
        public string IsNewStatus { get; set; }

        /// <summary>
        ///     上一次增值时间
        /// </summary>
        /// <value>The last growth time.</value>
        public DateTime? LastGrowthTime { get; set; }

        /// <summary>
        ///     业务主键
        /// </summary>
        [Description("业务主键")]
        public string OnSellAssetId { get; set; }

        /// <summary>
        ///     当前价值
        /// </summary>
        /// <value>The present value.</value>
        public long PresentValue { get; set; }

        /// <summary>
        ///     资产的优先级（在添加项目时赋值）
        /// </summary>
        public long PriorityLevel { get; set; }

        /// <summary>
        ///     募集状态 false-->募集未完成  true-->募集完成
        /// </summary>
        public bool RaiseStatus { get; set; }

        /// <summary>
        ///     该资产剩余金额
        /// </summary>
        public long RemainderTotal { get; set; }

        /// <summary>
        ///     售卖金额
        /// </summary>
        public long SellAmount { get; set; }

        /// <summary>
        ///     售罄时间
        /// </summary>
        /// <value>The sell out time.</value>
        public DateTime? SellOutTime { get; set; }

        /// <summary>
        ///     资产状态
        /// </summary>
        [Description("资产状态")]
        public int Status { get; set; }

        /// <summary>
        ///     资产增值状态
        /// </summary>
        /// <value>The growth time.</value>
        public bool ValueStatus { get; set; }

        /// <summary>
        ///     标的是否报备：true-已报备, false-未报备
        /// </summary>
        [Description("标的是否报备：true-已报备, false-未报备")]
        public bool YemBidIsReported { get; set; }
    }
}