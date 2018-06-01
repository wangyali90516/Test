using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRegularAssetToProduct
{
    /// <summary>
    ///     Class ReturnedMoneyBody.
    /// </summary>
    public class FinancierReturnedMoneyDetail : EntityBase<long>
    {
        //是否替换
        public bool IsReplaced { get; set; }
        //替换金额
        public long ReplacedAmount { get; set; }

        //剩余替换金额
        public long RemainingReplacedAmount { get; set; }
        //补贴
        public long SubsidyAmount { get; set; }

        /// <summary>
        /// 延迟天数
        /// </summary>
        /// <value>The subsidy amount.</value>
        public long LateDays { get; set; }
        /// <summary>
        ///     流程操作记录
        /// </summary>
        /// <value>The comment.</value>
        public string Comment { get; set; }

        public string FinancierReturnedMoneyDetailId { get; set; }

        public string FinancierReturnedMoneyId { get; set; }

        /// <summary>
        ///     操作流程状态
        /// </summary>
        /// <value>The flow status.</value>
        public string FlowStatus { get; set; }

        /// <summary>
        /// 还款状态
        /// </summary>
        public string Status { get; set; }

        #region 产品

        /// <summary>
        ///     实打金额
        /// </summary>
        [Description("实打金额")]
        public long ActualAmount { get; set; }

        /// <summary>
        ///     资产ID
        /// </summary>
        [Description("资产ID")]
        public string AssetId { get; set; }

        /// <summary>
        ///     本次回款
        /// </summary>
        public long CurrentActualAmount { get; set; }

        /// <summary>
        ///     累计回款金额
        /// </summary>
        [Description("累计回款金额")]
        public long TotalReturnedAmount { get; set; }

        /// <summary>
        ///     本次回款日期
        /// </summary>
        [Required(ErrorMessage = "回款日期必填")]
        public DateTime CurrnetReturnedMoneyDate { get; set; }

        /// <summary>
        ///     其他差额
        /// </summary>
        /// <value>The difference amount.</value>
        public long DiffAmount { get; set; }

        /// <summary>
        ///     产品Id
        /// </summary>
        [Description("产品Id")]
        public string ProductId { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; }

        /// <summary>
        ///     平台收款方
        /// </summary>
        [Required(ErrorMessage = "平台收款方必填")]
        public string ReceivablesPerson { get; set; }

        /// <summary>
        ///     应回金额
        /// </summary>
        [Description("应回金额")]
        public long ReturnedAmount { get; set; }

        /// <summary>
        ///     回款方式
        ///     10000  一次还清
        ///     20000  分期还款 
        /// </summary>
        //[Required(ErrorMessage = "回款方式必填")]
        public string ReturnedMoneyMethodCode { get; set; }

        /// <summary>
        ///     回款方式
        /// </summary>
        //[Required(ErrorMessage = "回款方式必填")]
        public string ReturnedMoneyMethodName { get; set; }

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
    }
}