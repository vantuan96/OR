using ProtoBuf;

namespace BMS.Contract.Route
{
    [ProtoContract]
    public class RouteForFEContract
    {
        [ProtoMember(9)]
        public int RouteId { get; set; }

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

        [ProtoMember(10)]
        public int Sort { get; set; }

        [ProtoMember(11)]
        public RouteDetailForFEContract RouteDetail { get; set; }
    }

    [ProtoContract]
    public class RouteDetailForFEContract
    {

        [ProtoMember(1)]
        public string RouteName { get; set; }

        [ProtoMember(2)]
        public string TravelTime { get; set; }

        [ProtoMember(3)]
        public string Distance { get; set; }

        [ProtoMember(4)]
        public string ContactName { get; set; }

        [ProtoMember(5)]
        public string ContactPhone { get; set; }

        [ProtoMember(6)]
        public string LangShortName { get; set; }
       
    }


}
