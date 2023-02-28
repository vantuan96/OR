using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Common
{
    public class SettingValues
    {
        //private static ISystemSettingClient _systemSettingClient = new SystemSettingClient();

        public static string PhoneNumberPattern
        {
            get
            {
                return "^[ 0-9\\-\\+\\.\\(\\)]{6,20}$"; //_systemSettingClient.GetSettingValueByKey("PhoneNumberPattern");
            }
        }

        public static int NumberOfPageShowOnPaging
        {
            get
            {
                return 7; //System.Configuration.ConfigurationManager.AppSettings["NumberOfPageShowOnPaging"];
            }
        }

        /*
        private string _onsiteEstimateDays = null;
        public string OnsiteEstimateDays
        {
            get
            {
                if (string.IsNullOrEmpty(_onsiteEstimateDays))
                {
                    _onsiteEstimateDays = _systemSettingClient.GetSettingValueByKey("OnsiteEstimateDays");
                }

                return _onsiteEstimateDays;
            }
        }

        private string _dealDefaultOnsiteTimeout = null;
        public string DealDefaultOnsiteTimeout
        {
            get
            {
                if (string.IsNullOrEmpty(_dealDefaultOnsiteTimeout))
                {
                    _dealDefaultOnsiteTimeout = _systemSettingClient.GetSettingValueByKey("DealDefaultOnsiteTimeout");
                }

                return _dealDefaultOnsiteTimeout;
            }
        }

        private string _voucherExpireDays = null;
        public string VoucherExpireDays
        {
            get
            {
                if (string.IsNullOrEmpty(_voucherExpireDays))
                {
                    _voucherExpireDays = _systemSettingClient.GetSettingValueByKey("VoucherExpireDays");
                }

                return _voucherExpireDays;
            }
        }

        private string _websiteDomain = null;
        public string WebsiteDomain
        {
            get
            {
                if (string.IsNullOrEmpty(_websiteDomain))
                {
                    _websiteDomain = _systemSettingClient.GetSettingValueByKey("CMS.WebsiteDomain");
                }

                return _websiteDomain;
            }
        }

        private bool? _isConstructing = null;
        public bool IsConstructing
        {
            get
            {
                try
                {
                    if (_isConstructing == null)
                    {
                        string keyIsConstructing = _systemSettingClient.GetSettingValueByKey("IsConstructing");
                        _isConstructing = bool.Parse(keyIsConstructing.ToLower());
                    }

                    return (bool)_isConstructing;
                }
                catch
                {
                    return false;
                }
            }
        }*/

        private static string _loggingDir { get; set; }
        public static string LoggingDir
        {
            get
            {
                if (string.IsNullOrEmpty(_loggingDir))
                {
                    _loggingDir = System.Configuration.ConfigurationManager.AppSettings["VG.LoggingDir"];
                }

                return _loggingDir;
            }
        }

        public static int RoleViewSurgerySchedule
        {
            get
            {
                return Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["RoleViewSurgerySchedule"] ?? "4");
            }
        }
        
        public static string RoleNameViewSurgerySchedule
        {
            get
            {
                return (System.Configuration.ConfigurationManager.AppSettings["RoleNameViewSurgerySchedule"] ?? "");
            }
        }
    }
}