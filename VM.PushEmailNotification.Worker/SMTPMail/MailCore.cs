using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OREmailNoti.SMTPMail
{
    public class MailCore
    {
        public MailCore()
        {
            Recipients = new List<string>();
            Tags = new List<string>();
            CC = new List<string>();
            BCC = new List<string>();
            CustomDataDictionary = new Dictionary<string, string>();
            CustomHeaderDictionary = new Dictionary<string, string>();
            EnableDKIM = true;
            EnableTest = false;
            EnableTracking = true;
            EnableTrackingOpens = true;
            TrackingClicks = TrackingClicks.Yes;
            DeliveryTime = DateTime.MinValue;
            FileAttachments = new List<FileAttachment>();
            Tokens = new List<Token>();
            NoRecipients = new List<string>();
            IsPriority = 0;
            TemplatePath = "";
            IsToken = 0;
            TokenNames = "";
            TokenValues = "";
        }
        public bool EnableTest { get; set; }
        public bool EnableDKIM { get; set; }
        public bool EnableTracking { get; set; }
        public bool EnableTrackingOpens { get; set; }
        public TrackingClicks TrackingClicks { get; set; }

        public string CampaignId { get; set; }
        public string FromName { get; set; }
        public string FromAddress { get; set; }
        public string ReplyName { get; set; }
        public string ReplyAddress { get; set; }
        public string Message { get; set; }
        public DateTime DeliveryTime { get; set; }

        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string PlainBody { get; set; }

        public Dictionary<string, string> CustomDataDictionary { get; set; }
        public Dictionary<string, string> CustomHeaderDictionary { get; set; }

        public IList<string> Recipients { get; set; }
        public IList<string> Tags { get; set; }
        public IList<string> CC { get; set; }
        public IList<string> BCC { get; set; }
        public IList<FileAttachment> FileAttachments { get; set; }
        public IList<Token> Tokens { get; set; }
        public IList<string> NoRecipients { get; set; }
        public int IsPriority { get; set; }
        public String TemplatePath { get; set; }
        public int IsToken { get; set; }
        public String TokenNames { get; set; }
        public String TokenValues { get; set; }

    }
    public enum TrackingClicks { Yes, No, HtmlOnly };
}
