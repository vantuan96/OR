using ProtoBuf;
using System.Collections.Generic;

namespace BMS.Contract.Qcs.SiteZones
{
    [ProtoContract]
    public class ZoneItemContract
    {
        [ProtoMember(1)]
        public int SiteZoneId { get; set; }

        [ProtoMember(2)]
        public string SiteZoneName { get; set; }

        [ProtoMember(3)]
        public string SiteZoneCode { get; set; }

        [ProtoMember(4)]
        public List<int> EvaluationCateGroupId { get; set; }
    }
}
