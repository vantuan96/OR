using ProtoBuf;
using System.Collections.Generic;
using System.Linq;
using Contract.Microsite;

namespace Contract.User
{
    [ProtoContract]
    public class MemberExtendContract
    {
        [ProtoMember(1)]
        public string DisplayName { get; set; }

        [ProtoMember(2)]
        public int UserId { get; set; }

        [ProtoMember(3)]
        public string Email { set; get; }
        
        [ProtoMember(4)]
        public List<AdminRoleContract> Roles { get; set; }
        
        [ProtoMember(5)]
        public List<AdminActionContract> ListMemberAllowedActions { get; set; }
        
        [ProtoMember(6)]
        public bool IsRequireChangePass { get; set; }

        [ProtoMember(7)]
        public List<MicrositeItemContract> ListMicrosites { get; set; }

        //[ProtoMember(8)]
        //public MicrositeItemContract CurrentMicrosite { get; set; }

        [ProtoMember(9)]
        public string PhoneNumber { set; get; }

        //[ProtoMember(10)]
        //public List<AdminUserPnLContract> PnLs { get; set; }

        //[ProtoMember(11)]
        //public List<AdminUserPnLSiteContract> Sites { get; set; }

        [ProtoMember(12)]
        public List<LocationContract> Locations { get; set; }
        public LocationContract CurrentLocaltion
        {
            get
            {
                LocationContract returnValue = null;
                //if (Locations != null && Locations.Count == 1)
                //{
                //    returnValue = Locations[0];
                //}
                //else 
                if (Locations != null && Locations.Count >= 1)
                {
                    returnValue = Locations.Where(x => x.IsCurrent).FirstOrDefault();
                    //if(returnValue==null)
                    //    returnValue = Locations.FirstOrDefault();
                }
                return returnValue;
            }
        }
        public int CurrentLocationId {
            get {
                int returnValue = 0;
                //if(Locations!=null && Locations.Count == 1)
                //{
                //    returnValue = Locations[0].LocationId;
                //}
                //else 
                if(Locations!=null && Locations.Count>= 1)
                {
                    returnValue = Locations.Where(x => x.IsCurrent).Select(x1 => x1.LocationId).FirstOrDefault();
                    //if(returnValue<=0)
                    //    returnValue = Locations.Select(x1 => x1.LocationId).FirstOrDefault();
                }
                return returnValue;
            }
        }

        public AdminActionContract DefaultAction
        {
            get
            {
                if (ListMemberAllowedActions == null)
                    return null;

                return ListMemberAllowedActions.Where(r => r.IsDefault && r.IsShowMenu).FirstOrDefault();
            }
        }

        public bool IsSuperAdmin
        {
            get
            {
                return Roles != null && Roles.Any(r => r.RoleId == (int)AdminRole.SuperAdmin);
            }
        }
        public bool IsManageSurgery
        {
            get
            {
                return Roles != null && Roles.Any(r => r.RoleId == (int)AdminRole.ManagSurgery);
            }
        }
        public bool IsManageAdminSurgery
        {
            get
            {
                return Roles != null && Roles.Any(r => r.RoleId == (int)AdminRole.ManagAdminSurgery);
            }
        }
        public bool IsManagAnes
        {
            get
            {
                return Roles != null && Roles.Any(r => r.RoleId == (int)AdminRole.ManagAnes);
            }
        }
        public bool ManagAnesProcessStep
        {
            get
            {
                return Roles != null && Roles.Any(r => r.RoleId == (int)AdminRole.ManagAnesProcessStep);
            }
        }
        [ProtoMember(13)]
        public string LineManagerEmail { set; get; }
        [ProtoMember(14)]
        public int LineManagerId { set; get; }
        public string UserAccount { set; get; }
        [ProtoMember(15)]
        public string ClientIp { get; set; }
        public string TokenKey { get; set; }
    }
}
