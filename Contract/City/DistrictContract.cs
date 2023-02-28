using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.City
{
    [ProtoContract]
    public class DistrictContract
    {
        [ProtoMember(1)]
        public int DistrictId { get; set; }

        [ProtoMember(2)]
        public int CityId { get; set; }

        [ProtoMember(3)]
        public string Prefix { get; set; }

        [ProtoMember(4)]
        public string DistrictName { get; set; }

        [ProtoMember(5)]
        public string DistrictNameEN { get; set; }

        [ProtoMember(6)]
        public string ShortName { get; set; }

        [ProtoMember(7)]
        public double? Latitude { get; set; }

        [ProtoMember(8)]
        public double? Longitude { get; set; }

        [ProtoMember(9)]
        public int Priority { get; set; }
    }
}
