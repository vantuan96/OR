using ProtoBuf;
using System;

namespace BMS.Contract.Address
{
    [ProtoContract]
    public class AddressContentContract
    {
        [ProtoMember(1)]
        public int ContentId { get; set; }
        [ProtoMember(3)]
        public string FullAddress { get; set; }
        [ProtoMember(4)]
        public string LangShortName { get; set; }
        [ProtoMember(5)]
        public int CreatedBy { get; set; }
        [ProtoMember(6)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(7)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(8)]
        public DateTime LastUpdatedDate { get; set; }
        [ProtoMember(9)]
        public int AddressId { get; set; }

        [ProtoMember(10)]
        public string Name { get; set; }
    }
}
