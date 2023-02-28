using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Microsite;

namespace Contract.User
{
    [ProtoContract]
    public class UserProfileContract
    {
        [ProtoMember(1)]
        public int UserId { get; set; }

        [ProtoMember(2)]
        public string Email { get; set; }

        [ProtoMember(3)]
        public string FullName { get; set; }

        [ProtoMember(4)]
        public DateTime CreatedDate { get; set; }

        [ProtoMember(5)]
        public List<AdminRoleContract> Roles { get; set; }

        [ProtoMember(6)]
        public List<MicrositeItemContract> MicroSites { get; set; }

        [ProtoMember(7)]
        public List<AdminUserPnLContract> PnLs { get; set; }

        [ProtoMember(8)]
        public List<AdminUserPnLSiteContract> Sites { get; set; }

        [ProtoMember(9)]
        public List<LocationContract> Locations { get; set; }
        
        public bool IsSuperAdmin
        {
            get
            {
                return Roles != null && Roles.Any(r => r.RoleId == (int)AdminRole.SuperAdmin);
            }
        }
    }
}
