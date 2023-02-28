using System.Collections.Generic;

namespace Contract.Question
{
    public class QuestionResultPostContract : RequestContract
    {
        public int LayoutTypeId { get; set; }

        public int QuestionGroupId { get; set; }

        public List<QuestionResultPostItemContract> Items { get; set; }
    }

    public class QuestionResultPostItemContract
    {
        public int QuestionId { get; set; }

        public int AnswerId { get; set; }

        public List<QuestionResultReasonPostContract> Reasons { get; set; }
    }

    public class QuestionResultReasonPostContract
    {
        public int QuestionReasonId { get; set; }

        public string Note { get; set; }
    }
}