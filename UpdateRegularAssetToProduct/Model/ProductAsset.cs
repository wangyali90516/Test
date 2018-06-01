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
    ///     产品都包含那些资产
    /// </summary>
    [Description("产品资产")]
    public class ProductAsset : EntityBase<long>
    {
        /// <summary>
        ///     资产大类编码
        /// </summary>
        /// <value>The asset category code.</value>
        public int AssetCategoryCode { get; set; }

        /// <summary>
        ///     资产Id
        /// </summary>
        /// <value>The asset identifier.</value>
        public string AssetId { get; set; }

        /// <summary>
        ///     资产匹配给 产品的金额(即资产打包的金额)
        /// </summary>
        /// <value>The asset match total.</value>
        public long AssetMatchTotal { get; set; }

        /// <summary>
        ///     Gets or sets the name of the asset.
        /// </summary>
        /// <value>The name of the asset.</value>
        public string AssetName { get; set; }

        /// <summary>
        ///     Gets or sets the asset ower.
        /// </summary>
        /// <value>The asset ower.</value>
        public string AssetOwer { get; set; }

        /// <summary>
        ///     资产小类型
        /// </summary>
        [StringLength(32)]
        public string AssetTypeId { get; set; }


        /// <summary>
        /// 资产类型名称
        /// </summary>
        /// <value>The name of the asset type.</value>
        public string AssetTypeName { get; set; }

        /// <summary>
        ///     过期时间===对应Asset中 BillDueDate字段
        /// </summary>
        /// <value>The expire date.</value>
        [Column(TypeName = "Date")]
        public DateTime ExpireDate { get; set; }

        /// <summary>
        ///     资产优先级
        /// </summary>
        /// <value>The priority level.</value>
        public long PriorityLevel { get; set; }

        /// <summary>
        ///     产品资产Id=本表业务主键
        /// </summary>
        /// <value>The product asset identifier.</value>
        public string ProductAssetId { get; set; }

        /// <summary>
        ///     产品Id
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductId { get; set; }

        /// <summary>
        ///     剩余金额==分配给用户的时候,该字段需要更新
        /// </summary>
        /// <value>The remainder total.</value>
        public long RemainderTotal { get; set; }

        #region 票面扫描件

        /// <summary>
        ///     票面扫描件
        /// </summary>
        [StringLength(300)]
        public string EndorserFin { get; set; }


        /// <summary>
        ///     票背面扫描件
        /// </summary>
        [StringLength(300)]
        public string BackOfScanning { get; set; }

        #endregion
        #region 质押物

        /// <summary>
        /// 抵押物编号(房产证编号，车辆编号等)
        /// </summary>
        /// <value>The collateral code.</value>
        public string CollateralNo { get; set; }

        #endregion
        #region 付款行
        /// <summary>
        ///     付款行全称
        /// </summary>
        public string PayBankFullName { get; set; }
        #endregion

        #region 融资方


        /// <summary>
        /// 融资方Id
        /// </summary>
        /// <value>The Financier identifier.</value>
        public string FinancierId { get; set; }


        /// <summary>
        /// 融资方名称(企业或者是个人名称)
        /// </summary>
        /// <value>The name of the Financier.</value>
        public string FinancierName { get; set; }


        /// <summary>
        ///     营业执照(企业需要)
        /// </summary>
        public string EnterpriseLicense { get; set; }


        /// <summary>
        ///     资产融资方(融资作用)
        /// </summary>
        [DataType(DataType.Text)]
        public string FinancierIntroduction { get; set; }


        #endregion


        #region 风控担保方

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

        #endregion

        #region 付款方

        /// <summary>
        ///     付款方名称(企业为企业名称)
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        ///     付款方信息--付款方介绍
        /// </summary>
        [DataType(DataType.Text)]
        public string PaymentIntroduction { get; set; }

        #endregion
    }
}
