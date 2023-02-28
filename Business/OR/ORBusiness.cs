using Business.API;
using Contract.Enum;
using Contract.OR;
using Contract.OR.SyncData;
using Contract.Shared;
using Contract.User;
using DataAccess.DAO;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VG.Common;

namespace Business.OR
{

    public interface IORBusiness
    {
        HospitalSiteContract GetHospitalSite(string HospitalCode);
        #region Room Management
        ORRoomContract GetORRoom(int iId);
        List<ORRoomContract> GetListRoom(string HospitalCode, string kw, int sourceClientId, int roomType, string IsDisplay,bool IsSort=false);
        SearchORRoom GetListRoom(string HospitalCode, string kw, int sourceClientId, int roomType, string IsDisplay, bool IsSort = false, int p = 1, int ps = 10);
        CUDReturnMessage CUDRoom(ORRoomContract data, int userId);
        CUDReturnMessage HideOrShowRoom(int iCurrentUserId, int iId, string actionType);
        CUDReturnMessage RoomDelete(int iCurrentUserId, int iId);
        #endregion . Room Management
        List<HpServiceContract> GetListHpServices(string HospitalCode, int sourceClientId);
        List<ORUserInfoContract> GetListORUsers(string HospitalCode, int PositionId, int sourceClientId);
        List<ORUserInfoContract> GetListORUsers(string HospitalCode, List<int> lisitPositionIds, int sourceClientId, DateTime dtStart, DateTime dtEnd);
        ORAnesthProgressContract GetORAnesthProgress(string kw, int typeSearch);
        CUDReturnMessage SaveORRegistorByJson(ORAnesthProgressContract contract, int CurrentUserId);
        CUDReturnMessage SaveORManagementByJson(ORAnesthProgressContract contract, int CurrentUserId, int actionType = 1);
        CUDReturnMessage CUDOperationLink(ORLinkContract data, int userId);
        ORLinkActive CheckOperationLink(Guid GuidCode);
        //linhht theem username
        SearchORProgress FindAnesthInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, int IsDashboard, int? serviceType = -1, string username = "");
        ORMappingEkipContract GetInfoEkip(long Id);
        CUDReturnMessage SaveCUDEkip(ORMappingEkipContract contract, int CurrentUserId);
        CUDReturnMessage DeleteAnesthEkip(long Id, int userId);
        CUDReturnMessage DeleteSurgery(long Id, int userId);
        CUDReturnMessage RollbackDeleteSurgery(long Id, int userId);
        //view public
        SearchORProgress FindAnesthPublicInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, bool isShowOpeartorOrHybrid);
        CUDReturnMessage UpdateSorting(long Id, int value, int userId);
        CUDReturnMessage ChangeState(long Id, int state, int userId);
        CUDReturnMessage UpdateReceiptSurgeryInfo(int pgId, string strKey, string strValue, int userId);
        IEnumerable<HpServiceSite> SearchHpServiceInfo(int State, string kw, int p, int ps, int HpServiceId, string siteId,int sourceClientId=-1);
        CUDReturnMessage CUDHpService(HpServiceSite data, int userId);
        CUDReturnMessage InsertNewService(PatientService data, int userId);
        List<ORNotifyMail> GetListORAnesthByMail(int quantity);
        void ExecuteBlock(Action blockFunction);
        CUDReturnMessage CheckExistPositionByScheduler(long Id, int userId);
        ORUserInfoContract GetORUserInfo(int userId);
        //linhht
        ORUserInfoContract GetORUserInfo(string userName);
        PagedList<ORTrackingContract> ListORTracking(DateTime fromDate, DateTime toDate, string keyword, int page, int pageSize, string HospitalCode);
        List<HpServiceContract> SearchHpServiceExt(string siteId, string kw, int page, int pageSize, int siteType);
        HpServiceContract GetServiceById(int serviceId);
        HpServiceContract GetServiceByCode(string code);
        List<PatientService> RemoveExistingServices(List<PatientService> list, string siteId);
        bool IsOrderExisted(string orderId, string chargeId, string siteId);
        List<PatientService> GetChargesFromOH(string siteId, string pid, int currentUserId);
        //linhht
        Task<bool> UpdatePatientInfo(ReportPatient patient, int currentUserId);
    }
    public class ORBusiness : BaseBusiness, IORBusiness
    {
        private Lazy<IORDataAccess> lazyDataAccess;
        private Lazy<IORProAccess> proAccess;
        public ORBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyDataAccess = new Lazy<IORDataAccess>(() => new ORDataAccess(appid, uid));
            proAccess = new Lazy<IORProAccess>(() => new ORProAccess(appid, uid));
        }
        public HospitalSiteContract GetHospitalSite(string HospitalCode)
        {
            return lazyDataAccess.Value.GetHospitalSite(HospitalCode);
        }
        public CUDReturnMessage CUDHpService(HpServiceSite data, int userId)
        {
            return lazyDataAccess.Value.CUDHpService(data, userId);
        }

        public List<HpServiceContract> GetListHpServices(string HospitalCode, int sourceClientId)
        {
            return lazyDataAccess.Value.GetListHpServices(HospitalCode, sourceClientId);
        }

        public List<ORUserInfoContract> GetListORUsers(string HospitalCode, int PositionId, int sourceClientId)
        {
            return lazyDataAccess.Value.GetListORUsers(HospitalCode, PositionId, sourceClientId);
        }
        #region Room Management
        public ORRoomContract GetORRoom(int iId)
        {
            return lazyDataAccess.Value.GetORRoom(iId);
        }
        public List<ORRoomContract> GetListRoom(string HospitalCode,string kw, int sourceClientId,int roomType, string IsDisplay, bool IsSort=false)
        {
            var query = lazyDataAccess.Value.GetListRoom(HospitalCode,kw, sourceClientId,roomType, IsDisplay,IsSort);
            if (query != null && query.Any())
            {
                return query.Select(c => new ORRoomContract()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Id_Mapping = c.Id_Mapping,
                    TypeRoom = c.TypeRoom,
                    Name_Mapping = c.Name_Mapping

                }).ToList();
            }
            return new List<ORRoomContract>();
        }
        public SearchORRoom GetListRoom(string HospitalCode, string kw, int sourceClientId, int roomType, string IsDisplay, bool IsSort = false, int p = 1, int ps = 10)
        {
            SearchORRoom result = new SearchORRoom() { Data = new List<ORRoomContract>(), TotalRows = 0 };
            var query = lazyDataAccess.Value.GetListRoom(HospitalCode, kw, sourceClientId, roomType, IsDisplay, IsSort);
            if (query != null && query.Any())
            {
                result.TotalRows = query.Count();
                var data = query.OrderByDescending(c => c.Id).OrderBy(d => d.Sorting)
                            .Skip((p - 1) * ps)
                            .Take(ps).ToList();
                result.Data = data.ToList();
            }
            return result;
        }
        public CUDReturnMessage CUDRoom(ORRoomContract data, int userId)
        {
            return lazyDataAccess.Value.CUDRoom(data, userId);
        }
        public CUDReturnMessage HideOrShowRoom(int iCurrentUserId, int iId, string actionType)
        {
            return lazyDataAccess.Value.HideOrShowRoom(iCurrentUserId,iId, actionType);
        }
        public CUDReturnMessage RoomDelete(int iCurrentUserId, int iId)
        {
            return lazyDataAccess.Value.RoomDelete(iCurrentUserId, iId);
        }
        #endregion .Room Management
        public ORAnesthProgressContract GetORAnesthProgress(string kw, int typeSearch)
        {
            return lazyDataAccess.Value.GetORAnesthProgress(kw, typeSearch);
        }

        public CUDReturnMessage SaveORRegistorByJson(ORAnesthProgressContract contract, int CurrentUserId)
        {
            return lazyDataAccess.Value.SaveORRegistorByJson(contract, CurrentUserId);
        }
        public CUDReturnMessage SaveORManagementByJson(ORAnesthProgressContract contract, int CurrentUserId, int actionType = 1)
        {
            return lazyDataAccess.Value.SaveORManagementByJson(contract, CurrentUserId, actionType);
        }

        public CUDReturnMessage CUDOperationLink(ORLinkContract data, int userId)
        {
            return lazyDataAccess.Value.CUDOperationLink(data, userId);
        }

        public ORLinkActive CheckOperationLink(Guid GuidCode)
        {
            return lazyDataAccess.Value.CheckOperationLink(GuidCode);
        }
        //linhht them username
        public SearchORProgress FindAnesthInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, int IsDashboard, int? serviceType=-1, string username = "")
        {
            SearchORProgress result = new SearchORProgress() { Data = new List<ORAnesthProgressContract>(), TotalRows = 0 };
            var query = proAccess.Value.FindAnesthInfo(FromDate, ToDate, State, kw, p, ps, HpServiceId, ORRoomId, siteId, sourceClientId, CurrentUserId, IsDashboard, username);
            if (query != null && query.Any())
            {
                if (serviceType != -1)
                {
                    query = query.Where(x=>x.ServiceType==serviceType)?.ToList();
                }
                result.TotalRows = query.Count();
                var data = query.OrderByDescending(c => c.ORRoomId).OrderBy(d => d.State)
                            .Skip((p - 1) * ps)
                            .Take(ps).ToList();
                result.Data = data;
            }
            return result;
        }
        public ORMappingEkipContract GetInfoEkip(long Id)
        {
            return lazyDataAccess.Value.GetInfoEkip(Id);
        }
        public CUDReturnMessage SaveCUDEkip(ORMappingEkipContract contract, int CurrentUserId)
        {
            return lazyDataAccess.Value.SaveCUDEkip(contract, CurrentUserId);
        }
        public CUDReturnMessage DeleteAnesthEkip(long Id, int userId)
        {
            return lazyDataAccess.Value.DeleteAnesthEkip(Id, userId);
        }

        public CUDReturnMessage DeleteSurgery(long Id, int userId)
        {
            return lazyDataAccess.Value.DeleteSurgery(Id, userId);
        }
        public CUDReturnMessage RollbackDeleteSurgery(long Id, int userId)
        {
            return lazyDataAccess.Value.RollbackDeleteSurgery(Id, userId);
        }
        public List<ORUserInfoContract> GetListORUsers(string HospitalCode, List<int> lisitPositionIds, int sourceClientId, DateTime dtStart, DateTime dtEnd)
        {
            return lazyDataAccess.Value.GetListORUsers(HospitalCode, lisitPositionIds, sourceClientId, dtStart, dtEnd);
        }

        public SearchORProgress FindAnesthPublicInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId, int CurrentUserId, bool isShowOperatorOrHybrid = false)
        {
            SearchORProgress result = new SearchORProgress() { Data = new List<ORAnesthProgressContract>(), TotalRows = 0 };
            var query = proAccess.Value.FindAnesthPublicInfo(FromDate, ToDate, State, kw, p, ps, HpServiceId, ORRoomId, siteId, sourceClientId, CurrentUserId);
            if (query != null && query.Any())
            {
                var data = new List<ORAnesthProgressContract>();
                if (isShowOperatorOrHybrid)
                {

                    result.TotalRows = query.Where(x => x.TypeRoom== (int)RoomTypeEnum.Surgery).Count();
                    data = query.Where(x => x.TypeRoom == (int)RoomTypeEnum.Surgery).OrderBy(c=>c.ORRoomId).ThenBy(c=>c.dtEnd).ThenBy(c => c.Sorting).ThenBy(d => d.State)
                                .Skip((p - 1) * ps)
                                .Take(ps).ToList();
                }
                else
                {
                    result.TotalRows = query.Count();
                    data = query.OrderBy(c => c.ORRoomId).ThenBy(c => c.dtEnd).ThenBy(c => c.Sorting).ThenBy(d => d.State)
                                .Skip((p - 1) * ps)
                                .Take(ps).ToList();
                }
                result.Data = data;
            }
            return result;
        }

        public CUDReturnMessage UpdateSorting(long Id, int value, int userId)
        {
            return lazyDataAccess.Value.UpdateSorting(Id, value, userId);
        }

        public CUDReturnMessage ChangeState(long Id, int state, int userId)
        {
            return lazyDataAccess.Value.ChangeState(Id, state, userId);
        }
        public CUDReturnMessage UpdateReceiptSurgeryInfo(int pgId, string strKey, string strValue, int userId)
        {
            return lazyDataAccess.Value.UpdateReceiptSurgeryInfo(pgId, strKey, strValue, userId);
        }
        public IEnumerable<HpServiceSite> SearchHpServiceInfo(int State, string kw, int p, int ps, int HpServiceId, string siteId, int sourceClientId = -1)
        {
            try
            {
                return proAccess.Value.SearchHpServiceInfo(State, kw, p, ps, HpServiceId, siteId, sourceClientId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #region worker
        public List<ORNotifyMail> GetListORAnesthByMail(int quantity)
        {
            return lazyDataAccess.Value.GetListORAnesthByMail(quantity);
        }
        public void ExecuteBlock(Action blockFunction)
        {
            lazyDataAccess.Value.ExecuteBlock(blockFunction);
        }
        #endregion

        public CUDReturnMessage CheckExistPositionByScheduler(long Id, int userId)
        {
            return lazyDataAccess.Value.CheckExistPositionByScheduler(Id, userId);
        }

        public ORUserInfoContract GetORUserInfo(int userId)
        {
            return lazyDataAccess.Value.GetORUserInfo(userId);
        }
        //linhht
        public ORUserInfoContract GetORUserInfo(string userName)
        {
            return lazyDataAccess.Value.GetORUserInfo(userName);
        }

        public PagedList<ORTrackingContract> ListORTracking(DateTime fromDate, DateTime toDate, string keyword, int page, int pageSize, string HospitalCode)
        {
            var query = lazyDataAccess.Value.ListORTracking(fromDate, toDate, keyword, HospitalCode).AsQueryable();
            var count = query != null ? query.Count() : 0;
            var result = new PagedList<ORTrackingContract>(count);

            if (count > 0)
            {
                result.List = query.OrderByDescending(r => r.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).Select(r => new ORTrackingContract
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    Email = r.AdminUser.Email,
                    FullName = r.AdminUser.Name,
                    CreatedDate = r.CreatedDate,
                    ContentTracking = r.ContentTracking,
                    State = r.State,
                    ORId = r.ORId

                }).ToList();
            }

            return result;
        }

        public List<HpServiceContract> SearchHpServiceExt(string siteId, string kw, int page, int pageSize, int siteType)
        {
            return lazyDataAccess.Value.SearchHpServiceExt(siteId, kw, page, pageSize, siteType);
        }
        public HpServiceContract GetServiceById(int serviceId)
        {
            return lazyDataAccess.Value.GetServiceById(serviceId);
        }

        public CUDReturnMessage InsertNewService(PatientService data, int userId)
        {
            return lazyDataAccess.Value.InsertNewService(data, userId);
        }

        public bool IsOrderExisted(string orderId, string chargeId, string siteId)
        {
            return lazyDataAccess.Value.IsOrderExisted(orderId,chargeId, siteId);
        }

        public HpServiceContract GetServiceByCode(string code)
        {
            return lazyDataAccess.Value.GetServiceByCode(code);
        }

        public List<PatientService> RemoveExistingServices(List<PatientService> list,string siteId)
        {
            return lazyDataAccess.Value.RemoveExistingServices(list,siteId);
        }
        private ISyncGateWay _syncData;
        public List<PatientService> GetChargesFromOH(string siteId,string pid,int currentUserId)
        {
            _syncData = new SyncGateWay();
            List<PatientService> entities = null;
            BenhNhanOR data = null;
            if (!string.IsNullOrEmpty(pid))
                data = _syncData.GetBenhNhanORInfo(pid, (int)SourceClientEnum.Oh);
            #region Insert new service
            if (data != null && data.ListServices != null)
            {
                foreach (var model in data.ListServices.Services)
                {
                    var response = InsertNewService(model, currentUserId);
                }
                var newList = RemoveExistingServices(data.ListServices.Services, siteId);
                entities = newList;
            }
            #endregion
            return entities;
        }
        public async Task<bool> UpdatePatientInfo(ReportPatient patient, int currentUserId)
        {
            return await lazyDataAccess.Value.UpdatePatientInfo(patient, currentUserId);
        }
    }
}
