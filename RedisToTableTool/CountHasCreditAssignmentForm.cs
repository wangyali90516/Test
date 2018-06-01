using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class CountHasCreditAssignmentForm : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;

        public CountHasCreditAssignmentForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromSeconds(120);
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        /// <summary>
        ///     开始计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.lbl_showMsg.Text = "正在从赎回表中获取所有符合条件的订单......";
                //正在从赎回订单表中拉数据.......
                CloudTable deptTable = this.tableClient.GetTableReference("AssetUserRedemptionOrders");
                string partitionKey = "RedeemOrder";
                TableQuery<RedeemOrderInfo> queryDeptInfo = new TableQuery<RedeemOrderInfo>()
                    .Where($"PartitionKey eq '{partitionKey}' and AlreadyDealtAmount eq 0");
                List<RedeemOrderInfo> creditInfos = deptTable.ExecuteQuery(queryDeptInfo).ToList();
                //数据获取完毕,正在开始分析.......
                this.txb_AllNumbers.Text = creditInfos.Count.ToString();
                //ransomorderId
                if (creditInfos.Count == 0)
                {
                    MessageBox.Show("获取赎回订单数为0......");
                    return;
                }
                this.lbl_showMsg.Text = "获取赎回订单完毕,开始分析......";
                this.btn_StartComputer.Enabled = false;
                for (int i = 0; i < 6; i++)
                {
                    Thread th = new Thread(() =>
                    {
                        while (creditInfos.Count > 0)
                        {
                            string userid = string.Empty;
                            int dbNumber = 0;
                            lock (this.objLock)
                            {
                                if (creditInfos.Count > 0)
                                {
                                    //userid = dicUserInfo.Keys.First();
                                    //dbNumber = dicUserInfo[userid];
                                    //dicUserInfo.Remove(userid);
                                }
                            }
                            if (!string.IsNullOrEmpty(userid))
                            {
                                //执行
                                //bool result = this.CheckPurchaseOrderInfo(userid, dbNumber, yemUserProductDtos, new RedisHelperSpecial(this.loadAppSettings.EndDbNumber));
                                //lock (this.objLock)
                                //{
                                //    this.UpdateTxbText(result ? this.txb_allUserSuccess : this.txb_allUserFail);
                                //}
                            }
                        }
                    });
                    th.IsBackground = true;
                    th.Start();
                }
                Thread thShow = new Thread(() =>
                {
                    //while (true)
                    //{
                    //    if (dicUserInfo.Count > 0)
                    //    {
                    //        Thread.Sleep(1500);
                    //        continue;
                    //    }
                    //    break;
                    //}
                    //Thread.Sleep(2000);
                    //this.btn_CheckAllUserInfo.Enabled = true;
                    //this.lbl_ShowAllUserInfo.Text = "验证完所有用户数据";
                    //提示
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                //Console.WriteLine(exception);
                //throw;
            }
        }

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
                Logger.LoadData(@"CountHasCreditAssignment\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        /// <summary>
        ///     获取网关数据/api/business/batchordersearch
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetOrderSearch(string orderId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.BankGatewayUrl}api/business/ordersearch", new { orderId });
                OrderSearchResponse response = await httpResponseMesage.Content.ReadAsAsync<OrderSearchResponse>();
                if (response.Status == "S")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"CountHasCreditAssignment\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }
    }
}