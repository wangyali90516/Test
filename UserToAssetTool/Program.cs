using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using UserToAssetTool.Mapping;

namespace UserToAssetTool
{
    /// <summary>
    ///     根据用户来循环分配资产，有可能一个用户分配很多资产
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            string searchAssetAzureTable = ConfigurationManager.AppSettings["SearchAssetAzureTable"];
            string searchYemUserProductAzureTable = ConfigurationManager.AppSettings["SearchYemUserProductAzureTable"];
            string writeAssetAzureTable = ConfigurationManager.AppSettings["WriteAssetAzureTable"];
            string writeYemUserProductAzureTable = ConfigurationManager.AppSettings["WriteYemUserProductAzureTable"];
            //string writeUserAssetRatioAzureTable = ConfigurationManager.AppSettings["WriteUserAssetRatioAzureTable"];
            //string writeAssetUserRatioAzureTable = ConfigurationManager.AppSettings["writeAssetUserRatioAzureTable"];
            Console.WriteLine("您当前操作的购买云Table的名称为:" + searchYemUserProductAzureTable);
            Console.WriteLine("您当前操作的资产云Table的名称为:" + searchAssetAzureTable);
            Console.WriteLine("写入购买云Table的名称为:" + writeYemUserProductAzureTable);
            Console.WriteLine("写入资产云Table的名称为:" + writeAssetAzureTable);
            //Console.WriteLine("写入比例云Table用户资产关系的名称为:" + writeUserAssetRatioAzureTable);
            //Console.WriteLine("写入比例云Table资产用户关系的名称为:" + writeAssetUserRatioAzureTable);

