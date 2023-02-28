using System;
using System.Web;

namespace VG.Framework.Mvc
{
    public class CookieUtils
    {
        private HttpContext _httpContext = HttpContext.Current;

        public string GetCookie(string strCookieName)
        {
            string strResult = "";
            if (_httpContext.Request.Cookies[strCookieName] != null)
            {
                strResult = _httpContext.Request.Cookies[strCookieName].Value;
            }
            return strResult;
        }

        public void SetCookie(string strCookieName, string strCookieVal, int iExpireDay, bool isHttpOnly)
        {
            HttpCookie ckCookie = new HttpCookie(strCookieName);
            ckCookie.Value = strCookieVal;
            ckCookie.HttpOnly = isHttpOnly;
            DateTime dNow = DateTime.Now;
            TimeSpan tsDays = new TimeSpan(iExpireDay, 0, 0, 0);
            ckCookie.Expires = dNow + tsDays;
            if (_httpContext.Response.Cookies[strCookieName] != null)
            {
                _httpContext.Response.Cookies.Remove(strCookieName);
            }
            _httpContext.Response.Cookies.Add(ckCookie);
        }
    }
}
