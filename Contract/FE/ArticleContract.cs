using ProtoBuf;
using System.Collections.Generic;
using BMS.Contract.AdminTag;
using BMS.Contract.Tag;

namespace BMS.Contract.FE
{
    [ProtoContract]
    public class ArticleContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(3)]
        public bool IsHot { get; set; }

        [ProtoMember(4)]
        public bool IsFocus { get; set; }

        [ProtoMember(5)]
        public int Status { get; set; }

        [ProtoMember(6)]
        public int Type { get; set; }

        [ProtoMember(7)]
        public bool IsVipPromotion { get; set; }

        [ProtoMember(8)]
        public System.DateTime StartDate { get; set; }

        [ProtoMember(9)]
        public System.DateTime EndDate { get; set; }

        [ProtoMember(10)]
        public int MsId { get; set; }

        [ProtoMember(11)]
        public int CreatedBy { get; set; }

        [ProtoMember(12)]
        public System.DateTime CreatedDate { get; set; }

        [ProtoMember(13)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(14)]
        public System.DateTime LastUpdatedDate { get; set; }

        [ProtoMember(15)]
        public int ArticleCategoryId { get; set; }

        [ProtoMember(16)]
        public string ArticleCategoryName { get; set; }

        [ProtoMember(17)]
        public ArticleContentContract ArticleContent { get; set; }

        [ProtoMember(18)]
        public List<AdminTagContract> AdminTags { get; set; }

        [ProtoMember(19)]
        public List<TagContract> Tags { get; set; }

    }


    [ProtoContract]
    public class ArticleContentContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Title { get; set; }
        [ProtoMember(3)]
        public string Title2 { get; set; }
        [ProtoMember(4)]
        public string Rewrite { get; set; }
        [ProtoMember(5)]
        public string ShortDescription { get; set; }
        [ProtoMember(6)]
        public string ArticleUrl { get; set; }
        [ProtoMember(7)]
        public string Body { get; set; }
        [ProtoMember(8)]
        public string ImageUrl { get; set; }
        [ProtoMember(9)]
        public string TargetLinkImage { get; set; }
        [ProtoMember(10)]
        public string ImageMobileUrl { get; set; }
        [ProtoMember(11)]
        public string TargetLinkImageMobile { get; set; }
        [ProtoMember(12)]
        public string LangShortName { get; set; }
        [ProtoMember(13)]
        public string MetaTitle { get; set; }
        [ProtoMember(14)]
        public string MetaDescription { get; set; }
        [ProtoMember(15)]
        public string MetaKeyword { get; set; }
        [ProtoMember(16)]
        public int ApprovalStatus { get; set; }
        [ProtoMember(17)]
        public int CreatedBy { get; set; }
        [ProtoMember(18)]
        public System.DateTime CreatedDate { get; set; }
        [ProtoMember(19)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(20)]
        public System.DateTime LastUpdatedDate { get; set; }
        [ProtoMember(21)]
        public string TitleWithoutAccent { get; set; }
        [ProtoMember(22)]
        public string Title2WithoutAccent { get; set; }

    }
    
}
