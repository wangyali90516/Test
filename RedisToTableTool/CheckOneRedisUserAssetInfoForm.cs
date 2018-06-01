using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;
using UserToAssetTool;

namespace RedisToTableTool
{
    public partial class CheckOneRedisUserAssetInfoForm : Form
    {
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;

        /// <summary>
        ///     初始化配置
        /// </summary>
        /// <param name="loadAppSettings"></param>
        /// <param name="tableClient"></param>
        public CheckOneRedisUserAssetInfoForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private void btn_allTableAsset_Click(object sender, EventArgs e)
        {
            //获取Asset对象
            try
            {
                //获取所有的AssetId
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchAssetAzureTable);
                TableQuery<OnSellAssetDto> queryOnSellAsset = new TableQuery<OnSellAssetDto>().Where("IsLock eq false");
                List<string> yemAssetIds = assertIdsTable.ExecuteQuery(queryOnSellAsset).Select(u => u.OnSellAssetId).ToList();
                if (yemAssetIds.Count == 0)
                {
                    MessageBox.Show("需要验证的资产数据为空");
                    return;
                }
                this.txb_AllTableAssetFail.Text = "0";
                this.txb_AllTableAssetSuccess.Text = "0";
                this.btn_allTableAsset.Enabled = false;
                this.lbl_showMsgAllTableAsset.Text = "正在开始验证所有的资产数据,请稍等.....";
                //执行资产
                for (int i = 0; i < 12; i++)
                {
                    Thread thread = new Thread(() =>
                    {
                        while (yemAssetIds.Count > 0)
                        {
                            string assetId = string.Empty;
                            lock (this.objLock)
                            {
                                if (yemAssetIds.Count > 0)
                                {
                                    assetId = yemAssetIds[0];
                                    yemAssetIds.Remove(assetId);
                                }
                            }
                            if (!string.IsNullOrEmpty(assetId))
                            {
                                //执行
                                bool result = this.CheckOneTableAssetInfo(assetId);
                                lock (this.objLock)
                                {
                                    this.UpdateTxbText(result ? this.txb_AllTableAssetSuccess : this.txb_AllTableAssetFail);
                                }
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (yemAssetIds.Count > 0)
                        {
                            Thread.Sleep(1500);
                            continue;
                        }
                        break;
                    }
                    Thread.Sleep(2000);
                    this.btn_allTableAsset.Enabled = true;
                    this.lbl_showMsgAllTableAsset.Text = "验证完所有资产数据，请查看日志信息是否存在";
                    //提示
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.lbl_showMsg_Asset.Text = "验证所有资产数据异常," + exception.Message;
                this.btn_checkAllAsset.Enabled = true;
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"验证单个redis所有资产数据发生异常{exception.Message}");
            }
        }

        private void btn_allTableUser_Click(object sender, EventArgs e)
        {
            //获取用户对象
            try
            {
                //获取所有的userId
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchUserInfoAzureTable);
                string partitionKey = "OldPurchaseOrder";
                TableQuery<YemUserProductDto> queryOnSellAsset = new TableQuery<YemUserProductDto>().Where($"PartitionKey eq  '{partitionKey}' and IsLock eq false ");
                var yemUserProductDtos = assertIdsTable.ExecuteQuery(queryOnSellAsset).ToList();
                List<string> yemUserIds = yemUserProductDtos.Select(u => u.UserId).ToList();
                if (yemUserIds.Count == 0)
                {
                    MessageBox.Show("需要验证的用户订单数据为空");
                    return;
                }
                this.txb_AllTableUserFail.Text = "0";
                this.txb_AllTableUserSuccess.Text = "0";
                this.btn_allTableUser.Enabled = false;
                this.lbl_showMsgAllTableUser.Text = "正在开始验证所有的用户数据,请稍等.....";
                //执行资产
                for (int i = 0; i < 12; i++)
                {
                    Thread thread = new Thread(() =>
                    {
                        while (yemUserIds.Count > 0)
                        {
                            string userId = string.Empty;
                            lock (this.objLock)
                            {
                                if (yemUserIds.Count > 0)
                                {
                                    userId = yemUserIds[0];
                                    yemUserIds.Remove(userId);
                                }
                            }
                            if (!string.IsNullOrEmpty(userId))
                            {
                                //执行
                                bool result = this.CheckOneTableUserInfo(userId, yemUserProductDtos);
                                lock (this.objLock)
                                {
                                    this.UpdateTxbText(result ? this.txb_AllTableUserSuccess : this.txb_AllTableUserFail);
                                }
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (yemUserIds.Count > 0)
                        {
                            Thread.Sleep(1500);
                            continue;
                        }
                        break;
                    }
                    Thread.Sleep(2000);
                    this.btn_allTableUser.Enabled = true;
                    this.lbl_showMsgAllTableUser.Text = "验证完所有用户数据，请查看日志信息是否存在";
                    //提示
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.lbl_showMsg_User.Text = "验证所有用户数据异常," + exception.Message;
                this.btn_checkAllUser.Enabled = true;
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"验证单个redis所有用户订单数据发生异常{exception.Message}");
            }
        }

        /// <summary>
        ///     所有的资产check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_checkAll_Click(object sender, EventArgs e)
        {
            try
            {
                //获取所有的AssetId
                List<string> assetIds = RedisHelper.GetRedisAssetInfos().Select(x => x.OnSellAssetId).ToList();
                if (assetIds.Count == 0)
                {
                    MessageBox.Show("该redis不存在资产数据");
                    return;
                }
                this.txb_checkAssetFailed.Text = "0";
                this.txb_checkAssetSuccess.Text = "0";
                this.btn_checkAllAsset.Enabled = false;
                this.lbl_showMsg_Asset.Text = "正在开始验证所有的资产数据,请稍等.....";
                //执行资产
                for (int i = 0; i < 6; i++)
                {
                    Thread thread = new Thread(() =>
                    {
                        while (assetIds.Count > 0)
                        {
                            string assetId = string.Empty;
                            lock (this.objLock)
                            {
                                if (assetIds.Count > 0)
                                {
                                    assetId = assetIds[0];
                                    assetIds.Remove(assetId);
                                }
                            }
                            if (!string.IsNullOrEmpty(assetId))
                            {
                                //执行
                                bool result = this.CheckAssetInfo(assetId, this.loadAppSettings.DbNumber, new RedisHelperSpecial(this.loadAppSettings.EndDbNumber));
                                lock (this.objLock)
                                {
                                    this.UpdateTxbText(result ? this.txb_checkAssetSuccess : this.txb_checkAssetFailed);
                                }
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (assetIds.Count > 0)
                        {
                            Thread.Sleep(1500);
                            continue;
                        }
                        break;
                    }
                    Thread.Sleep(2000);
                    this.btn_checkAllAsset.Enabled = true;
                    this.lbl_showMsg_Asset.Text = "验证完所有资产数据，请查看日志信息是否存在";
                    //提示
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.lbl_showMsg_Asset.Text = "验证所有资产数据异常," + exception.Message;
                this.btn_checkAllAsset.Enabled = true;
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"验证单个redis所有资产数据发生异常{exception.Message}");
            }
        }

        /// <summary>
        ///     验证所有的用户和资产
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_CheckAllInfo_Click(object sender, EventArgs e)
        {
            //验证所有的用户和资产信息
            try
            {
                //获取所有的asset信息
                string[] redisNumbers = ConfigurationManager.AppSettings["AllDbNumberNotExitEnd"].Split(',');
                Dictionary<string, int> dicAssetInfo = new Dictionary<string, int>();
                for (int i = 0; i < redisNumbers.Length; i++)
                {
                    int number = Convert.ToInt32(redisNumbers[i]);
                    RedisHelperSpecial redisHelperSpecial = new RedisHelperSpecial(number);
                    List<string> oneRedisAssetIds = redisHelperSpecial.GetRedisAssetInfos().Select(a => a.OnSellAssetId).ToList();
                    foreach (string t in oneRedisAssetIds)
                    {
                        if (!dicAssetInfo.ContainsKey(t))
                        {
                            dicAssetInfo.Add(t, number);
                        }
                    }
                }
                //循环跑
                if (dicAssetInfo.Count == 0)
                {
                    MessageBox.Show("redis不存在资产数据");
                }
                this.lbl_ShowAllAssetInfo.Text = "正在开始验证所有的资产数据,请稍等.....";
                this.btn_CheckAllAssetInfo.Enabled = false;
                this.txb_allAssetFail.Text = "0";
                this.txb_allAssetSuccess.Text = "0";
                //跑数据
                for (int i = 0; i < 6; i++)
                {
                    Thread th = new Thread(() =>
                    {
                        while (dicAssetInfo.Count > 0)
                        {
                            string assetId = string.Empty;
                            int dbNumber = 0;
                            lock (this.objLock)
                            {
                                if (dicAssetInfo.Count > 0)
                                {
                                    assetId = dicAssetInfo.Keys.First();
                                    dbNumber = dicAssetInfo[assetId];
                                    dicAssetInfo.Remove(assetId);
                                }
                            }
                            if (!string.IsNullOrEmpty(assetId))
                            {
                                //执行
                                bool result = this.CheckAssetInfo(assetId, dbNumber, new RedisHelperSpecial(this.loadAppSettings.EndDbNumber));
                                lock (this.objLock)
                                {
                                    this.UpdateTxbText(result ? this.txb_allAssetSuccess : this.txb_allAssetFail);
                                }
                            }
                        }
                    });
                    th.IsBackground = true;
                    th.Start();
                }
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (dicAssetInfo.Count > 0)
                        {
                            Thread.Sleep(1500);
                            continue;
                        }
                        break;
                    }
                    Thread.Sleep(2000);
                    this.btn_CheckAllAssetInfo.Enabled = true;
                    this.lbl_ShowAllAssetInfo.Text = "验证完所有资产数据";
                    //提示
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception ex)
            {
                this.btn_CheckAllAssetInfo.Enabled = true;
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"验证所有资产数据发生异常{ex.Message}");
            }
        }

        /// <summary>
        ///     验证所有的用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_checkAllUser_Click(object sender, EventArgs e)
        {
            try
            {
                //获取所有的AssetId
                List<string> userIds = RedisHelper.GetRedisUserInfos().Select(x => x.UserId).ToList();
                if (userIds.Count == 0)
                {
                    MessageBox.Show("该redis不存在用户数据");
                    return;
                }
                this.txb_checkUserFailed.Text = "0";
                this.txb_checkUserSuccess.Text = "0";
                this.btn_checkAllUser.Enabled = false;
                this.lbl_showMsg_User.Text = "正在开始验证所有的用户数据,请稍等.....";
                RedisHelperSpecial redisHelperSpecial = new RedisHelperSpecial(this.loadAppSettings.EndDbNumber);
                int i = 0;
                string oldPurchaseOrder = "OldPurchaseOrder";
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchUserInfoAzureTable);
                TableQuery<YemUserProductDto> queryYemProduct = new TableQuery<YemUserProductDto>()
                    .Where($"PartitionKey eq  '{oldPurchaseOrder}'  and IsLock eq false");
                List<YemUserProductDto> yemUserProductDtos = assertIdsTable.ExecuteQuery(queryYemProduct).Where(p => userIds.Contains(p.UserId)).ToList();
                Parallel.ForEach(userIds, userId =>
                {
                    bool result = this.CheckPurchaseOrderInfo(userId, this.loadAppSettings.DbNumber, yemUserProductDtos, redisHelperSpecial);
                    lock (this.objLock)
                    {
                        this.UpdateTxbText(result ? this.txb_checkUserSuccess : this.txb_checkUserFailed);
                        i = i + 1;
                    }
                });

                //for (int i = 0; i < 6; i++)
                //{
                //    Thread thread = new Thread(() =>
                //    {
                //        while (userIds.Count > 0)
                //        {
                //            string userId = string.Empty;
                //            lock (this.objLock)
                //            {
                //                if (userIds.Count > 0)
                //                {
                //                    userId = userIds[0];
                //                    userIds.Remove(userId);
                //                }
                //            }
                //            if (!string.IsNullOrEmpty(userId))
                //            {
                //                //执行
                //                bool result = this.CheckPurchaseOrderInfo(userId, this.loadAppSettings.DbNumber, redisHelperSpecial);
                //                lock (this.objLock)
                //                {
                //                    this.UpdateTxbText(result ? this.txb_checkUserSuccess : this.txb_checkUserFailed);
                //                }
                //            }
                //        }
                //    });
                //    thread.IsBackground = true;
                //    thread.Start();
                //}
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (i == userIds.Count)
                        {
                            break;
                        }
                        Thread.Sleep(2000);
                    }
                    Thread.Sleep(2000);
                    this.btn_checkAllUser.Enabled = true;
                    this.lbl_showMsg_User.Text = "验证完所有用户数据";
                    //提示
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.btn_checkAllUser.Enabled = true;
                this.lbl_showMsg_User.Text = "验证所有用户购买订单数据异常," + exception.Message;
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"验证所有用户数据发生异常{exception.Message}");
            }
        }

        private void btn_CheckAllUserInfo_Click(object sender, EventArgs e)
        {
            //验证所有的用户和资产信息
            try
            {
                //获取所有的asset信息
                string[] redisNumbers = ConfigurationManager.AppSettings["AllDbNumberNotExitEnd"].Split(',');
                Dictionary<string, int> dicUserInfo = new Dictionary<string, int>();
                List<string> userIds = new List<string>();
                for (int i = 0; i < redisNumbers.Length; i++)
                {
                    int number = Convert.ToInt32(redisNumbers[i]);
                    RedisHelperSpecial redisHelperSpecial = new RedisHelperSpecial(number);
                    List<string> oneRedisUserIds = redisHelperSpecial.GetRedisUserInfos().Select(a => a.UserId).ToList();
                    foreach (string t in oneRedisUserIds)
                    {
                        if (!dicUserInfo.ContainsKey(t))
                        {
                            userIds.Add(t);
                            dicUserInfo.Add(t, number);
                        }
                    }
                }
                //循环跑
                if (dicUserInfo.Count == 0)
                {
                    MessageBox.Show("redis不存在用户购买数据");
                }
                string oldPurchaseOrder = "OldPurchaseOrder";
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchUserInfoAzureTable);
                TableQuery<YemUserProductDto> queryYemProduct = new TableQuery<YemUserProductDto>()
                    .Where($"PartitionKey eq  '{oldPurchaseOrder}'  and IsLock eq false");
                List<YemUserProductDto> yemUserProductDtos = assertIdsTable.ExecuteQuery(queryYemProduct).Where(p => userIds.Contains(p.UserId)).ToList();

                this.lbl_ShowAllUserInfo.Text = "正在开始验证所有的用户数据,请稍等.....";
                this.btn_CheckAllUserInfo.Enabled = false;
                this.txb_allUserFail.Text = "0";
                this.txb_allUserSuccess.Text = "0";
                //跑数据
                for (int i = 0; i < 6; i++)
                {
                    Thread th = new Thread(() =>
                    {
                        while (dicUserInfo.Count > 0)
                        {
                            string userid = string.Empty;
                            int dbNumber = 0;
                            lock (this.objLock)
                            {
                                if (dicUserInfo.Count > 0)
                                {
                                    userid = dicUserInfo.Keys.First();
                                    dbNumber = dicUserInfo[userid];
                                    dicUserInfo.Remove(userid);
                                }
                            }
                            if (!string.IsNullOrEmpty(userid))
                            {
                                //执行
                                bool result = this.CheckPurchaseOrderInfo(userid, dbNumber, yemUserProductDtos, new RedisHelperSpecial(this.loadAppSettings.EndDbNumber));
                                lock (this.objLock)
                                {
                                    this.UpdateTxbText(result ? this.txb_allUserSuccess : this.txb_allUserFail);
                                }
                            }
                        }
                    });
                    th.IsBackground = true;
                    th.Start();
                }
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (dicUserInfo.Count > 0)
                        {
                            Thread.Sleep(1500);
                            continue;
                        }
                        break;
                    }
                    Thread.Sleep(2000);
                    this.btn_CheckAllUserInfo.Enabled = true;
                    this.lbl_ShowAllUserInfo.Text = "验证完所有用户数据";
                    //提示
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception ex)
            {
                this.btn_CheckAllUserInfo.Enabled = true;
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"验证所有用户数据发生异常{ex.Message}");
            }
        }

