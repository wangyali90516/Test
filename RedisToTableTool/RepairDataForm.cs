using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;
using UserToAssetTool;

namespace RedisToTableTool
{
    public partial class RepairDataForm : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;

        private bool isCompleteDeleteAt;

        private bool isCompleteDeleteDb;

        public RepairDataForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromMinutes(10);
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        public async Task UpdateRedeemInfo()
        {
            CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
            TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                .Where("Status eq 0");
            List<DeptModel> listDeptInfos = deptTable.ExecuteQuery(queryDeptInfo).ToList();

            List<string> buyRedeemIds = listDeptInfos.Select(d => d.RansomOrderId).ToList();
            string oldPurchaseOrder = "RedeemOrder";
            CloudTable yemPurchaseOrderTable = this.tableClient.GetTableReference("AssetUserRedemptionOrders");
            TableQuery<UserRedemptionInfo> queryYemInfo = new TableQuery<UserRedemptionInfo>()
                .Where($"PartitionKey eq  '{oldPurchaseOrder}'");
            List<UserRedemptionInfo> listYemUserProductDtos = yemPurchaseOrderTable.ExecuteQuery(queryYemInfo).Where(p => buyRedeemIds.Contains(p.OrderId)).ToList();
            //然后再算RemainingAmount=RemainingAmount+Amount+Interst
            //循环
            int index = 0;
            foreach (UserRedemptionInfo item in listYemUserProductDtos)
            {
                DeptModel deptModel = listDeptInfos[index];
                long remainingAmount = item.RemainingAmount + deptModel.Interest + deptModel.Amount;
                //调接口
                //bool result = await this.UpdateYemRedeemInfo(item.UserId, item.OrderId, item.AlreadyDealtAmount, remainingAmount, 0, item.Status);
                //if (!result)
                //{
                //    Logger.LoadData(@"RepairData/UpdateYemRedeemInfo.txt", deptModel.RansomOrderId);
                //}
                index++;
            }
        }

