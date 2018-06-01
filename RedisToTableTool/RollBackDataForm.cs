using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;
using UserToAssetTool;

namespace RedisToTableTool
{
    public partial class RollBackDataForm : Form
    {
        private readonly LoadAppSettings loadAppSettings;
        private readonly CloudTableClient tableClient;
        private readonly ToolStripButton toolStripButton;

        public RollBackDataForm(CloudTableClient tableClient, LoadAppSettings loadAppSettings, ToolStripButton toolStripButton)
        {
            this.tableClient = tableClient;
            this.loadAppSettings = loadAppSettings;
            this.toolStripButton = toolStripButton;
            this.InitializeComponent();
        }

        //购买订单还原
        public static async Task<bool> ResetPurchase(CloudTableClient tableClient, string writeYemUserProductAzureTable, YemUserProductDto yemUserProductDto, long currentDealPurchaseAmount)
        {
            try
            {
                YemUserProductDto resetPurchase = yemUserProductDto;
                resetPurchase.RemainingAmount += currentDealPurchaseAmount;
                resetPurchase.Allocated -= currentDealPurchaseAmount;
                resetPurchase.Status = resetPurchase.PurchaseMoney == resetPurchase.Allocated ? PurchaseOrderStatus.AllocationComplete.ToEnumInteger() : PurchaseOrderStatus.AllocationOn.ToEnumInteger();
                resetPurchase.WaitingBankBackAmount = 0;
                resetPurchase.UpdatedTime = DateTime.UtcNow.ToChinaStandardTime();
                await AddYemUserPurchase(tableClient, yemUserProductDto, writeYemUserProductAzureTable);
                return true;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"Rollback\Error.txt", e.Message + "----" + e.StackTrace);
                return false;
            }
        }

        //public static async Task RollerBack(CloudTableClient tableClient, string writeAssetAzureTable, string writeYemUserProductAzureTable, List<OnSellAssetDto> lastOnSellAssetDtos, List<UserAssetRatio> addUserAssetRatios, List<UserAssetRatio> addAssetUserRatios, YemUserProductDto yemUserProductDto, long currentDealPurchaseAmount, int batchNum)
        //{
        //    //资产还原
        //    await AddOnSellAsset(tableClient, lastOnSellAssetDtos, writeAssetAzureTable, batchNum); //lastOnSellAssetDtos为上一次的资产列表
        //    if (addUserAssetRatios.Any())
        //    {
        //        await RedisHelper.RemoveRedisUserAssetRatioAsync(addUserAssetRatios);
        //    }

        //        if (addUserAssetRatios.Any())
        //    {
        //        await RedisHelper.RemoveRedisAssetUserRatioAsync(addAssetUserRatios);
        //    }
        //}
        //    //订单还原
        //    await ResetPurchase(tableClient, writeYemUserProductAzureTable, yemUserProductDto, currentDealPurchaseAmount); //根据处理的金额还原
        //}

