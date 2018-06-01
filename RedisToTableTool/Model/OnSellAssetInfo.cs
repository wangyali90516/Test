using System;

namespace RedisToTableTool.Model
{
    public class OnSellAssetInfo
    {
        //OnSellAssetId,BillDueDate,CalculatedAmount,PresentValue

        public int AssetCategoryCode { get; set; }
        public DateTime BillDueDate { get; set; }
        public long CalculatedAmount { get; set; }
        public string OnSellAssetId { get; set; }
        public long PresentValue { get; set; }
    }
}