            Console.WriteLine("请选择您的操作:[");
            Console.WriteLine("       AllocateAsset");
            Console.WriteLine("       ResetPurchase");
            Console.WriteLine("       ResetOnSellAsset");
            Console.WriteLine("       ]");
            Console.WriteLine("请输入您的操作");
            string chance = Console.ReadLine();
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (chance == "AllocateAsset")
            {
                Console.WriteLine("您想处理某区间内的购买订单信息吗?《是》请输入y,《否》请输入n");
                string key = Console.ReadLine();
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (key == "y")
                {
                    string minAmount = ConfigurationManager.AppSettings["MinAmount"];
                    string maxAmount = ConfigurationManager.AppSettings["MaxAmount"];
                    string assetIds = ConfigurationManager.AppSettings["assetIds"];
                    string noIncludeAssetIds = ConfigurationManager.AppSettings["NoIncludeAssetIds"];
                    Console.WriteLine("您想处理的购买订单是在" + minAmount + "分---" + maxAmount + "分的购买订单吗？《是》请输入y,《否》请输入n");
                    key = Console.ReadLine();
                    if (key == "y")
                    {
                        Console.WriteLine("您想要排除某项资产吗？《是》请输入y,《否》请输入n");
                        string includeKey = Console.ReadLine();
                        if (includeKey == "y")
                        {
                            if (string.IsNullOrEmpty(noIncludeAssetIds))
                            {
                                Console.WriteLine("请配置您需要排除的资产Id");
                                return;
                            }

                            string[] noIncludeAssetIdList = noIncludeAssetIds.Split(',');
                            Console.WriteLine("请确定您要排除的资产Id信息:共" + noIncludeAssetIdList.Length + "条；《确定》请输入y,《不确定》请输入n");
                            foreach (var item in noIncludeAssetIdList)
                            {
                                Console.WriteLine(item);
                            }
                            string confirm = Console.ReadLine();
                            if (confirm == "y")
                            {
                                UserToAllocateAssetByNoIncludeAsync(noIncludeAssetIdList.ToArray(), long.Parse(minAmount), Convert.ToInt64(maxAmount), searchAssetAzureTable, searchYemUserProductAzureTable, writeAssetAzureTable, writeYemUserProductAzureTable).Wait();
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(assetIds))
                            {
                                Console.WriteLine("请配置您需要匹配的资产Id");
                                return;
                            }

                            string[] assetIdList = assetIds.Split(',');
                            Console.WriteLine("请确定您要配置的资产Id信息:共" + assetIdList.Length + "条；《确定》请输入y,《不确定》请输入n");
                            foreach (var item in assetIdList)
                            {
                                Console.WriteLine(item);
                            }
                            string confirm = Console.ReadLine();
                            if (confirm == "y")
                            {
                                UserToAllocateAssetAsync(assetIdList.ToArray(), long.Parse(minAmount), long.Parse(maxAmount), searchAssetAzureTable, searchYemUserProductAzureTable, writeAssetAzureTable, writeYemUserProductAzureTable).Wait();
                            }
                        }
                    }
                }
                else if (key == "n")
                {
                    UserToAllocateAssetAsync(new string[] { }, 0, 0, searchAssetAzureTable, searchYemUserProductAzureTable, writeAssetAzureTable, writeYemUserProductAzureTable).Wait();
                }
                Console.WriteLine("处理完成");
                Console.ReadKey();
            }
            else if (chance == "ResetPurchase")
            {
                DealResetYemUserProduct(searchYemUserProductAzureTable, writeYemUserProductAzureTable).Wait();
            }
            else if (chance == "ResetOnSellAsset")
            {
                DealResetOnSellAsset(searchAssetAzureTable, writeAssetAzureTable).Wait();
            }
        }

        #region 资产重设

        /// <summary>
        /// </summary>
        /// <param name="assetAzureTableName"></param>
        /// <param name="writeAssetAzureTable"></param>
        /// <returns></returns>
        private static async Task DealResetOnSellAsset(string assetAzureTableName, string writeAssetAzureTable)
        {
            Console.WriteLine("创建CloudTable对象");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(2.Seconds(), 6);
            tableClient.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
            tableClient.DefaultRequestOptions.ServerTimeout = 2.Minutes();

            CloudTable cloudOnSellAssetTable = tableClient.GetTableReference(assetAzureTableName);
            TableQuery<OnSellAssetDto> queryOnSellAsset = new TableQuery<OnSellAssetDto>()
                .Where("IsLock eq false");
            List<OnSellAssetDto> onSellAssetDtos = cloudOnSellAssetTable.ExecuteQuery(queryOnSellAsset).ToList();

            List<OnSellAssetDto> resetOnSellAssetDtos = new List<OnSellAssetDto>();
            foreach (var item in onSellAssetDtos)
            {
                var newItem = item;
                newItem.RemainderTotal = newItem.PresentValue;
                newItem.SellAmount = 0;
                newItem.Status = 20;
                newItem.UpdatedTime = DateTime.UtcNow.ToChinaStandardTime();
                newItem.ValueStatus = false;
                resetOnSellAssetDtos.Add(newItem);
            }
            await AddOnSellAsset(tableClient, resetOnSellAssetDtos, writeAssetAzureTable);
            Console.WriteLine("处理完成");
            Console.ReadKey();
        }

        #endregion 资产重设

        #region 重新设置购买订单

        private static async Task AddRangeYemUserPurchase(CloudTableClient tableClient, List<YemUserProductDto> yemUserProductDto, string writeYemUserProductAzureTable, int batchNum = 100)
        {
            if (yemUserProductDto != null)
            {
                CloudTable yemProductTable = tableClient.GetTableReference(writeYemUserProductAzureTable);

                int ratioCount = (int)Math.Ceiling((double)yemUserProductDto.Count / batchNum);
                for (int i = 0; i < ratioCount; i++)
                {
                    TableBatchOperation batch = new TableBatchOperation();
                    foreach (YemUserProductDto tableEntity in yemUserProductDto.Skip(batchNum * i).Take(batchNum))
                    {
                        batch.Add(TableOperation.InsertOrReplace(tableEntity));
                    }
                    await yemProductTable.ExecuteBatchAsync(batch);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="yemUserProductAzureTableName"></param>
        /// <param name="writeYemUserProductAzureTable"></param>
        /// <returns></returns>
        private static async Task DealResetYemUserProduct(string yemUserProductAzureTableName, string writeYemUserProductAzureTable)
        {
            string yemUserPurchase = ConfigurationManager.AppSettings["PurchasePartitionKey"];
            Console.WriteLine("创建CloudTable对象");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(2.Seconds(), 6);
            tableClient.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
            tableClient.DefaultRequestOptions.ServerTimeout = 2.Minutes();

            CloudTable cloudPurchaseOrderTable = tableClient.GetTableReference(yemUserProductAzureTableName);
            TableQuery<YemUserProductDto> queryPurchaseOrder = new TableQuery<YemUserProductDto>()
                .Where($"PartitionKey eq '{yemUserPurchase}'  and IsLock eq false");

            var yemUserProductDtos = cloudPurchaseOrderTable.ExecuteQuery(queryPurchaseOrder).OrderByDescending(p => p.RemainingAmount).ToList();

            List<YemUserProductDto> resetPurchaseList = new List<YemUserProductDto>();
            foreach (var item in yemUserProductDtos)
            {
                var newYemUserProductDto = item;
                newYemUserProductDto.RemainingAmount = newYemUserProductDto.PurchaseMoney;
                newYemUserProductDto.Allocated = 0;
                newYemUserProductDto.Status = 0;
                newYemUserProductDto.WaitingBankBackAmount = 0;
                newYemUserProductDto.UpdatedTime = DateTime.UtcNow.ToChinaStandardTime();
                resetPurchaseList.Add(newYemUserProductDto);
            }
            await AddRangeYemUserPurchase(tableClient, resetPurchaseList, writeYemUserProductAzureTable);

            Console.WriteLine("处理完成");
            Console.ReadLine();
        }

        #endregion 重新设置购买订单

        #region 资产分配逻辑

        public static async Task AddAssetUserRelation(CloudTableClient tableClient, List<UserAssetRatio> addAssetUserRatios, string writeAssetUserRatioAzureTable, int batchNum = 100)
        {
            if (addAssetUserRatios.Any())
            {
                CloudTable assetuserTable = tableClient.GetTableReference(writeAssetUserRatioAzureTable);
                var assetIds = addAssetUserRatios.Select(p => p.AssetId).ToList();
                foreach (var item in assetIds)
                {
                    var currentUserAsset = addAssetUserRatios.Where(p => p.AssetId == item).ToList();
                    if (currentUserAsset.Any())
                    {
                        int ratioCount = (int)Math.Ceiling((double)currentUserAsset.Count / batchNum);
                        for (int i = 0; i < ratioCount; i++)
                        {
                            TableBatchOperation batch = new TableBatchOperation();
                            foreach (UserAssetRatio tableEntity in currentUserAsset.Skip(batchNum * i).Take(batchNum))
                            {
                                batch.Add(TableOperation.InsertOrReplace(tableEntity));
                            }
                            await assetuserTable.ExecuteBatchAsync(batch);
                        }
                    }
                }
            }
        }

        public static async Task AddRedisAssetUserRelation(CloudTableClient tableClient, List<UserAssetRatio> addAssetUserRatios, string writeAssetUserRatioAzureTable, int batchNum = 100)
        {
            if (addAssetUserRatios.Any())
            {
                CloudTable assetuserTable = tableClient.GetTableReference(writeAssetUserRatioAzureTable);
                var assetIds = addAssetUserRatios.Select(p => p.AssetId).ToList();
                foreach (var item in assetIds)
                {
                    var currentUserAsset = addAssetUserRatios.Where(p => p.AssetId == item).ToList();
                    if (currentUserAsset.Any())
                    {
                        int ratioCount = (int)Math.Ceiling((double)currentUserAsset.Count / batchNum);
                        for (int i = 0; i < ratioCount; i++)
                        {
                            TableBatchOperation batch = new TableBatchOperation();
                            foreach (UserAssetRatio tableEntity in currentUserAsset.Skip(batchNum * i).Take(batchNum))
                            {
                                batch.Add(TableOperation.InsertOrReplace(tableEntity));
                            }
                            await assetuserTable.ExecuteBatchAsync(batch);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     更新用户比例信息
        /// </summary>
        /// <param name="tableClient"></param>
        /// <param name="yemUserProductDto"></param>
        /// <param name="addUserAssetRatios"></param>
        /// <param name="addAssetUserRatios"></param>
        /// <param name="modifyOnSellAssetDtos"></param>
        /// <param name="writeAssetAzureTable"></param>
        /// <param name="writeYemUserProductAzureTable"></param>
        /// <param name="currentDealPurchaseAmount"></param>
        /// <param name="lastOnSellAssetDtos"></param>
        /// <param name="batchNum"></param>
        /// InsertOrReplaceAzureTable
        public static async Task InsertOrReplaceAzureTable(CloudTableClient tableClient, YemUserProductDto yemUserProductDto, List<UserAssetRatio> addUserAssetRatios, List<UserAssetRatio> addAssetUserRatios, List<OnSellAssetDto> modifyOnSellAssetDtos, string writeAssetAzureTable, string writeYemUserProductAzureTable, long currentDealPurchaseAmount, List<OnSellAssetDto> lastOnSellAssetDtos, int batchNum = 100)
        {
            try
            {
                await AddOnSellAsset(tableClient, modifyOnSellAssetDtos, writeAssetAzureTable, batchNum);

                if (addUserAssetRatios.Any())
                {
                    await RedisHelper.SetRedisUserAssetRatioAsync(addUserAssetRatios);
                }
                if (addAssetUserRatios.Any())
                {
                    await RedisHelper.SetRedisAssetUserRatioAsync(addAssetUserRatios);
                }
                //购买
                await AddYemUserPurchase(tableClient, yemUserProductDto, writeYemUserProductAzureTable);
                int a = 0;
                int d = 10 / a;
            }
            catch (Exception e)
            {
                Console.WriteLine("出错了，正在还原数据");
                Logger.Logw("ErrorMessage", "订单号" + yemUserProductDto.OrderId + "  " + e.ToJson());
                //记日志
                await AddLogAsync(lastOnSellAssetDtos, addUserAssetRatios, addAssetUserRatios, yemUserProductDto, currentDealPurchaseAmount);
                //先还原
                // await RollerBackAsync(tableClient, writeAssetAzureTable, writeYemUserProductAzureTable, lastOnSellAssetDtos, addUserAssetRatios, addAssetUserRatios, yemUserProductDto, currentDealPurchaseAmount, batchNum);
                Console.WriteLine("还原数据完成");
                //记日志
                throw e;
            }
        }

        //购买订单还原
        public static async Task ResetPurchaseAsync(CloudTableClient tableClient, string writeYemUserProductAzureTable, YemUserProductDto yemUserProductDto, long currentDealPurchaseAmount)
        {
            var resetPurchase = yemUserProductDto.MapToYemUserProductDto();
            resetPurchase.RemainingAmount += currentDealPurchaseAmount;
            resetPurchase.Allocated -= currentDealPurchaseAmount;
            resetPurchase.Status = resetPurchase.PurchaseMoney == resetPurchase.Allocated ? PurchaseOrderStatus.AllocationComplete.ToEnumInteger() : PurchaseOrderStatus.AllocationOn.ToEnumInteger();
            resetPurchase.WaitingBankBackAmount = 0;
            resetPurchase.UpdatedTime = DateTime.UtcNow.ToChinaStandardTime();
            await AddYemUserPurchase(tableClient, yemUserProductDto, writeYemUserProductAzureTable);
        }

        /// <summary>
        ///     出错了之后，资产、订单、用户比例还原
        /// </summary>
        /// <param name="tableClient"></param>
        /// <param name="writeAssetAzureTable"></param>
        /// <param name="writeYemUserProductAzureTable"></param>
        /// <param name="lastOnSellAssetDtos"></param>
        /// <param name="addUserAssetRatios"></param>
        /// <param name="addAssetUserRatios"></param>
        /// <param name="yemUserProductDto"></param>
        /// <param name="currentDealPurchaseAmount"></param>
        /// <param name="batchNum"></param>
        /// <returns></returns>
        public static async Task RollerBackAsync(CloudTableClient tableClient, string writeAssetAzureTable, string writeYemUserProductAzureTable, List<OnSellAssetDto> lastOnSellAssetDtos, List<UserAssetRatio> addUserAssetRatios, List<UserAssetRatio> addAssetUserRatios, YemUserProductDto yemUserProductDto, long currentDealPurchaseAmount, int batchNum)
        {
            //资产还原
            await AddOnSellAsset(tableClient, lastOnSellAssetDtos, writeAssetAzureTable, batchNum); //lastOnSellAssetDtos为上一次的资产列表
            if (addUserAssetRatios.Any())
            {
                await RedisHelper.RemoveRedisUserAssetRatioAsync(addUserAssetRatios);
            }

            if (addUserAssetRatios.Any())
            {
                await RedisHelper.RemoveRedisAssetUserRatioAsync(addAssetUserRatios);
            }
            //订单还原
            await ResetPurchaseAsync(tableClient, writeYemUserProductAzureTable, yemUserProductDto, currentDealPurchaseAmount); //根据处理的金额还原
        }

        /// <summary>
        ///     记错误日志
        /// </summary>
        /// <param name="lastOnSellAssetDtos"></param>
        /// <param name="assetuserRatios"></param>
        /// <param name="yemUserProductDto"></param>
        /// <param name="currentDealPurchaseAmount"></param>
        /// <param name="userAssetRatios"></param>
        /// <returns></returns>
        private static async Task AddLogAsync(List<OnSellAssetDto> lastOnSellAssetDtos, List<UserAssetRatio> userAssetRatios, List<UserAssetRatio> assetuserRatios, YemUserProductDto yemUserProductDto, long currentDealPurchaseAmount)
        {
            ResetYemUserPurchase resetYemUserPurchase = new ResetYemUserPurchase();
            resetYemUserPurchase.OrderId = yemUserProductDto.OrderId;
            resetYemUserPurchase.YemUserProductDto = yemUserProductDto.MapToYemUserProductDto();
            resetYemUserPurchase.CurrentDealPurchaseAmount = currentDealPurchaseAmount;
            Logger.Logw("ErrorOldLastOnSellAsset", lastOnSellAssetDtos.ToJson());
            Logger.Logw("ErrorOldYemUserPurchase", resetYemUserPurchase.ToJson());
            Logger.Logw("ErrorOldUserAssetRatio", userAssetRatios.ToJson());
            Logger.Logw("ErrorOldAssetUserRatio", assetuserRatios.ToJson());

            //记Redis
            await RedisHelper.SetRedisOnSellAssetByOldAsync(lastOnSellAssetDtos);
            await RedisHelper.SetRedisUserAssetRatioByOldAsync(userAssetRatios);
            await RedisHelper.SetRedisAssetUserRatioByOldAsync(assetuserRatios);
            await RedisHelper.SetRedisYemUserPurchaseByOldAsync(resetYemUserPurchase);
        }

        /// <summary>
        ///     添加资产
        /// </summary>
        /// <param name="tableClient"></param>
        /// <param name="modifyOnSellAssetDtos"></param>
        /// <param name="writeAssetAzureTable"></param>
        /// <param name="batchNum"></param>
        /// <returns></returns>
        private static async Task AddOnSellAsset(CloudTableClient tableClient, List<OnSellAssetDto> modifyOnSellAssetDtos, string writeAssetAzureTable, int batchNum = 100)
        {
            CloudTable table = tableClient.GetTableReference(writeAssetAzureTable);

            //银票类资产
            var ypModifyOnSellAssetDtos = modifyOnSellAssetDtos.Where(p => p.PartitionKey == "YinPiao").ToList();
            if (ypModifyOnSellAssetDtos.Any())
            {
                int ypCount = (int)Math.Ceiling((double)ypModifyOnSellAssetDtos.Count / batchNum);
                for (int i = 0; i < ypCount; i++)
                {
                    TableBatchOperation batch = new TableBatchOperation();
                    foreach (OnSellAssetDto tableEntity in ypModifyOnSellAssetDtos.Skip(batchNum * i).Take(batchNum))
                    {
                        batch.Add(TableOperation.InsertOrReplace(tableEntity));
                    }
                    await table.ExecuteBatchAsync(batch);
                }
            }

            //商票类
            var spModifyOnSellAssetDtos = modifyOnSellAssetDtos.Where(p => p.PartitionKey == "ShangPiao").ToList();
            if (spModifyOnSellAssetDtos.Any())
            {
                int count = (int)Math.Ceiling((double)spModifyOnSellAssetDtos.Count / batchNum);
                for (int i = 0; i < count; i++)
                {
                    TableBatchOperation batch = new TableBatchOperation();
                    foreach (OnSellAssetDto tableEntity in spModifyOnSellAssetDtos.Skip(batchNum * i).Take(batchNum))
                    {
                        batch.Add(TableOperation.InsertOrReplace(tableEntity));
                    }
                    await table.ExecuteBatchAsync(batch);
                }
            }
        }

        private static async Task AddYemUserPurchase(CloudTableClient tableClient, YemUserProductDto yemUserProductDto, string writeYemUserProductAzureTable)
        {
            if (yemUserProductDto != null)
            {
                CloudTable yemProductTable = tableClient.GetTableReference(writeYemUserProductAzureTable);
                await yemProductTable.ExecuteAsync(TableOperation.InsertOrReplace(yemUserProductDto));
            }
        }

        /// <summary>
        ///     得到该用户分配该资产的金额
        /// </summary>
        /// <param name="purchaseMoney"></param>
        /// <param name="assetAmount"></param>
        /// <param name="sumAssetAmount"></param>
        /// <returns></returns>
        private static long AllocateAmountByAsset(long purchaseMoney, long assetAmount, double sumAssetAmount)
        {
            return (long)Math.Ceiling(purchaseMoney / sumAssetAmount * assetAmount);
        }

        private static OnSellAssetDto BuildOnSellAsset(OnSellAssetDto assetItem, long allocateAmount)
        {
            var newOnSellAsset = assetItem.MapToOnSellAsset();
            newOnSellAsset.ValueStatus = true;
            newOnSellAsset.UpdatedTime = DateTime.UtcNow.ToChinaStandardTime();
            if (newOnSellAsset.BillAccrualDate != null && newOnSellAsset.BillAccrualDate.Value.Date <= new DateTime(1900, 1, 1).Date)
            {
                newOnSellAsset.BillAccrualDate = DateTime.UtcNow.ToChinaStandardTime();
            }
            if (newOnSellAsset.GrowthTime != null && newOnSellAsset.GrowthTime.Value.Date <= new DateTime(1900, 1, 1).Date)
            {
                newOnSellAsset.GrowthTime = DateTime.UtcNow.ToChinaStandardTime(); //第一次增值时间
                newOnSellAsset.LastGrowthTime = newOnSellAsset.GrowthTime; //上一次增值时间
            }
            newOnSellAsset.YemBidIsReported = true;
            newOnSellAsset.SellAmount += allocateAmount;
            if (newOnSellAsset.RemainderTotal == 0)
            {
                if (newOnSellAsset.PresentValue == newOnSellAsset.SellAmount) //售卖金额等于融资金额
                {
                    newOnSellAsset.SellAmount = newOnSellAsset.CalculatedAmount;
                    newOnSellAsset.RemainderTotal = 0;
                    newOnSellAsset.RaiseStatus = true;
                    newOnSellAsset.Status = 15; //用户回款状态
                    newOnSellAsset.SellOutTime = DateTime.UtcNow.ToChinaStandardTime();
                }
            }
            return newOnSellAsset;
        }

        private static YemUserProductDto BuildYemUserProduct(YemUserProductDto yemUserProductDto, long allocateAmount)
        {
            var newYemUserProductDto = yemUserProductDto.MapToYemUserProductDto();
            newYemUserProductDto.Allocated += allocateAmount;
            newYemUserProductDto.Status = newYemUserProductDto.PurchaseMoney == newYemUserProductDto.Allocated ? PurchaseOrderStatus.AllocationComplete.ToEnumInteger() : PurchaseOrderStatus.AllocationOn.ToEnumInteger();
            newYemUserProductDto.WaitingBankBackAmount = 0;
            newYemUserProductDto.UpdatedTime = DateTime.UtcNow.ToChinaStandardTime();
            return newYemUserProductDto;
        }

        /// <summary>
        ///     创建比例关系
        /// </summary>
        /// <param name="item"></param>
        /// <param name="onSellAssetDto"></param>
        /// <param name="allocateAmount"></param>
        /// <returns></returns>
        private static Tuple<UserAssetRatio, UserAssetRatio> CreateUserAssetRatio(YemUserProductDto item, OnSellAssetDto onSellAssetDto, long allocateAmount)
        {
            UserAssetRatio userAssetRatio = new UserAssetRatio
            {
                AssetCategoryCode = onSellAssetDto.AssetCategoryCode,
                AssetId = onSellAssetDto.OnSellAssetId,
                BillDueDate = onSellAssetDto.BillDueDate,
                Capital = allocateAmount,
                Cellphone = item.Cellphone,
                CredentialNo = item.CredentialNo,
                Denominator = onSellAssetDto.PresentValue,
                IsInvestSuccess = true, //用户资产已报备
                IsNotifyTradingSuccess = true, //已通知交易系统
                IsReturned = false,
                NotifyTradingRespInfo = "Mock",
                NotifyTradingTime = DateTime.UtcNow.ToChinaStandardTime(),
                Numerator = allocateAmount,
                OrderId = item.OrderId,
                OrderTime = item.OrderTime,
                OriginalUserAssetRatioId = Guid.NewGuid().ToGuidString(),
                PurchaseMoney = item.PurchaseMoney,
                Reserve = "1", //放款成功
                Status = 4, //用户资产报备成功
                UserId = item.UserId,
                UserName = item.UserName,
                UserPresentValue = 0,
                CreatedBy = "System",
                CreatedTime = DateTime.UtcNow.ToChinaStandardTime(),
                UpdatedBy = "System",
                UpdatedTime = DateTime.UtcNow.ToChinaStandardTime(),
                PartitionKey = item.UserId,
                ETag = DateTime.UtcNow.ToChinaStandardTime().UnixTimestamp().ToString()
            };
            userAssetRatio.UserAssetRatioId = userAssetRatio.OriginalUserAssetRatioId;
            userAssetRatio.RowKey = userAssetRatio.AssetId + "_" + userAssetRatio.UserAssetRatioId;

            //资产用户关系
            var assetUserRatio = MapToUserAssetRatio(userAssetRatio);
            return Tuple.Create(userAssetRatio, assetUserRatio);
        }

        private static async Task DealUserToAsset(long minAmount, long maxAmount, string yemUserProductAzureTableName, string writeAssetAzureTable, string writeYemUserProductAzureTable,
            CloudTableClient tableClient, string yemUserPurchase, List<OnSellAssetDto> onSellAssetDtos)
        {
            CloudTable cloudPurchaseOrderTable = tableClient.GetTableReference(yemUserProductAzureTableName);
            TableQuery<YemUserProductDto> queryPurchaseOrder;
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (maxAmount > 0)
            {
                //queryPurchaseOrder = new TableQuery<YemUserProductDto>()
                //    .Where($"PartitionKey eq '{yemUserPurchase}' and  RemainingAmount ge {minAmount} and RemainingAmount lt {maxAmount} and IsLock eq false");
                string startsWithCondition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, yemUserPurchase),
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForLong("RemainingAmount", QueryComparisons.GreaterThanOrEqual, minAmount));

                string filterCondition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterConditionForLong("RemainingAmount", QueryComparisons.LessThan, maxAmount),
                    TableOperators.And,
                    startsWithCondition
                );
                string endCondition = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterConditionForBool("IsLock", QueryComparisons.Equal, false),
                    TableOperators.And,
                    filterCondition
                );
                queryPurchaseOrder = new TableQuery<YemUserProductDto>().Where(endCondition);
            }
            else
            {
                queryPurchaseOrder = new TableQuery<YemUserProductDto>()
                    .Where($"PartitionKey eq '{yemUserPurchase}' and RemainingAmount gt 0 and IsLock eq false");
            }
            var yemUserProductDtos = cloudPurchaseOrderTable.ExecuteQuery(queryPurchaseOrder).OrderByDescending(p => p.RemainingAmount).ToList();
            yemUserProductDtos = yemUserProductDtos.Where(p => p.RemainingAmount > 0).ToList();
            Console.WriteLine("要处理的购买订单条数=" + yemUserProductDtos.Count());
            Console.WriteLine("待分配的资产数=" + onSellAssetDtos.Count());
            List<OnSellAssetDto> oldOnSellAssetDtos = onSellAssetDtos;
            if (!oldOnSellAssetDtos.Any())
            {
                Console.WriteLine("没有处理的资产");
                return;
            }
            int index = 0;
            //int syncIndex = 0; //备份索引
            Stopwatch s = new Stopwatch();
            s.Start();
            List<OnSellAssetDto> lastOnSellAssetDtos = new List<OnSellAssetDto>(); //上一次的资产情况
            foreach (var item in yemUserProductDtos)
            {
                if (!lastOnSellAssetDtos.Any()) //第一次
                {
                    lastOnSellAssetDtos.AddRange(oldOnSellAssetDtos.MapToOnSellAssetList());
                }
                index++;
                var addUserAssetRatios = new List<UserAssetRatio>(); //记录每笔购买产生的比例  用户资产关系
                List<OnSellAssetDto> modifyOnSellAssets = new List<OnSellAssetDto>(); //记录每笔购买使用的资产
                var addAssetUserRatios = new List<UserAssetRatio>(); //记录每笔购买产生的比例  资产用户关系
                YemUserProductDto newYemUserProductDto = null;
                Console.WriteLine("总共处理的条数为：" + yemUserProductDtos.Count + ",当前处理的条数：" + index);
                Console.WriteLine("还需要处理的资产数=" + oldOnSellAssetDtos.Count());
                long sumAssetCount = oldOnSellAssetDtos.Sum(p => p.RemainderTotal);
                long waitingDealAmount = item.RemainingAmount; //购买订单待处理的金额
                long currentDealPurchaseAmount = 0; //本次处理的金额
                for (int i = oldOnSellAssetDtos.Count - 1; i >= 0; i--)
                {
                    if (item.RemainingAmount == 0)
                    {
                        break;
                    }
                    var assetItem = oldOnSellAssetDtos[i];
                    long allocateAmount = AllocateAmountByAsset(waitingDealAmount, assetItem.RemainderTotal, sumAssetCount);
                    if (item.RemainingAmount < allocateAmount)
                    {
                        allocateAmount = item.RemainingAmount;
                    }
                    if (assetItem.RemainderTotal < allocateAmount)
                    {
                        allocateAmount = assetItem.RemainderTotal;
                    }
                    if (allocateAmount <= 0)
                    {
                        continue;
                    }
                    item.RemainingAmount -= allocateAmount;
                    currentDealPurchaseAmount += allocateAmount; //统计该订单分配的总金额
                    assetItem.RemainderTotal -= allocateAmount;
                    var userAssetRatioTuple = CreateUserAssetRatio(item, assetItem, allocateAmount); //第一个是用户资产关系  第二个是资产用户关系
                    addUserAssetRatios.Add(userAssetRatioTuple.Item1);
                    addAssetUserRatios.Add(userAssetRatioTuple.Item2);
                    var updateOnSellAsset = BuildOnSellAsset(assetItem, allocateAmount); //修改资产的信息
                    modifyOnSellAssets.Add(updateOnSellAsset);
                    if (assetItem.RemainderTotal == 0)
                    {
                        oldOnSellAssetDtos.Remove(assetItem);
                    }
                }
                newYemUserProductDto = BuildYemUserProduct(item, currentDealPurchaseAmount); //修改购买订单需要配置的参数
                //保存数据库
                await InsertOrReplaceAzureTable(tableClient, newYemUserProductDto, addUserAssetRatios, addAssetUserRatios,
                    modifyOnSellAssets, writeAssetAzureTable, writeYemUserProductAzureTable, currentDealPurchaseAmount, lastOnSellAssetDtos);
                lastOnSellAssetDtos = new List<OnSellAssetDto>();
                lastOnSellAssetDtos.AddRange(modifyOnSellAssets.MapToOnSellAssetList()); //记录上一次资产列表

                if (!oldOnSellAssetDtos.Any())
                {
                    Console.WriteLine("没有处理的资产");
                    break;
                }
            }
            s.Stop();
            Console.WriteLine("End:" + s.ElapsedMilliseconds);
        }

        private static UserAssetRatio MapToUserAssetRatio(UserAssetRatio oldUserAssetRatio)
        {
            UserAssetRatio newAssetUserRatio = new UserAssetRatio
            {
                AssetCategoryCode = oldUserAssetRatio.AssetCategoryCode,
                AssetId = oldUserAssetRatio.AssetId,
                BillDueDate = oldUserAssetRatio.BillDueDate,
                Capital = oldUserAssetRatio.Capital,
                Cellphone = oldUserAssetRatio.Cellphone,
                CredentialNo = oldUserAssetRatio.CredentialNo,
                Denominator = oldUserAssetRatio.Denominator,
                IsInvestSuccess = oldUserAssetRatio.IsInvestSuccess, //用户资产已报备
                IsNotifyTradingSuccess = oldUserAssetRatio.IsNotifyTradingSuccess, //已通知交易系统
                IsReturned = oldUserAssetRatio.IsReturned,
                NotifyTradingRespInfo = oldUserAssetRatio.NotifyTradingRespInfo,
                NotifyTradingTime = oldUserAssetRatio.NotifyTradingTime,
                Numerator = oldUserAssetRatio.Numerator,
                OrderId = oldUserAssetRatio.OrderId,
                OrderTime = oldUserAssetRatio.OrderTime,
                OriginalUserAssetRatioId = oldUserAssetRatio.OriginalUserAssetRatioId,
                PurchaseMoney = oldUserAssetRatio.PurchaseMoney,
                Reserve = oldUserAssetRatio.Reserve, //放款成功
                Status = oldUserAssetRatio.Status, //用户资产报备成功
                UserId = oldUserAssetRatio.UserId,
                UserName = oldUserAssetRatio.UserName,
                UserPresentValue = oldUserAssetRatio.UserPresentValue,
                CreatedBy = oldUserAssetRatio.CreatedBy,
                CreatedTime = oldUserAssetRatio.CreatedTime,
                UpdatedBy = oldUserAssetRatio.UpdatedBy,
                UpdatedTime = oldUserAssetRatio.UpdatedTime,
                PartitionKey = oldUserAssetRatio.AssetId,
                ETag = DateTime.UtcNow.ToChinaStandardTime().UnixTimestamp().ToString()
            };
            newAssetUserRatio.UserAssetRatioId = oldUserAssetRatio.OriginalUserAssetRatioId;
            newAssetUserRatio.RowKey = oldUserAssetRatio.UserId + "_" + oldUserAssetRatio.UserAssetRatioId;
            return newAssetUserRatio;
        }

        /// <summary>
        ///     获取需要分配的资产、订单
        /// </summary>
        /// <returns></returns>
        private static async Task UserToAllocateAssetAsync(string[] assetIds, long minAmount, long maxAmount, string assetAzureTableName, string yemUserProductAzureTableName, string writeAssetAzureTable, string writeYemUserProductAzureTable)
        {
            string yemUserPurchase = ConfigurationManager.AppSettings["PurchasePartitionKey"];
            Console.WriteLine("创建CloudTable对象");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(2.Seconds(), 6);
            tableClient.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
            tableClient.DefaultRequestOptions.ServerTimeout = 2.Minutes();
            CloudTable cloudOnSellAssetTable = tableClient.GetTableReference(assetAzureTableName);

            //查询所有资产信息
            TableQuery<OnSellAssetDto> queryOnSellAsset = new TableQuery<OnSellAssetDto>()
                .Where("RemainderTotal gt 0 and IsLock eq false");
            List<OnSellAssetDto> onSellAssetDtos = assetIds.Any() ? cloudOnSellAssetTable.ExecuteQuery(queryOnSellAsset).Where(p => assetIds.Contains(p.OnSellAssetId)).ToList() : cloudOnSellAssetTable.ExecuteQuery(queryOnSellAsset).OrderBy(p => p.RemainderTotal).ToList();

            //获取购买信息
            await DealUserToAsset(minAmount, maxAmount, yemUserProductAzureTableName, writeAssetAzureTable, writeYemUserProductAzureTable, tableClient, yemUserPurchase, onSellAssetDtos);
        }

        /// <summary>
        ///     获取需要分配的资产、订单，排除资产
        /// </summary>
        /// <returns></returns>
        private static async Task UserToAllocateAssetByNoIncludeAsync(string[] noIncludeAssetIds, long minAmount, long maxAmount, string assetAzureTableName,
            string yemUserProductAzureTableName, string writeAssetAzureTable,
            string writeYemUserProductAzureTable)
        {
            string yemUserPurchase = ConfigurationManager.AppSettings["PurchasePartitionKey"];
            Console.WriteLine("创建CloudTable对象");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(2.Seconds(), 6);
            tableClient.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
            tableClient.DefaultRequestOptions.ServerTimeout = 2.Minutes();
            CloudTable cloudOnSellAssetTable = tableClient.GetTableReference(assetAzureTableName);

            //查询所有资产信息
            TableQuery<OnSellAssetDto> queryOnSellAsset = new TableQuery<OnSellAssetDto>()
                .Where("RemainderTotal gt 0 and IsLock eq false");
            List<OnSellAssetDto> onSellAssetDtos = noIncludeAssetIds.Any() ? cloudOnSellAssetTable.ExecuteQuery(queryOnSellAsset).Where(p => !noIncludeAssetIds.Contains(p.OnSellAssetId)).ToList() : cloudOnSellAssetTable.ExecuteQuery(queryOnSellAsset).OrderBy(p => p.RemainderTotal).ToList();

            //获取购买信息
            await DealUserToAsset(minAmount, maxAmount, yemUserProductAzureTableName, writeAssetAzureTable, writeYemUserProductAzureTable, tableClient, yemUserPurchase, onSellAssetDtos);
        }

        #endregion 资产分配逻辑
    }
}