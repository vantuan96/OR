using ProtoBuf;

namespace BMS.Contract.Qcs
{
    [ProtoContract]
    public class EvalCriteriaGroupContract
    {
        /// <summary>
        /// Mã nhóm hạng mục
        /// </summary>
        [ProtoMember(1)]
        public int EvaluationCriteriaGroupId { get; set; }
        /// <summary>
        /// Tên nhóm hạng mục
        /// </summary>
        [ProtoMember(2)]
        public string EvaluationCriteriaGroupName { get; set; }
        /// <summary>
        /// Mã nhóm hạng mục
        /// </summary>
        [ProtoMember(3)]
        public string EvaluationCriteriaGroupCode { get; set; }
    }
}
