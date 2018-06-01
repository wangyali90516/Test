using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToTableTool.Model
{
    public class InsertOnSellAssetInput
    {
        /// <summary>
        ///     资产类型
        ///     10000-银票
        ///     20000-商票
        ///     30000-个贷
        ///     40000-企贷
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
        ///     受托人银行卡号(isEntrustedPay=1时必填)
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     银行编码(isEntrustedPay=1时必填)
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        ///     标的类型(01-信用,02-抵债,03-债权转让,99-其它)
        /// </summary>
        public string BidType { get; set; }

        /// <summary>
        ///     票面金额
        /// </summary>
        public long BillAmount { get; set; }

        /// <summary>
        ///     到期日
        /// </summary>
        public DateTime BillDueDate { get; set; }

        /// <summary>
        ///     资产票号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        ///     借款用途
        /// </summary>
        public string BorrPurpose { get; set; }

        /// <summary>
        ///     票面金额计算出的融资金额
        /// </summary>
        public long CalculatedAmount { get; set; }

        /// <summary>
        ///     账户对公对私标识(isEntrustedPay=1时必填)1-对公,2-对私
        /// </summary>
        public string CardFlag { get; set; }

        /// <summary>
        ///     户名(isEntrustedPay=1时必填)
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        ///     个人-身份证号码
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        ///     企业-营业执照号码
        /// </summary>
        public string EnterpriseLicenseNum { get; set; }

        /// <summary>
        ///     证件类型(10-身份证,20-护照,30-港澳台身份证,40-军官证,50-BLC营业执照,60-USCC统一社会信用代码)
        /// </summary>

        public string EnterpriseLicenseType { get; set; }

        /// <summary>
        ///     融资方编号
        /// </summary>
        public string FinancierId { get; set; }

        /// <summary>
        ///     融资方名称
        /// </summary>
        public string FinancierName { get; set; }

        /// <summary>
        ///     个人/融资方类型 0-企业 1-个人
        /// </summary>
        public string FinancierType { get; set; }

        /// <summary>
        ///     受托支付标识(0-否(默认),1-是)
        /// </summary>

        public string IsEntrustedPay { get; set; }

        /// <summary>
        ///     联行号(isEntrustedPay=1且cardFlag=1时必填）
        /// </summary>

        public string Issuer { get; set; }

        /// <summary>
        ///     固定周期天数
        /// </summary>
        public long PeriodDays { get; set; }

        /// <summary>
        ///     周期类型
        /// </summary>

        public long PeriodType { get; set; }

        /// <summary>
        ///     标的产品类型(01-房贷类,02-车贷类,03-收益权转让类,04-信用贷款类,05-股票配资类,06-银行承兑汇票,07-商业承兑汇票,08-消费贷款类,09-供应链类,99-其他)
        /// </summary>

        public string ProductType { get; set; }

        /// <summary>
        ///     年化利率
        /// </summary>

        public int Rate { get; set; }

        /// <summary>
        ///     还款方式(01-一次还本付息,02-等额本金,03-等额本息,04-按期付息到期还本,99-其他)
        /// </summary>

        public string RepaymentType { get; set; }
    }
}