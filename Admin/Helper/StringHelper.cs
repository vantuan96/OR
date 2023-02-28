using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VG.Common;
using Admin.Models.Shared;
using Contract.Shared;
using Contract.OR.SyncData;
using Contract.OR;

namespace Admin.Helper
{
    public static class StringHelper
    {
        /// <summary>
        /// DateShortFormat = "dd/MM/yyyy"
        /// </summary>
        public const string DateShortFormat = "dd/MM/yyyy";

        /// <summary>
        /// DateShortFormat = "MM/dd/yyyy"
        /// </summary>
        public const string EnglishDateShortFormat = "MM/dd/yyyy";

        /// <summary>
        /// DateTimeFormat = "dd/MM/yyyy HH:mm:ss"
        /// </summary>
        public const string DateTimeFormat = "dd/MM/yyyy hh:mm tt";

        /// <summary>
        /// DateTimeShortFormat = "dd/MM/yyyy hh:mm:ss tt"
        /// </summary>
        public const string DateTimeShortFormat = "dd/MM/yyyy hh:mm tt";

        /// <summary>
        /// DateTimeShortFormat = "dd/MM/yyyy HH:mm"
        /// </summary>
        public const string DateTimeNoSecondFormat = "dd/MM/yyyy hh:mm tt";

        /// <summary>
        /// TimeNoSecondFormat = "h:mm tt"
        /// </summary>
        public const string TimeNoSecondFormat = "h:mm tt";

        /// <summary>
        /// MinDateTime = new DateTime(1900,1,1)
        /// </summary>
        public static DateTime MinDateTime = new DateTime(1900, 1, 1);

        public static string GetTrueFalseStatusCssClass(int stateId)
        {
            switch (stateId)
            {
                case 1:
                    return "badge badge-success";
                default:
                    return "badge badge-default";
            }
        }

        /// <summary>
        /// Truncate string
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string TruncateString(this string content, int length)
        {
            if (content.Length == 0 || content.Length <= length)
                return content;

            int countCharacter = 0;
            var result = new StringBuilder();
            foreach (char c in content)
            {
                if (countCharacter <= length || (countCharacter > length && c.ToString(CultureInfo.InvariantCulture).Trim() != string.Empty))
                    result.Append(c);
                countCharacter++;

                if (countCharacter > length && c.ToString().Trim() == string.Empty)
                    break;
            }

            return result.Append("...").ToString();
        }

        /// <summary>
        /// Extract domain name dfrom url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ExtractDomainNameFromUrl(string url)
        {
            try
            {
                //TODO: Modify later
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Chuyển null thành string Và Trim dữ liệu
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <created>03/17/2015 11:01 AM</created>
        public static string NullToStringTrim(this object value)
        {
            if (value == null)
                return "";
            return value.ToString().Trim();
        }

        public static string ReplaceUrlProtocol(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return url.Replace("https://", "").Replace("http://", "");
            }
            else return "";
        }

        /// <summary>
        /// Get domain name from domain format like 
        /// "http://deal.adayroi.dev/xem-demo-online-p"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SplitDomain(string input)
        {
            if (string.IsNullOrEmpty(input) || input.IndexOf("/", StringComparison.Ordinal) < 0)
                return null;
            try
            {
                List<string> text = input.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (!text.Any() || text.Count() != 3) //Example domain split to 3 parts (remove empty part)
                    return string.Empty;

                return string.Concat(text[0], "//", text[1], "/");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetResourceFromDesc(Type resource, Enum enumValue)
        {
            try
            {
                ResourceManager rm = new ResourceManager(resource);
                return rm.GetString(enumValue.GetDescription());
            }
            catch (Exception)
            {
                return "";
            }
            
            
        }

        public static string GetResourceFromOnsiteStatus(Type resource, bool value)
        {
            try
            {
                ResourceManager rm = new ResourceManager(resource);
                OnsiteStatus status = (OnsiteStatus)Convert.ToInt32(value);
                return rm.GetString(status.GetDescription());
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GenInheritSpace(int level)
        {
            string result = "";
            for(int i = 1; i < level; i++)
            {
                //if (i == level - 1)
                //{
                //    result += ">";
                //}
                result += "__";
            }
            return result;
        }
        /// <summary>
        /// Remove substring
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubString(string content, int length)
        {
            if (string.IsNullOrEmpty(content)) return content;
            int len = content.Length;
            if (len <= length) return content;
            return content.Substring(length)+"...";
        }

        public static string MakeRequestVisitOR(BenhNhanOR bn, string siteId ,string siteName, int sourceClientId)
        {
            var info = new ORVisitModel();
            info.MA_BN = bn.MA_BN;
            if (bn.NGAY_SINH != null)
            {
                info.NGAY_SINH = bn.NGAY_SINH.Value;
            }
            info.GIOI_TINH = bn.GIOI_TINH;
            info.QUOC_TICH = bn.QUOC_TICH;
            info.DIA_CHI = bn.DIA_CHI;
            info.HO_TEN = bn.HO_TEN;
            info.CHANDOAN = string.Empty;
            info.MAICD = string.Empty;
            info.LYDOVV = string.Empty;
            info.HospitalCode = siteId;
            info.HospitalName = siteName;
            info.HospitalPhone = string.Empty;
            info.SourceClientId = sourceClientId;
            info.Email = bn.EMAIL;
            info.PatientPhone = bn.PHONE;
            info.HospitalPhone = string.Empty;
            info.Age = bn.TUOI;
            info.PatientService = string.Empty;
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(info);
            return content;
        }
       
    }
}


