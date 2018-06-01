using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserToAssetTool.Mapping
{
    public static class OnSellAssetMapping
    {
        public static OnSellAssetDto MapToOnSellAsset(this OnSellAssetDto onSellAssetDto)
        {
            return new OnSellAssetDto
            {
                AssetCategoryCode = onSellAssetDto.AssetCategoryCode,
                AssetCode = onSellAssetDto.AssetCode,
                AssetName = onSellAssetDto.AssetName,
                AssetProductCategory = onSellAssetDto.AssetProductCategory,
                BillAccrualDate = onSellAssetDto.BillAccrualDate,
                PartitionKey = onSellAssetDto.PartitionKey,
                RowKey = onSellAssetDto.RowKey,
                ETag=onSellAssetDto.ETag,
                Timestamp=onSellAssetDto.Timestamp,
                BillDueDate = onSellAssetDto.BillDueDate,
                BillMoney = onSellAssetDto.BillMoney,
                BillNo = onSellAssetDto.BillNo,
                BillRate = onSellAssetDto.BillRate,
                CalculatedAmount = onSellAssetDto.CalculatedAmount,
                EffectiveValue = onSellAssetDto.EffectiveValue,
                EnterpriseRate = onSellAssetDto.EnterpriseRate,
                FinancierId = onSellAssetDto.FinancierId,
                FinancierName = onSellAssetDto.FinancierName,
                GrowthTime = onSellAssetDto.GrowthTime,
                IsLock = onSellAssetDto.IsLock,
                IsNewStatus = onSellAssetDto.IsNewStatus,
                LastGrowthTime = onSellAssetDto.LastGrowthTime,
                OnSellAssetId = onSellAssetDto.OnSellAssetId,
                PresentValue = onSellAssetDto.PresentValue,
                PriorityLevel = onSellAssetDto.PriorityLevel,
                RaiseStatus = onSellAssetDto.RaiseStatus,
                RemainderTotal = onSellAssetDto.RemainderTotal,
                SellAmount = onSellAssetDto.SellAmount,
                SellOutTime = onSellAssetDto.SellOutTime,
                Status = onSellAssetDto.Status,
                ValueStatus = onSellAssetDto.ValueStatus,
                YemBidIsReported = onSellAssetDto.YemBidIsReported,
                CreatedBy = onSellAssetDto.CreatedBy,
                CreatedTime = onSellAssetDto.CreatedTime,
                IsDeleted = onSellAssetDto.IsDeleted,
                UpdatedBy = onSellAssetDto.UpdatedBy,
                UpdatedTime = onSellAssetDto.UpdatedTime
            };
        }

        public static List<OnSellAssetDto> MapToOnSellAssetList(this List<OnSellAssetDto> onSellAssetDtos)
        {
			List<OnSellAssetDto> newOnSellAssetDtos=new List<OnSellAssetDto>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var item in onSellAssetDtos)
            {
               newOnSellAssetDtos.Add(item.MapToOnSellAsset());
            }
            return newOnSellAssetDtos;
        }
    }
}
