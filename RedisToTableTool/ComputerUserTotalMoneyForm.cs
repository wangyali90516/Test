using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using RedisToTableTool.JymService;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class ComputerUserTotalMoneyForm : Form
    {
        public ComputerUserTotalMoneyForm()
        {
            this.InitializeComponent();
        }

        private async void btn_CUserTotalMoney_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_CUserTotalMoney.Enabled = false;
                //获取用户
                string[] userInfos = File.ReadAllLines(@"ComputerUserTotalAsset/UserIds.txt");
                //获取所有的充值用户
                string[] rechargeInfos = File.ReadAllLines(@"ComputerUserTotalAsset/RechargeAmount.txt");
                Dictionary<string, long> dicRechargeInfos = this.MapToDicInfos(rechargeInfos);
                //获取所有的提现数据
                string[] withdrawInfos = File.ReadAllLines(@"ComputerUserTotalAsset/WithdrawAmount.txt");
                Dictionary<string, long> dicWithdrawInfos = this.MapToDicInfos(withdrawInfos);
                //获取所有的迁移钱包数据
                string[] walletInfos = File.ReadAllLines(@"ComputerUserTotalAsset/WalletAmont.txt");
                Dictionary<string, long> dicWalletInfos = this.MapToDicInfos(walletInfos);
                //获取余额猫在投
                string[] yemInfos = File.ReadAllLines(@"ComputerUserTotalAsset/YemAmont.txt");
                Dictionary<string, long> dicYemInfos = this.MapToDicInfos(yemInfos);
                //后去定期在投
                string[] regularInfos = File.ReadAllLines(@"ComputerUserTotalAsset/RegularInvestAmout.txt");
                Dictionary<string, long> dicRegularInfos = this.MapToDicInfos(regularInfos);

                foreach (string useridKey in userInfos)
                {
                    long rechargeAmount = !dicRechargeInfos.ContainsKey(useridKey) ? 0 : dicRechargeInfos[useridKey]; //充值金额
                    //提现金额
                    long withdrawAmont = !dicWithdrawInfos.ContainsKey(useridKey) ? 0 : dicWithdrawInfos[useridKey];
                    //钱包金额
                    long walletAmount = !dicWalletInfos.ContainsKey(useridKey) ? 0 : dicWalletInfos[useridKey];
                    //余额猫在投
                    long yemAmount = !dicYemInfos.ContainsKey(useridKey) ? 0 : dicYemInfos[useridKey];
                    //定期在投
                    long regularAmount = !dicRegularInfos.ContainsKey(useridKey) ? 0 : dicRegularInfos[useridKey];
                    //计算总在投 迁移的钱包+迁移的余额猫再投+迁移的定期再投+存管上线后的充值金额-存管上线后的提现金额 = 当前用户的总资产
                    long totalAsset = walletAmount + yemAmount + regularAmount + rechargeAmount - withdrawAmont;
                    Logger.LoadData(@"ComputerUserTotalAsset\UserDetailInfo.txt", $"{useridKey},{rechargeAmount},{withdrawAmont},{walletAmount},{yemAmount},{regularAmount},{totalAsset}");
                    this.txb_UserNums.Text = (Convert.ToInt32(this.txb_UserNums.Text) + 1).ToString();
                }
                //计算完毕
                this.btn_CUserTotalMoney.Enabled = true;
            }
            catch (Exception exception)
            {
                this.btn_CUserTotalMoney.Enabled = true;
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     获取流水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_Click(object sender, EventArgs e)
        {
            WebResponseBase webResponseBase = await BankGatewayService.GetInformation(this.txb_userId.Text.Trim());
            string urlText = "http://web.hkmdev.firstpay.com/phoenixFS-web/manage/showAccountQuery?reqId=web.p2p.member.info.manage-54df397e-39d0-4180-b3de-5972b4159ec4";
            if (webResponseBase != null)
            {
                string url = $"http://fsgw.hkmdev.firstpay.com/phoenixFS-fsgw/gateway?data={HttpUtility.UrlEncode(webResponseBase.Data)}&tm={HttpUtility.UrlEncode(webResponseBase.Tm)}&merchantId={webResponseBase.MerchantId}";
                //获取跳转后的数据
                WebClient webClient = new WebClient();
                byte[] htmlBytes = webClient.DownloadData(new Uri(urlText));

                string c = Encoding.UTF8.GetString(htmlBytes);
            }
            MessageBox.Show("获取网关异常");
        }

        private Dictionary<string, long> MapToDicInfos(string[] infos)
        {
            Dictionary<string, long> dicInfos = new Dictionary<string, long>();
            foreach (string t in infos)
            {
                try
                {
                    if (string.IsNullOrEmpty(t))
                    {
                        continue;
                    }
                    string[] info = t.Split(',');
                    if (dicInfos.ContainsKey(info[0]))
                    {
                        dicInfos[info[0]] += Convert.ToInt64(info[1]);
                    }
                    else
                    {
                        dicInfos.Add(info[0], Convert.ToInt64(info[1]));
                    }
                }
                catch (Exception e)
                {
                    string message = e.Message;
                }
            }
            // Dictionary<string, long> dicInfos = infos.Select(t => t.Split(',')).ToDictionary(info => info[0], info => Convert.ToInt64(info[1]));
            return dicInfos;
        }
    }
}