using ProtoBuf;
using System;
using System.Collections.Generic;

namespace BMS.Contract.ProductCategory
{
    [ProtoContract]
    public class ProductCategoryDetailContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public int ApprovalStatus { get; set; }
        [ProtoMember(3)]
        public bool IsOnsite { get; set; }
        //[ProtoMember(4)]
        //public int MsId { get; set; }
        [ProtoMember(5)]
        public int ParentId { get; set; }
        [ProtoMember(6)]
        public int CreatedBy { get; set; }
        [ProtoMember(7)]
        public System.DateTime CreatedDate { get; set; }
        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(9)]
        public System.DateTime LastUpdatedDate { get; set; }

        [ProtoMember(11)]
        public List<ProductCategoryContentContract> ListProductCategoryContent { get; set; }


    }
}
