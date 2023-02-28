using ProtoBuf;
using System;
using System.Collections.Generic;

namespace BMS.Contract.Product
{
    [ProtoContract]
    public class ProductAttributeContract
    {
        [ProtoMember(1)]
        public int AttrId { get; set; }

        [ProtoMember(22)]
        public int CreatedBy { get; set; }

        [ProtoMember(3)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(7)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(8)]
        public bool IsOnsite { get; set; }

        [ProtoMember(9)]
        public bool IsThumbnail { get; set; }

        [ProtoMember(10)]
        public bool IsRequired { get; set; }

        [ProtoMember(11)]
        public string ImageUrl { get; set; }

        [ProtoMember(12)]
        public int Sort { get; set; }

        [ProtoMember(13)]
        public virtual List<ProductAttributeContentContract> ProductAttributeContents { get; set; }

        [ProtoMember(14)]
        public string Name { get; set; }
    }
}