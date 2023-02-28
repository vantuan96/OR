using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Product
{
    [ProtoContract]
    public class ProductDetailContract
    {
        [ProtoMember(1)]
        public int ProductId { get; set; }

        [ProtoMember(3)]
        public int Status { get; set; }

        [ProtoMember(4)]
        public int? GalleryId { get; set; }

        [ProtoMember(5)]
        public string ImageUrl { get; set; }

        [ProtoMember(6)]
        public int MsId { get; set; }

        [ProtoMember(7)]
        public int ProductCategoryId { get; set; }

        [ProtoMember(8)]
        public string ProductCategoryName { get; set; }

        [ProtoMember(9)]
        public int ProductTypeId { get; set; }

        [ProtoMember(10)]
        public string ProductTypeName { get; set; }

        [ProtoMember(11)]
        public int CreatedBy { get; set; }

        [ProtoMember(12)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(13)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(14)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(15)]
        public List<ProductContentContract> ListContent { get; set; }

        [ProtoMember(16)]
        public List<ProductAttributeMappingContract> ListAttribute { get; set; }

    }
}
