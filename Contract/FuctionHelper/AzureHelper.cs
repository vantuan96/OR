using Contract.OR;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Contract.FuctionHelper
{
    public class AzureHelper
    {
        private static string TenantName { get => ConfigurationManager.AppSettings.Get("AzureTenantName"); }

        private static string ClientId { get => ConfigurationManager.AppSettings.Get("AzureClientId"); }

        private static string Redirect { get => ConfigurationManager.AppSettings.Get("AzureRedirect"); }
        private static string RedirectError { get => ConfigurationManager.AppSettings.Get("AzureRedirectError"); }

        private static string ClientSecret { get => ConfigurationManager.AppSettings.Get("ClientSecret"); }

        private static string AzurePostLogoutRedirectUri { get => ConfigurationManager.AppSettings.Get("AzurePostLogoutRedirectUri"); }

        private static string AzureAuthority { get => ConfigurationManager.AppSettings.Get("AzureAuthority"); }

        private static string Tenantid { get => ConfigurationManager.AppSettings.Get("AzureTenantId"); }


        private static string Proxy { get => ConfigurationManager.AppSettings.Get("AzureProxy"); }


        private static string iProxy { get => ConfigurationManager.AppSettings.Get("EnableIProxy"); }

        private static string Authority { get => ConfigurationManager.AppSettings.Get("AzureAuthority"); }

        private static string redirecttoken { get => ConfigurationManager.AppSettings.Get("redirecttoken"); }
        public static string GetLogoutUrl( string customRedirectUrl = "")
        {
            string r_Url = "";
            if (!string.IsNullOrWhiteSpace(customRedirectUrl))
                r_Url = customRedirectUrl;

            var url = $"{AzureAuthority}/{Tenantid}/oauth2/logout?post_logout_redirect_uri={r_Url}";

            return url;
        }

        public static Azure_User GetUserInfo(string code)
        {
            Azure_User result = null;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol =
              SecurityProtocolType.Tls12;
                var client = new RestClient(Authority  + Tenantid + "/oauth2/v2.0/token");
                if (!string.IsNullOrEmpty(Proxy))
                {
                    client.Proxy = new WebProxy(Proxy);
                }
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("client_id", ClientId);
                request.AddParameter("redirect_uri", redirecttoken);
                request.AddParameter("scope", "openid profile offline_access");
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("client_secret", ClientSecret);
                request.AddParameter("code", code);
                IRestResponse response = client.Execute(request);

                ParseJsonToModel json = JsonConvert.DeserializeObject<ParseJsonToModel>(response.Content);

                // gán token  từ azure

                result = new Azure_User()
                {
                    Id_Token = json.id_token,
                    Refresh_Token = json.refresh_token,
                    Access_Token = json.access_token
                };
                // lấy thông tin  từ email  trả ra azure 

                var idToken = ReadToken(result.Id_Token);
                var accessToken = ReadToken(result.Access_Token);

                result.Email = idToken.Claims.Where(c => c.Type == "preferred_username")
                              .Select(c => c.Value).FirstOrDefault(); // email user login 
            }
            catch { }

            return (result);
        }

        private static JwtSecurityToken ReadToken(string tokenStr)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadToken(tokenStr) as JwtSecurityToken;
            return token;
        }
    
        public static string GetLoginUrl()
        {
            string urlAzureLogin = string.Concat(Authority, Tenantid, "/oauth2/v2.0/authorize?client_id=", ClientId, "&scope=profile+email+openid&redirect_uri=",Redirect,"&response_type=code&response_mode=form_post");
            return urlAzureLogin;
        }
       
    }
}
