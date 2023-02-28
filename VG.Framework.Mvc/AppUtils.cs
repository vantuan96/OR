using System.Configuration;

namespace VG.Framework.Mvc
{
    public class AppUtils
    {
        /// <summary>
        /// Get appsetting in config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
