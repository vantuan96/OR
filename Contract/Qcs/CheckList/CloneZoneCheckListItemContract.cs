using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.CheckList
{
    [ProtoContract]
    public class CloneZoneCriteriaContract
    {
        [ProtoMember(1)]
        public int SiteId { get; set; }
        [ProtoMember(2)]
        public int EvaluationCalendarId { get; set; }
        [ProtoMember(3)]
        public int GroupCateId { get; set; }
        [ProtoMember(4)]
        public int SourceZoneId { get; set; }
        [ProtoMember(5)]
        public int DestinationZoneId { get; set; }        
    }
}