        /// <summary>
        ///     添加资产
        /// </summary>
        /// <param name="tableClient"></param>
        /// <param name="modifyOnSellAssetDtos"></param>
        /// <param name="writeAssetAzureTable"></param>
        /// <param name="batchNum"></param>
        /// <returns></returns>
        private static async Task<bool> AddOnSellAsset(CloudTableClient tableClient, List<OnSellAssetDto> modifyOnSellAssetDtos, string writeAssetAzureTable, int batchNum = 100)
        {
            try
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
                return true;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"Rollback\Error.txt", e.Message + "----" + e.StackTrace);
                return false;
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
        ///     回滚数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Rollback_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_Rollback.Enabled = false;
                int selectIndex = this.cbx_SelectItem.SelectedIndex;
                bool assetDemandRollback = this.ck_assetDtos.Checked;
                bool yemUserPurchaseRollback = this.ck_yemUserPurchase.Checked;
                bool userAssetRollback = this.ck_userAsset.Checked;
                bool assetUserRollback = this.ck_assetUser.Checked;
                List<UserAssetRatio> listUserAssetRatio = new List<UserAssetRatio>();
                List<UserAssetRatio> listAssetUserRatio = new List<UserAssetRatio>();
                List<RollBackDataModel> yemUserPurchase = new List<RollBackDataModel>();
                List<OnSellAssetDto> assetDtos = new List<OnSellAssetDto>();
                if (selectIndex == 1)
                {
                    //文本文件
                    if (assetDemandRollback)
                    {
                        string errorOldLastOnSellAsset = File.ReadAllText("logErrorOldLastOnSellAsset.txt");
                        assetDtos = errorOldLastOnSellAsset.FromJson<List<OnSellAssetDto>>();
                    }
                    if (yemUserPurchaseRollback)
                    {
                        string errorOldYemUserPurchase = File.ReadAllText("logErrorOldYemUserPurchase.txt");
                        yemUserPurchase.Add(errorOldYemUserPurchase.FromJson<RollBackDataModel>());
                    }
                    if (userAssetRollback)
                    {
                        string errorOldUserAssetRatio = File.ReadAllText("logErrorOldUserAssetRatio.txt");
                        listUserAssetRatio = errorOldUserAssetRatio.FromJson<List<UserAssetRatio>>();
                    }
                    if (assetUserRollback)
                    {
                        string errorOldAssetUserRatio = File.ReadAllText("logErrorOldAssetUserRatio.txt");
                        listAssetUserRatio = errorOldAssetUserRatio.FromJson<List<UserAssetRatio>>();
                    }
                }
                else
                {
                    //Redis
                    if (assetDemandRollback)
                    {
                        assetDtos = RedisHelper.GetRollbackAssetsData();
                    }
                    if (yemUserPurchaseRollback)
                    {
                        yemUserPurchase = RedisHelper.GetRollbackYemUserPurchaseData();
                    }
                    if (userAssetRollback)
                    {
                        listUserAssetRatio = RedisHelper.GetRollbackUserAssetData();
                    }
                    if (assetUserRollback)
                    {
                        listAssetUserRatio = RedisHelper.GetRollbackAssetUserData();
                    }
                }
                //转换后开始回滚
                //1.回滚资产
                if (assetDemandRollback)
                {
                    if (assetDtos.Count == 0)
                    {
                        MessageBox.Show("资产数据异常,请检查后再回滚");
                        return;
                    }
                    //开始回滚资产
                    this.lbl_assetDtsMsg.Text = "正在开始回滚资产数据......";
                    //资产还原
                    bool assetResult = await AddOnSellAsset(this.tableClient, assetDtos, this.loadAppSettings.SearchAssetAzureTable, this.loadAppSettings.BatchAssetNums);
                    this.lbl_assetDtsMsg.Text = assetResult ? "√更新资产数据成功" : "×更新资产数据失败";
                }
                if (yemUserPurchaseRollback)
                {
                    if (yemUserPurchase.Count != 1)
                    {
                        MessageBox.Show("用户余额猫订单数据异常,请检查后再回滚");
                        return;
                    }
                    //开始回滚资产
                    this.lbl_yemUserPurchaseMsg.Text = "正在开始回滚用户订单数据......";
                    //用户订单还原
                    bool assetResult = await ResetPurchase(this.tableClient, this.loadAppSettings.SearchUserInfoAzureTable, yemUserPurchase[0].YemUserProductDto, yemUserPurchase[0].CurrentDealPurchaseAmount);
                    this.lbl_yemUserPurchaseMsg.Text = assetResult ? "√更新用户订单数据成功" : "×更新用户订单数据失败";
                }
                if (userAssetRollback)
                {
                    //删除redis
                    //listAssetUserRatio
                    if (listUserAssetRatio.Count == 0)
                    {
                        MessageBox.Show("用户资产数据异常,请检查后再回滚");
                        return;
                    }
                    //开始回滚资产
                    this.lbl_userassetMsg.Text = "正在开始删除redis用户资产数据......";
                    //用户订单还原
                    bool assetResult = await RedisHelper.RemoveRedisUserAssetRatioAsync(listUserAssetRatio);
                    this.lbl_userassetMsg.Text = assetResult ? "√删除用户资产数据成功" : "×删除用户资产数据失败";
                }
                if (assetUserRollback)
                {
                    //删除redis
                    //listAssetUserRatio
                    if (listAssetUserRatio.Count == 0)
                    {
                        MessageBox.Show("资产用户数据异常,请检查后再回滚");
                        return;
                    }
                    //开始回滚资产
                    this.lbl_assetuserMsg.Text = "正在开始删除redis资产用户数据......";
                    //用户订单还原
                    bool assetResult = await RedisHelper.RemoveRedisAssetUserRatioAsync(listAssetUserRatio);
                    this.lbl_assetuserMsg.Text = assetResult ? "√删除资产用户数据成功" : "×删除资产用户数据失败";
                }
                //开始检查
                if (userAssetRollback)
                {
                    this.lbl_userAssetDelete.Text = "正在开始检查redis中用户资产关系是否删除......";
                    //检查
                    bool reusltCheck = await RedisHelper.CheckRedisUserAssetRatioAsync(listUserAssetRatio);
                    this.lbl_userAssetDelete.Text = reusltCheck ? "√的确删除了Redis中的用户资产数据" : "×√没有完全删除了Redis中的用户资产数据";
                }
                if (assetUserRollback)
                {
                    this.lbl_assetuserDelete.Text = "正在开始检查redis中资产用户关系是否删除......";
                    //检查
                    bool reusltCheck = await RedisHelper.CheckRedisAssetUserRatioAsync(listUserAssetRatio);
                    this.lbl_assetuserDelete.Text = reusltCheck ? "√的确删除了Redis中的资产用户数据" : "×√没有完全删除了Redis中的资产用户数据";
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void RollBackDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.toolStripButton.Enabled = true;
        }

        private void RollBackDataForm_Load(object sender, EventArgs e)
        {
            this.toolStripButton.Enabled = false;
            this.cbx_SelectItem.SelectedIndex = 0;
        }
    }
}