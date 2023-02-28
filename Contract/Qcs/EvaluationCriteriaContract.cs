using ProtoBuf;

namespace BMS.Contract.Qcs
{
    /// <summary>
    /// Thông tin Tiêu chuẩn đánh giá
    /// </summary>
    [ProtoContract]
    public class EvaluationCriteriaContract
    {
        /// <summary>
        /// Mã tiêu chuẩn đánh giá
        /// </summary>
        [ProtoMember(1)]
        public int Id { get; set; }
        /// <summary>
        /// Chi tiết tiêu chuẩn đánh giá
        /// </summary>
        [ProtoMember(2)]
        public string Name { get; set; }
        /// <summary>
        /// Điểm cho tiêu chuẩn đánh giá nếu đạt
        /// </summary>
        [ProtoMember(3)]
        public int Point { get; set; }

        /// <summary>
        /// Mã bộ phận đánh giá
        /// </summary>
        [ProtoMember(4)]
        public int DeptId { get; set; }
        /// <summary>
        /// Tên bộ phận đánh giá
        /// </summary>
        [ProtoMember(5)]
        public string DeptName { get; set; }
        /// <summary>
        /// Mã nhóm tiêu chuẩn đánh giá
        /// </summary>
        [ProtoMember(6)]
        public int CriteriaGroupId { get; set; }
        /// <summary>
        /// Tên nhóm tiêu chuẩn đánh giá
        /// </summary>
        [ProtoMember(7)]
        public string CriteriaGroupName { get; set; }
        /// <summary>
        /// Mã chuyên mục đánh giá
        /// </summary>
        [ProtoMember(8)]
        public int EvaluationCateId { get; set; }
        /// <summary>
        /// Tên chuyên mục đánh giá
        /// </summary>
        [ProtoMember(9)]
        public string EvaluationCateName { get; set; }
        /// <summary>
        /// Code chuyên mục đánh giá
        /// </summary>
        [ProtoMember(10)]
        public string EvaluationCateCode { get; set; }
        /// <summary>
        /// Mã nhóm chuyên mục đánh giá
        /// </summary>
        [ProtoMember(11)]
        public int EvaluationCateGroupId { get; set; }
        /// <summary>
        /// Tên nhóm chuyên mục đánh giá
        /// </summary>
        [ProtoMember(12)]
        public string EvaluationCateGroupName { get; set; }
    }
}