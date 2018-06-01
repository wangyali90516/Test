using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserToAssetTool.Mapping
{
    public static class YemUserPurchaseMapping
    {
        public static YemUserProductDto MapToYemUserProductDto(this YemUserProductDto yemUserProductDto)
        {
            return new YemUserProductDto
            {
                Allocated = yemUserProductDto.Allocated,
                Cellphone = yemUserProductDto.Cellphone,
                CredentialNo = yemUserProductDto.CredentialNo,
                InvestSuccessAmount = yemUserProductDto.InvestSuccessAmount,
                IsLock = yemUserProductDto.IsLock,
                PartitionKey = yemUserProductDto.PartitionKey,
                RowKey = yemUserProductDto.RowKey,
                ETag = yemUserProductDto.ETag,
                Timestamp = yemUserProductDto.Timestamp,
                IsSendTrade = yemUserProductDto.IsSendTrade,
                OrderBatchBookInvestInfos = yemUserProductDto.OrderBatchBookInvestInfos,
                OrderId = yemUserProductDto.OrderId,
                OrderInvestStatus = yemUserProductDto.OrderInvestStatus,
                OrderTime = yemUserProductDto.OrderTime,
                ProductId = yemUserProductDto.ProductId,
                ProductIdentifier = yemUserProductDto.ProductIdentifier,
                PurchaseMoney = yemUserProductDto.PurchaseMoney,
                WaitingBankBackAmount = yemUserProductDto.WaitingBankBackAmount,
                RemainingAmount = yemUserProductDto.RemainingAmount,
                SequenceNo = yemUserProductDto.SequenceNo,
                UserId = yemUserProductDto.UserId,
                UserName = yemUserProductDto.UserName,
                Status = yemUserProductDto.Status,
                CreatedBy = yemUserProductDto.CreatedBy,
                CreatedTime = yemUserProductDto.CreatedTime,
                IsDeleted = yemUserProductDto.IsDeleted,
                UpdatedBy = yemUserProductDto.UpdatedBy,
                UpdatedTime = yemUserProductDto.UpdatedTime
            };
        }
    }
}
