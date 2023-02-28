using ProtoBuf;
using System;

namespace BMS.Contract.Language
{
    [ProtoContract]
    public class LanguageContract
    {
        [ProtoMember(1)]
        public int LanguageId { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string ShortName { get; set; }

        [ProtoMember(4)]
        public bool IsDefault { get; set; }

        [ProtoMember(5)]
        public string IconUrl { get; set; }

        [ProtoMember(6)]
        public bool IsActive { get; set; }

        [ProtoMember(7)]
        public int Status { get; set; }

        [ProtoMember(8)]
        public bool IsOnsite { get; set; }

        [ProtoMember(9)]
        public int CreatedBy { get; set; }

        [ProtoMember(10)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(11)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(12)]
        public DateTime LastUpdatedDate { get; set; }
    }
}