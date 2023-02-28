using ProtoBuf;
using System;
using System.Collections.Generic;

namespace BMS.Contract.Address
{
    [ProtoContract]
    public class AddressDetailContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public int MsId { get; set; }
        [ProtoMember(3)]
        public int CityId { get; set; }
        [ProtoMember(4)]
        public int DistrictId { get; set; }
        [ProtoMember(5)]
        public double Latitude { get; set; }
        [ProtoMember(6)]
        public double Longitude { get; set; }
        [ProtoMember(7)]
        public int ApprovalStatus { get; set; }
        [ProtoMember(8)]
        public bool IsOnsite { get; set; }
        [ProtoMember(9)]
        public int Sort { get; set; }
        [ProtoMember(10)]
        public int CreatedBy { get; set; }
        [ProtoMember(11)]
        public DateTime CreatedDate { get; set; }
        [ProtoMember(12)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(13)]
        public DateTime LastUpdatedDate { get; set; }
        [ProtoMember(14)]
        public List<AddressContentContract> ListContent { get; set; }
        
        [ProtoMember(16)]
        public string Email { get; set; }
        [ProtoMember(17)]
        public string Fax { get; set; }

        [ProtoMember(15)]
        public string PhoneNumber1 { get; set; }

        [ProtoMember(18)]
        public string PhoneNumber2 { get; set; }
    }
}
