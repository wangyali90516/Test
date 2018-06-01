using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class UpdateYemUserProductInfoForm : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;

        public UpdateYemUserProductInfoForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            this.httpClient = new HttpClient();
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private void btn_batchUpdate_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     批量修改用户购买表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_batchUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                List<string> userIds = this.txb_handleUserIds.Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                if (userIds.Count == 0)
                {
                    MessageBox.Show("请输入需要执行的asstids");
                    return;
                }
                this.lbl_showmsg.Text = "正在准备数据.......";
                this.btn_batchUpdate.Enabled = false;
                //购买订单数据
                foreach (string userId in userIds)
                {
                    this.txb_CurrentUserId.Text = userId;
                    List<YemUserProductInfo> listYemUserProductInfos = this.GetYemUserOrderInfo(userId);
                    //获取所有的数据
                    List<YemUserProductInfo> listYemUserProductInfosOne = listYemUserProductInfos.Where(x => x.RemainingAmount < 0).ToList();

                    foreach (YemUserProductInfo yemUserProductInfo in listYemUserProductInfosOne)
                    {
                        // Logger.LoadData(@"UpdateYemUserProductInfo\Error_" + userId + ".txt", new { yemUserProductInfo.OrderId, yemUserProductInfo.RemainingAmount }.ToJson());
                        long resetPurchaseAmount = Math.Abs(yemUserProductInfo.RemainingAmount);
                        //逐个处理
                        List<YemUserProductInfo> listYemUserProductInfosTwo = listYemUserProductInfos.Where(x => x.RemainingAmount > 0).ToList();
                        long dealAmount = 0;
                        foreach (YemUserProductInfo yemItem in listYemUserProductInfosTwo)
                        {
                            if (yemItem.RemainingAmount > 0)
                            {
                                if (resetPurchaseAmount == 0)
                                {
                                    break;
                                }
                                if (yemItem.RemainingAmount > resetPurchaseAmount)
                                {
                                    yemItem.RemainingAmount -= resetPurchaseAmount;
                                    dealAmount = resetPurchaseAmount;
                                    resetPurchaseAmount = 0;
                                }
                                else
                                {
                                    dealAmount = yemItem.RemainingAmount;
                                    resetPurchaseAmount -= yemItem.RemainingAmount;
                                    yemItem.RemainingAmount = 0;
                                }
                            }
                            TextPurchase txtPurchase = new TextPurchase
                            {
                                OldOrderId = yemUserProductInfo.OrderId,
                                NewOrderId = yemItem.OrderId,
                                Amout = dealAmount
                            };
                            Logger.LoadData(@"UpdateYemUserProductInfo\" + userId + ".txt", txtPurchase.ToJson());
                            string sqlStr = yemItem.RemainingAmount == 0 ? ",Status=1" : "";
                            string sql = "update YEMUserProducts_Copy set RemainingAmount -=" + dealAmount + " ,Allocated+=" + dealAmount + " " + sqlStr + " where OrderId='" + yemItem.OrderId + "';update YEMUserProducts_Copy set RemainingAmount+=" + dealAmount + ",Allocated-=" + dealAmount + "" + sqlStr + " where OrderId='" + yemUserProductInfo.OrderId + "'";
                            int dbResult = SqlHelper.SqlHelper.ExecuteNoneQuery(sql);
                            if (dbResult > 0)
                            {
                                TifisfalPurchaseRequestModel triTifisfalPurchaseRequestModel = new TifisfalPurchaseRequestModel
                                {
                                    AllotAmount = dealAmount,
                                    IsReturnToAccount = false,
                                    ResultTime = DateTime.UtcNow.ToChinaStandardTime(),
                                    UserIdentifier = yemItem.UserId,
                                    YemAssetRecordIdentifier = Guid.NewGuid().ToGuidString(),
                                    YemOrderIdentifier = yemItem.OrderId
                                };
                                var result = await this.PurchaseSendToTrader(triTifisfalPurchaseRequestModel);
                                if (!result.Item1)
                                {
                                    Logger.LoadData(@"UpdateTrisferYemUserProductInfo\Error_" + userId + ".txt", triTifisfalPurchaseRequestModel.ToJson());
                                }
                                this.txb_CurrentAssetIdNums.Text = (Convert.ToInt32(this.txb_CurrentAssetIdNums.Text) + 1).ToString();
                            }
                            else
                            {
                                Logger.LoadData(@"UpdateDbYemUserProductInfo\Error_" + userId + ".txt", new { yemItem.OrderId }.ToJson());
                            }
                        }
                    }
                }
                this.btn_batchUpdate.Enabled = true;
                this.lbl_showmsg.Text = "执行完毕";
            }
            catch (Exception exception)
            {
                this.btn_batchUpdate.Enabled = true;
                this.lbl_showmsg.Text = "发生错误:" + exception.Message;
            }
        }

        private List<YemUserProductInfo> GetYemUserOrderInfo(string userId)
        {
            try
            {
                string sql = $"select OrderId,Allocated,PurchaseMoney,RemainingAmount,UserId,Status from YEMUserProducts_Copy where UserId ='{userId}'";
                DataTable yemuserOrder = SqlHelper.SqlHelper.Query(sql, null, false);
                if (yemuserOrder.Rows.Count == 0)
                {
                    return null;
                }
                List<YemUserProductInfo> listInfos = new List<YemUserProductInfo>();
                foreach (DataRow dr in yemuserOrder.Rows)
                {
                    listInfos.Add(new YemUserProductInfo
                    {
                        OrderId = dr["OrderId"].ToString(),
                        PurchaseMoney = Convert.ToInt64(dr["PurchaseMoney"]),
                        UserId = dr["UserId"].ToString(),
                        Status = Convert.ToInt32(dr["Status"]),
                        Allocated = Convert.ToInt64(dr["Allocated"]),
                        RemainingAmount = Convert.ToInt64(dr["RemainingAmount"])
                    });
                }
                return listInfos;
            }
            catch (Exception ex)
            {
                return null;
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
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        #region Nested type: TextPurchase

        public class TextPurchase
        {
            public long Amout { get; set; }
            public string NewOrderId { get; set; }
            public string OldOrderId { get; set; }
        }

        #endregion Nested type: TextPurchase
    }
}