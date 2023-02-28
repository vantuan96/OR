using ProtoBuf;

namespace BMS.Contract.Qcs.ViolRecords
{
    [ProtoContract]
    public class ViolRecordContract
    {        
        /// <summary>
        /// Mã số biên bản vi phạm
        /// </summary>
        [ProtoMember(1)]
        public int ViolRecordId { get; set; }
        /// <summary>
        /// Mã nhân viên vi phạm
        /// </summary>
        [ProtoMember(26)]
        public string StaffId { get; set; }
        /// <summary>
        /// Tên nhân viên vi phạm
        /// </summary>
        [ProtoMember(3)]
        public string StaffName { get; set; }
        /// <summary>
        /// Chức vụ nhân viên vi phạm
        /// </summary>
        [ProtoMember(4)]
        public string StaffPosition { get; set; }
        /// <summary>
        /// Mã master code của nhân viên vi phạm
        /// </summary>
        [ProtoMember(5)]
        public string StaffMasterCode { get; set; }
        /// <summary>
        /// Mã bộ phận của nhân viên vi phạm
        /// </summary>
        [ProtoMember(6)]
        public int StaffDeptId { get; set; }
        /// <summary>
        /// Tên bộ phận của nhân viên vi phạm
        /// </summary>
        [ProtoMember(7)]
        public string StaffDeptName { get; set; }

        /// <summary>
        /// Tên người lập biên bản
        /// </summary>
        [ProtoMember(8)]
        public string RecorderName { get; set; }
        /// <summary>
        /// Chức vụ người lập biên bản
        /// </summary>
        [ProtoMember(9)]
        public string RecorderPosition { get; set; }
        /// <summary>
        /// Bộ phận người lập biên bản
        /// </summary>
        [ProtoMember(10)]
        public int RecorderDeptId { get; set; }
        /// <summary>
        /// Tên bộ phận người lập biên bản
        /// </summary>
        [ProtoMember(11)]
        public string RecorderDeptName { get; set; }


        /// <summary>
        /// Mã nhóm lỗi vi phạm
        /// </summary>
        [ProtoMember(12)]
        public int ViolGroupId { get; set; }
        /// <summary>
        /// Tên nhóm lỗi vi phạm
        /// </summary>
        [ProtoMember(13)]
        public string ViolGroupName { get; set; }
        /// <summary>
        /// Nội dung biên bản vi phạm
        /// </summary>
        [ProtoMember(14)]
        public string ViolContent { get; set; }
        /// <summary>
        /// Đường dẫn hình biên bản vi phạm
        /// </summary>
        [ProtoMember(15)]
        public string ViolationUriImage { get; set; }        
        /// <summary>
        /// Ngày vi phạm
        /// </summary>
        [ProtoMember(16)]
        public System.DateTime ViolationDate { get; set; }
        /// <summary>
        /// Ngày lập biên bảng
        /// </summary>
        [ProtoMember(17)]
        public System.DateTime RecordDate { get; set; }
        /// <summary>
        /// Ngày phát hiện vi phạm
        /// </summary>
        [ProtoMember(18)]
        public System.DateTime ViolIssuesDate { get; set; }
        /// <summary>
        /// Ngày ký biên bản vi phạm
        /// </summary>
        [ProtoMember(19)]
        public System.DateTime SignDate { get; set; }
        /// <summary>
        /// Thời hạn khắc phụ vi phạm
        /// </summary>
        [ProtoMember(20)]
        public System.DateTime DeadlineToResolve { get; set; }
        
       
        /// <summary>
        /// Mã hình thức xử lý
        /// </summary>
        [ProtoMember(21)]
        public int ResolveTypeId { get; set; }
        /// <summary>
        /// Tên hình thức xử lý
        /// </summary>
        [ProtoMember(22)]
        public string ResolveTypeName { get; set; }
        /// <summary>
        /// Đề xuất thêm cho hình thức xử lý
        /// </summary>
        [ProtoMember(23)]
        public string ResolveTypeNote { get; set; }
        /// <summary>
        /// Đề xuất xử lý của KSCL
        /// </summary>
        [ProtoMember(24)]
        public string Proposes { get; set; }
        
        /// <summary>
        /// Mã trạng thái biên bản vi phạm
        /// </summary>
        [ProtoMember(25)]
        public int ViolationRecordStatusId { get; set; }
        /// <summary>
        /// Mã trạng thái biên bản vi phạm
        /// </summary>
        [ProtoMember(27)]
        public string ViolationRecordStatusName { get; set; }
        
    }
}
