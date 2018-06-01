using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncReadBlobTool
{
    public class AllocationYemUserProduct
    {
        /// <summary>
        ///     订单ID.
        /// </summary>
        [Description("订单ID")]
        public string OrderId { get; set; }

        /// <summary>
        ///     剩余金额
        /// </summary>
        [Description("剩余金额")]
        public long RemainingAmount { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 等待银行返回的结果
        /// </summary>
        public long WaitingBankBackAmount { get; set; }
    }
}
