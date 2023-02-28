using ProtoBuf;
using System.Collections.Generic;
using BMS.Contract.Qcs.SiteZoneViolations;

namespace BMS.Contract.Qcs.EvaluationSiteZones
{
    [ProtoContract]
    public class EvaluationSiteZoneDetailContract
    {
        [ProtoMember(1)]
        public int EvaluationSiteZoneId { get; set; }

        [ProtoMember(2)]
        public int SiteZoneId { get; set; }

        [ProtoMember(3)]
        public int EvaluationCalendarId { get; set; }

        [ProtoMember(4)]
        public int EvaluationCriteriaId { get; set; }

        [ProtoMember(5)]
        public int EvaluationStatusId { get; set; }

        [ProtoMember(6)]
        public List<SiteZoneViolationItemContract> ListViolations { get; set; }
    }
}
