using ProtoBuf;
using System.Collections.Generic;

namespace BMS.Contract.Product
{
    [ProtoContract]
    public class ProductAttributeMappingContract
    {        
        [ProtoMember(1)]
        public int ProductAttributeMappingId { get; set; }

        [ProtoMember(2)]
        public string LangShortName { get; set; }

        [ProtoMember(3)]
        public string Value { get; set; }

        [ProtoMember(4)]
        public int AttrId { get; set; }

        [ProtoMember(5)]
        public string AttrName { get; set; }

        [ProtoMember(6)]
        public bool IsDelete { get; set; }

    }
}
