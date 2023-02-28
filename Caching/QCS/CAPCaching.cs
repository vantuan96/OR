using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Business;
using BMS.Business.QCS;
using BMS.Contract.Qcs.CheckList;
using BMS.Contract.Qcs.ZoneViolations;
using BMS.Contract.Shared;

namespace BMS.Caching.QCS
{
    public interface ICAPCaching
    {
        /// <summary>
        ///  Add/Update thong tin vi pham cho cac khu vuc duoc danh gia
        /// </summary>
        /// <param name="CAPInfo"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateViolationToMultiZones(CAPViolationParam CAPInfo);
        /// <summary>
        /// Lay danh sach vi pham cho theo dieu kien filter
        /// </summary>
        /// <param name="siteId">Id trung tam thuong mai</param>
        /// <param name="evaluationCalendarId">Id chuong trinh ky kiem tra</param>
        /// <param name="groupCateId">Id:nhom hang muc</param>
        /// <param name="cateId">Id hang muc</param>
        /// <param name="keyWord">ten loi vi pham can tim kiem</param>
        /// <param name="page">trang can lay</param>
        /// <param name="pageSize">so record cua trang can lay</param>
        /// <param name="sourceClientId">Nguon lay data tu trang nao</param>
        /// <returns>ds ket qua</returns>
        SearchListCAPContract FindZoneViolations(int siteId, int evaluationCalendarId, int groupCateId, int cateId, int violationId, string keyWord, int page, int pageSize, int sourceClientId, DateTime startDate, DateTime endDate ,Boolean IsSearchDate);
        /// <summary>
        /// Xoa 1 bao cao vi pham cho khu vuc
        /// </summary>
        /// <param name="ViolationId"></param>
        /// <returns></returns>
        CUDReturnMessage DeleteViolation(int violationId);
        /// <summary>
        /// Lay thong tin 1 bao cao vi pham
        /// </summary>
        /// <param name="ViolationId"></param>
        /// <returns></returns>
        SiteZoneViolationContract GetViolation(int ViolationId);
        /// <summary>
        /// Ghi nhan thong tin loi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage UpdateZoneViolationReport(SiteZoneViolationContract model);
    }
    public class CAPCaching : BaseCaching, ICAPCaching
    {
        private Lazy<ICAPBusiness> capBusiness;
        public CAPCaching(/*string appid, int uid*/)  
        {
            capBusiness = new Lazy<ICAPBusiness>(() =>
            {
                var instance = new CAPBusiness(appid, uid);
                return instance;
            });
        }
        /// <summary>
        ///  Add/update thong tin vi pham cho cac khu vuc duoc danh gia
        /// </summary>
        /// <param name="CAPInfo"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateViolationToMultiZones(CAPViolationParam CAPInfo)
        {
            return capBusiness.Value.InsertUpdateViolationToMultiZones(CAPInfo);
        }
        /// <summary>
        /// Lay danh sach vi pham cho theo dieu kien filter
        /// </summary>
        /// <param name="siteId">Id trung tam thuong mai</param>
        /// <param name="evaluationCalendarId">Id chuong trinh ky kiem tra</param>
        /// <param name="groupCateId">Id:nhom hang muc</param>
        /// <param name="cateId">Id hang muc</param>
        /// <param name="keyWord">ten loi vi pham can tim kiem</param>
        /// <param name="page">trang can lay</param>
        /// <param name="pageSize">so record cua trang can lay</param>
        /// <param name="sourceClientId">Nguon lay data tu trang nao</param>
        /// <returns>ds ket qua</returns>
        public SearchListCAPContract FindZoneViolations(int siteId, int evaluationCalendarId, int groupCateId, int cateId, int violationId, string keyWord, int page, int pageSize, int sourceClientId, DateTime startDate, DateTime endDate, Boolean IsSearchDate)
        {
            return capBusiness.Value.FindZoneViolations(siteId, evaluationCalendarId, groupCateId, cateId, violationId, keyWord, page,
                pageSize, sourceClientId, startDate, endDate,IsSearchDate);
        }
        /// <summary>
        /// Xoa 1 loi vi pham cua bao cao
        /// </summary>
        /// <param name="violationId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteViolation(int violationId)
        {
            return capBusiness.Value.DeleteViolation(violationId);
        }

        public SiteZoneViolationContract GetViolation(int ViolationId)
        {
            return capBusiness.Value.GetViolation(ViolationId);
        }

        public CUDReturnMessage UpdateZoneViolationReport(SiteZoneViolationContract model)
        {
            return capBusiness.Value.UpdateZoneViolationReport(model);
        }
    }
}
