using ProtoBuf;
using System;

namespace Contract.Microsite
{
    [ProtoContract]
    public class MicrositeContentContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(22)]
        public int MsId { get; set; }

        [ProtoMember(3)]
        public string Title { get; set; }

        [ProtoMember(4)]
        public string Rewrite { get; set; }

        [ProtoMember(5)]
        public string ShortDescription { get; set; }

        [ProtoMember(6)]
        public string Description { get; set; }

        [ProtoMember(7)]
        public string ImageUrl { get; set; }

        [ProtoMember(8)]
        public string TargetLinkImage { get; set; }

        [ProtoMember(9)]
        public string ImageMobileUrl { get; set; }

        [ProtoMember(10)]
        public string TargetLinkImageMobile { get; set; }

        [ProtoMember(11)]
        public string LangShortName { get; set; }

        [ProtoMember(12)]
        public string MetaTitle { get; set; }

        [ProtoMember(13)]
        public string MetaDescription { get; set; }

        [ProtoMember(14)]
        public string MetaKeyword { get; set; }

        [ProtoMember(15)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(16)]
        public int CreatedBy { get; set; }

        [ProtoMember(17)]
        public System.DateTime CreatedDate { get; set; }

        [ProtoMember(18)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(19)]
        public System.DateTime LastUpdatedDate { get; set; }

        [ProtoMember(23)]
        public bool IsDeleted { get; set; }

        [ProtoMember(24)]
        public string ReferenceCode { get; set; }

        [ProtoMember(25)]
        public int MstId { get; set; }

        [ProtoMember(26)]
        public string GalleryKey { get; set; }

        [ProtoMember(27)]
        public string BannerUrl { get; set; }
    }
}
