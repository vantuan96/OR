using ProtoBuf;

namespace BMS.Contract.Qcs
{
    /// <summary>
    /// Bộ phận đánh giá
    /// </summary>
    [ProtoContract]
    public class AdminDepartmentContract
    {
        /// <summary>
        /// Mã bộ phận đánh giá
        /// </summary>
        [ProtoMember(1)]
        public int DeptId { get; set; }
        /// <summary>
        /// Tên bộ phận đánh giá
        /// </summary>
        [ProtoMember(2)]
        public string DeptName { get; set; }
        /// <summary>
        /// Mã code của bộ phận đánh giá
        /// </summary>
        [ProtoMember(3)]
        public string DeptCode { get; set; }
        /// <summary>
        /// Mã bộ phận cha
        /// </summary>
        [ProtoMember(4)]
        public int DeptParentId { get; set; }
        /// <summary>
        /// Ngày tạo bộ phận đánh giá
        /// </summary>
        [ProtoMember(5)]
        public System.DateTime CreatedDate { get; set; }
        /// <summary>
        /// Mã nhân viên tạo bộ phận đánh giá
        /// </summary>
        [ProtoMember(6)]
        public int CreatedBy { get; set; }
        /// <summary>
        /// Ngày chỉnh sửa cuối
        /// </summary>
        [ProtoMember(7)]
        public System.DateTime LastUpdatedDate { get; set; }
        /// <summary>
        /// Người thao tác chỉnh sửa cuối
        /// </summary>
        [ProtoMember(8)]
        public int LastUpdatedBy { get; set; }
        /// <summary>
        /// Trạng thái đã xóa hay không?
        /// </summary>
        [ProtoMember(9)]
        public bool IsDeleted { get; set; }        
    }
}