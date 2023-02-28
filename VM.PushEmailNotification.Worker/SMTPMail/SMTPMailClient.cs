using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace OREmailNoti.SMTPMail
{
    public class SMTPMailClient
    {
        string SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
        int SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
        int SmtpTimeOut = int.Parse(ConfigurationManager.AppSettings["SmtpTimeOut"]);
        string SmtpNoReply = ConfigurationManager.AppSettings["SmtpNoReply"];
        string SmtpNoReplyPassword = ConfigurationManager.AppSettings["SmtpNoReplyPassword"];
        string AlliasCompanyName = ConfigurationManager.AppSettings["AlliasCompanyName"].ToString();
        public SendResponse SendSMTPMail(MailCore mailInfo)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.IsBodyHtml = true;
                msg.Body = mailInfo.HtmlBody;
                //kiem tra to mail
                if (mailInfo.Recipients != null && mailInfo.Recipients.Count > 0)
                {
                    foreach (var toMail in mailInfo.Recipients)
                    {
                        msg.To.Add(toMail);
                    }
                }
                else
                {
                    return new SendResponse { Id = "0", Message = "Không tìm thấy mail cần gửi !" };
                }
                //kiem tra cc mail
                if (mailInfo.CC != null && mailInfo.CC.Count > 0)
                {
                    foreach (var ccMail in mailInfo.CC)
                    {
                        msg.CC.Add(ccMail);
                    }
                }
                //kiem tra bcc
                if (mailInfo.BCC != null && mailInfo.BCC.Count > 0)
                {
                    foreach (var bccMail in mailInfo.BCC)
                    {
                        msg.Bcc.Add(bccMail);
                    }
                }

                if (mailInfo.FileAttachments != null && mailInfo.FileAttachments.Count >= 0)
                {
                    foreach (var f in mailInfo.FileAttachments)
                    {

                        if (f != null && f.F.Length > 0)
                        {
                            Stream stream = new MemoryStream(f.F);
                            Attachment at = new Attachment(stream, f.FileName);
                            msg.Attachments.Add(at);
                        }
                        //msg.Attachments.Add(new Attachment(@"F:\Project TFS 2015\ADR.EmailSMSReport\ADR.EmailSMSReport.Dev\ADR.EmailSMSReport.AdminTools\Images\orderedList4.png"));
                    }

                }

                NetworkCredential authenSMTP;
                authenSMTP = new NetworkCredential(SmtpNoReply, SmtpNoReplyPassword);
                SmtpClient smtp = new SmtpClient
                {
                    Host = SmtpHost,
                    Port = SmtpPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = authenSMTP,
                    Timeout = SmtpTimeOut,
                   // EnableSsl = true,
                   // UseDefaultCredentials =false
            };
             // smtp.UseDefaultCredentials = true;
             // ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                msg.From = new MailAddress(mailInfo.FromAddress, AlliasCompanyName);
                msg.Subject = mailInfo.Subject;

                smtp.Send(msg);
                return new SendResponse() { Id = "1", Message = "goi mail SMTP thanh cong" };
            }
            catch (Exception ex)
            {             
                return new SendResponse() { Id = "0", Message = ex.ToString() };
            }
        }
    }
}
