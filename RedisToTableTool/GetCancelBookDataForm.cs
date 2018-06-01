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
    public partial class GetCancelBookDataForm : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly RedisHelperSpecial redisHelperSpecial;
        private readonly CloudTableClient tableClient;

        public GetCancelBookDataForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
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
                List<CancelBookModel> listCancelBookModels = this.GetOnSellAssetInfos(userIds);
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
                                List<CancelBookModel> modelsByUserId = listCancelBookModels.Where(x => x.UserId == userId).ToList();
                                if (modelsByUserId.Count == 0)
                                {
                                    continue;
                                }
                                long totalAmount = 0;
                                bool isFlag = false;
                                foreach (CancelBookModel item in modelsByUserId)
                                {
                                    //执行
                                    string orderId = item.OrderId;
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
                                            totalAmount += item.Amount;
                                            break;

                                        case -2:
                                            isFlag = true;
                                            //记录用户Id和orderid重试几次
                                            Logger.LoadData(@"GetUserCancelBookData\ErrorId.txt", $"UserId:{userId},OrderId{orderId}");
                                            break;
                                    }
                                }
                                //输入总流水
                                int data = isFlag ? 0 : 1;
                                string userData = $"{userId},{totalAmount},{data}";
                                Logger.LoadData(@"GetUserCancelBookData\UserInfo.txt", userData);
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

        private void GetCancelBookDataForm_Load(object sender, EventArgs e)
        {
        }

        private List<CancelBookModel> GetOnSellAssetInfos(List<string> userIds)
        {
            try
            {
                CloudTable deptTable = this.tableClient.GetTableReference("AssetBookFreezeCancelForBanks");
                List<CancelBookModel> bookModels = new List<CancelBookModel>();
                string partitionKey = "freezeCancel";
                TableQuery<CancelBookModel> queryDeptInfo = new TableQuery<CancelBookModel>()
                    .Where($"PartitionKey eq '{partitionKey}'");
                bookModels.AddRange(deptTable.ExecuteQuery(queryDeptInfo).Where(x => userIds.Contains(x.UserId)).ToList());
                return bookModels;
            }
            catch (Exception ex)
            {
                //即
                return null;
            }
        }
    }
}