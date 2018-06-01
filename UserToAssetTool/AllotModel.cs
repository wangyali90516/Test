using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserToAssetTool
{
    public class AllotModel
    {

        public AllotModel()
        {
            this.AddUserAssetRatiosDic = new Dictionary<string, List<UserAssetRatio>>();
            this.AddAssetUserRatiosDic = new Dictionary<string, List<UserAssetRatio>>();
            this.ModifyOnSellAssetsDic = new Dictionary<string, List<OnSellAssetDto>>();
            this.NewYemUserProductDtoList = new Dictionary<string, YemUserProductDto>();
        }

        public Dictionary<string, List<UserAssetRatio>> AddUserAssetRatiosDic { get; set; }

        public Dictionary<string, List<UserAssetRatio>> AddAssetUserRatiosDic { get; set; }

        public Dictionary<string, List<OnSellAssetDto>> ModifyOnSellAssetsDic { get; set; }

        public Dictionary<string, YemUserProductDto> NewYemUserProductDtoList { get; set; }
    }
}