        public async Task UpdateUserRatio()
        {
            int successCount = 0;
            int failedCount = 0;
            try
            {
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                    .Where("Status eq 0");
                List<DeptModel> listDeptInfos = deptTable.ExecuteQuery(queryDeptInfo).ToList();
                List<string> oldUserAssetUserIds = listDeptInfos.Select(d => d.OldUserId).ToList();
                List<string> oldUserAssetRatioIds = listDeptInfos.Select(d => d.OldUserAssetRatioId).ToList();
                CloudTable yemPurchaseOrderTable = this.tableClient.GetTableReference("AssetUserAssetRatios");
                int index = 0;
                oldUserAssetUserIds.ForEach(async userid =>
                {
                    TableQuery<UserAssetRatio> queryYemInfo = new TableQuery<UserAssetRatio>()
                        .Where($"PartitionKey eq  '{userid}' and UserAssetRatioId='{oldUserAssetRatioIds[index]}' ");

                    List<UserAssetRatio> listYemUserProductDtos = yemPurchaseOrderTable.ExecuteQuery(queryYemInfo).ToList();
                    //修改
                    listYemUserProductDtos.ForEach(async item =>
                    {
                        if (item.OriginalUserAssetRatioId == item.UserAssetRatioId && item.Status != 4)
                        {
                            item.Status = 4;
                        }
                        if (item.OriginalUserAssetRatioId != item.UserAssetRatioId && item.Status != 2)
                        {
                            item.Status = 2;
                        }
                        //
                        //调用接口
                        bool resultUpdate = await this.ModifyUserAssetRatiosByDisk(new ModifyUserAssetRatioRequest
                        {
                            AssetId = item.AssetId,
                            Capital = item.Capital,
                            Denominator = item.Denominator,
                            IsDeleted = true,
                            IsInvestSuccess = item.IsInvestSuccess,
                            IsNotifyTradingSuccess = item.IsNotifyTradingSuccess,
                            IsReturned = item.IsReturned,
                            Numerator = item.Numerator,
                            Reserve = item.Reserve,
                            Status = item.Status,
                            UserAssetRatioId = item.UserAssetRatioId,
                            UserId = item.UserId
                        });
                        if (resultUpdate)
                        {
                            successCount++;
                        }
                        else
                        {
                            failedCount++;
                        }
                    });
                    //bool result = await this.UpdateUserAssetRatio(this.tableClient, listYemUserProductDtos, "AssetUserAssetRatios", 100, userid);
                    index++;
                });
                MessageBox.Show($"成功{successCount}条,失败{failedCount}条,总共{oldUserAssetUserIds.Count}条", "", MessageBoxButtons.OKCancel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void btn_DeleteDatabase_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Reload_Click(object sender, EventArgs e)
        {
            try
            {
                //删除操作
                List<string> repairAssetIds = File.ReadAllLines("RepairAllAssetIds.txt").ToList();
                if (repairAssetIds.Count == 0)
                {
                    MessageBox.Show("没有需要修复的Assetid");
                    return;
                }
                //获取所有需要排除的uids
                List<string> removeUids = File.ReadAllLines("RemoveAllUids.txt").ToList();
                if (removeUids.Count == 0)
                {
                    MessageBox.Show("没有需要删除的Uids");
                    return;
                }
                //AssetUserRelation
                //根据所有的assetids从AssetUserRatio中获取所有的uids
                List<string> orderIds = new List<string>();
                List<string> allUserIds = removeUids;
                //int failueTimes = 0;this.GetUserIdsFromAssetUserRatio1(assetId, removeUids).ToList();
                this.txb_DeleteUserSuccess.Text = "0";
                this.txb_DeleteDatabaseUserF.Text = "0";
                this.txb_DeleteUserFailed.Text = "0";
                this.txb_DeleteDatabaseUserS.Text = "0";
                this.lbl_showMsg.Text = "正在准备数据......";
                foreach (string assetId in repairAssetIds)
                {
                    //获取
                    List<UserAssetRatio> listGetInfos = this.GetUserIdsFromAssetUserRatio1(assetId, removeUids).ToList();
                    //修改比例关系
                    if (listGetInfos.Count == 0)
                    {
                        //failueTimes = failueTimes + 1;
                        //记录错误的Uids
                        Logger.LoadData(@"RepairData/ErrorAssetIds.txt", "获取资产用户比例数据中的UserIds异常");
                        continue;
                    }
                    List<string> userIds = listGetInfos.Select(u => u.UserId).ToList();
                    //记录错误的Uids
                    allUserIds.AddRange(userIds);
                    //orderIds.AddRange(listGetInfos.Select(u => u.OrderId).ToList()); 33C4F593CB9A47B480164EAFE4AE1692  28D466916F6E48A3AEA6015AF0CA06C9
                }

                //判断是否全部跑完
                //if (failueTimes > 0)
                //{
                //    MessageBox.Show("获取的数据不全,存在没有拉去的数据");
                //}
                //然后再筛选 1.记录全部的uids 2.记录所有的已经排除后的uid
                //Logger.LoadData(@"RepairData/AllUserIds.txt", string.Join("\r\n", allUserIds));
                //allUserIds.RemoveAll(u => removeUids.Contains(u));
                //Logger.LoadData(@"RepairData/LeftUserIds.txt", string.Join(",", allUserIds));

                //删除数据 1.获取所有的需要删除的对象
                // List<YemUserProductDto>
                string oldPurchaseOrder = ConfigurationManager.AppSettings["PartitionKey"];
                string productIdentifier = ConfigurationManager.AppSettings["ProductIdentifer"];
                string dateTimeStr = ConfigurationManager.AppSettings["DeletePurchaseOrderTime"];
                DateTime orderTime = Convert.ToDateTime(dateTimeStr).ToChinaStandardTime();
                CloudTable yemUserPurchaseTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchUserInfoAzureTable); //购买订单表配置
                TableQuery<YemUserProductDto> queryYemProduct = new TableQuery<YemUserProductDto>()
                    .Where($"PartitionKey eq  '{oldPurchaseOrder}' and IsLock eq false");
                List<YemUserProductDto> yemUserProductDtos = yemUserPurchaseTable.ExecuteQuery(queryYemProduct).Where(p => allUserIds.Contains(p.UserId) && p.ProductIdentifier == productIdentifier && p.OrderTime > orderTime).ToList();
                this.btn_DeleteALl.Enabled = false;
                this.btn_SDelteAzureTUser.Enabled = false;
                this.lbl_showMsg.Text = "开始删除数据......";
                orderIds = yemUserProductDtos.Select(x => x.OrderId).ToList();
                Logger.LoadData(@"RepairData/LeftOrderIds.txt", string.Join("\r\n", orderIds));
                //2. 删除yuntable数据
                this.ParalleDeleteAtPurchaseOrder(yemUserProductDtos, "RepairData", this.txb_DeleteUserSuccess, this.txb_DeleteUserFailed);
                //3.删除数据库数据
                this.ParalleDeleteDbPurchaseOrder(orderIds, "RepairData", this.txb_DeleteDatabaseUserS, this.txb_DeleteDatabaseUserF);
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (!this.isCompleteDeleteDb || !this.isCompleteDeleteDb)
                        {
                            Thread.Sleep(1500);
                        }
                        else
                        {
                            break;
                        }
                    }
                    Thread.Sleep(1500);
                    this.btn_DeleteALl.Enabled = true;
                    this.btn_SDelteAzureTUser.Enabled = true;
                    this.lbl_showMsg.Text = "全部删除完毕";
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception ex)
            {
                this.btn_DeleteALl.Enabled = true;
                this.btn_SDelteAzureTUser.Enabled = true;
                this.btn_SDelteAzureTUser.Enabled = true;
                this.lbl_showMsg.Text = "发生异常.......";
                Logger.LoadData(@"RepairData/Error.txt", ex.Message);
            }
        }

        private void btn_SDelteAzureTUser_Click(object sender, EventArgs e)
        {
            try
            {
                this.txb_DeleteUserSuccess.Text = "0";
                this.txb_DeleteDatabaseUserF.Text = "0";
                this.txb_DeleteUserFailed.Text = "0";
                this.txb_DeleteDatabaseUserS.Text = "0";
                this.lbl_showMsg.Text = "正在准备数据......";
                //获取未删除的数据 ErrorDeleteDbIds ErrorDeleteTableIds
                List<string> hasNotDeleteDbOrderIds = File.ReadAllLines("RepairData/ErrorDeleteDbIds.txt").ToList();
                List<string> hasNotDeleteAtOrderIds = File.ReadAllLines("RepairData/ErrorDeleteTableIds.txt").ToList(); //
                this.btn_DeleteALl.Enabled = false;
                this.btn_SDelteAzureTUser.Enabled = false;
                this.lbl_showMsg.Text = "开始删除数据......";
                if (hasNotDeleteDbOrderIds.Count > 0)
                {
                    //zhiixng
                    this.ParalleDeleteDbPurchaseOrder(hasNotDeleteDbOrderIds, "RepairData", this.txb_DeleteDatabaseUserS, this.txb_F4db);
                }
                if (hasNotDeleteAtOrderIds.Count > 0)
                {
                    string oldPurchaseOrder = ConfigurationManager.AppSettings["PartitionKey"];
                    string productIdentifier = ConfigurationManager.AppSettings["ProductIdentifer"];
                    CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchUserInfoAzureTable);
                    string dateTimeStr = ConfigurationManager.AppSettings["DeletePurchaseOrderTime"];
                    DateTime orderTime = Convert.ToDateTime(dateTimeStr).ToChinaStandardTime();
                    TableQuery<YemUserProductDto> queryYemProduct = new TableQuery<YemUserProductDto>()
                        .Where($"PartitionKey eq  '{oldPurchaseOrder}' and IsLock eq false");
                    List<YemUserProductDto> yemUserProductDtos = assertIdsTable.ExecuteQuery(queryYemProduct).Where(p => hasNotDeleteAtOrderIds.Contains(p.UserId) && p.ProductIdentifier == productIdentifier && p.OrderTime > orderTime).ToList();
                    this.ParalleDeleteAtPurchaseOrder(yemUserProductDtos, "RepairData", this.txb_DeleteDatabaseUserS, this.txb_DeleteDatabaseUserF);
                }
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (!this.isCompleteDeleteDb || !this.isCompleteDeleteDb)
                        {
                            Thread.Sleep(1500);
                        }
                        else
                        {
                            break;
                        }
                    }
                    Thread.Sleep(1500);
                    this.btn_DeleteALl.Enabled = true;
                    this.btn_SDelteAzureTUser.Enabled = true;
                    this.lbl_showMsg.Text = "删除操作完成";
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception ex)
            {
                this.btn_DeleteALl.Enabled = true;
                this.btn_SDelteAzureTUser.Enabled = true;
                this.lbl_showMsg.Text = "发生异常.......";
                Logger.LoadData(@"RepairData/Error.txt", ex.Message);
            }
        }

        /// <summary>
        ///     开始修复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_StartRepair_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_StartRepair.Enabled = false;
                this.txb_S1.Text = "0";
                this.txb_F1.Text = "0";
                List<string> hasUpdateIds = new List<string>();

                if (File.Exists("RepairDataPurchaseUpdate/HasUpdateYemorderids.txt"))
                {
                    hasUpdateIds = File.ReadAllLines("RepairDataPurchaseUpdate/HasUpdateYemorderids.txt").ToList();
                }
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                    .Where("Status eq 0");
                List<DeptModel> listDeptInfos = deptTable.ExecuteQuery(queryDeptInfo).OrderBy(x => x.BuyOrderId).ToList();

                List<string> buyOrderIds = listDeptInfos.Where(x => !hasUpdateIds.Contains(x.BuyOrderId)).Select(d => d.BuyOrderId).ToList();
                string oldPurchaseOrder = ConfigurationManager.AppSettings["PartitionKey"];
                CloudTable yemPurchaseOrderTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchUserInfoAzureTable);
                TableQuery<YemUserProductDto> queryYemInfo = new TableQuery<YemUserProductDto>()
                    .Where($"PartitionKey eq  '{oldPurchaseOrder}'");
                List<YemUserProductDto> yemUserProductDtos = yemPurchaseOrderTable.ExecuteQuery(queryYemInfo).Where(p => buyOrderIds.Contains(p.OrderId)).OrderBy(x => x.OrderId).ToList();
                //循环
                int i;
                for (i = 0; i < 6; i++)
                {
                    Thread th = new Thread(async () =>
                    {
                        while (yemUserProductDtos.Count > 0)
                        {
                            YemUserProductDto yemUserProductDto = null;
                            lock (this.objLock)
                            {
                                if (yemUserProductDtos.Count > 0)
                                {
                                    yemUserProductDto = yemUserProductDtos[0];
                                    if (yemUserProductDto != null)
                                    {
                                        yemUserProductDtos.Remove(yemUserProductDto);
                                    }
                                }
                            }
                            if (yemUserProductDto != null)
                            {
                                //查询
                                //TableQuery<DeptModel> queryDeptInfo1 = new TableQuery<DeptModel>()
                                //    .Where($"BuyOrderId eq '{yemUserProductDto.OrderId}'and Status eq 1");
                                //List<DeptModel> listdeptMatchInfos = deptTable.ExecuteQuery(queryDeptInfo1).ToList();
                                //long amount = listdeptMatchInfos.Sum(x => x.Amount + x.Interest);
                                long remainingAmount = yemUserProductDto.PurchaseMoney - yemUserProductDto.Allocated;

                                if (remainingAmount < 0)
                                {
                                    remainingAmount = 0;
                                }
                                //调接口
                                bool result = await this.UpdateYemInfo(yemUserProductDto.UserId, yemUserProductDto.OrderId, yemUserProductDto.Allocated, remainingAmount, yemUserProductDto.Status);
                                //解锁
                                bool lockResult = await this.LockPurchaseOrder(yemUserProductDto.OrderId);
                                //删除
                                lock (this.objLock)
                                {
                                    if (result && lockResult)
                                    {
                                        this.UpdateTxbText(this.txb_S1);
                                        Logger.LoadData(@"RepairDataPurchaseUpdate/HasUpdateYemorderids.txt", yemUserProductDto.OrderId);
                                    }
                                    else
                                    {
                                        this.UpdateTxbText(this.txb_F1);
                                        Logger.LoadData(!result ? @"RepairDataPurchaseUpdate/UpdateYemorderids.txt" : @"RepairDataPurchaseUpdate/UpdateYemorderidsLockFail.txt", yemUserProductDto.OrderId);
                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(1000);
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
                        if (yemUserProductDtos.Count > 0)
                        {
                            Thread.Sleep(2500);
                        }
                        else
                        {
                            break;
                        }
                    }
                    Thread.Sleep(3500);
                    this.btn_StartRepair.Enabled = true;
                    MessageBox.Show("更改购买数据成功");
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                Logger.LoadData(@"RepairDataPurchaseUpdate/Error.txt", exception.Message);
            }
        }

        /// <summary>
        ///     修改资产用户比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_UpdateAssetUserRatio_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_UpdateRatio.Enabled = false;
                this.txb_S3.Text = "0";
                this.txb_F3.Text = "0";
                List<string> hasUpdateIds = new List<string>();
                if (File.Exists(@"UpdateDataRatios\HasUpdateRationids.txt"))
                {
                    hasUpdateIds = File.ReadAllLines(@"UpdateDataRatios\HasUpdateRationids.txt").ToList();
                }
                //资产用户比例关系数据UpdateDebtToTransferids.txt
                List<string> debtToTransferIds = File.ReadAllLines(@"UpdateDataRatios\UpdateDebtToTransferids.txt").Distinct().ToList();
                StringBuilder sb = new StringBuilder();
                sb.Append("(");
                foreach (string debtId in debtToTransferIds)
                {
                    sb.Append($"'{debtId}',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")");
                string sql = $"select DebtToTransferId,OldUserAssetRatioId,OldUserId from AssetDebtToTransfers where DebtToTransferId in {sb}";
                DataTable dtDebtInfos = SqlHelper.SqlHelper.ExecuteDataTable(sql);
                //转成对象
                List<AssetDebtToTransferModel> listDeptInfos = new List<AssetDebtToTransferModel>();
                for (int i = 0; i < dtDebtInfos.Rows.Count; i++)
                {
                    AssetDebtToTransferModel assetDebtToTransferModel = new AssetDebtToTransferModel();
                    DataRow dr = dtDebtInfos.Rows[i];
                    assetDebtToTransferModel.DebtToTransferId = dr["DebtToTransferId"] == null ? null : dr["DebtToTransferId"].ToString();
                    assetDebtToTransferModel.OldUserId = dr["OldUserId"] == null ? null : dr["OldUserId"].ToString();
                    assetDebtToTransferModel.OldUserAssetRatioId = dr["OldUserAssetRatioId"] == null ? null : dr["OldUserAssetRatioId"].ToString();
                    listDeptInfos.Add(assetDebtToTransferModel);
                }
                CloudTable yemPurchaseOrderTable = this.tableClient.GetTableReference("AssetUserAssetRatios");
                //执行数据
                for (int i = 0; i < 6; i++)
                {
                    Thread th = new Thread(async () =>
                    {
                        while (listDeptInfos.Count > 0)
                        {
                            AssetDebtToTransferModel deptModel = null;
                            lock (this.objLock)
                            {
                                if (listDeptInfos.Count > 0)
                                {
                                    deptModel = listDeptInfos[0];
                                    if (deptModel != null)
                                    {
                                        listDeptInfos.Remove(deptModel);
                                    }
                                }
                            }
                            if (deptModel != null)
                            {
                                TableQuery<UserAssetRatio> queryYemInfo = new TableQuery<UserAssetRatio>() //UserAssetRatioId
                                    .Where($"PartitionKey eq  '{deptModel.OldUserId}' and UserAssetRatioId eq '{deptModel.OldUserAssetRatioId}'");
                                List<UserAssetRatio> listYemUserProductDtos = yemPurchaseOrderTable.ExecuteQuery(queryYemInfo).Where(x => !hasUpdateIds.Contains(x.UserAssetRatioId)).ToList();
                                foreach (UserAssetRatio item in listYemUserProductDtos)
                                {
                                    item.Status = 2;
                                    //if (item.OriginalUserAssetRatioId == item.UserAssetRatioId)
                                    //{
                                    //    item.Status = 4;
                                    //}
                                    //if (item.OriginalUserAssetRatioId != item.UserAssetRatioId)
                                    //{
                                    //    item.Status = 2;
                                    //}
                                    //
                                    //调用接口
                                    bool resultUpdate = await this.ModifyUserAssetRatiosByDisk(new ModifyUserAssetRatioRequest
                                    {
                                        AssetId = item.AssetId,
                                        Capital = item.Capital,
                                        Denominator = item.Denominator,
                                        IsDeleted = item.IsDeleted,
                                        IsInvestSuccess = item.IsInvestSuccess,
                                        IsNotifyTradingSuccess = item.IsNotifyTradingSuccess,
                                        IsReturned = item.IsReturned,
                                        Numerator = item.Numerator,
                                        Reserve = item.Reserve,
                                        Status = item.Status,
                                        UserAssetRatioId = item.UserAssetRatioId,
                                        UserId = item.UserId,
                                        OriginalUserAssetRatioId = item.OriginalUserAssetRatioId
                                    });
                                    //执行
                                    lock (this.objLock)
                                    {
                                        if (resultUpdate)
                                        {
                                            this.UpdateTxbText(this.txb_S3);
                                            Logger.LoadData(@"UpdateDataRatios/HasUpdateRationids.txt", item.UserAssetRatioId);
                                        }
                                        else
                                        {
                                            this.UpdateTxbText(this.txb_F3);
                                            Logger.LoadData(@"UpdateDataRatios/HasUpdateErrorRationids.txt", item.UserAssetRatioId);
                                        }
                                    }
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
                        if (listDeptInfos.Count > 0)
                        {
                            Thread.Sleep(1200);
                        }
                        else
                        {
                            break;
                        }
                    }
                    this.btn_UpdateRedeem.Enabled = true;
                    MessageBox.Show("更改资产用户关系比例完成");
                });
                Thread.Sleep(1500);
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.btn_UpdateRedeem.Enabled = true;
                MessageBox.Show(exception.Message);
            }
        }

        private void btn_UpdateRatios_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     根据赎回订单修改赎回数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_UpdateRedemption_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_UpdateRedeem.Enabled = false;
                this.txb_S2.Text = "0";
                this.txb_F2.Text = "0";
                List<string> hasUpdateIds = new List<string>();
                if (File.Exists("UpdateRedemptionByIds/HasUpdateRedeemids.txt"))
                {
                    hasUpdateIds = File.ReadAllLines("UpdateRedemptionByIds/HasUpdateRedeemids.txt").ToList();
                }
                List<string> ransomIds = File.ReadAllLines(@"UpdateRedemptionByIds\UpdateRansomIds.txt").ToList();
                //List<string> buyRedeemIds = listDeptInfos.Where(x => !hasUpdateIds.Contains(x.RansomOrderId)).Select(d => d.RansomOrderId).ToList();
                //获取所有的ransomOrderIds
                ransomIds.RemoveAll(x => hasUpdateIds.Contains(x));
                string oldPurchaseOrder = "RedeemOrder";
                CloudTable yemPurchaseOrderTable = this.tableClient.GetTableReference("AssetUserRedemptionOrders");
                TableQuery<UserRedemptionInfo> queryYemInfo = new TableQuery<UserRedemptionInfo>()
                    .Where($"PartitionKey eq  '{oldPurchaseOrder}'");
                List<UserRedemptionInfo> listRemDtos = yemPurchaseOrderTable.ExecuteQuery(queryYemInfo).Where(p => ransomIds.Contains(p.OrderId)).OrderBy(x => x.OrderId).ToList();
                //再执行数据
                for (int i = 0; i < 1; i++)
                {
                    Thread th = new Thread(async () =>
                    {
                        while (listRemDtos.Count > 0)
                        {
                            UserRedemptionInfo userRedemptionInfo = null;
                            lock (this.objLock)
                            {
                                if (listRemDtos.Count > 0)
                                {
                                    userRedemptionInfo = listRemDtos[0];
                                    if (userRedemptionInfo != null)
                                    {
                                        listRemDtos.Remove(userRedemptionInfo);
                                    }
                                }
                            }
                            if (userRedemptionInfo != null)
                            {
                                long remainingAmount = userRedemptionInfo.OughtAmount - userRedemptionInfo.AlreadyDealtAmount;
                                if (remainingAmount < 0)
                                {
                                    remainingAmount = 0;
                                }
                                if (remainingAmount == 0)
                                {
                                    userRedemptionInfo.Status = 1;
                                }

                                //调接口
                                bool result = await this.UpdateYemRedeemInfo(userRedemptionInfo.OughtAmount, userRedemptionInfo.UserId, userRedemptionInfo.OrderId, userRedemptionInfo.AlreadyDealtAmount, remainingAmount, 0, userRedemptionInfo.Status);
                                //bool lockReuslt = await this.LockRedemptionOrder(userRedemptionInfo.OrderId);
                                //删除
                                lock (this.objLock)
                                {
                                    if (result)
                                    {
                                        //成功
                                        this.UpdateTxbText(this.txb_S2);
                                        Logger.LoadData(@"UpdateRedemptionByIds/HasUpdateRedeemids.txt", userRedemptionInfo.OrderId);
                                    }
                                    else
                                    {
                                        this.UpdateTxbText(this.txb_F2);
                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(3000);
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
                        if (listRemDtos.Count > 0)
                        {
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            break;
                        }
                    }
                    this.btn_UpdateRedeem.Enabled = true;
                    MessageBox.Show("更改赎回数据成功");
                });
                Thread.Sleep(2500);
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.btn_UpdateRedeem.Enabled = true;
                Logger.LoadData(@"UpdateRedemptionByIds/Error.txt", exception.Message + "-------" + exception.StackTrace);
            }
        }

        /// <summary>
        ///     修改赎回数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_UpdateRedeem.Enabled = false;
                this.txb_S2.Text = "0";
                this.txb_F2.Text = "0";
                List<string> hasUpdateIds = new List<string>();
                if (File.Exists("RepairDataRedeemDataUpdate/HasUpdateRedeemids.txt"))
                {
                    hasUpdateIds = File.ReadAllLines("RepairDataRedeemDataUpdate/HasUpdateRedeemids.txt").ToList();
                }
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                    .Where("Status eq 0");
                List<DeptModel> listDeptInfos = deptTable.ExecuteQuery(queryDeptInfo).OrderBy(x => x.RansomOrderId).ToList();

                List<string> buyRedeemIds = listDeptInfos.Where(x => !hasUpdateIds.Contains(x.RansomOrderId)).Select(d => d.RansomOrderId).ToList();
                string oldPurchaseOrder = "RedeemOrder";
                CloudTable yemPurchaseOrderTable = this.tableClient.GetTableReference("AssetUserRedemptionOrders");
                TableQuery<UserRedemptionInfo> queryYemInfo = new TableQuery<UserRedemptionInfo>()
                    .Where($"PartitionKey eq  '{oldPurchaseOrder}'");
                List<UserRedemptionInfo> listRemDtos = yemPurchaseOrderTable.ExecuteQuery(queryYemInfo).Where(p => buyRedeemIds.Contains(p.OrderId)).OrderBy(x => x.OrderId).ToList();
                //然后再算RemainingAmount=RemainingAmount+Amount+Interst
                //循环
                for (int i = 0; i < 1; i++)
                {
                    Thread th = new Thread(async () =>
                    {
                        while (listRemDtos.Count > 0)
                        {
                            UserRedemptionInfo userRedemptionInfo = null;
                            lock (this.objLock)
                            {
                                if (listRemDtos.Count > 0)
                                {
                                    userRedemptionInfo = listRemDtos[0];
                                    if (userRedemptionInfo != null)
                                    {
                                        listRemDtos.Remove(userRedemptionInfo);
                                    }
                                }
                            }
                            if (userRedemptionInfo != null)
                            {
                                //查询
                                //List<DeptModel> listdeptMatchInfos = listDeptInfos.Where(x => x.RansomOrderId == userRedemptionInfo.OrderId).ToList();
                                //去除status=1的数据
                                //TableQuery<DeptModel> queryDeptInfo1 = new TableQuery<DeptModel>()
                                //    .Where($"RansomOrderId eq '{userRedemptionInfo.OrderId}' and Status eq 1");
                                //List<DeptModel> listDeptInfos1 = deptTable.ExecuteQuery(queryDeptInfo1).ToList();
                                //List<string> buyRedeemIds = listDeptInfos.Where(x => !hasUpdateIds.Contains(x.RansomOrderId)).Select(d => d.RansomOrderId).ToList();
                                //if (listDeptInfos1.Count > 0)
                                //{
                                //    //记录日志
                                //    Logger.LoadData(@"RepairDataRedeemDataUpdate/json" + userRedemptionInfo.OrderId + ".txt", listDeptInfos1.ToJson());
                                //}
                                //long amount = listDeptInfos1.Sum(x => x.Amount + x.Interest);sum(amount+intest)
                                long remainingAmount = userRedemptionInfo.OughtAmount - userRedemptionInfo.AlreadyDealtAmount;
                                if (remainingAmount < 0)
                                {
                                    remainingAmount = 0;
                                }
                                //调接口
                                bool result = await this.UpdateYemRedeemInfo(userRedemptionInfo.OughtAmount, userRedemptionInfo.UserId, userRedemptionInfo.OrderId, userRedemptionInfo.AlreadyDealtAmount, remainingAmount, 0, userRedemptionInfo.Status);
                                bool lockReuslt = await this.LockRedemptionOrder(userRedemptionInfo.OrderId);
                                //删除
                                lock (this.objLock)
                                {
                                    if (result && lockReuslt)
                                    {
                                        //成功
                                        this.UpdateTxbText(this.txb_S2);
                                        Logger.LoadData(@"RepairDataRedeemDataUpdate/HasUpdateRedeemids.txt", userRedemptionInfo.OrderId);
                                    }
                                    else
                                    {
                                        this.UpdateTxbText(this.txb_F2);
                                        Logger.LoadData(!result ? @"RepairDataPurchaseUpdate/UpdateErrorRemorderids.txt" : @"RepairDataPurchaseUpdate/UpdateRemorderidsLockFail.txt", userRedemptionInfo.OrderId);
                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(3000);
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
                        if (listRemDtos.Count > 0)
                        {
                            Thread.Sleep(3000);
                        }
                        else
                        {
                            break;
                        }
                    }
                    this.btn_UpdateRedeem.Enabled = true;
                    MessageBox.Show("更改赎回数据成功");
                });
                Thread.Sleep(2500);
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.btn_UpdateRedeem.Enabled = true;
                Logger.LoadData(@"RepairDataRedeemDataUpdate/Error.txt", exception.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listDeptIds = File.ReadAllLines("RestoreDebtToTransferIds.txt").ToList();
                if (listDeptIds.Count == 0)
                {
                    MessageBox.Show("没有数据");
                    return;
                }
                this.btn_UpdateRatio.Enabled = false;
                this.txb_S3.Text = "0";
                this.txb_F3.Text = "0";
                List<string> hasUpdateIds = new List<string>();
                if (File.Exists("RepairDataRatiosUpdate/HasUpdateRationids.txt"))
                {
                    hasUpdateIds = File.ReadAllLines("RepairDataRatiosUpdate/HasUpdateRationids.txt").ToList();
                }
                //开始修复数据
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                    .Where("Status eq 0");
                List<DeptModel> listDeptInfos = deptTable.ExecuteQuery(queryDeptInfo).Where(x => listDeptIds.Contains(x.DebtToTransferId)).ToList();
                //List<string> oldUserIds = listDeptInfos.OrderBy(x => x.OldUserId).Select(d => d.OldUserId).ToList();
                //List<string> oldUserAssetRatioIds = listDeptInfos.OrderBy(x => x.OldUserId).Select(d => d.OldUserAssetRatioId).ToList();
                CloudTable yemPurchaseOrderTable = this.tableClient.GetTableReference("AssetUserAssetRatios");

                for (int i = 0; i < 6; i++)
                {
                    Thread th = new Thread(async () =>
                    {
                        while (listDeptInfos.Count > 0)
                        {
                            DeptModel deptModel = null;
                            lock (this.objLock)
                            {
                                if (listDeptInfos.Count > 0)
                                {
                                    deptModel = listDeptInfos[0];
                                    if (deptModel != null)
                                    {
                                        listDeptInfos.Remove(deptModel);
                                    }
                                }
                            }
                            if (deptModel != null)
                            {
                                TableQuery<UserAssetRatio> queryYemInfo = new TableQuery<UserAssetRatio>() //UserAssetRatioId
                                    .Where($"PartitionKey eq  '{deptModel.OldUserId}' and UserAssetRatioId eq '{deptModel.OldUserAssetRatioId}'");
                                List<UserAssetRatio> listYemUserProductDtos = yemPurchaseOrderTable.ExecuteQuery(queryYemInfo).Where(x => !hasUpdateIds.Contains(x.UserAssetRatioId)).ToList();
                                foreach (UserAssetRatio item in listYemUserProductDtos)
                                {
                                    if (item.IsDeleted)
                                    {
                                        item.IsDeleted = false;
                                    }
                                    if (item.OriginalUserAssetRatioId == item.UserAssetRatioId)
                                    {
                                        item.Status = 4;
                                    }
                                    if (item.OriginalUserAssetRatioId != item.UserAssetRatioId)
                                    {
                                        item.Status = 2;
                                    }
                                    //
                                    //调用接口
                                    bool resultUpdate = await this.ModifyUserAssetRatiosByDisk(new ModifyUserAssetRatioRequest
                                    {
                                        AssetId = item.AssetId,
                                        Capital = item.Capital,
                                        Denominator = item.Denominator,
                                        IsDeleted = true,
                                        IsInvestSuccess = item.IsInvestSuccess,
                                        IsNotifyTradingSuccess = item.IsNotifyTradingSuccess,
                                        IsReturned = item.IsReturned,
                                        Numerator = item.Numerator,
                                        Reserve = item.Reserve,
                                        Status = item.Status,
                                        UserAssetRatioId = item.UserAssetRatioId,
                                        UserId = item.UserId
                                    });
                                    //执行
                                    lock (this.objLock)
                                    {
                                        if (resultUpdate)
                                        {
                                            this.UpdateTxbText(this.txb_S3);
                                            Logger.LoadData(@"RepairDataRatiosUpdate/HasUpdateRationids.txt", item.UserAssetRatioId);
                                        }
                                        else
                                        {
                                            this.UpdateTxbText(this.txb_F3);
                                            Logger.LoadData(@"RepairDataRatiosUpdate/HasUpdateErrorRationids.txt", item.UserAssetRatioId);
                                        }
                                    }
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
                        if (listDeptInfos.Count > 0)
                        {
                            Thread.Sleep(1200);
                        }
                        else
                        {
                            break;
                        }
                    }
                    this.btn_UpdateRedeem.Enabled = true;
                    MessageBox.Show("更改资产用户关系比例完成");
                });
                Thread.Sleep(1500);
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.btn_UpdateRatio.Enabled = true;
                Logger.LoadData(@"RepairDataRatioUpdate/Error.txt", exception.Message);
            }
        }

        /// <summary>
        ///     修复比例数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //删除操作
                List<string> repairAssetIds = File.ReadAllLines("RepairAllAssetIds.txt").ToList();
                if (repairAssetIds.Count == 0)
                {
                    MessageBox.Show("没有需要修复的Assetid");
                    return;
                }
                //获取所有需要排除的uids
                List<string> removeUids = File.ReadAllLines("RepairRemoveUids.txt").ToList();
                if (removeUids.Count == 0)
                {
                    MessageBox.Show("没有需要排除的Uids");
                    return;
                }
                //根据所有的assetids从AssetUserRatio中获取所有的uids
                this.txb_DeleteUserSuccess.Text = "0";
                this.txb_DeleteDatabaseUserF.Text = "0";
                this.txb_DeleteUserFailed.Text = "0";
                this.txb_DeleteDatabaseUserS.Text = "0";
                this.lbl_showMsg.Text = "正在准备数据......";
                //多线程
                for (int i = 0; i < 1; i++)
                {
                    Thread th = new Thread(async () =>
                    {
                        while (repairAssetIds.Count > 0)
                        {
                            string assetId = string.Empty;
                            lock (this.objLock)
                            {
                                if (repairAssetIds.Count > 0)
                                {
                                    assetId = repairAssetIds[0];
                                    if (!string.IsNullOrEmpty(assetId))
                                    {
                                        repairAssetIds.Remove(assetId);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(assetId))
                            {
                                //获取
                                List<UserAssetRatio> listGetInfos = this.GetUserIdsFromAssetUserRatio(assetId, removeUids).ToList();
                                //修改比例关系
                                if (listGetInfos.Count == 0)
                                {
                                    //记录错误的Uids
                                    Logger.LoadData(@"RepairData/ErrorUpdateRatioAssetIds.txt", "获取资产用户比例数据中的UserIds异常");
                                    continue;
                                }
                                foreach (UserAssetRatio userAssetRatio in listGetInfos)
                                {
                                    //调用接口
                                    bool resultUpdate = await this.ModifyUserAssetRatiosByDisk(new ModifyUserAssetRatioRequest
                                    {
                                        AssetId = assetId,
                                        Capital = userAssetRatio.Capital,
                                        Denominator = userAssetRatio.Denominator,
                                        IsDeleted = true,
                                        IsInvestSuccess = userAssetRatio.IsInvestSuccess,
                                        IsNotifyTradingSuccess = userAssetRatio.IsNotifyTradingSuccess,
                                        IsReturned = userAssetRatio.IsReturned,
                                        Numerator = userAssetRatio.Numerator,
                                        Reserve = userAssetRatio.Reserve,
                                        Status = userAssetRatio.Status,
                                        UserAssetRatioId = userAssetRatio.UserAssetRatioId,
                                        UserId = userAssetRatio.UserId
                                    });
                                    lock (this.objLock)
                                    {
                                        if (!resultUpdate)
                                        {
                                            Logger.LoadData(@"RepairData/ErrorUpdateRatioAssetIds.txt", assetId);
                                            this.UpdateTxbText(this.txb_DeleteUserFailed);
                                        }
                                        else
                                        {
                                            this.UpdateTxbText(this.txb_DeleteUserSuccess);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Thread.Sleep(1000);
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
                        if (repairAssetIds.Count > 0)
                        {
                            Thread.Sleep(1500);
                        }
                        else
                        {
                            break;
                        }
                    }
                    Thread.Sleep(1500);
                    //MessageBox.Show($"成功{successCount}条,失败{failedCount}条,总共{count}条", "", MessageBoxButtons.OKCancel);
                    this.lbl_showMsg.Text = "全部更新完毕";
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception ex)
            {
                this.btn_DeleteALl.Enabled = true;
                this.btn_SDelteAzureTUser.Enabled = true;
                this.btn_SDelteAzureTUser.Enabled = true;
                this.lbl_showMsg.Text = "发生异常.......";
                Logger.LoadData(@"RepairData/Error.txt", ex.Message);
            }
        }

        /// <summary>
        ///     删除数据库中数据
        /// </summary>
        /// <returns></returns>
        private bool DeleteDatabasePurchaseProductInfos(string orderId)
        {
            try
            {
                SqlHelper.SqlHelper.ExecuteNoneQuery($"delete from YEMUserProducts where OrderId='{orderId}'", null);
                return true;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"RepairData/Error.txt", $"删除数据库中数据异常,orderid:{orderId}---" + e.Message);
                return false;
            }
        }

        /// <summary>
        ///     删除Azure Table中的用户购买订单
        /// </summary>
        /// <param name="yemUserProductDtos"></param>
        private bool DeletePurchaseProductsInfo(YemUserProductDto yemUserProductDtos)
        {
            try
            {
                //删除AzreTable
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchUserInfoAzureTable);
                //删除操作
                TableOperation retrieveOperation = TableOperation.Retrieve<YemUserProductDto>(yemUserProductDtos.PartitionKey, yemUserProductDtos.RowKey);
                TableResult retrievedResult = assertIdsTable.Execute(retrieveOperation);
                YemUserProductDto deleteEntity = (YemUserProductDto)retrievedResult.Result;
                if (deleteEntity != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                    assertIdsTable.Execute(deleteOperation);
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"RepairData/Error.txt", $"删除AzureTable中数据异常,orderid:{yemUserProductDtos.OrderId}---" + e.Message);
                return false;
            }
        }

        /// <summary>
        ///     获取所有的债转的用户ids
        /// </summary>
        private List<string> GetAllDeptUserIds()
        {
            try
            {
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.AssetDebtToTransfer);
                TableQuery<DeptModel> queryOnSellAsset = new TableQuery<DeptModel>()
                    .Where("IsDeleted eq false");
                List<string> oldUserIds = assertIdsTable.ExecuteQuery(queryOnSellAsset).Distinct().Select(x => x.OldUserId).ToList();
                return oldUserIds;
            }
            catch (Exception ex)
            {
                Logger.LoadData(@"RepairData/Error.txt", "获取债转的数据异常" + ex.Message);
                return new List<string>();
            }
        }

        /// <summary>
        /// </summary>
        private IEnumerable<UserAssetRatio> GetUserIdsFromAssetUserRatio(string assetId, List<string> removeList)
        {
            try
            {
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.WriteAssetUserTableName);
                TableQuery<UserAssetRatio> queryOnSellAsset = new TableQuery<UserAssetRatio>()
                    .Where($"PartitionKey eq  '{assetId}' and Status eq 4 and IsDeleted eq false");
                List<UserAssetRatio> allUserIds = assertIdsTable.ExecuteQuery(queryOnSellAsset).Distinct().Where(r => !removeList.Contains(r.UserId)).ToList();
                return allUserIds;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"RepairData/Error.txt", "获取比例关系中userids异常" + e.Message);
                return new List<UserAssetRatio>();
            }
        }

        private IEnumerable<UserAssetRatio> GetUserIdsFromAssetUserRatio1(string assetId, List<string> removeList)
        {
            try
            {
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.WriteAssetUserTableName);
                TableQuery<UserAssetRatio> queryOnSellAsset = new TableQuery<UserAssetRatio>()
                    .Where($"PartitionKey eq  '{assetId}'");
                List<UserAssetRatio> allUserIds = assertIdsTable.ExecuteQuery(queryOnSellAsset).Distinct().Where(r => removeList.Contains(r.UserId)).ToList();
                return allUserIds;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"RepairData/Error.txt", "获取比例关系中userids异常" + e.Message);
                return new List<UserAssetRatio>();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     购买订单上锁/解锁
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private async Task<bool> LockPurchaseOrder(string orderId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/LockPurchaseOrder?orderId={orderId}&locked=false", "");
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"RepairData\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        /// <summary>
        ///     赎回订单上锁/解锁
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private async Task<bool> LockRedemptionOrder(string orderId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/LockRedemptionOrder?orderId={orderId}&locked=false", "");
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"RepairData\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        /// <summary>
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
                Logger.LoadData(@"ModifyRatios\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            } //
        }

        /// <summary>
        ///     并行删除
        /// </summary>
        /// <param name="yemUserProductDtos"></param>
        /// <param name="typeName"></param>
        /// <param name="txbS"></param>
        /// <param name="txbF"></param>
        /// <returns></returns>
        private void ParalleDeleteAtPurchaseOrder(List<YemUserProductDto> yemUserProductDtos, string typeName, TextBox txbS, TextBox txbF)
        {
            //使用多线程并发执行
            for (int i = 0; i < this.loadAppSettings.AssetThreadNums; i++)
            {
                Thread th = new Thread(() =>
                {
                    while (yemUserProductDtos.Count > 0)
                    {
                        YemUserProductDto yemUserProductDto = null;
                        lock (this.objLock)
                        {
                            if (yemUserProductDtos.Count > 0)
                            {
                                yemUserProductDto = yemUserProductDtos[0];
                                if (yemUserProductDto != null)
                                {
                                    yemUserProductDtos.Remove(yemUserProductDto);
                                }
                            }
                        }
                        if (yemUserProductDto != null)
                        {
                            //删除
                            bool result = this.DeletePurchaseProductsInfo(yemUserProductDto);
                            lock (this.objLock)
                            {
                                if (result)
                                {
                                    this.UpdateTxbText(txbS);
                                    Logger.LoadData(@"" + typeName + "/SuccessTableIds.txt", yemUserProductDto.OrderId);
                                }
                                else
                                {
                                    this.UpdateTxbText(txbF);
                                    Logger.LoadData(@"" + typeName + "/ErrorDeleteTableIds.txt", yemUserProductDto.OrderId);
                                }
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    this.isCompleteDeleteAt = true;
                });
                th.IsBackground = true;
                th.Start();
            }
        }

        /// <summary>
        ///     删除数据
        /// </summary>
        /// <param name="orderIds"></param>
        /// <param name="typeName"></param>
        /// <param name="txbS"></param>
        /// <param name="txbF"></param>
        /// <returns></returns>
        private void ParalleDeleteDbPurchaseOrder(List<string> orderIds, string typeName, TextBox txbS, TextBox txbF)
        {
            //使用多线程并发执行
            for (int i = 0; i < 1; i++)
            {
                Thread th = new Thread(() =>
                {
                    while (orderIds.Count > 0)
                    {
                        string orderId = string.Empty;
                        lock (this.objLock)
                        {
                            if (orderIds.Count > 0)
                            {
                                orderId = orderIds[0];
                                if (!string.IsNullOrEmpty(orderId))
                                {
                                    orderIds.Remove(orderId);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(orderId))
                        {
                            //删除
                            bool result = this.DeleteDatabasePurchaseProductInfos(orderId);
                            lock (this.objLock)
                            {
                                if (result)
                                {
                                    this.UpdateTxbText(txbS);
                                    Logger.LoadData(@"" + typeName + "/SuccessDbIds.txt", orderId);
                                }
                                else
                                {
                                    this.UpdateTxbText(txbF);
                                    Logger.LoadData(@"" + typeName + "/ErrorDeleteDbIds.txt", orderId);
                                }
                            }
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                    }
                    this.isCompleteDeleteDb = true;
                });
                th.IsBackground = true;
                th.Start();
            }
        }

        private void RepairDataForm_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     删除购买数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void tbn_DeletePurchaseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                //删除购买订单 指定用户id
                //排除已经删除的orderids
                List<string> demandRemoveUserIds = File.ReadAllLines("RemovePurchaseUserIds.txt").ToList();
                if (demandRemoveUserIds.Count == 0)
                {
                    MessageBox.Show("需要删除的购买订单用户id数量为0........");
                    return;
                }
                //获取Azure Table数据 然后一并删除
                this.lbl_showMsgDelete.Text = "正在获取需要删除的购买订单信息.......";
                this.txb_S4.Text = "0";
                this.txb_F4.Text = "0";
                this.txb_S4db.Text = "0";
                this.txb_F4db.Text = "0";
                this.txb_totalDeletePurchaseNumber.Text = "0";
                this.btn_DeletePurchaseOrder.Enabled = false;
                string oldPurchaseOrder = ConfigurationManager.AppSettings["PartitionKey"];
                string productIdentifier = ConfigurationManager.AppSettings["ProductIdentifer"];
                string dateTimeStr = ConfigurationManager.AppSettings["DeletePurchaseOrderTime"];
                DateTime orderTime = Convert.ToDateTime(dateTimeStr).ToChinaStandardTime();
                CloudTable yemUserPurchaseTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchUserInfoAzureTable); //购买订单表配置
                TableQuery<YemUserProductDto> queryYemProduct = new TableQuery<YemUserProductDto>()
                    .Where($"PartitionKey eq  '{oldPurchaseOrder}' and IsLock eq false");
                List<YemUserProductDto> yemUserProductDtos = yemUserPurchaseTable.ExecuteQuery(queryYemProduct).Where(p => demandRemoveUserIds.Contains(p.UserId) && p.ProductIdentifier == productIdentifier && p.OrderTime > orderTime).ToList();
                if (yemUserProductDtos.Count == 0)
                {
                    this.btn_DeletePurchaseOrder.Enabled = true;
                    this.lbl_showMsgDelete.Text = "提示";
                    MessageBox.Show("在该用户列表id下未获取到需要删除的购买订单");
                    return;
                }
                List<string> orderIds = yemUserProductDtos.Select(x => x.OrderId).ToList();
                string saveOrderIds = string.Join("\r\n", orderIds);
                Logger.LoadData("RepairDataDeletePurchaseOrder/AllDemandOrderIds", saveOrderIds);
                this.txb_totalDeletePurchaseNumber.Text = yemUserProductDtos.Count.ToString();
                //开始删除
                this.ParalleDeleteAtPurchaseOrder(yemUserProductDtos, "RepairDataDeletePurchaseOrder", this.txb_S4, this.txb_F4); //txb_DemandDeleteDbNumber txb_F4db txb_S4Db
                //开始删除Database
                this.txb_DemandDeleteDbNumber.Text = orderIds.Count.ToString();
                this.ParalleDeleteDbPurchaseOrder(orderIds, "RepairDataDeletePurchaseOrder", this.txb_S4db, this.txb_F4db);
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (yemUserProductDtos.Count > 0)
                        {
                            Thread.Sleep(1500);
                        }
                        else
                        {
                            break;
                        }
                    }
                    Thread.Sleep(1500);
                    this.btn_DeletePurchaseOrder.Enabled = true;
                    this.lbl_showMsgDelete.Text = "删除操作结束.......";
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                Logger.LoadData(@"RepairDataDeletePurchaseOrder/Error.txt", exception.Message);
            }
        }

        private void txb_DeleteDatabaseUserF_TextChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="txb"></param>
        private void UpdateTxbText(TextBox txb)
        {
            txb.Text = (Convert.ToInt32(txb.Text) + 1).ToString();
        }

        /// <summary>
        ///     插入用户资产关系表
        /// </summary>
        /// <param name="tableClient1"></param>
        /// <param name="addUserAssetRatios"></param>
        /// <param name="writeUserAssetRatioAzureTable"></param>
        /// <param name="batchNum"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private async Task<bool> UpdateUserAssetRatio(CloudTableClient tableClient1, List<UserAssetRatio> addUserAssetRatios, string writeUserAssetRatioAzureTable, int batchNum, string orderId)
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
                            //UserAssetRatio insertModel = this.MapToTableUserAssetRatio(tableEntity, await RedisHelper.GetRedisUserInfoAsync(tableEntity.UserId), await RedisHelper.GetRedisAssetIdInfoAsync(tableEntity.AssetId), 0);
                            batch.Add(TableOperation.InsertOrReplace(tableEntity));
                        }
                        await userassetTable.ExecuteBatchAsync(batch);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"UserAssetRatio\Error.txt", $"修改UserAssetRatio，错误信息{e.Message}orderId为{orderId},{orderId}");
                Logger.LoadData(@"UserAssetRatio\Error.txt", $"{orderId}");
                return false;
            }
        }

        /// <summary>
        ///     更新比例关系
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="orderId"></param>
        /// <param name="allocated"></param>
        /// <param name="remainingAmount"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        private async Task<bool> UpdateYemInfo(string userid, string orderId, long allocated, long remainingAmount, int status)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                var modifyUserPurchaseOrderRequest = new { UserId = userid, OrderId = orderId, allocated, remainingAmount, status };
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/ModifyYemUserPurchaseByDisk", modifyUserPurchaseOrderRequest);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"UpdateYemInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            } //YEMAssetPool/ModifyUserAssetRatiosByDisk
        }

        private async Task<bool> UpdateYemRedeemInfo(long oughtAmount, string userid, string orderId, long alreadyDealtAmount, long remainingAmount, int serviceCharge, int status)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                var modifyUserRedeemRequest = new { oughtAmount, UserId = userid, OrderId = orderId, alreadyDealtAmount, remainingAmount, serviceCharge, status };
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/ModifyRedemptionOrderByDisk", modifyUserRedeemRequest);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"UpdateYemRedeemInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }
    }
}