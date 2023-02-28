using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Admin.Helper
{
    public class AuthorizeEhosAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var SessionUserInfo = MvcHelper.GetUserSession(HttpContext.Current);
            var bAccess= (SessionUserInfo != null) ? SessionUserInfo.CurrentLocaltion.NameEN.Equals("HTC") : false;
            if (!bAccess)
            {
                //Redirect to site access denied
                filterContext.Result = new RedirectResult(new UrlHelper(HttpContext.Current.Request.RequestContext).RouteUrl("ErrorHandler", new { id = 1001}));
            }
        }
    }
}