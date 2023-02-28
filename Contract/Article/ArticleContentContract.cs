using ProtoBuf;

namespace BMS.Contract.Article
{
    [ProtoContract]
    public class ArticleContentContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(3)]
        public int ArticleId { get; set; }
        [ProtoMember(4)]
        public string Title { get; set; }
        [ProtoMember(5)]
        public string Rewrite { get; set; }
        [ProtoMember(6)]
        public string ShortDescription { get; set; }
        [ProtoMember(8)]
        public string ArticleUrl { get; set; }
        [ProtoMember(9)]
        public string Body { get; set; }
        [ProtoMember(10)]
        public string ImageUrl { get; set; }
        [ProtoMember(11)]
        public string TargetLinkImage { get; set; }
        [ProtoMember(12)]
        public string ImageMobileUrl { get; set; }
        [ProtoMember(15)]
        public string TargetLinkImageMobile { get; set; }
        [ProtoMember(19)]
        public string LangShortName { get; set; }
        [ProtoMember(20)]
        public string MetaTitle { get; set; }
        [ProtoMember(21)]
        public string MetaDescription { get; set; }
        [ProtoMember(22)]
        public string MetaKeyword { get; set; }
        [ProtoMember(23)]
        public int ApprovalStatus { get; set; }
        [ProtoMember(24)]
        public int CreatedBy { get; set; }
        [ProtoMember(25)]
        public System.DateTime CreatedDate { get; set; }
        [ProtoMember(26)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(27)]
        public System.DateTime LastUpdatedDate { get; set; }
        [ProtoMember(28)]
        public bool IsDeleted { get; set; }

        [ProtoMember(29)]
        public System.DateTime StartDate { get; set; }

        [ProtoMember(30)]
        public System.DateTime EndDate { get; set; }

        [ProtoMember(31)]
        public int Type { get; set; }

        [ProtoMember(32)]
        public int MsId { get; set; }

        [ProtoMember(33)]
        public int ArticleCategoryId { get; set; }

        [ProtoMember(34)]
        public string Title2 { get; set; }

        [ProtoMember(35)]
        public string TitleWithoutAccent { get; set; }

        [ProtoMember(36)]
        public string Title2WithoutAccent { get; set; }

        [ProtoMember(37)]
        public string Key { get; set; }

    }
}