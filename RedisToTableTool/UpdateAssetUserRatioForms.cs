using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;
using UserToAssetTool;

namespace RedisToTableTool
{
    public partial class UpdateAssetUserRatioForms : Form
    {
        private readonly DateTime endTime;
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly RedisHelperSpecial redisHelperSpecial;
        private readonly DateTime startTime;
        private readonly CloudTableClient tableClient;
        private readonly int threadNums = 1;

        public UpdateAssetUserRatioForms(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.threadNums = Convert.ToInt32(ConfigurationManager.AppSettings["ThreadNumsByUpdate"]);
            this.redisHelperSpecial = new RedisHelperSpecial(11);
            this.startTime = Convert.ToDateTime(ConfigurationManager.AppSettings["startTime"]).ToChinaStandardTime();
            this.endTime = Convert.ToDateTime(ConfigurationManager.AppSettings["endTime"]).ToChinaStandardTime();
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromSeconds(180);
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        public UserAssetRatio MapToNewUserAssetRatio(UserAssetRatio model)
        {
            return new UserAssetRatio
            {
                AssetCategoryCode = model.AssetCategoryCode,
                AssetId = model.AssetId,
                BillDueDate = model.BillDueDate,
                Capital = model.Capital,
                Cellphone = model.Cellphone,
                CredentialNo = model.CredentialNo,
                Denominator = model.Denominator,
                IsInvestSuccess = model.IsInvestSuccess,
                IsNotifyTradingSuccess = model.IsNotifyTradingSuccess,
                IsReturned = model.IsReturned,
                NotifyTradingRespInfo = model.NotifyTradingRespInfo,
                NotifyTradingTime = model.NotifyTradingTime,
                Numerator = model.Numerator,
                OrderId = model.OrderId,
                OrderTime = model.OrderTime,
                OriginalUserAssetRatioId = model.OriginalUserAssetRatioId,
                PurchaseMoney = model.PurchaseMoney,
                Reserve = model.Reserve,
                Status = model.Status,
                UserAssetRatioId = model.UserAssetRatioId,
                UserId = model.UserId,
                UserName = model.UserName,
                IsDeleted = model.IsDeleted,
                CreatedBy = model.CreatedBy,
                CreatedTime = model.CreatedTime,
                UpdatedBy = model.UpdatedBy,
                UpdatedTime = model.UpdatedTime,
                UserPresentValue = model.UserPresentValue
            };
        }

        public bool UpdateAssetUserRatioInfo(UserAssetRatio userAssetRatio, string tableName)
        {
            try
            {
                CloudTable deptTable = this.tableClient.GetTableReference(tableName);
                TableOperation updateOperation = TableOperation.InsertOrReplace(userAssetRatio);
                deptTable.Execute(updateOperation);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async void btn_HandleThings_Click(object sender, EventArgs e)
        {
            //执行数量
            try
            {
                ShowUpdateAssetUserInfo updateAssetUserInfo = new ShowUpdateAssetUserInfo();
                DateTime dtDkTime = Convert.ToDateTime(this.txb_updateTime.Text.Trim());
                string tbNameByUser = ConfigurationManager.AppSettings["AssetUserAssetRatios"];
                string tbNameByAsset = ConfigurationManager.AppSettings["AssetUserRelation"];
                //获取需要执行的数据
                this.lbl_showmsg.Text = "正在准备数据......";
                this.btn_HandleThings.Enabled = false;
                List<string> userInfos = this.txb_handleUserIds.Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<UserReleaseAmount> listUserReleaseAmounts = userInfos.Select(t => t.Split(',')).Select(userInfo => new UserReleaseAmount { Amount = Convert.ToInt64(userInfo[1]), UserId = userInfo[0] }).ToList();
                //List<string> userIds = listUserReleaseAmounts.Select(x => x.UserId).ToList();
                //获取赎回订单
                //List<string> redemOrderIds = new List<string>();
                //修改所有的赎回订单数量 //备份修改之前的所有赎回订单数据
                //int count = 0;
                //List<UserRedemptionInfo> listRedemptionInfos = this.GetUserRedemptionOrders(userIds).ToList();
                //updateAssetUserInfo.RedemptionTotalNums = listRedemptionInfos.Count;
                //this.lbl_showmsg.Text = "开始修改这批用户的所有赎回订单数据,需执行数量为" + listRedemptionInfos.Count;
                //foreach (UserRedemptionInfo itemRedemption in listRedemptionInfos)
                //{
                //    bool redeemResult = await this.UpdateYemRedeemInfo(itemRedemption.UserId, itemRedemption.OrderId, itemRedemption.AlreadyDealtAmount, 0, 0, 1);
                //    if (redeemResult)
                //    {
                //        count = count + 1;
                //    }
                //}
                //updateAssetUserInfo.RedemptionSuccessNums = count;
                //this.lbl_showmsg.Text = "该次修改成功的赎回订单数据为" + count;
                //获取所有的资产数据
                List<OnSellAssetDto> onSellAssetInfos = this.GetOnSellAssetInfos();
                //修改比例关系并且插入数据库 //备份之前的比例关系
                this.lbl_showmsg.Text = "开始修改比例以及插入信息数据......";
                ConcurrentBag<int> concurrentBag = new ConcurrentBag<int>();
                //显示结果
                //Dictionary<string, int> dicInfos = new Dictionary<string, int>();
                int countDo = listUserReleaseAmounts.Count;
                this.txb_UserNums.Text = countDo.ToString();
                ConcurrentBag<UpdateUserAssetRatio> concurrentBagUpdateInfos = new ConcurrentBag<UpdateUserAssetRatio>();
                List<Task> listTasks = new List<Task>();
                for (int i = 0; i < this.threadNums; i++)
                {
                    Task task = Task.Run(async () =>
                    {
                        while (listUserReleaseAmounts.Count > 0)
                        {
                            UserReleaseAmount userReleaseAmount = null;
                            int nums = 0;
                            lock (this.objLock)
                            {
                                if (listUserReleaseAmounts.Count > 0)
                                {
                                    userReleaseAmount = listUserReleaseAmounts[0];
                                    if (userReleaseAmount != null)
                                    {
                                        //dicInfos.Add(userReleaseAmount.UserId, 0); //显示信息
                                        listUserReleaseAmounts.Remove(userReleaseAmount);
                                    }
                                }
                            }
                            if (userReleaseAmount != null)
                            {
                                //执行
                                string userId = userReleaseAmount.UserId;
                                long redeemAmount = userReleaseAmount.Amount; //赎回金额
                                //拉取所有的用户比例关系
                                List<UserAssetRatio> userAssetRatios = await this.GetSortUserAssetRatios(userId, tbNameByUser);
                                //拉取所有的已经成功的userassetRatioids
                                //List<string> successUserAssetRatioIds = this.redisHelperSpecial.GetRedisSuccessInsertUserAssetRatioIdsAsync(userId);
                                //userAssetRatios.RemoveAll(x => successUserAssetRatioIds.Contains(x.UserAssetRatioId));
                                Logger.LoadData(@"UD_log\logInfo" + Guid.NewGuid().ToGuidString() + ".txt", "需要执行的数量:" + userAssetRatios.Count);
                                foreach (UserAssetRatio userAssetRatio in userAssetRatios)
                                {
                                    if (redeemAmount == 0)
                                    {
                                        break;
                                    }
                                    List<UpdateUserAssetRatioInfos> addUserAssetRatioInfoses = new List<UpdateUserAssetRatioInfos>();
                                    //执行操作
                                    OnSellAssetDto onSellItem = onSellAssetInfos.FirstOrDefault(p => p.OnSellAssetId == userAssetRatio.AssetId);
                                    if (onSellItem == null)
                                    {
                                        continue;
                                    }
                                    long userPresentValue = this.GetUserPrentValue1(userAssetRatio, dtDkTime);
                                    if (userPresentValue > 0)
                                    {
                                        if (redeemAmount >= userPresentValue)
                                        {
                                            redeemAmount -= userPresentValue;
                                            addUserAssetRatioInfoses.Add(new UpdateUserAssetRatioInfos //给新用户
                                            {
                                                UserId = userAssetRatio.UserId,
                                                AssetId = userAssetRatio.AssetId,
                                                DealAmount = userPresentValue,
                                                Capital = userAssetRatio.Capital,
                                                UserAssetRatioId = userAssetRatio.UserAssetRatioId,
                                                OriginalUserAssetRatioId = userAssetRatio.OriginalUserAssetRatioId,
                                                Denominator = onSellItem.PresentValue,
                                                Numerator = userPresentValue //待确认
                                            });
                                            //修改部分东西
                                            userAssetRatio.IsDeleted = true;
                                            userAssetRatio.Reserve = "1";
                                            userAssetRatio.Denominator = onSellItem.PresentValue;
                                            //updateUserAssetRatioInfoses.Add(new ModifyUserAssetRatioRequest
                                            //{
                                            //    AssetId = userAssetRatio.AssetId,
                                            //    Capital = userAssetRatio.Capital,
                                            //    Denominator = onSellItem.PresentValue,
                                            //    Numerator = userAssetRatio.Numerator,
                                            //    IsInvestSuccess = userAssetRatio.IsInvestSuccess,
                                            //    IsNotifyTradingSuccess = userAssetRatio.IsNotifyTradingSuccess,
                                            //    IsReturned = userAssetRatio.IsReturned,
                                            //    Status = userAssetRatio.Status,
                                            //    UserAssetRatioId = userAssetRatio.UserAssetRatioId,
                                            //    UserId = userAssetRatio.UserId,
                                            //    IsDeleted = true,
                                            //    Reserve = "1",
                                            //    OriginalUserAssetRatioId = userAssetRatio.OriginalUserAssetRatioId
                                            //}); //老用户
                                        }
                                        else if (redeemAmount < userPresentValue)
                                        {
                                            //计算赎回本金
                                            long capital = this.GetCapital(redeemAmount, userPresentValue, userAssetRatio.Capital);

                                            addUserAssetRatioInfoses.Add(new UpdateUserAssetRatioInfos //给老用户
                                            {
                                                UserId = userAssetRatio.UserId,
                                                AssetId = userAssetRatio.AssetId,
                                                DealAmount = redeemAmount,
                                                Capital = capital,
                                                UserAssetRatioId = userAssetRatio.UserAssetRatioId,
                                                OriginalUserAssetRatioId = userAssetRatio.OriginalUserAssetRatioId,
                                                Denominator = onSellItem.PresentValue,
                                                Numerator = redeemAmount //待确认
                                            });
                                            userAssetRatio.Capital -= capital; //剩余本金
                                            userAssetRatio.Numerator = userPresentValue - redeemAmount;
                                            userAssetRatio.Denominator = onSellItem.PresentValue;
                                            userAssetRatio.Status = 2;
                                            userAssetRatio.IsReturned = false;
                                            userAssetRatio.IsInvestSuccess = true;
                                            userAssetRatio.IsNotifyTradingSuccess = true;
                                            userAssetRatio.Reserve = "1";
                                            //updateUserAssetRatioInfoses.Add(new ModifyUserAssetRatioRequest
                                            //{
                                            //    AssetId = userAssetRatio.AssetId,
                                            //    Capital = userAssetRatio.Capital,
                                            //    Denominator = onSellItem.PresentValue,
                                            //    Numerator = userPresentValue - redeemAmount,
                                            //    IsInvestSuccess = true,
                                            //    IsNotifyTradingSuccess = true,
                                            //    IsReturned = false,
                                            //    Status = 2,
                                            //    UserAssetRatioId = userAssetRatio.UserAssetRatioId,
                                            //    UserId = userAssetRatio.UserId,
                                            //    IsDeleted = false,
                                            //    Reserve = "1",
                                            //    OriginalUserAssetRatioId = userAssetRatio.OriginalUserAssetRatioId
                                            //}); //老用户
                                            redeemAmount = 0;
                                        }
                                        //插入数据库及更新 addUserAssetRatioInfoses
                                        //修改Azure Table数据
                                        userAssetRatio.ETag = DateTime.UtcNow.ToChinaStandardTime().UnixTimestamp().ToString();
                                        userAssetRatio.Timestamp = DateTimeOffset.UtcNow.ToLocalTime();
                                        //修改两张表中的数据 1.用户资产比例
                                        bool result1 = this.UpdateAssetUserRatioInfo(userAssetRatio, tbNameByUser);
                                        //2.资产用户比例
                                        userAssetRatio.PartitionKey = userAssetRatio.AssetId;
                                        userAssetRatio.RowKey = userAssetRatio.UserId + "_" + userAssetRatio.UserAssetRatioId;
                                        bool result2 = this.UpdateAssetUserRatioInfo(userAssetRatio, tbNameByAsset);
                                        if (result2 && result1)
                                        {
                                            //数据库
                                            //数据库
                                            UpdateUserAssetRatioInfos addUserAssetRationInfo = addUserAssetRatioInfoses.FirstOrDefault(x => x.UserAssetRatioId == userAssetRatio.UserAssetRatioId);
                                            bool result = this.InsertAssetUserRatioInfo(addUserAssetRationInfo);
                                            if (result)
                                            {
                                                lock (this.objLock)
                                                {
                                                    nums = nums + 1;
                                                }
                                                //记下redis
                                                //await this.redisHelperSpecial.SetRedisSuccessInsertUserAssetRatioIdsAsync(userAssetRatio.UserAssetRatioId, userAssetRatio.UserId);
                                            }
                                        }
                                        //foreach (ModifyUserAssetRatioRequest model in updateUserAssetRatioInfoses)
                                        //{
                                        //    //修改比例
                                        //    bool modifyRatioResult = await this.ModifyUserAssetRatiosByDisk(model);
                                        //    if (modifyRatioResult)
                                        //    {
                                        //        //数据库
                                        //        UpdateUserAssetRatioInfos addUserAssetRationInfo = addUserAssetRatioInfoses.FirstOrDefault(x => x.UserAssetRatioId == model.UserAssetRatioId);
                                        //        bool result = this.InsertAssetUserRatioInfo(addUserAssetRationInfo);
                                        //        if (result)
                                        //        {
                                        //            lock (this.objLock)
                                        //            {
                                        //                nums = nums + 1;
                                        //            }
                                        //            //记下redis
                                        //            await this.redisHelperSpecial.SetRedisSuccessInsertUserAssetRatioIdsAsync(model.UserAssetRatioId, model.UserAssetRatioId);
                                        //        }
                                        //    }
                                        //}
                                    }
                                }
                                concurrentBagUpdateInfos.Add(new UpdateUserAssetRatio { SuccessNums = nums, TotalNums = userAssetRatios.Count, UserId = userId });
                                concurrentBag.Add(0);
                                lock (this.objLock)
                                {
                                    this.txb_SuccessNums.Text = ((Convert.ToInt32(this.txb_SuccessNums.Text) + 1)).ToString();
                                }
                            }
                        }
                    });
                    listTasks.Add(task);
                }
                Task.WaitAll(listTasks.ToArray());
                updateAssetUserInfo.UdpateAssetUserInfos = concurrentBagUpdateInfos.ToList();
                //this.txb_SuccessNums.Text = ((Convert.ToInt32(this.txb_SuccessNums.Text)) + 1).ToString();
                //执行完毕
                //记下日志
                Logger.LoadData(@"UD_log\logInfo" + Guid.NewGuid().ToGuidString() + ".txt", updateAssetUserInfo.ToJson());
                this.btn_HandleThings.Enabled = true;
                this.lbl_showmsg.Text = "全部执行完毕,请查看执行日志";
            }
            catch (Exception exception)
            {
                this.lbl_showmsg.Text = "提示：发生一个错误" + exception.Message;
                this.btn_HandleThings.Enabled = true;
                Logger.LoadData(@"UD_Eoor\Error.txt", exception.Message + "-------------------" + exception.StackTrace);
            }
        }

        /// <summary>
        ///     计算本金
        /// </summary>
        /// <param name="userPresentValue"></param>
        /// <param name="captital"></param>
        /// <param name="leftRedemAmout"></param>
        /// <returns></returns>
        private long GetCapital(long leftRedemAmout, long userPresentValue, long captital)
        {
            decimal amount = (decimal)leftRedemAmout / userPresentValue;
            return (long)(amount * captital);
        }

        private List<OnSellAssetDto> GetOnSellAssetInfos()
        {
            try
            {
                //string tbName = ConfigurationManager.AppSettings["AssetOnSellAssets"]; //AssetOnSellAssets
                CloudTable deptTable = this.tableClient.GetTableReference("AssetOnSellAssets");
                List<string> partitionKeys = new List<string> { "ShangPiao", "YinPiao" };
                List<OnSellAssetDto> onsellAssetInfos = new List<OnSellAssetDto>();
                foreach (string partitionKey in partitionKeys)
                {
                    TableQuery<OnSellAssetDto> queryDeptInfo = new TableQuery<OnSellAssetDto>()
                        .Where($"PartitionKey eq '{partitionKey}'");
                    onsellAssetInfos.AddRange(deptTable.ExecuteQuery(queryDeptInfo).ToList());
                }
                return onsellAssetInfos;
            }
            catch (Exception ex)
            {
                //即
                return null;
            }
        }

        private async Task<List<UserAssetRatio>> GetSortUserAssetRatios(string userId, string tbName)
        {
            try
            {
                List<UserAssetRatio> userAssetRatios = this.redisHelperSpecial.GetRedisUserAssetRatioBeforeUpdateAsync(userId);
                if (userAssetRatios.Count == 0)
                {
                    CloudTable deptTable = this.tableClient.GetTableReference(tbName);
                    TableQuery<UserAssetRatio> queryDeptInfo = new TableQuery<UserAssetRatio>()
                        .Where($"PartitionKey eq '{userId}' and IsDeleted eq false and IsReturned eq false");
                    userAssetRatios = deptTable.ExecuteQuery(queryDeptInfo).ToList();
                }
                List<int> listInts = new List<int> { 6, 0, 2, 4 };
                List<UserAssetRatio> sortUserAssetRatio = new List<UserAssetRatio>();
                //排序
                foreach (int t in listInts)
                {
                    sortUserAssetRatio.AddRange(userAssetRatios.Where(x => x.Status == t));
                }
                //插入到redis
                await this.redisHelperSpecial.SetRedisUserAssetRatioBeforeUpdateAsync(userId, sortUserAssetRatio);
                return sortUserAssetRatio;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"UD_RedisError\Error_" + userId + ".txt", e.Message + "--------" + e.StackTrace);
                return null;
            }
        }

        /// <summary>
        ///     计算该用户的持有价值
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="demotr"></param>
        /// <param name="onSellAssetDto"></param>
        /// <returns></returns>
        private long GetUserPrentValue(long numerator, long demotr, OnSellAssetDto onSellAssetDto)
        {
            try
            {
                DateTime dateTime = DateTime.UtcNow.ToChinaStandardTime();
                DateTime useTime = onSellAssetDto.BillDueDate < dateTime ? onSellAssetDto.BillDueDate : dateTime;
                long days = 0;
                if (onSellAssetDto.LastGrowthTime != null)
                {
                    days = (useTime.Date - onSellAssetDto.LastGrowthTime.Value.Date).Days;
                }
                long assetPresentValue = onSellAssetDto.PresentValue;
                if (days > 0)
                {
                    assetPresentValue = (long)(Math.Pow((1 + (double)600 / 3600000), days) * onSellAssetDto.PresentValue);
                }
                //long assetPresentValue = onSellAssetDto.PresentValue * Math.Pow((1 + 0.06), ((useTime - onSellAssetDto.LastGrowthTime).Value.Days));
                decimal amount = (decimal)assetPresentValue / demotr;
                return (long)(amount * numerator);
            }
            catch (Exception e)
            {
                return onSellAssetDto.PresentValue;
            }
        }

        /// <summary>
        ///     计算该用户的持有价值1
        /// </summary>
        /// <param name="userAssetRatio"></param>
        /// <param name="dtTime"></param>
        /// <returns></returns>
        private long GetUserPrentValue1(UserAssetRatio userAssetRatio, DateTime dtTime)
        {
            try
            {
                //DateTime dateTime = Convert.ToDateTime(ConfigurationManager.AppSettings["DkTime"]);
                long days = (dtTime.Date - userAssetRatio.OrderTime.Date).Days;
                long assetPresentValue = 0;
                if (days >= 0)
                {
                    assetPresentValue = (long)(Math.Pow((1 + (double)600 / 3600000), days) * userAssetRatio.Numerator);
                }

                //long assetPresentValue = onSellAssetDto.PresentValue * Math.Pow((1 + 0.06), ((useTime - onSellAssetDto.LastGrowthTime).Value.Days));
                return assetPresentValue;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        ///     获取指定区间内该用户的赎回订单数量
        /// </summary>
        /// <returns></returns>
        private IEnumerable<UserRedemptionInfo> GetUserRedemptionOrders(List<string> userIds)
        {
            try
            {
                CloudTable deptTable = this.tableClient.GetTableReference("AssetUserRedemptionOrders");
                TableQuery<UserRedemptionInfo> queryDeptInfo = new TableQuery<UserRedemptionInfo>()
                    .Where("PartitionKey eq 'RedeemOrder' and RemainingAmount gt 0");
                return deptTable.ExecuteQuery(queryDeptInfo).Where(x => userIds.Contains(x.UserId) && x.OrderTime >= this.startTime && x.OrderTime <= this.endTime).ToList();
            }
            catch (Exception ex)
            {
                //即
                return null;
            }
        }

        /// <summary>
        ///     添加修改后的信息
        /// </summary>
        /// <param name="updateUserAssetRatioInfos"></param>
        /// <returns></returns>
        private bool InsertAssetUserRatioInfo(UpdateUserAssetRatioInfos updateUserAssetRatioInfos)
        {
            try
            {
                //判断是否存在
                string sqlQuery = $"select count(*) from UpdateAssetUserRatioInfo where UserAssetRatioId='{updateUserAssetRatioInfos.UserAssetRatioId}'";
                object count = SqlHelper.SqlHelper.ExecuteScalar(sqlQuery);
                if (count == null)
                {
                    return false;
                }
                if ((int)count > 0)
                {
                    return true;
                }
                string sql = $"insert into UpdateAssetUserRatioInfo(UserId, DealAmount, AssetId,UserAssetRatioId,Capital,Numerator,OriginalUserAssetRatioId,Denominator)values('{updateUserAssetRatioInfos.UserId}',{updateUserAssetRatioInfos.DealAmount},'{updateUserAssetRatioInfos.AssetId}','{updateUserAssetRatioInfos.UserAssetRatioId}',{updateUserAssetRatioInfos.Capital},{updateUserAssetRatioInfos.Numerator},'{updateUserAssetRatioInfos.OriginalUserAssetRatioId}',{updateUserAssetRatioInfos.Denominator})";
                return SqlHelper.SqlHelper.ExecuteNoneQuery(sql) > 0;
            }
            catch (Exception e)
            {
                //记录失败插入的数据
                Logger.LoadData(@"UD_UpdateAssetUserRation\UpdateUserAssetInfoError_" + updateUserAssetRatioInfos.UserAssetRatioId + ".txt", updateUserAssetRatioInfos.ToJson());
                return false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {
        }

        private void lbl_showmsg_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     修改用户资产关系比例
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<bool> ModifyUserAssetRatiosByDisk(ModifyUserAssetRatioRequest request)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/ModifyUserAssetRatiosByDisk", request);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"UD_ModifyUserAssetRatiosByDisk\Error_" + request.UserAssetRatioId + "", request.ToJson());
                return false;
            } //
        }

        private void txb_handleUserIds_TextChanged(object sender, EventArgs e)
        {
        }

        private void txb_SuccessNums_TextChanged(object sender, EventArgs e)
        {
        }

        private void txb_updateTime_TextChanged(object sender, EventArgs e)
        {
        }

        private void txb_UserNums_TextChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     修改赎回订单的数据
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="orderId"></param>
        /// <param name="alreadyDealtAmount"></param>
        /// <param name="remainingAmount"></param>
        /// <param name="serviceCharge"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private async Task<bool> UpdateYemRedeemInfo(string userid, string orderId, long alreadyDealtAmount, long remainingAmount, int serviceCharge, int status)
        {
            HttpResponseMessage httpResponseMesage;
            object modifyUserRedeemRequest = null;
            try
            {
                //记录下需要插入的资产ids
                modifyUserRedeemRequest = new { oughtAmount = 0, UserId = userid, OrderId = orderId, alreadyDealtAmount, remainingAmount, serviceCharge, status };
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/ModifyRedemptionOrderByDisk", modifyUserRedeemRequest);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"UD_UpdateYemRedeemInfo\Error" + orderId + ".txt", modifyUserRedeemRequest.ToJson());
                return false;
            }
        }

        #region Nested type: ShowUpdateAssetUserInfo

        private sealed class ShowUpdateAssetUserInfo
        {
            public int RedemptionSuccessNums { get; set; }
            public int RedemptionTotalNums { get; set; }
            public List<UpdateUserAssetRatio> UdpateAssetUserInfos { get; set; }
        }

        #endregion Nested type: ShowUpdateAssetUserInfo

        #region Nested type: UpdateUserAssetRatio

        public sealed class UpdateUserAssetRatio
        {
            public int SuccessNums { get; set; }
            public int TotalNums { get; set; }
            public string UserId { get; set; }
        }

        #endregion Nested type: UpdateUserAssetRatio

        #region Nested type: UserReleaseAmount

        private sealed class UserReleaseAmount
        {
            public long Amount { get; set; }
            public string UserId { get; set; }
        }

        #endregion Nested type: UserReleaseAmount
    }
}