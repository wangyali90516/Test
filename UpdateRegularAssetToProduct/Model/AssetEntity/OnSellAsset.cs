using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRegularAssetToProduct
{
    /// <summary>
    /// 余额猫在售资产池
    /// </summary>
    [Description("余额猫在售资产")]
    public class OnSellAsset : EntityBase<long>
    {
        /// <summary>
        ///     业务主键
        /// </summary>
        [StringLength(32)]
        public string OnSellAssetId { get; set; }

        #region 基础信息
        /// <summary>
        ///     资产编号
        /// </summary>
        [StringLength(20)]
        public string AssetCode { get; set; }

        /// <summary>
        ///     资产详情
        /// </summary>
        /// <value>The asset details.</value>
        public string AssetDetails { get; set; }

        /// <summary>
        ///     资产名字
        /// </summary>
        /// <value>The name.</value>
        public string AssetName { get; set; }

        /// <summary>
        ///     资产所属产品类别
        /// </summary>
        /// <value>The type of the asset owner.</value>
        public long AssetProductCategory { get; set; }

        /// <summary>
        ///     可用金额
        /// </summary>
        public long AvailableAmount { get; set; }

        /// <summary>
        ///     收票利率
        /// </summary>
        [Description("收票利率")]
        public long BillCost { get; set; }

        #region 计息时间

        /// <summary>
        ///     计息开始时间
        /// </summary>
        /// <value>The bill due date.</value>
        [Column(TypeName = "DateTime2")]
        public DateTime? BillAccrualDate { get; set; }

        /// <summary>
        ///     到期日
        /// </summary>
        /// <value>The bill due date.</value>
        [Column(TypeName = "DateTime2")]
        public DateTime BillDueDate { get; set; }

        #endregion 计息时间

        /// <summary>
        ///     票面金额
        /// </summary>
        [Required]
        public long BillMoney { get; set; }

        /// <summary>
        ///     资产票号
        /// </summary>
        [StringLength(50)]
        public string BillNo { get; set; }

        /// <summary>
        ///     出票利率
        /// </summary>
        [Description("售票利率")]
        public long BillRate { get; set; }

        /// <summary>
        ///     已经计算过后的融资金额
        /// </summary>
        [Description("已经计算过后的融资金额")]
        public long CalculatedAmount { get; set; }

        /// <summary>
        ///     流程中的批注信息
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        /// <summary>
        ///     累计赎回金额
        /// </summary>
        public long CumulativeRedemption { get; set; }

        /// <summary>
        ///     融资方id
        /// </summary>
        [Required]
        public string FinancierId { get; set; }

        /// <summary>
        ///     融资方名称
        /// </summary>
        [Required]
        public string FinancierName { get; set; }

        /// <summary>
        ///     融资类型（个人 & 中介 & 原收款企业）
        /// </summary>
        /// <value>The type of the financing.</value>
        public long FinancingType { get; set; }

        /// <summary>
        ///     资产状态
        /// </summary>
        [StringLength(10)]
        public string FlowStatus { get; set; }

        /// <summary>
        ///     获得的利息
        /// </summary>
        public long Interest { get; set; }

        /// <summary>
        ///     表示资产是否为新的
        /// </summary>
        public string IsNewStatus { get; set; }

        #region 类别

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
        ///     资产类型（小类）
        /// </summary>
        [StringLength(32)]
        public string AssetTypeId { get; set; }

        /// <summary>
        ///     资产类型（小类）
        /// </summary>
        [StringLength(50)]
        public string AssetTypeName { get; set; }

        #endregion 类别

        /// <summary>
        ///     资产的优先级（在添加项目时赋值）
        /// </summary>
        public long PriorityLevel { get; set; }

        /// <summary>
        ///     募集状态 false-->募集未完成  true-->募集完成
        /// </summary>
        public bool RaiseStatus { get; set; }

        /// <summary>
        ///     该资产赎回金额
        /// </summary>
        public long RedeemTotal { get; set; }

        /// <summary>
        ///     该资产剩余金额
        /// </summary>
        public long RemainderTotal { get; set; }

        /// <summary>
        /// 售卖金额
        /// </summary>
        public long SellAmount { get; set; }

        /// <summary>
        ///     资产状态
        /// </summary>
        [StringLength(10)]
        public string Status { get; set; }

        /// <summary>
        /// 融资用途
        /// </summary>
        /// <value>The fund usage.</value>
        public string FundUsage { get; set; }

        /// <summary>
        ///     售罄时间
        /// </summary>
        /// <value>The sell out time.</value>
        public DateTime? SellOutTime { get; set; }

        /// <summary>
        /// 当前价值
        /// </summary>
        /// <value>The present value.</value>
        public long PresentValue { get; set; }

        /// <summary>
        /// 资产增值开始时间
        /// </summary>
        /// <value>The growth time.</value>
        public DateTime? GrowthTime { get; set; }

        /// <summary>
        ///     上一次增值时间
        /// </summary>
        /// <value>The last growth time.</value>
        public DateTime? LastGrowthTime { get; set; }

        /// <summary>
        /// 资产增值状态
        /// </summary>
        /// <value>The growth time.</value>
        public bool ValueStatus { get; set; }

        #endregion 基础信息

        #region 新增字段
        public long EffectiveValue { get; set; }
        public long EnterpriseRate { get; set; }
        #endregion
    }
}
