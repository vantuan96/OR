using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using Admin.Models.User;
using Contract.User;

namespace Admin.Helper
{
    public class SessionHelper
    {
        public static T GetSession<T>(HttpContext context, string key)
        {
            T retSession = default(T);
            try
            {
                retSession = (T)context.Session[key];
            }
            catch (Exception ex)
            {
                throw new Exception("Could not convert Session[\"" + key + "\"] to type " + retSession.GetType().Name);
            }

            return retSession;
        }

        public static MemberExtendContract GetUserSession(HttpContext context)
        {
            return GetSession<MemberExtendContract>(context, AdminConfiguration.CurrentUserSessionKey);
        }

        public static string GetUser_IP(HttpContextBase context)
        {
            string VisitorsIPAddr = string.Empty;
            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (context.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = context.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }
        public static string GetRemoteIPAddress(HttpContext context, bool allowForwarded = true)
        {
            if (allowForwarded)
            {
                string header = (context.Request.Headers["CF-Connecting-IP"] ?? context.Request.Headers["X-Forwarded-For"]);
                if (IPAddress.TryParse(header, out IPAddress ip))
                {
                    return ip.ToString();
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
        public static ClientInfoContract GetClientInfo(HttpContextBase context, HttpBrowserCapabilitiesBase browser)
        {
            var clientInfo = new ClientInfoContract()
            {
                Browser = browser.Browser,
                Platform = browser.Platform,
                Type = browser.Type,
                Version = browser.Version,
                MobileDeviceModel = browser.MobileDeviceModel,
                IsMobileDevice = browser.IsMobileDevice,
                IPAddress = GetUser_IP(context)
                //IPAddress = GetRemoteIPAddress(HttpContext.Current, true)
                //IPAddress=GetLocalIPAddress()
            };

            return clientInfo;
        }
    }
}