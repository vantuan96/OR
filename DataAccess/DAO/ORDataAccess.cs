using Contract.OR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Contract.Enum;
using Contract.Shared;
using VG.Common;
using DataAccess.Shared;
using System.Data.Entity;
using System.Threading;
using Contract.User;
using System.Configuration;
using Contract.OR.SyncData;

namespace DataAccess.DAO
{
    public interface IORDataAccess
    {
        HospitalSiteContract GetHospitalSite(string HospitalCode);
        #region Room Management
        ORRoomContract GetORRoom(int iId);
        IEnumerable<ORRoomContract> GetListRoom(string HospitalCode, string kw, int sourceClientId, int roomType, string IsDisplay, bool IsSort = false);
        CUDReturnMessage CUDRoom(ORRoomContract data, int userId);
        CUDReturnMessage HideOrShowRoom(int iCurrentUserId, int iId, string actionType);
        CUDReturnMessage RoomDelete(int iCurrentUserId, int iId);
        #endregion .Room Management
        List<HpServiceContract> GetListHpServices(string HospitalCode, int sourceClientId);
        List<ORUserInfoContract> GetListORUsers(string HospitalCode, int PositionId, int sourceClientId);
        List<ORUserInfoContract> GetListORUsers(string HospitalCode, List<int> lisitPositionIds, int sourceClientId, DateTime dtStart, DateTime dtEnd);
        ORAnesthProgressContract GetORAnesthProgress(string kw, int typeSearch);
        CUDReturnMessage SaveORRegistorByJson(ORAnesthProgressContract contract, int CurrentUserId);
        CUDReturnMessage SaveORManagementByJson(ORAnesthProgressContract contract, int CurrentUserId, int actionType = 1);
        CUDReturnMessage CUDOperationLink(ORLinkContract data, int userId);
        ORLinkActive CheckOperationLink(Guid GuidCode);
        IQueryable<ORAnesthProgress> SearchAnesthInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId);
        ORMappingEkipContract GetInfoEkip(long Id);
        CUDReturnMessage SaveCUDEkip(ORMappingEkipContract contract, int CurrentUserId);
        CUDReturnMessage DeleteAnesthEkip(long Id, int userId);
        CUDReturnMessage DeleteSurgery(long Id, int userId);
        CUDReturnMessage RollbackDeleteSurgery(long Id, int userId);
        CUDReturnMessage UpdateSorting(long Id, int value, int userId);
        CUDReturnMessage ChangeState(long Id, int state, int userId);
        CUDReturnMessage UpdateReceiptSurgeryInfo(int pgId, string strKey, string strValue, int userId);
        CUDReturnMessage CUDHpService(HpServiceSite data, int userId);
        CUDReturnMessage InsertNewService(PatientService data, int userId);
        List<ORNotifyMail> GetListORAnesthByMail(int quantity);
        void ExecuteBlock(Action blockFunction);
        CUDReturnMessage CheckExistPositionByScheduler(long Id, int userId);
        ORUserInfoContract GetORUserInfo(int userId);
        //linhht
        ORUserInfoContract GetORUserInfo(string userName);
        IQueryable<ORTracking> ListORTracking(DateTime fromDate, DateTime toDate, string keyword, string HospitalCode);
        void InsertUserTracking(string log, int userId, int ORId, int state, int type);

