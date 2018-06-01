using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J.Base.Lib;
using JymTool;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;

namespace SmallJYMTool
{
    class Program
    {
        static void Main(string[] args)
        {
            AddDataToBlobAsync().GetAwaiter().GetResult();

            Console.ReadKey();
        }
        /// <summary>
        /// 将获取到的数据写到Blob中
        /// </summary>
        /// <returns></returns>
        private static async Task AddDataToBlobAsync()
        {
            Console.WriteLine("处理小额购买用户");
            string maxPurchaseAmount = ConfigurationManager.AppSettings["MaxPurchaseAmount"];
            Console.WriteLine("当前处理小额为0-" + maxPurchaseAmount + "(包含)分的购买订单用户");
            int i;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["BlobIndex"], out i))
            {
                Console.WriteLine("配置信息中的BlobIndex请输入数字");
                return;
            }
            Console.WriteLine("BlobIndex从" + i + "开始");
            Console.WriteLine("请确定信息，是否继续? y/ n");
            string key = Console.ReadLine();
            if (key != "y")
            {
                return;
            }
            List<AllocationTransfer> allocationTransfers = await GetWaitingAssetAndPurchaseOrderAsync(maxPurchaseAmount);
            CloudBlobContainer blobContainer = await BlobStorageProvider.InitAsync();

            int count = 0;
            foreach (var allocationTransferItem in allocationTransfers)
            {
                if (allocationTransferItem.YemUserProductDtos.Any())
                {
                    await BlobStorageProvider.WriteStateToBlob(blobContainer, "JinYinMao.Business.Assets.Grains.Actors.SmallSyncAssetPoolActor/" + i, allocationTransferItem.ToJson());
                    count++;
                    i++;
                }
            }
            Console.WriteLine("总共处理条数:" + count);
        }
        /// <summary>
        /// 获取需要分配的资产、订单
        /// </summary>
        /// <returns></returns>
        private static async Task<List<AllocationTransfer>> GetWaitingAssetAndPurchaseOrderAsync(string maxPurchaseAmount)
        {
            string yemUserPurchase = ConfigurationManager.AppSettings["PurchasePartitionKey"];
            string assetId = ConfigurationManager.AppSettings["AssetId"];
            Console.WriteLine("创建CloudTable对象");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(2.Seconds(), 6);
            tableClient.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
            tableClient.DefaultRequestOptions.ServerTimeout = 2.Minutes();

            CloudTable cloudOnSellAssetTable = tableClient.GetTableReference("AssetOnSellAssets");

            TableQuery<OnSellAssetDto> queryOnSellYinPiaoAsset = new TableQuery<OnSellAssetDto>()
                .Where($"PartitionKey eq 'YinPiao' and RemainderTotal gt 0 and OnSellAssetId eq '{assetId}' and IsLock eq false");
            //根据剩余金额排序
            var onYinPiaoSellAssetDtos = cloudOnSellAssetTable.ExecuteQuery(queryOnSellYinPiaoAsset).OrderByDescending(p => p.RemainderTotal)
                .ToList();

            //获取购买信息
            CloudTable cloudPurchaseOrderTable = tableClient.GetTableReference("AssetYEMUserProducts");
            TableQuery<YemUserProductDto> queryPurchaseOrder = new TableQuery<YemUserProductDto>()
                .Where($"PartitionKey eq '{yemUserPurchase}' and RemainingAmount gt 0 and RemainingAmount le {maxPurchaseAmount} and IsLock eq false");

            //根据剩余金额排序,因为购买在循环的时候。是倒序的，所以这里如果是倒序的，则循环的时候则是从小到大
            var yemUserProductDtos = cloudPurchaseOrderTable.ExecuteQuery(queryPurchaseOrder).OrderByDescending(p => p.RemainingAmount).ToList();

            List<AllocationTransfer> allocationTransfers = new List<AllocationTransfer>();

            if (onYinPiaoSellAssetDtos.Any())
            {
                AllocationTransfer allocationTransfer = new AllocationTransfer();
                var remainderTotal = 0L;
                //处理银票类资产
                OnSellAssetDto onSellAssetDto = onYinPiaoSellAssetDtos.FirstOrDefault();
                if (onSellAssetDto != null)
                {
                    allocationTransfer.OnYinPiaoSellAssetDtos.Add(new AllocationOnSellAssetDto
                    {
                        OnSellAssetId = onSellAssetDto.OnSellAssetId,
                        RemainderTotal = onSellAssetDto.RemainderTotal
                    });
                    remainderTotal = onSellAssetDto.RemainderTotal; //银票的待处理金额
                }
                //找购买
                var userTotal = 0L;
                for (int i = yemUserProductDtos.Count - 1; i >= 0; i--)
                {
                    var yemUserProductItem = yemUserProductDtos[i];
                    userTotal += yemUserProductItem.RemainingAmount;

                    allocationTransfer.YemUserProductDtos.Add(new AllocationYemUserProduct
                    {
                        OrderId = yemUserProductItem.OrderId,
                        UserId = yemUserProductItem.UserId,
                        WaitingBankBackAmount = yemUserProductItem.WaitingBankBackAmount,
                        RemainingAmount = yemUserProductItem.RemainingAmount
                    });
                    yemUserProductDtos.Remove(yemUserProductItem);
                    if (remainderTotal <= userTotal)
                    {
                        break;
                    }
                }
                allocationTransfers.Add(allocationTransfer);
            }

            return await Task.FromResult(allocationTransfers);

        }
    }
}
