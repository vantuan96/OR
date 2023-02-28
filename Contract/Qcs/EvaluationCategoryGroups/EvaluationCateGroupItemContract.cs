using ProtoBuf;

namespace BMS.Contract.Qcs.EvaluationCategoryGroups
{
    [ProtoContract]
    public class EvaluationCateGroupItemContract
    {
        [ProtoMember(1)]
        public int EvaluationCateGroupId { get; set; }
        [ProtoMember(2)]
        public string EvaluationCateGroupName { get; set; }
        [ProtoMember(3)]
        public string EvaluationCateGroupCode { get; set; }
    }
}
