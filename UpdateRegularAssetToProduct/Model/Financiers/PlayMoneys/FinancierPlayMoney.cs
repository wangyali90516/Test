using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRegularAssetToProduct
{
    /// <summary>
    ///     企业打款信息
    /// </summary>
    public class FinancierPlayMoney : EntityBase<long>
    {
        /// <summary>
        ///     业务主键
        /// </summary>
        public string FinancierPlayMoneyId { get; set; }

        #region 新字段

        /// <summary>
        ///     Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args { get; set; }

        #endregion 新字段

        #region 产品

        /// <summary>
        ///     产品类型编号
        /// </summary>
        public long ProductCategoryCode { get; set; }

        /// <summary>
        ///     产品类型名称
        /// </summary>
        public string ProductCategoryName { get; set; }

        /// <summary>
        ///     产品编号
        /// </summary>
        /// <value>The product code.</value>
        public string ProductCode { get; set; }

        /// <summary>
        ///     Gets or sets the product identifier.
        /// </summary>
        /// <value>The product identifier.</value>
        public string ProductId { get; set; }

        /// <summary>
        ///     产品期数
        /// </summary>
        /// <value>The period number.</value>
        public int ProductIssueNo { get; set; }

        /// <summary>
        ///     产品名称
        /// </summary>
        /// <value>The name of the product.</value>
        public string ProductName { get; set; }

        /// <summary>
        ///     募集结束时间
        /// </summary>
        [Column(TypeName = "DateTime2")]
        public DateTime? ProductRaiseEndDateTime { get; set; }

        /// <summary>
        ///     募集开始时间
        /// </summary>
        [Column(TypeName = "DateTime2")]
        public DateTime ProductRaiseStartDateTime { get; set; }

        /// <summary>
        ///     产品售卖金额(实际为所有用户的订单总金额)
        /// </summary>
        /// <value>The product sell money.</value>
        public long ProductSellMoney { get; set; }

        /// <summary>
        ///     产品总金额（必填----就是融资金额）
        /// </summary>
        /// <value>The product sum money.</value>
        public long ProductSumMoney { get; set; }

        /// <summary>
        ///     产品年化率
        /// </summary>
        /// <value>The product yield.</value>
        public long ProductYield { get; set; }

        #endregion 产品

        #region 资产

        /// <summary>
        ///     资产所属类型
        /// </summary>
        /// <value>The asset category.</value>
        public int AssetCategoryCode { get; set; }

        /// <summary>
        ///     资产编号
        /// </summary>
        public string AssetCode { get; set; }

        /// <summary>
        ///     资产创建时间
        /// </summary>
        /// <value>The asset date time.</value>
        [Column(TypeName = "DateTime2")]
        public DateTime? AssetDateTime { get; set; }

        /// <summary>
        ///     业务主键
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        ///     资产名字
        /// </summary>
        /// <value>The name.</value>
        public string AssetName { get; set; }

        /// <summary>
        ///     资产所属产品类别
        /// </summary>
        /// <value>The type of the asset owner.</value>
        public string AssetProductCategory { get; set; }

        /// <summary>
        ///     到期日期,精度到天
        /// </summary>
        /// <value>The bill due date.</value>
        [Column(TypeName = "DateTime2")]
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
        ///     已经计算过后的融资金额(实际融资金额)
        /// </summary>
        /// <value>The calculated amount.</value>
        public long CalculatedAmount { get; set; }

        /// <summary>
        ///     流程中的批注信息
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///     最终打款时间
        /// </summary>
        /// <value>The result time.</value>
        public DateTime? ResultTime { get; set; }

        /// <summary>
        ///     状态(实际为资产的状态)
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }

        #endregion 资产

        #region 融资企业

        /// <summary>
        ///     融资方Id
        /// </summary>
        /// <value>The Financier identifier.</value>
        public string FinancierId { get; set; }

        /// <summary>
        ///     融资企业名称
        /// </summary>
        /// <value>The name of the enterprise.</value>
        public string FinancierName { get; set; }

        /// <summary>
        ///     收款方账户
        /// </summary>
        public string ReceivablesAccount { get; set; }

        /// <summary>
        ///     银行名称
        /// </summary>
        public string ReceivablesBankName { get; set; }

        /// <summary>
        ///     联系人姓名
        /// </summary>
        public string ReceivablesContactName { get; set; }

        /// <summary>
        ///     联系方式
        /// </summary>
        /// <value>The receivables contact way.</value>
        public string ReceivablesContactWay { get; set; }

        /// <summary>
        ///     开户行所在地
        /// </summary>
        public string ReceivablesOpenBankAddr { get; set; }

        /// <summary>
        ///     收款方全称
        /// </summary>
        public string ReceivingSideFullName { get; set; }

        #endregion 融资企业

        #region 支付方式

        /// <summary>
        ///     批次号
        /// </summary>
        public string BatchCode { get; set; }

        /// <summary>
        ///     支付方式编号
        /// </summary>
        public string PaymentMethodCode { get; set; }

        /// <summary>
        ///     支付方式名称
        /// </summary>
        public string PaymentMethodName { get; set; }

        /// <summary>
        ///     账户交易流水号
        /// </summary>
        public string SequenceNo { get; set; }

        /// <summary>
        ///     订单交易流水号
        /// </summary>
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        public string UserIdentifier { get; set; }

        /// <summary>
        ///     用户订单累计金额(实打金额)
        /// </summary>
        /// <value>The user order sum money.</value>
        public long UserOrderSumMoney { get; set; }

        #endregion 支付方式

        #region 存管

        /// <summary>
        ///     受托人银行卡号（isEntrustedPay =1时必填）
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNo { get; set; }

        /// <summary>
        ///     户名（isEntrustedPay =1 时必填）
        /// </summary>
        /// <value>The name of the card.</value>
        public string CardName { get; set; }

        /// <summary>
        ///     受托支付标识（默认 0）
        ///     0- 否
        ///     1- 是
        /// </summary>
        /// <value>The is entrusted pay.</value>
        public string IsEntrustedPay { get; set; }

        /// <summary>
        ///     放款原单号（与银行交互的流水号）
        /// </summary>
        /// <value>The loan original order identifier.</value>
        public string LoanOriginalOrderId { get; set; }

        /// <summary>
        ///     分润金额
        /// </summary>
        /// <value>The share profit.</value>
        public long ShareProfit { get; set; }

        /// <summary>
        ///     分润账户
        /// </summary>
        /// <value>The share profit account.</value>
        public string ShareProfitAccount { get; set; }

        /// <summary>
        ///     分润账户ID
        /// </summary>
        /// <value>The share profit account identifier.</value>
        public string ShareProfitAccountId { get; set; }

        #endregion 存管
    }
}