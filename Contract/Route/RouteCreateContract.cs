using ProtoBuf;

namespace BMS.Contract.Route
{
    [ProtoContract]
    public class RouteCreateContract
    {
        [ProtoMember(1)]
        public int MsId { get; set; }

        [ProtoMember(2)]
        public double CenterPosLatitude { get; set; }

        [ProtoMember(3)]
        public double CenterPosLongitude { get; set; }

        [ProtoMember(4)]
        public double FromLatitude { get; set; }

        [ProtoMember(5)]
        public double FromLongitude { get; set; }

        [ProtoMember(6)]
        public double ToLatitude { get; set; }

        [ProtoMember(7)]
        public double ToLongitude { get; set; }

        [ProtoMember(8)]
        public int ZoomLevel { get; set; }

        [ProtoMember(9)]
        public string RouteName { get; set; }

        [ProtoMember(10)]
        public string TravelTime { get; set; }

        [ProtoMember(11)]
        public string Distance { get; set; }

        [ProtoMember(12)]
        public string ContactName { get; set; }

        [ProtoMember(13)]
        public string ContactPhone { get; set; }

        [ProtoMember(14)]
        public string LangShortName { get; set; }

        [ProtoMember(15)]
        public int Status { get; set; }

        [ProtoMember(16)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(17)]
        public int Sort { get; set; }
    }
}
