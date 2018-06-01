using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using WindowsFormsMigrationData;
using RedisToTableTool.Model;

namespace RedisToTableTool.JymService
{
    public class BankGatewayService
    {
        /// <summary>
        ///     获取网关数据/api/business/batchordersearch
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> BooKFreeze(BookFrezzeModel bookFrezzeModel)
        {
            try
            {
                //记录下需要插入的资产ids
                HttpResponseMessage httpResponseMesage = await InitialHttpClient().PostAsJsonAsync("api/book/freezebyapi", bookFrezzeModel);
                BankGatewayBaseResponse response = await httpResponseMesage.Content.ReadAsAsync<BankGatewayBaseResponse>();
                if (response.RespCode == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                return false;
            }
        }

        //http://jym-dev-sf.jinyinmao.com.cn/gateway/api/users/infomanage
        /// <summary>
        ///     获取网关数据
        /// </summary>
        /// <returns></returns>
        public static async Task<WebResponseBase> GetInformation(string userId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync("api/users/infomanage", new { returnUrl = "http://www.baidu.com", userId, clientType = 900 });
                WebResponseBase response = await httpResponseMesage.Content.ReadAsAsync<WebResponseBase>();
                return response;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"BankError\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return null;
            }
        }

        /// <summary>
        ///     获取网关数据/api/business/batchordersearch
        /// </summary>
        /// <returns></returns>
        public static async Task<int> GetOrderSearch(string orderId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync("api/business/ordersearch", new { orderId });
                OrderSearchResponse response = await httpResponseMesage.Content.ReadAsAsync<OrderSearchResponse>();
                switch (response.Status)
                {
                    case "S":
                        return 1;

                    case "F":
                    case null:
                        return -1;
                }
                return -2;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"BankError\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return -2;
            }
        }

        private static HttpClient InitialHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["Bankgateway"]);
            httpClient.Timeout = TimeSpan.FromMinutes(2);
            return httpClient;
        }
    }
}