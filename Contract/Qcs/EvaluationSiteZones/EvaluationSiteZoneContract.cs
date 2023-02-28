using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.EvaluationSiteZones
{
    [ProtoContract]
    public class EvaluationSiteZoneContract
    {
        [ProtoMember(0)]
        public int EvaluationSiteZoneId { get; set; }
        [ProtoMember(1)]
        public int SiteZoneId { get; set; }
        [ProtoMember(2)]
        public int EvaluationCalendarId { get; set; }
        [ProtoMember(3)]
        public int EvaluationCriteriaId { get; set; }
        [ProtoMember(4)]
        public int EvaluationStatusId { get; set; }
        [ProtoMember(5)]
        public System.DateTime LastUpdatedDate { get; set; }
        [ProtoMember(6)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(7)]
        public bool IsDeleted { get; set; }
    }
}
