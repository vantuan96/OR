using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.ZoneViolations
{
    [ProtoContract]
    public class SearchListCAPContract
    {
        [ProtoMember(1)]
        public List<CAPViolationInfoContract> Data { get; set; }
        [ProtoMember(11)]
        public int TotalRows { get; set; }
    }

    [ProtoContract]
    public class CAPViolationInfoContract
    {
        [ProtoMember(1)]
        public int EvaluationCriteriaId { get; set; }
        [ProtoMember(16)]
        public int EvaluationCalendarId { get; set; }
        [ProtoMember(3)]
        public int SiteZoneId { get; set; }
        [ProtoMember(4)]
        public string UrlImageError { get; set; }
        [ProtoMember(5)]
        public int DeppartmentId { get; set; }
        [ProtoMember(6)]
        public int StatusId { get; set; }

        [ProtoMember(7)]
        public string NoteReport { get; set; }

        [ProtoMember(8)]
        public string CateName { get; set; }
        [ProtoMember(9)]
        public string GroupCateName { get; set; }
        [ProtoMember(10)]
        public int CateId { get; set; }
        [ProtoMember(11)]
        public int GroupCateId { get; set; }

        [ProtoMember(12)]
        public string SiteZoneName { get; set; }
        [ProtoMember(13)]
        public string Propose { get; set; }
        [ProtoMember(14)]
        public int ViolationId { get; set; }
        [ProtoMember(15)]
        public string ViolationError { get; set; }

        [ProtoMember(17)]
        public int SiteId { get; set; }
        [ProtoMember(18)]
        public int SourceClientId { get; set; }
        [ProtoMember(19)]
        public int ViolationStatusId { get; set; }
        [ProtoMember(20)]
        public string ViolationStatusName { get; set; }
        [ProtoMember(21)]
        public string DepartmentName { get; set; }
        [ProtoMember(22)]
        public string CriteriaName { get; set; }

        [ProtoMember(23)]
        public DateTime? ReportDateEnd { get; set; }
        [ProtoMember(24)]
        public DateTime? ReportDateEndSecond { get; set; }
        [ProtoMember(25)]
        public string ReportDepartmentName { get; set; }

        [ProtoMember(26)]
        public DateTime? DateFinish { get; set; }
        [ProtoMember(27)]
        public string ImageReportUri { get; set; }

        [ProtoMember(28)]
        public int ReportDeptId { get; set; }
        [ProtoMember(29)]
        public DateTime CreatedDate { get; set; }





    }
}
