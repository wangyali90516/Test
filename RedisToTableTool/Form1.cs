using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;
using UserToAssetTool;

namespace RedisToTableTool
{
    public partial class Form1 : Form
    {
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;
        private bool isCompleteByAsset;
        private bool isCompleteByUser;

        public Form1()
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);
                this.tableClient = storageAccount.CreateCloudTableClient();
                this.tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(2.Seconds(), 6);
                this.tableClient.DefaultRequestOptions.MaximumExecutionTime = 2.Minutes();
                this.tableClient.DefaultRequestOptions.ServerTimeout = 2.Minutes();
                this.loadAppSettings = new LoadAppSettings
                {
                    AssetThreadNums = Convert.ToInt32(ConfigurationManager.AppSettings["AssetThreadNums"]),
                    BatchAssetNums = Convert.ToInt32(ConfigurationManager.AppSettings["BatchAssetNums"]),
                    UserThreadNums = Convert.ToInt32(ConfigurationManager.AppSettings["UserThreadNums"]),
                    BatchUserNums = Convert.ToInt32(ConfigurationManager.AppSettings["BatchUserNums"]),
                    WriteAssetUserTableName = ConfigurationManager.AppSettings["writeAssetUserRatioAzureTable"],
                    WriteUserAssetTableName = ConfigurationManager.AppSettings["WriteUserAssetRatioAzureTable"],
                    SearchAssetAzureTable = ConfigurationManager.AppSettings["SearchAssetAzureTable"],
                    SearchUserInfoAzureTable = ConfigurationManager.AppSettings["SearchYemUserProductAzureTable"],
                    YemApiUrl = ConfigurationManager.AppSettings["YemApiUrl"],
                    CheckAssetInfoThreadNums = Convert.ToInt32(ConfigurationManager.AppSettings["CheckAssetInfoThreadNums"]),
                    ReloadTableDataToDiskThreadNums = Convert.ToInt32(ConfigurationManager.AppSettings["ReloadTableDataToDiskThreadNums"]),
                    EndDbNumber = Convert.ToInt32(ConfigurationManager.AppSettings["endDbNumber"]),
                    DbNumber = Convert.ToInt32(ConfigurationManager.AppSettings["dbNumber"]),
                    AssetDebtToTransfer = ConfigurationManager.AppSettings["AssetDebtToTransfer"],
                    BankGatewayUrl = ConfigurationManager.AppSettings["Bankgateway"],
                    AssetApiUrl = ConfigurationManager.AppSettings["AssetApiUrl"],
                    MerchantId = ConfigurationManager.AppSettings["MerchantId"],
                    ServiceBusConnectionString = ConfigurationManager.AppSettings["ServiceBusConnectionString"]
                };
                CheckForIllegalCrossThreadCalls = false;
                this.InitializeComponent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async void btn_start_Click(object sender, EventArgs e)
        {
            try
            {
                //CloudTable yemOnSellAssetTable = this.tableClient.GetTableReference("AssetOnSellAssets"); //购买订单表配置
                //TableQuery<OnSellAssetDto> queryOnSellAsset = new TableQuery<OnSellAssetDto>().Where("IsDeleted eq false");
                //List<OnSellAssetDto> onSellAssets = yemOnSellAssetTable.ExecuteQuery(queryOnSellAsset).ToList();
                List<string> listUserIds = new List<string>();
                List<string> listAssetIds = new List<string>();
                //从文件中获取数据
                if (this.ck_IsFromAzureData.Checked)
                {
                    listUserIds = File.ReadAllLines("UserAssetIds.txt").ToList();
                    listAssetIds = File.ReadAllLines("AssetUserIds.txt").ToList();
                }
                else
                {
                    int insertAssertUserRangeStartInfo = Convert.ToInt32(this.txb_AssertRangeStart.Text.Trim());
                    int insertAssertUserRangeEndInfo = Convert.ToInt32(this.txb_AssertRangeEnd.Text.Trim());
                    int insertUserAssertRangeStartInfo = Convert.ToInt32(this.txb_UserRangeStart.Text.Trim());
                    int insertUserAssertRangeEndInfo = Convert.ToInt32(this.txb_UserRangeEnd.Text.Trim());
                    //从azureTable中获取数据
                    if (insertAssertUserRangeStartInfo != 0 && insertAssertUserRangeEndInfo == 0)
                    {
                        MessageBox.Show("用户Range格式不正确");
                        return;
                    }
                    if (insertAssertUserRangeStartInfo > insertAssertUserRangeEndInfo)
                    {
                        MessageBox.Show("资产Range开始数值必须小于结束数值");
                        return;
                    }
                    if (insertUserAssertRangeStartInfo != 0 && insertUserAssertRangeEndInfo == 0)
                    {
                        MessageBox.Show("用户Range格式不正确");
                        return;
                    }
                    if (insertUserAssertRangeStartInfo > insertUserAssertRangeEndInfo)
                    {
                        MessageBox.Show("用户Range开始数值必须小于结束数值");
                        return;
                    }
                    this.lbl_showMessage.Text = "正在从redis拉出ids.......";
                    if (insertAssertUserRangeEndInfo != 0)
                    {
                        listAssetIds = RedisHelper.GetRedisAssetInfos().Select(asset => asset.OnSellAssetId).Distinct().Skip(insertAssertUserRangeStartInfo).Take(insertAssertUserRangeEndInfo - insertAssertUserRangeStartInfo).ToList();
                        //记录下需要插入的资产ids
                        string aids = string.Join("\r\n", listAssetIds);
                        Logger.LoadData(@"RedisToTable\" + $"RecordAssets_{DateTime.UtcNow.ToChinaStandardTime():yyyyMMddHHmmss}.txt", aids);
                    }
                    if (insertUserAssertRangeEndInfo != 0)
                    {
                        //开始执行用户资产关系表
                        listUserIds = RedisHelper.GetRedisUserInfos().Select(u => u.UserId).Distinct().Skip(insertUserAssertRangeStartInfo).Take(insertUserAssertRangeEndInfo - insertUserAssertRangeStartInfo).ToList();
                        string uids = string.Join("\r\n", listUserIds);
                        Logger.LoadData(@"RedisToTable\" + $"RecordUids_{DateTime.UtcNow.ToChinaStandardTime():yyyyMMddHHmmss}.txt", uids);
                    }
                }
                if (listAssetIds.Count == 0 && listUserIds.Count == 0)
                {
                    MessageBox.Show("没有需要插入到YUNTABLE的数据");
                    this.lbl_showMessage.Text = "";
                    return;
                }
                //开始执行数据
                this.btn_start.Enabled = false;
                this.lbl_showMessage.Text = "正在往YUN Table中插入数据.......";
                this.txb_assetFNums.Text = "0";
                this.txb_assetSNums.Text = "0";
                this.txb_userFNums.Text = "0";
                this.txb_userSNums.Text = "0";
                //获取云table中的数据
                Stopwatch watch = new Stopwatch();
                watch.Start();
                if (listAssetIds.Count != 0)
                {
                    //开始执行资产用户关系表
                    await this.ParalleOrOneAddAssetUserRadio(listAssetIds, this.ck_Asset.Checked ? 1 : 0);
                }
                else
                {
                    this.isCompleteByAsset = true;
                }
                if (listUserIds.Count > 0)
                {
                    //开始执行用户资产关系表
                    await this.ParalleOrOneAddUserAssetRadio(listUserIds, this.ck_Asset.Checked ? 1 : 0);
                }
                else
                {
                    this.isCompleteByUser = true;
                }
                Thread thEnd = new Thread(() =>
                {
                    while (!this.isCompleteByUser || !this.isCompleteByAsset)
                    {
                        Thread.Sleep(5000);
                    }
                    watch.Stop();
                    //获取运行时间[毫秒]
                    long times = watch.ElapsedMilliseconds;
                    this.isCompleteByUser = false;
                    this.isCompleteByAsset = false;
                    this.btn_start.Enabled = true;
                    this.lbl_showMessage.Text = $"执行完毕,一共使用时间{times / 1000}s";
                });
                thEnd.IsBackground = true;
                thEnd.Start();
            }
            catch (Exception exception)
            {
                this.btn_start.Enabled = true;
                this.lbl_showMessage.Text = "";
                Logger.LoadData(@"RedisToTable\Error.txt", exception.Message + "---------" + exception.StackTrace);
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     插入资产用户关系数据
        /// </summary>
        /// <param name="tableClient1"></param>
        /// <param name="addUserAssetRatios"></param>
        /// <param name="writeUserAssetRatioAzureTable"></param>
        /// <param name="batchNum"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        private async Task AddAssetUserRatio(CloudTableClient tableClient1, List<UserAssetRatio> addUserAssetRatios, string writeUserAssetRatioAzureTable, int batchNum, string assetId)
        {
            try
            {
                //用户资产关系
                if (addUserAssetRatios.Any())
                {
                    int ratioCount = (int)Math.Ceiling((double)addUserAssetRatios.Count / batchNum);
                    CloudTable userassetTable = tableClient1.GetTableReference(writeUserAssetRatioAzureTable);
                    for (int i = 0; i < ratioCount; i++)
                    {
                        TableBatchOperation batch = new TableBatchOperation();
                        foreach (UserAssetRatio tableEntity in addUserAssetRatios.Skip(batchNum * i).Take(batchNum))
                        {
                            UserAssetRatio insertModel = this.MapToTableUserAssetRatio(tableEntity, await RedisHelper.GetRedisUserInfoAsync(tableEntity.UserId), await RedisHelper.GetRedisAssetIdInfoAsync(tableEntity.AssetId), 1);
                            batch.Add(TableOperation.Insert(insertModel));
                        }
                        await userassetTable.ExecuteBatchAsync(batch);
                    }
                    this.UpdateNums(0, 0);
                }
            }
            catch (Exception e)
            {
                Logger.LoadData(@"RedisToTable\ErrorAsset.txt", $"插入资产用户关系表时错误，错误信息{e.Message}userId为{assetId},{assetId}");
                this.UpdateNums(0, 1);
                Logger.LogRecord(assetId, 1);
            }
        }

        /// <summary>
        ///     插入用户资产关系表
        /// </summary>
        /// <param name="tableClient1"></param>
        /// <param name="addUserAssetRatios"></param>
        /// <param name="writeUserAssetRatioAzureTable"></param>
        /// <param name="batchNum"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task AddUserAssetRatio(CloudTableClient tableClient1, List<UserAssetRatio> addUserAssetRatios, string writeUserAssetRatioAzureTable, int batchNum, string userId)
        {
            try
            {
                //用户资产关系
                if (addUserAssetRatios.Any())
                {
                    int ratioCount = (int)Math.Ceiling((double)addUserAssetRatios.Count / batchNum);
                    CloudTable userassetTable = tableClient1.GetTableReference(writeUserAssetRatioAzureTable);
                    for (int i = 0; i < ratioCount; i++)
                    {
                        TableBatchOperation batch = new TableBatchOperation();
                        foreach (UserAssetRatio tableEntity in addUserAssetRatios.Skip(batchNum * i).Take(batchNum))
                        {
                            UserAssetRatio insertModel = this.MapToTableUserAssetRatio(tableEntity, await RedisHelper.GetRedisUserInfoAsync(tableEntity.UserId), await RedisHelper.GetRedisAssetIdInfoAsync(tableEntity.AssetId), 0);
                            batch.Add(TableOperation.Insert(insertModel));
                        }
                        await userassetTable.ExecuteBatchAsync(batch);
                    }
                    this.UpdateNums(1, 0);
                }
            }
            catch (Exception e)
            {
                this.UpdateNums(1, 1);
                Logger.LoadData(@"RedisToTable\ErrorUser.txt", $"插入用户资产关系表时错误，错误信息{e.Message}userId为{userId},{userId}");
                Logger.LogRecord(userId, 0);
            }
        }

        private void btn_doAllThings_Click(object sender, EventArgs e)
        {
            ProcessAllDeptForm form = new ProcessAllDeptForm(this.loadAppSettings, this.tableClient);
            form.Show();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            long totalNums = 0;
            for (int i = 0; i < numbers.Count; i++)
            {
                int number = numbers[i];
                RedisHelperSpecial redisHelperSpecial = new RedisHelperSpecial(number);
                //啦list
                List<string> userids = redisHelperSpecial.GetRedisUserInfos().Select(u => u.UserId).ToList();
                for (int j = 0; j < userids.Count; j++)
                {
                    //获取list
                    totalNums += redisHelperSpecial.GetRedisUserAssetRatiosAsync(userids[j]).Count;
                }
            }
            //
            MessageBox.Show(totalNums.ToString());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Stopwatch sp = new Stopwatch();
            int[] datas = new int[10000];
            Random random = new Random();
            for (int i = 0; i < 10000; i++)
            {
                datas[i] = random.Next(0, 100);
            }
            sp.Start();
            //int[] datas = { 100, 100, 20, 50, 60, 10, 100, 20, 50, 100, 100, 30, 15 };
            List<long> result1 = this.GetDpData(datas, 10000000);
            sp.Stop();
            MessageBox.Show(sp.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            int count = result1.Count;
            //MessageBox.Show(string.Join(",", result1) + "---" + result1.Max() + "--" + count);
            List<int> listDatas = new List<int> { 1, 3, 4, 8, 11, 38, 234, 2345, 5678, 111111, 345678, 10000000, 222222222 };
            Tuple<int, int> result = this.GetSuitableData(listDatas, Convert.ToInt32(this.txb_suitableData.Text.Trim()));
            MessageBox.Show($"{result.Item1},{result.Item2}");
        }

        /// <summary>
        /// yushenggou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            PrePurchaseForm form = new PrePurchaseForm();
            form.ShowDialog();
        }

        /// <summary>
        ///     当状态发生改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ck_IsFromAzureData_CheckedChanged(object sender, EventArgs e)
        {
            this.gb_auzer.Enabled = !this.ck_IsFromAzureData.Checked;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //查找某个数值
            //List<int> listDatas = new List<int> { 1, 3, 4, 8, 11, 38, 234, 2345, 5678, 111111, 345678, 10000000, 222222222 };
            //Tuple<int, int> result =this.GetSuitableData(listDatas, 25);
            //MessageBox.Show($"{this.GetSuitableData(listDatas, 25).Item1},{this.GetSuitableD}");
            //int a = 0;
            //Tuple<int, int> result1 = this.GetNumberFromListData(listDatas, 22232111);
            // MessageBox.Show($"{result1.Item1},{result1.Item2}");
            //如何找到一个有序数组中比目标值大于或者等于的
        }

        private List<long> GetDpData(int[] datas, long closeData)
        {
            long[] best = new long[closeData];
            foreach (int t in datas)
            {
                for (long j = closeData - 1; j >= t; j--)
                {
                    long maxData = Math.Max(best[j], best[j - t] + t);
                    best[j] = maxData;
                }
            }
            return best.ToList();
        }

        private Tuple<int, int> GetNumberFromListData(List<int> datas, int number)
        {
            //排序查找数据 是怎样的数据
            int indexStart = 0;
            int indexEnd = datas.Count - 1;
            int compareCount = 0;
            while (indexStart <= indexEnd)
            {
                compareCount++;
                int indexMid = (indexStart + indexEnd) / 2;
                if (datas[indexMid] > number)
                {
                    indexEnd = indexMid - 1;
                }
                else if (datas[indexMid] < number)
                {
                    indexStart = indexMid + 1;
                }
                else
                {
                    return new Tuple<int, int>(compareCount, number);
                }
            }
            return new Tuple<int, int>(compareCount, -1);
        }

        private Tuple<int, int> GetSuitableData(List<int> datas, int number)
        {
            //如果集合里面存在比目标数据大的则取出比它大的最小值,如果不存在数据
            int indexStart = 0;
            int indexEnd = datas.Count - 1;
            int compareCount = 0;
            if (datas[indexEnd] >= number)
            {
                //二分查找
                while (indexStart <= indexEnd)
                {
                    compareCount++;
                    int indexMid = (indexStart + indexEnd) / 2;
                    if (datas[indexMid] > number)
                    {
                        //indexEnd = indexMid - 1;
                        int indexLeft = indexMid - 1;
                        if (indexLeft >= 0 && datas[indexLeft] <= number)
                        {
                            return new Tuple<int, int>(compareCount, datas[indexMid]);
                        }
                        indexEnd = indexMid - 1;
                    }
                    else if (datas[indexMid] < number)
                    {
                        int indexRight = indexMid + 1;
                        if (indexRight <= datas.Count - 1 && datas[indexRight] >= number)
                        {
                            return new Tuple<int, int>(compareCount, datas[indexRight]);
                        }
                        indexStart = indexMid + 1;
                    }
                    else
                    {
                        return new Tuple<int, int>(compareCount, datas[indexMid]);
                        //return new Tuple<int, int>(compareCount, number);
                    }
                }
                return new Tuple<int, int>(compareCount, datas[indexStart]);
            }
            else
            {
                return new Tuple<int, int>(compareCount, datas[indexEnd]);
            }
        }

        /// <summary>
        ///     map云table数据
        /// </summary>
        /// <param name="userAssetRatio"></param>
        /// <param name="userInfo"></param>
        /// <param name="assetInfoRedis"></param>
        /// <param name="type">0,1</param>
        /// <returns></returns>
        private UserAssetRatio MapToTableUserAssetRatio(UserAssetRatio userAssetRatio, UserInfoRedis userInfo, OnSellAssetToRedis assetInfoRedis, int type)
        {
            //6
            UserAssetRatio newUserAssetRation = userAssetRatio;
            //1.用户信息6
            newUserAssetRation.Cellphone = userInfo.Cellphone;
            newUserAssetRation.CredentialNo = userInfo.CredentialNo;
            newUserAssetRation.OrderTime = userInfo.OrderTime;
            newUserAssetRation.OrderId = userInfo.OrderId;
            newUserAssetRation.PurchaseMoney = userInfo.PurchaseMoney;
            newUserAssetRation.UserName = userInfo.UserName;
            //2.资产信息3
            newUserAssetRation.AssetCategoryCode = assetInfoRedis.AssetCategoryCode;
            newUserAssetRation.BillDueDate = assetInfoRedis.BillDueDate;
            newUserAssetRation.Denominator = assetInfoRedis.Denominator;
            //3.拼接其余默认配置 17
            newUserAssetRation.Reserve = "1";
            newUserAssetRation.Status = 4;
            newUserAssetRation.IsInvestSuccess = true;
            newUserAssetRation.IsNotifyTradingSuccess = true;
            newUserAssetRation.IsReturned = false;
            newUserAssetRation.NotifyTradingRespInfo = "Mock";
            newUserAssetRation.NotifyTradingTime = DateTime.UtcNow.ToChinaStandardTime();
            newUserAssetRation.UserPresentValue = 0;
            newUserAssetRation.CreatedBy = "System";
            newUserAssetRation.CreatedTime = DateTime.UtcNow.ToChinaStandardTime();
            newUserAssetRation.IsDeleted = false;
            newUserAssetRation.UpdatedBy = "System";
            newUserAssetRation.UpdatedTime = DateTime.UtcNow.ToChinaStandardTime();
            newUserAssetRation.PartitionKey = type == 0 ? userInfo.UserId : assetInfoRedis.OnSellAssetId; //0是用户资产 1是资产用户
            newUserAssetRation.RowKey = (type == 0 ? newUserAssetRation.AssetId : newUserAssetRation.UserId) + "_" + newUserAssetRation.UserAssetRatioId;
            newUserAssetRation.OriginalUserAssetRatioId = newUserAssetRation.UserAssetRatioId;
            newUserAssetRation.ETag = DateTime.UtcNow.ToChinaStandardTime().UnixTimestamp().ToString();
            return newUserAssetRation;
        }

        /// <summary>
        ///     并行插入资产用户关系表数据
        /// </summary>
        /// <param name="assetIds"></param>
        /// <param name="operateType"></param>
        /// <returns></returns>
        private async Task ParalleOrOneAddAssetUserRadio(List<string> assetIds, int operateType)
        {
            //operateType
            switch (operateType)
            {
                case 0:
                    foreach (string onSellAssetId in assetIds)
                    {
                        //获取redis数据
                        List<UserAssetRatio> listuserAssetRatios = RedisHelper.GetRedisAssetUserRatiosAsync(onSellAssetId);
                        //插入云table
                        //用户资产关系
                        await this.AddAssetUserRatio(this.tableClient, listuserAssetRatios, ConfigurationManager.AppSettings["writeAssetUserRatioAzureTable"], this.loadAppSettings.BatchAssetNums, onSellAssetId);
                        //结束
                    }
                    this.isCompleteByAsset = true;
                    break;

                case 1:
                    //使用多线程并发执行
                    for (int i = 0; i < this.loadAppSettings.AssetThreadNums; i++)
                    {
                        Thread th = new Thread(async () =>
                        {
                            while (assetIds.Count > 0)
                            {
                                string catchAssetId = string.Empty;
                                lock (this.objLock)
                                {
                                    if (assetIds.Count > 0)
                                    {
                                        catchAssetId = assetIds[0];
                                        if (!catchAssetId.IsNullOrEmpty())
                                        {
                                            assetIds.Remove(catchAssetId);
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(catchAssetId))
                                {
                                    //执行插入
                                    //获取redis数据
                                    List<UserAssetRatio> listuserAssetRatios = RedisHelper.GetRedisAssetUserRatiosAsync(catchAssetId);
                                    //插入云table
                                    //资产用户关系
                                    if (listuserAssetRatios.Count > 0)
                                    {
                                        await this.AddAssetUserRatio(this.tableClient, listuserAssetRatios, ConfigurationManager.AppSettings["writeAssetUserRatioAzureTable"], this.loadAppSettings.BatchAssetNums, catchAssetId);
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(1000);
                                }
                            }
                            this.isCompleteByAsset = true;
                        });
                        th.IsBackground = true;
                        th.Start();
                    }
                    break;
            }
        }

        /// <summary>
        ///     用户资产关系
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="operateType"></param>
        /// <returns></returns>
        private async Task ParalleOrOneAddUserAssetRadio(List<string> userIds, int operateType)
        {
            //operateType
            switch (operateType)
            {
                case 0:
                    foreach (string uid in userIds)
                    {
                        //获取redis数据
                        List<UserAssetRatio> listuserAssetRatios = RedisHelper.GetRedisUserAssetRatiosAsync(uid);
                        //插入云table
                        //用户资产关系
                        if (listuserAssetRatios.Count > 0)
                        {
                            await this.AddUserAssetRatio(this.tableClient, listuserAssetRatios, this.loadAppSettings.WriteUserAssetTableName, this.loadAppSettings.BatchUserNums, uid);
                        }
                    }
                    this.isCompleteByUser = true;
                    break;

                case 1:
                    //使用多线程并发执行
                    for (int i = 0; i < this.loadAppSettings.UserThreadNums; i++)
                    {
                        Thread th = new Thread(async () =>
                        {
                            while (userIds.Count > 0)
                            {
                                string catchUserId = string.Empty;
                                lock (this.objLock)
                                {
                                    if (userIds.Count > 0)
                                    {
                                        catchUserId = userIds[0];
                                        if (!catchUserId.IsNullOrEmpty())
                                        {
                                            userIds.Remove(catchUserId);
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(catchUserId))
                                {
                                    //执行插入
                                    //获取redis数据
                                    List<UserAssetRatio> listuserAssetRatios = RedisHelper.GetRedisUserAssetRatiosAsync(catchUserId);
                                    //插入云table
                                    //用户资产关系
                                    if (listuserAssetRatios.Count > 0)
                                    {
                                        await this.AddUserAssetRatio(this.tableClient, listuserAssetRatios, this.loadAppSettings.WriteUserAssetTableName, this.loadAppSettings.BatchUserNums, catchUserId);
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(1000);
                                }
                            }
                            this.isCompleteByUser = true;
                        });
                        th.IsBackground = true;
                        th.Start();
                    }
                    break;
            }
        }

        /// <summary>
        ///     reload到磁盘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadUserAssetRatioForm reloadUserAssetRatioForm = new ReloadUserAssetRatioForm(this.loadAppSettings, this.tableClient);
            reloadUserAssetRatioForm.ShowDialog();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            UpdateUserAssetRatioInfoForm form = new UpdateUserAssetRatioInfoForm(this.loadAppSettings, this.tableClient);
            form.Show();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            NotifyTrisferSystemForm form = new NotifyTrisferSystemForm(this.loadAppSettings, this.tableClient);
            form.Show();
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            ComputerUserTotalMoneyForm form = new ComputerUserTotalMoneyForm();
            form.ShowDialog();
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            GetCancelBookDataForm form = new GetCancelBookDataForm(this.loadAppSettings, this.tableClient);
            form.Show();
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            GetRebateToBank form = new GetRebateToBank(this.loadAppSettings, this.tableClient);
            form.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            RepairDataForm form = new RepairDataForm(this.loadAppSettings, this.tableClient);
            form.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SearchRedisForm form = new SearchRedisForm(this.loadAppSettings, this.tableClient);
            form.Show();
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            ProcessAllDeptForm form = new ProcessAllDeptForm(this.loadAppSettings, this.tableClient);
            form.ShowDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            GetJsonForm form = new GetJsonForm(this.loadAppSettings, this.tableClient);
            form.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            UpdateDataFromBank updateDataFromBank = new UpdateDataFromBank(this.loadAppSettings, this.tableClient);
            updateDataFromBank.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            UpdateYemUserProductInfoForm form = new UpdateYemUserProductInfoForm(this.loadAppSettings, this.tableClient);
            form.ShowDialog();
        }

        /// <summary>
        ///     修改比例关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            UpdateAssetUserRatioForms form = new UpdateAssetUserRatioForms(this.loadAppSettings, this.tableClient);
            form.Show();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            ConsumeDeatMsgForm consumeDeatMsgForm = new ConsumeDeatMsgForm(this.loadAppSettings);
            consumeDeatMsgForm.Show();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            UpdateUserAssetratio form = new UpdateUserAssetratio(this.loadAppSettings, this.tableClient);
            form.Show();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            AddAssets addAssets = new AddAssets();
            addAssets.Show();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            AddOnSellAssetForm obj = new AddOnSellAssetForm();
            obj.Show();
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            GrantForm grantForm = new GrantForm();
            grantForm.Show();
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            AddBatchUserAssetRatio obj = new AddBatchUserAssetRatio();
            obj.Show();
        }

        private void tsb_checkAssetInfo_Click(object sender, EventArgs e)
        {
            CheckDiskAssetInfo checkForm = new CheckDiskAssetInfo(this.loadAppSettings, this.tableClient);
            checkForm.ShowDialog();
        }

        private void tsb_CheckUserAssetInfo_Click(object sender, EventArgs e)
        {
            CheckOneRedisUserAssetInfoForm checkOneRedisUserAssetInfoForm = new CheckOneRedisUserAssetInfoForm(this.loadAppSettings, this.tableClient);
            checkOneRedisUserAssetInfoForm.ShowDialog();
        }

        /// <summary>
        ///     回滚数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsp_Rollback_Click(object sender, EventArgs e)
        {
            //RollBackDataForm rollBackDataForm = new RollBackDataForm(this.tableClient, this.loadAppSettings, this.tsp_Rollback);
            //rollBackDataForm.Show();
        }

        private void UpdateNums(int type, int successOrFail)
        {
            lock (this.objLock)
            {
                //asset
                if (type == 0)
                {
                    if (successOrFail == 0)
                    {
                        this.txb_assetSNums.Text = (Convert.ToInt32(this.txb_assetSNums.Text) + 1).ToString();
                    }
                    else
                    {
                        this.txb_assetFNums.Text = (Convert.ToInt32(this.txb_assetFNums.Text) + 1).ToString();
                    }
                }
                else
                {
                    if (successOrFail == 0)
                    {
                        this.txb_userSNums.Text = (Convert.ToInt32(this.txb_userSNums.Text) + 1).ToString();
                    }
                    else
                    {
                        this.txb_userFNums.Text = (Convert.ToInt32(this.txb_userFNums.Text) + 1).ToString();
                    }
                }
            }
            //lock (userassetTable)
            //{
            //}
        }
    }
}