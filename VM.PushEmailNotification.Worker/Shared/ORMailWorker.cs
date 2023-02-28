using Caching.Core;
using Caching.OR;
using Contract.OperationCheckList;
using Contract.OR;
using OREmailNoti.SMTPMail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using VG.Common;

namespace OREmailNoti.WindowService.Shared
{
    public class ORMailWorker : BaseCallWebservice
    {
        #region main thread
        public void Run()
        {
            StartThread();
        }
        public void StartThread()
        {
            for (int i = 0; i < TotalThread; i++)
            {
                if (_Thread[i] == null)
                {
                    CurThread = i;
                    break;
                }
            }
            if (_Thread[CurThread] == null && Extension.RunValidateSchuduler())
            {
                _Thread[CurThread] = new System.Threading.Thread(new System.Threading.ThreadStart(SendOREmailNotify));
                _Thread[CurThread].Priority = System.Threading.ThreadPriority.Normal;
                _Thread[CurThread].Start();
            }
        }
        //Account Domain:
        //vmec.or/Qwerty~123456&
        #endregion
        private void SendOREmailNotify()
        {
            try
            {
                new LogFile().Write(string.Format("Begin Process SendOREmailNotify at {0}", DateTime.Now), "SendOREmailNotify");
                List<ORNotifyMail> listSites = new List<ORNotifyMail>();
                var orcaching = new ORCaching();
                orcaching.ExecuteBlock(() => { listSites = orcaching.GetListORAnesthByMail(-1); });
                foreach (var site in listSites)
                {
                    var titleMail = string.Format("Danh sách thông tin đăng ký ca mổ ngày {0} tại {1}", DateTime.Now.ToString("dd/MM/yyyy"), site.SiteName);

                    #region mail content format
                    String html = "<html><head><meta charset='utf - 8' />"
                      + "<title>" + titleMail + "</title>"
                      + "</head>"
                      + "<body>"
                      + "<table    border='0'>"
                      + "<tr><td class ='text12' ><br>Dear Anh/Chị Bác Sĩ Quản lý.</td></tr>"
                      + "<tr><td class ='text12' ><br>Hiện tại danh sách thông tin đăng ký ca mổ cần anh chị điều phối  :</td></tr>"
                      + "<tr><td><table border='1' style='border-spacing:0' >"
                      + "<tr  class='centerheading' style='background-color:#32c69a' >"
                              + "<td  ><b>Bác sĩ đặt mổ</b></td>"
                              + "<td  ><b>Tên dịch vụ mổ</b></td>"
                              + "<td  ><b>Phòng mổ</b></td>"
                              + "<td  ><b>Thời gian mổ</b></td>"
                      + "</tr>";

                    foreach (var data in site.listData)
                    {
                        string rangeTimeOperation = string.Format("{0}h đến {1}h ngày {2}", (data.dtStart ?? DateTime.Now).ToString("HH:mm"), (data.dtEnd ?? DateTime.Now).ToString("HH:mm"), (data.dtOperation ?? DateTime.Now).ToVEShortDate());
                        html = html + "<tr>"
                                         + "<td  >" + data.NameCreatedBy + "</b></td>"
                                         + "<td  >" + data.HpServiceName + "</td>"
                                         + "<td  >" + data.ORRoomName + "</td>"
                                         + "<td  >" + rangeTimeOperation + "</td>"
                                       + "</tr>";
                    }
                    html = html + "</table>"
                         + "</td>"
                 + "</tr>"
                  + "<tr>"
                       + "<td ></td>"
                  + "</tr>"
                  + "<tr><td  class ='text12' >" + "Trân trọng" + "</td></tr>"
                  + "</table>"
                  + "</body></html>";
                    #endregion
                    if (!string.IsNullOrEmpty(site.Email))
                    {
                        string[] strEmail = site.Email.Split(',');
                        foreach(var itemEmail in strEmail)
                        {
                            if (IsEmail(itemEmail))
                            {
                                int iCountSend = 0;
                                StepSend:
                                var statusSend= SendMail(itemEmail, string.Empty, string.Empty, titleMail, html);
                                iCountSend++;
                                if (!statusSend)
                                {
                                    //Sleep to resend
                                    System.Threading.Thread.Sleep(60000);
                                    if (iCountSend <= 3)
                                    {
                                        new LogFile().Write(string.Format("Begin ReSend {0} mail for: [To: {1}] - [CC: {2}] - [BCC: {3}] ", iCountSend, itemEmail, string.Empty, string.Empty), "SendOREmailNotify");
                                        goto StepSend;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new LogFile().Write(ex.Message, ex.Source);
            }
            finally
            {
                SendOREmailNotify_onComplected(this, new EventHandleWebservice(null));
            }
        }
        void SendOREmailNotify_onComplected(object sender, EventHandleWebservice e)
        {
            RaiseEventComplected(sender, e);
        }
        #region smtp mail
        private bool SendMail(string toEmail, string ccMail, string bccMail, string Subject, string Description)
        {
            bool returnSend = true;
            try
            {
                new LogFile().Write(string.Format("Begin Send mail for: [To: {0}] - [CC: {1}] - [BCC: {2}] ",toEmail,ccMail,bccMail), "SendMail");
                Boolean IsConfigSendMail = Boolean.Parse(ConfigurationManager.AppSettings["IsConfigSendMail"] != null ? ConfigurationManager.AppSettings["IsConfigSendMail"].ToString() : "false");
                if (!IsConfigSendMail || string.IsNullOrEmpty(toEmail)) return true;
                string AlliasCompanyName = ConfigurationManager.AppSettings["AlliasCompanyName"].ToString();
                MailCore info = new MailCore();
                info.HtmlBody = Description;
                info.FromAddress = new FromEmail().getNoReplyEmail();
                info.Subject = Subject;
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
                catch (Exception) {
                    returnSend = false;
                }
                #endregion
                #region Place for send email to enduser
                var resultMail = new SMTPMailClient().SendSMTPMail(info);
                new LogFile().Write(string.Format("End Send mail for: [To: {0}] - [CC: {1}] - [BCC: {2}]. Status: {3} ", toEmail, ccMail, bccMail, resultMail.Message), "SendMail");
                returnSend = (resultMail.Id=="1");
                #endregion .Place for send email to enduser
            }
            catch (Exception ex)
            {
                new LogFile().Write(string.Format("Error when Send mail for: [To: {0}] - [CC: {1}] - [BCC: {2}]. Ex: {3} ", toEmail, ccMail, bccMail, ex), "SendMail");
                returnSend = false;
            }
            return returnSend;
        }
        private static bool IsEmail(string email)
        {
            string matchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                    + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{2,5})$";//4-->5 ky tu ten mien
            if (email != null) return Regex.IsMatch(email, matchEmailPattern);
            else return false;
        }

        #endregion


    }
}
