using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class GetJsonForm : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;

        public GetJsonForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            CheckForIllegalCrossThreadCalls = false;
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromSeconds(180);
            this.InitializeComponent();
        }

        /// <summary>
        ///     批量执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_AllDebug_Click(object sender, EventArgs e)
        {
            //获取list数据
            try
            {
                string deptInfo = File.ReadAllText("AllDeptIds.txt");
                //转Json
                if (string.IsNullOrEmpty(deptInfo))
                {
                    MessageBox.Show("配置文件中没有数据");
                    return;
                }
                List<SubInvestOrderInputDto> listDepts = deptInfo.FromJson<List<SubInvestOrderInputDto>>();
                List<string> deptIds = listDepts.Select(x => x.DebtToTransferId).ToList();

                this.btn_AllDebug.Enabled = false;
                this.lbl_showMsg.Text = "正在执行.........";
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                    .Where("");
                List<DeptModel> deptModels = deptTable.ExecuteQuery(queryDeptInfo).Where(x => deptIds.Contains(x.DebtToTransferId)).ToList();
                for (int i = 0; i < deptModels.Count; i++)
                {
                    DeptModel deptModel = deptModels[i];
                    int comfirmType = this.ck_1.Checked ? 1 : 2;
                    //查询table
                    //拼接数据
                    //申购成功通知交易系统
                    var purchaseData = new
                    {
                        allotAmount = deptModel.Amount + deptModel.Interest,
                        isReturnToAccount = false,
                        resultTime = deptModel.CreatedTime,
                        userIdentifier = deptModel.NewUserId,
                        yemAssetRecordIdentifier = deptModel.NewUserAssetRatioId,
                        yemOrderIdentifier = deptModel.BuyOrderId
                    };
                    string jsonPurchaseData = purchaseData.ToJson();
                    var redeptionData = new
                    {
                        amount = deptModel.Amount + deptModel.Interest,
                        resultTime = deptModel.CreatedTime,
                        userIdentifier = deptModel.OldUserId,
                        yemAssetRecordIdentifier = deptModel.DebtToTransferId,
                        yemOrderIdentifier = deptModel.RansomOrderId,
                        systemAssetRecordIdentifier = Guid.Empty.ToGuidString(),
                        confirmType = comfirmType
                    };
                    string jsonRedemJson = redeptionData.ToJson();
                    bool oneResult = true;
                    //执行
                    for (int j = 0; j < 5; j++)
                    {
                        Tuple<bool, string> resultPurchase = await this.PurchaseSendToTrader(jsonPurchaseData.FromJson<TifisfalPurchaseRequestModel>());
                        if (resultPurchase.Item1)
                        {
                            break;
                        }
                        if (j == 4)
                        {
                            oneResult = false;
                            Logger.LoadData(@"GetJson\Error1DeptIds_" + deptModel.DebtToTransferId + ".txt", deptModel.DebtToTransferId);
                        }
                    }
                    //执行
                    for (int kIndex = 0; kIndex < 5; kIndex++)
                    {
                        Tuple<bool, string> resultRe = await this.RedemptionSendToTrader(jsonRedemJson.FromJson<TirisfalUserRedemptionInfoModel>());
                        if (resultRe.Item1)
                        {
                            break;
                        }
                        if (kIndex == 4)
                        {
                            oneResult = false;
                            //失败
                            Logger.LoadData(@"GetJson\Error2DeptIds_" + deptModel.DebtToTransferId + ".txt", deptModel.DebtToTransferId);
                        }
                    }
                    if (oneResult)
                    {
                        this.txb_S1.Text = (Convert.ToInt32(this.txb_S1.Text) + 1).ToString();
                    }
                    else
                    {
                        this.txb_F1.Text = (Convert.ToInt32(this.txb_F1.Text) + 1).ToString();
                    }
                }
                this.btn_AllDebug.Enabled = true;
                this.lbl_showMsg.Text = "执行完毕";
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            this.txb_purchaseNotifyJson.Text = "";
        }

        private void btn_GetJsonData_Click(object sender, EventArgs e)
        {
            try
            {
                string newUserAssetRatioId = this.txb_NewUserAseetRatioId.Text.Trim();
                string debtToTransferId = this.txb_DebtToTransferId.Text.Trim();
                string ransomOrderId = this.txb_RansomOrderId.Text.Trim();
                StringBuilder sb = new StringBuilder();
                bool hasPartitionKey = false;
                if (!string.IsNullOrEmpty(ransomOrderId))
                {
                    hasPartitionKey = true;
                    sb.Append($"PartitionKey eq 'DebtToTransfer_{ransomOrderId}'");
                }
                bool hasDeptTransferId = false;
                if (!string.IsNullOrEmpty(debtToTransferId))
                {
                    hasDeptTransferId = true;
                    sb.Append(hasPartitionKey ? $" and DebtToTransferId eq '{debtToTransferId}'" : $"DebtToTransferId eq '{debtToTransferId}'");
                }
                if (!hasDeptTransferId)
                {
                    if (!string.IsNullOrEmpty(newUserAssetRatioId))
                    {
                        sb.Append(hasPartitionKey ? $" and NewUserAssetRatioId eq '{newUserAssetRatioId}'" : $"NewUserAssetRatioId eq '{newUserAssetRatioId}'");
                    }
                }
                int comfirmType = this.ck_1.Checked ? 1 : 2;
                //查询table
                StringBuilder sbText = new StringBuilder();
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                    .Where(sb.ToString());
                DeptModel deptModel = deptTable.ExecuteQuery(queryDeptInfo).FirstOrDefault();
                //拼接数据
                if (deptModel != null)
                {
                    //申购成功通知交易系统
                    var purchaseData = new
                    {
                        allotAmount = deptModel.Amount + deptModel.Interest,
                        isReturnToAccount = false,
                        resultTime = deptModel.CreatedTime,
                        userIdentifier = deptModel.NewUserId,
                        yemAssetRecordIdentifier = deptModel.NewUserAssetRatioId,
                        yemOrderIdentifier = deptModel.BuyOrderId
                    };
                    string jsonPurchaseData = purchaseData.ToJson();
                    this.txb_purchaseNotifyJson.Text = jsonPurchaseData;
                    var redeptionData = new
                    {
                        amount = deptModel.Amount + deptModel.Interest,
                        resultTime = deptModel.CreatedTime,
                        userIdentifier = deptModel.OldUserId,
                        yemAssetRecordIdentifier = deptModel.DebtToTransferId,
                        yemOrderIdentifier = deptModel.RansomOrderId,
                        systemAssetRecordIdentifier = Guid.Empty.ToGuidString(),
                        confirmType = comfirmType
                    };
                    string jsonredeemData = redeptionData.ToJson();
                    this.txb_redeemNotifyJson.Text = jsonredeemData;
                    //MessageBox.Show("获取成功");
                }
                else
                {
                    MessageBox.Show("该debtToTransferId下面没有数据");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        ///     购买成功通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_purchaseNotify_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txb_purchaseNotifyJson.Text.Trim()))
            {
                Tuple<bool, string> result = await this.PurchaseSendToTrader(this.txb_purchaseNotifyJson.Text.Trim().FromJson<TifisfalPurchaseRequestModel>());
                MessageBox.Show(result.Item2);
            }
            else
            {
                MessageBox.Show("请先获取jsondata");
            }
        }

        /// <summary>
        ///     赎回通知交易系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_redeemNotify_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txb_redeemNotifyJson.Text.Trim()))
            {
                Tuple<bool, string> result = await this.RedemptionSendToTrader(this.txb_redeemNotifyJson.Text.Trim().FromJson<TirisfalUserRedemptionInfoModel>());
                MessageBox.Show(result.Item2);
            }
            else
            {
                MessageBox.Show("请先获取jsondata");
            }
        }

        //购买债转通知交易系统
        private async Task<Tuple<bool, string>> PurchaseSendToTrader(TifisfalPurchaseRequestModel request)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.AssetApiUrl}MessageToTifisfal/PurchaseSendToTrader", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return new Tuple<bool, string>(true, "成功");
                }
                return new Tuple<bool, string>(false, response.Message);
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"GetJson\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        //赎回通知交易系统/MessageToTifisfal/RedemptionSendToTrader
        private async Task<Tuple<bool, string>> RedemptionSendToTrader(TirisfalUserRedemptionInfoModel request)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.AssetApiUrl}MessageToTifisfal/RedemptionSendToTrader", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return new Tuple<bool, string>(true, "成功");
                }
                return new Tuple<bool, string>(false, response.Message);
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"GetJson\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }
    }
}