        /// <summary>
        ///     根据资产id查询一个资产是否正确
        /// </summary>
        /// <param name="asseId"></param>
        /// <param name="reidsHelperSpecial"></param>
        /// <param name="dbNumber"></param>
        /// <returns></returns>
        private bool CheckAssetInfo(string asseId, int dbNumber, RedisHelperSpecial reidsHelperSpecial)
        {
            try
            {
                //获取Asset对象
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchAssetAzureTable);
                TableQuery<OnSellAssetDto> queryOnSellAsset = new TableQuery<OnSellAssetDto>()
                    .Where($"RowKey eq '{asseId}' and IsLock eq false"); //
                //queryPurchaseOrder = new TableQuery<YemUserProductDto>()
                //    .Where($"PartitionKey eq '{yemUserPurchase}' and RemainingAmount gt 0 and IsLock eq false");
                OnSellAssetDto onSellAssetDto = assertIdsTable.ExecuteQuery(queryOnSellAsset).FirstOrDefault();
                //获取融资金额以及剩余金额
                if (onSellAssetDto == null)
                {
                    //记录下来
                    Logger.LoadData(@"CheckOneRedisUserAssetInfo\ErrorInfo.txt", $"{asseId}:根据assetid拉取AzureTable数据异常" + DateTime.UtcNow.ToChinaStandardTime());
                    return false;
                }
                long calculatedAmount = onSellAssetDto.PresentValue;
                long remainAmount = onSellAssetDto.RemainderTotal;
                //再从redis中拉出所有的资产的下对应
                List<UserAssetRatio> listAssetUserInfos = new RedisHelperSpecial(dbNumber).GetRedisAssetUserRatiosAsync(asseId).ToList();
                long sumSellAmount = 0;
                if (listAssetUserInfos.Count > 0)
                {
                    sumSellAmount += listAssetUserInfos.Sum(info => info.Capital);
                }
                //从收尾那边拉出所有的资产比例
                List<UserAssetRatio> listEndAssetUserInfos = reidsHelperSpecial.GetRedisAssetUserRatiosAsync(asseId).ToList();
                if (listEndAssetUserInfos.Count > 0)
                {
                    sumSellAmount += listEndAssetUserInfos.Sum(info => info.Capital);
                }
                //List<UserAssetRatio>
                if (calculatedAmount - sumSellAmount != remainAmount)
                {
                    //记录下来
                    Logger.LoadData(@"CheckOneRedisUserAssetInfo\ErrorInfo.txt", $"{asseId}:该资产剩余金额不等于该资产总融资金额减去该资产下用户购买金额之和" + DateTime.UtcNow.ToChinaStandardTime());
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"{asseId}:该资产发生异常{e.Message}");
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckOneRedisUserAssetInfoForm_Load(object sender, EventArgs e)
        {
            //Thread thShow = new Thread(() =>
            //{
            //    if (this.isSuccessAsset)
            //    {
            //        this.btn_checkAllAsset.Enabled = true;
            //        this.lbl_showmsg.Text = "验证完所有资产数据，请查看日志信息是否存在";
            //    }
            //    if (this.isUserSuccess)
            //    {
            //        Thread.Sleep(2000);
            //    }

            //    Thread.Sleep(2500);
            //    //提示
            //    MessageBox.Show("执行完毕！");
            //});
            //thShow.IsBackground = true;
            //thShow.Start();
        }

        /// <summary>
        ///     reload
        /// </summary>
        /// <param name="assetId"></param>
        private bool CheckOneTableAssetInfo(string assetId)
        {
            try
            {
                //获取Asset对象
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchAssetAzureTable);
                TableQuery<OnSellAssetDto> queryOnSellAsset = new TableQuery<OnSellAssetDto>()
                    .Where($"RowKey eq '{assetId}' and IsLock eq false"); //
                OnSellAssetDto onSellAssetDto = assertIdsTable.ExecuteQuery(queryOnSellAsset).FirstOrDefault();
                //获取融资金额以及剩余金额
                if (onSellAssetDto == null)
                {
                    //记录下来
                    Logger.LoadData(@"CheckOneRedisUserAssetInfo\ErrorInfo.txt", $"{assetId}:根据assetid拉取AzureTable数据异常" + DateTime.UtcNow.ToChinaStandardTime());
                    return false;
                }
                //获取所有的该资产的资产用户比例
                CloudTable assetUserTable = this.tableClient.GetTableReference(this.loadAppSettings.WriteAssetUserTableName);
                TableQuery<UserAssetRatio> queryAssetUser = new TableQuery<UserAssetRatio>()
                    .Where($"PartitionKey eq  '{assetId}' and IsDeleted eq false");
                long sellAmount = assetUserTable.ExecuteQuery(queryAssetUser).Sum(x => x.Capital);
                if (onSellAssetDto.PresentValue - onSellAssetDto.RemainderTotal != sellAmount)
                {
                    //记录下来
                    Logger.LoadData(@"CheckOneRedisUserAssetInfo\ErrorInfo.txt", $"{assetId}:yuntable中该资产现有总价值不等于剩余金额与卖出金额之和" + DateTime.UtcNow.ToChinaStandardTime());
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"{assetId}:该资产发生异常{e.Message}---" + DateTime.UtcNow.ToChinaStandardTime());
                return false;
            }
        }

