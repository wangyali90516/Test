using System;

namespace RedisToTableTool.Model
{
    public class DraftBillAddRequest : BaseRequest
    {
        /// <summary>
        ///     资产编号
        /// </summary>
        public string AssetCode { get; set; }

        /// <summary>
        ///     资产详情说明
        /// </summary>
        /// <value>The asset details.</value>

        public string AssetDetails { get; set; }

        /// <summary>
        ///     资产名字
        /// </summary>
        /// <value>The name.</value>

        public string AssetName { get; set; }

        /// <summary>
        ///     资产类型ID（小类）
        /// </summary>

        public string AssetTypeId { get; set; }

        /// <summary>
        ///     资产类型名称（小类）
        /// </summary>

        public string AssetTypeName { get; set; }

        /// <summary>
        ///     收入利率(资产进入金银猫的利率)
        /// </summary>
        /// <value>The bill rate.</value>

        public long BillCost { get; set; }

        /// <summary>
        ///     到期日期,精度到天
        /// </summary>
        /// <value>The bill due date.</value>

        //[Required(ErrorMessage = "到期日必填")]
        public DateTime BillDueDate { get; set; }

        /// <summary>
        ///     融资方id
        /// </summary>

        public string FinancierId { get; set; }

        /// <summary>
        ///     融资方名称
        /// </summary>
        public string FinancierName { get; set; }

        /// <summary>
        ///     融资用途
        /// </summary>
        /// <value>The fund usage.</value>
        public string FundUsage { get; set; }

        /// <summary>
        ///     资产增值开始时间
        /// </summary>
        /// <value>The growth time.</value>
        public DateTime GrowthTime { get; set; }

        /// <summary>
        ///     周期类型
        ///     ///
        ///     <summary>
        ///         /// 到期日
        ///         ///
        ///     </summary>
        ///     DueDate = 0,
        ///     ///
        ///     <summary>
        ///         /// 固定周期
        ///         ///
        ///     </summary>
        ///     FixedCycle = 1
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        ///     当前价值
        /// </summary>
        /// <value>The present value.</value>
        public long PresentValue { get; set; }

        /// <summary>
        ///     资产增值开始时间状态
        /// </summary>
        /// <value>The growth time.</value>
        public bool ValueStatus { get; set; }

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

        public int FlowStatus { get; set; }

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

        #endregion 存管新增字段

        #region 整理

        /// <summary>
        ///     票背面扫描件
        /// </summary>
        public string BackOfScanning { get; set; }

        /// <summary>
        ///     出票-出票日期
        /// </summary>
        /// <value>The bill date.</value>
        public DateTime BillDate { get; set; }

        /// <summary>
        ///     票面金额
        /// </summary>
        /// <value>The bill money.</value>
        public long BillMoney { get; set; }

        /// <summary>
        ///     票号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        ///     持有天数
        /// </summary>
        public long BillOwnDays { get; set; }

        /// <summary>
        ///     出票人全称（必填）
        /// </summary>
        public string DrawerName { get; set; }

        /// <summary>
        ///     票面扫描件
        /// </summary>
        public string EndorserFin { get; set; }

        /// <summary>
        ///     收票日期
        /// </summary>
        /// <value>The get bill date.</value>
        public DateTime GetBillDate { get; set; }

        /// <summary>
        ///     收票利息
        /// </summary>
        public long Interest { get; set; }

        /// <summary>
        ///     付款人开户行全称（必填）
        /// </summary>
        public string PayBankFullName { get; set; }

        /// <summary>
        ///     付款行行号
        /// </summary>
        public string PayingBankNo { get; set; }

        /// <summary>
        ///     收款人账号
        /// </summary>
        //[RegularExpression(@"^\d{15,19}$")]
        public string ReceiverAccount { get; set; }

        /// <summary>
        ///     收款人开户银行
        /// </summary>
        public string ReceiverBank { get; set; }

        /// <summary>
        ///     收款人全称
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        ///     收款方全称
        /// </summary>
        public string ReceivingSideFullName { get; set; }

        /// <summary>
        ///     收款方ID
        /// </summary>
        public string ReceivingSideId { get; set; }

        #endregion 整理
    }
}