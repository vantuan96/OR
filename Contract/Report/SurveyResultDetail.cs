using System;

namespace Contract.Report
{
    public class SurveyResultDetail
    {
        public string DeviceImei { get; set; }

        public string LocationName { get; set; }

        public string ResultGroup { get; set; }

        public string QuestionGroupTextVN { get; set; }

        public string QuestionTextVN { get; set; }

        public string QuestionReasonTextVN { get; set; }

        public string AnswerTextVN { get; set; }

        public string Note { get; set; }

        public Nullable<System.DateTime> SurveyTime { get; set; }

        public string QuestionGroupTextEN { get; set; }

        public string QuestionTextEN { get; set; }

        public string AnswerTextEN { get; set; }

        public string QuestionReasonTextEN { get; set; }

        

        public int CreatedBy { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public int LastUpdatedBy { get; set; }

        public System.DateTime LastUpdatedDate { get; set; }
    }
}