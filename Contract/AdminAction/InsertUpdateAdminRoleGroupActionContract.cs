using ProtoBuf;
using System;
using System.Collections.Generic;

namespace Contract.AdminAction
{
    [ProtoContract]
    public class InsertUpdateAdminRoleGroupActionContract
    {
        [ProtoMember(1)]
        public int RId { get; set; }
        
        [ProtoMember(2)]
        public List<int> GroupActions { get; set; }
        
    }
}
