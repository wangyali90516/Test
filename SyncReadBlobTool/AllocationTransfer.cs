

using System.Collections.Generic;

namespace SyncReadBlobTool
{
    public class AllocationTransfer
    {
        public AllocationTransfer()
        {
            YemUserProductDtos = new List<AllocationYemUserProduct>();
            OnYinPiaoSellAssetDtos = new List<AllocationOnSellAssetDto>();
            OnShangPiaoSellAssetDtos = new List<AllocationOnSellAssetDto>();
        }
        public List<AllocationYemUserProduct> YemUserProductDtos { get; set; }
        /// <summary>
        /// 资产分配时，银票所分配的比例
        /// </summary>
        public double YinPiaoPrecent { get; set; }
        public List<AllocationOnSellAssetDto> OnYinPiaoSellAssetDtos { get; set; }
        public List<AllocationOnSellAssetDto> OnShangPiaoSellAssetDtos { get; set; }
    }
}