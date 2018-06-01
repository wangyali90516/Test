using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class NotifyTrisferSystemForm : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;

        public NotifyTrisferSystemForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            this.httpClient = new HttpClient();
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private void btn_HandleThings_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    this.txb_UserNums.Text = "0";
                    this.txb_SuccessNums.Text = "0";
                    List<string> userids = this.txb_handleUserIds.Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (userids.Count == 0)
                    {
                        MessageBox.Show("请输入userid");
                        return;
                    }
                    int count = 0;
                    this.btn_HandleThings.Enabled = false;
                    this.txb_UserNums.Text = (Convert.ToInt32(this.txb_UserNums.Text) + 1).ToString();
                    this.lbl_showmsg.Text = "正在执行.........";
                    int countDo = userids.Count;
                    //多线程通知
                    for (int i = 0; i < this.loadAppSettings.AssetThreadNums; i++)
                    {
                        Thread thStart = new Thread(async () =>
                        {
                            while (userids.Count > 0)
                            {
                                string userId = string.Empty;
                                lock (this.objLock)
                                {
                                    if (userids.Count > 0)
                                    {
                                        userId = userids[0];
                                        if (!String.IsNullOrEmpty(userId))
                                        {
                                            userids.Remove(userId);
                                        }
                                    }
                                }
                                //
                                if (!String.IsNullOrEmpty(userId))
                                {
                                    //执行
                                    string sql = $"select UserId,DebtToTransferId,(TotalAmount-TotalFee)as allotAmount,RedemptionOrderId from AssetAdvanceDebts where userid='{userId}'";
                                    DataTable dtAdvanceInfos = SqlHelper.SqlHelper.ExecuteDataTable(sql);
                                    for (int j = 0; j < dtAdvanceInfos.Rows.Count; j++)
                                    {
                                        DataRow dr = dtAdvanceInfos.Rows[j];
                                        string debtToTransferId = dr["DebtToTransferId"].ToString();
                                        long allocateAmount = Convert.ToInt64(dr["allotAmount"]);
                                        string redemptionId = dr["RedemptionOrderId"].ToString();
                                        bool isExitBankSuccess = false;
                                        //查询银行
                                        string sqlComfirm = $"select count(1) from AssetAdvanceDebts_Confirm where DebtToTransferId='{debtToTransferId}'";
                                        Object obj = SqlHelper.SqlHelper.ExecuteScalar(sqlComfirm);
                                        if (obj.ToString() == "0")
                                        {
                                            //判断银行
                                            bool isExitBank = await this.GetBatchordersearch(debtToTransferId);
                                            if (isExitBank)
                                            {
                                                isExitBankSuccess = true;
                                            }
                                        }
                                        else if (Convert.ToInt32(obj) > 0)
                                        {
                                            isExitBankSuccess = true;
                                        }
                                        if (isExitBankSuccess)
                                        {
                                            //通知交易
                                            //调用交易
                                            bool resultTrisfer = await this.NotifyTrisfer(allocateAmount, userId, debtToTransferId, redemptionId);
                                            if (resultTrisfer)
                                            {
                                                //成功
                                            }
                                        }
                                    }
                                    //执行完毕
                                    lock (this.objLock)
                                    {
                                        count = count + 1;
                                        this.txb_SuccessNums.Text = (Convert.ToInt32(this.txb_SuccessNums.Text) + 1).ToString();
                                    }
                                }
                            }
                        });
                        thStart.IsBackground = true;
                        thStart.Start();
                    }
                    while (countDo > count)
                    {
                        Thread.Sleep(5000);
                    }
                    //执行完毕
                    this.btn_HandleThings.Enabled = true;
                    this.lbl_showmsg.Text = "执行完毕......";
                }
                catch (Exception exception)
                {
                    this.btn_HandleThings.Enabled = true;
                    this.lbl_showmsg.Text = "发生一个或多个错误" + exception.Message;
                }
            });
        }

        private void btn_notifytrisfer_Click(object sender, EventArgs e)
        {
        }

        //http://jym-dev-api.jinyinmao.com.cn/YemInvesting/SetYemEndScale

        /// <summary>
        ///     获取网关数据
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetBatchordersearch(string orderId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.BankGatewayUrl}api/business/batchordersearch", new { orderId });
                BatchOrderSearchResponse response = await httpResponseMesage.Content.ReadAsAsync<BatchOrderSearchResponse>();
                if (response.Status == "S")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        /// <summary>
        ///     通知交易
        /// </summary>
        /// <returns></returns>
        private async Task<bool> NotifyTrisfer(long allotAmount, string userid, string debtToTransferId, string redemptionOrderId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                //{
                //    "allotAmount": 0,
                //    "isReturnToAccount": false,
                //    "resultTime": "2017-11-22T08:11:35.976Z",
                //    "userIdentifier": "string",
                //    "yemAssetRecordIdentifier": "string",
                //    "yemOrderIdentifier": "string"
                //}
                bool isReturnToAccount = false;
                DateTime resultTime = DateTime.UtcNow.ToChinaStandardTime();
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{ConfigurationManager.AppSettings["TrisferApiUrl"]}YemInvesting/SetYemEndScaleDeposit", new { allotAmount, isReturnToAccount, resultTime, userIdentifier = userid, yemAssetRecordIdentifier = debtToTransferId, yemOrderIdentifier = redemptionOrderId });
                //BatchOrderSearchResponse response = await httpResponseMesage.Content.ReadAsAsync<BatchOrderSearchResponse>();
                if (httpResponseMesage.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"NotifyT\Error.txt", ex.Message);
                return false;
            }
        }
    }
}