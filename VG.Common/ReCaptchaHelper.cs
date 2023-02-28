using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VG.Common
{
    public class ReCaptchaHelper
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static bool IsReCaptchValid(string EncodedResponse)
        {
            var result = false;
            try {
                string PrivateKey = AppUtils.ReCaptChaSecretKey;
                var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
                var requestUri = string.Format(apiUrl, PrivateKey, EncodedResponse);
                var request = (HttpWebRequest)WebRequest.Create(requestUri);
                if (!string.IsNullOrEmpty(AppUtils.ProxyAddress))
                {
                    WebProxy wp = new WebProxy(AppUtils.ProxyAddress);
                    request.Proxy = wp;
                }
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                    {
                        JObject jResponse = JObject.Parse(stream.ReadToEnd());
                        var isSuccess = jResponse.Value<bool>("success");
                        result = (isSuccess) ? true : false;
                    }
                }
            } catch(Exception ex)
            {
                log.Debug(ex);
            }
            return result;
        }
        public static string Validate(string EncodedResponse)
        {
            var client = new System.Net.WebClient
            {
                UseDefaultCredentials = true
            };

            //string PrivateKey = "6Le-TpgUAAAAAMCss4Qg_VpvNjDXZ5kjuOSLXEJ4";
            string PrivateKey = AppUtils.ReCaptChaSecretKey;
            try
            {
                if (!string.IsNullOrEmpty(AppUtils.ProxyAddress))
                {
                    WebProxy wp = new WebProxy(AppUtils.ProxyAddress);
                    client.Proxy = wp;
                }
                var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));
                var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptchaHelper>(GoogleReply);
                return captchaResponse.Success.ToLower();
            }
            catch(Exception ex)
            {
                log.Debug("EncodedResponse: "+ EncodedResponse);
                log.Debug("ReCaptCha Secret Key: " + PrivateKey);
                log.Debug(ex);
                return string.Empty;
            }
        }

        [JsonProperty("success")]
        public string Success
        {
            get { return m_Success; }
            set { m_Success = value; }
        }

        private string m_Success;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_ErrorCodes; }
            set { m_ErrorCodes = value; }
        }
        private List<string> m_ErrorCodes;
    }
}
