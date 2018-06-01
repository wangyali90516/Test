using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using J.Base.Lib;
using RedisToTableTool.Model;
using RedisToTableTool.SqlHelper;

namespace RedisToTableTool
{
    public partial class AddAssets : Form
    {
        private readonly List<int> days = new List<int>();

        public AddAssets()
        {
            CheckForIllegalCrossThreadCalls = false;

            this.InitializeComponent();
        }

        private void AddAssets_Load(object sender, EventArgs e)
        {
            this.comType.SelectedIndex = 0;
            this.comboBox1.SelectedIndex = 0;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string assetName = this.textBox3.Text;
            if (string.IsNullOrWhiteSpace(assetName))
            {
                MessageBox.Show("请输入资产名称前缀");
                return;
            }
            string comType = this.comType.SelectedItem.ToString();
            if (comType != "随机")
            {
                if (string.IsNullOrWhiteSpace(this.txtBegin.Text) || string.IsNullOrWhiteSpace(this.txtEnd.Text))
                {
                    MessageBox.Show("请输入开始值和结束值");
                    return;
                }
            }
            string billDueDate = "";
            if (this.comboBox1.SelectedItem.ToString().Equals("固定到期日"))
            {
                billDueDate = this.txtbillDueDate.Text;
            }
            else
            {
                for (int i = int.Parse(this.txtDueBegin.Text); i <= int.Parse(this.txtDueEnd.Text); i++)
                {
                    this.days.Add(i);
                }
            }

            string assetApiUrl = ConfigurationManager.AppSettings["assetApiUrl"];
            int assetCount = Convert.ToInt32(this.textBox1.Text);
            int usedCount = 0;
            for (int i = 0; i < assetCount; i++)
            {
                DraftBillAddRequest draftBillAddRequest = new DraftBillAddRequest
                {
                    BackOfScanning = "https://jymstoredev.blob.core.chinacloudapi.cn/publicfiles/office/EnterpriseManager/GuaranteeWay/2018-03-26/20180326132356420.jpg,",
                    BillDate = DateTime.UtcNow.ToChinaStandardTime(),
                    BillNo = assetName + DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMMddHHmmssfff"),
                    DrawerName = DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMMddHHmmssfff"),
                    GetBillDate = DateTime.UtcNow.ToChinaStandardTime(),
                    EndorserFin = "https://jymstoredev.blob.core.chinacloudapi.cn/publicfiles/office/EnterpriseManager/GuaranteeWay/2018-03-26/20180326132356420.jpg,",
                    PayBankFullName = DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMMddHHmmssfff"),
                    PayingBankNo = "11111111",
                    ReceiverAccount = null,
                    ReceiverBank = null,
                    ReceiverName = null,
                    ReceivingSideFullName = "交易系统",
                    ReceivingSideId = "DFFF53861092448AB39D34864581EFA3",
                    //   AssetCode = "银票" + DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMMddHHmmssfff"),
                    AssetDetails = "",
                    //  AssetName = "银票" + DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMMddHHmmssfff"),
                    AssetTypeId = "30034180F16E4B1C99F9966FBEA799C1",
                    AssetTypeName = "银企众盈",
                    BillCost = 600,
                    BillDueDate = !string.IsNullOrWhiteSpace(billDueDate) ? DateTime.Parse(billDueDate) : new DateTime(2019, 12, 12),
                    FinancierId = "418CE607F0424FCEAAF0E004B65E75B1",
                    FinancierName = "权威借款户",
                    PeriodType = "0",
                    FundUsage = "日常经营",
                    PresentValue = 100000000,
                    GrowthTime = DateTime.UtcNow.ToChinaStandardTime(),
                    ValueStatus = false,
                    IsEntrustedPay = "0",
                    BankCardNO = "",
                    CardName = "",
                    BankCode = "",
                    CardFlag = "",
                    Issuer = "",
                    BidType = "01",
                    ProductType = "06",
                    BorrPurpose = "日常经营",
                    RepaymentType = "01",
                    FlowStatus = 0,
                    CreatedBy = "System",
                    CreatedTime = DateTime.UtcNow.ToChinaStandardTime(),
                    UpdatedBy = "System",
                    UpdatedTime = DateTime.UtcNow.ToChinaStandardTime()
                };
                draftBillAddRequest.AssetName = draftBillAddRequest.BillNo;
                draftBillAddRequest.AssetCode = draftBillAddRequest.BillNo;
                Random random = new Random();
                if (comType == "随机")
                {
                    int ranBillMoney = random.Next(1000000, 100000000);
                    draftBillAddRequest.BillMoney = ranBillMoney;
                }
                else
                {
                    draftBillAddRequest.BillMoney = random.Next(Convert.ToInt32(this.txtBegin.Text) * 100, Convert.ToInt32(this.txtEnd.Text) * 100) / 100 * 100;
                }
                var day = 0;
                if (!this.comboBox1.SelectedItem.ToString().Equals("固定到期日"))
                {
                    day = this.days[0];
                    draftBillAddRequest.BillDueDate = DateTime.UtcNow.ToChinaStandardTime().AddDays(day);
                }
                draftBillAddRequest.BillOwnDays = (draftBillAddRequest.BillDueDate.Date - DateTime.UtcNow.ToChinaStandardTime().Date).Days;

                draftBillAddRequest.Interest = draftBillAddRequest.BillMoney / 100 * draftBillAddRequest.BillOwnDays * draftBillAddRequest.BillCost / 100 / 365;
                var result = await AssetApiService.Add(assetApiUrl + "/AssetPool/DraftBill/Add", draftBillAddRequest);
                if (result)
                {
                    object assetId = AssetApiSqlHelper.ExecuteScalar("select AssetId from Assets where  AssetName like N'%" + draftBillAddRequest.AssetName + "%'");
                    if (assetId != null)
                    {
                        string sql = "Update Assets set Status=0,FlowStatus=3 where AssetId='" + assetId + "'";
                        if (AssetApiSqlHelper.ExecuteNoneQuery(sql) > 0)
                        {
                            this.textBox2.Text = Convert.ToInt32(this.textBox2.Text) + 1 + "";
                            usedCount++;
                            if (!this.comboBox1.SelectedItem.ToString().Equals("固定到期日"))
                            {
                                if (usedCount == int.Parse(this.txtNum.Text))
                                {
                                    usedCount = 0;
                                    this.days.Remove(day);
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string assetName = this.textBox3.Text;
            if (string.IsNullOrWhiteSpace(assetName))
            {
                MessageBox.Show("请输入资产名称前缀");
                return;
            }
            string comType = this.comType.SelectedText;
            if (comType != "随机")
            {
                if (!string.IsNullOrWhiteSpace(this.txtBegin.Text) && !string.IsNullOrWhiteSpace(this.txtEnd.Text))
                {
                    MessageBox.Show("请输入开始值和结束值");
                    return;
                }
            }
            string billDueDate = "";
            if (this.comboBox1.SelectedItem.ToString().Equals("固定到期日"))
            {
                billDueDate = this.txtbillDueDate.Text;
            }
            else
            {
                for (int i = int.Parse(this.txtDueBegin.Text); i <= int.Parse(this.txtDueEnd.Text); i++)
                {
                    this.days.Add(i);
                }
            }
            string assetApiUrl = ConfigurationManager.AppSettings["assetApiUrl"];
            int assetCount = Convert.ToInt32(this.textBox1.Text);
            int usedCount = 0;
            for (int i = 0; i < assetCount; i++)
            {
                MerchantBillAddRequest merchantBillAddRequest = new MerchantBillAddRequest
                {
                    BillDate = DateTime.UtcNow.ToChinaStandardTime(),
                    BillNo = assetName + DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMMddHHmmssfff"),
                    EndorserFin = "https://jymstoredev.blob.core.chinacloudapi.cn/publicfiles/office/EnterpriseManager/GuaranteeWay/2018-03-26/20180326132356420.jpg,",
                    FinancierNameOfUser = "权威**款户",
                    GetBillDate = DateTime.UtcNow.ToChinaStandardTime(),
                    Guarantee = "https://jymstoredev.blob.core.chinacloudapi.cn/publicfiles/office/EnterpriseManager/GuaranteeWay/2018-03-22/20180322110453788.jpg,",
                    GuaranteeTypeCode = "10",
                    GuaranteeTypeName = "银行保兑",
                    PayAccount = "***",
                    PayBankFullName = "招商银行",
                    PayerId = "11239132C2394BA3AEA2B9BF78961CA7",
                    PayerName = "委屈了翁",
                    PayerNameOfUser = "委屈了翁",
                    PayFullName = "委屈了翁",
                    ReceiverAccount = "",
                    ReceiverBank = "",
                    ReceiverName = "",
                    //    AssetCode = "商票" + DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMMddHHmmssfff"),
                    AssetDetails = "",
                    //   AssetName = "商票" + DateTime.UtcNow.ToChinaStandardTime().ToString("yyyyMMddHHmmssfff"),
                    AssetTypeId = "1E59F289AE1A4A719156AAF9CE9D0498",
                    AssetTypeName = "商融保盈",
                    BillCost = 600,
                    BillDueDate = !string.IsNullOrWhiteSpace(billDueDate) ? DateTime.Parse(billDueDate) : new DateTime(2019, 12, 12),
                    FinancierId = "418CE607F0424FCEAAF0E004B65E75B1",
                    FinancierName = "权威借款户",
                    PeriodType = "0",
                    FundUsage = "日常经营",
                    GrowthTime = DateTime.UtcNow.ToChinaStandardTime(),
                    ValueStatus = false,
                    IsEntrustedPay = "0",
                    BankCardNO = "",
                    CardName = "",
                    BankCode = "",
                    CardFlag = "",
                    Issuer = "",
                    BidType = "01",
                    ProductType = "04",
                    BorrPurpose = "日常经营",
                    RepaymentType = "01",
                    FlowStatus = 0,
                    CreatedBy = "System",
                    CreatedTime = DateTime.UtcNow.ToChinaStandardTime(),
                    IsDeleted = false,
                    UpdatedBy = "System",
                    UpdatedTime = DateTime.UtcNow.ToChinaStandardTime()
                };
                merchantBillAddRequest.AssetName = merchantBillAddRequest.BillNo;
                merchantBillAddRequest.AssetCode = merchantBillAddRequest.BillNo;
                merchantBillAddRequest.FinancierIntroduction = merchantBillAddRequest.BillNo;
                merchantBillAddRequest.GuaranteeFullName = merchantBillAddRequest.BillNo;
                merchantBillAddRequest.GuaranteeIntroduction = merchantBillAddRequest.BillNo;
                merchantBillAddRequest.PaymentIntroduction = merchantBillAddRequest.BillNo;
                Random random = new Random();
                if (comType == "随机")
                {
                    int ranBillMoney = random.Next(1000000, 100000000);
                    merchantBillAddRequest.BillMoney = ranBillMoney;
                }
                else
                {
                    merchantBillAddRequest.BillMoney = random.Next(Convert.ToInt32(this.txtBegin.Text) * 100, Convert.ToInt32(this.txtEnd.Text) * 100) / 100 * 100;
                }
                merchantBillAddRequest.CalculatedAmount = merchantBillAddRequest.BillMoney;
                merchantBillAddRequest.PresentValue = merchantBillAddRequest.CalculatedAmount;
                var day = 0;
                if (!this.comboBox1.SelectedItem.ToString().Equals("固定到期日"))
                {
                    day = this.days[0];
                    merchantBillAddRequest.BillDueDate = DateTime.UtcNow.ToChinaStandardTime().AddDays(day);
                }

                merchantBillAddRequest.BillOwnDays = (merchantBillAddRequest.BillDueDate.Date - DateTime.UtcNow.ToChinaStandardTime().Date).Days;

                var result = await AssetApiService.Add(assetApiUrl + "/AssetPool/MerchantBill/Add", merchantBillAddRequest);
                if (result)
                {
                    object assetId = AssetApiSqlHelper.ExecuteScalar("select AssetId from Assets where  AssetName like N'%" + merchantBillAddRequest.AssetName + "%'");
                    if (assetId != null)
                    {
                        string sql = "Update Assets set Status=0,FlowStatus=3 where AssetId='" + assetId + "'";
                        if (AssetApiSqlHelper.ExecuteNoneQuery(sql) > 0)
                        {
                            this.textBox2.Text = Convert.ToInt32(this.textBox2.Text) + 1 + "";
                            usedCount++;
                            if (!this.comboBox1.SelectedItem.ToString().Equals("固定到期日"))
                            {
                                if (usedCount == int.Parse(this.txtNum.Text))
                                {
                                    usedCount = 0;
                                    this.days.Remove(day);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dueType = this.comboBox1.SelectedItem.ToString();
            if (dueType == "固定到期日")
            {
                this.txtbillDueDate.Enabled = true;
                this.txtDueEnd.Enabled = false;
                this.txtDueBegin.Enabled = false;
            }
            else
            {
                this.txtbillDueDate.Enabled = false;
                this.txtDueEnd.Enabled = true;
                this.txtDueBegin.Enabled = true;
            }
        }
    }
}