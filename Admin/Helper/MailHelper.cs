using Admin.SMTPMail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace Admin.Helper
{
    public static class MailHelper
    {
        public static string DomainMailUri = ConfigurationManager.AppSettings["DomainMailURI"].ToString();
        public static bool IsEmail(string email)
        {
            string matchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                    + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{2,5})$";//4-->5 ky tu ten mien
            if (email != null) return Regex.IsMatch(email, matchEmailPattern);
            else return false;
        }
        public static void SendMail(string toEmail, string ccMail, string bccMail, string subject, string Description)
        {
            try
            {
                Boolean IsConfigSendMail = Boolean.Parse(ConfigurationManager.AppSettings["IsConfigSendMail"] != null ? ConfigurationManager.AppSettings["IsConfigSendMail"].ToString() : "false");
                if (!IsConfigSendMail || string.IsNullOrEmpty(toEmail)) return;
                string AlliasCompanyName = ConfigurationManager.AppSettings["AlliasCompanyName"].ToString();
                MailCore info = new MailCore();
                info.HtmlBody = Description;
                info.FromAddress = new FromEmail().getNoReplyEmail();
                info.Subject = subject;
                if (!String.IsNullOrEmpty(toEmail))
                {
                    info.Recipients = toEmail.Split(',').ToList();
                }
                info.FileAttachments = new List<FileAttachment>() { };
                info.FromName = AlliasCompanyName;
                if (!String.IsNullOrEmpty(ccMail))
                {
                    info.CC = ccMail.Split(',').ToList();
                }
                if (!String.IsNullOrEmpty(bccMail))
                {
                    info.BCC = bccMail.Split(',').ToList();
                }
                #region "Load File Attach"
                try
                {
                    string attachContent = string.Empty;
                    string jsonAttachFile = String.IsNullOrEmpty(attachContent)
                        ? "[]"
                        : attachContent;
                    info.FileAttachments =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileAttachment>>(jsonAttachFile);
                }
                catch (Exception) { }
                #endregion

                var resultMail = new SMTPMailClient().SendSMTPMail(info);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
