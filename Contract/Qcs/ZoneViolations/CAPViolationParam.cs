using System;
using System.Collections.Generic;
using ProtoBuf;

namespace BMS.Contract.Qcs.ZoneViolations
{
    [ProtoContract]
    public class CAPViolationParam
    {
        public CAPViolationParam()
        {
            SiteZoneIds=new List<int>();
        }
        [ProtoMember(1)]
        public int EvaluationCriteriaId { get; set; }
        [ProtoMember(11)]
        public int EvaluationCalendarId { get; set; }
        [ProtoMember(3)]
        public List<int> SiteZoneIds { get; set; }
        [ProtoMember(4)]
        public string UrlImageError { get; set; }
        [ProtoMember(5)]
        public int DeppartmentId { get; set; }
        [ProtoMember(6)]
        public int StatusId { get; set; }
        [ProtoMember(7)]
        public string ViolationError { get; set; }
        [ProtoMember(8)]
        public string Propose { get; set; }
        [ProtoMember(9)]
        public string NoteReport { get; set; }
        [ProtoMember(10)]
        public int ViolationId { get; set; }

    }
}