        List<HpServiceContract> SearchHpServiceExt(string siteId, string kw, int page, int pageSize, int siteType);
        HpServiceContract GetServiceById(int serviceId);
        bool IsOrderExisted(string orderId, string chargeId, string siteId);
        List<PatientService> RemoveExistingServices(List<PatientService> list, string siteId);
        HpServiceContract GetServiceByCode(string code);
        //linhht
        Task<bool> UpdatePatientInfo(ReportPatient patient, int userId);
    }

    public class ORDataAccess : BaseDataAccess, IORDataAccess
    {
        public ORDataAccess(string appid, int uid) : base(appid, uid)
        {
        }
        public HospitalSiteContract GetHospitalSite(string HospitalCode)
        {
            var site = DbContext.HospitalSites.FirstOrDefault(r => (r.Id.Equals(HospitalCode) && r.IsDeleted == false));
            if (site == null)
                return null;
            return new HospitalSiteContract()
            {
                Id = site.Id,
                SiteName = site.SiteName,
                SiteNameFull = site.SiteNameFull,
                Description = site.Description,
                Address = site.Address,
                Phone = site.Phone,
                AreaId = site.AreaId,
                IsDeleted = site.IsDeleted

            };
        }
        #region Room Management
        public ORRoomContract GetORRoom(int iId)
        {
            var query = DbContext.ORRooms.Where(x => x.Id == iId);
            if (query != null && query.Any())
            {
                var listEntity = (from d in query
                                  join cr in DbContext.AdminUsers on d.CreatedBy equals cr.UId into crx
                                  from crxn in crx.DefaultIfEmpty()
                                  join up in DbContext.AdminUsers on d.UpdatedBy equals up.UId into upx
                                  from upxn in upx.DefaultIfEmpty()
                                  select new ORRoomContract
                                  {
                                      Id = d.Id,
                                      Name = d.Name,
                                      Description = d.Description,
                                      IsDeleted = d.IsDeleted,
                                      Sorting = d.Sorting,
                                      CreatedBy = d.CreatedBy,
                                      CreatedByName = crxn.Name,
                                      CreatedDate = d.CreatedDate != null ? d.CreatedDate.Value : DateTime.MinValue,
                                      UpdatedBy = d.UpdatedBy != null ? d.UpdatedBy.Value : 0,
                                      UpdatedByName = upxn.Name,
                                      UpdatedDate = d.UpdatedDate != null ? d.UpdatedDate.Value : DateTime.MinValue,
                                      Id_Mapping = d.Id_Mapping,
                                      Name_Mapping = d.Name_Mapping,
                                      IsDisplay = d.IsDisplay,
                                      HospitalCode = d.HospitalCode,
                                      SourceClientId = d.SourceClientId,
                                      TypeRoom = d.TypeRoom != null ? d.TypeRoom.Value : 4
                                  });
                return listEntity.SingleOrDefault();
            }
            return null;


        }
        public IEnumerable<ORRoomContract> GetListRoom(string HospitalCode, string kw, int sourceClientId, int roomType, string IsDisplay, bool IsSort = false)
        {
            var query = DbContext.ORRooms.Where(x => x.IsDeleted != true);
            if (!string.IsNullOrEmpty(HospitalCode))
                query = query.Where(c => c.HospitalCode.Equals(HospitalCode));
            if (!string.IsNullOrEmpty(kw))
            {
                query = query.Where(c => c.Name.Contains(kw));
            }
            if (sourceClientId != 0)
                query = query.Where(c => c.SourceClientId.Equals(sourceClientId));
            if (roomType != -1)
                query = query.Where(c => c.TypeRoom == roomType);
            if (!string.IsNullOrEmpty(IsDisplay) && !IsDisplay.Equals("-1"))
                query = query.Where(c => c.IsDisplay == IsDisplay);
            var listEntity = (from d in query
                              join cr in DbContext.AdminUsers on d.CreatedBy equals cr.UId into crx
                              from crxn in crx.DefaultIfEmpty()
                              join up in DbContext.AdminUsers on d.UpdatedBy equals up.UId into upx
                              from upxn in upx.DefaultIfEmpty()
                              select new ORRoomContract
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  Description = d.Description,
                                  IsDeleted = d.IsDeleted,
                                  Sorting = d.Sorting,
                                  CreatedBy = d.CreatedBy,
                                  CreatedByName = crxn.Name,
                                  CreatedDate = d.CreatedDate != null ? d.CreatedDate.Value : DateTime.MinValue,
                                  UpdatedBy = d.UpdatedBy != null ? d.UpdatedBy.Value : 0,
                                  UpdatedByName = upxn.Name,
                                  UpdatedDate = d.UpdatedDate != null ? d.UpdatedDate.Value : DateTime.MinValue,
                                  Id_Mapping = d.Id_Mapping,
                                  Name_Mapping = d.Name_Mapping,
                                  IsDisplay = d.IsDisplay,
                                  HospitalCode = d.HospitalCode,
                                  SourceClientId = d.SourceClientId,
                                  TypeRoom = d.TypeRoom != null ? d.TypeRoom.Value : 4
                              });
            if (IsSort)
                listEntity = listEntity.OrderBy(x => x.Sorting);
            return listEntity;
        }
        public CUDReturnMessage CUDRoom(ORRoomContract data, int userId)
        {
            try
            {
                Models.ORRoom attr;
                if (data == null)
                {
                    return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
                }

                attr = DbContext.ORRooms.FirstOrDefault(d => d.Id.Equals(data.Id));
                //if (data.Id == 0 || attr == null) //Innsert
                if (attr == null) //Innsert
                {
                    Models.ORRoom room = new Models.ORRoom
                    {
                        Name = data.Name
                        ,
                        HospitalCode = data.HospitalCode
                        ,
                        IsDisplay = data.IsDisplay
                        ,
                        Sorting = data.Sorting
                       ,
                        CreatedBy = userId
                       ,
                        CreatedDate = DateTime.Now
                       ,
                        UpdatedBy = userId
                       ,
                        UpdatedDate = DateTime.Now
                       ,
                        SourceClientId = data.SourceClientId
                       ,
                        TypeRoom = data.TypeRoom
                       ,
                        Description = data.Description
                       ,
                        Name_Mapping = data.Name_Mapping
                       ,
                        Id_Mapping = data.Id_Mapping ?? "0"
                       ,
                        IsDeleted = false
                    };
                    DbContext.ORRooms.Add(room);
                    DbContext.SaveChanges();
                    return new CUDReturnMessage() { Id = (int)room.Id, Message = "lưu thông tin hành công" };
                }
                else
                {
                    attr.Name = data.Name;
                    attr.HospitalCode = data.HospitalCode;
                    attr.IsDisplay = data.IsDisplay;
                    attr.Sorting = data.Sorting;
                    attr.UpdatedBy = userId;
                    attr.UpdatedDate = DateTime.Now;
                    attr.SourceClientId = data.SourceClientId;
                    attr.TypeRoom = data.TypeRoom;
                    attr.Description = data.Description;
                    attr.Name_Mapping = data.Name_Mapping;
                    attr.Id_Mapping = data.Id_Mapping ?? "0";
                    attr.IsDeleted = false;
                    DbContext.SaveChanges();
                    return new CUDReturnMessage() { Id = (int)attr.Id, Message = "lưu thông tin hành công" };
                }
            }
            catch (Exception ex)
            {
                return new CUDReturnMessage() { Id = 0, Message = "Có lỗi trong quá trình xử lý" };
            }

        }
        public CUDReturnMessage HideOrShowRoom(int iCurrentUserId, int iId, string actionType)
        {
            CUDReturnMessage ret = null;
            var entity = DbContext.ORRooms.Find(iId);
            switch (actionType)
            {
                case "ACT-HIDE":
                    entity.IsDisplay = "0";
                    ret = new CUDReturnMessage(ResponseCode.OR_Room_SuccessHide);
                    break;
                case "ACT-SHOW":
                    entity.IsDisplay = "1";
                    ret = new CUDReturnMessage(ResponseCode.OR_Room_SuccessShow);
                    break;
            }
            entity.UpdatedBy = iCurrentUserId;
            entity.UpdatedDate = DateTime.Now;

            DbContext.SaveChanges();

            return ret;
        }
        public CUDReturnMessage RoomDelete(int iCurrentUserId, int iId)
        {
            var entity = DbContext.ORRooms.Find(iId);
            entity.IsDeleted = true;
            entity.UpdatedBy = iCurrentUserId;
            entity.UpdatedDate = DateTime.Now;

            DbContext.SaveChanges();

            return new CUDReturnMessage(ResponseCode.OR_Room_SuccessDelete);
        }
        #endregion .Room Management
        public List<HpServiceContract> GetListHpServices(string HospitalCode, int sourceClientId)
        {
            List<HpServiceContract> lstService = new List<HpServiceContract>();
            var listPublicActions = (from s in DbContext.HpServices
                                     join ms in DbContext.ORMappingServices on s.Id equals ms.ObjectId
                                     where s.IsDeleted == false
                                     && ms.IsDeleted == false
                                     && ms.HospitalCode.Equals(HospitalCode)
                                     && (sourceClientId == 0 || s.SourceClientId == sourceClientId)
                                     && ms.TypeMappingId == (int)TypeMappingServiceEnum.ORService
                                     select new HpServiceContract
                                     {
                                         Id = s.Id,
                                         Oh_Code = s.Oh_Code,
                                         Name = s.Name,
                                         CleaningTime = s.CleaningTime ?? 0,
                                         PreparationTime = s.PreparationTime ?? 0,
                                         AnesthesiaTime = s.AnesthesiaTime ?? 0,
                                         OtherTime = s.OtherTime ?? 0,
                                         Description = s.Description,
                                         IdMapping = s.IdMapping,
                                         Sort = s.Sort ?? 100,
                                         Type = s.Type ?? 2
                                     }).OrderBy(x => x.Name).ToList();
            return listPublicActions;
        }

        public List<ORUserInfoContract> GetListORUsers(string HospitalCode, int PositionId, int sourceClientId)
        {
            List<ORUserInfoContract> lstUsers = new List<ORUserInfoContract>();
            var listPublicActions = (from oru in DbContext.ORUserInfoes
                                     join u in DbContext.AdminUsers on oru.Id equals u.UId
                                     join ul in DbContext.AdminUser_Location on u.UId equals ul.UId
                                     join l in DbContext.Locations on ul.LocationId equals l.LocationId
                                     where oru.IsDeleted == false
                                     //&& oru.PositionId == PositionId
                                     && u.IsDeleted == false
                                     && ul.IsDeleted == false
                                     && l.IsDeleted == false
                                     && l.NameEN.Equals(HospitalCode)
                                     select new ORUserInfoContract
                                     {
                                         Id = oru.Id,
                                         Name = oru.Name,
                                         Email = oru.Email,
                                         //PositionId = oru.PositionId,

                                         Phone = oru.Phone,
                                     }).ToList();

            listPublicActions.ForEach(c => c.PositionName = EnumExtension.GetDescription((ORPositionEnum)c.PositionId));
            return listPublicActions;
        }

        public List<ORUserInfoContract> GetListORUsers(string HospitalCode, List<int> lisitPositionIds, int sourceClientId, DateTime dtStart, DateTime dtEnd)
        {
            List<ORUserInfoContract> lstUsers = new List<ORUserInfoContract>();
            var listPublicActions = (from oru in DbContext.ORUserInfoes
                                     join u in DbContext.AdminUsers on oru.Id equals u.UId
                                     join ul in DbContext.AdminUser_Location on u.UId equals ul.UId
                                     join l in DbContext.Locations on ul.LocationId equals l.LocationId
                                     join uip in DbContext.ORUserInfor_Position on oru.Id equals uip.UserInforId
                                     where oru.IsDeleted == false
                                     && lisitPositionIds.Contains(uip.PositionId)
                                     && u.IsDeleted == false
                                     && ul.IsDeleted == false
                                     && l.IsDeleted == false
                                     && !uip.IsDeleted
                                     && l.NameEN.Equals(HospitalCode)
                                     select new ORUserInfoContract
                                     {
                                         Id = oru.Id,
                                         Name = oru.Name,
                                         Email = oru.Email,
                                         PositionId = uip.PositionId,
                                         Phone = oru.Phone,
                                         //linhht
                                         Username = u.Username
                                     }).ToList();
            //var getAllAnesthProgress = DbContext.ORAnesthProgresses.Where(x => x.dtStart >= dtStart && x.dtEnd <= dtEnd).ToList();
            //foreach (var item in getAllAnesthProgress)
            //{
            //    listPublicActions.RemoveAll(x => x.Id == item.UIdAnesthNurse1 || x.Id == item.UIdAnesthNurse2 || x.Id == item.UIdCECDoctor ||
            //                                x.Id == item.UIdMainAnesthDoctor || x.Id == item.UIdNurseOutRun1 || x.Id == item.UIdNurseOutRun2 ||
            //                                x.Id == item.UIdNurseTool1 || x.Id == item.UIdNurseTool2 || x.Id == item.UIdPTVMain || x.Id == item.UIdPTVSub1 ||
            //                                x.Id == item.UIdPTVSub2 || x.Id == item.UIdPTVSub3 || x.Id == item.UIdSubAnesthDoctor);
            //}

            listPublicActions.ForEach(c => c.PositionName = EnumExtension.GetDescription((ORPositionEnum)c.PositionId));
            return listPublicActions;
        }



        public ORAnesthProgressContract GetORAnesthProgress(string kw, int typeSearch)
        {
            ORAnesthProgress r;
            long Id = 0;
            long.TryParse(kw, out Id);
            switch (typeSearch)
            {
                case (int)TypeSearchEnum.Id:
                    r = DbContext.ORAnesthProgresses.FirstOrDefault(c => c.Id == (Id));
                    break;
                default:
                    r = null;
                    break;
            }
            if (r != null)
            {
                var objPTVMain = new AdminUser();
                var objORUserInfo = new ORUserInfo();
                if (r.UIdPTVMain != null && r.UIdPTVMain != 0)
                {
                    objPTVMain = DbContext.AdminUsers.Find(r.UIdPTVMain);
                    objORUserInfo = objPTVMain != null ? DbContext.ORUserInfoes.FirstOrDefault(x => x.Email.Equals(objPTVMain.Email)) : null;
                }
                var data = new ORAnesthProgressContract()
                {
                    #region info data
                    Id = r.Id,
                    PId = r.PId,

                    VisitCode = r.VisitCode,
                    HospitalCode = r.HospitalCode,
                    HospitalName = r.HospitalName,
                    ORRoomId = r.ORRoomId,
                    SurgeryType = r.SurgeryType != null ? r.SurgeryType.Value : 0,
                    dtStart = r.dtStart,
                    dtEnd = r.dtEnd,
                    dtOperation = r.dtOperation,
                    TimeAnesth = r.TimeAnesth,
                    dtAdmission = r.dtAdmission,
                    OrderID = r.OrderID,
                    ChargeDetailId = r.ChargeDetailId,
                    //vutv7
                    ChargeDate = r.ChargeDate,
                    ChargeBy = r.ChargeBy,
                    AdmissionWard = r.AdmissionWard,
                    RegDescription = r.RegDescription,
                    DepartmentCode = r.DepartmentCode,
                    Diagnosis = r.Diagnosis,
                    State = r.State,

                    UIdPTVMain = r.UIdPTVMain,
                    UIdPTVSub1 = r.UIdPTVSub1,
                    UIdPTVSub2 = r.UIdPTVSub2,
                    UIdPTVSub3 = r.UIdPTVSub3,
                    UIdPTVSub4 = r.UIdPTVSub4,
                    UIdPTVSub5 = r.UIdPTVSub5,
                    UIdPTVSub6 = r.UIdPTVSub6,
                    UIdPTVSub7 = r.UIdPTVSub7,
                    UIdPTVSub8 = r.UIdPTVSub8,
                    SurgeryDescription = r.SurgeryDescription,
                    UIdCECDoctor = r.UIdCECDoctor,
                    UIdNurseTool1 = r.UIdNurseTool1,

                    UIdNurseTool2 = r.UIdNurseTool2,
                    UIdNurseOutRun1 = r.UIdNurseOutRun1,
                    UIdNurseOutRun2 = r.UIdNurseOutRun2,
                    UIdNurseOutRun3 = r.UIdNurseOutRun3,
                    UIdNurseOutRun4 = r.UIdNurseOutRun4,
                    UIdNurseOutRun5 = r.UIdNurseOutRun5,
                    UIdNurseOutRun6 = r.UIdNurseOutRun6,
                    //vutv7
                    UIdKTVSubSurgery = r.UIdKTVSubSurgery,
                    UIdKTVDiagnose = r.UIdKTVDiagnose,
                    UIdKTVCEC = r.UIdKTVCEC,
                    UIdDoctorDiagnose = r.UIdDoctorDiagnose,
                    UIdDoctorNewBorn = r.UIdDoctorNewBorn,
                    UIdMidwives = r.UIdMidwives,
                    UIdSubAnesthDoctor2 = r.UIdSubAnesthDoctor2,
                    UIdAnesthesiologist = r.UIdAnesthesiologist,

                    UIdMainAnesthDoctor = r.UIdMainAnesthDoctor,
                    UIdSubAnesthDoctor = r.UIdSubAnesthDoctor,
                    UIdAnesthNurse1 = r.UIdAnesthNurse1,
                    UIdAnesthNurse2 = r.UIdAnesthNurse2,
                    UIdAnesthNurseRecovery = r.UIdAnesthNurseRecovery,

                    NameCreatedBy = objPTVMain?.Name,
                    EmailCreatedBy = objPTVMain?.Email,
                    //PositionCreatedBy = (objORUserInfo != null && objORUserInfo.PositionId != 0) ? DbContext.ORPositionTypes.Find(objORUserInfo.PositionId).Name : string.Empty,
                    PhoneCreatedBy = objPTVMain.PhoneNumber,


                    AnesthDescription = r.AnesthDescription,
                    NameProject = r.NameProject,
                    HpServiceId = r.HpServiceId,
                    CreatedBy = r.CreatedBy ?? 0,
                    listUsers = r.ORMappingEkips.Where(d => d.IsDeleted != true).Select(c => new ORMappingEkipContract()
                    {
                        Id = c.Id,
                        HospitalCode = c.HospitalCode,
                        UId = c.UId,
                        UserName = (c.ORUserInfo != null ? c.ORUserInfo.Name : string.Empty),
                        Email = (c.ORUserInfo.Email != null ? c.ORUserInfo.Email : string.Empty),
                        Phone = (c.ORUserInfo.Phone != null ? c.ORUserInfo.Phone : string.Empty),
                        //PositionId = (c.ORUserInfo.PositionId > 0 ? c.ORUserInfo.PositionId : 0),
                        //PositionName = (c.ORUserInfo.PositionId > 0 ? c.ORUserInfo.ORPositionType.Name : string.Empty),
                        TypePageId = c.TypePageId
                    }).OrderBy(c => c.PositionId).ToList()

                    #endregion
                };
                #region patient

                var p = DbContext.ReportPatients.FirstOrDefault(d => d.Id.Equals(data.PId));
                if (p != null)
                {
                    data.HoTen = p.HoTen;
                    data.NgaySinh = p.NgaySinh;
                    data.Address = p.Address;
                    data.Sex = p.Sex ?? 2;
                    data.PatientPhone = p.Phone;
                    data.Email = p.Email;
                    data.Ages = p.Age;
                }
                #endregion
                #region mapping user data

                if (data.UIdPTVMain != null && data.UIdPTVMain > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdPTVMain);
                    if (ORUserInfo != null)
                    {
                        data.NamePTVMain = ORUserInfo.Name;
                        data.EmailPTVMain = ORUserInfo.Email;
                        data.PhonePTVMain = ORUserInfo.Phone;
                        data.PositionPTVMain = "PTV chính";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }

                if (data.UIdPTVSub1 != null && data.UIdPTVSub1 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdPTVSub1);
                    if (ORUserInfo != null)
                    {
                        data.NamePTVSub1 = ORUserInfo.Name;
                        data.EmailPTVSub1 = ORUserInfo.Email;
                        data.PhonePTVSub1 = ORUserInfo.Phone;
                        data.PositionPTVSub1 = "PTV phụ 1";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }

                if (data.UIdPTVSub2 != null && data.UIdPTVSub2 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdPTVSub2);
                    if (ORUserInfo != null)
                    {
                        data.NamePTVSub2 = ORUserInfo.Name;
                        data.EmailPTVSub2 = ORUserInfo.Email;
                        data.PhonePTVSub2 = ORUserInfo.Phone;
                        data.PositionPTVSub2 = "PTV phụ 2";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdPTVSub3 != null && data.UIdPTVSub3 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdPTVSub3);
                    if (ORUserInfo != null)
                    {
                        data.NamePTVSub3 = ORUserInfo.Name;
                        data.EmailPTVSub3 = ORUserInfo.Email;
                        data.PhonePTVSub3 = ORUserInfo.Phone;
                        data.PositionPTVSub3 = "PTV phụ 3";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdPTVSub4 != null && data.UIdPTVSub4 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdPTVSub4);
                    if (ORUserInfo != null)
                    {
                        data.NamePTVSub4 = ORUserInfo.Name;
                        data.EmailPTVSub4 = ORUserInfo.Email;
                        data.PhonePTVSub4 = ORUserInfo.Phone;
                        data.PositionPTVSub4 = "PTV phụ 4";
                    }
                }
                if (data.UIdPTVSub5 != null && data.UIdPTVSub5 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdPTVSub5);
                    if (ORUserInfo != null)
                    {
                        data.NamePTVSub5 = ORUserInfo.Name;
                        data.EmailPTVSub5 = ORUserInfo.Email;
                        data.PhonePTVSub5 = ORUserInfo.Phone;
                        data.PositionPTVSub5 = "PTV phụ 5";
                    }
                }
                if (data.UIdPTVSub6 != null && data.UIdPTVSub6 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdPTVSub6);
                    if (ORUserInfo != null)
                    {
                        data.NamePTVSub6 = ORUserInfo.Name;
                        data.EmailPTVSub6 = ORUserInfo.Email;
                        data.PhonePTVSub6 = ORUserInfo.Phone;
                        data.PositionPTVSub6 = "PTV phụ 6";
                    }
                }
                if (data.UIdPTVSub7 != null && data.UIdPTVSub7 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdPTVSub7);
                    if (ORUserInfo != null)
                    {
                        data.NamePTVSub7 = ORUserInfo.Name;
                        data.EmailPTVSub7 = ORUserInfo.Email;
                        data.PhonePTVSub7 = ORUserInfo.Phone;
                        data.PositionPTVSub7 = "PTV phụ 7";
                    }
                }
                if (data.UIdPTVSub8 != null && data.UIdPTVSub8 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdPTVSub8);
                    if (ORUserInfo != null)
                    {
                        data.NamePTVSub8 = ORUserInfo.Name;
                        data.EmailPTVSub8 = ORUserInfo.Email;
                        data.PhonePTVSub8 = ORUserInfo.Phone;
                        data.PositionPTVSub8 = "PTV phụ 8";
                    }
                }
                if (data.UIdCECDoctor != null && data.UIdCECDoctor > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdCECDoctor);
                    if (ORUserInfo != null)
                    {
                        data.NameCECDoctor = ORUserInfo.Name;
                        data.EmailCECDoctor = ORUserInfo.Email;
                        data.PhoneCECDoctor = ORUserInfo.Phone;
                        data.PositionCECDoctor = "Bác sĩ CEC";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }



                if (data.UIdNurseTool1 != null && data.UIdNurseTool1 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdNurseTool1);
                    if (ORUserInfo != null)
                    {
                        data.NameNurseTool1 = ORUserInfo.Name;
                        data.EmailNurseTool1 = ORUserInfo.Email;
                        data.PhoneNurseTool1 = ORUserInfo.Phone;
                        data.PositionNurseTool1 = "Điều dưỡng dụng cụ 1";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }

                if (data.UIdNurseTool2 != null && data.UIdNurseTool2 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdNurseTool2);
                    if (ORUserInfo != null)
                    {
                        data.NameNurseTool2 = ORUserInfo.Name;
                        data.EmailNurseTool2 = ORUserInfo.Email;
                        data.PhoneNurseTool2 = ORUserInfo.Phone;
                        data.PositionNurseTool2 = "Điều dưỡng dụng cụ 2";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }

                if (data.UIdNurseOutRun1 != null && data.UIdNurseOutRun1 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdNurseOutRun1);
                    if (ORUserInfo != null)
                    {
                        data.NameNurseOutRun1 = ORUserInfo.Name;
                        data.EmailNurseOutRun1 = ORUserInfo.Email;
                        data.PhoneNurseOutRun1 = ORUserInfo.Phone;
                        data.PositionNurseOutRun1 = " Điều dưỡng chạy ngoài 1";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdNurseOutRun2 != null && data.UIdNurseOutRun2 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdNurseOutRun2);
                    if (ORUserInfo != null)
                    {
                        data.NameNurseOutRun2 = ORUserInfo.Name;
                        data.EmailNurseOutRun2 = ORUserInfo.Email;
                        data.PhoneNurseOutRun2 = ORUserInfo.Phone;
                        data.PositionNurseOutRun2 = " Điều dưỡng chạy ngoài 2";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdNurseOutRun3 != null && data.UIdNurseOutRun3 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdNurseOutRun3);
                    if (ORUserInfo != null)
                    {
                        data.NameNurseOutRun3 = ORUserInfo.Name;
                        data.EmailNurseOutRun3 = ORUserInfo.Email;
                        data.PhoneNurseOutRun3 = ORUserInfo.Phone;
                        data.PositionNurseOutRun3 = " Điều dưỡng chạy ngoài 3";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdNurseOutRun4 != null && data.UIdNurseOutRun4 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdNurseOutRun4);
                    if (ORUserInfo != null)
                    {
                        data.NameNurseOutRun4 = ORUserInfo.Name;
                        data.EmailNurseOutRun4 = ORUserInfo.Email;
                        data.PhoneNurseOutRun4 = ORUserInfo.Phone;
                        data.PositionNurseOutRun4 = " Điều dưỡng chạy ngoài 4";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdNurseOutRun5 != null && data.UIdNurseOutRun5 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdNurseOutRun5);
                    if (ORUserInfo != null)
                    {
                        data.NameNurseOutRun5 = ORUserInfo.Name;
                        data.EmailNurseOutRun5 = ORUserInfo.Email;
                        data.PhoneNurseOutRun5 = ORUserInfo.Phone;
                        data.PositionNurseOutRun5 = " Điều dưỡng chạy ngoài 5";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdNurseOutRun6 != null && data.UIdNurseOutRun6 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdNurseOutRun6);
                    if (ORUserInfo != null)
                    {
                        data.NameNurseOutRun6 = ORUserInfo.Name;
                        data.EmailNurseOutRun6 = ORUserInfo.Email;
                        data.PhoneNurseOutRun6 = ORUserInfo.Phone;
                        data.PositionNurseOutRun6 = " Điều dưỡng chạy ngoài 6";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdMainAnesthDoctor != null && data.UIdMainAnesthDoctor > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdMainAnesthDoctor);
                    if (ORUserInfo != null)
                    {
                        data.NameMainAnesthDoctor = ORUserInfo.Name;
                        data.EmailMainAnesthDoctor = ORUserInfo.Email;
                        data.PhoneMainAnesthDoctor = ORUserInfo.Phone;
                        data.PositionMainAnesthDoctor = "Bác sĩ gây mê ";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }

                if (data.UIdSubAnesthDoctor != null && data.UIdSubAnesthDoctor > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdSubAnesthDoctor);
                    if (ORUserInfo != null)
                    {
                        data.NameSubAnesthDoctor = ORUserInfo.Name;
                        data.EmailSubAnesthDoctor = ORUserInfo.Email;
                        data.PhoneSubAnesthDoctor = ORUserInfo.Phone;
                        data.PositionSubAnesthDoctor = "Bác sĩ phụ mê 1";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }

                if (data.UIdAnesthNurse1 != null && data.UIdAnesthNurse1 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdAnesthNurse1);
                    if (ORUserInfo != null)
                    {
                        data.NameAnesthNurse1 = ORUserInfo.Name;
                        data.EmailAnesthNurse1 = ORUserInfo.Email;
                        data.PhoneAnesthNurse1 = ORUserInfo.Phone;
                        data.PositionAnesthNurse1 = "Điều dưỡng phụ mê 1";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }


                if (data.UIdAnesthNurse2 != null && data.UIdAnesthNurse2 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdAnesthNurse2);
                    if (ORUserInfo != null)
                    {
                        data.NameAnesthNurse2 = ORUserInfo.Name;
                        data.EmailAnesthNurse2 = ORUserInfo.Email;
                        data.PhoneAnesthNurse2 = ORUserInfo.Phone;
                        data.PositionAnesthNurse2 = "Điều dưỡng phụ mê 2";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdAnesthNurseRecovery != null && data.UIdAnesthNurseRecovery > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdAnesthNurseRecovery);
                    if (ORUserInfo != null)
                    {
                        data.NameAnesthNurseRecovery = ORUserInfo.Name;
                        data.EmailAnesthNurseRecovery = ORUserInfo.Email;
                        data.PhoneAnesthNurseRecovery = ORUserInfo.Phone;
                        data.PositionAnesthNurseRecovery = "Điều dưỡng hồi tỉnh";
                    }
                }

                if (data.HpServiceId > 0)
                {
                    var dataService = DbContext.HpServices.FirstOrDefault(d => d.Id == data.HpServiceId);
                    if (dataService != null)
                    {
                        data.HpServiceCode = dataService.Oh_Code;
                        data.HpServiceName = dataService.Name;
                    }
                }
                if (data.ORRoomId > 0)
                {
                    var dataRoom = DbContext.ORRooms.FirstOrDefault(d => d.Id == data.ORRoomId);
                    if (dataRoom != null)
                    {
                        data.ORRoomName = dataRoom.Name;
                    }
                }
                if (data.CreatedBy > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.CreatedBy);
                    if (ORUserInfo != null)
                    {
                        data.NameCreatedBy = ORUserInfo.Name;
                        data.EmailCreatedBy = ORUserInfo.Email;
                        data.PhoneCreatedBy = ORUserInfo.Phone;
                        data.PositionCreatedBy = "Bác sĩ đăng ký mổ";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                //vutv7
                if (data.UIdKTVSubSurgery != null && data.UIdKTVSubSurgery > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdKTVSubSurgery);
                    if (ORUserInfo != null)
                    {
                        data.NameKTVSubSurgery = ORUserInfo.Name;
                        data.EmailKTVSubSurgery = ORUserInfo.Email;
                        data.PhoneKTVSubSurgery = ORUserInfo.Phone;
                        data.PositionKTVSubSurgery = "KTV phụ mổ";
                    }
                }
                if (data.UIdKTVDiagnose != null && data.UIdKTVDiagnose > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdKTVDiagnose);
                    if (ORUserInfo != null)
                    {
                        data.NameKTVDiagnose = ORUserInfo.Name;
                        data.EmailKTVDiagnose = ORUserInfo.Email;
                        data.PhoneKTVDiagnose = ORUserInfo.Phone;
                        data.PositionKTVDiagnose = "KTV CĐHA";
                    }
                }
                if (data.UIdKTVCEC != null && data.UIdKTVCEC > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdKTVCEC);
                    if (ORUserInfo != null)
                    {
                        data.NameKTVCEC = ORUserInfo.Name;
                        data.EmailKTVCEC = ORUserInfo.Email;
                        data.PhoneKTVCEC = ORUserInfo.Phone;
                        data.PositionKTVCEC = "KTV chạy máy CEC";
                    }
                }
                if (data.UIdDoctorDiagnose != null && data.UIdDoctorDiagnose > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdDoctorDiagnose);
                    if (ORUserInfo != null)
                    {
                        data.NameDoctorDiagnose = ORUserInfo.Name;
                        data.EmailDoctorDiagnose = ORUserInfo.Email;
                        data.PhoneDoctorDiagnose = ORUserInfo.Phone;
                        data.PositionDoctorDiagnose = "BS CĐHA";
                    }
                }
                if (data.UIdDoctorNewBorn != null && data.UIdDoctorNewBorn > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdDoctorNewBorn);
                    if (ORUserInfo != null)
                    {
                        data.NameDoctorNewBorn = ORUserInfo.Name;
                        data.EmailDoctorNewBorn = ORUserInfo.Email;
                        data.PhoneDoctorNewBorn = ORUserInfo.Phone;
                        data.PositionDoctorNewBorn = "BS sơ sinh";
                    }
                }
                if (data.UIdMidwives != null && data.UIdMidwives > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdMidwives);
                    if (ORUserInfo != null)
                    {
                        data.NameMidwives = ORUserInfo.Name;
                        data.EmailMidwives = ORUserInfo.Email;
                        data.PhoneMidwives = ORUserInfo.Phone;
                        data.PositionMidwives = "Nữ hộ sinh";
                    }
                }
                if (data.UIdSubAnesthDoctor2 != null && data.UIdSubAnesthDoctor2 > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdSubAnesthDoctor2);
                    if (ORUserInfo != null)
                    {
                        data.NameSubAnesthDoctor2 = ORUserInfo.Name;
                        data.EmailSubAnesthDoctor2 = ORUserInfo.Email;
                        data.PhoneSubAnesthDoctor2 = ORUserInfo.Phone;
                        data.PositionSubAnesthDoctor2 = "Bác sĩ phụ mê 2";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                if (data.UIdAnesthesiologist != null && data.UIdAnesthesiologist > 0)
                {
                    var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.UIdAnesthesiologist);
                    if (ORUserInfo != null)
                    {
                        data.NameAnesthesiologist = ORUserInfo.Name;
                        data.EmailAnesthesiologist = ORUserInfo.Email;
                        data.PhoneAnesthesiologist = ORUserInfo.Phone;
                        data.PositionAnesthesiologist = "BS khám phụ mê";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
                    }
                }
                #endregion
                return data;
            }
            return null;
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage SaveORRegistorByJson(ORAnesthProgressContract data, int userId)
        {
            try
            {
                ORAnesthProgress progress;

                if (data == null || string.IsNullOrEmpty(data.PId))
                {
                    return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
                }

                if (data.dtStart == null || data.dtEnd == null)
                {
                    return new CUDReturnMessage() { Id = 0, Message = "Thời gian bắt đầu hoặc kết thúc không hợp lệ" };
                }

                if (data.HpServiceId == 0)
                {
                    return new CUDReturnMessage() { Id = 0, Message = "Chưa chọn dịch vụ" };
                }

                if (data.dtStart > data.dtEnd)
                {
                    return new CUDReturnMessage() { Id = 0, Message = "Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc" };
                }

                if (data.dtEnd.Value.Subtract(data.dtStart.Value) < TimeSpan.FromMinutes(15))
                {
                    return new CUDReturnMessage() { Id = 0, Message = "Thời gian tối thiểu là 15 phút" };
                }
                data.NgaySinh = data.NgaySinh == DateTime.MinValue ? null : data.NgaySinh;
                #region check validate duplicate time 
                if (data.SurgeryType != 2)
                {
                    var rsCheckExist = ExecStoredProc<ExecuteQueryIntIdResult>("Core_CheckExistORRegistor", new object[] { data.Id, data.dtStart, data.dtEnd, data.ORRoomId, data.HpServiceId, data.HospitalCode, userId }).FirstOrDefault();

                    if (rsCheckExist != null && rsCheckExist.IsSuccess)
                        return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại" };
                }
                #endregion
                //Check Role permission for location data
                #region Check Role permission for location data
                if (data.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                #endregion .Check Role permission for location data
                var objHpService = DbContext.HpServices.Find(data.HpServiceId);
                var dtStart = data.dtStart.Value.AddMinutes((double)-data.TimeAnesth);
                var dtEnd = data.dtEnd.Value.AddMinutes((double)objHpService?.CleaningTime);
                if (data.Id == 0) //Innsert
                {
                    #region OR
                    progress = new ORAnesthProgress()
                    {
                        VisitCode = data.VisitCode,
                        PId = data.PId,
                        Id = data.Id,
                        HospitalCode = data.HospitalCode,
                        HospitalName = data.HospitalName,
                        ORRoomId = data.ORRoomId,
                        SurgeryType = data.SurgeryType,
                        HpServiceId = data.HpServiceId,
                        dtStart = data.dtStart,
                        dtEnd = data.dtEnd,
                        dtOperation = data.dtOperation,
                        dtAdmission = data.dtAdmission,
                        TimeAnesth = data.TimeAnesth,
                        RegDescription = data.RegDescription,
                        State = data.State,

                        UIdPTVMain = data.UIdPTVMain ?? 0,
                        UIdPTVSub1 = data.UIdPTVSub1 ?? 0,
                        UIdPTVSub2 = data.UIdPTVSub2 ?? 0,
                        UIdPTVSub3 = data.UIdPTVSub3 ?? 0,
                        UIdPTVSub4 = data.UIdPTVSub4 ?? 0,
                        UIdPTVSub5 = data.UIdPTVSub5 ?? 0,
                        UIdPTVSub6 = data.UIdPTVSub6 ?? 0,
                        UIdPTVSub7 = data.UIdPTVSub7 ?? 0,
                        UIdPTVSub8 = data.UIdPTVSub8 ?? 0,
                        UIdCECDoctor = data.UIdCECDoctor ?? 0,
                        UIdNurseTool1 = data.UIdNurseTool1 ?? 0,
                        UIdNurseTool2 = data.UIdNurseTool2 ?? 0,
                        UIdNurseOutRun1 = data.UIdNurseOutRun1 ?? 0,
                        UIdNurseOutRun2 = data.UIdNurseOutRun2 ?? 0,

                        SurgeryDescription = data.SurgeryDescription,
                        UIdMainAnesthDoctor = data.UIdMainAnesthDoctor ?? 0,
                        UIdSubAnesthDoctor = data.UIdSubAnesthDoctor ?? 0,
                        UIdAnesthNurse1 = data.UIdAnesthNurse1 ?? 0,
                        UIdAnesthNurse2 = data.UIdAnesthNurse2 ?? 0,

                        AnesthDescription = data.AnesthDescription,
                        NameProject = data.NameProject,
                        OrderID = data.OrderID,
                        ChargeDetailId = data.ChargeDetailId,
                        DepartmentCode = data.DepartmentCode,
                        AdmissionWard = data.AdmissionWard,
                        Diagnosis = data.Diagnosis,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = 0,
                        UpdatedDate = DateTime.Now,
                        IsDeleted = false,
                        ChargeDate = data.ChargeDate,
                        ChargeBy = data.ChargeBy
                    };
                    #endregion
                    #region patient

                    var p = DbContext.ReportPatients.FirstOrDefault(d => d.Id.Equals(data.PId));
                    if (p == null)
                    {
                        p = new ReportPatient()
                        {
                            Id = data.PId,
                            HoTen = data.HoTen,
                            NgaySinh = data.NgaySinh,
                            Sex = data.Sex,
                            National = data.National == null ? string.Empty : data.National,
                            Address = data.Address,
                            CreatedBy = userId,
                            UpdatedBy = 0,
                            IsDeleted = false,
                            Email = data.Email,
                            Age = data.Ages,
                            Phone = data.PatientPhone
                        };
                        DbContext.ReportPatients.Add(p);
                    }
                    #endregion

                    if (data.dtStart.Value == data.dtEnd.Value)
                    {
                        return new CUDReturnMessage() { Id = 0, Message = "Thời gian bắt đầu và kết thúc không được trùng nhau" };
                    }

                    DbContext.ORAnesthProgresses.Add(progress);
                    if (data.SurgeryType != 2)
                    {
                        var reCheckExist = ExecStoredProc<ExecuteQueryIntIdResult>("Core_CheckExistORRegistor", new object[] { data.Id, data.dtStart, data.dtEnd, data.ORRoomId, data.HpServiceId, data.HospitalCode, userId }).FirstOrDefault();

                        if (reCheckExist != null && reCheckExist.IsSuccess)
                            return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại " };
                    }

                    #region Comment Code ko dung
                    //var lastDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
                    //var bookRooms = DbContext.ORAnesthProgresses.Where(x => !(x.IsDeleted ?? false) && x.PId == data.PId && x.dtStart >= DateTime.Today && x.dtEnd <= lastDate).ToList();
                    //foreach (var item in bookRooms)
                    //{
                    //    if ((item.dtStart.Value.AddMinutes((double)-data.TimeAnesth) <= dtStart && item.dtEnd.Value.AddMinutes((double)objHpService?.CleaningTime) >= dtStart) || (item.dtStart.Value.AddMinutes((double)-data.TimeAnesth) <= dtEnd && item.dtEnd.Value.AddMinutes((double)objHpService?.CleaningTime) >= dtEnd))
                    //    {
                    //        return new CUDReturnMessage() { Id = 0, Message = "Bệnh nhân đã được đặt phòng, vui lòng kiểm tra lại" };
                    //    }
                    //}

                    //var dtStarts = DbContext.ORAnesthProgresses.Where(x => !(x.IsDeleted ?? false) && x.dtStart >= dtStart && x.dtStart <= lastDate && x.ORRoomId == data.ORRoomId).ToList();
                    //foreach (var item in dtStarts)
                    //{
                    //    var timeAnesth = -item.TimeAnesth ?? 0.0;
                    //    if (item.dtStart.Value.AddMinutes(timeAnesth) < dtEnd)
                    //    {
                    //        return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại" };
                    //    }
                    //}

                    //var dtEnds = DbContext.ORAnesthProgresses.Where(x => !(x.IsDeleted ?? false) && x.dtEnd >= DateTime.Today && x.dtEnd <= dtEnd && x.ORRoomId == data.ORRoomId).ToList();
                    //foreach (var item in dtEnds)
                    //{
                    //    if(item.dtEnd.Value.AddMinutes((double)objHpService?.CleaningTime) > dtStart)
                    //    {
                    //        return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại" };
                    //    }
                    //}
                    #endregion .Comment Code ko dung

                    var startDayOfSurgery = data.dtStart.Value.Date.AddHours(00).AddMinutes(00).AddSeconds(00);
                    var endDayOfSurgery = data.dtEnd.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    var orAnesthProgresses = DbContext.ORAnesthProgresses.Where(x => !(x.IsDeleted ?? false) && x.dtStart >= startDayOfSurgery && x.dtEnd <= endDayOfSurgery);
                    //Kiem tra xem PID da co lich mo? tai khung gio duoc chi dinh hay chua
                    #region Kiem tra xem PID da co lich mo? tai khung gio duoc chi dinh hay chua
                    if (orAnesthProgresses.Any(x => x.PId == data.PId))
                    {
                        var patients = orAnesthProgresses.Where(x => x.PId == data.PId).ToList();
                        foreach (var item in patients)
                        {
                            var objHpServiceCurent = DbContext.HpServices.Find(item.HpServiceId);
                            if (
                                item.ORRoomId == data.ORRoomId &&
                                ((item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtStart && dtStart < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime)) ||
                                (item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd && dtEnd < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent.CleaningTime)) ||
                                (dtStart < item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd) ||
                                (dtStart == item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) == dtEnd) ||
                                (dtStart < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) && item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) < dtEnd)))
                            {
                                return new CUDReturnMessage() { Id = 0, Message = "Bệnh nhân đã được đặt phòng, vui lòng kiểm tra lại" };
                            }
                            else if (
                               item.ORRoomId != data.ORRoomId &&
                                (/*(item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtStart && dtStart < item.dtEnd.Value) ||*/
                                (item.dtStart.Value < dtStart && dtStart < item.dtEnd.Value) ||
                                //(item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd && dtEnd < item.dtEnd.Value) ||
                                (item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd.AddMinutes((double)-objHpService.CleaningTime) && dtEnd.AddMinutes((double)-objHpService.CleaningTime) < item.dtEnd.Value) ||
                                (dtStart < item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value < dtEnd.AddMinutes((double)-objHpService.CleaningTime)) ||
                                (dtStart < item.dtEnd.Value && item.dtEnd.Value < dtEnd.AddMinutes((double)-objHpService.CleaningTime)) ||
                                (dtStart == item.dtEnd.Value.AddMinutes((double)-item.TimeAnesth) && item.dtEnd.Value == dtEnd.AddMinutes((double)-objHpService.CleaningTime))))
                            {
                                return new CUDReturnMessage() { Id = 0, Message = "Bệnh nhân đã được đặt phòng, vui lòng kiểm tra lại" };
                            }
                        }
                    }
                    #endregion .Kiem tra xem PID da co lich mo? tai khung gio duoc chi dinh hay chua
                    //Kiem tra xem Phong mo da co lich mo? tai khung gio duoc chi dinh hay chua
                    #region Kiem tra xem Phong mo da co lich mo? tai khung gio duoc chi dinh hay chua
                    if (data.SurgeryType != 2)
                    {
                        if (orAnesthProgresses.Any(x => x.ORRoomId == data.ORRoomId))
                        {
                            var patients = orAnesthProgresses.Where(x => x.ORRoomId == data.ORRoomId).ToList();
                            foreach (var item in patients)
                            {
                                var objHpServiceCurent = DbContext.HpServices.Find(item.HpServiceId);
                                if ((item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtStart && dtStart < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime)) ||
                                    (item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd && dtEnd < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent.CleaningTime)) ||
                                    (dtStart < item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd) ||
                                    (dtStart < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) && item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) < dtEnd) ||
                                    (dtStart == item.dtStart.Value.AddMinutes((double)-item.TimeAnesth)))
                                {
                                    return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại" };
                                }
                            }
                        }
                    }
                    #endregion .Kiem tra xem Phong mo da co lich mo? tai khung gio duoc chi dinh hay chua
                    //if(orAnesthProgresses.Any(x => x.ORRoomId == data.ORRoomId && ))
                    DbContext.SaveChanges();
                    InsertUserTracking(Newtonsoft.Json.JsonConvert.SerializeObject(data), userId, progress.Id, progress.State, 1);
                    return new CUDReturnMessage() { Id = (int)progress.Id, Message = "Tạo mới hành công" };
                }
                //Update
                progress = DbContext.ORAnesthProgresses.FirstOrDefault(r => (r.Id == data.Id));
                if (progress == null)
                    return new CUDReturnMessage() { Id = 0, Message = "Lỗi trong quá trình xử lý" };


                if (progress.State == (int)ORProgressStateEnum.Registor || progress.State == (int)ORProgressStateEnum.NoApproveSurgeryManager || progress.State == (int)ORProgressStateEnum.AssignEkip)//đăng ký mỗ
                {
                    progress.VisitCode = data.VisitCode;
                    progress.PId = data.PId;
                    progress.Id = data.Id;
                    progress.HospitalCode = data.HospitalCode;
                    progress.HospitalName = data.HospitalName;
                    progress.ORRoomId = data.ORRoomId;
                    progress.SurgeryType = data.SurgeryType;
                    progress.dtStart = data.dtStart;
                    progress.dtEnd = data.dtEnd;
                    progress.dtOperation = data.dtOperation;
                    progress.TimeAnesth = data.TimeAnesth;
                    progress.AdmissionWard = data.AdmissionWard;
                    progress.dtAdmission = data.dtAdmission;
                    progress.RegDescription = data.RegDescription;
                    progress.Diagnosis = data.Diagnosis;
                    progress.State = progress.State == (int)ORProgressStateEnum.AssignEkip ? progress.State : (int)ORProgressStateEnum.Registor;
                    progress.NameProject = data.NameProject;
                    progress.HpServiceId = data.HpServiceId;
                    progress.UIdPTVMain = data.UIdPTVMain;

                    var startDayOfSurgery = data.dtStart.Value.Date.AddHours(00).AddMinutes(00).AddSeconds(00);
                    var endDayOfSurgery = data.dtEnd.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    var orAnesthProgresses = DbContext.ORAnesthProgresses.Where(x => !(x.IsDeleted ?? false) && x.dtStart >= startDayOfSurgery && x.dtEnd <= endDayOfSurgery);
                    //Kiem tra xem PID da co lich mo? tai khung gio duoc chi dinh hay chua
                    #region Kiem tra xem PID da co lich mo? tai khung gio duoc chi dinh hay chua
                    if (orAnesthProgresses.Any(x => x.PId == data.PId))
                    {
                        var patients = orAnesthProgresses.Where(x => x.PId == data.PId && x.Id != data.Id).ToList();
                        foreach (var item in patients)
                        {
                            var objHpServiceCurent = DbContext.HpServices.Find(item.HpServiceId);
                            if (
                                item.ORRoomId == data.ORRoomId &&
                                ((item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtStart && dtStart < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime)) ||
                                (item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd && dtEnd < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent.CleaningTime)) ||
                                (dtStart < item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd) ||
                                (dtStart < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) && item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) < dtEnd)) ||
                                (dtStart == item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) == dtEnd))
                            {
                                return new CUDReturnMessage() { Id = 0, Message = "Bệnh nhân đã được đặt phòng, vui lòng kiểm tra lại" };
                            }
                            else if (
                               item.ORRoomId != data.ORRoomId &&
                               (/*(item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtStart && dtStart < item.dtEnd.Value) ||*/
                               (item.dtStart.Value < dtStart && dtStart < item.dtEnd.Value) ||
                               //(item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd && dtEnd < item.dtEnd.Value) ||
                               (item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd.AddMinutes((double)-objHpService.CleaningTime) && dtEnd.AddMinutes((double)-objHpService.CleaningTime) < item.dtEnd.Value) ||
                               (dtStart < item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value < dtEnd.AddMinutes((double)-objHpService.CleaningTime)) ||
                               (dtStart < item.dtEnd.Value && item.dtEnd.Value < dtEnd.AddMinutes((double)-objHpService.CleaningTime)) ||
                               (dtStart == item.dtEnd.Value.AddMinutes((double)-item.TimeAnesth) && item.dtEnd.Value == dtEnd.AddMinutes((double)-objHpService.CleaningTime))))
                            {
                                return new CUDReturnMessage() { Id = 0, Message = "Bệnh nhân đã được đặt phòng, vui lòng kiểm tra lại" };
                            }
                        }
                    }
                    #endregion .Kiem tra xem PID da co lich mo? tai khung gio duoc chi dinh hay chua
                    //Kiem tra xem Phong mo da co lich mo? tai khung gio duoc chi dinh hay chua
                    #region Kiem tra xem Phong mo da co lich mo? tai khung gio duoc chi dinh hay chua
                    if (orAnesthProgresses.Any(x => x.ORRoomId == data.ORRoomId))
                    {
                        var patients = orAnesthProgresses.Where(x => x.ORRoomId == data.ORRoomId && x.Id != data.Id).ToList();
                        foreach (var item in patients)
                        {
                            var objHpServiceCurent = DbContext.HpServices.Find(item.HpServiceId);
                            if ((item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtStart && dtStart < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime)) ||
                                (item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd && dtEnd < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent.CleaningTime)) ||
                                (dtStart < item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) < dtEnd) ||
                                (dtStart < item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) && item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) < dtEnd) ||
                                (dtStart == item.dtStart.Value.AddMinutes((double)-item.TimeAnesth)))
                            {
                                return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại" };
                            }
                        }
                    }
                    #endregion .Kiem tra xem Phong mo da co lich mo? tai khung gio duoc chi dinh hay chua

                    #region update patient
                    var pt = DbContext.ReportPatients.FirstOrDefault(d => d.Id.Equals(data.PId));
                    if (pt != null)
                    {
                        if (!string.IsNullOrEmpty(data.Address))
                            pt.Address = data.Address;
                        if (!string.IsNullOrEmpty(data.Email))
                            pt.Email = data.Email;
                        if (!string.IsNullOrEmpty(data.Ages))
                            pt.Age = data.Ages;
                        if (!string.IsNullOrEmpty(data.PatientPhone))
                            pt.Phone = data.PatientPhone;
                    }
                    #endregion
                }
                else if ((progress.State == (int)ORProgressStateEnum.ApproveSurgeryManager) || (progress.State == (int)ORProgressStateEnum.NoApproveSurgeryManager))
                {
                    progress.UIdPTVMain = data.UIdPTVMain;
                    progress.UIdPTVSub1 = data.UIdPTVSub1;
                    progress.UIdPTVSub2 = data.UIdPTVSub2;
                    progress.UIdPTVSub3 = data.UIdPTVSub3;
                    progress.UIdPTVSub4 = data.UIdPTVSub4;
                    progress.UIdPTVSub5 = data.UIdPTVSub5;
                    progress.UIdPTVSub6 = data.UIdPTVSub6;
                    progress.UIdPTVSub7 = data.UIdPTVSub7;
                    progress.UIdPTVSub8 = data.UIdPTVSub8;
                    progress.UIdCECDoctor = data.UIdCECDoctor;
                    progress.UIdNurseTool1 = data.UIdNurseTool1;
                    progress.UIdNurseTool2 = data.UIdNurseTool2;
                    progress.UIdNurseOutRun1 = data.UIdNurseOutRun1;
                    progress.UIdNurseOutRun2 = data.UIdNurseOutRun2;
                    progress.SurgeryDescription = data.SurgeryDescription;
                }
                else if ((progress.State == (int)ORProgressStateEnum.CancelAnesthManager) || (progress.State == (int)ORProgressStateEnum.ApproveAnesthManager))
                {
                    progress.UIdMainAnesthDoctor = data.UIdMainAnesthDoctor;
                    progress.UIdSubAnesthDoctor = data.UIdSubAnesthDoctor;
                    progress.UIdAnesthNurse1 = data.UIdAnesthNurse1;
                    progress.UIdAnesthNurse2 = data.UIdAnesthNurse2;
                    progress.AnesthDescription = data.AnesthDescription;
                }
                progress.UpdatedBy = userId;
                progress.UpdatedDate = DateTime.Now;
                DbContext.SaveChanges();
                InsertUserTracking(Newtonsoft.Json.JsonConvert.SerializeObject(data), userId, progress.Id, progress.State, 2);
                return new CUDReturnMessage() { Id = (int)progress.Id, Message = "cập nhật hành công" };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo Site
        /// actionType=2: Trực tiếp ghi nhận ekip (Không qua book phòng)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public CUDReturnMessage SaveORManagementByJson(ORAnesthProgressContract data, int userId, int actionType = 1)
        {
            ORAnesthProgress progress;
            #region Check Approve Anesth or Sugery
            if (data != null)
            {
                if (data.State != (int)ORProgressStateEnum.CancelAnesthManager && data.State != (int)ORProgressStateEnum.NoApproveSurgeryManager && data.State != (int)ORProgressStateEnum.AssignEkip)
                {
                    if (CheckChargeExistInApproved(data.Id, data.OrderID, data.ChargeDetailId, data.HospitalCode))
                    {
                        return new CUDReturnMessage() { Id = -1001171, Message = "Chỉ định cho ca mổ/mê này đã được ghi nhận. Vui lòng chọn chỉ định khác!" };
                    }
                }
            }
            #endregion
            if (data == null || data.Id == 0)
            {
                if (actionType == 2)
                {
                    #region Xử lý TH trực tiếp ghi nhận ekip
                    if (IsOrderExisted(data.OrderID, data.ChargeDetailId, data.HospitalCode))
                        return new CUDReturnMessage() { Id = 0, Message = "Chỉ định này đã được ghi nhận ekip, vui lòng kiểm tra lại!" };
                    #region OR
                    progress = new ORAnesthProgress()
                    {
                        VisitCode = data.VisitCode,
                        PId = data.PId,
                        Id = data.Id,
                        HospitalCode = data.HospitalCode,
                        HospitalName = data.HospitalName,
                        ORRoomId = data.ORRoomId,
                        SurgeryType = data.SurgeryType,
                        HpServiceId = data.HpServiceId,
                        dtStart = data.dtStart,
                        dtEnd = data.dtEnd,
                        dtOperation = data.dtOperation,
                        TimeAnesth = data.TimeAnesth,
                        RegDescription = data.RegDescription,
                        State = data.State,

                        UIdPTVMain = data.UIdPTVMain ?? 0,
                        UIdPTVSub1 = data.UIdPTVSub1 ?? 0,
                        UIdPTVSub2 = data.UIdPTVSub2 ?? 0,
                        UIdPTVSub3 = data.UIdPTVSub3 ?? 0,
                        UIdPTVSub4 = data.UIdPTVSub4 ?? 0,
                        UIdPTVSub5 = data.UIdPTVSub5 ?? 0,
                        UIdPTVSub6 = data.UIdPTVSub6 ?? 0,
                        UIdPTVSub7 = data.UIdPTVSub7 ?? 0,
                        UIdPTVSub8 = data.UIdPTVSub8 ?? 0,
                        UIdCECDoctor = data.UIdCECDoctor ?? 0,
                        UIdNurseTool1 = data.UIdNurseTool1 ?? 0,
                        UIdNurseTool2 = data.UIdNurseTool2 ?? 0,
                        UIdNurseOutRun1 = data.UIdNurseOutRun1 ?? 0,
                        UIdNurseOutRun2 = data.UIdNurseOutRun2 ?? 0,

                        SurgeryDescription = data.SurgeryDescription,
                        UIdMainAnesthDoctor = data.UIdMainAnesthDoctor ?? 0,
                        UIdSubAnesthDoctor = data.UIdSubAnesthDoctor ?? 0,
                        UIdAnesthNurse1 = data.UIdAnesthNurse1 ?? 0,
                        UIdAnesthNurse2 = data.UIdAnesthNurse2 ?? 0,

                        AnesthDescription = data.AnesthDescription,
                        NameProject = data.NameProject,
                        OrderID = data.OrderID,
                        ChargeDetailId = data.ChargeDetailId,
                        DepartmentCode = data.DepartmentCode,
                        AdmissionWard = data.AdmissionWard,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = 0,
                        UpdatedDate = DateTime.Now,
                        IsDeleted = false,
                        //vutv7
                        ChargeDate = data.ChargeDate,
                        ChargeBy = data.ChargeBy,
                        UIdKTVSubSurgery = data.UIdKTVSubSurgery ?? 0,
                        UIdKTVDiagnose = data.UIdKTVDiagnose ?? 0,
                        UIdKTVCEC = data.UIdKTVCEC ?? 0,
                        UIdDoctorDiagnose = data.UIdDoctorDiagnose ?? 0,
                        UIdDoctorNewBorn = data.UIdDoctorNewBorn ?? 0,
                        UIdMidwives = data.UIdMidwives ?? 0,
                        UIdSubAnesthDoctor2 = data.UIdSubAnesthDoctor2 ?? 0,
                        UIdAnesthesiologist = data.UIdAnesthesiologist ?? 0,

                    };
                    #endregion
                    #region patient

                    var p = DbContext.ReportPatients.FirstOrDefault(d => d.Id.Equals(data.PId));
                    if (p == null)
                    {
                        p = new ReportPatient()
                        {
                            Id = data.PId,
                            HoTen = data.HoTen,
                            NgaySinh = data.NgaySinh,
                            Sex = data.Sex,
                            National = data.National == null ? string.Empty : data.National,
                            Address = data.Address,
                            CreatedBy = userId,
                            UpdatedBy = 0,
                            IsDeleted = false,
                            Email = data.Email,
                            Age = data.Ages,
                            Phone = data.PatientPhone
                        };
                        DbContext.ReportPatients.Add(p);
                    }
                    #endregion
                    if (progress.ORRoomId <= 0)
                    {
                        //get default room for direct approve action
                        var roomEnitty = DbContext.ORRooms.FirstOrDefault(x => x.HospitalCode == data.HospitalCode && x.TypeRoom == (int)RoomTypeEnum.Approve4Pay);
                        progress.ORRoomId = roomEnitty != null ? roomEnitty.Id : 0;
                    }
                    DbContext.ORAnesthProgresses.Add(progress);
                    DbContext.SaveChanges();
                    data.Id = progress.Id;
                    #endregion
                }
                else
                {
                    return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
                }
            }
            //Update
            progress = DbContext.ORAnesthProgresses.FirstOrDefault(r => (r.Id == data.Id));
            if (progress == null)
                return new CUDReturnMessage() { Id = 0, Message = "Lỗi trong quá trình xử lý" };
            #region Check access denied on Data
            if (progress.CreatedBy != MemberExtInfo.UserId && !MemberExtInfo.IsSuperAdmin && !MemberExtInfo.IsManageAdminSurgery && !MemberExtInfo.IsManagAnes)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check access denied on Data
            //Check Role permission for location data
            #region Check Role permission for location data
            if (progress.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check Role permission for location data
            //else if (((data.State == (int)ORProgressStateEnum.ApproveSurgeryManager) || (data.State == (int)ORProgressStateEnum.NoApproveSurgeryManager))&& (progress.State==(int)ORProgressStateEnum.Registor))
            else if (data.State == (int)ORProgressStateEnum.NoApproveSurgeryManager || Constant.ListStateAllowCoordinator.Contains(data.State))
            {
                if (data.UIdPTVMain != null && data.UIdPTVMain != 0)
                    progress.UIdPTVMain = data.UIdPTVMain;
                if (data.UIdPTVSub1 != null && data.UIdPTVSub1 != 0)
                    progress.UIdPTVSub1 = data.UIdPTVSub1;
                if (data.UIdPTVSub2 != null && data.UIdPTVSub2 != 0)
                    progress.UIdPTVSub2 = data.UIdPTVSub2;
                if (data.UIdPTVSub3 != null && data.UIdPTVSub3 != 0)
                    progress.UIdPTVSub3 = data.UIdPTVSub3;
                if (data.UIdPTVSub4 != null && data.UIdPTVSub4 != 0)
                    progress.UIdPTVSub4 = data.UIdPTVSub4;
                if (data.UIdPTVSub5 != null && data.UIdPTVSub5 != 0)
                    progress.UIdPTVSub5 = data.UIdPTVSub5;
                if (data.UIdPTVSub6 != null && data.UIdPTVSub6 != 0)
                    progress.UIdPTVSub6 = data.UIdPTVSub6;
                if (data.UIdPTVSub7 != null && data.UIdPTVSub7 != 0)
                    progress.UIdPTVSub7 = data.UIdPTVSub7;
                if (data.UIdPTVSub8 != null && data.UIdPTVSub8 != 0)
                    progress.UIdPTVSub8 = data.UIdPTVSub8;
                if (data.UIdCECDoctor != null && data.UIdCECDoctor != 0)
                    progress.UIdCECDoctor = data.UIdCECDoctor;
                if (data.UIdNurseTool1 != null && data.UIdNurseTool1 != 0)
                    progress.UIdNurseTool1 = data.UIdNurseTool1;
                if (data.UIdNurseTool2 != null && data.UIdNurseTool2 != 0)
                    progress.UIdNurseTool2 = data.UIdNurseTool2;
                if (data.UIdNurseOutRun1 != null && data.UIdNurseOutRun1 != 0)
                    progress.UIdNurseOutRun1 = data.UIdNurseOutRun1;
                if (data.UIdNurseOutRun2 != null && data.UIdNurseOutRun2 != 0)
                    progress.UIdNurseOutRun2 = data.UIdNurseOutRun2;
                if (data.UIdNurseOutRun3 != null && data.UIdNurseOutRun3 != 0)
                    progress.UIdNurseOutRun3 = data.UIdNurseOutRun3;
                if (data.UIdNurseOutRun4 != null && data.UIdNurseOutRun4 != 0)
                    progress.UIdNurseOutRun4 = data.UIdNurseOutRun4;
                if (data.UIdNurseOutRun5 != null && data.UIdNurseOutRun5 != 0)
                    progress.UIdNurseOutRun5 = data.UIdNurseOutRun5;
                if (data.UIdNurseOutRun6 != null && data.UIdNurseOutRun6 != 0)
                    progress.UIdNurseOutRun6 = data.UIdNurseOutRun6;
                if (!string.IsNullOrEmpty(data.SurgeryDescription))
                    progress.SurgeryDescription = data.SurgeryDescription;

                if (data.UIdMainAnesthDoctor != null && data.UIdMainAnesthDoctor != 0)
                    progress.UIdMainAnesthDoctor = data.UIdMainAnesthDoctor;
                if (data.UIdSubAnesthDoctor != null && data.UIdSubAnesthDoctor != 0)
                    progress.UIdSubAnesthDoctor = data.UIdSubAnesthDoctor;
                if (data.UIdAnesthNurse1 != null && data.UIdAnesthNurse1 != 0)
                    progress.UIdAnesthNurse1 = data.UIdAnesthNurse1;
                if (data.UIdAnesthNurse2 != null && data.UIdAnesthNurse2 != 0)
                    progress.UIdAnesthNurse2 = data.UIdAnesthNurse2;
                if (data.UIdAnesthNurseRecovery != null && data.UIdAnesthNurseRecovery != 0)
                    progress.UIdAnesthNurseRecovery = data.UIdAnesthNurseRecovery;
                progress.OrderID = data.OrderID;
                progress.ChargeDetailId = data.ChargeDetailId;
                //Vutv7
                progress.ChargeDate = data.ChargeDate;
                progress.ChargeBy = data.ChargeBy;
                if (data.State == (int)ORProgressStateEnum.ApproveSurgeryManager)
                {
                    progress.DepartmentCode = data.DepartmentCode;
                    progress.AdmissionWard = string.IsNullOrEmpty(progress.AdmissionWard) ? data.AdmissionWard : progress.AdmissionWard;
                }
                if (!string.IsNullOrEmpty(data.AnesthDescription))
                    progress.AnesthDescription = data.AnesthDescription;
                progress.State = data.State;
                //vutv7
                if (data.UIdKTVSubSurgery != null && data.UIdKTVSubSurgery != 0)
                    progress.UIdKTVSubSurgery = data.UIdKTVSubSurgery;
                if (data.UIdKTVDiagnose != null && data.UIdKTVDiagnose != 0)
                    progress.UIdKTVDiagnose = data.UIdKTVDiagnose;
                if (data.UIdKTVCEC != null && data.UIdKTVCEC != 0)
                    progress.UIdKTVCEC = data.UIdKTVCEC;
                if (data.UIdDoctorDiagnose != null && data.UIdDoctorDiagnose != 0)
                    progress.UIdDoctorDiagnose = data.UIdDoctorDiagnose;
                if (data.UIdDoctorNewBorn != null && data.UIdDoctorNewBorn != 0)
                    progress.UIdDoctorNewBorn = data.UIdDoctorNewBorn;
                if (data.UIdMidwives != null && data.UIdMidwives != 0)
                    progress.UIdMidwives = data.UIdMidwives;
                if (data.UIdSubAnesthDoctor2 != null && data.UIdSubAnesthDoctor2 != 0)
                    progress.UIdSubAnesthDoctor2 = data.UIdSubAnesthDoctor2;
                if (data.UIdAnesthesiologist != null && data.UIdAnesthesiologist != 0)
                    progress.UIdAnesthesiologist = data.UIdAnesthesiologist;
            }
            else if ((data.State == (int)ORProgressStateEnum.CancelAnesthManager) || Constant.ListStateAllowCoordinator.Contains(data.State))
            {
                if (data.UIdPTVMain != null && data.UIdPTVMain != 0)
                    progress.UIdPTVMain = data.UIdPTVMain;
                if (data.UIdPTVSub1 != null && data.UIdPTVSub1 != 0)
                    progress.UIdPTVSub1 = data.UIdPTVSub1;
                if (data.UIdPTVSub2 != null && data.UIdPTVSub2 != 0)
                    progress.UIdPTVSub2 = data.UIdPTVSub2;
                if (data.UIdPTVSub3 != null && data.UIdPTVSub3 != 0)
                    progress.UIdPTVSub3 = data.UIdPTVSub3;
                if (data.UIdPTVSub4 != null && data.UIdPTVSub4 != 0)
                    progress.UIdPTVSub4 = data.UIdPTVSub4;
                if (data.UIdPTVSub5 != null && data.UIdPTVSub5 != 0)
                    progress.UIdPTVSub5 = data.UIdPTVSub5;
                if (data.UIdPTVSub6 != null && data.UIdPTVSub6 != 0)
                    progress.UIdPTVSub6 = data.UIdPTVSub6;
                if (data.UIdPTVSub7 != null && data.UIdPTVSub7 != 0)
                    progress.UIdPTVSub7 = data.UIdPTVSub7;
                if (data.UIdPTVSub8 != null && data.UIdPTVSub8 != 0)
                    progress.UIdPTVSub8 = data.UIdPTVSub8;
                if (data.UIdCECDoctor != null && data.UIdCECDoctor != 0)
                    progress.UIdCECDoctor = data.UIdCECDoctor;
                if (data.UIdNurseTool1 != null && data.UIdNurseTool1 != 0)
                    progress.UIdNurseTool1 = data.UIdNurseTool1;
                if (data.UIdNurseTool2 != null && data.UIdNurseTool2 != 0)
                    progress.UIdNurseTool2 = data.UIdNurseTool2;
                if (data.UIdNurseOutRun1 != null && data.UIdNurseOutRun1 != 0)
                    progress.UIdNurseOutRun1 = data.UIdNurseOutRun1;
                if (data.UIdNurseOutRun2 != null && data.UIdNurseOutRun2 != 0)
                    progress.UIdNurseOutRun2 = data.UIdNurseOutRun2;
                if (data.UIdNurseOutRun3 != null && data.UIdNurseOutRun3 != 0)
                    progress.UIdNurseOutRun3 = data.UIdNurseOutRun3;
                if (data.UIdNurseOutRun4 != null && data.UIdNurseOutRun4 != 0)
                    progress.UIdNurseOutRun4 = data.UIdNurseOutRun4;
                if (data.UIdNurseOutRun5 != null && data.UIdNurseOutRun5 != 0)
                    progress.UIdNurseOutRun5 = data.UIdNurseOutRun5;
                if (data.UIdNurseOutRun6 != null && data.UIdNurseOutRun6 != 0)
                    progress.UIdNurseOutRun6 = data.UIdNurseOutRun6;
                if (!string.IsNullOrEmpty(data.SurgeryDescription))
                    progress.SurgeryDescription = data.SurgeryDescription;

                if (data.UIdMainAnesthDoctor != null && data.UIdMainAnesthDoctor != 0)
                    progress.UIdMainAnesthDoctor = data.UIdMainAnesthDoctor;
                if (data.UIdSubAnesthDoctor != null && data.UIdSubAnesthDoctor != 0)
                    progress.UIdSubAnesthDoctor = data.UIdSubAnesthDoctor;
                if (data.UIdAnesthNurse1 != null && data.UIdAnesthNurse1 != 0)
                    progress.UIdAnesthNurse1 = data.UIdAnesthNurse1;
                if (data.UIdAnesthNurse2 != null && data.UIdAnesthNurse2 != 0)
                    progress.UIdAnesthNurse2 = data.UIdAnesthNurse2;
                if (data.UIdAnesthNurseRecovery != null && data.UIdAnesthNurseRecovery != 0)
                    progress.UIdAnesthNurseRecovery = data.UIdAnesthNurseRecovery;
                progress.OrderID = data.OrderID;
                progress.ChargeDetailId = data.ChargeDetailId;
                //vutv7
                progress.ChargeDate = data.ChargeDate;
                progress.ChargeBy = data.ChargeBy;
                if (data.State == (int)ORProgressStateEnum.ApproveAnesthManager)
                {
                    progress.DepartmentCode = data.DepartmentCode;
                    progress.AdmissionWard = string.IsNullOrEmpty(progress.AdmissionWard) ? data.AdmissionWard : progress.AdmissionWard;
                }
                if (!string.IsNullOrEmpty(data.AnesthDescription))
                    progress.AnesthDescription = data.AnesthDescription;
                progress.State = data.State;
                //vutv7
                if (data.UIdKTVSubSurgery != null && data.UIdKTVSubSurgery != 0)
                    progress.UIdKTVSubSurgery = data.UIdKTVSubSurgery;
                if (data.UIdKTVDiagnose != null && data.UIdKTVDiagnose != 0)
                    progress.UIdKTVDiagnose = data.UIdKTVDiagnose;
                if (data.UIdKTVCEC != null && data.UIdKTVCEC != 0)
                    progress.UIdKTVCEC = data.UIdKTVCEC;
                if (data.UIdDoctorDiagnose != null && data.UIdDoctorDiagnose != 0)
                    progress.UIdDoctorDiagnose = data.UIdDoctorDiagnose;
                if (data.UIdDoctorNewBorn != null && data.UIdDoctorNewBorn != 0)
                    progress.UIdDoctorNewBorn = data.UIdDoctorNewBorn;
                if (data.UIdMidwives != null && data.UIdMidwives != 0)
                    progress.UIdMidwives = data.UIdMidwives;
                if (data.UIdSubAnesthDoctor2 != null && data.UIdSubAnesthDoctor2 != 0)
                    progress.UIdSubAnesthDoctor2 = data.UIdSubAnesthDoctor2;
                if (data.UIdAnesthesiologist != null && data.UIdAnesthesiologist != 0)
                    progress.UIdAnesthesiologist = data.UIdAnesthesiologist;
            }
            progress.UpdatedBy = userId;
            progress.UpdatedDate = DateTime.Now;
            DbContext.SaveChanges();
            InsertUserTracking(Newtonsoft.Json.JsonConvert.SerializeObject(data), userId, progress.Id, progress.State, 2);
            return new CUDReturnMessage() { Id = (int)progress.Id, Message = "cập nhật hành công" };
        }

        #region Link Redirect
        public ORLinkActive CheckOperationLink(Guid GuidCode)
        {
            var linkContent = DbContext.OROperationLinks.FirstOrDefault(r => (r.IsDeleted == false && r.IsActive == false && r.Guid == GuidCode && r.LimitDate >= DateTime.Now));
            if (linkContent == null)
                return new ORLinkActive()
                {
                    GuidCode = GuidCode,
                    IsValidate = false,
                    ReDirectUrl = string.Empty
                };
            return new ORLinkActive()
            {
                GuidCode = GuidCode,
                IsValidate = true,
                ReDirectUrl = linkContent.Code
            };
        }

        public CUDReturnMessage CUDOperationLink(ORLinkContract data, int userId)
        {
            OROperationLink linkContent;

            if (data == null)
            {
                return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
            }
            linkContent = DbContext.OROperationLinks.FirstOrDefault(d => (d.Guid.Equals(data.GuidCode)));
            if (data.Id == 0 && linkContent == null) //Innsert
            {
                #region OR
                linkContent = new OROperationLink()
                {
                    Guid = data.GuidCode,
                    Code = data.Code,
                    LimitDate = data.LimitDate,
                    IsActive = false,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };
                #endregion
                DbContext.OROperationLinks.Add(linkContent);
                DbContext.SaveChanges();
                return new CUDReturnMessage() { Id = (int)linkContent.Id, Message = "Tạo mới hành công" };
            }
            if (linkContent.IsActive == false)
            {
                linkContent.dtActive = DateTime.Now;
                linkContent.IsActive = data.IsActive;
                linkContent.IpActive = data.IpActive;
                DbContext.SaveChanges();
            }
            return new CUDReturnMessage() { Id = (int)linkContent.Id, Message = "cập nhật hành công" };

        }
        public IQueryable<ORAnesthProgress> SearchAnesthInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId)
        {
            var query = DbContext.ORAnesthProgresses.AsQueryable();
            if (State > 0)
                return query = query.Where(c => c.State == State);
            if (!string.IsNullOrEmpty(kw))
                query = query.Where(c => c.PId.Contains(kw));
            if (HpServiceId > 0)
                query = query.Where(c => c.HpServiceId == HpServiceId);
            if (ORRoomId > 0)
                query = query.Where(c => c.ORRoomId == ORRoomId);
            return query;
        }
        public ORMappingEkipContract GetInfoEkip(long Id)
        {
            var c = DbContext.ORMappingEkips.FirstOrDefault(r => (r.Id == Id));
            if (c == null)
                return null;

            return new ORMappingEkipContract()
            {
                Id = c.Id,
                HospitalCode = c.HospitalCode,
                UId = c.UId,
                UserName = (c.ORUserInfo != null ? c.ORUserInfo.Name : string.Empty),
                Email = (c.ORUserInfo.Email != null ? c.ORUserInfo.Email : string.Empty),
                Phone = (c.ORUserInfo.Phone != null ? c.ORUserInfo.Phone : string.Empty),
                //PositionId = (c.ORUserInfo.PositionId > 0 ? c.ORUserInfo.PositionId : 0),
                //PositionName = (c.ORUserInfo.PositionId > 0 ? c.ORUserInfo.ORPositionType.Name : string.Empty),
                TypePageId = c.TypePageId
            };
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo Site
        /// </summary>
        /// <param name="data"></param>
        /// <param name="CurrentUserId"></param>
        /// <returns></returns>
        public CUDReturnMessage SaveCUDEkip(ORMappingEkipContract data, int CurrentUserId)
        {
            ORMappingEkip attr;

            if (data == null)
            {
                return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
            }

            //Check Role permission for location data
            #region Check Role permission for location data
            if (data.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check Role permission for location data

            attr = DbContext.ORMappingEkips.FirstOrDefault(d => d.Id.Equals(data.Id));
            if (data.Id == 0 || attr == null) //Innsert
            {
                #region attr
                attr = new ORMappingEkip()
                {
                    HospitalCode = data.HospitalCode,
                    UId = data.UId,
                    TypePageId = data.TypePageId,
                    ORAnesthProgessId = data.ORAnesthProgessId,
                    CreatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };
                #endregion
                DbContext.ORMappingEkips.Add(attr);
                DbContext.SaveChanges();
                return new CUDReturnMessage() { Id = (int)attr.Id, Message = "Tạo mới hành công" };
            }
            else
            {
                if (attr == null)
                    return new CUDReturnMessage() { Id = 0, Message = "Lỗi trong quá trình xử lý" };
                attr.HospitalCode = data.HospitalCode;
                attr.ORAnesthProgessId = data.ORAnesthProgessId;
                attr.UId = data.UId;
                attr.TypePageId = data.TypePageId;
                attr.UpdateBy = CurrentUserId;
                attr.UpdateDate = DateTime.Now;
                attr.IsDeleted = false;
                DbContext.SaveChanges();
                return new CUDReturnMessage() { Id = (int)attr.Id, Message = "cập nhật hành công" };
            }
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo Site
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteAnesthEkip(long Id, int userId)
        {
            ORMappingEkip attr;
            if (Id == 0)
            {
                return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
            }
            attr = DbContext.ORMappingEkips.FirstOrDefault(d => d.Id.Equals(Id));

            if (attr == null)
                return new CUDReturnMessage() { Id = 0, Message = "Không tìm thấy thông tin này" };
            #region Check access denied on Data
            if (attr.CreatedBy != MemberExtInfo.UserId && !MemberExtInfo.IsSuperAdmin && !MemberExtInfo.IsManageAdminSurgery && !MemberExtInfo.IsManagAnes)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check access denied on Data
            //Check Role permission for location data
            #region Check Role permission for location data
            if (attr.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check Role permission for location data
            attr.IsDeleted = true;
            attr.UpdateBy = userId;
            attr.UpdateDate = DateTime.Now;

            DbContext.SaveChanges();


            return new CUDReturnMessage() { Id = (int)attr.Id, Message = "Xóa thông tin thành công" };
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo Site
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteSurgery(long Id, int userId)
        {
            ORAnesthProgress attr;
            if (Id == 0)
            {
                return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
            }
            attr = DbContext.ORAnesthProgresses.FirstOrDefault(d => d.Id == Id);

            if (attr == null)
                return new CUDReturnMessage() { Id = 0, Message = "Không tìm thấy thông tin này" };
            #region Check access denied on Data
            if (attr.CreatedBy != MemberExtInfo.UserId && !MemberExtInfo.IsSuperAdmin && !MemberExtInfo.IsManageAdminSurgery && !MemberExtInfo.IsManagAnes)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check access denied on Data
            //Check Role permission for location data
            #region Check Role permission for location data
            if (attr.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check Role permission for location data
            attr.IsDeleted = true;
            attr.UpdatedBy = userId;
            attr.UpdatedDate = DateTime.Now;

            DbContext.SaveChanges();
            return new CUDReturnMessage() { Id = (int)attr.Id, Message = "Xóa thông tin thành công", SystemMessage = "Xóa thông tin thành công" };
        }
        public CUDReturnMessage RollbackDeleteSurgery(long Id, int userId)
        {
            ORAnesthProgress attr;
            if (Id == 0)
            {
                return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
            }
            attr = DbContext.ORAnesthProgresses.FirstOrDefault(d => d.Id == Id);

            if (attr == null)
                return new CUDReturnMessage() { Id = 0, Message = "Không tìm thấy thông tin này" };
            #region Check access denied on Data
            if (attr.CreatedBy != MemberExtInfo.UserId && !MemberExtInfo.IsSuperAdmin && !MemberExtInfo.IsManageAdminSurgery && !MemberExtInfo.IsManagAnes)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check access denied on Data
            //Check Role permission for location data
            #region Check Role permission for location data
            if (attr.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check Role permission for location data
            attr.IsDeleted = false;
            attr.UpdatedBy = userId;
            attr.UpdatedDate = DateTime.Now;

            DbContext.SaveChanges();


            return new CUDReturnMessage() { Id = (int)attr.Id, Message = "Rollback Xóa thông tin thành công", SystemMessage = "Rollback Xóa thông tin thành công" };
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo Site
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="value"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage UpdateSorting(long Id, int value, int userId)
        {
            var setting = DbContext.ORAnesthProgresses.SingleOrDefault(r => r.Id == Id && r.IsDeleted != true);

            if (setting != null)
            {
                #region Check access denied on Data
                if (setting.CreatedBy != MemberExtInfo.UserId && !MemberExtInfo.IsSuperAdmin && !MemberExtInfo.IsManageAdminSurgery && !MemberExtInfo.IsManagAnes)
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                #endregion .Check access denied on Data
                //Check Role permission for location data
                #region Check Role permission for location data
                if (setting.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                #endregion .Check Role permission for location data
                setting.Sorting = value;
                setting.UpdatedBy = userId;
                setting.UpdatedDate = DateTime.Now;

                int affectedRow = DbContext.SaveChanges();
                if (affectedRow > 0)
                {
                    InsertUserTracking(value.ToString(), userId, setting.Id, setting.State, 3);
                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.Successed,
                        Message = ""
                    };
                }
                else
                {
                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.NoChanged,
                        Message = ""
                    };
                }
            }
            else
            {
                return new CUDReturnMessage()
                {
                    Id = (int)ResponseCode.KeyNotExist,
                    Message = ""
                };
            }
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo Site
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="state"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage ChangeState(long Id, int state, int userId)
        {
            var setting = DbContext.ORAnesthProgresses.SingleOrDefault(r => r.Id == Id && r.IsDeleted != true);
            if (setting != null)
            {
                //Check Role permission for location data
                #region Check Role permission for location data
                if (setting.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                #endregion .Check Role permission for location data
                //if (state >= (int)ORProgressStateEnum.ApproveAnesthManager)
                if (state >= (int)ORProgressStateEnum.ApproveSurgeryManager)
                    setting.State = state;
                setting.UpdatedBy = userId;
                setting.UpdatedDate = DateTime.Now;

                int affectedRow = DbContext.SaveChanges();
                if (affectedRow > 0)
                {
                    InsertUserTracking(state.ToString(), userId, (int)Id, state, 2);

                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.Successed,
                        Message = ""
                    };
                }
                else
                {
                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.NoChanged,
                        Message = ""
                    };
                }
            }
            else
            {
                return new CUDReturnMessage()
                {
                    Id = (int)ResponseCode.KeyNotExist,
                    Message = ""
                };
            }
        }
        public CUDReturnMessage UpdateReceiptSurgeryInfo(int pgId, string strKey, string strValue, int userId)
        {
            var entity = DbContext.ORAnesthProgresses.SingleOrDefault(r => r.Id == pgId && r.IsDeleted != true);
            if (entity != null)
            {
                //Check Role permission for location data
                #region Check Role permission for location data
                if (entity.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                #endregion .Check Role permission for location data
                switch (strKey)
                {
                    case "print_AdmissionWard":
                        entity.AdmissionWard = strValue;
                        break;
                    case "print_NoFoodFrom":

                        break;
                    case "print_NoDrinkFrom":

                        break;
                    case "print_AdvanceAmount":

                        break;
                }
                entity.UpdatedBy = userId;
                entity.UpdatedDate = DateTime.Now;

                int affectedRow = DbContext.SaveChanges();
                if (affectedRow > 0)
                {
                    InsertUserTracking(string.Format("Update Receipt surgery info: [Key={0}]; [Value={1}]", strKey, strValue), userId, pgId, entity.State, 2);
                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.Successed,
                        Message = ""
                    };
                }
                else
                {
                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.NoChanged,
                        Message = ""
                    };
                }
            }
            else
            {
                return new CUDReturnMessage()
                {
                    Id = (int)ResponseCode.KeyNotExist,
                    Message = ""
                };
            }
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo Site
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage CUDHpService(HpServiceSite data, int userId)
        {
            try
            {
                HpService attr;
                if (data == null)
                {
                    return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
                }
                //Check Have role on location
                var CheckExistLoc = data.listSites.Select(x1 => x1.Id).ToList().Except(MemberExtInfo.Locations.Select(x => x.NameEN).ToList()).ToList();
                if (CheckExistLoc.Any())
                {
                    //Return Invalid location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }

                attr = DbContext.HpServices.FirstOrDefault(d => d.Id.Equals(data.Id) || (d.Oh_Code.Equals(data.Oh_Code) && d.SourceClientId == data.SourceClientId));
                //if (data.Id == 0 || attr == null) //Innsert
                if (attr == null) //Innsert
                {
                    return CreateHpService(data, userId);
                }
                else
                {
                    data.Id = attr.Id;
                    return UpdateHpService(data, userId);

                }
            }
            catch (Exception ex)
            {
                return new CUDReturnMessage() { Id = 0, Message = "Có lỗi trong quá trình xử lý" };
            }

        }
        public CUDReturnMessage UpdateHpService(HpServiceSite data, int userId)
        {
            var sv = DbContext.HpServices.Find(data.Id);
            sv.Name = data.Name;
            sv.Oh_Code = data.Oh_Code;
            sv.CleaningTime = data.CleaningTime;
            sv.PreparationTime = data.PreparationTime;
            sv.AnesthesiaTime = data.AnesthesiaTime;
            sv.OtherTime = data.OtherTime;
            sv.UpdatedBy = userId;
            sv.UpdatedDate = DateTime.Now;
            sv.SourceClientId = data.SourceClientId;
            sv.Type = data.Type;
            sv.Description = data.Description;
            sv.IdMapping = data.IdMapping ?? "0";
            UpdateSiteService(sv, data.listSites.Select(c => c.HospitalCode).Distinct().ToList(), userId);
            DbContext.SaveChanges();
            return new CUDReturnMessage() { Id = (int)sv.Id, Message = "lưu thông tin hành công" };
        }
        public CUDReturnMessage CreateHpService(HpServiceSite data, int userId)
        {
            Models.HpService sv = new Models.HpService
            {
                Name = data.Name
               ,
                Oh_Code = data.Oh_Code
               ,
                CleaningTime = data.CleaningTime
               ,
                PreparationTime = data.PreparationTime
               ,
                AnesthesiaTime = data.AnesthesiaTime
               ,
                OtherTime = data.OtherTime
               ,
                CreatedBy = userId
               ,
                CreatedDate = DateTime.Now
               ,
                UpdatedBy = userId
               ,
                UpdatedDate = DateTime.Now
               ,
                SourceClientId = data.SourceClientId
               ,
                Type = data.Type
               ,
                Description = data.Description
               ,
                IdMapping = data.IdMapping ?? "0"
               ,
                IsDeleted = false
            };
            DbContext.HpServices.Add(sv);
            DbContext.SaveChanges();
            foreach (var item in data.listSites)
            {
                Models.ORMappingService site = new Models.ORMappingService
                {
                    ObjectId = sv.Id,
                    HospitalCode = item.HospitalCode,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = userId,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now
                };
                DbContext.ORMappingServices.Add(site);
            };
            DbContext.SaveChanges();



            return new CUDReturnMessage() { Id = (int)sv.Id, Message = "lưu thông tin hành công" };
        }

        private void InsertUserTracking(string v, int userId)
        {
            throw new NotImplementedException();
        }

        private void UpdateSiteService(HpService sv, List<string> newSiteIds, int userId)
        {
            var listCurrentSites = DbContext.ORMappingServices.Where(r => r.IsDeleted == false && r.ObjectId == sv.Id).Select(c => c.HospitalCode).ToList();
            // đánh dấu xóa site
            var deleteSites = DbContext.ORMappingServices.Where(r => r.IsDeleted == false && r.ObjectId == sv.Id && newSiteIds.Contains(r.HospitalCode) == false);
            if (deleteSites.Any())
            {
                foreach (var item in deleteSites)
                {
                    item.IsDeleted = true;
                    item.UpdateBy = uid;
                    item.UpdateDate = DateTime.Now;
                }
            }
            // bỏ đánh dấu xóa site
            var restoreSites = DbContext.ORMappingServices.Where(r => r.IsDeleted == true && r.ObjectId == sv.Id && newSiteIds.Contains(r.HospitalCode));
            if (restoreSites.Any())
            {
                foreach (var item in restoreSites)
                {
                    item.IsDeleted = false;
                    item.UpdateBy = uid;
                    item.UpdateDate = DateTime.Now;
                }
            }

            // thêm site  mới
            var insertSites = newSiteIds.Where(r => listCurrentSites.Any(ur => ur == r) == false);
            if (insertSites.Any())
            {
                foreach (var item in insertSites)
                {
                    Models.ORMappingService site = new Models.ORMappingService
                    {
                        ObjectId = sv.Id,
                        HospitalCode = item,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        CreatedBy = userId,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };
                    DbContext.ORMappingServices.Add(site);
                }
            }
        }


        #region worker mail
        public List<ORNotifyMail> GetListORAnesthByMail(int quantity)
        {
            #region "site info "
            Dictionary<string, string> siteInfo = new Dictionary<string, string>();
            string strConfigReceiver = ConfigurationManager.AppSettings["ListEmailReceiver"];
            bool hasValueInConfig = false;
            if (!string.IsNullOrEmpty(strConfigReceiver))
            {
                //Set from config
                string[] listConfigBySite = strConfigReceiver.Split('-');
                if (listConfigBySite != null && listConfigBySite.Length > 0)
                {
                    hasValueInConfig = true;
                    foreach (var item in listConfigBySite)
                    {
                        string[] valueInSite = item.Replace("[", "").Replace("]", "").Split('|');
                        if (valueInSite != null && valueInSite.Length == 2)
                        {
                            siteInfo.Add(valueInSite[0], valueInSite[1]);
                        }
                    }
                }
            }
            if (!hasValueInConfig)
            {
                //Set fixed from code
                siteInfo.Add("HHL", "pmhhl@vinmec.com");
                siteInfo.Add("HCP", "pmhcp@vinmec.com");
                siteInfo.Add("HDN", "pmhdn@vinmec.com");
                siteInfo.Add("HHP", "pmhhp@vinmec.com");
                siteInfo.Add("HNT", "pmhnt@vinmec.com");
                siteInfo.Add("HPQ", "pmhpq@vinmec.com");
            }
            #endregion

            DateTime dtNow = DateTime.Now;
            // DateTime dtStartOR = DateTime.Now.AddDays(-30);//chi test 
            DateTime dtStartOR = Extension.dtStartOR(dtNow);
            DateTime dtEndOR = Extension.dtEndOR(dtNow);
            var query = DbContext.ORAnesthProgresses.AsQueryable();
            query = query.Where(c => c.IsDeleted == false
                                        && (c.IsProcess == false)
                                        && c.CreatedDate >= dtStartOR
                                        && c.CreatedDate <= dtEndOR
                                        && (c.IsSync == false)
                                        && c.HospitalCode != ""
                                );
            var data = quantity == -1 ? query.ToList() : query.Take(quantity).ToList();
            data.ForEach(c => { c.IsProcess = true; c.DateSync = dtNow; });
            DbContext.SaveChanges();
            var listData = (from or in data
                            group or by new
                            {
                                HospitalCode = or.HospitalCode,
                                HospitalName = or.HospitalName,
                                Email = siteInfo.FirstOrDefault(c => c.Key.Equals(or.HospitalCode)).Value
                            } into newGroup
                            select new ORNotifyMail
                            {
                                HospitalCode = newGroup.Key.HospitalCode,
                                SiteName = newGroup.Key.HospitalName,
                                Email = newGroup.Key.Email,
                                listData = newGroup.Select(c => CloneData(c)).ToList()
                            }).ToList();
            data.ForEach(c => { c.IsProcess = false; c.IsSync = true; c.DateSync = DateTime.Now; });
            DbContext.SaveChanges();
            return listData;
            #endregion
        }
        private ORAnesthProgressContract CloneData(ORAnesthProgress data)
        {

            var result = new ORAnesthProgressContract()
            {
                VisitCode = data.VisitCode,
                PId = data.PId,
                Id = data.Id,
                HospitalCode = data.HospitalCode,
                HospitalName = data.HospitalName,
                ORRoomId = data.ORRoomId,
                HpServiceId = data.HpServiceId,
                dtStart = data.dtStart,
                dtEnd = data.dtEnd,
                dtOperation = data.dtOperation,
                TimeAnesth = data.TimeAnesth,
                RegDescription = data.RegDescription,
                State = data.State,
                UIdPTVMain = data.UIdPTVMain ?? 0,
                UIdPTVSub1 = data.UIdPTVSub1 ?? 0,
                UIdPTVSub2 = data.UIdPTVSub2 ?? 0,
                UIdPTVSub3 = data.UIdPTVSub3 ?? 0,
                UIdPTVSub4 = data.UIdPTVSub4 ?? 0,
                UIdPTVSub5 = data.UIdPTVSub5 ?? 0,
                UIdPTVSub6 = data.UIdPTVSub6 ?? 0,
                UIdPTVSub7 = data.UIdPTVSub7 ?? 0,
                UIdPTVSub8 = data.UIdPTVSub8 ?? 0,
                UIdCECDoctor = data.UIdCECDoctor ?? 0,
                UIdNurseTool1 = data.UIdNurseTool1 ?? 0,
                UIdNurseTool2 = data.UIdNurseTool2 ?? 0,
                UIdNurseOutRun1 = data.UIdNurseOutRun1 ?? 0,
                UIdNurseOutRun2 = data.UIdNurseOutRun2 ?? 0,
                UIdNurseOutRun3 = data.UIdNurseOutRun3 ?? 0,
                UIdNurseOutRun4 = data.UIdNurseOutRun4 ?? 0,
                UIdNurseOutRun5 = data.UIdNurseOutRun5 ?? 0,
                UIdNurseOutRun6 = data.UIdNurseOutRun6 ?? 0,
                UIdAnesthNurseRecovery = data.UIdAnesthNurseRecovery ?? 0,
                SurgeryDescription = data.SurgeryDescription,
                UIdMainAnesthDoctor = data.UIdMainAnesthDoctor ?? 0,
                UIdSubAnesthDoctor = data.UIdSubAnesthDoctor ?? 0,
                UIdAnesthNurse1 = data.UIdAnesthNurse1 ?? 0,
                UIdAnesthNurse2 = data.UIdAnesthNurse2 ?? 0,
                AnesthDescription = data.AnesthDescription,
                NameProject = data.NameProject,
                CreatedBy = data.CreatedBy ?? 0,
                ORRoomName = data.ORRoom.Name,
            };
            if (data.CreatedBy > 0)
            {
                var ORUserInfo = DbContext.ORUserInfoes.FirstOrDefault(d => d.Id == data.CreatedBy);
                if (ORUserInfo != null)
                {
                    result.NameCreatedBy = ORUserInfo.Name;
                    result.EmailCreatedBy = ORUserInfo.Email;
                    result.PhoneCreatedBy = ORUserInfo.Phone;
                    result.PositionCreatedBy = "Bác sĩ đăng ký mổ";
                }
                if (data.HpServiceId > 0)
                {
                    var dataService = DbContext.HpServices.FirstOrDefault(d => d.Id == data.HpServiceId);
                    if (dataService != null)
                    {
                        result.HpServiceName = dataService.Name;
                    }
                }
            }
            return result;
        }

        public void ExecuteBlock(Action blockFunction)
        {
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    blockFunction();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public CUDReturnMessage CheckExistPositionByScheduler(long Id, int userId)
        {
            var rsCheckExist = ExecStoredProc<ExecuteQueryIntIdResult>("Core_CheckExistPositionByScheduler", new object[] { Id, userId }).FirstOrDefault();
            if (rsCheckExist != null && rsCheckExist.IsSuccess)
                return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại " };
            return new CUDReturnMessage() { Id = 1, Message = "" };
        }
        public ORUserInfoContract GetORUserInfo(int userId)
        {
            var userOrInfo = DbContext.ORUserInfoes.FirstOrDefault(c => c.Id == userId);
            if (userOrInfo != null) return new ORUserInfoContract
            {
                Id = userOrInfo.Id,
                Name = userOrInfo.Name,
                Email = userOrInfo.Email,
                //PositionId = userOrInfo.PositionId,
                //PositionName = EnumExtension.GetDescription((ORPositionEnum)userOrInfo.PositionId),
                Phone = userOrInfo.Phone,
                IsDeleted = userOrInfo.IsDeleted ?? false,
            };
            var userData = DbContext.AdminUsers.FirstOrDefault(c => c.UId == userId);
            if (userData != null) return new ORUserInfoContract()
            {
                Id = userData.UId,
                Name = userData.Name,
                Email = userData.Email,
                PositionId = 0,
                PositionName = "",
                Phone = userData.PhoneNumber,
                IsDeleted = false,
            };
            return null;
        }
        //linhht
        public ORUserInfoContract GetORUserInfo(string userName)
        {
            var userOrInfo = DbContext.ORUserInfoes.FirstOrDefault(c => c.Email.Contains(userName));
            if (userOrInfo != null) return new ORUserInfoContract
            {
                Id = userOrInfo.Id,
                Name = userOrInfo.Name,
                Email = userOrInfo.Email,
                //PositionId = userOrInfo.PositionId,
                //PositionName = EnumExtension.GetDescription((ORPositionEnum)userOrInfo.PositionId),
                Phone = userOrInfo.Phone,
                IsDeleted = userOrInfo.IsDeleted ?? false,
            };
            var userData = DbContext.AdminUsers.FirstOrDefault(c => c.Username == userName);
            if (userData != null) return new ORUserInfoContract()
            {
                Id = userData.UId,
                Name = userData.Name,
                Email = userData.Email,
                PositionId = 0,
                PositionName = "",
                Phone = userData.PhoneNumber,
                IsDeleted = false,
            };
            return null;
        }
        #endregion

        #region OR tracking 
        public IQueryable<ORTracking> ListORTracking(DateTime fromDate, DateTime toDate, string keyword, string HospitalCode)
        {
            var query = DbContext.ORTrackings.Where(r => r.CreatedDate >= fromDate && r.CreatedDate <= toDate && r.ORAnesthProgress.HospitalCode.Equals(HospitalCode));
            if (string.IsNullOrEmpty(keyword) == false)
            {
                query = query.Where(r => (r.ContentTracking.Contains(keyword) || r.AdminUser.Name.Contains(keyword) || r.AdminUser.Email.Contains(keyword) || r.ORId.ToString().Equals(keyword)));
            }
            return query;
        }
        public void InsertUserTracking(string log, int userId, int ORId, int state, int type)
        {
            try
            {
                Models.ORTracking track = new Models.ORTracking
                {
                    UserId = userId,
                    ContentTracking = log,
                    CreatedDate = DateTime.Now,
                    TypeId = type,
                    ORId = ORId,
                    State = state
                };

                DbContext.ORTrackings.Add(track);
                DbContext.SaveChanges();
            }
            catch (Exception e)
            {
            }
        }



        #endregion

        public List<HpServiceContract> SearchHpServiceExt(string siteId, string kw, int page, int pageSize, int siteType)
        {
            List<HpServiceContract> lstService = new List<HpServiceContract>();
            var listPublicActions = (from s in DbContext.HpServices
                                     join ms in DbContext.ORMappingServices on s.Id equals ms.ObjectId
                                     where s.IsDeleted == false
                                     && ms.IsDeleted == false
                                     && ms.HospitalCode.Equals(siteId)
                                     && s.Type == siteType
                                     && (s.Name.Contains(kw) || s.Oh_Code.Contains(kw))
                                     select new HpServiceContract
                                     {
                                         Id = s.Id,
                                         Oh_Code = s.Oh_Code,
                                         Name = s.Name,
                                         CleaningTime = s.CleaningTime ?? 0,
                                         PreparationTime = s.PreparationTime ?? 0,
                                         AnesthesiaTime = s.AnesthesiaTime ?? 0,
                                         OtherTime = s.OtherTime ?? 0,
                                         Description = s.Description,
                                         IdMapping = s.IdMapping,
                                         Sort = s.Sort ?? 100,
                                         Type = s.Type ?? 2
                                     }).OrderBy(x => x.Name).ToList();
            return listPublicActions;
        }
        public HpServiceContract GetServiceById(int serviceId)
        {
            return (from s in DbContext.HpServices
                    where s.IsDeleted == false
                    && s.Id == serviceId
                    select new HpServiceContract
                    {
                        Id = s.Id,
                        Oh_Code = s.Oh_Code,
                        Name = s.Name,
                        CleaningTime = s.CleaningTime ?? 0,
                        PreparationTime = s.PreparationTime ?? 0,
                        AnesthesiaTime = s.AnesthesiaTime ?? 0,
                        OtherTime = s.OtherTime ?? 0,
                        Description = s.Description,
                        IdMapping = s.IdMapping,
                        Sort = s.Sort ?? 100,
                        Type = s.Type ?? 2
                    }).SingleOrDefault();
        }

        public CUDReturnMessage InsertNewService(PatientService data, int userId)
        {
            try
            {
                HpService attr;
                if (data == null)
                {
                    return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
                }

                var listSites = DbContext.HospitalSites.Where(s => s.AreaId == 0);
                HpService sv = new HpService
                {
                    Name = data.ItemName,
                    Oh_Code = data.ItemCode,
                    CleaningTime = 30,
                    PreparationTime = 0,
                    AnesthesiaTime = 0,
                    OtherTime = 0,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = userId,
                    UpdatedDate = DateTime.Now,
                    SourceClientId = (int)SourceClientEnum.Oh,
                    Type = 2,
                    Description = string.Empty,
                    IdMapping = "0",
                    IsDeleted = false
                };

                attr = DbContext.HpServices.FirstOrDefault(d => d.Oh_Code.Equals(sv.Oh_Code));
                if (attr == null)
                {
                    DbContext.HpServices.Add(sv);
                    DbContext.SaveChanges();
                    foreach (var item in listSites)
                    {
                        ORMappingService site = new ORMappingService
                        {
                            ObjectId = sv.Id,
                            HospitalCode = item.Id,
                            TypeMappingId = 1,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                            CreatedBy = userId,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };
                        DbContext.ORMappingServices.Add(site);
                    };
                    DbContext.SaveChanges();
                }
                return new CUDReturnMessage() { Id = (int)sv.Id, Message = "lưu thông tin hành công" };
            }
            catch (Exception ex)
            {
                return new CUDReturnMessage() { Id = 0, Message = "Có lỗi trong quá trình xử lý" };
            }
        }

        public bool IsOrderExisted(string orderId, string chargeId, string siteId)
        {
            return DbContext.ORAnesthProgresses.Any(s => s.HospitalCode == siteId /*11-07-2022:Phubq bổ sung thêm kđ && s.State!=(int)ORProgressStateEnum.Registor*/&& !Constant.ListStateNotCheckChargeId.Contains(s.State) && ((s.OrderID.Equals(orderId) && string.IsNullOrEmpty(s.ChargeDetailId)) || (!string.IsNullOrEmpty(chargeId) && !string.IsNullOrEmpty(s.ChargeDetailId) && s.ChargeDetailId == chargeId)) && s.IsDeleted.HasValue && !s.IsDeleted.Value);
        }
        public bool CheckChargeExistInApproved(int pgId, string orderId, string chargeId, string siteId)
        {
            return DbContext.ORAnesthProgresses.Any(s => s.Id != pgId && s.HospitalCode == siteId /*11-07-2022:Phubq bổ sung thêm kđ && s.State!=(int)ORProgressStateEnum.Registor*/&& !Constant.ListStateNotCheckChargeId.Contains(s.State) && ((s.OrderID.Equals(orderId) && string.IsNullOrEmpty(s.ChargeDetailId)) || (!string.IsNullOrEmpty(chargeId) && !string.IsNullOrEmpty(s.ChargeDetailId) && s.ChargeDetailId == chargeId)) && !Constant.ListStateNotCheckChargeId.Contains(s.State) && s.IsDeleted.HasValue && !s.IsDeleted.Value);
        }
        public HpServiceContract GetServiceByCode(string code)
        {
            return (from s in DbContext.HpServices
                    where s.IsDeleted == false
                    && s.Oh_Code == code
                    select new HpServiceContract
                    {
                        Id = s.Id,
                        Oh_Code = s.Oh_Code,
                        Name = s.Name,
                        CleaningTime = s.CleaningTime ?? 0,
                        PreparationTime = s.PreparationTime ?? 0,
                        AnesthesiaTime = s.AnesthesiaTime ?? 0,
                        OtherTime = s.OtherTime ?? 0,
                        Description = s.Description,
                        IdMapping = s.IdMapping,
                        Sort = s.Sort ?? 100,
                        Type = s.Type ?? 2
                    }).SingleOrDefault();
        }

        public List<PatientService> RemoveExistingServices(List<PatientService> list, string siteId)
        {
            var result = new List<PatientService>();
            foreach (var item in list)
            {
                var db = DbContext.ORAnesthProgresses.FirstOrDefault(s => s.HospitalCode == siteId /*11-07-2022:Phubq bổ sung thêm kđ && s.State!=(int)ORProgressStateEnum.Registor*/&& !Constant.ListStateNotCheckChargeId.Contains(s.State) && ((s.OrderID == item.OrderID && string.IsNullOrEmpty(s.ChargeDetailId)) || (!string.IsNullOrEmpty(s.ChargeDetailId) && s.ChargeDetailId == item.ChargeDetailId)) && s.IsDeleted.HasValue && !s.IsDeleted.Value);
                if (db == null)
                {
                    //get HPService Info
                    var svEntity = DbContext.HpServices.FirstOrDefault(x => x.Oh_Code == item.ItemCode && x.IsDeleted != true);
                    item.ServiceType = svEntity?.ServiceType != null ? svEntity.ServiceType.Value : 1;
                    result.Add(item);
                }
            }
            return result;
        }
        #region linhht update thông tin bệnh nhân
        public async Task<bool> UpdatePatientInfo(ReportPatient patient, int userId)
        {
            var result = await DbContext.ReportPatients.FirstOrDefaultAsync(x => x.Id == patient.Id);
            if (result != null)
            {
                result.Address = patient.Address;
                result.Age = patient.Age;
                result.Sex = patient.Sex;
                result.National = patient.National;
                result.HoTen = patient.HoTen;
                result.NgaySinh = patient.NgaySinh;
                result.Phone = patient.Phone;
                result.UpdatedBy = userId;
                result.UpdatedDate = DateTime.Now;
                DbContext.Entry(result).State = System.Data.Entity.EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }
            return true;
        }

        #endregion
    }
}

