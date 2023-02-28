using ProtoBuf;
using System.Collections.Generic;

namespace BMS.Contract.Product
{
    [ProtoContract]
    public class ProductCreateContract
    {
        [ProtoMember(1)]
        public int MsId { get; set; }

        [ProtoMember(2)]
        public string ImageUrl { get; set; }

        [ProtoMember(3)]
        public int ProductTypeId { get; set; }

        [ProtoMember(4)]
        public string NameLine1 { get; set; }

        [ProtoMember(5)]
        public string ShortDescription { get; set; }

        [ProtoMember(6)]
        public string Description { get; set; }

        [ProtoMember(7)]
        public string Body { get; set; }

        [ProtoMember(8)]
        public string RewriteUrl { get; set; }

        [ProtoMember(9)]
        public string MetaTitle { get; set; }

        [ProtoMember(10)]
        public string MetaDescription { get; set; }

        [ProtoMember(11)]
        public string MetaKeyword { get; set; }

        [ProtoMember(13)]
        public string LangShortName { get; set; }

        [ProtoMember(14)]
        public int ProductCategoryId { get; set; }

        [ProtoMember(15)]
        public int? GalleryId { get; set; }

        [ProtoMember(16)]
        public string NameLine2 { get; set; }

    }
}
