using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using RedisToTableTool.JymService;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class GrantForm : Form
    {
        public GrantForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            List<string> bb = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                bb.Add(i + "");
            }

            int nums = 20;
            int count = 0;
            List<string> cc = new List<string>();
            while (bb.Count > 0)
            {
                List<string> relationOrdersSelected;
                if (count != 0)
                {
                    int cha = bb.Count - count * nums;
                    if (cha <= 0)
                    {
                        break;
                    }
                    relationOrdersSelected = cha >= nums ? bb.GetRange(count * nums, nums) : bb.GetRange(count * nums, cha);
                }
                else
                {
                    relationOrdersSelected = bb.Count >= nums ? bb.GetRange(count * nums, nums) : bb;
                }

                if (relationOrdersSelected.Count <= 0) continue;
                //执行
                cc.AddRange(relationOrdersSelected);
                count = count + 1;
            }

            List<string> assetIds = this.textBox1.Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.textBox2.Text = assetIds.Count + "";
            //查询资产详细
            string assetApiUrl = ConfigurationManager.AppSettings["assetApiUrl"];
            foreach (var item in assetIds)
            {
                var result = await AssetApiService.Get<AssetResponse>(assetApiUrl + "AssetPool/" + item);
                if (result != null)
                {
                    var yembizApiResult = await YemApiService.GetOnSellAssetInfoByAssetId(ConfigurationManager.AppSettings["YemApiUrl"], item);
                    if (yembizApiResult != null)
                    {
                        LoanMoneyRequest loanMoneyRequest = new LoanMoneyRequest
                        {
                            AssetId = result.AssetId,
                            BankCardNO = result.BankCardNO,
                            CardName = result.CardName,
                            IsEntrustedPay = result.IsEntrustedPay,
                            ReceiveUserId = "C9C28A7AD0D345B29D463E73D08EA613",
                            ShareProfitAccount = "6222609163618260515",
                            ShareProfit = 1,
                            UpdatedBy = "System",
                            UserOrderSumMoney = yembizApiResult.ActualCalculatedAmount - 1 + ""
                        };
                        var loadResult = await AssetApiService.Add(assetApiUrl + "YemFinanciereGrant/Loan", loanMoneyRequest);
                        if (loadResult)
                        {
                            // string url = "http://10.1.25.66:806/YemEnterprisePayer/UpdateStatus?assetId='" + result.AssetId + "'&status=%E9%80%9A%E8%BF%87&assetType=10000&MendReason=";
                            // var submitLoadResult = await AssetApiService.AddNoParam(url);
                            var submitLoadResult = await AssetApiService.AddNoParam(assetApiUrl + "YemFinanciereGrant/SubmitLoan?assetId=" + item + "&updatedBy=1");
                            if (submitLoadResult)
                            {
                                this.textBox3.Text = Convert.ToInt32(this.textBox3.Text) + 1 + "";
                            }
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TextLog.Show();
            //        List<string> assetIds = this.textBox1.Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //        this.textBox2.Text = assetIds.Count + "";
            //        //查询资产详细
            //        string assetApiUrl = ConfigurationManager.AppSettings["assetApiUrl"];
            //        foreach (var item in assetIds)
            //        {
            //            var result = await AssetApiService.Get<AssetResponse>(assetApiUrl + "AssetPool/" + item);
            //            if (result != null)
            //            {
            //                var yembizApiResult = await YemApiService.GetOnSellAssetInfoByAssetId(ConfigurationManager.AppSettings["YemApiUrl"], item);
            //                if (yembizApiResult != null)
            //                {
            //                    RefundMoneyRequest loanMoneyRequest = new RefundMoneyRequest
            //                    {
            //                        AssetId = result.AssetId,
            //                        BankCardNO = result.BankCardNO,
            //                        CardName = result.CardName,
            //                        IsEntrustedPay = result.IsEntrustedPay,
            //                        ReceiveUserId = "C9C28A7AD0D345B29D463E73D08EA613",
            //                        ShareProfitAccount = "6222609163618260515",
            //                        ShareProfit = 1,
            //                        UpdatedBy = "System",
            //                        UserOrderSumMoney = yembizApiResult.ActualCalculatedAmount - 1 + ""
            //                    };
            //                    var loadResult = await AssetApiService.Add(assetApiUrl + "YemFinanciereGrant/Loan", loanMoneyRequest);
            //                    if (loadResult)
            //                    {
            //                        var submitLoadResult = await AssetApiService.AddNoParam(assetApiUrl + "YemFinanciereGrant/SubmitLoan?assetId=" + item + "&updatedBy=1");
            //                        if (submitLoadResult)
            //                        {
            //                            this.textBox3.Text = Convert.ToInt32(this.textBox3.Text) + 1 + "";
            //                        }
            //                    }
            //                }
            //            }
        }
    }

    public class TextLog : NLogger
    {
        public static void Show()
        {
            for (int i = 0; i < 100000; i++)
            {
                Logger.Info("测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测");
            }
        }
    }
}