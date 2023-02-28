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

namespace DataAccess.DAO
{
    public interface IORDataAccess_Backup
    {
        HospitalSiteContract GetHospitalSite(string HospitalCode);
        IEnumerable<Models.ORRoom> GetListRoom(string HospitalCode, int sourceClientId, string IsDisplay);
        List<HpServiceContract> GetListHpServices(string HospitalCode, int sourceClientId);
        List<ORUserInfoContract> GetListORUsers(string HospitalCode, int PositionId, int sourceClientId);
        List<ORUserInfoContract> GetListORUsers(string HospitalCode, List<int> lisitPositionIds, int sourceClientId, DateTime dtStart, DateTime dtEnd);
        ORAnesthProgressContract GetORAnesthProgress(string kw, int typeSearch);
        CUDReturnMessage SaveORRegistorByJson(ORAnesthProgressContract contract, int CurrentUserId);
        CUDReturnMessage SaveORManagementByJson(ORAnesthProgressContract contract, int CurrentUserId);
        CUDReturnMessage CUDOperationLink(ORLinkContract data, int userId);
        ORLinkActive CheckOperationLink(Guid GuidCode);
        IQueryable<ORAnesthProgress> SearchAnesthInfo(DateTime FromDate, DateTime ToDate, int State, string kw, int p, int ps, int HpServiceId, int ORRoomId, string siteId, int sourceClientId);
        ORMappingEkipContract GetInfoEkip(long Id);
        CUDReturnMessage SaveCUDEkip(ORMappingEkipContract contract, int CurrentUserId);
        CUDReturnMessage DeleteAnesthEkip(long Id, int userId);
        CUDReturnMessage DeleteSurgery(long Id, int userId);
        CUDReturnMessage UpdateSorting(long Id, int value, int userId);
        CUDReturnMessage ChangeState(long Id, int state, int userId);
        CUDReturnMessage CUDHpService(HpServiceSite data, int userId);
        List<ORNotifyMail> GetListORAnesthByMail(int quantity);
        void ExecuteBlock(Action blockFunction);
        CUDReturnMessage CheckExistPositionByScheduler(long Id, int userId);
        ORUserInfoContract GetORUserInfo(int userId);
        IQueryable<ORTracking> ListORTracking(DateTime fromDate, DateTime toDate, string keyword, string HospitalCode);
        void InsertUserTracking(string log, int userId, int ORId, int state, int type);

        List<HpServiceContract> SearchHpServiceExt(string siteId, string kw, int page, int pageSize, int siteType);

    }

    public class ORDataAccess_Backup : BaseDataAccess, IORDataAccess_Backup
    {
        public ORDataAccess_Backup(string appid, int uid) : base(appid, uid)
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

