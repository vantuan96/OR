using ProtoBuf;

namespace BMS.Contract.ArticleCategory
{

    [ProtoContract]
    public class ArticleCategoryUpdateContract
    {
        [ProtoMember(1)]
        public int ArticleCategoryId { get; set; }

        [ProtoMember(3)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(4)]
        public bool IsOnsite { get; set; }

        [ProtoMember(5)]
        public int ParentId { get; set; }

        [ProtoMember(6)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(7)]
        public System.DateTime LastUpdatedDate { get; set; }
    }
}
