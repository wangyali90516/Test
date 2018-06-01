using System.Collections.Generic;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;

namespace RedisToTableTool.Model
{
    public class AssetDebtRebatesModel : TableEntity
    {
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public string RebateType { get; set; }
        public string RedemptionOrderId { get; set; }
        public string Status { get; set; }
        public string SubOrderList { get; set; }

        public List<SubOrder> subOrderListArray
        {
            get { return string.IsNullOrEmpty(this.SubOrderList) ? new List<SubOrder>() : this.SubOrderList.FromJson<List<SubOrder>>(); }
        }

        public string UserId { get; set; }
    }

    public class SubOrder
    {
        public long Amount { get; set; }
        public string BizType { get; set; }
        public string SubOrderId { get; set; }
    }
}