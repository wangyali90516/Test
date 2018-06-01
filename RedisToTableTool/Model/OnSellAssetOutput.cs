using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RedisToTableTool.Model
{
    public class OnSellAssetOutput
    { /// <summary>
      ///     实际融资金额
      /// </summary>
        [JsonProperty("actualCalculatedAmount")]
        public long ActualCalculatedAmount { get; set; }

        /// <summary>
        ///     资产类型
        ///     10000-银票
        ///     20000-商票
        ///     30000-个贷
        ///     40000-企贷
        /// </summary>
        [JsonProperty("assetCategoryCode")]
        public int AssetCategoryCode { get; set; }

        /// <summary>
        ///     资产编号
        /// </summary>
        [JsonProperty("assetCode")]
        public string AssetCode { get; set; }

        /// <summary>
        ///     资产Id
        /// </summary>
        [JsonProperty("assetId")]
        public string AssetId { get; set; }

        /// <summary>
        ///     资产名称
        /// </summary>
        [JsonProperty("assetName")]
        public string AssetName { get; set; }

        /// <summary>
        ///     计息开始时间
        /// </summary>
        [JsonProperty("billAccrualDate")]
        public DateTime? BillAccrualDate { get; set; }

        /// <summary>
        ///     票面金额
        /// </summary>
        [JsonProperty("billAmount")]
        public long BillAmount { get; set; }

        /// <summary>
        ///     到期日
        /// </summary>
        [JsonProperty("billDueDate")]
        public DateTime BillDueDate { get; set; }

        /// <summary>
        ///     资产票号
        /// </summary>
        [JsonProperty("billNo")]
        public string BillNo { get; set; }

        /// <summary>
        ///     票面金额计算出的融资金额
        /// </summary>
        [JsonProperty("calculatedAmount")]
        public long CalculatedAmount { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [JsonProperty("createTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///     每份金额
        /// </summary>
        [JsonProperty("eachAmount")]
        public long EachAmount { get; set; }

        /// <summary>
        ///     融资方编号
        /// </summary>
        [JsonProperty("financierId")]
        public string FinancierId { get; set; }

        /// <summary>
        ///     融资方名称
        /// </summary>
        [JsonProperty("financierName")]
        public string FinancierName { get; set; }

        /// <summary>
        ///     是否是新资产
        /// </summary>
        [JsonProperty("isNew")]
        public bool IsNew { get; set; }

        /// <summary>
        ///     匹配优先级
        /// </summary>
        [JsonProperty("matchingLevel")]
        public int? MatchingLevel { get; set; }

        /// <summary>
        ///     固定周期天数
        /// </summary>
        [JsonProperty("periodDays")]
        public long PeriodDays { get; set; }

        /// <summary>
        ///     周期类型
        /// </summary>
        [JsonProperty("periodType")]
        public long PeriodType { get; set; }

        /// <summary>
        ///     年化利率
        /// </summary>
        [JsonProperty("rate")]
        public int Rate { get; set; }

        /// <summary>
        ///     剩余金额
        /// </summary>
        [JsonProperty("remainderAmount")]
        public long RemainderAmount { get; set; }

        /// <summary>
        ///     售卖金额
        /// </summary>
        [JsonProperty("sellAmount")]
        public long SellAmount { get; set; }

        /// <summary>
        ///     售罄时间
        /// </summary>
        [JsonProperty("sellOutTime")]
        public DateTime? SellOutTime { get; set; }

        /// <summary>
        ///     资产状态
        ///     20-产品使用中
        ///     25-报备中
        ///     24-报备取消
        ///     26-报备成功
        ///     27-报备失败
        ///     4-待打款
        ///     10-打款成功
        ///     17-还款成功
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        #region 需要补充字段

        /// <summary>
        ///     受托人银行卡号(isEntrustedPay=1时必填)
        /// </summary>
        [JsonProperty("bankCardNO")]
        public string BankCardNO { get; set; }

        /// <summary>
        ///     银行编码(isEntrustedPay=1时必填)
        /// </summary>
        [JsonProperty("bankCode")]
        public string BankCode { get; set; }

        /// <summary>
        ///     标的类型(01-信用,02-抵债,03-债权转让,99-其它)
        /// </summary>
        [JsonProperty("bidType")]
        public string BidType { get; set; }

        /// <summary>
        ///     借款用途
        /// </summary>
        [JsonProperty("borrPurpose")]
        public string BorrPurpose { get; set; }

        /// <summary>
        ///     账户对公对私标识(isEntrustedPay=1时必填)1-对公,2-对私
        /// </summary>
        [JsonProperty("cardFlag")]
        public string CardFlag { get; set; }

        /// <summary>
        ///     户名(isEntrustedPay=1时必填)
        /// </summary>
        [JsonProperty("cardName")]
        public string CardName { get; set; }

        /// <summary>
        ///     个人-身份证号码
        /// </summary>
        [JsonProperty("cardNo")]
        public string CardNo { get; set; }

        /// <summary>
        ///     企业-营业执照号码
        /// </summary>
        [JsonProperty("enterpriseLicenseNum")]
        public string EnterpriseLicenseNum { get; set; }

        /// <summary>
        ///     企业-证件类型
        /// </summary>
        [JsonProperty("enterpriseLicenseType")]
        public string EnterpriseLicenseType { get; set; }

        /// <summary>
        ///     个人/融资方类型
        /// </summary>
        [JsonProperty("financierType")]
        public int FinancierType { get; set; }

        /// <summary>
        ///     受托支付标识(0-否(默认),1-是)
        /// </summary>
        [JsonProperty("isEntrustedPay")]
        public string IsEntrustedPay { get; set; }

        /// <summary>
        ///     联行号(isEntrustedPay=1且cardFlag=1时必填）
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        ///     标的产品类型(01-房贷类,02-车贷类,03-收益权转让类,04-信用贷款类,05-股票配资类,06-银行承兑汇票,07-商业承兑汇票,08-消费贷款类,09-供应链类,99-其他)
        /// </summary>
        [JsonProperty("productType")]
        public string ProductType { get; set; }

        /// <summary>
        ///     还款方式(01-一次还本付息,02-等额本金,03-等额本息,04-按期付息到期还本,99-其他)
        /// </summary>
        [JsonProperty("repaymentType")]
        public string RepaymentType { get; set; }

        #endregion 需要补充字段
    }
}