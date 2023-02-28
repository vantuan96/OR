using Contract.OR;
using System;
using System.Configuration;

namespace Contract.Shared
{

    public static class ConfigUrl
    {
        public static string UrlSyncHis = ConfigurationManager.AppSettings["UrlSyncHis"] != null ? ConfigurationManager.AppSettings["UrlSyncHis"] : string.Empty;
        private static string UrlSyncPatient = ConfigurationManager.AppSettings["UrlSyncPatient"] != null ? ConfigurationManager.AppSettings["UrlSyncPatient"] : string.Empty;
        private static string OhVersion = ConfigurationManager.AppSettings["OHVersion"] != null ? ConfigurationManager.AppSettings["OHVersion"] : string.Empty;
        private static string EhosVersion = ConfigurationManager.AppSettings["EHosVersion"] != null ? ConfigurationManager.AppSettings["EHosVersion"] : string.Empty;
        public static string ORVersion = ConfigurationManager.AppSettings["ORVersion"] != null ? ConfigurationManager.AppSettings["ORVersion"] : string.Empty;
        public static string Authen_BearAPI = ConfigurationManager.AppSettings["Authen_BearAPI"] != null ? ConfigurationManager.AppSettings["Authen_BearAPI"] : string.Empty;
        public static string PreFixMemmoryCache = ConfigurationManager.AppSettings["PreFixMemmoryCache"] ?? "OR-OH";
        public static int CF_ApiTimeout_minutes { get { return ConfigurationManager.AppSettings["ApiTimeout.minutes"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["ApiTimeout.minutes"].ToString()) : 3; } }
        public static string CF_SyncOHService_CS { get { return ConfigurationManager.AppSettings["SyncOHService_CS"] != null ? ConfigurationManager.AppSettings["SyncOHService_CS"].ToString() : "0 0/45 0/1 ? * * *"; } }
        public static string CF_AutoUpdateStatus_CS { get { return ConfigurationManager.AppSettings["AutoUpdateStatus_CS"] != null ? ConfigurationManager.AppSettings["AutoUpdateStatus_CS"].ToString() : "0 0/5 0/1 ? * * *"; } }
        /// <summary>
        /// Lays thong tin visit
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="visitCode"></param>
        /// <param name="sourceClientId"></param>
        /// <returns></returns>
        public static string GetUrlListVisitOR(string pId, int sourceClientId)
        {
            if (sourceClientId == (int)SourceClientEnum.Oh)
            {
                return string.Format(UrlSyncHis + "{0}/1.0.0/getSinhHieu_PhongMo?PID={1}", ((sourceClientId == (int)SourceClientEnum.Oh) ? OhVersion : EhosVersion), pId);
            }
            else
            {
                return string.Format(UrlSyncHis + "{0}/1.0.0/getSinhHieuV3?PID={1}", ((sourceClientId == (int)SourceClientEnum.Oh) ? OhVersion : EhosVersion), pId);
            }          
        }
        public static string GetUrlPatientByPID(string pId)
        {
            return string.Format(UrlSyncHis + "{0}/1.0.0/getPatientByPID?PID={1}",  OhVersion , pId);
        }
        public static string GetPatientORInfo(string pId, string visitCode, int sourceClientId)
        {
            return string.Format(UrlSyncHis + "{0}/1.0.0/getExaminationInfo?PID={1}&visit_code={2}", ((sourceClientId == (int)SourceClientEnum.Oh) ? OhVersion : EhosVersion), pId, visitCode);            
        }
    }
}