        public IEnumerable<Models.ORRoom> GetListRoom(string HospitalCode, int sourceClientId, string IsDisplay)
        {
            var query = DbContext.ORRooms.AsQueryable();
            if (!string.IsNullOrEmpty(HospitalCode))
                query = query.Where(c => c.HospitalCode.Equals(HospitalCode));
            if (sourceClientId != 0)
                query = query.Where(c => c.SourceClientId.Equals(sourceClientId));
            if (!string.IsNullOrEmpty(IsDisplay))
                query = query.Where(c => c.IsDisplay.Equals(IsDisplay));
            return query;

        }
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
                                     && l.NameEN.Equals(HospitalCode)
                                     select new ORUserInfoContract
                                     {
                                         Id = oru.Id,
                                         Name = oru.Name,
                                         Email = oru.Email,
                                         PositionId = uip.PositionId,
                                         Phone = oru.Phone
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
                    dtStart = r.dtStart,
                    dtEnd = r.dtEnd,
                    dtOperation = r.dtOperation,
                    TimeAnesth = r.TimeAnesth,
                    RegDescription = r.RegDescription,
                    State = r.State,

                    UIdPTVMain = r.UIdPTVMain,
                    UIdPTVSub1 = r.UIdPTVSub1,
                    UIdPTVSub2 = r.UIdPTVSub2,
                    UIdPTVSub3 = r.UIdPTVSub3,
                    SurgeryDescription = r.SurgeryDescription,
                    UIdCECDoctor = r.UIdCECDoctor,
                    UIdNurseTool1 = r.UIdNurseTool1,

                    UIdNurseTool2 = r.UIdNurseTool2,
                    UIdNurseOutRun1 = r.UIdNurseOutRun1,
                    UIdNurseOutRun2 = r.UIdNurseOutRun2,

                    UIdMainAnesthDoctor = r.UIdMainAnesthDoctor,
                    UIdSubAnesthDoctor = r.UIdSubAnesthDoctor,
                    UIdAnesthNurse1 = r.UIdAnesthNurse1,
                    UIdAnesthNurse2 = r.UIdAnesthNurse2,

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
                        data.PositionSubAnesthDoctor = "Bác sĩ phụ mê";// EnumExtension.GetDescription((ORPositionEnum)ORUserInfo.PositionId);
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

                if (data.HpServiceId > 0)
                {
                    var dataService = DbContext.HpServices.FirstOrDefault(d => d.Id == data.HpServiceId);
                    if (dataService != null)
                    {
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

            if(data.dtEnd.Value.Subtract(data.dtStart.Value) < TimeSpan.FromMinutes(15))
            {
                return new CUDReturnMessage() { Id = 0, Message = "Thời gian tối thiểu là 15 phút" };
            }
            #region check validate duplicate time 

            var rsCheckExist = ExecStoredProc<ExecuteQueryIntIdResult>("Core_CheckExistORRegistor", new object[] { data.Id, data.dtStart, data.dtEnd, data.ORRoomId, data.HpServiceId, data.HospitalCode, userId }).FirstOrDefault();

            if (rsCheckExist != null && rsCheckExist.IsSuccess)
                return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại " };
            #endregion
            //Check Role permission for location data
            #region Check Role permission for location data
            if (data.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check Role permission for location data
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
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    UpdatedBy = 0,
                    UpdatedDate = DateTime.Now,
                    IsDeleted = false
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
                var reCheckExist = ExecStoredProc<ExecuteQueryIntIdResult>("Core_CheckExistORRegistor", new object[] { data.Id, data.dtStart, data.dtEnd, data.ORRoomId, data.HpServiceId, data.HospitalCode, userId }).FirstOrDefault();

                if (reCheckExist != null && reCheckExist.IsSuccess)
                    return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại " };

                var objHpService = DbContext.HpServices.Find(data.HpServiceId);
                var dtStart = data.dtStart.Value.AddMinutes((double)-data.TimeAnesth);
                var dtEnd = data.dtEnd.Value.AddMinutes((double)objHpService?.CleaningTime);
                var lastDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);

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
                            //item.ORRoomId == data.ORRoomId &&
                            ((item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) <= dtStart && dtStart <= item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime)) ||
                            (item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) <= dtEnd && dtEnd <= item.dtEnd.Value.AddMinutes((double)objHpServiceCurent.CleaningTime)) ||
                            (dtStart <= item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) <= dtEnd) ||
                            (dtStart <= item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) && item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) <= dtEnd)))
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
                    var patients = orAnesthProgresses.Where(x => x.ORRoomId == data.ORRoomId).ToList();
                    foreach (var item in patients)
                    {
                        var objHpServiceCurent = DbContext.HpServices.Find(item.HpServiceId);
                        if ((item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) <= dtStart && dtStart <= item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime)) ||
                            (item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) <= dtEnd && dtEnd <= item.dtEnd.Value.AddMinutes((double)objHpServiceCurent.CleaningTime)) ||
                            (dtStart <= item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) && item.dtStart.Value.AddMinutes((double)-item.TimeAnesth) <= dtEnd) ||
                            (dtStart <= item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) && item.dtEnd.Value.AddMinutes((double)objHpServiceCurent?.CleaningTime) <= dtEnd))
                        {
                            return new CUDReturnMessage() { Id = 0, Message = "Thời gian đăng ký bị trùng, vui lòng kiểm tra lại" };
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


            if (progress.State == (int)ORProgressStateEnum.Registor || progress.State == (int)ORProgressStateEnum.NoApproveSurgeryManager)//đăng ký mỗ
            {
                progress.VisitCode = data.VisitCode;
                progress.PId = data.PId;
                progress.Id = data.Id;
                progress.HospitalCode = data.HospitalCode;
                progress.HospitalName = data.HospitalName;
                progress.ORRoomId = data.ORRoomId;
                progress.dtStart = data.dtStart;
                progress.dtEnd = data.dtEnd;
                progress.dtOperation = data.dtOperation;
                progress.TimeAnesth = data.TimeAnesth;
                progress.RegDescription = data.RegDescription;
                progress.State = (int)ORProgressStateEnum.Registor;
                progress.NameProject = data.NameProject;
                progress.HpServiceId = data.HpServiceId;

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
        /// <summary>
        /// Da fix phan quyen du lieu theo Site
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage SaveORManagementByJson(ORAnesthProgressContract data, int userId)
        {
            ORAnesthProgress progress;
            if (data == null || data.Id == 0)
            {
                return new CUDReturnMessage() { Id = 0, Message = "tham số không hợp lệ" };
            }
            //Update
            progress = DbContext.ORAnesthProgresses.FirstOrDefault(r => (r.Id == data.Id));
            if (progress == null)
                return new CUDReturnMessage() { Id = 0, Message = "Lỗi trong quá trình xử lý" };
            //Check Role permission for location data
            #region Check Role permission for location data
            if (progress.HospitalCode != MemberExtInfo.CurrentLocaltion.NameEN)
            {
                //Return Invalid current location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }
            #endregion .Check Role permission for location data
            //else if (((data.State == (int)ORProgressStateEnum.ApproveSurgeryManager) || (data.State == (int)ORProgressStateEnum.NoApproveSurgeryManager))&& (progress.State==(int)ORProgressStateEnum.Registor))
            else if (data.State == (int)ORProgressStateEnum.ApproveSurgeryManager || data.State == (int)ORProgressStateEnum.NoApproveSurgeryManager || progress.State == (int)ORProgressStateEnum.Registor)
            {
                if (data.UIdPTVMain != null && data.UIdPTVMain != 0)
                    progress.UIdPTVMain = data.UIdPTVMain;
                if (data.UIdPTVSub1 != null && data.UIdPTVSub1 != 0)
                    progress.UIdPTVSub1 = data.UIdPTVSub1;
                if (data.UIdPTVSub2 != null && data.UIdPTVSub2 != 0)
                    progress.UIdPTVSub2 = data.UIdPTVSub2;
                if (data.UIdPTVSub3 != null && data.UIdPTVSub3 != 0)
                    progress.UIdPTVSub3 = data.UIdPTVSub3;
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
                if (!string.IsNullOrEmpty(data.AnesthDescription))
                    progress.AnesthDescription = data.AnesthDescription;
                progress.State = data.State;
            }
            else if ((data.State == (int)ORProgressStateEnum.CancelAnesthManager) || (data.State == (int)ORProgressStateEnum.ApproveAnesthManager))
            {
                if (data.UIdPTVMain != null && data.UIdPTVMain != 0)
                    progress.UIdPTVMain = data.UIdPTVMain;
                if (data.UIdPTVSub1 != null && data.UIdPTVSub1 != 0)
                    progress.UIdPTVSub1 = data.UIdPTVSub1;
                if (data.UIdPTVSub2 != null && data.UIdPTVSub2 != 0)
                    progress.UIdPTVSub2 = data.UIdPTVSub2;
                if (data.UIdPTVSub3 != null && data.UIdPTVSub3 != 0)
                    progress.UIdPTVSub3 = data.UIdPTVSub3;
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
                if (!string.IsNullOrEmpty(data.AnesthDescription))
                    progress.AnesthDescription = data.AnesthDescription;
                progress.State = data.State;
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
                if (state >= (int)ORProgressStateEnum.ApproveAnesthManager)
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
                var CheckExistLoc = data.listSites.Select(x1=>Convert.ToInt32(x1.Id)).ToList().Except(MemberExtInfo.Locations.Select(x => x.LocationId).ToList()).ToList();
                if (CheckExistLoc.Any())
                {
                    //Return Invalid location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }

                attr = DbContext.HpServices.FirstOrDefault(d => d.Id.Equals(data.Id));
                if (data.Id == 0 || attr == null) //Innsert
                {
                    return CreateHpService(data, userId);
                }
                else
                {
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
                UpdatedBy = userId
               ,
                UpdatedDate = DateTime.Now
               ,
                SourceClientId = data.SourceClientId
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
            siteInfo.Add("HHL", "pmhhl@vinmec.com");
            siteInfo.Add("HCP", "pmhcp@vinmec.com");
            siteInfo.Add("HDN", "pmhdn@vinmec.com");
            siteInfo.Add("HHP", "pmhhp@vinmec.com");
            siteInfo.Add("HNT", "pmhnt@vinmec.com");
            siteInfo.Add("HPQ", "pmhpq@vinmec.com");
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
                                     && s.Name.Contains(kw)
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

    }
}

