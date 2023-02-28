using System;

namespace Contract.Question
{
    public class QuestionGroupContract
    {
        public int QuestionGroupId { get; set; }
        public int LayoutTypeId { get; set; }
        public string LayoutTypeName { get; set; }
        public string NameVN { get; set; }
        public string NameEN { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public byte Status { get; set; }
    }
}