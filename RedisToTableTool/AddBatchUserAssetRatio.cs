using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using J.Base.Lib;
using RedisToTableTool.SqlHelper;

namespace RedisToTableTool
{
    public partial class AddBatchUserAssetRatio : Form
    {
        public AddBatchUserAssetRatio()
        {
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Parallel.For(0, Convert.ToInt32(this.textBox1.Text), item =>
            {
                string sql = "INSERT INTO [yem_biz].[Core].[UserAssetRatios] ([UserAssetRatioId], [OriginalUserAssetRatioId], [AssetId], [AssetCode], [BillDueDate], [Rate], [Principal], [PrincipalInterest], [CoinPrice], [ContractAmount], [YesterdayInterest], [AfterFenInterest], [IsTransferred], [DeptContactInfoId], [UserId], [OrderId], [ChildOrderId], [InterestDateTime], [Status], [InvestmentBankOrderId], [UserTotalInterest], [IsDelete], [CreateTime], [UpdateTime], [DeptRedeemStatus], [RedeemDateTime], [MatchedDateTime], [TransactionDateTime], [BatchId], [RedeemOrderId], [IsExpired], [IsReadyTransaction], [RedeemSort], [Id], [DataSourceType])";
                sql += " VALUES (N'00000000000000000000000000000000', '00000000000000000000000000000000', N'02B20F9D4CCA4DDDA83E067BBF2CE25E', N'Y1111111333', '2020 - 12 - 12 00:00:00.000', '600', '857500', '422', '1000000', '857500', '141', '93', '0', N'000010025B03B86F1320CB7CA4CA76B0', N'8556B951A7324E348A57016265138FB8', N'06A819236E7040C7AB0140973BFD2027', NULL, '2018 - 05 - 25 01:00:44.480', '10', N'5DAD434B3B0F48A5B7E8E04C9D2DAB8D', '422', '1', '2018 - 05 - 22 14:00:05.810', '2018 - 05 - 25 13:08:27.860', '30', '2018 - 05 - 25 13:08:27.860', '2018 - 05 - 25 14:06:51.197', '2018 - 05 - 22 14:00:44.513', N'201805221400', N'4CA2E710D9774A6CBFDAE597DBD889D5', '0', '1', '0', N'" + Guid.NewGuid().ToGuidString() + "', '0');";

                sql += "INSERT INTO [yem_biz].[Core].[RequestBatchDetails] ";
                sql += " ([BatchId], [ErrorMessage], [Info], [IsFinished], [IsRetry], [OperateNums], [TypeId], [IsDelete],";
                sql += " [CreateTime], [UpdateTime], [IsDequeue], [IsNotifySuccess], [IsRelation], [IsRequestSuccess], [NotifyNums], [OrderId],";
                sql += " [RelationOrderId], [SuborderId], [SeqNo],  [DequeueHandleTime], [NotifyHandleTime], [RequestHandleTime], [NotifyAbnormalNums], [RequestAbnormalNums])";
                sql += " VALUES (N'201805221746', N'', N'', '1', '1', '1', '60', '0', ";
                sql += " '2018-05-19 11:16:30.910', '2018-05-23 11:16:18.000', '1', '1', '0', '1', '1', N'" + Guid.NewGuid().ToGuidString() + "', N'00000000000000000000000000000000', N'" + Guid.NewGuid().ToGuidString() + "', N'" + Guid.NewGuid().ToGuidString() + "',  '2018-05-19 11:16:47.7520000', '2018-05-23 11:16:18.0850000', '2018-05-23 11:16:17.8340000', '0', '0');";
                YemApiSqlHelper.ExecuteNoneQuery(sql);
            });
            MessageBox.Show("处理完成");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Parallel.For(0, Convert.ToInt32(this.textBox1.Text), item =>
            {
                string sql = "INSERT INTO [yem_biz].[Core].[BankStayConfirm] ([ErrorMessage], [InterfaceType], [OrderId], [RequestJson], [IsDelete], [CreateTime], [UpdateTime], [Status]) VALUES (N'', '5', N'" + Guid.NewGuid().ToGuidString() + "', N'', '1', '2018-05-25 12:44:33.843', '2018-05-25 12:44:34.077', '10');";
                YemApiSqlHelper.ExecuteNoneQuery(sql);
            });
            MessageBox.Show("处理完成");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Parallel.For(0, Convert.ToInt32(this.textBox1.Text), item =>
            {
                string sql = "INSERT INTO [yem_biz].[Core].[DeptAssignRelations] ([DeptContactInfoId], [DeptAssignID], [BuyUserId], [RedeemUserId], [ContractAmount], [Capital], [Interest], [RedeemOrderId], [PurchaseOrderId], [OrderType], [OldUserAssetRatioId], [NewUserAssetRatioId], [Status], [MatchingDateTime], [TransactionDateTime], [BatchDebtBankOrderId], [DebtLoanBankOrderId], [BatchId], [IsDelete], [CreateTime], [UpdateTime], [LastUserTotalInterest]) VALUES (N'00000000000000000000000000000000', N'" + Guid.NewGuid().ToGuidString() + "', N'00000000000000000000000000000000', N'00000000000000000000000000000000', '8579', '8575', '4', N'E7E811AFBC6143F083245A3D8B4DF22F', N'000010025B03E5F31321027CA4EFA7C4', '1', N'000010025B03BFF6CAB97633682AE888', N'000010025B07A7DFCAB976786CBE3020', '5', '2018-05-25 14:06:23.637', '2018-05-25 14:15:17.527', N'B2A27A8D92D044E1806418E8FCCCA92F', N'AC03909F1F4C4D30810726E848E27E1E', N'201805251406', '1', '2018-05-25 14:06:36.870', '2018-05-25 14:15:17.527', '4');";
                YemApiSqlHelper.ExecuteNoneQuery(sql);
            });
            MessageBox.Show("处理完成");
        }
    }
}