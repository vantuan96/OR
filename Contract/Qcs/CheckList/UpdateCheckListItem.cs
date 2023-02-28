using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.CheckList
{
  
    [ProtoContract]
    public class UpdateCheckListItem
    {
        [ProtoMember(1)]
        public int ZoneId { get; set; }
        [ProtoMember(2)]
        public int CriteriaId { get; set; }
        [ProtoMember(3)]
        public int ItemStatusId { get; set; }
        [ProtoMember(4)]
        public int EvaluationCalendarId { get; set; }
        [ProtoMember(5)]
        public int EvaluationSiteZoneId { get; set; }
        [ProtoMember(6)]
        public Boolean IsDeleted { get; set; }
    }
}
