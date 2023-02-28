using ProtoBuf;

namespace BMS.Contract.City
{
    [ProtoContract]
    public class CityContract
    {
        [ProtoMember(1)]
        public int CityId { get; set; }

        [ProtoMember(2)]
        public string Prefix { get; set; }

        [ProtoMember(3)]
        public string CityName { get; set; }

        [ProtoMember(4)]
        public string CityNameEN { get; set; }

        [ProtoMember(5)]
        public string ShortName { get; set; }

        [ProtoMember(6)]
        public double? Latitude { get; set; }

        [ProtoMember(7)]
        public double? Longitude { get; set; }

        [ProtoMember(8)]
        public int Priority { get; set; }
    }
}
