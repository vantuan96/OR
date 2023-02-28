using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs.Report
{
    [ProtoContract]
    public class ChecklistBySiteReportContract
    {
        [ProtoMember(1)]
        public int DeptId { get; set; }

        [ProtoMember(2)]
        public string DeptName { get; set; }

        [ProtoMember(3)]
        public int EvaluationCalendarId { get; set; }

        [ProtoMember(4)]
        public string EvaluationCalendarName { get; set; }

        [ProtoMember(5)]
        public int Total { get; set; }

        [ProtoMember(6)]
        public int Checked { get; set; }

        [ProtoMember(7)]
        public int SiteId { get; set; }
    }
}
