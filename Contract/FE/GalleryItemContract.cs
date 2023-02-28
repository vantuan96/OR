using ProtoBuf;
using System.Collections.Generic;

namespace BMS.Contract.FE
{
    [ProtoContract]
    public class GalleryContract
    {
        [ProtoMember(1)]
        public int GalleryId { get; set; }

        [ProtoMember(2)]
        public GalleryContentContract Content { get; set; }

        [ProtoMember(3)]
        public List<GalleryItemContract> ListItem { get; set; }
    }

    [ProtoContract]
    public class GalleryContentContract
    {
        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public string ShortDescription { get; set; }

        [ProtoMember(3)]
        public string Description { get; set; }

        [ProtoMember(4)]
        public string MetaTitle { get; set; }

        [ProtoMember(5)]
        public string MetaDescription { get; set; }

        [ProtoMember(6)]
        public string MetaKeyword { get; set; }
    }

    [ProtoContract]
    public class GalleryItemContract
    {
        [ProtoMember(1)]
        public int ImageId { get; set; }

        [ProtoMember(2)]
        public int MediaType { get; set; }

        [ProtoMember(3)]
        public string TargetUrl { get; set; }

        [ProtoMember(4)]
        public string ImageUrl { get; set; }

        [ProtoMember(5)]
        public string VideoUrl { get; set; }

        [ProtoMember(6)]
        public GalleryItemContentContract Content { get; set; }
    }

    [ProtoContract]
    public class GalleryItemContentContract
    {
        [ProtoMember(1)]
        public string Title { get; set; }

        [ProtoMember(2)]
        public string ShortDescription { get; set; }

        [ProtoMember(3)]
        public string Description { get; set; }

        [ProtoMember(4)]
        public string TargetUrl { get; set; }

        [ProtoMember(5)]
        public string AltText { get; set; }

        [ProtoMember(6)]
        public string MetaTitle { get; set; }

        [ProtoMember(7)]
        public string MetaDescription { get; set; }

        [ProtoMember(8)]
        public string MetaKeyword { get; set; }
    }
}
