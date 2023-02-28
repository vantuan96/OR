using ProtoBuf;
using System;

namespace BMS.Contract.Gallery
{
    [ProtoContract]
    public class GalleryContentContract
    {
        [ProtoMember(1)]
        public int MsId { get; set; }
        [ProtoMember(16)]
        public int GalleryId { get; set; }
        [ProtoMember(3)]
        public int GalleryContentId { get; set; }
        [ProtoMember(4)]
        public string Name { get; set; }
        [ProtoMember(5)]
        public string ShortDescription { get; set; }
        [ProtoMember(6)]
        public string Description { get; set; }
        [ProtoMember(7)]
        public string RewriteUrl { get; set; }
        [ProtoMember(8)]
        public string LangShortName { get; set; }
        [ProtoMember(9)]
        public string MetaTitle { get; set; }
        [ProtoMember(10)]
        public string MetaDescription { get; set; }
        [ProtoMember(11)]
        public string MetaKeyword { get; set; }
        [ProtoMember(12)]
        public int CreatedBy { get; set; }
        [ProtoMember(13)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(14)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(15)]
        public DateTime LastUpdatedDate { get; set; }
        [ProtoMember(17)]
        public bool IsPredefined { get; set; }
        [ProtoMember(18)]
        public string Key { get; set; }
    }
}
