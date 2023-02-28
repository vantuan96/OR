using Admin.Helper;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Admin.App_Start
{
    public partial class OwinStartup
    {
        // These public properties are required to enable the partial views to access their values.

        private static string appKey = ConfigurationManager.AppSettings["ida:AppKey"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static string cookieName = CookieAuthenticationDefaults.CookiePrefix + CookieAuthenticationDefaults.AuthenticationType;
        public static readonly string Authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
        private static string metadataAddress = ConfigurationManager.AppSettings["ida:ADFSDiscoveryDoc"];
        public static int IsLogin = 0;
        public static string sessionID = string.Empty;
        public static string code = string.Empty;
        public static string currentDeviceID;
        public static string Email = "";

        public static string PostLogoutRedirectUri
        {
            get { return postLogoutRedirectUri; }
        }
        public static string email
        {
            get { return Email; }
        }
        public static string AADInstance
        {
            get { return aadInstance; }
        }

        public static string ClientId
        {
            get { return clientId; }
        }

        public static string CheckSessionIFrame
        {
            get;
            set;
        }

        public static string RedirectUri
        {
            get { return redirectUri; }
        }

        public static TicketDataFormat ticketDataFormat
        {
            get;
            set;
        }
        public static string CookieName
        {
            get { return cookieName; }
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
           
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                CookieManager = new SystemWebCookieManager()
            });
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            CookieAuthenticationOptions cookieOptions = new CookieAuthenticationOptions
            {
                CookieName = CookieName
            };

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    RedirectUri = redirectUri,
                    Authority = Authority,
                   
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        //SaveSigninToken = true,
                        ValidateIssuer = false,
                    },
                   
                    Notifications =
                        new OpenIdConnectAuthenticationNotifications
                        {
                            AuthorizationCodeReceived = OwinStartup.AuthorizationCodeRecieved,
                            AuthenticationFailed = OwinStartup.AuthenticationFailed,
                            RedirectToIdentityProvider = OwinStartup.RedirectToIdentityProvider,
                            SecurityTokenValidated = OwinStartup.SecurityTokenValidated,
                        },
                });

            IDataProtector dataProtector = app.CreateDataProtector(
                typeof(CookieAuthenticationMiddleware).FullName,
                cookieOptions.AuthenticationType, "v1");
            ticketDataFormat = new TicketDataFormat(dataProtector);
        }

        #region Notification hooks
        public string GenUniqueIdOnDevice()
        {
            ManagementObjectCollection objectList = null;
            ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher("Select * from Win32_processor");
            objectList = objectSearcher.Get();
            String id = "";
            foreach (ManagementObject obj in objectList)
            {
                id = obj["ProcessorID"].ToString();
            }
            objectSearcher = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            objectList = objectSearcher.Get();
            String mtherBoard = "";
            foreach (ManagementObject obj in objectList)
            {
                mtherBoard = (string)obj["SerialNumber"];
            }
            string uniqueId = id + mtherBoard;
            return uniqueId;
        }
        public static Task RedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            
            UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            if (notification.Request.Uri.AbsolutePath == url.Action("SessionChanged", "Authen"))
            {
                ICookieManager cookieManager = new ChunkingCookieManager();
                string cookie = cookieManager.GetRequestCookie(notification.OwinContext, CookieName);

                AuthenticationTicket ticket = ticketDataFormat.Unprotect(cookie);
                if (ticket?.Properties.Dictionary != null)
                {
                    ticket.Properties.Dictionary[OpenIdConnectAuthenticationDefaults.AuthenticationType + "SingleSignOut"] = notification.ProtocolMessage.State;
                    cookieManager.AppendResponseCookie(notification.OwinContext, CookieName, ticketDataFormat.Protect(ticket), new CookieOptions());
                }
                notification.ProtocolMessage.Prompt = "none";
                notification.ProtocolMessage.IssuerAddress = notification.OwinContext.Authentication.User.FindFirst("issEndpoint")?.Value;
                string redirectUrl = notification.ProtocolMessage.BuildRedirectUrl();
                if (ticket == null)
                {
                    notification.Response.Redirect(url.Action("SignOut", "Authen") + "?" + redirectUrl);
                }
                else
                {
                    notification.Response.Redirect(url.Action("SessionChanged", "Authen") + "?" + redirectUrl);
                }
                notification.HandleResponse();
            }

            return Task.FromResult<object>(null);
        }

        public async static Task SecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            OpenIdConnectAuthenticationOptions tenantSpecificOptions = new OpenIdConnectAuthenticationOptions();
            tenantSpecificOptions.Authority = string.Format(AADInstance, notification.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value);
            tenantSpecificOptions.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(tenantSpecificOptions.Authority + "/.well-known/openid-configuration");

            OpenIdConnectConfiguration tenantSpecificConfig = await tenantSpecificOptions.ConfigurationManager.GetConfigurationAsync(notification.Request.CallCancelled);
            notification.AuthenticationTicket.Identity.AddClaim(new Claim("issEndpoint", tenantSpecificConfig.AuthorizationEndpoint, ClaimValueTypes.String, "Admin"));
            OwinStartup.code = notification.ProtocolMessage.Code;
            OwinStartup.sessionID = notification.ProtocolMessage.SessionState;
            Email = notification.AuthenticationTicket.Identity.Name;
            CheckSessionIFrame = notification.AuthenticationTicket.Properties.Dictionary[OpenIdConnectSessionProperties.CheckSessionIFrame];
            return;
        }
        public static Task AuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            string cookieStateValue = null;
            ICookieManager cookieManager = new ChunkingCookieManager();
            string cookie = cookieManager.GetRequestCookie(notification.OwinContext, CookieName);
            string cookie1 = HttpContext.Current.Request.Cookies.Get(cookieName)?.Value;
            AuthenticationTicket ticket = ticketDataFormat.Unprotect(cookie);
            if (ticket?.Properties.Dictionary != null)
                ticket.Properties.Dictionary.TryGetValue(OpenIdConnectAuthenticationDefaults.AuthenticationType + "SingleSignOut", out cookieStateValue);

            if (cookieStateValue != null && cookieStateValue.Contains(notification.ProtocolMessage.State) || notification.Exception.Message == "login_required")
            {
                OwinStartup.IsLogin = 0;
                cookieManager.AppendResponseCookie(notification.OwinContext, CookieName, null, new CookieOptions());
                notification.Response.Redirect("Authen/SignOut");
            }
           
            
            notification.HandleResponse();
            return Task.FromResult<object>(null);
        }

        public static Task AuthorizationCodeRecieved(AuthorizationCodeReceivedNotification notification)
        {
            if (notification.AuthenticationTicket.Properties.RedirectUri.Contains("SessionChanged"))
            {
                ICookieManager cookieManager = new ChunkingCookieManager();
                string cookie = cookieManager.GetRequestCookie(notification.OwinContext, CookieName);
                AuthenticationTicket ticket = ticketDataFormat.Unprotect(cookie);
                if (ticket.Properties.Dictionary != null)
                    ticket.Properties.Dictionary[OpenIdConnectAuthenticationDefaults.AuthenticationType + "SingleSignOut"] = "";
                cookieManager.AppendResponseCookie(notification.OwinContext, CookieName, ticketDataFormat.Protect(ticket), new CookieOptions());

                Claim existingUserObjectId = notification.OwinContext.Authentication.User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");
                Claim incomingUserObjectId = notification.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");

                if (existingUserObjectId.Value != null && incomingUserObjectId != null)
                {
                    if (existingUserObjectId.Value != incomingUserObjectId.Value)
                    {
                        notification.Response.Redirect("Authen/SingleSignOut");
                        notification.HandleResponse();
                    }
                    else if (existingUserObjectId.Value == incomingUserObjectId.Value)
                    {
                     
                        notification.Response.Redirect("/");
                        notification.HandleResponse();
                    }
                }
            }

            return Task.FromResult<object>(null);
        }
        #endregion
    }
}

