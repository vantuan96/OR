using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Admin.Helper
{
    public class ApiHelper
    {
        public static HttpClient client;
        static ApiHelper()
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
        }
        public static async Task<HttpResponseMessage> HttpGet(string uri, string token = "")
        {
            var url = ConfigurationManager.AppSettings["API_ManageApp_URL"] + uri != null ? ConfigurationManager.AppSettings["API_ManageApp_URL"].ToString() + uri : "";

            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await client.GetAsync(url);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }
        }
        public static async Task<HttpResponseMessage> HttpPost(string uri,Dictionary<string,string> model ,string token = "")
        {
            var url = ConfigurationManager.AppSettings["API_ManageApp_URL"] + uri != null ? ConfigurationManager.AppSettings["API_ManageApp_URL"].ToString() + uri : "";

            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var data = JsonConvert.SerializeObject(model);
                var buffer = System.Text.Encoding.UTF8.GetBytes(data);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return await client.PostAsync(url, byteContent);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }
        }
    }
}