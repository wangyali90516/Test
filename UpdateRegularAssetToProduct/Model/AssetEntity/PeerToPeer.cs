using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRegularAssetToProduct
{
    /// <summary>
    ///     Class PeerToPeer.
    /// </summary>
    [Description("P2P")]
    public class PeerToPeer : EntityBase<long>
    {
        /// <summary>
        ///     资产描述
        /// </summary>
        [Description("资产描述")]
        public string AssetIntroduction { get; set; }

        /// <summary>
        ///     抵押物ID
        /// </summary>
        public string CollateralId { get; set; }

        /// <summary>
        ///     抵押物名称
        /// </summary>
        [Description("抵押物名称")]
        public string CollateralName { get; set; }

        /// <summary>
        /// 抵押物编号(房产证编号，车辆编号等)
        /// </summary>
        /// <value>The collateral code.</value>
        public string CollateralNo { get; set; }

        /// <summary>
        ///     用户显示融资方全称(必填----从融资方中获取)
        /// </summary>
        [Description("用户显示融资方全称")]
        public string FinancierNameOfUser { get; set; }

        /// <summary>
        ///     借贷协议
        /// </summary>
        [Description("借贷协议")]
        public string LoanAgreement { get; set; }

        /// <summary>
        ///     原债务人ID
        /// </summary>
        public string OriginalDebtorId { get; set; }

        /// <summary>
        ///     姓名（全称）
        /// </summary>
        [Description("元债务人名称")]
        public string OriginalDebtorName { get; set; }

        /// <summary>
        ///     实际就是AssetId
        /// </summary>
        [Description("个人ID")]
        public string PeerToPeerId { get; set; }

        /// <summary>
        ///     固定周期天数
        /// </summary>
        [Description("固定周期天数")]
        public long PeriodDays { get; set; }

        #region 新增字段

        /// <summary>
        /// 资产子类型--抵押物类型（1--代表房贷，2--代表车贷）
        /// </summary>
        /// <value>The asset subtype.</value>
        public int AssetSubtype { get; set; }

        /// <summary>
        /// 债权转让协议
        /// </summary>
        /// <value>The credito transfer agreement.</value>
        public string CreditoTransferAgreement { get; set; }

        /// <summary>
        ///     担保方式
        /// </summary>
        /// <value>The guaranty style.</value>
        public string GuarantyStyle { get; set; }

        #endregion 新增字段
    }
}