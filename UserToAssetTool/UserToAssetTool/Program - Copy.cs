using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;

namespace UserToAssetTool
{
    /// <summary>
    /// 根据用户来循环分配资产，有可能一个用户分配很多资产
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string guid = Guid.Empty.ToGuidString();
            string searchAssetAzureTable = ConfigurationManager.AppSettings["SearchAssetAzureTable"];
            string searchYemUserProductAzureTable = ConfigurationManager.AppSettings["SearchYemUserProductAzureTable"];

            string writeAssetAzureTable = ConfigurationManager.AppSettings["WriteAssetAzureTable"];

            string writeYemUserProductAzureTable = ConfigurationManager.AppSettings["WriteYemUserProductAzureTable"];
            string writeUserAssetRatioAzureTable = ConfigurationManager.AppSettings["WriteUserAssetRatioAzureTable"];
            string writeAssetUserRatioAzureTable = ConfigurationManager.AppSettings["writeAssetUserRatioAzureTable"];
            Console.WriteLine("您当前操作的购买云Table的名称为:" + searchYemUserProductAzureTable);
            Console.WriteLine("您当前操作的资产云Table的名称为:" + searchAssetAzureTable);

            Console.WriteLine("写入购买云Table的名称为:" + writeYemUserProductAzureTable);
            Console.WriteLine("写入资产云Table的名称为:" + writeAssetAzureTable);
            Console.WriteLine("写入比例云Table用户资产关系的名称为:" + writeUserAssetRatioAzureTable);
            Console.WriteLine("写入比例云Table资产用户关系的名称为:" + writeAssetUserRatioAzureTable);

            Console.WriteLine("您想处理某区间内的购买订单信息吗?《是》请输入y,《否》请输入n");

