using System;
using System.Net.Http;
using System.Threading.Tasks;
using J.Base.Lib;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public static class AssetApiService
    {
        public static async Task<bool> Add<T>(string url, T draftBillAddRequest)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                string b = draftBillAddRequest.ToJson();
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}", draftBillAddRequest);
                return httpResponseMesage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //log记录日志
                return false;
            }
        }

        public static async Task<bool> AddNoParam(string url)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                httpResponseMesage = await InitialHttpClient().PostAsJsonAsync($"{url}", "");
                //var result = await httpResponseMesage.Content.ReadAsStringAsync();

                return httpResponseMesage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                //log记录日志
                return false;
            }
        }

        public static async Task<T> Get<T>(string url)
        {
            try
            {
                HttpResponseMessage httpResponseMesage = await InitialHttpClient().GetAsync($"{url}");
                if (httpResponseMesage != null)
                {
                    var result = await httpResponseMesage.Content.ReadAsAsync<CommonResult<T>>();
                    return result.Result;
                }
            }
            catch (Exception ex)
            {
            }
            return default(T);
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