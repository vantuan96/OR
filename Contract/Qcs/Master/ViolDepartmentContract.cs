using ProtoBuf;

namespace BMS.Contract.Qcs.Master
{
    [ProtoContract]
    public class ViolDepartmentContract
    {
        /// <summary>
        /// Mã bộ phận
        /// </summary>
        [ProtoMember(1)]
        public int DeptId { get; set; }
        /// <summary>
        /// Tên bộ phận
        /// </summary>
        [ProtoMember(2)]
        public string DeptName { get; set; }
        /// <summary>
        /// Mã code bộ phận
        /// </summary>
        [ProtoMember(3)]
        public string DeptCode { get; set; }
        /// <summary>
        /// Mã bộ phận cấp trên của bộ phận hiện tại
        /// </summary>
        [ProtoMember(4)]
        public int DeptParentId { get; set; }

        /// <summary>
        /// Loại phòng ban
        /// </summary>
        [ProtoMember(5)]
        public int TypeId { get; set; }
    }
}
