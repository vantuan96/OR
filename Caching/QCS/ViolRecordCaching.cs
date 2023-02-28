using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Business.QCS;
using BMS.Contract.Qcs.ViolRecords;
using BMS.Contract.Shared;

namespace BMS.Caching.QCS
{
    public interface IViolRecordCaching
    {
        PagedList<ViolRecordContract> FindViolRecords(int iViolDeptId, int iViolGrpId, int iViolStatusId, DateTime dtStartDate, DateTime dtEndDate, string strViolContent, int iPage, int iPageSize);

        ViolRecordContract getViolRecord(int violRecordId);

        CUDReturnMessage AddViolationRecord(ViolRecordContract violRecordContract);

        CUDReturnMessage UpdViolationRecord(ViolRecordContract violRecordContract);
    }
    public class ViolRecordCaching : BaseCaching, IViolRecordCaching
    {
        private readonly Lazy<IViolRecordBusiness> _violRecordBusiness;
        public ViolRecordCaching(/*string appid, int uid*/)
             
        {
            _violRecordBusiness = new Lazy<IViolRecordBusiness>(() => new ViolRecordBusiness(appid, uid));
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
            return _violRecordBusiness.Value.FindViolRecords(iViolDeptId, iViolGrpId, iViolStatusId, dtStartDate, dtEndDate, strViolContent, iPage, iPageSize);
        }

        public ViolRecordContract getViolRecord(int violRecordId)
        {
            return _violRecordBusiness.Value.getViolRecord(violRecordId);
        }

        /// <summary>
        /// Thêm mới biên bản vi phạm
        /// </summary>
        /// <param name="violRecordContract">Thông tin thêm mới biên bản vi phạm</param>
        /// <returns></returns>
        public CUDReturnMessage AddViolationRecord(ViolRecordContract violRecordContract)
        {
            return _violRecordBusiness.Value.AddViolationRecord(violRecordContract);
        }
        /// <summary>
        /// Cập nhật thông tin biên bản vi phạm
        /// </summary>
        /// <param name="violRecordContract">Thông tin biên bản vi phạm cần cập nhật lại thông tin</param>
        /// <returns></returns>
        public CUDReturnMessage UpdViolationRecord(ViolRecordContract violRecordContract)
        {
            return _violRecordBusiness.Value.UpdViolationRecord(violRecordContract);
        }
    }
}
