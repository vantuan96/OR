using System.Configuration;

namespace OREmailNoti.SMTPMail
{
    public class FromEmail
    {
        private string fromDefaultEmail { get; set; }
        private string fromInfoEmail { get; set; }
        private string fromNoReplyEmail { get; set; }
        public FromEmail()
        {
            this.fromDefaultEmail = ConfigurationManager.AppSettings["default_email"].ToString();
            this.fromInfoEmail = ConfigurationManager.AppSettings["default_email"].ToString();
            this.fromNoReplyEmail = ConfigurationManager.AppSettings["default_email"].ToString();
        }
        public string getDefaultEmail()
        {
            return this.fromDefaultEmail;
        }
        public string getInfoEmail()
        {
            return this.fromInfoEmail;
        }
        public string getNoReplyEmail()
        {
            return this.fromNoReplyEmail;
        }
    }
}
