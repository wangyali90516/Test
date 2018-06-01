using System;
using System.Configuration;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using JinYinMao.Business.Assets.Request;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json.Linq;
using RedisToTableTool.JymService;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class ConsumeDeatMsgForm : Form
    {
        private readonly LoadAppSettings loadAppSettings;
        private readonly MessageReceiver messageReceiverAdvanceDebt;
        private readonly MessageReceiver messageReceiverBatchbookcredit;
        private readonly MessageReceiver messageReceiverBatchCredit;
        private readonly MessageReceiver messageReceiverGrant;
        private readonly MessageReceiver messageReceiverRedemption;
        private readonly MessageReceiver messageReiceiverPurchase;

        public ConsumeDeatMsgForm(LoadAppSettings loadAppSettings)
        {
            //初始化两个队列的messageReceiver
            //1.bankgateway -creditassignmentgrantnotificationqueue
            string grantQueuename = ConfigurationManager.AppSettings["creditassignmentgrantQName"];
            string batchCreditQueueName = ConfigurationManager.AppSettings["bookcreditcreatebatchQName"]; //债转关系银行回调
            string confirmdebtpurchaseQName = ConfigurationManager.AppSettings["confirmdebtpurchaseQName"];
            string confirmredemptionQName = ConfigurationManager.AppSettings["confirmredemptionQName"];
            string advancedebtforbankQName = ConfigurationManager.AppSettings["advancedebtforbankQName"];
            string batchbookcreditQName = ConfigurationManager.AppSettings["batchbookcreditQName"];
            var namespaceManager = NamespaceManager.CreateFromConnectionString(loadAppSettings.ServiceBusConnectionString);
            if (!namespaceManager.QueueExists(grantQueuename))
            {
                MessageBox.Show($"不存在{grantQueuename}该队列");
                return;
            }
            if (!namespaceManager.QueueExists(batchCreditQueueName))
            {
                MessageBox.Show($"不存在{batchCreditQueueName}该队列");
                return;
            }
            if (!namespaceManager.QueueExists(confirmdebtpurchaseQName))
            {
                MessageBox.Show($"不存在{confirmdebtpurchaseQName}该队列");
                return;
            }
            if (!namespaceManager.QueueExists(confirmredemptionQName))
            {
                MessageBox.Show($"不存在{confirmredemptionQName}该队列");
                return;
            }
            if (!namespaceManager.QueueExists(advancedebtforbankQName))
            {
                MessageBox.Show($"不存在{advancedebtforbankQName}该队列");
                return;
            }
            if (!namespaceManager.QueueExists(batchbookcreditQName))
            {
                MessageBox.Show($"不存在{batchbookcreditQName}该队列");
                return;
            }
            var messagingFactory = MessagingFactory.CreateFromConnectionString(loadAppSettings.ServiceBusConnectionString);
            this.messageReceiverGrant = messagingFactory.CreateMessageReceiver(QueueClient.FormatDeadLetterPath(grantQueuename), ReceiveMode.ReceiveAndDelete); //放款银行回调
            this.messageReceiverBatchCredit = messagingFactory.CreateMessageReceiver(QueueClient.FormatDeadLetterPath(batchCreditQueueName), ReceiveMode.ReceiveAndDelete); //债转关系银行回调
            this.messageReiceiverPurchase = messagingFactory.CreateMessageReceiver(QueueClient.FormatDeadLetterPath(confirmdebtpurchaseQName), ReceiveMode.ReceiveAndDelete); //购买债转通知交易
            this.messageReceiverRedemption = messagingFactory.CreateMessageReceiver(QueueClient.FormatDeadLetterPath(confirmredemptionQName), ReceiveMode.ReceiveAndDelete); //赎回确认通知交易
            this.messageReceiverAdvanceDebt = messagingFactory.CreateMessageReceiver(QueueClient.FormatDeadLetterPath(advancedebtforbankQName), ReceiveMode.ReceiveAndDelete); //债转放款
            this.messageReceiverBatchbookcredit = messagingFactory.CreateMessageReceiver(QueueClient.FormatDeadLetterPath(batchbookcreditQName), ReceiveMode.ReceiveAndDelete); //批量债转
            CheckForIllegalCrossThreadCalls = false;
            this.loadAppSettings = loadAppSettings;
            this.InitializeComponent();
        }

        /// <summary>
        ///     批量债转回调消耗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_batchCreditNotify_Click(object sender, EventArgs e)
        {
            try
            {
                this.lbl2.Text = "正在消耗.......";
                this.btn_batchCreditNotify.Enabled = false;
                BrokeredMessage brokeredMessage;
                this.txb_BatchCreditNotifyFnums.Text = "0";
                this.txb_BatchCreditNotifySnums.Text = "0";
                do
                {
                    brokeredMessage = this.messageReceiverBatchCredit.Receive();
                    if (brokeredMessage != null)
                    {
                        string batchBookCreditInfos = brokeredMessage.GetBody<string>();
                        JObject jObject = JObject.Parse(batchBookCreditInfos);
                        if (jObject == null)
                        {
                            continue;
                        }
                        //转json
                        NotifyBatchCreditRequest notifyBatchCreditRequest = jObject["data"].ToString().FromJson<NotifyBatchCreditRequest>();
                        //重试5次执行数据
                        for (int i = 0; i < 5; i++)
                        {
                            bool result = await YemApiService.ConfirmDebt(this.loadAppSettings.YemApiUrl, notifyBatchCreditRequest, "BatchCreditNotifyLogs");
                            if (result)
                            {
                                this.UpdateTxtInfos(this.txb_BatchCreditNotifySnums);
                                break;
                            }
                            else
                            {
                                if (i == 4)
                                {
                                    this.UpdateTxtInfos(this.txb_BatchCreditNotifyFnums);
                                    //记录下来数据
                                    Logger.LoadData(@"BatchCreditNotifyInfos\ErrorDeadLineInfos.txt", notifyBatchCreditRequest.ToJson());
                                    Logger.LoadData(@"BatchCreditNotifyInfos\ErrorDeadLineInfos.txt", "\r\n");
                                }
                            }
                        }
                    }
                } while (brokeredMessage != null);
                this.btn_batchCreditNotify.Enabled = true;
                MessageBox.Show("本轮次执行完毕");
            }
            catch (Exception exception)
            {
                this.lbl2.Text = "发生一个错误" + exception.Message;
                this.btn_batchCreditNotify.Enabled = true;
                Logger.LoadData(@"BatchCreditNotifyInfos\Errors.txt", exception.Message + "----------" + exception.StackTrace);
            }
        }

        /// <summary>
        ///     批量债转通知银行队列消耗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_consumebatchCreditBank_Click(object sender, EventArgs e)
        {
            try
            {
                this.lbl_showMsg.Text = "正在消耗......";
                this.btn_consumebatchCreditBank.Enabled = false;
                BrokeredMessage brokeredMessage;
                this.txb_batchCreditBankSNums.Text = "0";
                this.txb_batchCreditBankFNums.Text = "0";
                do
                {
                    brokeredMessage = await this.messageReceiverBatchbookcredit.ReceiveAsync(TimeSpan.FromMinutes(1));
                    if (brokeredMessage != null)
                    {
                        string batchBookCreditInfos = brokeredMessage.GetBody<string>();
                        JObject jObject = JObject.Parse(batchBookCreditInfos);
                        if (jObject == null)
                        {
                            continue;
                        }
                        //转json
                        SendDebtForBankModel sendDebtForBankModel = jObject["MessageData"].ToString().FromJson<SendDebtForBankModel>();
                        //重试5次执行数据
                        for (int i = 0; i < 5; i++)
                        {
                            bool result = await YemApiService.SendDebtForBank(this.loadAppSettings.AssetApiUrl, sendDebtForBankModel, "SendDebtForBankLogs");
                            if (result)
                            {
                                this.UpdateTxtInfos(this.txb_batchCreditBankSNums);
                                break;
                            }
                            else
                            {
                                if (i == 4)
                                {
                                    this.UpdateTxtInfos(this.txb_batchCreditBankFNums);
                                    //记录下来数据
                                    Logger.LoadData(@"SendDebtForBankInfos\ErrorDeadLineInfos.txt", sendDebtForBankModel.ToJson());
                                    Logger.LoadData(@"SendDebtForBankInfos\ErrorDeadLineInfos.txt", "\r\n");
                                }
                            }
                        }
                    }
                } while (brokeredMessage != null);
                this.btn_consumebatchCreditBank.Enabled = true;
                MessageBox.Show("本轮次执行完毕");
            }
            catch (Exception exception)
            {
                this.lbl_showMsg.Text = "发生一个错误" + exception.Message;
                this.btn_consumebatchCreditBank.Enabled = true;
                Logger.LoadData(@"SendDebtForBankInfos\Errors.txt", exception.Message);
            }
        }

        /// <summary>
        ///     放款通知银行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_GrantBank_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblshwMsg3.Text = "正在执行.......";
                this.btn_GrantBank.Enabled = false;
                BrokeredMessage brokeredMessage;
                this.txb__GrantBankSNums.Text = "0";
                this.txb_GrantBankFnums.Text = "0";
                do
                {
                    brokeredMessage = this.messageReceiverAdvanceDebt.Receive();
                    if (brokeredMessage != null)
                    {
                        string batchBookCreditInfos = brokeredMessage.GetBody<string>();
                        JObject jObject = JObject.Parse(batchBookCreditInfos);
                        if (jObject == null)
                        {
                            continue;
                        }
                        //转json
                        AdvanceDebtRequest advanceDebtRequest = jObject["MessageData"].ToString().FromJson<AdvanceDebtRequest>();
                        //重试5次执行数据
                        for (int i = 0; i < 5; i++)
                        {
                            bool result = await YemApiService.SendAdvanceDebt(this.loadAppSettings.AssetApiUrl, advanceDebtRequest, "SendAdvanceDebtLogs");
                            if (result)
                            {
                                this.UpdateTxtInfos(this.txb__GrantBankSNums);
                                break;
                            }
                            else
                            {
                                if (i == 4)
                                {
                                    this.UpdateTxtInfos(this.txb_GrantBankFnums);
                                    //记录下来数据
                                    Logger.LoadData(@"SendAdvanceDebtInfos\ErrorDeadLineInfos.txt", advanceDebtRequest.ToJson());
                                    Logger.LoadData(@"SendAdvanceDebtInfos\ErrorDeadLineInfos.txt", "\r\n");
                                }
                            }
                        }
                    }
                } while (brokeredMessage != null);
                this.btn_GrantBank.Enabled = true;
                MessageBox.Show("本轮次执行完毕");
            }
            catch (Exception exception)
            {
                this.lblshwMsg3.Text = "发生一个错误" + exception.Message;
                this.btn_GrantBank.Enabled = true;
                Logger.LoadData(@"SendAdvanceDebtInfos\Errors.txt", exception.Message);
            }
        }

        /// <summary>
        ///     放款回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_grantNotify_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblmsg4.Text = "正在消耗,请稍等......";
                this.btn_grantNotify.Enabled = false;
                BrokeredMessage brokeredMessage;
                this.txb_GrantNotifySnums.Text = "0";
                this.txb_GrantNotifyFnums.Text = "0";
                do
                {
                    brokeredMessage = this.messageReceiverGrant.Receive();
                    if (brokeredMessage != null)
                    {
                        string batchBookCreditInfos = brokeredMessage.GetBody<string>();
                        JObject jObject = JObject.Parse(batchBookCreditInfos);
                        if (jObject == null)
                        {
                            continue;
                        }
                        //转json
                        ConfirmAdvanceDebtRequest confirmAdvanceDebtRequest = jObject["data"].ToString().FromJson<ConfirmAdvanceDebtRequest>();
                        //重试5次执行数据
                        for (int i = 0; i < 5; i++)
                        {
                            bool result = await YemApiService.ConfirmAdvanceDebt(this.loadAppSettings.YemApiUrl, confirmAdvanceDebtRequest, "ConfirmAdvanceDebtLogs");
                            if (result)
                            {
                                this.UpdateTxtInfos(this.txb_GrantNotifySnums);
                                break;
                            }
                            else
                            {
                                if (i == 4)
                                {
                                    this.UpdateTxtInfos(this.txb_GrantNotifyFnums);
                                    //记录下来数据
                                    Logger.LoadData(@"ConfirmAdvanceDebtInfos\ErrorDeadLineInfos.txt", confirmAdvanceDebtRequest.ToJson());
                                    Logger.LoadData(@"ConfirmAdvanceDebtInfos\ErrorDeadLineInfos.txt", "\r\n");
                                }
                            }
                        }
                    }
                } while (brokeredMessage != null);
                this.btn_grantNotify.Enabled = true;
                MessageBox.Show("本轮次执行完毕");
            }
            catch (Exception exception)
            {
                this.lblmsg4.Text = "发生一个错误" + exception.Message;
                this.btn_grantNotify.Enabled = true;
                Logger.LoadData(@"ConfirmAdvanceDebtInfos\Errors.txt", exception.Message);
            }
        }

        /// <summary>
        ///     购买通知交易
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_PurchaseTrisfer_Click(object sender, EventArgs e)
        {
            //
            try
            {
                this.lblmsg6.Text = "正在消耗........";
                this.btn_PurchaseTrisfer.Enabled = false;
                BrokeredMessage brokeredMessage;
                this.txb_PurchaseTrisferFnums.Text = "0";
                this.txb_PurchaseTrisferSnums.Text = "0";
                do
                {
                    brokeredMessage = this.messageReiceiverPurchase.Receive();
                    if (brokeredMessage != null)
                    {
                        string batchBookCreditInfos = brokeredMessage.GetBody<string>();
                        JObject jObject = JObject.Parse(batchBookCreditInfos);
                        if (jObject == null)
                        {
                            continue; //MessageData
                        }
                        //转json
                        TifisfalPurchaseRequestModel tifisfalPurchaseRequestModel = jObject["MessageData"].ToString().FromJson<TifisfalPurchaseRequestModel>();
                        //重试5次执行数据
                        for (int i = 0; i < 5; i++)
                        {
                            Tuple<bool, string> result = await YemApiService.PurchaseSendToTrader(this.loadAppSettings.AssetApiUrl, tifisfalPurchaseRequestModel, "PurchaseSendToTraderLogs");
                            if (result.Item1)
                            {
                                this.UpdateTxtInfos(this.txb_PurchaseTrisferSnums);
                                break;
                            }
                            else
                            {
                                if (i == 4)
                                {
                                    this.UpdateTxtInfos(this.txb_PurchaseTrisferFnums);
                                    //记录下来数据
                                    Logger.LoadData(@"PurchaseSendToTraderInfos\ErrorDeadLineInfos.txt", tifisfalPurchaseRequestModel.ToJson());
                                    Logger.LoadData(@"PurchaseSendToTraderInfos\ErrorDeadLineInfos.txt", "\r\n");
                                }
                            }
                        }
                    }
                } while (brokeredMessage != null);
                this.btn_PurchaseTrisfer.Enabled = true;
                MessageBox.Show("本轮次执行完毕");
            }
            catch (Exception exception)
            {
                this.lblmsg6.Text = "发生一个错误" + exception.Message;
                this.btn_PurchaseTrisfer.Enabled = true;
                Logger.LoadData(@"PurchaseSendToTraderInfos\Errors.txt", exception.Message);
            }
        }

        /// <summary>
        ///     赎回通知交易
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_RedemptionTrisfer_Click(object sender, EventArgs e)
        {
            //
            try
            {
                this.btn_RedemptionTrisfer.Enabled = false;
                BrokeredMessage brokeredMessage;
                this.lbl_showmsg5.Text = "正在消耗,请稍等......";
                this.txb_RedemptionFnums.Text = "0";
                this.txb_RedemptionSnums.Text = "0";
                do
                {
                    brokeredMessage = this.messageReceiverRedemption.Receive();
                    if (brokeredMessage != null)
                    {
                        string batchBookCreditInfos = brokeredMessage.GetBody<string>();
                        //转json
                        JObject jObject = JObject.Parse(batchBookCreditInfos);
                        if (jObject == null)
                        {
                            continue; //MessageData
                        }
                        TirisfalUserRedemptionInfoModel tirisfalUserRedemptionInfoModel = jObject["MessageData"].ToString().FromJson<TirisfalUserRedemptionInfoModel>();
                        //重试5次执行数据
                        for (int i = 0; i < 5; i++)
                        {
                            Tuple<bool, string> result = await YemApiService.RedemptionSendToTrader(this.loadAppSettings.AssetApiUrl, tirisfalUserRedemptionInfoModel, "RedemptionSendToTraderLogs");
                            if (result.Item1)
                            {
                                this.UpdateTxtInfos(this.txb_RedemptionSnums);
                                break;
                            }
                            else
                            {
                                if (i == 4)
                                {
                                    this.UpdateTxtInfos(this.txb_RedemptionFnums);
                                    //记录下来数据
                                    Logger.LoadData(@"RedemptionSendToTraderInfos\ErrorDeadLineInfos.txt", tirisfalUserRedemptionInfoModel.ToJson());
                                    Logger.LoadData(@"RedemptionSendToTraderInfos\ErrorDeadLineInfos.txt", "\r\n");
                                }
                            }
                        }
                    }
                } while (brokeredMessage != null);
                this.btn_RedemptionTrisfer.Enabled = true;
                MessageBox.Show("本轮次执行完毕");
            }
            catch (Exception exception)
            {
                this.lbl_showmsg5.Text = "发生一个错误" + exception.Message;
                this.btn_RedemptionTrisfer.Enabled = true;
                Logger.LoadData(@"RedemptionSendToTraderInfos\Errors.txt", exception.Message);
            }
        }

        /// <summary>
        ///     消耗债转放款死队列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //消耗放款死队列数据
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void ConsumeDeatMsgForm_Load(object sender, EventArgs e)
        {
        }

        private void UpdateTxtInfos(TextBox textBox)
        {
            textBox.Text = (Convert.ToInt32(textBox.Text.Trim()) + 1).ToString();
        }
    }
}