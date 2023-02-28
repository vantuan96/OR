using ProtoBuf;

namespace BMS.Contract.Qcs
{
    /// <summary>
    /// Thông tin hạng mục đánh giá
    /// </summary>
    [ProtoContract]
    public class EvalCategoryGroupContract
    {
        /// <summary>
        /// Mã loai hạng mục đánh giá
        /// </summary>
        [ProtoMember(1)]
        public int EvaluationCateGroupId { get; set; }
        /// <summary>
        /// Tên loai hạng mục đánh giá
        /// </summary>
        [ProtoMember(2)]
        public string EvaluationCateGroupName { get; set; }
        /// <summary>
        /// Mã code loai hạng mục đánh giá
        /// </summary>
        [ProtoMember(3)]
        public string EvaluationCateGroupCode { get; set; }
        /// <summary>
        /// Ngày tao
        /// </summary>
        [ProtoMember(4)]
        public System.DateTime CreatedDate { get; set; }
        /// <summary>
        /// Mã nhân viên tạo loai hang muc
        /// </summary>
        [ProtoMember(5)]
        public int CreatedBy { get; set; }
        /// <summary>
        /// Ngày chỉnh sửa cuối
        /// </summary>
        [ProtoMember(6)]
        public System.DateTime LastUpdatedDate { get; set; }
        /// <summary>
        /// Người thao tác chỉnh sửa cuối
        /// </summary>
        [ProtoMember(7)]
        public int LastUpdatedBy { get; set; }
        /// <summary>
        /// Trạng thái đã xóa hay không?
        /// </summary>
        [ProtoMember(8)]
        public bool IsDeleted { get; set; }
        /// <summary>
        /// SourceClientId
        /// </summary>
        [ProtoMember(9)]
        public int SourceClientId { get; set; }
        
    }
}