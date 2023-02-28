using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace BMS.Contract.Qcs.EvaluationCategoryGroups
{
    [ProtoContract]
    public class EvaluationCategoryGroupContract
    {
        [ProtoMember(1)]
        public int EvaluationCateGroupId { get; set; }
        [ProtoMember(2)]
        public string EvaluationCateGroupName { get; set; }
        [ProtoMember(3)]
        public string EvaluationCateGroupCode { get; set; }
        [ProtoMember(4)]
        public System.DateTime CreatedDate { get; set; }
        [ProtoMember(5)]
        public int CreatedBy { get; set; }
        [ProtoMember(6)]
        public System.DateTime LastUpdatedDate { get; set; }
        [ProtoMember(7)]
        public int LastUpdatedBy { get; set; }
        [ProtoMember(8)]
        public bool IsDeleted { get; set; }
        [ProtoMember(9)]
        public int SourceClientId { get; set; }
    }
}
