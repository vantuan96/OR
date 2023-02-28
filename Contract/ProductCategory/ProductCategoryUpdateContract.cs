using ProtoBuf;
using System;
using System.Collections.Generic;

namespace BMS.Contract.ProductCategory
{
    [ProtoContract]
    public class ProductCategoryUpdateContract
    {
        [ProtoMember(11)]
        public int ProductCategoryId { get; set; }
        [ProtoMember(12)]
        public int ApprovalStatus { get; set; }
        [ProtoMember(13)]
        public bool IsOnsite { get; set; }
        [ProtoMember(15)]
        public int ParentId { get; set; }
        [ProtoMember(18)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(19)]
        public System.DateTime LastUpdatedDate { get; set; }
    }
}
