

using System.Collections.Generic;

namespace JymTool
{
    public class AllocationTransfer
    {
        public AllocationTransfer()
        {
            YemUserProductDtos = new List<AllocationYemUserProduct>();
            OnYinPiaoSellAssetDtos=new List<AllocationOnSellAssetDto>();
            OnShangPiaoSellAssetDtos = new List<AllocationOnSellAssetDto>();
        }
        public List<AllocationYemUserProduct> YemUserProductDtos { get; set; }
        public List<AllocationOnSellAssetDto> OnYinPiaoSellAssetDtos { get; set; }
        public List<AllocationOnSellAssetDto> OnShangPiaoSellAssetDtos { get; set; }
    }

    public class AllocationTransfer2
    {
        public AllocationTransfer2()
        {
            YemUserProductDtos = new List<YemUserProductDto>();
            OnYinPiaoSellAssetDtos = new List<OnSellAssetDto>();
            OnShangPiaoSellAssetDtos = new List<OnSellAssetDto>();
        }
        public List<YemUserProductDto> YemUserProductDtos { get; set; }
        public List<OnSellAssetDto> OnYinPiaoSellAssetDtos { get; set; }
        public List<OnSellAssetDto> OnShangPiaoSellAssetDtos { get; set; }
    }
}