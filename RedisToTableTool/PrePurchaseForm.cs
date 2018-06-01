using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using J.Base.Lib;
using RedisToTableTool.JymService;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class PrePurchaseForm : Form
    {
        public PrePurchaseForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            //LocalDb
            //开始申购
            //1.获取所有的用户 充值了的
            //10000*(1000)
            try
            {
                Random random = new Random();
                DataTable dt = SqlHelper.SqlHelper.ExecuteDataTable("select * from AccountUsers where  IsVerifed=1 and IsActivity=1 and RechargeAmount>0");
                int nums = random.Next(1, 6);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    string userId = dr["UserIdentifier"].ToString();
                    //授权
                    while (nums > 0)
                    {
                        int num = random.Next(1, 1001);
                        //预约批量投资
                        long frezzeAmount = num * 10000;
                        string orderId = Guid.NewGuid().ToGuidString();
                        BookFrezzeModel bookFrezzeModel = new BookFrezzeModel
                        {
                            FreezeAccountAmount = frezzeAmount,
                            FreezeSumAmount = frezzeAmount,
                            FreezeType = "01",
                            OrderId = orderId,
                            Remark = "",
                            ReturnUrl = "http://www.baidu.com",
                            RpOrderList = null,
                            UserId = userId
                        };
                        //预约冻结
                        bool result = await BankGatewayService.BooKFreeze(bookFrezzeModel);
                        if (result)
                        {
                            //购买
                            BookInvestingModel bookInvestingModel = new BookInvestingModel
                            {
                                Cellphone = dr["CellPhone"].ToString(),
                                CredentialNo = dr["CredentialNo"].ToString(),
                                CredentialType = "10",
                                OrderId = orderId,
                                PurchaseAmount = frezzeAmount,
                                PurchaseStartDateTime = DateTime.Now.ToChinaStandardTime(),
                                RealName = dr["RealName"].ToString(),
                                UserId = userId
                            };
                            //调用接口
                            bool result1 = await YemApiService.Booking(ConfigurationManager.AppSettings["yemApiUrl"], bookInvestingModel);
                        }
                        nums -= 1;
                    }
                    nums = random.Next(1, 6);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        ///     下订单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrePurchaseForm_Load(object sender, EventArgs e)
        {
        }
    }
}