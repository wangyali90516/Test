using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
    public partial class UpdateDataFromBank : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly RedisHelperSpecial redisHelperSpecial;
        private readonly CloudTableClient tableClient;
        private readonly string tableName = ConfigurationManager.AppSettings["TbName"];
        private readonly List<YemUserOrderInfo> yemUserOrderInfos = new List<YemUserOrderInfo>();

        public UpdateDataFromBank(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.redisHelperSpecial = new RedisHelperSpecial(10);
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
                    int threadNums = Convert.ToInt32(ConfigurationManager.AppSettings["updateThreadNums"]);
                    List<string> assetIds = this.txb_handleAssetIds.Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
                    if (assetIds.Count == 0)
                    {
                        MessageBox.Show("请输入需要执行的asstids");
                        return;
                    }
                    this.lbl_showmsg.Text = "正在准备数据.......";
                    this.btn_HandleThings.Enabled = false;
                    List<OnSellAssetInfo> listOnSellAssetInfos = this.MapToOnSellAssetInfos();
                    if (listOnSellAssetInfos == null)
                    {
                        this.btn_HandleThings.Enabled = true;
                        this.lbl_showmsg.Text = "提示";
                        MessageBox.Show("获取在售资产信息异常,请查看日志");
                        return;
                    }
                    foreach (string assetId in assetIds)
                    {
                        //根据AssetId获取在售资产信息
                        this.lbl_showmsg.Text = $"正在开始将资产{assetId}的银行数据导入.......";
                        this.txb_CurrentAssetId.Text = assetId;
                        OnSellAssetInfo onSellAssetInfo = listOnSellAssetInfos.FirstOrDefault(x => x.OnSellAssetId == assetId);
                        if (onSellAssetInfo == null)
                        {
                            this.lbl_showmsg.Text = $"资产{assetId}在OnsellAsset中没有找到数据,跳过了......";
                            continue;
                        }
                        //再获取所有CgYemOrders 该资产下的所有的数据
                        List<CgYemOrderInfo> listCgYemOrderInfos = this.MapToCgModelinfos(assetId);
                        this.txb_CurrentAssetIdNums.Text = listCgYemOrderInfos.Count.ToString();
                        //去除已经完成
                        List<string> sucessOrderIds = this.redisHelperSpecial.GetCgbankYemSuccessListData(assetId);
                        int countHandle = listCgYemOrderInfos.Count - sucessOrderIds.Count;
                        for (int i = 0; i < threadNums; i++)
                        {
                            Thread thread = new Thread(async () =>
                            {
                                while (listCgYemOrderInfos.Count > 0)
                                {
                                    CgYemOrderInfo cgYemOrderInfo = null;
                                    lock (this.objLock)
                                    {
                                        if (listCgYemOrderInfos.Count > 0)
                                        {
                                            cgYemOrderInfo = listCgYemOrderInfos[0];
                                            if (cgYemOrderInfo != null)
                                            {
                                                listCgYemOrderInfos.Remove(cgYemOrderInfo);
                                            }
                                        }
                                    }
                                    if (cgYemOrderInfo != null)
                                    {
                                        //执行操作
                                        string cgOrderId = cgYemOrderInfo.ORDER_NO;
                                        if (sucessOrderIds.Contains(cgOrderId))
                                        {
                                            continue;
                                        }
                                        //插入语句
                                        bool result = this.InsertUserAssetRatio(cgYemOrderInfo, onSellAssetInfo);
                                        if (result)
                                        {
                                            lock (this.objLock)
                                            {
                                                this.txb_SuccessNums.Text = (Convert.ToInt32(this.txb_SuccessNums.Text) + 1).ToString();
                                                this.txb_totalNums.Text = (Convert.ToInt32(this.txb_totalNums.Text) + 1).ToString();
                                            }
                                            //插入到redis
                                            await this.redisHelperSpecial.SetRedisCgBankOrderListSuccessAsync(assetId, cgOrderId);
                                        }
                                        lock (this.objLock)
                                        {
                                            countHandle = countHandle - 1;
                                        }
                                    }
                                }
                            });
                            thread.IsBackground = true;
                            thread.Start();
                        }
                        while (true)
                        {
                            if (countHandle == 0)
                            {
                                break;
                            }
                            Thread.Sleep(5000);
                        }
                        //执行
                        this.txb_SuccessNums.Text = "0";
                        this.txb_CurrentAssetIdNums.Text = "0";
                        this.lbl_showmsg.Text = $"资产{assetId}的银行数据导入完毕......";
                    }
                    this.btn_HandleThings.Enabled = true;
                    this.lbl_showmsg.Text = "全部执行完毕........";
                }
                catch (Exception ex)
                {
                    this.btn_HandleThings.Enabled = true;
                    this.lbl_showmsg.Text = "发生一个错误:" + ex.Message;
                    Logger.LoadData(@"UdpateCgBankOrders\TotalError.txt", $"错误信息：{ex.Message}+--------" + ex.StackTrace);
                }
            });
        }

        private long GetFenziAmount(string orderId, long orgAmount, long presentValue, long calculatAmount, string assetId)
        {
            try
            {
                decimal zjAmount = (decimal)orgAmount / calculatAmount;
                return (long)(zjAmount * presentValue);
            }
            catch (Exception ex)
            {
                Logger.LoadData(@"UdpateCgBankOrders\Error_" + assetId + ".txt", $"错误Orderid{orderId}-----错误信息：{ex.Message}+--------" + ex.StackTrace);
                return -1;
            }
        }

        private YemUserOrderInfo GetYemUserOrderInfo(string userId, string orderid, string bidid)
        {
            try
            {
                YemUserOrderInfo yemUserOrderInfo = this.yemUserOrderInfos.FirstOrDefault(x => x.UserId == userId);
                if (yemUserOrderInfo != null)
                {
                    return yemUserOrderInfo;
                }
                string sql = $"select top 1 UserId,UserName,CellPhone,PurchaseMoney,OrderId,OrderTime,SequenceNo,CredentialNo from YEMUserProducts where UserId ='{userId}'";
                DataTable yemuserOrder = SqlHelper.SqlHelper.Query(sql, null, false);
                if (yemuserOrder.Rows.Count == 0)
                {
                    return null;
                }
                DataRow dr = yemuserOrder.Rows[0];
                yemUserOrderInfo = new YemUserOrderInfo
                {
                    Cellphone = dr["Cellphone"].ToString(),
                    CredentialNo = dr["CredentialNo"].ToString(),
                    OrderId = dr["OrderId"].ToString(),
                    OrderTime = Convert.ToDateTime(dr["OrderTime"]),
                    PurchaseMoney = Convert.ToInt64(dr["PurchaseMoney"]),
                    SequenceNo = dr["SequenceNo"].ToString(),
                    UserId = dr["UserId"].ToString(),
                    UserName = dr["UserName"].ToString()
                };
                this.yemUserOrderInfos.Add(yemUserOrderInfo);
                return yemUserOrderInfo;
            }
            catch (Exception ex)
            {
                Logger.LoadData(@"UdpateCgBankOrders\Error_" + bidid + ".txt", $"orderId{orderid}: 获取用户订单信息异常userId{userId}-----错误信息：{ex.Message}+--------" + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        ///     插入一条数据
        /// </summary>
        /// <returns></returns>
        private bool InsertUserAssetRatio(CgYemOrderInfo cgYemOrderInfo, OnSellAssetInfo onSellAssetInfo)
        {
            string cgOrderId = cgYemOrderInfo.ORDER_NO;
            try
            {
                //DeductedAmount,AddAmount,DiffDays,Comment
                long orgAmount = cgYemOrderInfo.ORG_AMOUNT; //本金 Numerator denominator
                long numerator = this.GetFenziAmount(cgOrderId, orgAmount, onSellAssetInfo.PresentValue, onSellAssetInfo.CalculatedAmount, onSellAssetInfo.OnSellAssetId); //分子
                string userId = cgYemOrderInfo.PAY_USER_OUT_ID;
                string originalUserAssetRatioId = cgYemOrderInfo.SOURCE_ORDER_ID;
                if (string.IsNullOrEmpty(originalUserAssetRatioId))
                {
                    originalUserAssetRatioId = cgOrderId;
                }
                int status = 4;
                if (originalUserAssetRatioId != cgOrderId)
                {
                    status = 2;
                }

                YemUserOrderInfo yemUserOrderInfo = this.GetYemUserOrderInfo(userId, cgOrderId, onSellAssetInfo.OnSellAssetId);
                if (yemUserOrderInfo == null)
                {
                    return false;
                }

                string sql = $"insert into {this.tableName}(UserAssetRatioId,UserId,UserName,AssetId,Numerator,Denominator,UserPresentValue,Status,Reserve,UpdatedBy,UpdatedTime,CreatedBy,CreatedTime,IsDeleted,OrderId,OrderTime,IsReturned,BillDueDate,AssetCategoryCode,Capital,Cellphone,CredentialNo,IsInvestSuccess,IsNotifyTradingSuccess,NotifyTradingRespInfo,NotifyTradingTime,OriginalUserAssetRatioId,PurchaseMoney)values('{cgOrderId}','{yemUserOrderInfo.UserId}','{yemUserOrderInfo.UserName}','{onSellAssetInfo.OnSellAssetId}',{numerator},{onSellAssetInfo.PresentValue},{0},{status},{1},'System','{DateTime.UtcNow.ToChinaStandardTime()}','System','{DateTime.UtcNow.ToChinaStandardTime()}',{0},'{yemUserOrderInfo.OrderId}','{yemUserOrderInfo.OrderTime}',{0},'{onSellAssetInfo.BillDueDate}',{onSellAssetInfo.AssetCategoryCode},{orgAmount},'{yemUserOrderInfo.Cellphone}','{yemUserOrderInfo.CredentialNo}',{1},{1},'与银行同步','{DateTime.UtcNow.ToChinaStandardTime()}','{originalUserAssetRatioId}',{yemUserOrderInfo.PurchaseMoney})";
                bool result = SqlHelper.SqlHelper.ExecuteNoneQuery(sql) > 0;
                return result;
            }
            catch (Exception ex)
            {
                Logger.LoadData(@"UdpateCgBankOrders\Error_" + onSellAssetInfo.OnSellAssetId + ".txt", $"插入比例信息异常orderId{cgOrderId}-----错误信息：{ex.Message}+--------" + ex.StackTrace);
                return false;
            }
        }

        private List<CgYemOrderInfo> MapToCgModelinfos(string assetId)
        {
            try
            {
                List<CgYemOrderInfo> list = new List<CgYemOrderInfo>();
                DataTable dtCgYemOrders = SqlHelper.SqlHelper.ExecuteDataTable($"select SOURCE_ORDER_ID,ORDER_NO,PAY_USER_OUT_ID,ORG_AMOUNT from CgYemOrders where BID_ID='{assetId}'");
                for (int i = 0; i < dtCgYemOrders.Rows.Count; i++)
                {
                    CgYemOrderInfo cgYemOrderInfo = new CgYemOrderInfo();
                    DataRow dr = dtCgYemOrders.Rows[i];
                    if (dr["SOURCE_ORDER_ID"] != null)
                    {
                        cgYemOrderInfo.SOURCE_ORDER_ID = dr["SOURCE_ORDER_ID"].ToString();
                    }
                    if (dr["ORDER_NO"] != null)
                    {
                        cgYemOrderInfo.ORDER_NO = dr["ORDER_NO"].ToString();
                    }
                    if (dr["PAY_USER_OUT_ID"] != null)
                    {
                        cgYemOrderInfo.PAY_USER_OUT_ID = dr["PAY_USER_OUT_ID"].ToString();
                    }
                    if (dr["ORG_AMOUNT"] != null)
                    {
                        cgYemOrderInfo.ORG_AMOUNT = Convert.ToInt64(dr["ORG_AMOUNT"]);
                    }
                    list.Add(cgYemOrderInfo);
                }
                return list;
            }
            catch (Exception ex)
            {
                Logger.LoadData(@"UdpateCgBankOrders\Error_" + assetId + ".txt", $"转换银行数据信息异常错误信息：{ex.Message}+--------" + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        ///     转换
        /// </summary>
        /// <returns></returns>
        private List<OnSellAssetInfo> MapToOnSellAssetInfos()
        {
            try
            {
                DataTable dtOnsellAssetInfo = SqlHelper.SqlHelper.ExecuteDataTable("select OnSellAssetId,BillDueDate,CalculatedAmount,PresentValue,AssetCategoryCode from OnSellAssets");
                List<OnSellAssetInfo> listonAssetInfos = new List<OnSellAssetInfo>();
                for (int i = 0; i < dtOnsellAssetInfo.Rows.Count; i++)
                {
                    OnSellAssetInfo onSellAssetInfo = new OnSellAssetInfo();
                    DataRow dr = dtOnsellAssetInfo.Rows[i];
                    if (dr["BillDueDate"] != null)
                    {
                        onSellAssetInfo.BillDueDate = Convert.ToDateTime(dr["BillDueDate"]);
                    }
                    onSellAssetInfo.PresentValue = Convert.ToInt64(dr["PresentValue"]);
                    onSellAssetInfo.CalculatedAmount = Convert.ToInt64(dr["CalculatedAmount"]);
                    onSellAssetInfo.OnSellAssetId = dr["OnSellAssetId"].ToString();
                    onSellAssetInfo.AssetCategoryCode = Convert.ToInt32(dr["AssetCategoryCode"]);
                    listonAssetInfos.Add(onSellAssetInfo);
                }
                return listonAssetInfos;
            }
            catch (Exception ex)
            {
                Logger.LoadData(@"UdpateCgBankOrders\TotalError.txt", $"错误信息获取资产信息异常：{ex.Message}+--------" + ex.StackTrace);
                return null;
            }
        }

        private void UpdateDataFromBank_Load(object sender, EventArgs e)
        {
        }
    }
}