using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace VG.Common
{
    public class AppUtils
    {
        public static bool IsAutoUpdateStatusProcessing = false;
        public static string AppName { get { return ConfigurationManager.AppSettings["AppName"] != null ? ConfigurationManager.AppSettings["AppName"].ToString() : string.Empty; } }
        /// <summary>
        /// Get appsetting in config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string ProxyAddress = (ConfigurationManager.AppSettings["ProxyAddress"] != null) ? ConfigurationManager.AppSettings["ProxyAddress"] : string.Empty;
        #region ReCaptCha Config
        public static int NumberShowCaptCha = (ConfigurationManager.AppSettings["NumberShowCaptCha"] != null) ? int.Parse(ConfigurationManager.AppSettings["NumberShowCaptCha"]) : 3;
        public static string ReCaptChaSiteKey = (ConfigurationManager.AppSettings["ReCaptCha-Site-Key"] != null) ? ConfigurationManager.AppSettings["ReCaptCha-Site-Key"] : string.Empty;
        public static string ReCaptChaSecretKey = (ConfigurationManager.AppSettings["ReCaptCha-Secret-Key"] != null) ? ConfigurationManager.AppSettings["ReCaptCha-Secret-Key"] : string.Empty;
        #endregion End ReCaptcha Config
        #region Security
        public static string SecuKey =(ConfigurationManager.AppSettings["Secu-Key"] != null) ? ConfigurationManager.AppSettings["Secu-Key"] : "Vinmec@1712020";
        public static List<string> ListRefUrlWhiteList= (ConfigurationManager.AppSettings["Refurl.Whitelist"] != null) ? ConfigurationManager.AppSettings["Refurl.Whitelist"].Split(';').ToList() : new List<string>();
        #endregion
    }
}
