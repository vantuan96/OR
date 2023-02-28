using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.CheckList
{

    [ProtoContract]
    public class AnalysicGroupCateContract
    {
        public AnalysicGroupCateContract()
        {
          
        }
        [ProtoMember(1)]
        public int GroupCateId { get; set; }
        [ProtoMember(2)]
        public string GroupCateName { get; set; }
        [ProtoMember(3)]
        public int QuantityCheck { get; set; }
        [ProtoMember(4)]
        public int Total { get; set; }
    }
    [ProtoContract]
    public class AnalysicZoneContract
    {
        [ProtoMember(1)]
        public int ZoneId { get; set; }
        [ProtoMember(2)]
        public string ZoneName { get; set; }
        [ProtoMember(3)]
        public int QuantityCheck { get; set; }
        [ProtoMember(4)]
        public int Total { get; set; }
    }
    [ProtoContract]
    public class AnalysicCateContract
    {
        public AnalysicCateContract()
        {
            Items = new List<AnalysicCheckListItem>();
        }
        [ProtoMember(1)]
        public int CateId { get; set; }
        [ProtoMember(2)]
        public string CateName { get; set; }
        [ProtoMember(3)]
        public List<AnalysicCheckListItem> Items { get; set; }

    }

    [ProtoContract]
    public class AnalysicCheckListItem
    {
        public AnalysicCheckListItem()
        {
            Violations=new List<SiteZoneViolationContract>();
        }

        [ProtoMember(1)]
        public int DeptId { get; set; }
        [ProtoMember(2)]
        public string DeptName { get; set; }
        [ProtoMember(3)]
        public int CateId { get; set; }
        [ProtoMember(4)]
        public string CateName { get; set; }
        [ProtoMember(5)]
        public int CriteriaGroupId { get; set; }
        [ProtoMember(6)]
        public string CriteriaGroupName { get; set; }

        [ProtoMember(7)]
        public int CriteriaId { get; set; }

        [ProtoMember(8)]
        public string CriteriaName { get; set; }

        [ProtoMember(9)]
        public int ItemStatusId { get; set; }
        [ProtoMember(10)]
        public string ItemStatusName { get; set; }
       [ProtoMember(12)]
        public int Point { get; set; }
        [ProtoMember(13)]
        public int GroupCateId { get; set; }
        [ProtoMember(14)]
        public string GroupCateName { get; set; }
        [ProtoMember(15)]
        public int ZoneId { get; set; }
        [ProtoMember(16)]
        public string ZoneName { get; set; }
        [ProtoMember(17)]
        public List<SiteZoneViolationContract> Violations { get; set; }

        [ProtoMember(18)]
        public int EvaluationSiteZoneId { get; set; }

    }

    
    public class AnalysicGroupCate
    {
        public  int GroupCateId { get; set; }
        public int CriteriaId { get; set; }
        public int ZoneId { get; set; }
        public int CalendarId { get; set; }
        public int SiteId { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int CateId { get; set; }
        public string CateName { get; set; }
        public int EvaluationSiteZoneId { get; set; }
    }

    public class AnalysicGroupCateInfo
    {
        public int GroupCateId { get; set; }
        public int TotalCheck { get; set; }
    }
    [ProtoContract]
    public class SiteZoneViolationContract
    {
        [ProtoMember(1)]
        public int ViolationId { get; set; }
        [ProtoMember(2)]
        public int SiteZoneId { get; set; }
        [ProtoMember(3)]
        public string UriImageViolationError { get; set; }
        [ProtoMember(4)]
        public string ViolationError { get; set; }
        [ProtoMember(5)]
        public int EvaluationDeptId { get; set; }
        [ProtoMember(6)]
        public string Propose { get; set; }
        [ProtoMember(7)]
        public string NoteReport { get; set; }
        [ProtoMember(8)]
        public int ViolationStatusId { get; set; }
        [ProtoMember(9)]
        public int EvaluationSiteZoneId { get; set; }
        [ProtoMember(10)]
        public DateTime ReportDateEnd { get; set; }
        [ProtoMember(11)]
        public int ReportDeptId { get; set; }
        [ProtoMember(12)]
        public string ImageReportUri { get; set; }
    }

}
