using System;

namespace RedisToTableTool.Model
{
    public class MerchantBillAddRequest : BaseRequest
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

        public long BillCost { get; set; }

        /// <summary>
        ///     出票-出票日期(必填)
        /// </summary>
        /// <value>The bill date.</value>
        public DateTime BillDate { get; set; }

        /// <summary>
        ///     收入利率(资产进入金银猫的利率)
        /// </summary>
        /// <value>The bill rate.</value>
        /// <summary>
        ///     到期日期,精度到天
        /// </summary>
        /// <value>The bill due date.</value>
        //[Required(ErrorMessage = "到期日必填")]
        public DateTime BillDueDate { get; set; }

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
        ///     借款用途
        /// </summary>
        /// <value>The borr purpose.</value>
        public string BorrPurpose { get; set; }

        /// <summary>
        ///     融资金额
        /// </summary>
        public long CalculatedAmount { get; set; }

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
        ///     票面扫描件（必填）
        /// </summary>
        public string EndorserFin { get; set; }

        public string FinancierId { get; set; }

        /// <summary>
        ///     融资方介绍
        /// </summary>
        public string FinancierIntroduction { get; set; }

        /// <summary>
        ///     融资方id
        /// </summary>
        /// <summary>
        ///     融资方名称
        /// </summary>
        public string FinancierName { get; set; }

        /// <summary>
        ///     用户显示融资方全称(必填----从融资方中获取)
        /// </summary>
        public string FinancierNameOfUser { get; set; }

        public int FlowStatus { get; set; }

        /// <summary>
        ///     融资用途
        /// </summary>
        /// <value>The fund usage.</value>
        public string FundUsage { get; set; }

        /// <summary>
        ///     收票日期（必填）
        /// </summary>
        /// <value>The get bill date.</value>
        public DateTime? GetBillDate { get; set; }

        /// <summary>
        ///     资产增值开始时间
        /// </summary>
        /// <value>The growth time.</value>
        public DateTime GrowthTime { get; set; }

        /// <summary>
        ///     担保函（必填）
        /// </summary>
        /// <value>The guarantee.</value>
        public string Guarantee { get; set; }

        /// <summary>
        ///     担保方全称
        /// </summary>
        public string GuaranteeFullName { get; set; }

        /// <summary>
        ///     担保方介绍 风控信息介绍
        /// </summary>
        /// <value>The guarantee introduction.</value>
        public string GuaranteeIntroduction { get; set; }

        /// <summary>
        ///     擔保類型編碼担保类型 (担保类型 10、银行保兑 20、央企担保 30、国企担保 40、国有担保公司担保 50、担保公司担保 60、上市集团担保 70、集团担保 80、国资参股担保公司担保 90、银行担保 100、金银猫全程监管资金流向)
        /// </summary>
        /// <value>The guarantee type code.</value>
        public string GuaranteeTypeCode { get; set; }

        /// <summary>
        ///     擔保方類型名稱担保类型 (担保类型 10、银行保兑 20、央企担保 30、国企担保 40、国有担保公司担保 50、担保公司担保 60、上市集团担保 70、集团担保 80、国资参股担保公司担保 90、银行担保 100、金银猫全程监管资金流向)
        /// </summary>
        /// <value>The name of the guarantee type.</value>
        public string GuaranteeTypeName { get; set; }

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
        ///     付款人账号
        /// </summary>
        public string PayAccount { get; set; }

        /// <summary>
        ///     付款人开户银行全称（必填）
        /// </summary>
        public string PayBankFullName { get; set; }

        /// <summary>
        ///     付款方Id.
        /// </summary>
        /// <value>The payer identifier.</value>
        public string PayerId { get; set; }

        /// <summary>
        ///     付款方名称(企业为企业名称)
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        ///     付款方全称(针对实际客户)
        /// </summary>
        public string PayerNameOfUser { get; set; }

        /// <summary>
        ///     付款人全称（必填----在付款方管理中获取）
        /// </summary>
        public string PayFullName { get; set; }

        /// <summary>
        ///     付款方信息--付款方介绍（必填----在付款方管理中获取）
        /// </summary>
        public string PaymentIntroduction { get; set; }

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
        ///     收款人账号（必填）
        /// </summary>
        /// <value>The receiver account.</value>
        //[Required(ErrorMessage = "收款人账号（必填）")]
        public string ReceiverAccount { get; set; }

        /// <summary>
        ///     收款人开户银行（必填）
        /// </summary>
        //[Required(ErrorMessage = "收款人开户银行（必填）")]
        public string ReceiverBank { get; set; }

        /// <summary>
        ///     收款人全称（必填）
        /// </summary>
        //[Required(ErrorMessage = "收款人全称（必填）")]
        public string ReceiverName { get; set; }

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
        ///     资产增值开始时间状态
        /// </summary>
        /// <value>The growth time.</value>
        public bool ValueStatus { get; set; }
    }
}