using ProtoBuf;

namespace BMS.Contract.Qcs
{
    /// <summary>
    /// Thông tin hạng mục đánh giá
    /// </summary>
    [ProtoContract]
    public class EvaluationCategoryContract
    {
        /// <summary>
        /// Mã hạng mục đánh giá
        /// </summary>
        [ProtoMember(1)]
        public int CateId { get; set; }
        /// <summary>
        /// Tên hạng mục đánh giá
        /// </summary>
        [ProtoMember(2)]
        public string CateName { get; set; }
        /// <summary>
        /// Mã code hạng mục đánh giá
        /// </summary>
        [ProtoMember(3)]
        public string CateCode { get; set; }
        /// <summary>
        /// Mã nhóm hạng mục đánh giá
        /// </summary>
        [ProtoMember(4)]
        public int CateGroupId { get; set; }
        /// <summary>
        /// Tên nhóm hạng mục đánh giá
        /// </summary>
        [ProtoMember(5)]
        public string CateGroupName { get; set; }

        /// <summary>
        /// Loại đánh giá
        /// </summary>
        [ProtoMember(6)]
        public int EvalTypeId { get; set; }
    }
}