        /// <summary>
        ///     reload
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="yemUserProductDtos"></param>
        private bool CheckOneTableUserInfo(string userId, List<YemUserProductDto> yemUserProductDtos)
        {
            try
            {
                //获取购买订单对象
                YemUserProductDto yemUserProductDto = yemUserProductDtos.FirstOrDefault(p => p.UserId == userId);
                //获取融资金额以及剩余金额
                if (yemUserProductDto == null)
                {
                    //记录下来 待修改
                    Logger.LoadData(@"CheckOneRedisUserAssetInfo\ErrorInfo.txt", $"{userId}:根据assetid拉取AzureTable数据异常" + DateTime.UtcNow.ToChinaStandardTime());
                    return false;
                }
                //购买-剩余
                long waitAllot = yemUserProductDto.PurchaseMoney - yemUserProductDto.RemainingAmount;
                //用户资产比例
                CloudTable assetUserTable = this.tableClient.GetTableReference(this.loadAppSettings.WriteUserAssetTableName);
                TableQuery<UserAssetRatio> queryAssetUser = new TableQuery<UserAssetRatio>()
                    .Where($"PartitionKey eq  '{userId}' and IsDeleted eq false");
                long sellAmount = assetUserTable.ExecuteQuery(queryAssetUser).Sum(x => x.Numerator);
                if (waitAllot != sellAmount)
                {
                    //记录下来
                    Logger.LoadData(@"CheckOneRedisUserAssetInfo\ErrorInfo.txt", $"{userId}:yuntable中该用户订单总购买金额不等于剩余购买金额加上已经分配金额" + DateTime.UtcNow.ToChinaStandardTime());
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"{userId}:该用户订单发生异常{e.Message}---" + DateTime.UtcNow.ToChinaStandardTime());
                return false;
            }
        }

