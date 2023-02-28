using ProtoBuf;
using System;

namespace Contract.AdminAction
{
    [ProtoContract]
    public class AdminGroupActionContract
    {
        [ProtoMember(1)]
        public int GaId { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string Description { get; set; }

        [ProtoMember(4)]
        public int CreatedBy { get; set; }

        [ProtoMember(5)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(6)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(7)]
        public DateTime LastUpdatedDate { get; set; }

        [ProtoMember(8)]
        public bool IsDeleted { get; set; }
    }
}
