using ProtoBuf;

namespace BMS.Contract.Qcs.SiteZoneViolations
{
    [ProtoContract]
    public class SiteZoneViolationItemContract
    {
        [ProtoMember(1)]
        public int ViolationId { get; set; }

        [ProtoMember(2)]
        public string UriImageViolationError { get; set; }

        [ProtoMember(3)]
        public int StatusId { get; set; }
    }
}
