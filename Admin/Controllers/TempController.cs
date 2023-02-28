using Admin.Helper;
using Admin.Shared;
using Caching.OR;
using Contract.OR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VG.Framework.Mvc;

namespace Admin.Controllers
{
    public class TempController : Controller
    {
        // GET: Temp
        private readonly ORCaching _orService;
     //   private readonly ORVisitLink _visitLink;
        public TempController( ORCaching orService)
        {
            _orService = orService;
         //   _visitLink = new ORVisitLink(new CookieUtils().GetCookie(AdminGlobal.TokenVisitORLink));
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CheckActiveLink(string code="")
        {
            Guid guidCode ;
            if (string.IsNullOrEmpty(code))
                return RedirectToAction("Login", "Authen");
            Guid.TryParse(code, out guidCode);
            if(guidCode==null)
                return RedirectToAction("Login", "Authen");

            var activeLink=_orService.CheckOperationLink(guidCode);
            if(activeLink==null || activeLink.IsValidate==false )
                return RedirectToAction("Login", "Authen");
            else
            {
               // _visitLink.SetCurrentVisit(activeLink);
                //_orService.CUDOperationLink(new ORLinkContract()
                //{
                //    GuidCode = activeLink.GuidCode,
                //    IsActive = true,
                //    IpActive = Request.UserHostAddress
                //},0);
                return Redirect(activeLink.ReDirectUrl);                
            }          
        }

        private static void setVisitORCookie(ORLinkActive datalink, HttpContextBase _curentHttpContext)
        {
            var tokenVisit = new FormsAuthenticationTicket(1, AdminGlobal.TokenVisitORLink,
                DateTime.Now, DateTime.Now.AddHours(24),
                true, JsonConvert.SerializeObject(datalink));
            SetAuthCookie(_curentHttpContext, tokenVisit, AdminGlobal.TokenVisitORLink);
        }
        private static void SetAuthCookie(HttpContextBase httpContext,
           FormsAuthenticationTicket authenticationTicket, string CookieName)
        {
            var encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
            var cookie = new HttpCookie(CookieName, encryptedTicket);
            cookie.HttpOnly = true;
            cookie.Expires = authenticationTicket.Expiration;
            httpContext.Response.Cookies.Add(cookie);
        }
    }
}