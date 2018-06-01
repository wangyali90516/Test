using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;
using UserToAssetTool;

namespace RedisToTableTool
{
    public partial class UpdateUserAssetRatioInfoForm : Form
    {
        private readonly DateTime endTime;
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly RedisHelperSpecial redisHelperSpecial;
        private readonly DateTime startTime;
        private readonly CloudTableClient tableClient;

        public UpdateUserAssetRatioInfoForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.redisHelperSpecial = new RedisHelperSpecial(11);
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromSeconds(180);
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private async void btn_handeluserids_Click(object sender, EventArgs e)
        {
            try
            {
                this.btn_handeluserids.Enabled = false;
                List<string> userIds = this.txb_handleUserIds.Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string t in userIds)
                {
                    CloudTable deptTable = this.tableClient.GetTableReference("AssetUserAssetRatios");
                    TableQuery<UserAssetRatio> queryDeptInfo = new TableQuery<UserAssetRatio>()
                        .Where($"PartitionKey eq '{t}'");
                    List<UserAssetRatio> userAssetRatios = deptTable.ExecuteQuery(queryDeptInfo).Where(x => x.Status == 6).ToList();
                    foreach (UserAssetRatio item in userAssetRatios)
                    {
                        ModifyUserAssetRatioRequest modifyUserAssetRatioRequest = new ModifyUserAssetRatioRequest
                        {
                            AssetId = item.AssetId,
                            Capital = item.Capital,
                            Denominator = item.Denominator,
                            IsDeleted = item.IsDeleted,
                            IsInvestSuccess = item.IsInvestSuccess,
                            IsNotifyTradingSuccess = item.IsNotifyTradingSuccess,
                            IsReturned = item.IsReturned,
                            Numerator = item.Numerator,
                            Reserve = item.Reserve,
                            Status = 2,
                            UserAssetRatioId = item.UserAssetRatioId,
                            UserId = item.UserId,
                            OriginalUserAssetRatioId = item.UserAssetRatioId
                        };
                        //update
                        bool result = await this.ModifyUserAssetRatiosByDisk(modifyUserAssetRatioRequest);
                        if (!result)
                        {
                            //日志
                            Logger.LoadData(@"UpdateStatusLogs\logInfo.txt", modifyUserAssetRatioRequest.ToJson());
                            Logger.LoadData(@"UpdateStatusLogs\logInfo.txt", "\r\n");
                        }
                    }
                    this.txb_UserNums.Text = (Convert.ToInt32(this.txb_UserNums.Text.Trim()) + 1).ToString();
                }
                this.btn_handeluserids.Enabled = true;
            }
            catch (Exception ex)
            {
                this.btn_handeluserids.Enabled = true;
                Logger.LoadData(@"UpdateStatusLogs\Error.txt", ex.Message + "----" + ex.StackTrace);
            }
        }

        /// <summary>
        ///     修改用户资产关系比例
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<bool> ModifyUserAssetRatiosByDisk(ModifyUserAssetRatioRequest request)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/ModifyUserAssetRatiosByDisk", request);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"UD_ModifyUserAssetRatiosByDisk\Error_" + request.UserAssetRatioId + "", request.ToJson());
                return false;
            } //
        }
    }
}