            string key = Console.ReadLine();
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
                            UserToAllocateAssetByNoIncludeAsync(noIncludeAssetIdList.ToArray(), long.Parse(minAmount), Convert.ToInt64(maxAmount), searchAssetAzureTable, searchYemUserProductAzureTable, writeAssetAzureTable, writeYemUserProductAzureTable, writeUserAssetRatioAzureTable, writeAssetUserRatioAzureTable).Wait();
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
                            UserToAllocateAssetAsync(assetIdList.ToArray(), long.Parse(minAmount), long.Parse(maxAmount), searchAssetAzureTable, searchYemUserProductAzureTable, writeAssetAzureTable, writeYemUserProductAzureTable, writeUserAssetRatioAzureTable, writeAssetUserRatioAzureTable).Wait();
                        }
                    }


                }

            }
            else if (key == "n")
            {
                UserToAllocateAssetAsync(new string[] { }, 0, 0, searchAssetAzureTable, searchYemUserProductAzureTable, writeAssetAzureTable, writeYemUserProductAzureTable, writeUserAssetRatioAzureTable, writeAssetUserRatioAzureTable).Wait();
            }
            Console.WriteLine("处理完成");
            Console.ReadKey();
        }

        /// <summary>
        /// 获取需要分配的资产、订单
        /// </summary>
        /// <returns></returns>
        private static async Task UserToAllocateAssetAsync(string[] assetIds, long minAmount, long maxAmount, string assetAzureTableName, string yemUserProductAzureTableName, string writeAssetAzureTable, string writeYemUserProductAzureTable, string writeUserAssetRatioAzureTable, string writeAssetUserRatioAzureTable)
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
            List<OnSellAssetDto> onSellAssetDtos = assetIds.Any() ?
                cloudOnSellAssetTable.ExecuteQuery(queryOnSellAsset).Where(p => assetIds.Contains(p.OnSellAssetId)).ToList() : cloudOnSellAssetTable.ExecuteQuery(queryOnSellAsset).OrderBy(p => p.RemainderTotal).ToList();

            //获取购买信息
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

            Console.WriteLine("要处理的购买订单条数=" + yemUserProductDtos.Count());
            Console.WriteLine("待分配的资产数=" + onSellAssetDtos.Count());
            List<OnSellAssetDto> oldOnSellAssetDtos = onSellAssetDtos;
            if (!oldOnSellAssetDtos.Any())
            {
                Console.WriteLine("没有处理的资产");
                return;
            }
            int index = 0;
            foreach (var item in yemUserProductDtos)
            {
                index++;
                var addUserAssetRatios = new List<UserAssetRatio>();//记录每笔购买产生的比例  用户资产关系
                var addAssetUserRatios = new List<UserAssetRatio>();//记录每笔购买产生的比例  资产用户关系
                List<OnSellAssetDto> modifyOnSellAssets = new List<OnSellAssetDto>(); //记录每笔购买使用的资产
                YemUserProductDto newYemUserProductDto = null;
                Console.WriteLine("总共处理的条数为：" + yemUserProductDtos.Count + ",当前处理的条数：" + index);
                Console.WriteLine("还需要处理的资产数=" + oldOnSellAssetDtos.Count());
                long sumAssetCount = oldOnSellAssetDtos.Sum(p => p.RemainderTotal);
                long waitingDealAmount = item.RemainingAmount; //购买订单待处理的金额
                for (int i = oldOnSellAssetDtos.Count - 1; i >= 0; i--)
                {
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
                    if (allocateAmount <= 0) { continue; }
                    var userAssetRatioTuple = CreateUserAssetRatio(item, assetItem, allocateAmount); //第一个是用户资产关系  第二个是资产用户关系
                    addUserAssetRatios.Add(userAssetRatioTuple.Item1);
                    addAssetUserRatios.Add(userAssetRatioTuple.Item2);
                    var updateOnSellAsset = BuildOnSellAsset(assetItem, allocateAmount); //修改资产的信息
                    modifyOnSellAssets.Add(updateOnSellAsset);
                    newYemUserProductDto = BuildYemUserProduct(item, allocateAmount); //修改购买订单需要配置的参数
                    assetItem.RemainderTotal -= allocateAmount;
                    if (assetItem.RemainderTotal == 0)
                    {
                        oldOnSellAssetDtos.Remove(assetItem);
                    }
                }
                //保存数据库
                await InsertOrReplaceAzureTable(tableClient, newYemUserProductDto, addUserAssetRatios, addAssetUserRatios, modifyOnSellAssets, writeAssetAzureTable, writeYemUserProductAzureTable, writeUserAssetRatioAzureTable, writeAssetUserRatioAzureTable);

                if (!oldOnSellAssetDtos.Any())
                {
                    Console.WriteLine("没有处理的资产");
                    break;
                }
            }
        }
        /// <summary>
        /// 获取需要分配的资产、订单，排除资产
        /// </summary>
        /// <returns></returns>
        private static async Task UserToAllocateAssetByNoIncludeAsync(string[] noIncludeAssetIds, long minAmount, long maxAmount, string assetAzureTableName,
            string yemUserProductAzureTableName, string writeAssetAzureTable,
            string writeYemUserProductAzureTable, string writeUserAssetRatioAzureTable, string writeAssetUserRatioAzureTable)
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
            List<OnSellAssetDto> onSellAssetDtos = noIncludeAssetIds.Any() ?
                cloudOnSellAssetTable.ExecuteQuery(queryOnSellAsset).Where(p => !noIncludeAssetIds.Contains(p.OnSellAssetId)).ToList() : cloudOnSellAssetTable.ExecuteQuery(queryOnSellAsset).OrderBy(p => p.RemainderTotal).ToList();

            //获取购买信息
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

            Console.WriteLine("要处理的购买订单条数=" + yemUserProductDtos.Count());
            Console.WriteLine("待分配的资产数=" + onSellAssetDtos.Count());
            List<OnSellAssetDto> oldOnSellAssetDtos = onSellAssetDtos;
            if (!oldOnSellAssetDtos.Any())
            {
                Console.WriteLine("没有处理的资产");
                return;
            }
            int index = 0;
            foreach (var item in yemUserProductDtos)
            {
                index++;
                var addUserAssetRatios = new List<UserAssetRatio>();//记录每笔购买产生的比例  用户资产关系
                var addAssetUserRatios = new List<UserAssetRatio>();//记录每笔购买产生的比例  资产用户关系
                List<OnSellAssetDto> modifyOnSellAssets = new List<OnSellAssetDto>(); //记录每笔购买使用的资产
                YemUserProductDto newYemUserProductDto = null;
                Console.WriteLine("总共处理的条数为：" + yemUserProductDtos.Count + ",当前处理的条数：" + index);
                Console.WriteLine("还需要处理的资产数=" + oldOnSellAssetDtos.Count());
                long sumAssetCount = oldOnSellAssetDtos.Sum(p => p.RemainderTotal);
                long waitingDealAmount = item.RemainingAmount; //购买订单待处理的金额
                for (int i = oldOnSellAssetDtos.Count - 1; i >= 0; i--)
                {
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
                    if (allocateAmount <= 0) { continue; }
                    var userAssetRatioTuple = CreateUserAssetRatio(item, assetItem, allocateAmount); //第一个是用户资产关系  第二个是资产用户关系
                    addUserAssetRatios.Add(userAssetRatioTuple.Item1);
                    addAssetUserRatios.Add(userAssetRatioTuple.Item2);
                    var updateOnSellAsset = BuildOnSellAsset(assetItem, allocateAmount); //修改资产的信息
                    modifyOnSellAssets.Add(updateOnSellAsset);
                    newYemUserProductDto = BuildYemUserProduct(item, allocateAmount); //修改购买订单需要配置的参数
                    assetItem.RemainderTotal -= allocateAmount;
                    if (assetItem.RemainderTotal == 0)
                    {
                        oldOnSellAssetDtos.Remove(assetItem);
                    }
                }
                //保存数据库
                await InsertOrReplaceAzureTable(tableClient, newYemUserProductDto, addUserAssetRatios, addAssetUserRatios, modifyOnSellAssets, writeAssetAzureTable, writeYemUserProductAzureTable, writeUserAssetRatioAzureTable, writeAssetUserRatioAzureTable);

                if (!oldOnSellAssetDtos.Any())
                {
                    Console.WriteLine("没有处理的资产");
                    break;
                }
            }
        }

        private static YemUserProductDto BuildYemUserProduct(YemUserProductDto yemUserProductDto, long allocateAmount)
        {
            var newYemUserProductDto = yemUserProductDto;
            newYemUserProductDto.RemainingAmount -= allocateAmount;
            newYemUserProductDto.Allocated += allocateAmount;
            newYemUserProductDto.Status = newYemUserProductDto.PurchaseMoney == newYemUserProductDto.Allocated ? PurchaseOrderStatus.AllocationComplete.ToEnumInteger() : PurchaseOrderStatus.AllocationOn.ToEnumInteger();
            newYemUserProductDto.WaitingBankBackAmount = 0;
            newYemUserProductDto.UpdatedTime = DateTime.UtcNow.ToChinaStandardTime();
            return newYemUserProductDto;
        }

        private static OnSellAssetDto BuildOnSellAsset(OnSellAssetDto assetItem, long allocateAmount)
        {
            var newOnSellAsset = assetItem;
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
        /// <summary>
        /// 添加资产
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
        /// <summary>
        /// 更新用户比例信息
        /// </summary>
        /// <param name="tableClient"></param>
        /// <param name="yemUserProductDto"></param>
        /// <param name="addUserAssetRatios"></param>
        /// <param name="addAssetUserRatios"></param>
        /// <param name="modifyOnSellAssetDtos"></param>
        /// <param name="writeAssetAzureTable"></param>
        /// <param name="writeYemUserProductAzureTable"></param>
        /// <param name="writeUserAssetRatioAzureTable"></param>
        /// <param name="writeAssetUserRatioAzureTable"></param>
        /// <param name="batchNum"></param>
        public static async Task InsertOrReplaceAzureTable(CloudTableClient tableClient, YemUserProductDto yemUserProductDto, List<UserAssetRatio> addUserAssetRatios, List<UserAssetRatio> addAssetUserRatios, List<OnSellAssetDto> modifyOnSellAssetDtos, string writeAssetAzureTable, string writeYemUserProductAzureTable, string writeUserAssetRatioAzureTable, string writeAssetUserRatioAzureTable, int batchNum = 100)
        {
            try
            {
                List<Task> tasks = new List<Task>();
                tasks.Add(AddOnSellAsset(tableClient, modifyOnSellAssetDtos, writeAssetAzureTable, batchNum));

                tasks.Add(AddUserAssetRatio(tableClient, addUserAssetRatios, writeUserAssetRatioAzureTable, batchNum));

                //资产用户关系
                tasks.Add(AddAssetUserRelation(tableClient, addAssetUserRatios, writeAssetUserRatioAzureTable));

                //购买
                tasks.Add(AddYemUserPurchase(tableClient, yemUserProductDto, writeYemUserProductAzureTable));

                await Task.WhenAll(tasks);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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

        public static Task AddAssetUserRelation(CloudTableClient tableClient, List<UserAssetRatio> addAssetUserRatios, string writeAssetUserRatioAzureTable)
        {
            if (addAssetUserRatios.Any())
            {
                CloudTable assetuserTable = tableClient.GetTableReference(writeAssetUserRatioAzureTable);
                var assetIds = addAssetUserRatios.Select(p => p.AssetId).ToList();
                Parallel.For(0, assetIds.Count, i =>
                {
                    var currentUserAsset = addAssetUserRatios.FirstOrDefault(p => p.AssetId == assetIds[i]);
                    if (currentUserAsset != null)
                    {
                        assetuserTable.ExecuteAsync(TableOperation.InsertOrReplace(currentUserAsset));
                    }
                });
            }
            return Task.FromResult(0);
        }

        private static async Task AddUserAssetRatio(CloudTableClient tableClient, List<UserAssetRatio> addUserAssetRatios, string writeUserAssetRatioAzureTable, int batchNum)
        {
            //用户资产关系
            if (addUserAssetRatios.Any())
            {
                int ratioCount = (int)Math.Ceiling((double)addUserAssetRatios.Count / batchNum);
                CloudTable userassetTable = tableClient.GetTableReference(writeUserAssetRatioAzureTable);
                for (int i = 0; i < ratioCount; i++)
                {
                    TableBatchOperation batch = new TableBatchOperation();
                    foreach (UserAssetRatio tableEntity in addUserAssetRatios.Skip(batchNum * i).Take(batchNum))
                    {
                        batch.Add(TableOperation.InsertOrReplace(tableEntity));
                    }
                    await userassetTable.ExecuteBatchAsync(batch);
                }
            }
        }

        /// <summary>
        /// 创建比例关系
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
                Status = 4,  //用户资产报备成功
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
                Status = oldUserAssetRatio.Status,  //用户资产报备成功
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
        /// 得到该用户分配该资产的金额
        /// </summary>
        /// <param name="purchaseMoney"></param>
        /// <param name="assetAmount"></param>
        /// <param name="sumAssetAmount"></param>
        /// <returns></returns>
        private static long AllocateAmountByAsset(long purchaseMoney, long assetAmount, double sumAssetAmount)
        {
            return (long)Math.Ceiling(purchaseMoney * assetAmount / sumAssetAmount);
        }
    }
}
