using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRegularAssetToProduct
{
    /// <summary>
    ///     企贷信息
    /// </summary>
    public class EnterpriseLoan : EntityBase<long>
    {
        /// <summary>
        ///     企貸資產ID
        /// </summary>
        /// <value>The enterprise loan identifier.</value>
        [StringLength(32)]
        public string EnterpriseLoanId { get; set; }

        /// <summary>
        ///     融资方介绍
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public string FinancierIntroduction { get; set; }

        /// <summary>
        ///     用户显示融资方全称(必填----从融资方中获取)
        /// </summary>
        [Required]
        public string FinancierNameOfUser { get; set; }


        /// <summary>
        /// 担保方名称
        /// </summary>
        /// <value>The full name of the guarantee.</value>
        public string GuaranteeFullName { get; set; }

        /// <summary>
        ///     担保方介绍
        /// </summary>
        /// <value>The guarantee introduction.</value>
        [DataType(DataType.Text)]
        public string GuaranteeIntroduction { get; set; }

        /// <summary>
        ///     担保方类型编码担保类型 (担保类型 10、银行保兑 20、央企担保 30、国企担保 40、国有担保公司担保 50、担保公司担保 60、上市集团担保 70、集团担保 80、国资参股担保公司担保 90、银行担保 100、金银猫全程监管资金流向)
        /// </summary>
        /// <value>The guarantee type code.</value>
        public string GuaranteeTypeCode { get; set; }

        ///// <summary>
        /////     担保方类型ID
        ///// </summary>
        ///// <value>The guarantee type identifier.</value>
        //public string GuaranteeTypeId { get; set; }

        /// <summary>
        ///     担保方类型名称担保类型 (担保类型 10、银行保兑 20、央企担保 30、国企担保 40、国有担保公司担保 50、担保公司担保 60、上市集团担保 70、集团担保 80、国资参股担保公司担保 90、银行担保 100、金银猫全程监管资金流向)
        /// </summary>
        /// <value>The name of the guarantee type.</value>
        public string GuaranteeTypeName { get; set; }

        /// <summary>
        ///     固定周期天数
        /// </summary>
        /// <value>The investment horizon.</value>
        public long PeriodDays { get; set; }

        #region 新增字段

        /// <summary>
        ///     保兑证明
        /// </summary>
        /// <value>The ensure exchange.</value>
        public string EnsureExchange { get; set; }

        /// <summary>
        ///     担保函（必填）
        /// </summary>
        /// <value>The guarantee.</value>
        [Required(ErrorMessage = " 担保函（必填）")]
        public string Guarantee { get; set; }

        /// <summary>
        ///     融资用途
        /// </summary>
        /// <value>The financier purpose.</value>
        [Required(ErrorMessage = "资产用途（必填）")]
        public string FinancierPurpose { get; set; }

        #endregion 新增字段
    }
}