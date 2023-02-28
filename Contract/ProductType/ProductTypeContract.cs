using ProtoBuf;
using System;

namespace BMS.Contract.ProductType
{
    [ProtoContract]
    public class ProductTypeContract
    {
        [ProtoMember(1)]
        public int ProductTypeId { get; set; }
        [ProtoMember(2)]
        public int Status { get; set; }
        [ProtoMember(3)]
        public int CreatedBy { get; set; }
        [ProtoMember(4)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(5)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(6)]
        public DateTime LastUpdatedDate { get; set; }
        [ProtoMember(7)]
        public string Name { get; set; }
    }
}
