using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using RedisToTableTool.JymService;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class AddOnSellAssetForm : Form
    {
        public AddOnSellAssetForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            List<string> assetIds = this.textBox1.Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.textBox2.Text = assetIds.Count + "";
            //查询资产详细
            string assetApiUrl = ConfigurationManager.AppSettings["assetApiUrl"];
            foreach (var item in assetIds)
            {
                var result = await AssetApiService.Get<AssetResponse>(assetApiUrl + "/AssetPool/" + item);

                InsertOnSellAssetInput insertOnSellAssetInput = new InsertOnSellAssetInput
                {
                    AssetCategoryCode = result.AssetCategoryCode,
                    AssetCode = result.AssetCode,
                    AssetId = result.AssetId,
                    AssetName = result.AssetName,
                    BankCardNo = result.BankCardNO,
                    BankCode = result.BankCode,
                    BidType = result.BidType,
                    BillAmount = result.BillMoney,
                    BillDueDate = result.BillDueDate,
                    BillNo = result.BillNo,
                    BorrPurpose = result.BorrPurpose,
                    CalculatedAmount = result.BillMoney,
                    CardFlag = result.CardFlag,
                    CardName = result.CardName,
                    CardNo = result.BankCardNO,
                    EnterpriseLicenseNum = "320106197711100842",
                    EnterpriseLicenseType = "50",
                    FinancierId = result.FinancierId,
                    FinancierName = result.FinancierName,
                    FinancierType = result.FinancingType + "",
                    IsEntrustedPay = result.IsEntrustedPay,
                    Issuer = result.Issuer,
                    PeriodDays = result.PeriodDays,
                    PeriodType = long.Parse(result.PeriodType),
                    Rate = 600,
                    ProductType = result.ProductType,
                    RepaymentType = result.RepaymentType
                };

                bool result1 = await YemApiService.InsertOnSellAssets(ConfigurationManager.AppSettings["yemApiUrl"], new List<InsertOnSellAssetInput> { insertOnSellAssetInput });
                if (result1)
                {
                    this.textBox3.Text += 1;
                }
            }
        }
    }
}