using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using Microsoft.WindowsAzure.Storage.Table;
using RedisToTableTool.Model;
using UserToAssetTool;

namespace RedisToTableTool
{
    public partial class ReloadUserAssetRatioForm : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly object objLock1 = new object();
        private readonly CloudTableClient tableClient;

        public ReloadUserAssetRatioForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromMinutes(10);
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        private async void btn_Reload_Click(object sender, EventArgs e)
        {
            try
            {
                int insertAssertUserRangeStartInfo = Convert.ToInt32(this.txb_AssertRangeStart.Text.Trim());
                int insertAssertUserRangeEndInfo = Convert.ToInt32(this.txb_AssertRangeEnd.Text.Trim());
                //从azureTable中获取数据
                if (insertAssertUserRangeStartInfo != 0 && insertAssertUserRangeEndInfo == 0)
                {
                    MessageBox.Show("资产Range格式不正确");
                    return;
                }
                if (insertAssertUserRangeStartInfo > insertAssertUserRangeEndInfo)
                {
                    MessageBox.Show("资产Range开始数值必须小于结束数值");
                    return;
                }
                //
                CloudTable assertIdsTable = this.tableClient.GetTableReference(this.loadAppSettings.SearchAssetAzureTable);
                TableQuery<OnSellAssetDto> queryOnSellAsset = new TableQuery<OnSellAssetDto>()
                    .Where("IsLock eq false");
                List<string> listAssetIds = assertIdsTable.ExecuteQuery(queryOnSellAsset).OrderBy(p => p.RemainderTotal).Skip(insertAssertUserRangeStartInfo).Take(insertAssertUserRangeEndInfo - insertAssertUserRangeStartInfo).Select(a => a.OnSellAssetId).ToList();
                //直接执行
                if (listAssetIds.Count == 0)
                {
                    MessageBox.Show("没有获取到需要reload的资产id");
                    return;
                }
                //执行
                this.txb_userSNums.Text = "0";
                this.txb_userFNums.Text = "0";
                this.btn_Reload.Enabled = false;
                this.btn_ReloadTxt.Enabled = false;
                this.lbl_showMsg.Text = "信息提示：正在开始Reload......";
                this.ReloadData(listAssetIds, 0);
            }
            catch (Exception exception)
            {
                this.btn_ReloadTxt.Enabled = true;
                this.txb_userSNums.Text = "0";
                this.txb_userFNums.Text = "0";
                this.btn_Reload.Enabled = true;
                this.lbl_showMsg.Text = "信息提示：发生了错误" + exception.Message;
                Logger.LoadData(@"ReloadYunTableToDisk\Error.txt", exception.Message + "---------" + exception.StackTrace);
            }
        }

        /// <summary>
        ///     reload数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_ReloadTxt_Click(object sender, EventArgs e)
        {
            try
            {
                //获取txt中的数据
                if (!File.Exists(@"ReloadYunTableToDisk\ErrorAssetIds.txt"))
                {
                    MessageBox.Show("存储ErrorAssetIds的文件不存在.....");
                    return;
                }
                string[] assetIds = File.ReadAllLines(@"ReloadYunTableToDisk\ErrorAssetIds.txt");
                if (assetIds.Length == 0)
                {
                    MessageBox.Show("没有获取到需要reload的资产id");
                    return;
                }
                List<string> allReloadIds = assetIds.ToList();
                //成功的
                List<string> listSuccess = new List<string>();
                if (File.Exists(@"ReloadYunTableToDisk\TxtSuccessAssetIds.txt"))
                {
                    //执行数据
                    string[] assetIdsSuccess = File.ReadAllLines(@"ReloadYunTableToDisk\TxtSuccessAssetIds.txt");
                    if (assetIdsSuccess.Length > 0)
                    {
                        listSuccess = assetIdsSuccess.ToList();
                    }
                }
                if (listSuccess.Count > 0)
                {
                    //去除
                    allReloadIds.RemoveAll(id => listSuccess.Contains(id));
                }
                //执行数据 删除这个数据
                //File.Delete("ReloadYunTableToDisk/ErrorAssetIds.txt");
                this.txb_userSNums.Text = "0";
                this.txb_userFNums.Text = "0";
                this.btn_Reload.Enabled = false;
                this.btn_ReloadTxt.Enabled = false;
                this.lbl_showMsg.Text = "信息提示：正在开始Reload......";
                this.ReloadData(allReloadIds, 1);
            }
            catch (Exception exception)
            {
                this.btn_ReloadTxt.Enabled = true;
                this.txb_userSNums.Text = "0";
                this.txb_userFNums.Text = "0";
                this.btn_Reload.Enabled = true;
                //this.lbl_showMsg.Text = "信息提示：发生了错误" + exception.Message;
                Logger.LoadData(@"ReloadYunTableToDisk\Error.txt", exception.Message + "---------" + exception.StackTrace);
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     将yuntable数据reload到磁盘中
        /// </summary>
        /// <param name="assetIds"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private void ReloadData(List<string> assetIds, int type)
        {
            try
            {
                for (int i = 0; i < this.loadAppSettings.ReloadTableDataToDiskThreadNums; i++)
                {
                    Thread thread = new Thread(async async =>
                    {
                        while (assetIds.Count > 0)
                        {
                            string assetId = string.Empty;
                            if (assetIds.Count > 0)
                            {
                                lock (this.objLock)
                                {
                                    if (assetIds.Count > 0)
                                    {
                                        assetId = assetIds[0];
                                        assetIds.Remove(assetId);
                                    }
                                }
                                if (!string.IsNullOrEmpty(assetId))
                                {
                                    //执行assetId
                                    bool result = await this.ReloadTableDataToDisk(assetId);
                                    //执行
                                    lock (this.objLock1)
                                    {
                                        //将错误和正确的都显示并将错误的放入到txt中
                                        if (result)
                                        {
                                            this.txb_userSNums.Text = (Convert.ToInt32(this.txb_userSNums.Text) + 1).ToString();
                                            if (type == 1)
                                            {
                                                Logger.LoadData(@"ReloadYunTableToDisk\TxtSuccessAssetIds.txt", assetId);
                                            }
                                        }
                                        else
                                        {
                                            this.txb_userFNums.Text = (Convert.ToInt32(this.txb_userFNums.Text) + 1).ToString();
                                            if (type == 0)
                                            {
                                                //放入到txt中
                                                Logger.LoadData(@"ReloadYunTableToDisk\ErrorAssetIds.txt", assetId);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                Thread thWhile = new Thread(() =>
                {
                    while (assetIds.Count > 0)
                    {
                        Thread.Sleep(2000);
                    }
                    Thread.Sleep(1000);
                    this.lbl_showMsg.Text = "信息提示：已经全部Reload";
                    this.btn_ReloadTxt.Enabled = true;
                    this.btn_Reload.Enabled = true;
                });
                thWhile.IsBackground = true;
                thWhile.Start();
            }
            catch (Exception ex)
            {
                Logger.LoadData(@"ReloadYunTableToDisk\Error.txt", ex.Message + "---------" + ex.StackTrace);
            }
        }

        /// <summary>
        ///     单个资产的资产用户比例数据reload到磁盘中
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        private async Task<bool> ReloadTableDataToDisk(string assetId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await this.httpClient.GetAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/ReloadTableToDiskByUserAssetRatio/{assetId}");
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"ReloadYunTableToDisk\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }
    }
}