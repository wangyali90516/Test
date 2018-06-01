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
    ///     资产信息 基类
    ///     对于资产的扩展
    /// </summary>
    [Description("资产信息")]
    public class Asset : EntityBase<long>
    {
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
        ///     业务主键
        /// </summary>
        [StringLength(32)]
        public string AssetId { get; set; }

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
        ///     融资用途
        /// </summary>
        /// <value>The fund usage.</value>
        public string FundUsage { get; set; }

        /// <summary>
        ///     资产增值开始时间
        /// </summary>
        /// <value>The growth time.</value>
        public DateTime? GrowthTime { get; set; }

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
        ///     周期类型
        /// </summary>
        /// <value>The type of the period.</value>
        public string PeriodType { get; set; }

        ///// <summary>
        ///// 融资方回款本息和
        ///// </summary>
        ///// <value>The returned money.</value>
        //public long ReturnedMoney { get; set; }
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
        ///     该资产赎回金额
        /// </summary>
        public long RedeemTotal { get; set; }

        /// <summary>
        ///     该资产剩余金额
        /// </summary>
        public long RemainderTotal { get; set; }

        /// <summary>
        ///     售卖金额
        /// </summary>
        public long SellAmount { get; set; }

        /// <summary>
        ///     资产状态
        /// </summary>
        [StringLength(10)]
        public string Status { get; set; }

        /// <summary>
        ///     资产增值状态
        /// </summary>
        /// <value>The growth time.</value>
        public bool ValueStatus { get; set; }

        #endregion 基础信息

        #region 存管新增字段

        /// <summary>
        ///     受托人银行卡号（isEntrustedPay =1时必填）
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNO { get; set; }

        /// <summary>
        ///     银行编码（isEntrustedPay =1 时必填）
        /// </summary>
        /// <value>The bank code.</value>
        public string BankCode { get; set; }

        /// <summary>
        ///     标的类型
        ///     01-信用
        ///     02-抵押
        ///     03-债权转让
        ///     99-其他
        /// </summary>
        /// <value>The type of the bid.</value>
        public string BidType { get; set; }

        /// <summary>
        ///     借款用途
        /// </summary>
        /// <value>The borr purpose.</value>
        public string BorrPurpose { get; set; }

        /// <summary>
        ///     账户对公对私标识（isEntrustedPay=1 时必填）
        ///     1-对公
        ///     2-对私
        /// </summary>
        /// <value>The card flag.</value>
        public string CardFlag { get; set; }

        /// <summary>
        ///     户名（isEntrustedPay =1 时必填）
        /// </summary>
        /// <value>The name of the card.</value>
        public string CardName { get; set; }

        /// <summary>
        ///     受托支付标识（默认 0）
        ///     0- 否
        ///     1- 是
        /// </summary>
        /// <value>The is entrusted pay.</value>
        public string IsEntrustedPay { get; set; }

        /// <summary>
        ///     联行号（isEntrustedPay =1 且 cardFlag=1 时必填）
        /// </summary>
        /// <value>The issuer.</value>
        public string Issuer { get; set; }

        /// <summary>
        ///     标的产品类型
        ///     01-房贷类
        ///     02-车贷类
        ///     03-收益权转让类
        ///     04-信用贷款类
        ///     05-股票配资类
        ///     06-银行承兑汇票
        ///     07-商业承兑汇票
        ///     08-消费贷款类
        ///     09-供应链类
        ///     99-其他
        /// </summary>
        /// <value>The type of the product.</value>
        public string ProductType { get; set; }

        /// <summary>
        ///     标的还款方式
        ///     01 一次还款方式
        ///     02 等额本金
        ///     03 等额本息
        ///     04 按期付息到期还本
        ///     99 其他
        /// </summary>
        /// <value>The type of the repayment.</value>
        public string RepaymentType { get; set; }

        /// <summary>
        ///     报备失败原因
        /// </summary>
        public string ReportedFailureReason { get; set; }

        #endregion 存管新增字段

        #region 余额猫标的报备新增字段

        /// <summary>
        ///     标的是否报备：true-已报备, false-未报备
        /// </summary>
        public bool YemBidIsReported { get; set; }

        /// <summary>
        ///     余额猫标的报备备注
        /// </summary>
        public string YemBidReportMemo { get; set; }

        /// <summary>
        ///     余额猫标的报备时间
        /// </summary>
        public DateTime? YemBidReportTime { get; set; }

        #endregion 余额猫标的报备新增字段
    }
}