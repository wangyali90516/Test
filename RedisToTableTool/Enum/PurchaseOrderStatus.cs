using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserToAssetTool
{
    [Description("购买订单状态")]
    public enum PurchaseOrderStatus
    {
        [Description("待申购")]
        WaitingForAllocation = 2,

        [Description("申购中")]
        AllocationOn = 0,

        [Description("申购完成")]
        AllocationComplete = 1,

        [Description("已更新钱包")]
        IsModifyWallet = -2,

        [Description("异常订单")]
        ExceptionalOrders = -1,

        [Description("取消订单")]
        CancelOrders = -3
    }

    public static class PurchaseOrderStatusExt
    {
        public static int ToEnumInteger(this PurchaseOrderStatus purchaseOrderStatus)
        {
            return Convert.ToInt32(purchaseOrderStatus);
        }

        public static string ToEnumString(this PurchaseOrderStatus purchaseOrderStatus)
        {
            return Convert.ToInt32(purchaseOrderStatus).ToString();
        }
    }
}
