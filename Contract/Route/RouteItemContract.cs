using ProtoBuf;

namespace BMS.Contract.Route
{
    [ProtoContract]
    public class RouteItemContract
    {
        [ProtoMember(1)]
        public int RouteId { get; set; }

        [ProtoMember(2)]
        public int MsId { get; set; }

        [ProtoMember(3)]
        public string MicrositeName { get; set; }

        [ProtoMember(4)]
        public string RouteName { get; set; }

        [ProtoMember(5)]
        public int Status { get; set; }
    }
}
