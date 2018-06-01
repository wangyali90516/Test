using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;
using UserToAssetTool;
using UserToAssetTool.Enum;

namespace RedisToTableTool
{
    public partial class CheckDiskAssetInfo : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;

        public CheckDiskAssetInfo(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            this.httpClient = new HttpClient();
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        /// <summary>
        ///     检验所有的assetIds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_checkAll_Click(object sender, EventArgs e)
        {
            //全部检验数据
            try
            {
                this.btn_checkAll.Enabled = false;
                //获取所有的AssetIds
                List<string> assetIds = this.GetAllCheckAssetIds();
                if (assetIds.Count == 0)
                {
                    this.btn_checkAll.Enabled = true;
                    //获取所有的assetids异常
                    MessageBox.Show("获取所有资产Id异常!");
                }
                //获取所有的assetInfo
                List<OnSellAssetResponse> onSellAssetResponses = await this.GetAllAssetCheckInfos(assetIds);
                if (onSellAssetResponses.Count == 0 || onSellAssetResponses.Count != assetIds.Count)
                {
                    this.btn_checkAll.Enabled = true;
                    //获取所有的assetids异常
                    MessageBox.Show("获取所有资产信息异常!");
                }
                this.txb_DemandCheckAssetIdNums.Text = assetIds.Count.ToString();
                //并发去检验数据是否正常
                for (int i = 0; i < this.loadAppSettings.CheckAssetInfoThreadNums; i++)
                {
                    Thread thread = new Thread(async () =>
                    {
                        while (true)
                        {
                            string assetId = string.Empty;
                            OnSellAssetResponse onSellAssetResponse = null;
                            if (assetIds.Count > 0)
                            {
                                lock (this.objLock)
                                {
                                    if (assetIds.Count > 0 && onSellAssetResponses.Count > 0)
                                    {
                                        assetId = assetIds[0];
                                        onSellAssetResponse = onSellAssetResponses[0];
                                        //remove
                                        assetIds.Remove(assetId);
                                        //remove
                                        onSellAssetResponses.Remove(onSellAssetResponse);
                                    }
                                }
                                //执行
                                Tuple<bool, int, string> result = await this.CheckOneDiskAssetInfo(assetId, onSellAssetResponse);
                                //显示数据
                                switch (result.Item2)
                                {
                                    case 0:
                                        //Success
                                        this.UpdateTxtInfo(this.txb_checkSuccess);
                                        break;

                                    case 1:
                                        this.UpdateTxtInfo(this.txb_checkFailed);
                                        break;

                                    case 2:
                                        this.UpdateTxtInfo(this.txb_checkError);
                                        break;
                                }
                                //在大文本框中显示错误信息
                                lock (this.objLock)
                                {
                                    this.txb_ShowMsgAll.AppendText($"AssetId:{result.Item1} {result.Item3}---{DateTime.UtcNow.ToChinaStandardTime()}");
                                    this.txb_ShowMsgAll.ScrollToCaret();
                                }
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                Thread threadShow = new Thread(() =>
                {
                    while (true)
                    {
                        if (assetIds.Count == 0)
                        {
                            Thread.Sleep(2000);
                            break;
                        }
                        //如果没跑完就停止5s
                        Thread.Sleep(2000);
                    }
                    this.btn_checkAll.Enabled = true;
                });
                threadShow.IsBackground = true;
                threadShow.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生了一个错误:" + ex.Message);
                Logger.LoadData(@"CheckDiskAsset\Error.txt", ex.Message + "----" + ex.StackTrace);
                this.btn_checkAll.Enabled = true;
            }
        }

        /// <summary>
        ///     检验一个assetid是否正确
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_CheckOneAsset_Click(object sender, EventArgs e)
        {
            try
            {
                string assetId = this.txb_oneAssetId.Text.Trim();
                if (string.IsNullOrEmpty(assetId))
                {
                    MessageBox.Show("请输入需要check的AssetId再点按钮!");
                    return;
                }
                //检验数据
                List<OnSellAssetResponse> onSellAssetResponse = await this.GetAllAssetCheckInfos(new List<string> { assetId });
                if (onSellAssetResponse.Count == 0)
                {
                    MessageBox.Show("获取资产信息异常！请重试!");
                    return;
                }
                this.btn_CheckOneAsset.Enabled = false;
                this.lbl_CheckOneAssetInfo.Text = "正在检测数据,请稍等......";
                Tuple<bool, int, string> result = await this.CheckOneDiskAssetInfo(assetId, onSellAssetResponse[0]);
                //判断数据
                this.lbl_CheckOneAssetInfo.Text = result.Item3;
                this.btn_CheckOneAsset.Enabled = true;
            }
            catch (Exception exception)
            {
                this.lbl_CheckOneAssetInfo.Text = "发生错误信息" + exception.Message;
                this.btn_CheckOneAsset.Enabled = false;
            }
        }

        private void CheckDiskAssetInfo_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     检验磁盘资产信息
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="sellAssetResponse"></param>
        /// <returns></returns>
        private async Task<Tuple<bool, int, string>> CheckOneDiskAssetInfo(string assetId, OnSellAssetResponse sellAssetResponse)
        {
            try
            {
                //获取本金之和
                AssetUserRelationResponse assetUserRelationResponse = await this.GetAssetUserInfo(assetId);
                if (assetUserRelationResponse == null)
                {
                    Logger.Logw(assetId + "错误信息：获取资产用户比例数据异常或者该资产用户比例关系未load到disk中", 4);
                    return Tuple.Create(false, (int)CheckStatus.CheckError, "获取资产用户比例数据异常或者该资产用户比例关系未load到disk中");
                }
                //获取数据
                long originalTotalPrincipal = assetUserRelationResponse.SumCapital;
                //计算数据
                long computerTotalPrincipal = assetUserRelationResponse.UserAssetRatioDtos.Sum(u => u.Capital);
                if (originalTotalPrincipal != computerTotalPrincipal)
                {
                    Logger.Logw(assetId + "错误信息：该资产的本金之和SumCapital与从资产用户关系比例Sum出来的本金之和不正确", 4);
                    return Tuple.Create(false, (int)CheckStatus.CheckFail, "该资产的本金之和SumCapital与从资产用户关系比例Sum出来的本金之和不正确");
                }
                //比较资产的总融资金额-本金之和==剩余融资金额
                if (sellAssetResponse == null)
                {
                    Logger.Logw(assetId + "错误信息：获取资产信息异常", 4);
                    return Tuple.Create(false, (int)CheckStatus.CheckError, "获取资产信息异常");
                }
                if (sellAssetResponse.CalculatedAmount - originalTotalPrincipal != sellAssetResponse.RemainderTotal)
                {
                    Logger.Logw(assetId + "错误信息：资产的总融资金额-本金之和!=剩余融资金额", 4);
                    return Tuple.Create(false, (int)CheckStatus.CheckFail, "验证完毕,该资产的总融资金额-本金之和 != 剩余融资金额");
                }
                return Tuple.Create(true, (int)CheckStatus.CheckSuccess, "验证完毕,该资产数据无异常!");
            }
            catch (Exception ex)
            {
                Logger.LoadData(@"CheckDiskAsset\Error.txt", $"错误信息：{ex.Message}");
                return Tuple.Create(false, (int)CheckStatus.CheckError, ex.Message);
            }
        }

        /// <summary>
        ///     获取所有的资产检验信息
        /// </summary>
        /// <returns></returns>
        private async Task<List<OnSellAssetResponse>> GetAllAssetCheckInfos(List<string> assetIds)
        {
            try
            {
                HttpResponseMessage httpResponseMesageAssetInfo = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/GetAssetListByAssetIds", assetIds.ToArray());
                ResultResponse<List<OnSellAssetResponse>> responseAssetInfo = await httpResponseMesageAssetInfo.Content.ReadAsAsync<ResultResponse<List<OnSellAssetResponse>>>();
                return responseAssetInfo.Result;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"CheckDiskAsset\Error.txt", e.Message + "----" + e.StackTrace);
                return new List<OnSellAssetResponse>();
            }
        }

        /// <summary>
        ///     获取所有需要check的assetids
        /// </summary>
        /// <returns></returns>
        private List<string> GetAllCheckAssetIds()
        {
            try
            {
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchAssetAzureTable);
                TableQuery<OnSellAssetDto> queryOnSellAsset = new TableQuery<OnSellAssetDto>()
                    .Where("IsLock eq false");
                return assertIdsTable.ExecuteQuery(queryOnSellAsset).Select(a => a.OnSellAssetId).ToList();
            }
            catch (Exception e)
            {
                Logger.LoadData(@"CheckDiskAsset\Error.txt", e.Message + "----" + e.StackTrace);
                return new List<string>();
            }
        }

        /// <summary>
        ///     获取资产用户比例的金额信息
        /// </summary>
        /// <returns></returns>
        private async Task<AssetUserRelationResponse> GetAssetUserInfo(string assetId)
        {
            try
            {
                HttpResponseMessage httpResponseMesage = await this.httpClient.GetAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/GetDiskByUserAssetRatiosByAssetId?assetId={assetId}");
                ResultResponse<AssetUserRelationResponse> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<AssetUserRelationResponse>>();
                return response.Result;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"CheckDiskAsset\Error.txt", e.Message + "----" + e.StackTrace);
                return null;
            }
        }

        private void UpdateTxtInfo(TextBox txb)
        {
            lock (this.objLock)
            {
                txb.Text = (Convert.ToInt32(txb.Text.Trim()) + 1).ToString();
            }
        }
    }
}