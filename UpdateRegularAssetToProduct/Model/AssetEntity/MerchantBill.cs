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
    [Description("商票")]
    public class MerchantBill : EntityBase<long>
    {
        /// <summary>
        ///     出票-出票日期
        /// </summary>
        /// <value>The bill date.</value>
        [Column(TypeName = "DateTime2")]
        public DateTime? BillDate { get; set; }

        /// <summary>
        ///     持有天数
        /// </summary>
        /// <value>The bill own days.</value>
        public long? BillOwnDays { get; set; }

        /// <summary>
        ///     票面扫描件
        /// </summary>
        [StringLength(1024)]
        public string EndorserFin { get; set; }

        /// <summary>
        ///     融资方介绍
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string FinancierIntroduction { get; set; }

        /// <summary>
        ///     用户显示融资方全称
        /// </summary>
        [Required]
        [StringLength(500)]
        public string FinancierNameOfUser { get; set; }

        /// <summary>
        ///     收票日期
        /// </summary>
        /// <value>The get bill date.</value>
        [Column(TypeName = "DateTime2")]
        public DateTime? GetBillDate { get; set; }

        /// <summary>
        ///     担保方
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
        [DataType(DataType.Text)]
        public string GuaranteeIntroduction { get; set; }

        /// <summary>
        ///     风控类型    担保类型 (担保类型 10、银行保兑 20、央企担保 30、国企担保 40、国有担保公司担保 50、担保公司担保 60、上市集团担保 70、集团担保 80、国资参股担保公司担保 90、银行担保 100、金银猫全程监管资金流向)
        /// </summary>
        /// <value>The guarantee property.</value>
        public string GuaranteeTypeCode { get; set; }

        /// <summary>
        ///     风控类型    担保类型 (担保类型 10、银行保兑 20、央企担保 30、国企担保 40、国有担保公司担保 50、担保公司担保 60、上市集团担保 70、集团担保 80、国资参股担保公司担保 90、银行担保 100、金银猫全程监管资金流向)
        /// </summary>
        /// <value>The name of the guarantee type.</value>
        public string GuaranteeTypeName { get; set; }

        /// <summary>
        ///     实际就是AssetId
        /// </summary>
        /// <value>The draft bill identifier.</value>
        [StringLength(32)]
        public string MerchantBillId { get; set; }

        /// <summary>
        ///     付款人账号
        /// </summary>
        /// <value>The pay account.</value>
        public string PayAccount { get; set; }

        /// <summary>
        ///     付款行全称 --付款人开户行全称
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
        ///     用户显示付款方全称
        /// </summary>
        public string PayerNameOfUser { get; set; }

        /// <summary>
        ///     付款人全称
        /// </summary>
        [Required]
        public string PayFullName { get; set; }

        /// <summary>
        ///     付款方信息--付款方介绍
        /// </summary>
        [DataType(DataType.Text)]
        public string PaymentIntroduction { get; set; }

        /// <summary>
        ///     收款人账号
        /// </summary>
        /// <value>The receiver account.</value>
        public string ReceiverAccount { get; set; }

        /// <summary>
        ///     收款人开户银行
        /// </summary>
        [StringLength(100)]
        public string ReceiverBank { get; set; }

        /// <summary>
        ///     收款人全称
        /// </summary>
        [StringLength(100)]
        public string ReceiverName { get; set; }
    }
}