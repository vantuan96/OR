using ProtoBuf;
using System.Collections.Generic;

namespace BMS.Contract.FE
{
    [ProtoContract]
    public class AddressContract
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Email { get; set; }

        [ProtoMember(3)]
        public string PhoneNumber1 { get; set; }

        [ProtoMember(4)]
        public string PhoneNumber2 { get; set; }

        [ProtoMember(5)]
        public string Fax { get; set; }

        [ProtoMember(6)]
        public double Latitude { get; set; }

        [ProtoMember(7)]
        public double Longitude { get; set; }

        [ProtoMember(8)]
        public string FullAddress { get; set; }

        [ProtoMember(9)]
        public string CityName { get; set; }

        [ProtoMember(10)]
        public string DistrictName { get; set; }

        [ProtoMember(11)]
        public string CityNameEN { get; set; }

        [ProtoMember(12)]
        public string DistrictNameEN { get; set; }

        [ProtoMember(13)]
        public int CityId { get; set; }

        [ProtoMember(14)]
        public int DistrictId { get; set; }

        [ProtoMember(15)]
        public int? RegionId { get; set; }

        [ProtoMember(16)]
        public string Name { get; set; }

        private string showname = null;
        public string ShowName(Dictionary<int, string> dicSpecialCity)
        {
            if (showname == null)
            {
                if (dicSpecialCity.ContainsKey(CityId))
                {
                    showname = dicSpecialCity[CityId];
                }
                else
                {
                    showname = CityName;
                }
            }

            return showname;
        }
    }
}
