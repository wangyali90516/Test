using J.Base.Lib;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace JymTool
{
    class Program
    {

        static void Main(string[] args)
        {
            //Stopwatch st = new Stopwatch();
            //st.Start();
            //CAsset().GetAwaiter().GetResult();
            //st.Stop();
            //Console.WriteLine("Time:" + st.ElapsedMilliseconds);
            //Console.ReadKey();

            AddDataToBlobAsync().GetAwaiter().GetResult();

            Console.ReadKey();
        }

        /// <summary>
        /// 将获取到的数据写到Blob中
        /// </summary>
        /// <returns></returns>
        private static async Task AddDataToBlobAsync()
        {
            Console.WriteLine("处理大额购买用户");
            string minPurchaseAmount = ConfigurationManager.AppSettings["MinPurchaseAmount"];

            double stoYprecent;
            if (!double.TryParse(ConfigurationManager.AppSettings["StoYprecent"], out stoYprecent))
            {
                Console.WriteLine("商票占银票的百分比格式不正确");
                return;
            }
            int i;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["BlobIndex"], out i))
            {
                Console.WriteLine("配置信息中的BlobIndex请输入数字");
                return;
            }
            Console.WriteLine("当前处理大额用户从" + minPurchaseAmount + "分(不包含)到最大值的购买订单用户");
            Console.WriteLine("银票占商票的百分比为:" + stoYprecent);
            Console.WriteLine("Blob从:" + i+"开始处理");
            Console.WriteLine("请确定上面信息是否正确，如果正确，请输入y,否则输入n");
            string key = Console.ReadLine();
            if (key != "y")
            {
                return;
            }
            List<AllocationTransfer> allocationTransfers = await GetWaitingAssetAndPurchaseOrderAsync(minPurchaseAmount, stoYprecent);
            CloudBlobContainer blobContainer = await BlobStorageProvider.InitAsync();
           
            int count = 0;
            foreach (var allocationTransferItem in allocationTransfers)
            {
                if (allocationTransferItem.YemUserProductDtos.Any())
                {
                    await BlobStorageProvider.WriteStateToBlob(blobContainer, "JinYinMao.Business.Assets.Grains.Actors.SyncAssetPoolActor/" + i, allocationTransferItem.ToJson());
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
        private static async Task<List<AllocationTransfer>> GetWaitingAssetAndPurchaseOrderAsync(string maxPurchaseAmount,double stoYprecent)
        {
            string yemUserPurchase = ConfigurationManager.AppSettings["PurchasePartitionKey"];
            string noContainAssetId = ConfigurationManager.AppSettings["NoContainAssetId"];
            Console.WriteLine("创建CloudTable对象");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(2.Seconds(), 6);
            tableClient.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
            tableClient.DefaultRequestOptions.ServerTimeout = 2.Minutes();

            CloudTable cloudOnSellAssetTable = tableClient.GetTableReference("AssetOnSellAssets");

            //获取待分配资产
            TableQuery<OnSellAssetDto> queryOnSellShangPiaoAsset = new TableQuery<OnSellAssetDto>()
                .Where("PartitionKey eq 'ShangPiao' and RemainderTotal gt 0 and IsLock eq false");
            //根据剩余金额排序
            var onShangPiaoSellAssetDtos = cloudOnSellAssetTable.ExecuteQuery(queryOnSellShangPiaoAsset).OrderByDescending(p => p.RemainderTotal).ToList();

            Console.WriteLine("ShangPiao:" + onShangPiaoSellAssetDtos.Count());

            TableQuery<OnSellAssetDto> queryOnSellYinPiaoAsset = new TableQuery<OnSellAssetDto>()
                .Where($"PartitionKey eq 'YinPiao' and RemainderTotal gt 0 and IsLock eq false and OnSellAssetId ne '{noContainAssetId}'");
            //根据剩余金额排序
            var onYinPiaoSellAssetDtos = cloudOnSellAssetTable.ExecuteQuery(queryOnSellYinPiaoAsset).OrderByDescending(p => p.RemainderTotal)
               .ToList();

            //获取购买信息
            CloudTable cloudPurchaseOrderTable = tableClient.GetTableReference("AssetYEMUserProducts");
            TableQuery<YemUserProductDto> queryPurchaseOrder = new TableQuery<YemUserProductDto>()
                .Where($"PartitionKey eq '{yemUserPurchase}' and RemainingAmount gt {maxPurchaseAmount} and IsLock eq false");

            //根据剩余金额排序,因为购买在循环的时候。是倒序的，所以这里如果是倒序的，则循环的时候则是从小到大
            var yemUserProductDtos = cloudPurchaseOrderTable.ExecuteQuery(queryPurchaseOrder).OrderByDescending(p => p.RemainingAmount).ToList();

            List<AllocationTransfer> allocationTransfers = new List<AllocationTransfer>();

            if (onShangPiaoSellAssetDtos.Any())
            {
                //处理商票
                foreach (var shangPiaoItem in onShangPiaoSellAssetDtos)
                {
                    var remainderTotal = shangPiaoItem.RemainderTotal;

                    AllocationTransfer allocationTransfer = new AllocationTransfer();

                    var yinPiaoDealAmount = shangPiaoItem.RemainderTotal * stoYprecent; //计算银票应该匹配的金额
                    allocationTransfer.OnShangPiaoSellAssetDtos.Add(new AllocationOnSellAssetDto
                    {
                        OnSellAssetId = shangPiaoItem.OnSellAssetId,
                        RemainderTotal = shangPiaoItem.RemainderTotal
                    });
                    var yinPiaoRemainderTotal = 0L;

                    //找银票
                    for (int i = onYinPiaoSellAssetDtos.Count - 1; i >= 0; i--)
                    {
                        var yinPiaoItem = onYinPiaoSellAssetDtos[i];
                        yinPiaoRemainderTotal += yinPiaoItem.RemainderTotal;

                        allocationTransfer.OnYinPiaoSellAssetDtos.Add(new AllocationOnSellAssetDto
                        {
                            OnSellAssetId = yinPiaoItem.OnSellAssetId,
                            RemainderTotal = yinPiaoItem.RemainderTotal
                        });

                        remainderTotal += yinPiaoItem.RemainderTotal;
                        onYinPiaoSellAssetDtos.Remove(yinPiaoItem);
                        if (yinPiaoRemainderTotal >= yinPiaoDealAmount)
                        {
                            break;
                        }
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
            }
            else
            {
                //处理银票
                foreach (var yinPiaoItem in onYinPiaoSellAssetDtos)
                {
                    var remainderTotal = yinPiaoItem.RemainderTotal;

                    AllocationTransfer allocationTransfer = new AllocationTransfer();
                    allocationTransfer.OnYinPiaoSellAssetDtos.Add(new AllocationOnSellAssetDto
                    {
                        OnSellAssetId = yinPiaoItem.OnSellAssetId,
                        RemainderTotal = yinPiaoItem.RemainderTotal
                    });

                    //找购买
                    var userTotal = 0L;
                    foreach (var yemUserProductItem in Enumerable.Reverse(yemUserProductDtos))
                    {
                        userTotal += yemUserProductItem.RemainingAmount;
                        allocationTransfer.YemUserProductDtos.Add(new AllocationYemUserProduct
                        {
                            OrderId = yemUserProductItem.OrderId,
                            RemainingAmount = yemUserProductItem.RemainingAmount,
                            UserId = yemUserProductItem.UserId,
                            WaitingBankBackAmount = yemUserProductItem.WaitingBankBackAmount
                        });
                        yemUserProductDtos.Remove(yemUserProductItem);
                        if (remainderTotal <= userTotal)
                        {
                            break;
                        }
                    }
                    allocationTransfers.Add(allocationTransfer);
                }
            }

            return await Task.FromResult(allocationTransfers);
        }
    }

}
