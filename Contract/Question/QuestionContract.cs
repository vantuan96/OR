using System.Collections.Generic;

namespace Contract.Question
{
    public class QuestionContract
    {
        public int QuestionId { get; set; }

        public string QuestionTextVN { get; set; }

        public string QuestionTextEN { get; set; }

        public int Sort { get; set; }

        public List<QuestionAnswerContract> QuestionAnswers { get; set; }

        public int UserId { get; set; }

        public int QuestionGroupId { get; set; }
    }

    public class QuestionAnswerContract
    {
        public int QuestionAnswerMappingId { get; set; }

        public int AnswerId { get; set; }

        public string AnswerTextVN { get; set; }

        public string AnswerTextEN { get; set; }

        /// <summary>
        /// Title chọn các check box sau khi chấm điểm
        /// </summary>
        public string FeedbackTitleVN { get; set; }

        public string FeedbackTitleEN { get; set; }

        public int Rate { get; set; }

        public string IconName { get; set; }

        public List<QuestionReasonContract> QuestionReasons { get; set; }

        public int UserId { get; set; }
    }

    public class QuestionReasonContract
    {
        public int QuestionReasonId { get; set; }

        public string ReasonTextVN { get; set; }

        public string ReasonTextEN { get; set; }

        public int Sort { get; set; }

        /// <summary>
        /// Loại góp ý: 1-Checkbox; 2-Textbox
        /// </summary>
        public byte Type { get; set; }

        public int UserId { get; set; }

        public int QuestionAnswerMappingId { get; set; }

    }
}