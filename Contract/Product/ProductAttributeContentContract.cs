using ProtoBuf;
using System;

namespace BMS.Contract.Product
{
    [ProtoContract]
    public class ProductAttributeContentContract
    {
        [ProtoMember(1)]
        public int AttrContentId { get; set; }

        [ProtoMember(22)]
        public int AttrId { get; set; }

        [ProtoMember(3)]
        public string Name { get; set; }

        [ProtoMember(4)]
        public string LangShortName { get; set; }

        [ProtoMember(5)]
        public int CreatedBy { get; set; }

        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(7)]
        public int ApprovalStatus { get; set; }
    }
}