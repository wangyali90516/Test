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
    [Description("项目-资产 内容")]
    public class InvestProjectAsset : EntityBase<long>
    {
        /// <summary>
        ///     可用金额
        /// </summary>
        public long AvailableAmount { get; set; }

        /// <summary>
        ///     票面金额(原来的出票-金额)
        /// </summary>
        public long BillMoney { get; set; }

        /// <summary>
        ///     业务主键
        /// </summary>
        [StringLength(32)]
        [Required]
        public string InvestProjectAssetId { get; set; }

        /// <summary>
        ///     项目Id
        /// </summary>
        [StringLength(32)]
        [Required]
        public string InvestProjectId { get; set; }

        /// <summary>
        ///     该资产赎回金额
        /// </summary>
        public long RedeemTotal { get; set; }

        /// <summary>
        ///     该资产剩余金额
        /// </summary>
        public long RemainderTotal { get; set; }

        #region 主要信息

        /// <summary>
        ///     资产Id
        /// </summary>
        [StringLength(32)]
        [Required]
        public string AssetId { get; set; }

        /// <summary>
        ///     资产名称
        /// </summary>
        /// <value>The name of the asset.</value>
        public string AssetName { get; set; }

        /// <summary>
        ///     资产优先级
        /// </summary>
        [Required]
        public long AssetPriorityLevel { get; set; }

        /// <summary>
        ///     资产金额(融资金额)
        /// </summary>
        [Required]
        public long AssetSumMoney { get; set; }

        /// <summary>
        ///     资产类型编码
        /// </summary>
        public string AssetTypeCode { get; set; }

        /// <summary>
        ///     资产类型(0001代表银票,0002代表商票,0003代表P2P)
        /// </summary>
        [Required]
        public string AssetTypeId { get; set; }

        /// <summary>
        ///     资产类型名称
        /// </summary>
        public string AssetTypeName { get; set; }

        /// <summary>
        ///     资产到期日
        /// </summary>
        /// <value>The expire date.</value>
        [Column(TypeName = "DateTime2")]
        public DateTime BillDueDate { get; set; }

        /// <summary>
        ///     融资方Id
        /// </summary>
        [StringLength(32)]
        public string FinancierId { get; set; }

        /// <summary>
        ///     融资方名称
        /// </summary>
        [StringLength(100)]
        [Required]
        public string FinancierName { get; set; }

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

        #endregion 主要信息
    }
}