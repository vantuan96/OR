using Admin.Common;
using Caching.Core;
using Contract.User;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Admin.Helper
{
    public class CheckUserCachingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginContract userApp = null;
            var LoginTokenCookie = filterContext.HttpContext.Request.Cookies[AdminGlobal.SignInToken];
            if (LoginTokenCookie != null)
            {
                try
                {
                    var token = FormsAuthentication.Decrypt(LoginTokenCookie.Value);
                    userApp = JsonConvert.DeserializeObject<LoginContract>(token.UserData);
                    if (userApp == null || (userApp != null && UserCaching.Instant.GetUserModel(userApp.NickName) == null))
                    {
                        VinAuthentication.SignOutLocal(filterContext.HttpContext);
                        filterContext.Result = new RedirectResult(new UrlHelper(filterContext.HttpContext.Request.RequestContext).Action("Login", "Authen"));
                        base.OnActionExecuting(filterContext);
                    }
                }
                catch (Exception ex)
                {
                    CommonHelper.CatchExceptionToLog(ex, "Admin.VinAuthentication", MethodBase.GetCurrentMethod().Name);
                }

            }
        }
    }
}