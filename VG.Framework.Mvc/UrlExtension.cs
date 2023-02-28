using System.Web;

namespace VG.Framework.Mvc
{
    public class UrlExtension
    {
        /// <summary>
        /// Get current url shema
        /// </summary>
        /// <returns></returns>
        public static string UrlShema()
        {
            string url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host;

            if (HttpContext.Current.Request.Url.Host.Contains("localhost"))
            {
                if (HttpContext.Current.Request.Url.Port > 0)//HttpContext.Current.Request.IsSecureConnection || 
                {
                    url += ":" + HttpContext.Current.Request.Url.Port;
                }
            }
            return url;
        }
    }
}
