using ProtoBuf;
using System;

namespace Contract.User
{
    [ProtoContract]
    public class AdminRoleContract
    {
        [ProtoMember(1)]
        public int RoleId { get; set; }

        [ProtoMember(2)]
        public string RoleName { get; set; }

        [ProtoMember(3)]
        public int Sort { get; set; }

        [ProtoMember(4)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(5)]
        public DateTime LastUpdatedDate { get; set; }

    }
}
