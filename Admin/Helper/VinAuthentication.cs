using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Web;
using System.Web.Security;
using Admin.Helper;
using Contract.User;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using Admin.Common;
using Caching;
using System.Collections.Generic;
using VG.Common;
using Caching.Core;
using System.Threading.Tasks;

namespace Admin.Helper
{
    public class VinAuthentication
    {
        private static Lazy<IAuthenCaching> authenCaching = new Lazy<IAuthenCaching>(() => new AuthenCaching());
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void UserSignIn(LoginContract loginResponse, HttpContextBase _curentHttpContext, MemberExtendContract memberExtendContract, bool isLogon = false)
        {
            var setTimeExpire = DateTime.Now.AddMinutes(30);
            if (memberExtendContract != null && (memberExtendContract.Roles.Any(x => x.RoleId == SettingValues.RoleViewSurgerySchedule) || memberExtendContract.Roles.Any(x => x.RoleName.ToLower().Contains(SettingValues.RoleNameViewSurgerySchedule.ToLower()))))
            {
                setTimeExpire = DateTime.Now.AddMonths(3);
                //setTimeExpire = DateTime.Now.AddMinutes(5);
            }
            loginResponse.ClientIp = SessionHelper.GetUser_IP(_curentHttpContext);
            //loginResponse.ClientIp = SessionHelper.GetRemoteIPAddress(HttpContext.Current, true);
            //loginResponse.ClientIp = SessionHelper.GetLocalIPAddress();
            FormsAuthenticationTicket loginToken = null;
            loginResponse.IsLogon = true;
            if (isLogon)
            {
                loginResponse = UserCaching.Instant.AddUserModel(loginResponse, isLogon);

                loginToken = new FormsAuthenticationTicket(1, AdminGlobal.SignInToken,
                DateTime.Now, setTimeExpire,
                true, JsonConvert.SerializeObject(loginResponse));
            }
            else
            {
                loginResponse.TokenKey = StringUtil.RandomString(20);
                loginToken = new FormsAuthenticationTicket(1, AdminGlobal.SignInToken,
                DateTime.Now, setTimeExpire,
                true, JsonConvert.SerializeObject(loginResponse));

                UserCaching.Instant.AddUserModel(loginResponse);
            }
            authenCaching.Value.UpdateLoginLogout(loginResponse, isLogon);
            SetAuthCookie(_curentHttpContext, loginToken, AdminGlobal.SignInToken);
        }
        //public static void UserSignIn(LoginContract loginResponse, HttpContextBase _curentHttpContext, MemberExtendContract memberExtendContract)
        //{
        //    var setTimeExpire = DateTime.Now.AddMinutes(30);
        //    if (memberExtendContract != null && (memberExtendContract.Roles.Any(x => x.RoleId == SettingValues.RoleViewSurgerySchedule) || memberExtendContract.Roles.Any(x => x.RoleName.ToLower().Equals(SettingValues.RoleNameViewSurgerySchedule.ToLower()))))
        //    {
        //        setTimeExpire = DateTime.Now.AddMonths(3);
        //        //setTimeExpire = DateTime.Now.AddMinutes(5);
        //        loginResponse.IsViewPublic = true;
        //    }
        //    loginResponse.TokenKey = StringUtil.RandomString(20);
        //    loginResponse.ClientIp = SessionHelper.GetUser_IP(_curentHttpContext);
        //    var loginToken = new FormsAuthenticationTicket(1, AdminGlobal.SignInToken,
        //        DateTime.Now, setTimeExpire,
        //        true, JsonConvert.SerializeObject(loginResponse));

        //    loginResponse.IsLogon = true;
        //    UserCaching.Instant.AddUserModel(loginResponse);
        //    SetAuthCookie(_curentHttpContext, loginToken, AdminGlobal.SignInToken);
        //}
        public static bool IsLogon(HttpContextBase _curentHttpContext,string placeCheck = "AnyWhere")
        {
            try {
                
                var LoginTokenCookie = _curentHttpContext.Request.Cookies[AdminGlobal.SignInToken];
                if (LoginTokenCookie != null && LoginTokenCookie.Value != "")
                {
                    var token = FormsAuthentication.Decrypt(LoginTokenCookie.Value);
                    var userApp = JsonConvert.DeserializeObject<LoginContract>(token.UserData);
                    if (userApp == null || (userApp != null && UserCaching.Instant.GetUserModel(userApp.NickName) == null))
                    {
                        //log.Debug(string.Format("Check IsLogon at {0}, status: {1}", placeCheck, (userApp!=null? "UserApp null or LoginContract object not exist. TokenKey "+ userApp.TokenKey + ".": "UserApp null or LoginContract object not exist.")));
                        return false;
                    }
                    else
                    {
                        var setTimeExpire = DateTime.Now.AddMinutes(30);
                        if(userApp.IsViewPublic)
                            setTimeExpire = DateTime.Now.AddMonths(3);
                        var loginToken = new FormsAuthenticationTicket(1, AdminGlobal.SignInToken,
                        DateTime.Now, setTimeExpire,
                        true, JsonConvert.SerializeObject(userApp));
                        SetAuthCookie(_curentHttpContext, loginToken, AdminGlobal.SignInToken);
                        return true;
                    }
                }
                else
                {
                    //log.Debug(string.Format("Check IsLogon at {0}, status: {1}", placeCheck, "SignInToken null"));
                    return false;
                }

            } catch(Exception ex) {
                //log.Debug(string.Format("Check IsLogon at {0}. Error: {1}", placeCheck, ex));
                SignOutLocal(_curentHttpContext);
                return false;
            }
        }

