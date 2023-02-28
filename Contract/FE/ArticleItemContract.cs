using ProtoBuf;
using System;

namespace BMS.Contract.FE
{
    [ProtoContract]
    public class ArticleKeyContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Key { get; set; }
    }

    [ProtoContract]
    public class ArticleItemContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public bool IsHot { get; set; }

        [ProtoMember(3)]
        public bool IsFocus { get; set; }

        [ProtoMember(4)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(5)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(6)]
        public ArticleContentItemContract Content { get; set; }
    }

    [ProtoContract]
    public class ArticleContentItemContract
    {
        [ProtoMember(1)]
        public string Title { get; set; }

        [ProtoMember(2)]
        public string Title2 { get; set; }

        [ProtoMember(3)]
        public string Rewrite { get; set; }

        [ProtoMember(4)]
        public string ShortDescription { get; set; }

        [ProtoMember(5)]
        public string ArticleUrl { get; set; }

        [ProtoMember(6)]
        public string ImageUrl { get; set; }

        [ProtoMember(7)]
        public string ImageMobileUrl { get; set; }

        [ProtoMember(8)]
        public string MetaTitle { get; set; }

        [ProtoMember(9)]
        public string MetaDescription { get; set; }

        [ProtoMember(10)]
        public string MetaKeyword { get; set; }

        [ProtoMember(11)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(12)]
        public DateTime LastUpdatedDate { get; set; }
    }
}
