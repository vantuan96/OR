using Business.OR;
using Contract.OR;
using Contract.OR.SyncData;
using Contract.Shared;
using Contract.User;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caching.OR
{

    public interface IORCaching
    {
        HospitalSiteContract GetHospitalSite(string HospitalCode);
        List<ORRoomContract> GetListRoom(string HospitalCode, string kw, int sourceClientId, int roomType, string IsDisplay, bool IsSort = false);
        List<HpServiceContract> GetListHpServices(string HospitalCode, int sourceClientId);
        List<ORUserInfoContract> GetListORUsers(string HospitalCode, int PositionId, int sourceClientId);
        List<ORUserInfoContract> GetListORUsers(string HospitalCode, List<int> lisitPositionIds, int sourceClientId, DateTime dtStart, DateTime dtEnd);
        ORAnesthProgressContract GetORAnesthProgress(string kw, int typeSearch);
        CUDReturnMessage SaveORRegistorByJson(ORAnesthProgressContract contract, int CurrentUserId);
        CUDReturnMessage SaveORManagementByJson(ORAnesthProgressContract contract, int CurrentUserId, int actionType = 1);
        CUDReturnMessage CUDOperationLink(ORLinkContract data, int userId);
        ORLinkActive CheckOperationLink(Guid GuidCode);
        //linhht them username
        SearchORProgress FindAnesthInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, int IsDashboard, string username = "", int? serviceType = -1);
        ORMappingEkipContract GetInfoEkip(long Id);
        CUDReturnMessage SaveCUDEkip(ORMappingEkipContract contract, int CurrentUserId);
        CUDReturnMessage DeleteAnesthEkip(long Id, int userId);
        //view public
        SearchORProgress FindAnesthPublicInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, bool isShowOperatorOrHybrid);

        CUDReturnMessage UpdateSorting(long Id, int value, int userId);
        CUDReturnMessage ChangeState(long Id, int state, int userId);
        CUDReturnMessage UpdateReceiptSurgeryInfo(int pgId, string strKey, string strValue, int userId);
        IEnumerable<HpServiceSite> SearchHpServiceInfo(int State, string kw, int p, int ps, int HpServiceId, string siteId, int sourceClientId = -1);

        CUDReturnMessage CUDHpService(HpServiceSite data, int userId);
        CUDReturnMessage InsertNewService(PatientService data, int userId);
        #region worker
        List<ORNotifyMail> GetListORAnesthByMail(int quantity);
        void ExecuteBlock(Action blockFunction);
        #endregion
        CUDReturnMessage CheckExistPositionByScheduler(long Id, int userId);
        ORUserInfoContract GetORUserInfo(int userId);
        PagedList<ORTrackingContract> ListORTracking(DateTime fromDate, DateTime toDate, string keyword, int page, int pageSize, string HospitalCode);
        List<PatientService> RemoveExistingServices(List<PatientService> list, string siteId);
        bool IsOrderExisted(string orderId, string chargeId, string siteId);
        List<PatientService> GetChargesFromOH(string siteId, string pid, int currentUserId);
        //linhht
        Task<bool> UpdatePatientInfo(ReportPatient patient, int currentUserId);
    }
    public class ORCaching : BaseCaching, IORCaching
    {
        private const string CachingTypeName = "ORCachingCaching";
        private readonly Lazy<IORBusiness> lazyBusiness;
        public ORCaching()

        {
            lazyBusiness = new Lazy<IORBusiness>(() => new ORBusiness(appid, uid));
        }
        public CUDReturnMessage CUDHpService(HpServiceSite data, int userId)
        {
            return lazyBusiness.Value.CUDHpService(data, userId);
        }
        public HospitalSiteContract GetHospitalSite(string HospitalCode)
        {
            return lazyBusiness.Value.GetHospitalSite(HospitalCode);
        }

        public List<HpServiceContract> GetListHpServices(string HospitalCode, int sourceClientId)
        {
            return lazyBusiness.Value.GetListHpServices(HospitalCode, sourceClientId);
        }

        public List<ORUserInfoContract> GetListORUsers(string HospitalCode, int PositionId, int sourceClientId)
        {
            return lazyBusiness.Value.GetListORUsers(HospitalCode, PositionId, sourceClientId);
        }
        #region Room Management
        public ORRoomContract GetORRoom(int iId)
        {
            return lazyBusiness.Value.GetORRoom(iId);
        }
        public List<ORRoomContract> GetListRoom(string HospitalCode, string kw, int sourceClientId, int roomType, string IsDisplay, bool IsSort = false)
        {
            return lazyBusiness.Value.GetListRoom(HospitalCode, kw, sourceClientId, roomType, IsDisplay, IsSort);
        }
        public SearchORRoom SearchORRoom(string HospitalCode, string kw, int sourceClientId, int roomType, string IsDisplay, bool IsSort = false, int p = 1, int ps = 10)
        {
            return lazyBusiness.Value.GetListRoom(HospitalCode, kw, sourceClientId, roomType, IsDisplay, IsSort, p, ps);
        }
        public CUDReturnMessage CUDRoom(ORRoomContract data, int userId)
        {
            return lazyBusiness.Value.CUDRoom(data, userId);
        }
        public CUDReturnMessage HideOrShowRoom(int iCurrentUserId, int iId, string actionType)
        {
            return lazyBusiness.Value.HideOrShowRoom(iCurrentUserId, iId, actionType);
        }
        public CUDReturnMessage RoomDelete(int iCurrentUserId, int iId)
        {
            return lazyBusiness.Value.RoomDelete(iCurrentUserId, iId);
        }
        #endregion .Room Management
        public ORAnesthProgressContract GetORAnesthProgress(string kw, int typeSearch)
        {
            return lazyBusiness.Value.GetORAnesthProgress(kw, typeSearch);
        }

        public CUDReturnMessage SaveORRegistorByJson(ORAnesthProgressContract contract, int CurrentUserId)
        {
            return lazyBusiness.Value.SaveORRegistorByJson(contract, CurrentUserId);
        }
        public CUDReturnMessage SaveORManagementByJson(ORAnesthProgressContract contract, int CurrentUserId, int actionType = 1)
        {
            return lazyBusiness.Value.SaveORManagementByJson(contract, CurrentUserId, actionType);
        }

        public CUDReturnMessage CUDOperationLink(ORLinkContract data, int userId)
        {
            return lazyBusiness.Value.CUDOperationLink(data, userId);
        }

        public ORLinkActive CheckOperationLink(Guid GuidCode)
        {
            return lazyBusiness.Value.CheckOperationLink(GuidCode);
        }
        //linht theem username
        public SearchORProgress FindAnesthInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, int IsDashboard, string username = "", int? serviceType = -1)
        {
            return lazyBusiness.Value.FindAnesthInfo(FromDate, ToDate, State, kw, p, ps, HpServiceId, ORRoomId, siteId, sourceClientId, CurrentUserId, IsDashboard, serviceType, username);
        }
        public ORMappingEkipContract GetInfoEkip(long Id)
        {
            return lazyBusiness.Value.GetInfoEkip(Id);
        }

        public CUDReturnMessage SaveCUDEkip(ORMappingEkipContract contract, int CurrentUserId)
        {
            return lazyBusiness.Value.SaveCUDEkip(contract, CurrentUserId);
        }

        public CUDReturnMessage DeleteAnesthEkip(long Id, int userId)
        {
            return lazyBusiness.Value.DeleteAnesthEkip(Id, userId);
        }

        public CUDReturnMessage DeleteSurgery(long Id, int userId)
        {
            return lazyBusiness.Value.DeleteSurgery(Id, userId);
        }
        public CUDReturnMessage RollbackDeleteSurgery(long Id, int userId)
        {
            return lazyBusiness.Value.RollbackDeleteSurgery(Id, userId);
        }
        public List<ORUserInfoContract> GetListORUsers(string HospitalCode, List<int> lisitPositionIds, int sourceClientId, DateTime dtStart, DateTime dtEnd)
        {
            return lazyBusiness.Value.GetListORUsers(HospitalCode, lisitPositionIds, sourceClientId, dtStart, dtEnd);
        }

        public SearchORProgress FindAnesthPublicInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, bool isShowOperatorOrHybrid)
        {
            return lazyBusiness.Value.FindAnesthPublicInfo(FromDate, ToDate, State, kw, p, ps, HpServiceId, ORRoomId, siteId, sourceClientId, CurrentUserId, isShowOperatorOrHybrid);
        }

        public CUDReturnMessage UpdateSorting(long Id, int value, int userId)
        {
            return lazyBusiness.Value.UpdateSorting(Id, value, userId);
        }

        public CUDReturnMessage ChangeState(long Id, int state, int userId)
        {
            return lazyBusiness.Value.ChangeState(Id, state, userId);
        }
        public CUDReturnMessage UpdateReceiptSurgeryInfo(int pgId, string strKey, string strValue, int userId)
        {
            return lazyBusiness.Value.UpdateReceiptSurgeryInfo(pgId, strKey, strValue, userId);
        }
        public IEnumerable<HpServiceSite> SearchHpServiceInfo(int State, string kw, int p, int ps, int HpServiceId, string siteId, int sourceClientId = -1)
        {
            return lazyBusiness.Value.SearchHpServiceInfo(State, kw, p, ps, HpServiceId, siteId, sourceClientId);
        }
        public HpServiceContract GetServiceById(int serviceId)
        {
            return lazyBusiness.Value.GetServiceById(serviceId);
        }
        public HpServiceContract GetServiceByCode(string code)
        {
            return lazyBusiness.Value.GetServiceByCode(code);
        }
        #region worker
        public List<ORNotifyMail> GetListORAnesthByMail(int quantity)
        {
            return lazyBusiness.Value.GetListORAnesthByMail(quantity);
        }
        public void ExecuteBlock(Action blockFunction)
        {
            lazyBusiness.Value.ExecuteBlock(blockFunction);
        }
        #endregion
        public CUDReturnMessage CheckExistPositionByScheduler(long Id, int userId)
        {
            return lazyBusiness.Value.CheckExistPositionByScheduler(Id, userId);
        }

        public ORUserInfoContract GetORUserInfo(int userId)
        {
            return lazyBusiness.Value.GetORUserInfo(userId);
        }
        //linhht
        public ORUserInfoContract GetORUserInfo(string userName)
        {
            return lazyBusiness.Value.GetORUserInfo(userName);
        }
        public PagedList<ORTrackingContract> ListORTracking(DateTime fromDate, DateTime toDate, string keyword, int page, int pageSize, string HospitalCode)
        {
            return lazyBusiness.Value.ListORTracking(fromDate, toDate, keyword, page, pageSize, HospitalCode);
        }

        public IEnumerable<HpServiceContract> SearchHpServiceExt(string siteId, string kw, int page, int pageSize, int siteType)
        {
            return lazyBusiness.Value.SearchHpServiceExt(siteId, kw, page, pageSize, siteType);
        }

        public CUDReturnMessage InsertNewService(PatientService data, int userId)
        {
            return lazyBusiness.Value.InsertNewService(data, userId);
        }

        public bool IsOrderExisted(string orderId, string chargeId, string siteId)
        {
            return lazyBusiness.Value.IsOrderExisted(orderId, chargeId, siteId);
        }

        public List<PatientService> RemoveExistingServices(List<PatientService> list, string siteId)
        {
            return lazyBusiness.Value.RemoveExistingServices(list, siteId);
        }

        public List<PatientService> GetChargesFromOH(string siteId, string pid, int currentUserId)
        {
            return lazyBusiness.Value.GetChargesFromOH(siteId, pid, currentUserId);
        }

        public async Task<bool> UpdatePatientInfo(ReportPatient patient, int currentUserId)
        {
            return await lazyBusiness.Value.UpdatePatientInfo(patient, currentUserId);
        }
    }
}