        public static LoginContract CurrentUser(HttpContextBase _curentHttpContext)
        {
            LoginContract userApp = null;
            var LoginTokenCookie = _curentHttpContext.Request.Cookies[AdminGlobal.SignInToken];
            if (LoginTokenCookie != null)
            {
                try
                {
                    var token = FormsAuthentication.Decrypt(LoginTokenCookie.Value);
                    userApp = JsonConvert.DeserializeObject<LoginContract>(token.UserData);
                    if (userApp == null || (userApp != null && UserCaching.Instant.GetUserModel(userApp.NickName) == null))
                    {
                        //log.Debug(string.Format("CurrentUser, status {0}", (userApp != null ? "UserApp null or LoginContract object not exist. TokenKey " + userApp.TokenKey + "." : "UserApp null or LoginContract object not exist.")));
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //log.Debug("CurrentUser error: " + ex);
                    CommonHelper.CatchExceptionToLog(ex, "Admin.VinAuthentication", MethodBase.GetCurrentMethod().Name);
                }
                
            }
            return userApp;
        }

        private static void SetAuthCookie(HttpContextBase httpContext,
            FormsAuthenticationTicket authenticationTicket, string CookieName)
        {
            var encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
            HttpCookie cookie = httpContext.Request.Cookies[CookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(CookieName, encryptedTicket);
                cookie.Expires = authenticationTicket.Expiration;
                cookie.HttpOnly = true;
                httpContext.Response.Cookies.Add(cookie);
            }else
            {
                httpContext.Response.Cookies[CookieName].Path = cookie.Path;
                httpContext.Response.Cookies[CookieName].Value = encryptedTicket;
                httpContext.Response.Cookies[CookieName].Expires = authenticationTicket.Expiration;
                //httpContext.Request.Cookies[CookieName].Path = cookie.Path;
                //httpContext.Request.Cookies[CookieName].Value = encryptedTicket;
                //httpContext.Request.Cookies[CookieName].Expires = authenticationTicket.Expiration;
            }
        }

        public static void SignOutLocal(HttpContextBase httpContext)
        {
            var cookie = new HttpCookie(AdminGlobal.SignInToken);
            DateTime nowDateTime = DateTime.Now;
            cookie.Expires = nowDateTime.AddDays(-1);
            httpContext.Response.Cookies.Add(cookie);

            //cookie OR
            var cookieOR = new HttpCookie(AdminGlobal.SignInTokenVisitOR);
            cookieOR.Expires = nowDateTime.AddDays(-1);
            httpContext.Response.Cookies.Add(cookieOR);

            //cookie OR Signin
            var cookieSignin = new HttpCookie(AdminGlobal.SignInToken);
            cookieSignin.Expires = nowDateTime.AddDays(-1);
            httpContext.Response.Cookies.Add(cookieSignin);

            //Cookie OR Link Active Mail
            //var cookieORMail = new HttpCookie(AdminGlobal.TokenVisitORLink);
            //cookieOR.Expires = nowDateTime.AddDays(-1);
            //httpContext.Response.Cookies.Add(cookieORMail);

            //FormsAuthentication.SignOut();
            //httpContext.Session.Abandon();
            //HttpContext.Current.Session.Abandon();
            RemoveUserModel(httpContext);
        }

        public static void setVisitORCookie(LoginContract loginResponse, HttpContextBase _curentHttpContext)
        {
            //var tokenVisit = new FormsAuthenticationTicket(1, AdminGlobal.SignInTokenVisitOR,
            //    DateTime.Now, DateTime.Now.AddHours(24),
            //    true, JsonConvert.SerializeObject(loginResponse));
            var tokenVisit = new FormsAuthenticationTicket(1, AdminGlobal.SignInTokenVisitOR,
                DateTime.Now, DateTime.Now.AddMinutes(30),
                true, JsonConvert.SerializeObject(loginResponse));

            SetAuthCookie(_curentHttpContext, tokenVisit, AdminGlobal.SignInTokenVisitOR);
        }


        /// <summary>
        /// Lấy thông tin tên công ty từ AD
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public static DirectoryEntry GetADUserInfo(string userName, string domainName)
        {
            try
            {
                //Create a shortcut to the appropriate Windows domain
                PrincipalContext domainContext = new PrincipalContext(ContextType.Domain,
                                                                      domainName);

                //Create a "user object" in the context
                UserPrincipal user = new UserPrincipal(domainContext);

                //Specify the search parameters
                user.SamAccountName = userName;

                //Create the searcher
                //pass (our) user object
                PrincipalSearcher pS = new PrincipalSearcher();
                pS.QueryFilter = user;

                //Perform the search
                PrincipalSearchResult<Principal> results = pS.FindAll();

                //If necessary, request more details
                Principal pc = results.ToList()[0];
                DirectoryEntry de = (DirectoryEntry)pc.GetUnderlyingObject();

                return de;//.Properties["company"].Value.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static void LogOutFromBase(string userId)
        {
            LoginContract entity = new LoginContract() ;
            entity.IsLogon = false;
            entity.TokenKey = string.Empty;
            entity.NickName = userId;
            authenCaching.Value.UpdateLogout(entity);
        }
        public static void RemoveUserModel(HttpContextBase _curentHttpContext)
        {
            var LoginTokenCookie = _curentHttpContext.Request.Cookies[AdminGlobal.SignInToken];
            if (LoginTokenCookie != null)
            {
                try
                {
                    var token = FormsAuthentication.Decrypt(LoginTokenCookie.Value);
                    LoginContract userApp = JsonConvert.DeserializeObject<LoginContract>(token.UserData);
                    if (userApp != null)
                    {
                        string userKey = userApp.NickName;
                        string keyCaches = string.Format("{0}", CachingConstant.CachingKey.ListUser);
                        LoginContract entity = null;
                        if (!string.IsNullOrEmpty(userKey))
                        {
                            var listResult = BaseCaching.Instant.GetValue2<List<LoginContract>>(keyCaches);
                            if (listResult != null && listResult.Count > 0)
                            {
                                entity = listResult.Where(x => (!string.IsNullOrEmpty(x.NickName) && x.NickName.ToLower().Equals(userKey.ToLower()))
                                 || (!string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(userKey.ToLower()))
                                 //|| (!string.IsNullOrEmpty(x.ClientIp) && x.ClientIp.ToLower().Equals(userKey.ToLower()))
                                 ).FirstOrDefault();
                                if (entity != null)
                                {
                                    listResult.Remove(entity);
                                    //Upgrade status logon on Database
                                    entity.IsLogon = false;
                                    entity.TokenKey = string.Empty;
                                    authenCaching.Value.UpdateLoginLogout(entity);
                                }
                                //Insert to Cache
                                BaseCaching.Instant.Add2(keyCaches, listResult, 8);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //CommonHelper.CatchExceptionToLog(ex, "Admin.VinAuthentication", MethodBase.GetCurrentMethod().Name);
                }

            }
        }
        #region Function helper 4 check  valid request, ex: Have many login fail
        public static int UpgradeUserModelLoginFail(string userName, HttpContextBase httpContext, bool reset = false)
        {
            int countFail = 0;
            string clientIp = SessionHelper.GetUser_IP(httpContext);
            //string clientIp = SessionHelper.GetRemoteIPAddress(HttpContext.Current, true);
            //string clientIp = SessionHelper.GetLocalIPAddress();
            LoginContract entity = (!string.IsNullOrEmpty(userName)) ? UserCaching.Instant.GetUserModel(userName) :
                UserCaching.Instant.GetUserModel(clientIp);
            if (entity == null)
                entity = UserCaching.Instant.GetUserModel(clientIp);
            if (entity != null)
            {
                if (reset)
                    entity.CurrentLoginFail = 0;
                else
                    entity.CurrentLoginFail++;
                countFail = entity.CurrentLoginFail;
                entity.ClientIp = clientIp;
            }
            else
            {
                if (reset)
                    countFail = 0;
                else
                    countFail = 1;
                entity = new LoginContract()
                {
                    NickName = userName,
                    ClientIp = clientIp,
                    CurrentLoginFail = countFail
                };
            }
            entity.IsLogon = reset;
            UserCaching.Instant.AddUserModel(entity);
            return countFail;
        }
        public static int GetUserModel(string userName, HttpContextBase httpContext)
        {
            int countFail = 0;
            string clientIp = SessionHelper.GetUser_IP(httpContext);
            //string clientIp = SessionHelper.GetRemoteIPAddress(HttpContext.Current,true);
            //string clientIp = SessionHelper.GetLocalIPAddress();
            LoginContract entity = UserCaching.Instant.GetUserModel(userKey:userName);
            if (entity == null)
                entity = UserCaching.Instant.GetUserModel(ipClient:clientIp);
            if (entity != null)
            {
                countFail = entity.CurrentLoginFail;
            }
            return countFail;
        }
    
        #endregion Function helper 4 check  valid request, ex: Have many login fail
    }

    public class SessionViewModel
    {
        public int Id { get; set; }

        public string Birthday { get; set; }

        public string MobileNumber { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string CreatedOnUtc { get; set; }

        public string LastLoginDateUtc { get; set; }

        public string LastActivityDateUtc { get; set; }

        public string Username { get; set; }

        public string CheckSum { get; set; }

        public string EncryptData { get; set; }

        public short Status { get; set; }
    }
}