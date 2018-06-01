using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WindowsFormsMigrationData;
using J.Base.Lib;
using JinYinMao.Business.Assets.Request;
using RedisToTableTool.Model;

namespace RedisToTableTool.JymService
{
    /// <summary>
    ///     余额猫服务接口
    /// </summary>
    public static class YemApiService
    {
        public static async Task<bool> Booking(string url, BookInvestingModel bookInvestingModel)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}Yem/Order/Booking", bookInvestingModel);
                return httpResponseMesage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //log记录日志
                return false;
            }
        }

        //确认放款成功
        public static async Task<bool> ConfirmAdvanceDebt(string url, ConfirmAdvanceDebtRequest request, string logFolderName)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}MessageFromBank/ConfirmAdvanceDebt", request);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                if (response.IsTrue)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(Path.Combine(logFolderName, "Error.txt"), ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync() + "----" + request.ToJson());
                return false;
            }
        }

        /// <summary>
        ///     债转确认消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <param name="logFolderName"></param>
        /// <returns></returns>
        public static async Task<bool> ConfirmDebt(string url, NotifyBatchCreditRequest request, string logFolderName)
        {
            //MessageFromBank/ConfirmDebt
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}MessageFromBank/ConfirmDebt", request);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                if (response.IsTrue)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(Path.Combine(logFolderName, "Error.txt"), ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync() + "----" + request.ToJson());
                return false;
            }
        }

        public static async Task<OnSellAssetOutput> GetOnSellAssetInfoByAssetId(string url, string assetId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await InitialHttpClient().GetAsync($"{url}/AssetOperation/GetOnSellAssetInfo?assetId=" + assetId);
                ResponseResult<OnSellAssetOutput> response = await httpResponseMesage.Content.ReadAsAsync<ResponseResult<OnSellAssetOutput>>();
                return response.data;
            }
            catch (Exception ex)
            {
                //log记录日志
                return null;
            }
        }

        public static async Task<bool> InsertOnSellAssets(string url, List<InsertOnSellAssetInput> insertOnSellAssetInputs)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}AssetOperation/InsertOnSellAssets", insertOnSellAssetInputs);
                return httpResponseMesage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //log记录日志
                return false;
            }
        }

        //购买债转通知交易系统
        public static async Task<Tuple<bool, string>> PurchaseSendToTrader(string url, TifisfalPurchaseRequestModel request, string logFolderName)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}MessageToTifisfal/PurchaseSendToTrader", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return new Tuple<bool, string>(true, "成功");
                }
                return new Tuple<bool, string>(false, response.Message);
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(Path.Combine(logFolderName, "Error.txt"), ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync() + "----" + request.ToJson());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        //赎回通知交易系统/MessageToTifisfal/RedemptionSendToTrader
        public static async Task<Tuple<bool, string>> RedemptionSendToTrader(string url, TirisfalUserRedemptionInfoModel request, string logFolderName)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}MessageToTifisfal/RedemptionSendToTrader", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return new Tuple<bool, string>(true, "成功");
                }
                return new Tuple<bool, string>(false, response.Message);
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(Path.Combine(logFolderName, "Error.txt"), ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync() + "----" + request.ToJson());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        //调用放款
        public static async Task<bool> SendAdvanceDebt(string url, AdvanceDebtRequest request, string logFolderName)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}DebtForBank/SendAdvanceDebt", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(Path.Combine(logFolderName, "Error.txt"), ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync() + "----" + request.ToJson());
                return false;
            }
        }

        public static async Task<bool> SendDebtForBank(string url, SendDebtForBankModel request, string logFolderName)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}DebtForBank/SendDebtForBank", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(Path.Combine(logFolderName, "Error.txt"), ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync() + "----" + request.ToJson());
                return false;
            }
        }

        private static HttpClient InitialHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["YemApiUrl"]);
            httpClient.Timeout = TimeSpan.FromMinutes(2);
            return httpClient;
        }
    }
}