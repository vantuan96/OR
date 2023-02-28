using ProtoBuf;
using System.Collections.Generic;

namespace BMS.Contract.Product
{
    [ProtoContract]
    public class ProductUpdateContract
    {
        [ProtoMember(1)]

        public int ProductId { get; set; }

        [ProtoMember(2)]
        public string ImageUrl { get; set; }

        [ProtoMember(3)]
        public int? GalleryId { get; set; }

        [ProtoMember(4)]
        public int ProductCategoryId { get; set; }

        [ProtoMember(5)]
        public int ProductTypeId { get; set; }

        [ProtoMember(6)]
        public int Status { get; set; }        
    }
}
