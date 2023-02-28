using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Contract.Qcs.Report
{
    [ProtoContract]
    public class ChecklistByCateReportContract
    {
        [ProtoMember(1)]
        public int DeptId { get; set; }

        [ProtoMember(2)]
        public string DeptName { get; set; }

        [ProtoMember(3)]
        public int EvaluationCateGroupId { get; set; }

        [ProtoMember(4)]
        public string EvaluationCateGroupName { get; set; }

        [ProtoMember(5)]
        public int Total { get; set; }

        [ProtoMember(6)]
        public int Checked { get; set; }
    }
}
