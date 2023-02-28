using ProtoBuf;
using System.Collections.Generic;

namespace Contract.User
{
    [ProtoContract]
    public class CreateUpdateUserContract
    {
        [ProtoMember(1)]
        public int UserId { get; set; }

        [ProtoMember(2)]
        public string Email { get; set; }

        [ProtoMember(3)]
        public string Fullname { get; set; }

        [ProtoMember(4)]
        public List<int> Roles { get; set; }

        [ProtoMember(5)]
        public List<int> MicroSites { get; set; }

        [ProtoMember(6)]
        public int? DeptId { get; set; }

        [ProtoMember(7)]
        public string PhoneNumber { get; set; }

        [ProtoMember(8)]
        public string Username { get; set; }

        [ProtoMember(9)]
        public bool IsADAccount { get; set; }

        //[ProtoMember(10)]
        //public List<int> PnLs { get; set; }

        //[ProtoMember(11)]
        //public List<int> Sites { get; set; }

        [ProtoMember(12)]
        public List<int> Locations { get; set; }
        [ProtoMember(13)]
        public int LineManagerId { get; set; }
    }
}
