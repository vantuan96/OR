using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.EvaluationSiteZones
{
    

    [ProtoContract]
    public class SearchEvaluationCalendarContract
    {
        [ProtoMember(1)]
        public List<EvaluationCalendarInfoContract> Data { get; set; }
        [ProtoMember(2)]
        public int TotalRows { get; set; }
    }
    [ProtoContract]
    public class EvaluationCalendarInfoContract
    {
        [ProtoMember(1)]
        public int EvaluationCalendarId { get; set; }
        [ProtoMember(2)]
        public string EvaluationCalendarName { get; set; }
        [ProtoMember(3)]
        public DateTime EvaluationStartDate { get; set; }
        [ProtoMember(4)]
        public DateTime EvaluationEndDate { get; set; }
        [ProtoMember(5)]
        public int SiteId { get; set; }
        [ProtoMember(6)]
        public string SiteName { get; set; }
        [ProtoMember(7)]
        public int EvaluationCalendarStatusId { get; set; }
        [ProtoMember(8)]
        public string EvaluationCalendarStatusName { get; set; }
    }
}
