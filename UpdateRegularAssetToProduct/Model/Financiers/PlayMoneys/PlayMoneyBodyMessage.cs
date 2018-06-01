using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRegularAssetToProduct
{
    /// <summary>
    ///     打款支付方式
    /// </summary>
    public class PlayMoneyBodyMessage : EntityBase<long>
    {
        public string PlayMoneyBodyMessageId { get; set; }

        #region 支付方式

        /// <summary>
        ///     资产类别
        /// </summary>
        /// <value>The asset category code.</value>
        public int AssetCategoryCode { get; set; }

        /// <summary>
        ///     资产编号
        /// </summary>
        /// <value>The asset code.</value>
        public string AssetCode { get; set; }

        /// <summary>
        ///     Gets or sets the asset identifier.
        /// </summary>
        /// <value>The asset identifier.</value>
        public string AssetId { get; set; }

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
        [StringLength(14)]
        public string SequenceNo { get; set; }

        /// <summary>
        ///     订单交易流水号
        /// </summary>
        [StringLength(32)]
        public string TransactionIdentifier { get; set; }

        /// <summary>
        ///     用户编号
        /// </summary>
        [StringLength(32)]
        public string UserIdentifier { get; set; }

        #endregion 支付方式
    }
}
