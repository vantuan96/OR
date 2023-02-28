using ProtoBuf;

namespace BMS.Contract.Address
{
    [ProtoContract]
    public class AddressCreateContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(3)]
        public int CityId { get; set; }
        [ProtoMember(4)]
        public int DistrictId { get; set; }
        [ProtoMember(5)]
        public float Latitude { get; set; }
        [ProtoMember(6)]
        public float Longitude { get; set; }
        [ProtoMember(7)]
        public string FullAddress { get; set; }
        [ProtoMember(8)]
        public int Sort { get; set; }
       
        [ProtoMember(10)]
        public string Email { get; set; }
        [ProtoMember(11)]
        public string Fax { get; set; }
        [ProtoMember(12)]
        public int MsId { get; set; }
        [ProtoMember(13)]
        public int ApprovalStatus { get; set; }
        [ProtoMember(14)]
        public bool IsOnsite { get; set; }

        [ProtoMember(9)]
        public string PhoneNumber1 { get; set; }

        [ProtoMember(15)]
        public string PhoneNumber2 { get; set; }

        [ProtoMember(16)]
        public string Name { get; set; }
    }
}
