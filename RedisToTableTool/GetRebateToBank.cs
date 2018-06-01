using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.JymService;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class GetRebateToBank : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly RedisHelperSpecial redisHelperSpecial;
        private readonly CloudTableClient tableClient;

        public GetRebateToBank(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.redisHelperSpecial = new RedisHelperSpecial(11);
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromSeconds(180);
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private void btn_HandleThings_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> userIds = this.txb_handleUserIds.Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (userIds.Count == 0)
                {
                    MessageBox.Show("请输入userIds.....");
                    return;
                }
                this.lbl_showmsg.Text = "正在准备数据......";
                this.btn_HandleThings.Enabled = false;
                List<AssetDebtRebatesModel> listDebtRebateModels = this.GetDebtRebatesInfos(userIds);
                if (userIds.Count == 0)
                {
                    this.lbl_showmsg.Text = "提示";
                    this.btn_HandleThings.Enabled = true;
                    return;
                }
                this.txb_UserNums.Text = userIds.Count.ToString();
                //对listCancelBookModles分组
                //获取集合
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < 6; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        while (userIds.Count > 0)
                        {
                            string userId = string.Empty;
                            lock (this.objLock)
                            {
                                if (userIds.Count > 0)
                                {
                                    userId = userIds[0];
                                    if (!string.IsNullOrEmpty(userId))
                                    {
                                        userIds.Remove(userId);
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(userId))
                            {
                                List<AssetDebtRebatesModel> modelsByUserId = listDebtRebateModels.Where(x => x.UserId == userId).ToList();
                                if (modelsByUserId.Count == 0)
                                {
                                    continue;
                                }
                                long totalAmount = 0;
                                bool isFlag = false;
                                foreach (AssetDebtRebatesModel item in modelsByUserId)
                                {
                                    //执行
                                    SubOrder firstOrDefault = item.subOrderListArray.FirstOrDefault();
                                    if (firstOrDefault != null)
                                    {
                                        string orderId = firstOrDefault.SubOrderId;
                                        //查询银行
                                        int status = await BankGatewayService.GetOrderSearch(orderId);
                                        for (int j = 0; j < 5; j++)
                                        {
                                            status = await BankGatewayService.GetOrderSearch(orderId);
                                            if (status == 1)
                                            {
                                                break;
                                            }
                                        }
                                        switch (status)
                                        {
                                            case 1:
                                                //获取金额
                                                totalAmount += firstOrDefault.Amount;
                                                break;

                                            case -2:
                                                isFlag = true;
                                                //记录用户Id和orderid重试几次
                                                Logger.LoadData(@"GetDebtRebateData\ErrorId.txt", $"UserId:{userId},OrderId{orderId}");
                                                break;
                                        }
                                    }
                                }
                                //输入总流水
                                int data = isFlag ? 0 : 1;
                                string userData = $"{userId},{totalAmount},{data}";
                                Logger.LoadData(@"GetDebtRebateData\UserInfo.txt", userData);
                                lock (this.objLock)
                                {
                                    this.txb_SuccessNums.Text = (Convert.ToInt32(this.txb_SuccessNums.Text) + 1).ToString();
                                }
                            }
                        }
                    }));
                }
                Task.WaitAll(tasks.ToArray());
                //执行完毕后
                this.lbl_showmsg.Text = "全部执行完毕";
                this.btn_HandleThings.Enabled = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private List<AssetDebtRebatesModel> GetDebtRebatesInfos(List<string> userIds)
        {
            try
            {
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtRebates");
                //List<AssetDebtRebatesModel> bookModels = new List<AssetDebtRebatesModel>();
                TableQuery<AssetDebtRebatesModel> queryDeptInfo = new TableQuery<AssetDebtRebatesModel>()
                    .Where("");
                List<AssetDebtRebatesModel> rebateList = deptTable.ExecuteQuery(queryDeptInfo).Where(x => userIds.Contains(x.UserId)).ToList();
                //foreach (AssetDebtRebatesModel item in rebateList)
                //{
                //    item.subOrderListArray = item.SubOrderList.FromJson<List<SubOrder>>();
                //}
                return rebateList;
            }
            catch (Exception ex)
            {
                //即
                return null;
            }
        }
    }
}