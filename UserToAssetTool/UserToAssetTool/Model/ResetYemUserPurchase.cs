using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserToAssetTool
{
    public class ResetYemUserPurchase
    {
        /// <summary>
        /// 订单
        /// </summary>
        public YemUserProductDto YemUserProductDto { get; set; }

        /// <summary>
        /// 本次处理金额
        /// </summary>
        public long CurrentDealPurchaseAmount { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }
    }
}
