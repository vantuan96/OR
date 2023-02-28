using ProtoBuf;
using System;

namespace BMS.Contract.ArticleCategory
{
    [ProtoContract]
    public class ArticleCategoryContentContract
    {

        [ProtoMember(1)]
        public int ContentId { get; set; }
        [ProtoMember(22)]
        public int ArticleCategoryId { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Rewrite { get; set; }
        [ProtoMember(5)]
        public string CategoryUrl { get; set; }
        [ProtoMember(6)]
        public string ShortDescription { get; set; }
        [ProtoMember(7)]
        public string Description { get; set; }
        [ProtoMember(8)]
        public string LangShortName { get; set; }
        [ProtoMember(9)]
        public int ApprovalStatus { get; set; }
        [ProtoMember(10)]
        public int CreatedBy { get; set; }
        [ProtoMember(11)]
        public System.DateTime CreatedDate { get; set; }
        [ProtoMember(12)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(13)]
        public System.DateTime LastUpdatedDate { get; set; }
        [ProtoMember(14)]
        public int MsId { get; set; }


        
    }
}
