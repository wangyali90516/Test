using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRegularAssetToProduct
{
    /// <summary>
    ///     Class ReturnedMoneyBody.
    /// </summary>
    public class FinancierReturnedMoney : EntityBase<long>
    {
        /// <summary>
        /// Gets or sets the financier returned money identifier.
        /// </summary>
        /// <value>The financier returned money identifier.</value>
        public string FinancierReturnedMoneyId { get; set; }

        #region 产品

        /// <summary>
        ///     实打金额
        /// </summary>
        [Description("实打金额")]
        public long ActualAmount { get; set; }

        /// <summary>
        ///     资产所属类型
        /// </summary>
        /// <value>The asset category.</value>
        public int AssetCategoryCode { get; set; }

        /// <summary>
        ///     资产编号
        /// </summary>
        [Description("资产编号")]
        public string AssetCode { get; set; }

        /// <summary>
        ///     业务主键
        /// </summary>
        [Description("资产Id")]
        public string AssetId { get; set; }

        /// <summary>
        ///     资产名字
        /// </summary>
        [Description("资产名字")]
        public string AssetName { get; set; }

        /// <summary>
        ///     到期日期,精度到天
        /// </summary>
        /// <value>The bill due date.</value>
        public DateTime BillDueDate { get; set; }

        /// <summary>
        ///     票号
        /// </summary>
        [Description("票号")]
        public string BillNo { get; set; }

        /// <summary>
        ///     打款成功时间
        /// </summary>
        [Description("回款全部成功时间")]
        public DateTime? ResultTime { get; set; }

        /// <summary>
        ///     产品类型编号
        /// </summary>
        [Description("产品类型编号")]
        public long ProductCategoryCode { get; set; }

        /// <summary>
        ///     产品类型名称
        /// </summary>
        [Description("产品类型名称")]
        public string ProductCategoryName { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        /// <value>The product code.</value>
        public string ProductCode { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        [Description("产品类型编号")]
        public string ProductId { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        [Description("产品名称")]
        public string ProductName { get; set; }

        /// <summary>
        ///     融资金额
        /// </summary>
        [Description("融资金额")]
        public long ProductSumMoney { get; set; }

        /// <summary>
        ///     产品期数
        /// </summary>
        /// <value>The period number.</value>
        public int ProductIssueNo { get; set; }

        /// <summary>
        ///     应回金额
        /// </summary>
        [Description("应回金额")]
        public long ReturnedAmount { get; set; }


        /// <summary>
        ///     状态(实际为资产的状态)
        /// </summary>
        [Description("回款状态")]
        public string Status { get; set; }



        /// <summary>
        ///     累计回款金额
        /// </summary>
        [Description("累计回款金额")]
        public long TotalReturnedAmount { get; set; }

        #endregion 产品

        #region 融资企业

        /// <summary>
        ///     融资方Id
        /// </summary>
        [Description("融资方Id")]
        public string FinancierId { get; set; }

        /// <summary>
        ///     回款融资企业名称
        /// </summary>
        [Description("回款融资企业名称")]
        public string FinancierName { get; set; }

        #endregion 融资企业

        #region 回款

        /// <summary>
        ///     回款方式编号
        /// </summary>
        [Description("回款方式编号")]
        public string ReturnedMoneyMethodCode { get; set; }

        /// <summary>
        ///     回款方式名称
        /// </summary>
        [Description("回款方式名称")]
        public string ReturnedMoneyMethodName { get; set; }

        #endregion 回款

        #region 存管
        /// <summary>
        /// 平台Id
        /// </summary>
        /// <value>The platform identifier.</value>
        public string PlatformId { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        /// <value>The name of the platform.</value>
        public string PlatformName { get; set; }

        /// <summary>
        /// 平台佣金
        /// </summary>
        /// <value>The platform brokerage.</value>
        public long? PlatformBrokerage { get; set; }

        /// <summary>
        /// 平台运营可用金额
        /// </summary>
        /// <value>The platform avail amount.</value>
        public long? PlatformAvailAmount { get; set; }

        /// <summary>
        /// 还款企业Id
        /// </summary>
        /// <value>The repayment identifier.</value>
        public string RepaymentId { get; set; }

        /// <summary>
        /// 还款企业名称
        /// </summary>
        /// <value>The name of the repayment.</value>
        public string RepaymentName { get; set; }

        /// <summary>
        /// 还款企业可用金额
        /// </summary>
        /// <value>The repayment avail amount.</value>
        public long? RepaymentAvailAmount { get; set; }

        #endregion 存管
    }
}