        /// <summary>
        ///     根据用户id查看用户购买订单是否正确
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dbNumber"></param>
        /// <param name="redisHelperSpecial"></param>
        /// <returns></returns>
        private bool CheckPurchaseOrderInfo(string userId, int dbNumber, List<YemUserProductDto> yemUserProductDtos, RedisHelperSpecial redisHelperSpecial)
        {
            try
            {
                //获取Asset对象
                //string oldPurchaseOrder = "OldPurchaseOrder";
                //CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchUserInfoAzureTable);
                //TableQuery<YemUserProductDto> queryYemProduct = new TableQuery<YemUserProductDto>()
                //    .Where($"PartitionKey eq  '{oldPurchaseOrder}'  and IsLock eq false and UserId eq '{userId}'");
                //YemUserProductDto yemUserProductDto = assertIdsTable.ExecuteQuery(queryYemProduct).FirstOrDefault();
                YemUserProductDto yemUserProductDto = yemUserProductDtos.FirstOrDefault(p => p.UserId == userId);
                //获取融资金额以及剩余金额
                if (yemUserProductDto == null)
                {
                    //记录下来
                    Logger.LoadData(@"CheckOneRedisUserAssetInfo\ErrorInfoUserInfo.txt", $"{userId}:根据userId拉取AzureTable数据异常---" + DateTime.UtcNow.ToChinaStandardTime());
                    return false;
                }
                long purchaseAmount = yemUserProductDto.PurchaseMoney;
                long remainingAmount = yemUserProductDto.RemainingAmount;
                //再从reedis中拉出所有的资产的下对应
                List<UserAssetRatio> listUserAssetInfos = new RedisHelperSpecial(dbNumber).GetRedisUserAssetRatiosAsync(userId).ToList();
                long sumPurchaseAmount = 0;
                if (listUserAssetInfos.Count > 0)
                {
                    sumPurchaseAmount += listUserAssetInfos.Sum(info => info.Capital);
                }
                //最后的节点
                List<UserAssetRatio> listEndUserAssetinfos = redisHelperSpecial.GetRedisUserAssetRatiosAsync(userId).ToList();
                if (listEndUserAssetinfos.Count > 0)
                {
                    sumPurchaseAmount += listEndUserAssetinfos.Sum(info => info.Capital);
                }
                if (purchaseAmount - sumPurchaseAmount != remainingAmount)
                {
                    //记录下来
                    Logger.LoadData(@"CheckOneRedisUserAssetInfo\ErrorInfoUserInfo.txt", $"{userId}:该用户购买订单总购买金额减去所有本金不等于剩余金额" + DateTime.UtcNow.ToChinaStandardTime());
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"CheckOneRedisUserAssetInfo\Error.txt", $"{userId}:该资产发生异常{e.Message}");
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="txb"></param>
        private void UpdateTxbText(TextBox txb)
        {
            txb.Text = (Convert.ToInt32(txb.Text) + 1).ToString();
        }
    }
}