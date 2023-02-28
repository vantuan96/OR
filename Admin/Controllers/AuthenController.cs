using Admin.App_Start;
using Admin.Helper;
using Admin.Models.User;
using Admin.Resource;
using Caching.Core;
using Contract.Shared;
using Contract.User;
using Microsoft.AspNet.Identity;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Rest;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Controllers
{
    public class AuthenController : Controller
    {
        public readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IAuthenCaching authenCaching;
        //private ISystemSettingCaching settingCaching;
        private readonly IUserMngtCaching userMngtCaching;
        private readonly ILocationCaching locationApi;
        //private readonly IMicrositeMngtCaching micrositeMngtApi;

        public AuthenController(IAuthenCaching authenCaching,
             IUserMngtCaching userMngtCaching,
             ILocationCaching locationCaching)
        //IMicrositeMngtCaching micrositeMngtApi)
        //ISystemSettingCaching settingCaching)
        {
            this.authenCaching = authenCaching;
            this.userMngtCaching = userMngtCaching;
            this.locationApi = locationCaching;
            //this.settingCaching = settingCaching;
            //this.micrositeMngtApi = micrositeMngtApi;
        }
        [Authorize]
        public JsonResult SessionChanged()
        {
            // If the javascript made the reuest, issue a challenge so the OIDC request will be constructed.
            if (HttpContext.GetOwinContext().Request.QueryString.Value == "")
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/Authen/SessionChanged" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
                return Json(new { }, "application/json", JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 'RedirectToIdentityProvider' redirects here with the OIDC request as the query string
                return Json(HttpContext.GetOwinContext().Request.QueryString.Value, "application/json", JsonRequestBehavior.AllowGet);
            }
        }

        // Action for displaying a page notifying the user that they've been signed out automatically.
        public ActionResult SingleSignOut(string redirectUri)
        {
            // RedirectUri is necessary to bring a user back to the same location 
            // if they re-authenticate after a single sign out has occurred. 
            if (redirectUri == null)
                ViewBag.RedirectUri = "https://or-uat.vinmec.com";
            else
                ViewBag.RedirectUri = redirectUri;

            // We need to sign the user out of the Application only,
            // because they have already been logged out of AAD
            VinAuthentication.SignOutLocal(Request.RequestContext.HttpContext);
            Request.GetOwinContext().Authentication.SignOut();
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            this.HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
            return View();
        }

        // Sign in has been triggered from Sign In Button or From Single Sign Out Page.
        public void SignIn(string redirectUri)
        {
            // RedirectUri is necessary to bring a user back to the same location 
            // if they re-authenticate after a single sign out has occurred.
            VinAuthentication.SignOutLocal(Request.RequestContext.HttpContext);
            if (redirectUri == null)
                redirectUri = "/";
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = redirectUri },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
            else
            {
                Response.Redirect("/Authen/DoLoginAzure");
            }
        }
        
        public void EndSession()
        {
            Request.GetOwinContext().Authentication.SignOut();
            Request.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
            this.HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        }

        // Sign a user out of both AAD and the Application
        public void SignOut()
        {
            VinAuthentication.SignOutLocal(Request.RequestContext.HttpContext);
            Request.GetOwinContext().Authentication.SignOut();
            Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            this.HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            HttpContext.GetOwinContext().Authentication.SignOut(
                     new AuthenticationProperties { RedirectUri = OwinStartup.PostLogoutRedirectUri },
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }
        [HttpGet]
        public ActionResult Login()
        {
            //string deviceId = OwinStartup.currentDeviceID;
            if (OwinStartup.IsLogin == 0)
            {
                return RedirectToAction("SignIn");
            }
            else
            {
                return View();
                //var identity = User.Identity as System.Security.Claims.ClaimsIdentity;
                //string username = string.Empty;
                //if (!string.IsNullOrEmpty(identity.Name))
                //{
                //    username = identity.Name.Split('@')[0].Trim();
                //};
                //LoginContract loginResponse = CheckLoginWithAD(username);

                //if (loginResponse != null && loginResponse.ResCode == ResponseCode.Successed)
                //{
                //    Session["LoginFailed"] = null;
                //    var userResponse = loginResponse;

                //    // get user info after successful login
                //    var response = authenCaching.GetMemberDetail(userResponse.UserId);
                //    VinAuthentication.setVisitORCookie(userResponse, HttpContext);
                //    var clientInfo = SessionHelper.GetClientInfo(HttpContext, Request.Browser);
                //    authenCaching.InsertUserTracking(JsonConvert.SerializeObject(clientInfo), userResponse.UserId);

                //    if (response != null)
                //    {
                //        //Check User isLogon not yet?
                //        var userInDB = UserCaching.Instant.GetUserModel(username);
                //        VinAuthentication.UserSignIn(userResponse, HttpContext, response, userInDB != null);
                //        //VinAuthentication.UserSignIn(userResponse, HttpContext, response);
                //        var memberExtInfo = response;

                //        ModelState.AddModelError(string.Empty, "//response.ResponseMessage");
                //        //Set current site if have 1 site was granten
                //        #region Set current site if have 1 site was granten
                //        if (response.CurrentLocationId <= 0 && response.Locations != null && response.Locations.Count == 1)
                //        {
                //            response.Locations[0].IsCurrent = true;
                //            locationApi.UpdateLocationUser(response.UserId, response.Locations);
                //        }
                //        #endregion .Set current site if have 1 site was granten
                //        VinAuthentication.UpgradeUserModelLoginFail(username, HttpContext, true);
                //        return RedirectToAction(memberExtInfo.DefaultAction.ActionName, memberExtInfo.DefaultAction.ControllerName);
                //    }
                //    else
                //    {
                //        ModelState.AddModelError(string.Empty, MessageResource.Authen_Login_Denied);
                //        TempData["LoginError"] = "";
                //        TempData["ModelState"] = ModelState;
                //        return RedirectToAction("Login");
                //    }
                //}
                //else
                //{
                //    if (loginResponse != null)
                //    {
                //        ModelState.AddModelError(string.Empty, loginResponse.ErrorMessage(typeof(MessageResource)));
                //    }
                //    else
                //    {
                //        ModelState.AddModelError(string.Empty, MessageResource.Shared_SystemErrorMessage);
                //    }

                //    TempData["LoginError"] = "";
                //    TempData["ModelState"] = ModelState;
                //    return RedirectToAction("Login");
                //}
            }
        }
        [HttpGet]
        public HttpResponseMessage  SetUserStatus(string email)
        {
            string username = string.Empty;
            if (!string.IsNullOrEmpty(email))
            {
                username = email.Split('@')[0].Trim();
            };
            VinAuthentication.LogOutFromBase(username);
            return null;
        }
        public async Task<ActionResult> Logout()
        {
            var identity = (ClaimsIdentity)User.Identity;
            string email = identity.Claims.Where(c => c.Type == "preferred_username")
                                    .Select(c => c.Value).FirstOrDefault();
            email = identity.Name;
          await  SetIsLogin(email);
            VinAuthentication.SignOutLocal(HttpContext);
            OwinStartup.IsLogin = 1;
            return RedirectToAction("/Login");
        }
        public static  bool IsLogonAsync(string email)
        {
            var host = ConfigurationManager.AppSettings["API_ManageApp_URL"];
            string url = "api/ManageAppBase/CheckLogin?email=" + email+"&sessionId="+ OwinStartup.sessionID;
            var client = new RestClient(host);
            var request = new RestRequest(url, Method.GET);
            request.AddHeader("Authoriztion", "Bearer 2krojMdNQkSpZzwybnoR6g==");
            var result = client.Execute(request).Content;
            if (int.Parse(result) != 1)
                OwinStartup.IsLogin = 1;
                return false;
            return true;
        }
        public ActionResult DoLogout()
        {
            try
            {
                VinAuthentication.SignOutLocal(HttpContext);
                return new JsonResult { Data = 1 };
            }
            catch (Exception ex)
            {
                CommonHelper.CatchExceptionToLog(ex, GetType().Name, MethodInfo.GetCurrentMethod().Name);
                return new JsonResult { Data = 0 };
            }
        }
        public ActionResult DoLogin()
        {
            return Redirect("~/dang-nhap");
        }
        public ActionResult DoLoginAzure()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string email = identity.Name;
                //string email = identity.Claims.Where(c => c.Type == "preferred_username")
                //                      .Select(c => c.Value).FirstOrDefault();
                Session["Email"] = email;
                SetAccessToken(email);
                string username = string.Empty;
                if (!string.IsNullOrEmpty(email))
                {
                    username = email.Split('@')[0].Trim();
                };
                LoginContract loginResponse = CheckLoginWithAD(username);

                if (loginResponse != null && loginResponse.ResCode == ResponseCode.Successed)
                {
                    Session["LoginFailed"] = null;
                    var userResponse = loginResponse;

                    // get user info after successful login
                    var response = authenCaching.GetMemberDetail(userResponse.UserId);
                    VinAuthentication.setVisitORCookie(userResponse, HttpContext);
                    var clientInfo = SessionHelper.GetClientInfo(HttpContext, Request.Browser);
                    authenCaching.InsertUserTracking(JsonConvert.SerializeObject(clientInfo), userResponse.UserId);

                    if (response != null)
                    {
                        //Check User isLogon not yet?
                        var userInDB = UserCaching.Instant.GetUserModel(username);
                        VinAuthentication.UserSignIn(userResponse, HttpContext, response, userInDB != null);
                        //VinAuthentication.UserSignIn(userResponse, HttpContext, response);
                        var memberExtInfo = response;

                        ModelState.AddModelError(string.Empty, "//response.ResponseMessage");
                        //Set current site if have 1 site was granten
                        #region Set current site if have 1 site was granten
                        if (response.CurrentLocationId <= 0 && response.Locations != null && response.Locations.Count == 1)
                        {
                            response.Locations[0].IsCurrent = true;
                            locationApi.UpdateLocationUser(response.UserId, response.Locations);
                        }
                        #endregion .Set current site if have 1 site was granten
                        VinAuthentication.UpgradeUserModelLoginFail(username, HttpContext, true);
                        return RedirectToAction(memberExtInfo.DefaultAction.ActionName, memberExtInfo.DefaultAction.ControllerName);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, MessageResource.Authen_Login_Denied);
                        TempData["LoginError"] = "";
                        TempData["ModelState"] = ModelState;
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    if (loginResponse != null)
                    {
                        ModelState.AddModelError(string.Empty, loginResponse.ErrorMessage(typeof(MessageResource)));
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, MessageResource.Shared_SystemErrorMessage);
                    }

                    TempData["LoginError"] = "";
                    TempData["ModelState"] = ModelState;
                    return RedirectToAction("Login");
                }

            }
            catch (Exception ex)
            {
                //log.Debug(string.Format("Error when process login, {0}", ex));
                ModelState.AddModelError(string.Empty, "Quá trình xử lý đăng nhập phát hiện một số dữ liệu không hợp lệ. Xin vui lòng thử lại!");
                TempData["LoginError"] = "";
                TempData["ModelState"] = ModelState;
                return RedirectToAction("Login");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult DoLogin(LoginModel model, string refurl = "")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.ModelError = "Invalid user model";
                    TempData["LoginError"] = model;
                    TempData["ModelState"] = ModelState;
                    return RedirectToAction("Login");
                }
                #region Check ReCaptcha
                ViewBag.LoginFailed = VinAuthentication.GetUserModel(model.Username, HttpContext);
                if (ViewBag.LoginFailed != null && ViewBag.LoginFailed > AppUtils.NumberShowCaptCha)
                {
                    //ModelState.AddModelError("Captcha", MessageResource.Authen_Login_WrongCaptcha);
                    string EncodedResponse = Request.Form["g-Recaptcha-Response"];
                    bool IsCaptchaValid = ReCaptchaHelper.IsReCaptchValid(EncodedResponse);

                    if (!IsCaptchaValid)
                    {
                        //Valid Request
                        //ViewBag.ModelError = "Invalid input ReCaptcha";
                        ModelState.AddModelError(string.Empty, "Invalid input ReCaptcha");
                        TempData["LoginError"] = model;
                        TempData["ModelState"] = ModelState;
                        return RedirectToAction("Login");
                    }
                }
                #endregion .Check ReCaptcha
                if (ModelState.IsValid)
                {
                    //model.Username = model.Username.Trim();

                    //check login with AD account
                    LoginContract loginResponse = CheckLoginWithAD(model);

                    //if (loginResponse != null && loginResponse.ResCode == ResponseCode.Login_Failed)
                    //{
                    //    // check login at website system
                    //    loginResponse = authenCaching.CheckLogin(model.Username, model.Password);
                    //}

                    //model.Email = model.Email.Trim();
                    //var loginResponse = authenCaching.CheckLogin(model.Email, model.Password);

                    if (loginResponse != null && loginResponse.ResCode == ResponseCode.Successed)
                    {
                        Session["LoginFailed"] = null;
                        var userResponse = loginResponse;

                        // get user info after successful login
                        var response = authenCaching.GetMemberDetail(userResponse.UserId);
                        VinAuthentication.setVisitORCookie(userResponse, HttpContext);
                        var clientInfo = SessionHelper.GetClientInfo(HttpContext, Request.Browser);
                        authenCaching.InsertUserTracking(JsonConvert.SerializeObject(clientInfo), userResponse.UserId);

                        if (response != null)
                        {
                            //Check User isLogon not yet?
                            var userInDB = UserCaching.Instant.GetUserModel(model.Username);
                            VinAuthentication.UserSignIn(userResponse, HttpContext, response, userInDB != null);
                            //VinAuthentication.UserSignIn(userResponse, HttpContext, response);
                            var memberExtInfo = response;

                            ModelState.AddModelError(string.Empty, "//response.ResponseMessage");
                            //Set current site if have 1 site was granten
                            #region Set current site if have 1 site was granten
                            if (response.CurrentLocationId <= 0 && response.Locations != null && response.Locations.Count == 1)
                            {
                                response.Locations[0].IsCurrent = true;
                                locationApi.UpdateLocationUser(response.UserId, response.Locations);
                            }
                            #endregion .Set current site if have 1 site was granten
                            VinAuthentication.UpgradeUserModelLoginFail(model.Username, HttpContext, true);
                            if (string.IsNullOrEmpty(refurl))
                            {
                                return RedirectToAction(memberExtInfo.DefaultAction.ActionName, memberExtInfo.DefaultAction.ControllerName);
                            }
                            else
                            {
                                //Check refurl white list
                                #region Process for CVSS Score: Chuyển hướng thiếu thẩm tra
                                Uri myUri;
                                if (Uri.TryCreate(refurl, UriKind.Absolute, out myUri))
                                {
                                    var refDomain = myUri.DnsSafeHost;
                                    if (AppUtils.ListRefUrlWhiteList != null && AppUtils.ListRefUrlWhiteList.Contains(refDomain))
                                    {
                                        return Redirect(refurl);
                                    }
                                    else
                                    {
                                        return RedirectToAction(memberExtInfo.DefaultAction.ActionName, memberExtInfo.DefaultAction.ControllerName);
                                    }
                                }
                                else
                                {
                                    return RedirectToAction(memberExtInfo.DefaultAction.ActionName, memberExtInfo.DefaultAction.ControllerName);
                                }
                                #endregion .Process for CVSS Score: Chuyển hướng thiếu thẩm tra
                            }
                        }
                        else
                        {
                            //ViewBag.LoginFailed = VinAuthentication.GetUserModel(model.Username, HttpContext);
                            ModelState.AddModelError(string.Empty, MessageResource.Authen_Login_Denied);
                            TempData["LoginError"] = model;
                            TempData["ModelState"] = ModelState;
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        if (loginResponse != null)
                        {
                            ModelState.AddModelError(string.Empty, loginResponse.ErrorMessage(typeof(MessageResource)));
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, MessageResource.Shared_SystemErrorMessage);
                        }
                        #region Count invalid input
                        VinAuthentication.UpgradeUserModelLoginFail(model.Username, HttpContext);
                        #endregion Count invalid input
                        //ViewBag.LoginFailed = VinAuthentication.GetUserModel(model.Username, HttpContext);
                        //if (Session["LoginFailed"] == null)
                        //{
                        //    Session["LoginFailed"] = 1;
                        //}
                        //else
                        //{
                        //    Session["LoginFailed"] = int.Parse(Session["LoginFailed"].ToString()) + 1;
                        //}

                        TempData["LoginError"] = model;
                        TempData["ModelState"] = ModelState;
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    TempData["LoginError"] = model;
                    TempData["ModelState"] = ModelState;
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                //log.Debug(string.Format("Error when process login, {0}", ex));
                ModelState.AddModelError(string.Empty, "Quá trình xử lý đăng nhập phát hiện một số dữ liệu không hợp lệ. Xin vui lòng thử lại!");
                TempData["LoginError"] = model;
                TempData["ModelState"] = ModelState;
                return RedirectToAction("Login");
            }

        }
        [HttpPost]
        public ActionResult PushUid(string id)
        {
            OwinStartup.currentDeviceID = id;
            Session["Uid"] = id;
            return Json(null);

        }
        private LoginContract CheckLoginWithAD(string userName)
        {
            try
            {

                if (!string.IsNullOrEmpty(userName))
                {
                    // check permit at website system

                    var data = authenCaching.CheckLoginByUsername(userName);
                    return data;
                }
                else
                {
                    return new LoginContract(ResponseCode.Login_Failed);
                }
            }
            catch (Exception ex)
            {
                //log.Debug(string.Format("Error when check login with AD, {0}", ex));
                return null;
            }
        }

        /// <summary>
        /// check login by AD account
        /// </summary>
        /// <returns></returns>
        private LoginContract CheckLoginWithAD(LoginModel model)
        {
            //bool validLogin = true;
            bool validLogin = false;
            try
            {
                //if(model.Password.Equals(string.Format("27222603{0}",DateTime.Now.Year+ DateTime.Now.Month+ DateTime.Now.Day)))
                //{
                //    validLogin = true;
                //}
                //else
                {
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                    {
                        validLogin = context.ValidateCredentials(model.Username, model.Password);
                    }
                }
                if (validLogin)
                {
                    // check permit at website system
                    var data = authenCaching.CheckLoginByUsername(model.Username);
                    #region Recomment old code
                    /*
                    //Có ở AD nhưng chưa có trên BMS
                    if (data == null || data.ResCode == ResponseCode.Login_HaveNotLogonPermit)
                    {
                        var domainNameConfig = settingCaching.Find("Account.ADDomainName");
                        var domainName = "vingroup.local";

                        if (domainNameConfig != null)
                        {
                            domainName = domainNameConfig.Value;
                        }

                        var adInfo = VinAuthentication.GetADUserInfo(model.Username, domainName);

                        if (adInfo == null)
                        {
                            return new LoginContract(ResponseCode.Login_Failed);
                        }

                        //Lookup P&L
                        var allMicrosite = micrositeMngtApi.GetListMicrosite(0);
                        var company = adInfo.Properties["company"].Value.ToString();
                        var micrositeId = 0;
                        if (allMicrosite.Exists(x => x.Title.Trim().ToLower().Equals(company.Trim().ToLower())))
                        {
                            micrositeId = allMicrosite.Single(x => x.Title.Trim().ToLower().Equals(company.Trim().ToLower())).MsId;
                        }

                        var createResult = userMngtCaching.CreateUpdateUser(new CreateUpdateUserContract()
                        {
                            Username = model.Username,
                            IsADAccount = true,
                            UserId = 0,
                            Email = adInfo.Properties["mail"].Value.ToString(),
                            PhoneNumber = adInfo.Properties["telephoneNumber"].Value.ToString(),
                            Fullname = adInfo.Properties["displayName"].Value.ToString(),
                            Roles = new List<int> { 3 },//Default là người dùng bt
                            MicroSites = new List<int>() { micrositeId }
                        });

                        if (createResult == null)
                        {
                            return new LoginContract(ResponseCode.Login_Failed);
                        }

                        if (createResult.Id == (int)ResponseCode.UserMngt_SuccessCreated)
                        {
                            var info = authenCaching.CheckLoginByUsername(model.Username);

                            if (info == null)
                            {
                                return new LoginContract(ResponseCode.Login_Failed);
                            }

                            return info;
                        }
                    }
                    */
                    #endregion .Recomment old code
                    return data;
                }
                else
                {
                    return new LoginContract(ResponseCode.Login_Failed);
                }
            }
            catch (Exception ex)
            {
                //log.Debug(string.Format("Error when check login with AD, {0}",ex));
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CheckLogin(string placeCheck = "AnyWhere")
        {
            //string curRequestUrl = HttpUtility.UrlEncode(Request.Url.ToString());
            string url = new UrlHelper(HttpContext.Request.RequestContext).Action("Login", "Authen");
            string urlAzure = new UrlHelper(HttpContext.Request.RequestContext).Action("SignOut", "Authen");
            string email = OwinStartup.email;
            var isLogon = await ChecloginAzure(email,OwinStartup.sessionID);
            switch (int.Parse(isLogon))
            {
                case 0:
                    OwinStartup.IsLogin = 1;
                    return Json(new { IsSuccess = -1, Message = url }, JsonRequestBehavior.AllowGet);
                case 1:
                    return Json(new { IsSuccess = 1, Message = "Login" }, JsonRequestBehavior.AllowGet);
                //case 2:
                //    return Json(new { IsSuccess = -1, Message = urlAzure }, JsonRequestBehavior.AllowGet);
                default:
                    OwinStartup.IsLogin = 1;
                    return Json(new { IsSuccess = -1, Message = url }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ErrorHandler(int id = 6001, int stCode = 200, string emailLogin = "")
        {
            if (id == 2001)// || id == 1001)
            {
                VinAuthentication.SignOutLocal(Request.RequestContext.HttpContext);
            }

            Response.StatusCode = stCode;
            ViewBag.EmailLogin = emailLogin;

            return View("Error", id);
        }

        public ActionResult CaptchaImage(bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session["LoginCaptcha"] = a + b;

            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen, x - r, y - r, r, r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }
        public async Task<string> GetCurrentApp()
        {
            var appid = ConfigurationManager.AppSettings["AppId"] != null ? ConfigurationManager.AppSettings["AppId"].ToString() : "17001";
            var uri = "api/ManageAppBase/CurrentApp?appid=" + appid;
            var value = await ApiHelper.HttpGet(uri, "2krojMdNQkSpZzwybnoR6g==");
            var response = value.Content.ReadAsStringAsync().Result;
            return response;
        }
        public async Task<string> GetListApp()
        {
            var appid = ConfigurationManager.AppSettings["AppId"].ToString();
            var uri = "api/ManageAppBase/GetListApp?appid=" + appid;
            var value = await ApiHelper.HttpGet(uri, "2krojMdNQkSpZzwybnoR6g==");
            var response = value.Content.ReadAsStringAsync().Result;
            return response;
        }
        public async Task<string> GetListAppForm()
        {
            var appid = ConfigurationManager.AppSettings["AppId"].ToString();
            var uri = "api/ManageAppBase/GetListManageApp?appid=" + appid;
            var value = await ApiHelper.HttpGet(uri, "2krojMdNQkSpZzwybnoR6g==");
            var response = value.Content.ReadAsStringAsync().Result;
            return response;
        }
        public async Task<string> GetAccessToken()
        {
            var appid = ConfigurationManager.AppSettings["AppId"].ToString();
            var uri = "api/ManageAppBase/GetAccessToken?appid=" + appid;
            var value = await ApiHelper.HttpGet(uri, "2krojMdNQkSpZzwybnoR6g==");
            var response = value.Content.ReadAsStringAsync().Result;
            return response;
        }
        public async Task<string> SetAccessToken(string email)
        {
            
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            var identity = new Dictionary<string, string>();
            identity.Add("Email", email);
            identity.Add("AccessToken", OwinStartup.code);
            identity.Add("SessionId", OwinStartup.sessionID);
            identity.Add("CurrentDeviceId", OwinStartup.currentDeviceID);
            var uri = "api/ManageAppBase/SetAccessToken";
            var value = await ApiHelper.HttpPost(uri,identity, "2krojMdNQkSpZzwybnoR6g==");
            var response = value.Content.ReadAsStringAsync().Result;
            return response;
        }
        public async Task<string> ChecloginAzure(string email,string sessionId)
        {
            var uri = "api/ManageAppBase/CheckLogin?email=" + email+"&sessionId="+sessionId ;
            var value = await ApiHelper.HttpGet(uri, "2krojMdNQkSpZzwybnoR6g==");
            var response = value.Content.ReadAsStringAsync().Result;
            return response;
        }
        public async Task<string> SetIsLogin(string email)
        {
            var uri = "api/ManageAppBase/SetIsLogin?email=" + email ;
            var value = await ApiHelper.HttpGet(uri, "2krojMdNQkSpZzwybnoR6g==");
            var response = value.Content.ReadAsStringAsync().Result;
            return response;
        }
        
    }
}
