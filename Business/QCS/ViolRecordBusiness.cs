using System;
using System.Linq;
using BMS.Contract.Qcs.ViolRecords;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO.QCS;

namespace BMS.Business.QCS
{
    public interface IViolRecordBusiness
    {
        PagedList<ViolRecordContract> FindViolRecords(int iViolDeptId, int iViolGrpId, int iViolStatusId, DateTime dtStartDate, DateTime dtEndDate, string strViolContent, int iPage, int iPageSize);

        ViolRecordContract getViolRecord(int violRecordId);
        CUDReturnMessage AddViolationRecord(ViolRecordContract violRecordContract);

        CUDReturnMessage UpdViolationRecord(ViolRecordContract violRecordContract);
    }

    public class ViolRecordBusiness : BaseBusiness, IViolRecordBusiness
    {
        private readonly Lazy<IViolRecordAccess> _violRecordAccess;
        public ViolRecordBusiness(string appid, int uid) : base(appid, uid)
        {
            _violRecordAccess = new Lazy<IViolRecordAccess>(() => new ViolRecordAccess(appid, uid));
        }
        /// <summary>
        /// Tìm thông tin biên bản vi phạm
        /// </summary>
        /// <param name="iViolDeptId">Mã bộ phận nhân viên vi phạm</param>
        /// <param name="iViolGrpId">Mã nhóm lỗi vi phạm</param>
        /// <param name="iViolStatusId">Mã trạng thái của biên bản vi phạm</param>
        /// <param name="dtStartDate">Từ ngày vi phạm</param>
        /// <param name="dtEndDate">Đến ngày vi phạm</param>
        /// <param name="strViolContent">Nội dung biên bản vi phạm</param>
        /// <param name="iPage">Trang thông tin cần lấy</param>
        /// <param name="iPageSize">Số lượng record cần lấy trên mỗi trang</param>
        /// <returns>Danh sách thông tin biên bản vi phạm</returns>
        public PagedList<ViolRecordContract> FindViolRecords(int iViolDeptId, int iViolGrpId, int iViolStatusId, DateTime dtStartDate, DateTime dtEndDate, string strViolContent, int iPage, int iPageSize)
        {
            var query = _violRecordAccess.Value.FindViolRecords(iViolDeptId, iViolGrpId, iViolStatusId, dtStartDate, dtEndDate, strViolContent);

            var result = new PagedList<ViolRecordContract>(query.Count());
            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(r => r.CreatedDate).Skip((iPage - 1) * iPageSize).Take(iPageSize).Select(r => new ViolRecordContract
                {
                    ViolRecordId = r.ViolationRecordId,
                    /* Thông tin nhân viên vi phạm*/
                    StaffId = r.StaffId,
                    StaffName = r.StaffName,
                    StaffMasterCode = r.StaffMasterCode,
                    StaffPosition = r.StaffPosition,
                    StaffDeptId = r.StaffDeptId ?? 0,
                    StaffDeptName = r.ViolationDepartment.DeptName,
                    /* Thông tin nhân viên lập biên bản*/
                    RecorderName = r.RecorderName,
                    RecorderPosition = r.RecorderPos,
                    RecorderDeptId = r.RecorderDept ?? 0,
                    RecorderDeptName = r.ViolationDepartment1.DeptName,
                    /* Thông tin vi phạm */
                    ViolGroupId = r.ViolationGroupId ?? 0,
                    ViolGroupName = r.ViolationGroup.ViolationGroupName,
                    ViolContent = r.ViolationContent,
                    ViolationUriImage = r.ViolationUriImage,
                    ViolationDate = r.ViolationDate ?? DateTime.MinValue,
                    RecordDate = r.RecordDate ?? DateTime.MinValue,
                    ViolIssuesDate = r.ViolationIssuesDate ?? DateTime.MinValue,
                    SignDate = r.SignDate ?? DateTime.MinValue,
                    /* Hình thúc phạt */
                    ResolveTypeId = r.ResolveTypeId ?? 0,
                    ResolveTypeName = r.ResolveType.ResolveTypeName,
                    ResolveTypeNote = r.ResolveTypeNote,
                    DeadlineToResolve = r.DeadlineToResolve ?? DateTime.MinValue,
                    Proposes = r.Proposes,
                    /* Trạng thái biên bản vi phạm*/
                    ViolationRecordStatusId = r.ViolationRecordStatusId,
                    ViolationRecordStatusName = r.ViolationRecordStatu.ViolationRecordStatusName
                }).ToList();
            }
            return result;

        }

        public ViolRecordContract getViolRecord(int violRecordId)
        {
            var result = _violRecordAccess.Value.GetViolRecord(violRecordId);
            if (result != null)
            {
                return new ViolRecordContract
                {
                    ViolRecordId = result.ViolationRecordId,
                    /* Thông tin nhân viên vi phạm*/
                    StaffId = result.StaffId,
                    StaffName = result.StaffName,
                    StaffMasterCode = result.StaffMasterCode,
                    StaffPosition = result.StaffPosition,
                    StaffDeptId = result.StaffDeptId ?? 0,
                    StaffDeptName = result.ViolationDepartment != null ? result.ViolationDepartment.DeptName : "",
                    /* Thông tin nhân viên lập biên bản*/
                    RecorderName = result.RecorderName,
                    RecorderPosition = result.RecorderPos,
                    RecorderDeptId = result.RecorderDept ?? 0,
                    RecorderDeptName = result.ViolationDepartment1 != null ? result.ViolationDepartment1.DeptName : "",
                    /* Thông tin vi phạm */
                    ViolGroupId = result.ViolationGroupId ?? 0,
                    ViolGroupName = result.ViolationGroup != null ? result.ViolationGroup.ViolationGroupName : "",
                    ViolContent = result.ViolationContent,
                    ViolationUriImage = result.ViolationUriImage,
                    ViolationDate = result.ViolationDate ?? DateTime.MinValue,
                    RecordDate = result.RecordDate ?? DateTime.MinValue,
                    ViolIssuesDate = result.ViolationIssuesDate ?? DateTime.MinValue,
                    SignDate = result.SignDate ?? DateTime.MinValue,
                    /* Hình thúc phạt */
                    ResolveTypeId = result.ResolveTypeId ?? 0,
                    ResolveTypeName = result.ResolveType != null ? result.ResolveType.ResolveTypeName : "",
                    ResolveTypeNote = result.ResolveTypeNote,
                    DeadlineToResolve = result.DeadlineToResolve ?? DateTime.MinValue,
                    Proposes = result.Proposes,
                    /* Trạng thái biên bản vi phạm*/
                    ViolationRecordStatusId = result.ViolationRecordStatusId,
                    ViolationRecordStatusName = result.ViolationRecordStatu.ViolationRecordStatusName
                };
            }

            return null;
        }





        /// <summary>
        /// Thêm mới biên bản vi phạm
        /// </summary>
        /// <param name="violRecordContract">Thông tin thêm mới biên bản vi phạm</param>
        /// <returns></returns>
        public CUDReturnMessage AddViolationRecord(ViolRecordContract violRecordContract)
        {
            return _violRecordAccess.Value.AddViolationRecord(violRecordContract);
        }
        /// <summary>
        /// Cập nhật thông tin biên bản vi phạm
        /// </summary>
        /// <param name="violRecordContract">Thông tin biên bản vi phạm cần cập nhật lại thông tin</param>
        /// <returns></returns>
        public CUDReturnMessage UpdViolationRecord(ViolRecordContract violRecordContract)
        {
            return _violRecordAccess.Value.UpdViolationRecord(violRecordContract);
        }
    }
}
