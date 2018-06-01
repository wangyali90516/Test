using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Moe.Lib;

namespace SyncReadBlobTool
{
    class Program
    {
        static void Main(string[] args)
        {
            string blobAddress = System.Configuration.ConfigurationManager.AppSettings["blobAddress"];
          
            Console.WriteLine("您想处理的Blob名称为：" + blobAddress + "");
            double yinPiaoPrecent;
            if (!double.TryParse(System.Configuration.ConfigurationManager.AppSettings["YinPiaoPrecent"], out yinPiaoPrecent))
            {
                Console.WriteLine("银票在资产分配时的比例请输入数字");
                return;
            }
            Console.WriteLine("银票在资产分配时的比例为：" + yinPiaoPrecent + "");
            Console.WriteLine("请确定信息，是否继续，y/n");
            string key = Console.ReadLine();
            if (key != "y")
            {
                return;
            }
            Console.WriteLine("请输入您要处理的blob的区间");

            Console.WriteLine("最小区间(包含)");
            string minBlobKey = Console.ReadLine();
            int minBlobName;
            if (!int.TryParse(minBlobKey, out minBlobName))
            {
                Console.WriteLine("最小区间请输入数字");
            }

            Console.WriteLine("最大区间(不包含）");
            string maxBlobKey = Console.ReadLine();
            int maxBlobName;
            if (!int.TryParse(maxBlobKey, out maxBlobName))
            {
                Console.WriteLine("最大区间请输入数字");
            }
            if (minBlobName >= 0 && maxBlobName >= 0)
            {
                List<Task> taskList = new List<Task>();
                // ReSharper disable once LoopCanBeConvertedToQuery
                for (int i = minBlobName; i < maxBlobName; i++)
                {
                    taskList.Add(GetBlobDataAsync(i + "", blobAddress, yinPiaoPrecent));
                }
                Task.WhenAll(taskList).ContinueWith(p => Console.WriteLine("处理完成"));
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 获取Blob中的数据
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="blobAddress"></param>
        /// <param name="yinPiaoPrecent"></param>
        /// <returns></returns>
        private static async Task<bool> GetBlobDataAsync(string blobName, string blobAddress,double yinPiaoPrecent)
        {
            CloudBlobContainer blobContainer = await BlobStorageProvider.InitAsync();
            //获取blob中的数据
            string content = await BlobStorageProvider.GetBlobConent(blobAddress + blobName, blobContainer);
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }
            AllocationTransfer allocationTransfer = content.FromJson<AllocationTransfer>();
            return await SendToAllcationAsset(allocationTransfer.YemUserProductDtos, allocationTransfer.OnYinPiaoSellAssetDtos, allocationTransfer.OnShangPiaoSellAssetDtos,yinPiaoPrecent);
        }
        private static int perCount;

        /// <summary>
        /// 调用接口信息
        /// </summary>
        /// <param name="yemUserProduct"></param>
        /// <param name="onSellYinPiaoAssets"></param>
        /// <param name="onSellShangPiaoAssets"></param>
        /// <param name="yinPiaoPrecent"></param>
        /// <returns></returns>
        private static async Task<bool> SendToAllcationAsset(List<AllocationYemUserProduct> yemUserProduct, List<AllocationOnSellAssetDto> onSellYinPiaoAssets, List<AllocationOnSellAssetDto> onSellShangPiaoAssets,double yinPiaoPrecent)
        {
            //调用资产分配接口
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ServiceFabricUrl"]);
            client.Timeout = TimeSpan.FromHours(3);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = client.PostAsJsonAsync("/Inner/MessageFromBusiness/TransferDataForAllocation", new AllocationTransfer
            {
                OnShangPiaoSellAssetDtos = onSellShangPiaoAssets,
                OnYinPiaoSellAssetDtos = onSellYinPiaoAssets,
                YemUserProductDtos = yemUserProduct,
                YinPiaoPrecent= yinPiaoPrecent
            }).GetAwaiter().GetResult();
            var result = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine($"Per:{++perCount};Time:" + result);
                return true;
            }
            return false;
        }
    }
}
