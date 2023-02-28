using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using BMS.Contract.Shared;
using BMS.Contract.Microsite;

namespace BMS.Contract.Qcs.EvaluationSiteZones
{
    [ProtoContract]
    public partial class EvaluationCalendarContract
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
        public int EvaluationCalendarStatusId { get; set; }
        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(9)]
        public bool IsDeleted { get; set; }
      
        [ProtoMember(11)]
        public int SourceClientId { get; set; }
    }



    [ProtoContract]
    public partial class InsertManyEvaluationCalendarContract
    {
       
        [ProtoMember(2)]
        public string EvaluationCalendarName { get; set; }

        [ProtoMember(3)]
        public DateTime EvaluationStartDate { get; set; }

        [ProtoMember(4)]
        public DateTime EvaluationEndDate { get; set; }

        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }

        [ProtoMember(11)]
        public int SourceClientId { get; set; }

        [ProtoMember(12)]
        public List<MicrositeItemContract> ListMicrosite { get; set; }

        [ProtoMember(13)]
        public List<WeekRangeContract> ListWeekRange { get; set; }
    }




}
