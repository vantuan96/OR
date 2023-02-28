using ProtoBuf;

namespace BMS.Contract.Route
{
    [ProtoContract]
    public class RouteUpdateContract
    {
        [ProtoMember(1)]
        public int RouteId { get; set; }

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
        public int MsId { get; set; }
        
        [ProtoMember(10)]
        public int Status { get; set; }

        [ProtoMember(11)]
        public int Sort { get; set; }
    }
}
