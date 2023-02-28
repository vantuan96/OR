using ProtoBuf;

namespace BMS.Contract.Route
{
    [ProtoContract]
    public class RouteContentCreateUpdateContract
    {
        [ProtoMember(1)]
        public int RouteContentId { get; set; }
                
        [ProtoMember(2)]
        public string RouteName { get; set; }

        [ProtoMember(3)]
        public string TravelTime { get; set; }

        [ProtoMember(4)]
        public string Distance { get; set; }

        [ProtoMember(5)]
        public string ContactName { get; set; }

        [ProtoMember(6)]
        public string ContactPhone { get; set; }

        [ProtoMember(7)]
        public string LangShortName { get; set; }
        
        [ProtoMember(8)]
        public int ApprovalStatus { get; set; }

        [ProtoMember(9)]
        public int RouteId { get; set; }

    }
}
