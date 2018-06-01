using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToTableTool.Model
{
    public class LoanMoneyRequest
    {
        /// <summary>
        ///     标的ID
        /// </summary>
        /// <value>The asset identifier.</value>
        public string AssetId { get; set; }

        /// <summary>
        ///     受托人银行卡号（isEntrustedPay =1时必填）
        /// </summary>
        /// <value>The bank card no.</value>
        public string BankCardNO { get; set; }

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
        ///     分润收款用户，网贷平台唯一的用户编码
        /// </summary>
        /// <value>The receive user identifier.</value>
        public string ReceiveUserId { get; set; }

        public long ShareProfit { get; set; }

        /// <summary>
        ///     分润账户
        /// </summary>
        /// <value>The share profit account.</value>
        public string ShareProfitAccount { get; set; }

        public string UpdatedBy { get; set; }

        /// <summary>
        ///     实打金额
        /// </summary>
        /// <value>The user order sum money.</value>
        public string UserOrderSumMoney { get; set; }

        /// <summary>
        ///     分润金额
        /// </summary>
        /// <value>The share profit.</value>
        /// <summary>
        ///     修改人
        /// </summary>
        /// <value>The updated by.</value>
    }
}