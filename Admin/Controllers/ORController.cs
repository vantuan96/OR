using Admin.Helper;
using Admin.Models;
using Admin.Models.OR;
using Admin.Models.Shared;
using Admin.Resource;
using Business.API;
using Caching;
using Caching.Core;
using Caching.OR;
using Contract.Enum;
using Contract.OR;
using Contract.OR.SyncData;
using Contract.Shared;
using DataAccess.Models;
using Microsoft.Security.Application;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using VG.Common;
using VG.Framework.Mvc;

namespace Admin.Controllers
{
    [CheckUserCaching]
    [AuthorizeOH]
    public class ORController : BaseController
    {
        private readonly ISyncGateWay _syncData;
        private readonly Admin.Shared.ORVisitBase _visitService;
        private readonly ORCaching _orService;
        private readonly ILocationCaching _locationApi;
        private readonly IUserMngtCaching _userMngtCaching;
        public ORController(IAuthenCaching authenCaching, ISystemSettingCaching systemSettingApi, ISyncGateWay syncData, ORCaching orService, ILocationCaching locationApi, IUserMngtCaching userMngtCaching) : base(authenCaching, systemSettingApi)
        {
            _syncData = syncData;
            _visitService = new Admin.Shared.ORVisitBase(new CookieUtils().GetCookie(AdminGlobal.SignInTokenVisitOR), CurrentUserName);
            _orService = orService;
            _locationApi = locationApi;
            _userMngtCaching = userMngtCaching;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchPatientOR(string kw = "", string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            try
            {
                //Check role access on location
                siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;
                if (siteId != memberExtendedInfo.CurrentLocaltion.NameEN)
                {
                    //Redirect to site access denied
                    return new RedirectResult(new UrlHelper(HttpContext.Request.RequestContext).RouteUrl("ErrorHandler", new { id = 1001 }));
                }
                //show list site allows
                #region master data
                var listSites = new List<SelectListItem>();
                if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
                {
                    foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                    {
                        listSites.Add(new SelectListItem()
                        {
                            Text = item.NameVN,
                            Value = item.NameEN
                        });
                    }
                }
                if (!listSites.Any()) return RedirectToAction("Login", "Authen");
                if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
                {
                    siteId = listSites.FirstOrDefault().Value;
                }
                var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
                // collect data
                #endregion

                string LastSiteName = string.Empty;
                string LastVistDate = string.Empty;
                kw = HttpUtility.UrlDecode(kw).Trim();
                BenhNhanOR data = null;
                if (!string.IsNullOrEmpty(kw))
                {
                    data = _syncData.GetBenhNhanORInfo(kw, sourceClientId);
                    if (data == null)
                    {
                        //Tìm kiếm theo thông tin Patient
                        data = _syncData.GetPatientInfo(kw);
                    }
                }
                if (data != null && data.VisitSyncs != null && data.VisitSyncs.VisitSync != null && data.VisitSyncs.VisitSync.Any())
                {
                    var lastSiteVisit = data.VisitSyncs.VisitSync.OrderByDescending(c => c.NGAY_VAO).FirstOrDefault();
                    LastVistDate = lastSiteVisit.NGAY_VAO != null ? lastSiteVisit.NGAY_VAO.ToString("dd/MM/yyyy") : string.Empty;
                    if (lastSiteVisit != null && !string.IsNullOrEmpty(lastSiteVisit.MA_BENH_VIEN))
                    {
                        var lastSite = _orService.GetHospitalSite(lastSiteVisit.MA_BENH_VIEN);
                        LastSiteName = lastSite != null ? lastSite.SiteNameFull : string.Empty;
                    }
                }

                #region Insert new service
                if (data != null && data.ListServices != null)
                {
                    //log.Debug(string.Format("OR.GetBenhNhanORInfo ListService return: {0}", JsonConvert.SerializeObject(data.ListServices)));
                    foreach (var model in data.ListServices.Services)
                    {
                        var response = _orService.InsertNewService(model, CurrentUserId);
                    }
                    var newList = _orService.RemoveExistingServices(data.ListServices.Services, siteId);
                    data.ListServices.Services = newList;
                }
                #endregion

                var viewModel = new PatientORSyncModel()
                {
                    Data = data,
                    kw = kw,
                    siteId = siteId,
                    sourceClientId = sourceClientId,
                    listSites = listSites,
                    siteName = site != null ? site.Text : string.Empty,
                    LastSiteName = LastSiteName,
                    LastVistDate = LastVistDate

                };
                //log.Debug(string.Format("OR.SearchPatientOR return: {0}", JsonConvert.SerializeObject(viewModel)));
                return View(viewModel);

            }
            catch (Exception ex)
            {
                //log.Debug(ex);
                return View();
            }

        }
        /// <summary>
        /// //1=Book phòng mổ//2=Ghi nhận ekip
        /// </summary>
        /// <param name="dataJson"></param>
        /// <param name="typeAction"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SyncVisitOR(string dataJson, int typeAction = 1)
        {
            try
            {
                ORVisitModel model = JsonConvert.DeserializeObject<ORVisitModel>(dataJson);

                if (model != null && !string.IsNullOrEmpty(model.HospitalCode))
                {
                    //Check site is equal current site
                    if (!memberExtendedInfo.CurrentLocaltion.NameEN.Equals(model.HospitalCode))
                    {
                        //Invalid current site
                        return Json(new { ID = 0, Message = "Thông tin lượt khám không hợp lệ (Invalid Site)" });
                    }
                    if (typeAction == 2)
                    {
                        if (string.IsNullOrEmpty(model.PatientService))
                            return Json(new { ID = 0, Message = "Vui lòng chọn chỉ định cần ghi nhận." });
                        else if (_orService.IsOrderExisted(model.PatientService.Split(new[] { "//" }, StringSplitOptions.None).First(), model.PatientService.Split(new[] { "//" }, StringSplitOptions.None)?.Last(), model.HospitalCode))
                            return Json(new { ID = 0, Message = "Chỉ định này đã được ghi nhận, vui lòng kiểm tra lại." });
                        //vutv7 - check trường hợp chỉ định quá hạn không cho ghi nhận ekip
                        //else if (!string.IsNullOrEmpty(model.PatientService))
                        //{
                        //    var splitPatienSr = model.PatientService.Split(new[] { "//" }, System.StringSplitOptions.RemoveEmptyEntries);
                        //    PatientService newObj = new PatientService();
                        //    newObj.OrderID = splitPatienSr[0];
                        //    if (splitPatienSr.Length >= 2)
                        //    {
                        //        newObj.ItemCode = splitPatienSr[1];
                        //    }
                        //    if (splitPatienSr.Length >= 3)
                        //    {
                        //        newObj.ChargeDetailId = splitPatienSr[2];
                        //    }
                        //    if (splitPatienSr.Length >= 4)
                        //    {
                        //        newObj.DepartmentCode = splitPatienSr[3];
                        //    }
                        //    if (splitPatienSr.Length >= 5)
                        //    {
                        //        newObj.LocationName = splitPatienSr[4];
                        //    }
                        //    if (splitPatienSr.Length >= 6)
                        //    {
                        //        newObj.ServiceType = Convert.ToInt32(splitPatienSr[5]);
                        //    }
                        //    if (splitPatienSr.Length >= 7)
                        //    {
                        //        newObj.ChargeDate = Convert.ToDateTime(splitPatienSr[6]);
                        //    }
                        //    if (newObj.ChargeDetailId != null && newObj.ChargeDate != null)
                        //    {
                        //        int ExprireMonth = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ExprireMonth"].ToString());
                        //        int months = (DateTime.Now.Year - newObj.ChargeDate.Year) * 12 + (DateTime.Now.Month - newObj.ChargeDate.Month);
                        //        if (months > ExprireMonth)
                        //        {
                        //            return Json(new { ID = 0, Message = "Chỉ định cho ca mổ này đã vượt quá 2 tháng cho phép để ghi nhận doanh thu theo quy đinh." });
                        //        }
                        //    }
                        //}
                    }
                    var site = _orService.GetHospitalSite(model.HospitalCode);
                    if (string.IsNullOrEmpty(model.HospitalName))
                        model.HospitalName = site != null ? site.SiteNameFull : string.Empty;
                    if (string.IsNullOrEmpty(model.HospitalCode))
                        model.HospitalPhone = site != null ? site.Phone : string.Empty;
                    _visitService.SetCurrentVisit(model);
                    return Json(new { ID = 1, Message = "" });
                }
                return Json(new { ID = 0, Message = "Thông tin lượt khám không hợp lệ" });
            }
            catch (Exception ex)
            {
                //log.Debug(ex);
                return Json(new { ID = 0, Message = "Lỗi trong quá trình xử lý" });
            }
        }
        public ActionResult ViewVisit()
        {
            var visit = _visitService.GetCurrentVisit();
            if (visit == null) return RedirectToAction("SearchPatientOR", "OR");
            return View(visit);
        }
        public ActionResult ViewORRegistor(int Id = 0)
        {

            //show list site allows
            var model = new ORAnesthProgressModel();
            model.dtStart = DateTime.Now;
            model.dtOperation = DateTime.Now;
            model.dtEnd = DateTime.Now.AddHours(1);
            ORAnesthProgressContract orProgress;
            ORVisitModel v = null;
            string siteId = string.Empty;
            try
            {
                if (Id > 0)
                {
                    orProgress = _orService.GetORAnesthProgress(Id.ToString(), (int)TypeSearchEnum.Id);

                    var dataServices = _orService.GetDataWithMemCache<ORCaching, List<HpServiceContract>>(_orService, ConfigUrl.PreFixMemmoryCache, "GetListHpServices",
                          new object[] { orProgress.HospitalCode, 0 }, CacheTimeout.Medium);
                    // var dataServices = _orService.GetListHpServices(orProgress.HospitalCode, 0);
                    if (dataServices != null && dataServices.Any())
                        model.listHpServices = dataServices.Where(x => x.Type == 2).ToList();

                    var listORRooms = _orService.GetDataWithMemCache<ORCaching, List<ORRoomContract>>(_orService, ConfigUrl.PreFixMemmoryCache, "GetListRoom",
                         new object[] { orProgress.HospitalCode, string.Empty, (int)SourceClientEnum.Oh, -1, "1", true }, CacheTimeout.Medium);
                    // var listORRooms = _orService.GetListRoom(orProgress.HospitalCode, 0, string.Empty);
                    if (listORRooms != null && listORRooms.Any())
                    {
                        if (model.SurgeryType == 2)
                        {
                            listORRooms = listORRooms?.Where(x => x.TypeRoom == (int)RoomTypeEnum.Emergency)?.ToList();
                        }
                        else
                        {
                            listORRooms = listORRooms?.Where(x => x.TypeRoom != (int)RoomTypeEnum.Emergency && x.TypeRoom != (int)RoomTypeEnum.Approve4Pay)?.ToList();
                        }
                        model.listORRooms = listORRooms;
                    }
                    model.CreatedBy = orProgress.CreatedBy;
                    siteId = orProgress.HospitalCode;
                    #region Build PTV main info
                    model.EmailSurgeryDoctorManager = orProgress.EmailPTVMain;
                    model.PhoneSurgeryDoctorManager = orProgress.PhonePTVMain;
                    model.NameSurgeryDoctorManager = orProgress.NamePTVMain;
                    #endregion
                    #region Check access denied on Data
                    if (model.CreatedBy != CurrentUserId && !memberExtendedInfo.IsSuperAdmin && !memberExtendedInfo.IsManageAdminSurgery)
                    {
                        //Redirect to site access denied
                        return new RedirectResult(new UrlHelper(HttpContext.Request.RequestContext).RouteUrl("ErrorHandler", new { id = -1 }));
                    }
                    #endregion .Check access denied on Data

                }
                else
                {
                    v = _visitService.GetCurrentVisit();
                    if (v == null)
                        return RedirectToAction("SearchPatientOR", "OR");
                    siteId = v.HospitalCode;

                    #region master data
                    var listSites = new List<SelectListItem>();
                    if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
                    {
                        foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                        {
                            listSites.Add(new SelectListItem()
                            {
                                Text = item.NameVN,
                                Value = item.NameEN
                            });
                        }
                    }
                    if (!listSites.Any()) return RedirectToAction("Login", "Authen");
                    if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
                    {
                        siteId = listSites.FirstOrDefault().Value;
                    }
                    var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));

                    #endregion


                    var dataServices = _orService.GetDataWithMemCache<ORCaching, List<HpServiceContract>>(_orService, ConfigUrl.PreFixMemmoryCache, "GetListHpServices",
                           new object[] { siteId, 0 }, CacheTimeout.Medium);
                    // var dataServices = _orService.GetListHpServices(siteId, 0);
                    if (dataServices != null && dataServices.Any())
                    {
                        model.listHpServices = dataServices;
                        model.TimeAnesth = dataServices.First().AnesthesiaTime;
                    }

                    var listORRooms = _orService.GetDataWithMemCache<ORCaching, List<ORRoomContract>>(_orService, ConfigUrl.PreFixMemmoryCache, "GetListRoom",
                         new object[] { siteId, string.Empty, 0, -1, "1", true }, CacheTimeout.Medium);
                    //var listORRooms = _orService.GetListRoom(siteId, 0, string.Empty);
                    if (listORRooms != null && listORRooms.Any())
                    {
                        if (v.SurgeryType == 2)
                        {
                            listORRooms = listORRooms?.Where(x => x.TypeRoom == (int)RoomTypeEnum.Emergency)?.ToList();
                        }
                        else
                        {
                            listORRooms = listORRooms?.Where(x => x.TypeRoom != (int)RoomTypeEnum.Emergency && x.TypeRoom != (int)RoomTypeEnum.Approve4Pay)?.ToList();
                        }
                        model.listORRooms = listORRooms;
                    }
                    orProgress = null;

                    #region GetServiceId
                    var oCode = v.PatientService.Split(new[] { "//" }, StringSplitOptions.None);
                    if (oCode.Count() > 1)
                    {
                        var serviceId = _orService.GetServiceByCode(oCode[1]);
                        if (serviceId != null)
                        {
                            model.HpServiceId = serviceId.Id;
                            model.HpServiceName = serviceId.Name;
                        }
                        model.OrderID = oCode[0];
                        if (oCode.Length >= 3)
                            model.ChargeDetailId = oCode[2];
                    }
                    #endregion
                }
                if (orProgress != null)
                {
                    SetDataModel(ref model, orProgress);
                    model.VisitCode = Guid.NewGuid().ToString();
                }
                else if (Id == 0)
                {
                    model = GetInfoVisitCurrent(model, v);

                }
                else
                {
                    var r = _orService.GetORAnesthProgress(v.VISIT_CODE, (int)TypeSearchEnum.Id);
                    if (r != null)
                    {
                        SetDataModel(ref model, r);
                    }
                    else
                    {
                        return RedirectToAction("SearchPatientOR", "OR");
                    }
                }
                var Charges = _orService.GetChargesFromOH(model.HospitalCode, model.PId, CurrentUserId);
                if (Charges?.Count > 0)
                {
                    model.Charges = Charges.Where(x => model.listHpServices.Any(y => y.Oh_Code == x.ItemCode))?.ToList();
                    if (model.listHpServices?.Count > 0 && model.Charges?.Count > 0)
                    {
                        foreach (var itemC in model.Charges)
                        {
                            var itemSv = model.listHpServices.Find(x => x.Oh_Code == itemC.ItemCode);
                            if (itemSv != null)
                            {
                                itemSv.OrderID = itemC.OrderID;
                                itemSv.ChargeDetailId = itemC.ChargeDetailId;
                                itemSv.DepartmentCode = itemC.DepartmentCode;
                                itemSv.LocationName = itemC.LocationName;
                                //vutv7
                                itemSv.ChargeDate = itemC.ChargeDate.ToString();
                                itemSv.ChargeBy = itemC.ChargeBy;
                            }
                        }
                        model.listHpServices = model.listHpServices.Where(x => !string.IsNullOrEmpty(x.ChargeDetailId))?.ToList();
                        if (string.IsNullOrEmpty(model.AdmissionWard) && model.listHpServices?.Count > 0)
                        {
                            model.AdmissionWard = model.listHpServices[0].LocationName;
                            //Get Chẩn đoán từ E-Form
                            model.Diagnosis = _syncData.GetDiagnosisByCharge(model.listHpServices[0].ChargeDetailId);
                        }
                    }
                }
                else
                {
                    model.listHpServices = new List<HpServiceContract>();
                }
                model.ShowdtEnd = model.dtEnd.ToVEShortTime();
                model.ShowdtStart = model.dtStart.ToVEShortTime();
                model.ShowTimeAdmission = model.dtAdmission != null ? model.dtAdmission.Value.ToVEShortTime() : string.Empty;
                if (model.State != (int)ORProgressStateEnum.Registor && model.State != 0 && model.State != (int)ORProgressStateEnum.NoApproveSurgeryManager && model.State != (int)ORProgressStateEnum.AssignEkip)
                    return RedirectToAction("SearchPatientOR", "OR");

                var location = memberExtendedInfo.Locations.FirstOrDefault(l => l.NameEN.ToLower().Equals(siteId.ToLower()));
                if (location != null)
                {
                    model.ORUnitName = location.ORUnitName;
                    model.ORUnitEmail = location.ORUnitEmail;
                }
                if (string.IsNullOrEmpty(model.HospitalCode))
                    model.HospitalCode = siteId;
                model.CurrentUserId = CurrentUserId;
                #region Check access denied on location
                if (siteId != memberExtendedInfo.CurrentLocaltion.NameEN)
                {
                    //Redirect to site access denied
                    return new RedirectResult(new UrlHelper(HttpContext.Request.RequestContext).RouteUrl("ErrorHandler", new { id = -1 }));
                }
                #endregion .Check access denied on location
            }
            catch (Exception ex)
            {
                //log.Debug(ex);
            }
            return View(model);
        }
        private ORAnesthProgressModel GetInfoVisitCurrent(ORAnesthProgressModel model, ORVisitModel v)
        {
            if (v == null) return model;
            model.PId = v.MA_BN;
            model.Address = v.DIA_CHI;
            model.Sex = v.GIOI_TINH;
            model.NgaySinh = v.NGAY_SINH;
            model.HoTen = v.HO_TEN;
            model.VisitCode = v.VISIT_CODE;
            model.HospitalCode = v.HospitalCode;
            model.HospitalName = v.HospitalName;
            model.HospitalPhone = v.HospitalPhone;
            model.PatientPhone = v.PatientPhone;
            model.Email = v.Email;
            model.Ages = v.Age;
            model.PatientPhone = v.PatientPhone;
            model.SurgeryType = v.SurgeryType;
            return model;
        }

        private void SetDataModel(ref ORAnesthProgressModel model, ORAnesthProgressContract r)
        {
            model.Id = r.Id;
            model.PId = r.PId;
            model.VisitCode = r.VisitCode;
            model.HoTen = r.HoTen;
            model.NgaySinh = r.NgaySinh;
            model.Sex = r.Sex;
            model.Address = r.Address;
            model.Email = r.Email;
            model.Ages = r.Ages;
            model.NameProject = r.NameProject;
            model.HpServiceId = r.HpServiceId;
            model.HpServiceName = r.HpServiceName;
            model.ORRoomId = r.ORRoomId;
            model.SurgeryType = r.SurgeryType;
            model.dtStart = r.dtStart ?? DateTime.Now;
            model.dtEnd = r.dtEnd ?? DateTime.Now;
            model.dtOperation = r.dtOperation ?? DateTime.Now;
            model.dtAdmission = r.dtAdmission ?? null;
            model.AdmissionWard = r.AdmissionWard;

            model.HospitalName = r.HospitalName;
            model.HospitalPhone = r.HospitalPhone;
            model.PatientPhone = r.PatientPhone;

            model.HospitalCode = r.HospitalCode;

            model.State = r.State;
            model.TimeAnesth = r.TimeAnesth ?? 0;
            model.RegDescription = r.RegDescription;
            model.Diagnosis = r.Diagnosis;

            model.NameCreatedBy = !(string.IsNullOrEmpty(r.NameCreatedBy)) ? r.NameCreatedBy : CurrentUserDisplayName;
            model.EmailCreatedBy = !(string.IsNullOrEmpty(r.EmailCreatedBy)) ? r.EmailCreatedBy : CurrentEmail;
            model.CreatedBy = r.CreatedBy != 0 ? r.CreatedBy : 0;
            model.CurrentUserId = CurrentUserId;


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult SaveORRegistorByJson(ORAnesthProgressModel model)
        {
            if (ModelState.IsValid)
            {
                ActionMessage actionMessage;
                var contract = new ORAnesthProgressContract();
                contract.Id = model.Id;
                contract.PId = model.PId;
                contract.HoTen = Sanitizer.GetSafeHtmlFragment(model.HoTen);
                contract.NgaySinh = model.NgaySinh;
                contract.Sex = model.Sex;
                contract.VisitCode = Guid.NewGuid().ToString();
                contract.HospitalCode = Sanitizer.GetSafeHtmlFragment(model.HospitalCode);
                contract.HospitalName = Sanitizer.GetSafeHtmlFragment(model.HospitalName);
                contract.Address = Sanitizer.GetSafeHtmlFragment(model.Address);
                contract.Ages = Sanitizer.GetSafeHtmlFragment(model.Ages);
                contract.Email = Sanitizer.GetSafeHtmlFragment(model.Email);
                contract.PatientPhone = Sanitizer.GetSafeHtmlFragment(model.PatientPhone);
                //ext
                contract.HpServiceId = model.HpServiceId;
                contract.ORRoomId = model.ORRoomId;
                contract.SurgeryType = model.SurgeryType;
                contract.dtOperation = model.dtOperation;
                contract.dtStart = model.dtOperation.CloneTime(model.ShowdtStart);
                contract.dtEnd = model.dtOperation.CloneTime(model.ShowdtEnd);
                if (model.dtAdmission != null)
                {
                    if (!string.IsNullOrEmpty(model.ShowTimeAdmission))
                    {
                        contract.dtAdmission = model.dtAdmission.Value.CloneTime(model.ShowTimeAdmission);
                        if (contract.dtAdmission.Value > contract.dtStart.Value.AddMinutes(-model.TimeAnesth))
                        {
                            return Json(new ActionMessage(-1, "Thời gian dự kiến nhập viện không được muộn hơn thời gian bắt đầu phẫu thuật và gây mê (nếu có)!"));
                        }
                    }
                    else
                    {
                        return Json(new ActionMessage(-1, "Vui lòng chọn thời gian dự kiến nhập viện"));
                    }
                }
                contract.HospitalPhone = Sanitizer.GetSafeHtmlFragment(model.HospitalPhone);
                contract.State = (int)ORProgressStateEnum.Registor;
                contract.RegDescription = model.RegDescription;
                contract.AdmissionWard = model.AdmissionWard;
                contract.Diagnosis = model.Diagnosis;
                contract.ProjectName = Sanitizer.GetSafeHtmlFragment(model.ProjectName);
                contract.TimeAnesth = model.TimeAnesth;
                contract.OrderID = model.OrderID;
                contract.ChargeDetailId = model.ChargeDetailId;
                contract.DepartmentCode = model.DepartmentCode;
                contract.UIdPTVMain = model.UIdPTVMain;
                //vutv7
                contract.ChargeDate = model.ChargeDate;
                contract.ChargeBy = model.ChargeBy;

                #region Check access denied on location
                if (memberExtendedInfo.CurrentLocaltion.NameEN != contract.HospitalCode)
                {
                    actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied));
                    return Json(actionMessage);
                }
                #endregion .Check access denied on location

                if (DateTime.Compare(contract.dtStart ?? DateTime.Now, contract.dtEnd ?? DateTime.Now) > 0)
                {
                    actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.Error));
                    TempData[AdminGlobal.ActionMessage] = actionMessage;
                    var responseMessage = new CUDReturnMessage() { Id = 0, Message = "Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu" };
                    var actionMessageCheckTime = new ActionMessage(responseMessage);
                    actionMessageCheckTime.Message = responseMessage.Message;
                    return Json(actionMessageCheckTime);
                }
                //Kiem tra cho phep sua
                if (contract.Id == 0 || (contract.Id > 0 && (contract.State == (int)ORProgressStateEnum.NoApproveSurgeryManager || contract.State == (int)ORProgressStateEnum.Registor || contract.State == (int)ORProgressStateEnum.AssignEkip) && (model.CreatedBy == CurrentUserId || memberExtendedInfo.IsSuperAdmin || memberExtendedInfo.IsManageAdminSurgery) && ((contract.dtOperation.HasValue && (contract.dtOperation.Value.EqualMonth() || DateTime.Today.Date <= contract.dtOperation.Value)) || (contract.dtExtend.HasValue && DateTime.Today.Date <= contract.dtExtend.Value))))
                {
                    CUDReturnMessage response = _orService.SaveORRegistorByJson(contract, CurrentUserId);

                    if (response.Id > 0)
                    {
                        actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.OR_SuccessCreate));
                        TempData[AdminGlobal.ActionMessage] = actionMessage;
                        #region send mail to registor 
                        //string hrefLink = MailHelper.DomainMailUri + "phan-cong-va-ghi-nhan-ekip-mo" + "?Id=" + response.Id;

                        //var dataLink = new ORLinkContract()
                        //{
                        //    GuidCode = Guid.NewGuid(),
                        //    Code = hrefLink,
                        //    LimitDate = DateTime.Now.AddDays(30),
                        //    IsActive = false,
                        //    IpActive = string.Empty
                        //};
                        //_orService.CUDOperationLink(dataLink, CurrentUserId);
                        //string hrefActiveCode = MailHelper.DomainMailUri + "kich-hoat-link" + "?code=" + dataLink.GuidCode;

                        //var contentMail = string.Format(@"Dear Anh/Chị Điều Phối Phòng Mổ. <br/> 
                        //                                     Hiện tại đã có bác sĩ {0} đăng ký ca mổ với thông tin sau: <br/>
                        //                                      - Họ tên bệnh nhân: {1} <br/>
                        //                                      - Ngày tháng năm sinh: {2} <br/> 
                        //                                      - Tên dịch vụ mổ: {3} <br/>
                        //                                      - Thời gian gây mê: {4} <br/>
                        //                                      - Phòng mổ yêu cầu: {5} <br/>
                        //                                      - Ngày thực hiện yêu cầu: {6} <br/>
                        //                                      - Thời gian bắt đầu: {7} <br/>
                        //                                      - Thời gian kết thúc: {8} <br/>
                        //    Kính nhờ anh/chị quản lý phòng mổ click vào link sau đây <a href={9}> Click xem chi tiết </a> để kiểm tra và xác nhận hoàn thành  . <br/>
                        //    Trân trọng.<br/>",model.NameCreatedBy, model.HoTen,model.NgaySinh.ToVEShortDate(),model.HpServiceName,model.TimeAnesth,model.ORRoomName,model.ShowdtOperation,model.ShowdtStart,model.ShowdtEnd, hrefActiveCode);                   
                        //if(MailHelper.IsEmail(model.EmailSurgeryDoctorManager))
                        //    MailHelper.SendMail(model.EmailSurgeryDoctorManager, string.Empty, string.Empty, "Phiếu Đăng ký Ca Mổ", contentMail);

                        #endregion
                        _visitService.SetCurrentVisit(null);
                    }
                    else
                    {
                        actionMessage = new ActionMessage(response.Id, response.Message);
                    }
                    var actionMessageExt = new ActionMessage(response);
                    actionMessageExt.Message = response.Message;
                    return Json(actionMessageExt);
                }
                else
                {
                    return Json(new ActionMessage(-1, "Dữ liệu không hợp lệ hoặc bạn không có quyền sửa"));
                }
            }
            return Json(new ActionMessage(-1, MessageResource.Shared_ModelState_InValid));
        }
        #region ORView Management
        /// <summary>
        /// actionType=1: Điếu phối mổ hoặc ghi nhận ekip mổ
        /// actionType=2: Trực tiện ghi nhận ekip mổ không qua điều phối hoặc qua book phòng
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public ActionResult ViewORManagement(int Id = 0, int actionType = 1)
        {
            var model = new ORAnesthManagementModel();
            model.listUserEkips = new List<ORMappingEkipContract>();
            model.listEkips = new List<ORUserInfoContract>();
            model.dtStart = DateTime.Now;
            model.dtOperation = DateTime.Now;
            model.dtEnd = DateTime.Now;
            ORVisitModel v = null;
            if (Id == 0)
            {
                #region Process when direct approve with out book room
                if (actionType == 2)
                {
                    v = _visitService.GetCurrentVisit();
                }
                if (v == null)
                    return RedirectToAction("SearchPatientOR", "OR");
                model = GetCoordinatorSurgicalFromVisitCurrent(model, v);
                model.CreatedBy = CurrentUserId;
                #region GetServiceId
                //if (!string.IsNullOrEmpty(v.PatientService))
                //{
                //    var oCode = v.PatientService.Split(new[] { "//" }, StringSplitOptions.None);
                //    if (oCode.Count() > 1)
                //    {
                //        var svEntity = _orService.GetServiceByCode(oCode[1]);
                //        if (svEntity != null)
                //        {
                //            model.HpServiceId = svEntity.Id;
                //            model.HpServiceName = svEntity.Name;
                //        }
                //        model.OrderID = oCode[0];
                //        if (oCode.Length >= 3)
                //            model.ChargeDetailId = oCode[2];
                //        if (oCode.Length >= 4)
                //            model.DepartmentCode = oCode[3];
                //        if (oCode.Length >= 5)
                //            model.AdmissionWard = oCode[4];
                //        if (oCode.Length >= 7)
                //            model.ChargeDate = Convert.ToDateTime(oCode[6]);
                //    }
                //}

                if (!string.IsNullOrEmpty(v.PatientService))
                {
                    List<PatientService> lstPatientService = new List<PatientService>();
                    var lstPatienSr = v.PatientService.Split(new[] { ";" }, System.StringSplitOptions.RemoveEmptyEntries);
                    if (lstPatienSr.Count() > 0)
                    {
                        foreach (var item in lstPatienSr)
                        {
                            var oCode = item.Split(new[] { "//" }, System.StringSplitOptions.RemoveEmptyEntries);
                            if (oCode.Count() > 1)
                            {
                                var svEntity = _orService.GetServiceByCode(oCode[1]);
                                if (svEntity != null)
                                {
                                    model.HpServiceId = svEntity.Id;
                                    model.HpServiceName = svEntity.Name;
                                }
                                model.OrderID = oCode[0];
                                if (oCode.Length >= 3)
                                    model.ChargeDetailId += String.IsNullOrEmpty(model.ChargeDetailId) ? oCode[2] : "," + oCode[2];
                                if (oCode.Length >= 4)
                                    model.DepartmentCode = oCode[3];
                                if (oCode.Length >= 5)
                                    model.AdmissionWard = oCode[4];
                                if (oCode.Length >= 7)
                                    //model.ChargeDate = Convert.ToDateTime(oCode[6]);
                                    model.ChargeDateStr += String.IsNullOrEmpty(model.ChargeDateStr) ? oCode[6] : "," + oCode[6];
                                if (oCode.Length >= 9)
                                    model.ChargeBy += String.IsNullOrEmpty(model.ChargeBy) ? oCode[8] : "," + oCode[8];
                            }
                        }
                    }
                }

                else
                {
                    return RedirectToAction("SearchPatientOR", "OR");
                }
                #endregion
                #endregion
            }
            else
            {
                var orProgress = _orService.GetORAnesthProgress(Id.ToString(), (int)TypeSearchEnum.Id);
                if (orProgress != null)
                {
                    SetDataModel(ref model, orProgress);
                    #region Get charges/service from core OH
                    if (string.IsNullOrEmpty(model.ChargeDetailId))
                    {
                        model.Charges = _orService.GetChargesFromOH(model.HospitalCode, model.PId, CurrentUserId);
                        if (model.Charges?.Count > 0)
                            model.Charges = model.Charges.Where(x => x.ItemCode == model.HpServiceCode)?.ToList();
                    }
                    //vutv7 check trường hợp chỉ định quá hạn muốn thay thế chỉ định khác
                    else if (!string.IsNullOrEmpty(model.ChargeDetailId) && model.ChargeDate != null)
                    {
                        int ExprireMonth = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ExprireMonth"].ToString());
                        int months = (DateTime.Now.Year - model.ChargeDate.Value.Year) * 12 + (DateTime.Now.Month - model.ChargeDate.Value.Month);
                        if (months > ExprireMonth)
                        {
                            var lstChargeOH = _orService.GetChargesFromOH(model.HospitalCode, model.PId, CurrentUserId);
                            if (lstChargeOH.Count > 0)
                            {
                                foreach (var item in lstChargeOH.Where(x => x.ItemName == model.HpServiceName))
                                {
                                    months = (DateTime.Now.Year - item.ChargeDate.Year) * 12 + (DateTime.Now.Month - item.ChargeDate.Month);
                                    if (months <= ExprireMonth)
                                    {
                                        model.ChargesReplace.Add(item);
                                    }
                                }
                            }
                        }

                    }
                    #endregion
                }
                else
                {
                    return RedirectToAction("SearchPatientOR", "OR");
                }
            }

            #region master data
            var listEkipTypes = new List<int>()
            {
                (int)ORPositionEnum.PTVMain,
                (int)ORPositionEnum.PTVSub1,
                (int)ORPositionEnum.PTVSub2,
                (int)ORPositionEnum.PTVSub3,
                (int)ORPositionEnum.PTVSub4,
                (int)ORPositionEnum.PTVSub5,
                (int)ORPositionEnum.PTVSub6,
                (int)ORPositionEnum.PTVSub7,
                (int)ORPositionEnum.PTVSub8,
                (int)ORPositionEnum.CECDoctor,
                (int)ORPositionEnum.NurseTool1,
                (int)ORPositionEnum.NurseTool2,
                (int)ORPositionEnum.NurseOutRun1,
                (int)ORPositionEnum.NurseOutRun2,
                (int)ORPositionEnum.NurseOutRun3,
                (int)ORPositionEnum.NurseOutRun4,
                (int)ORPositionEnum.NurseOutRun5,
                (int)ORPositionEnum.NurseOutRun6,
                (int)ORPositionEnum.MainAnesthDoctor,
                (int)ORPositionEnum.SubAnesthDoctor,
                (int)ORPositionEnum.AnesthNurse1,
                (int)ORPositionEnum.AnesthNurse2,
                //vutv7
                (int)ORPositionEnum.KTVSubSurgery,
                (int)ORPositionEnum.KTVDiagnose,
                (int)ORPositionEnum.KTVCEC,
                (int)ORPositionEnum.DoctorDiagnose,
                (int)ORPositionEnum.DoctorNewBorn,
                (int)ORPositionEnum.Midwives,
            };
            var dataEkips = _orService.GetListORUsers(model.HospitalCode, listEkipTypes, 0, model.dtStart, model.dtEnd);
            dataEkips.Insert(0, new ORUserInfoContract()
            {
                Id = 0,
                Name = string.Empty,
                Email = string.Empty,
                PositionId = 0,
                Phone = string.Empty,
                PositionName = string.Empty,
                //linhht
                Username = string.Empty

            });
            if (dataEkips != null && dataEkips.Any())
                model.listEkips = dataEkips;
            #endregion
            model.ShowdtEnd = model.dtEnd.ToVEShortTime();
            model.ShowdtStart = model.dtStart.ToVEShortTime();
            #region Check access denied on location
            if ((model.HospitalCode != memberExtendedInfo.CurrentLocaltion.NameEN) ||
                (model.CreatedBy != CurrentUserId && !memberExtendedInfo.IsSuperAdmin && !memberExtendedInfo.IsManageAdminSurgery && !memberExtendedInfo.IsManageSurgery))
            {
                //Redirect to site access denied
                return new RedirectResult(new UrlHelper(HttpContext.Request.RequestContext).RouteUrl("ErrorHandler", new { id = -1 }));
            }
            #endregion .Check access denied on location
            ViewBag.ActionType = actionType;
            return View(model);
        }
        private ORAnesthManagementModel GetCoordinatorSurgicalFromVisitCurrent(ORAnesthManagementModel model, ORVisitModel v)
        {
            if (v == null) return model;
            model.PId = v.MA_BN;
            model.Address = v.DIA_CHI;
            model.Sex = v.GIOI_TINH;
            model.NgaySinh = v.NGAY_SINH;
            model.HoTen = v.HO_TEN;
            model.VisitCode = v.VISIT_CODE;
            model.HospitalCode = v.HospitalCode;
            model.HospitalName = v.HospitalName;
            model.HospitalPhone = v.HospitalPhone;
            model.PatientPhone = v.PatientPhone;
            model.Email = v.Email;
            model.Ages = v.Age;
            model.PatientPhone = v.PatientPhone;
            model.SurgeryType = v.SurgeryType;
            return model;
        }
        private void SetCoordinatorSurgicalFromVisitCurrent(ref ORAnesthProgressContract model, ORVisitModel v)
        {
            model.PId = v.MA_BN;
            model.Address = v.DIA_CHI;
            model.Sex = v.GIOI_TINH;
            model.NgaySinh = v.NGAY_SINH;
            model.HoTen = v.HO_TEN;
            model.VisitCode = v.VISIT_CODE;
            model.HospitalCode = v.HospitalCode;
            model.HospitalName = v.HospitalName;
            model.HospitalPhone = v.HospitalPhone;
            model.PatientPhone = v.PatientPhone;
            model.Email = v.Email;
            model.Ages = v.Age;
            model.PatientPhone = v.PatientPhone;
            model.SurgeryType = v.SurgeryType;
        }
        private void SetDataModel(ref ORAnesthManagementModel model, ORAnesthProgressContract r)
        {
            model.Id = r.Id;
            model.PId = r.PId;
            model.VisitCode = r.VisitCode;
            model.HoTen = r.HoTen;
            model.NgaySinh = r.NgaySinh;
            model.Sex = r.Sex;
            model.Address = r.Address;
            model.Email = r.Email;
            model.Ages = r.Ages;
            model.NameProject = r.NameProject;
            model.NameProject = r.NameProject;
            model.HpServiceId = r.HpServiceId;
            model.ORRoomId = r.ORRoomId;
            model.SurgeryType = r.SurgeryType == 0 ? 1 : r.SurgeryType;
            model.dtStart = r.dtStart ?? DateTime.Now;
            model.dtEnd = r.dtEnd ?? DateTime.Now;
            model.dtOperation = r.dtOperation ?? DateTime.Now;

            model.dtAdmission = r.dtAdmission ?? null;
            model.OrderID = r.OrderID;
            model.ChargeDetailId = r.ChargeDetailId;
            model.DepartmentCode = r.DepartmentCode;
            model.AdmissionWard = r.AdmissionWard;

            model.NamePTVMain = r.NamePTVMain;
            model.EmailPTVMain = r.EmailPTVMain;
            model.PhonePTVMain = r.PhonePTVMain;
            model.PositionPTVMain = r.PositionPTVMain;
            model.UIdPTVMain = r.UIdPTVMain ?? 0;

            model.NamePTVSub1 = r.NamePTVSub1;
            model.EmailPTVSub1 = r.EmailPTVSub1;
            model.PhonePTVSub1 = r.PhonePTVSub1;
            model.PositionPTVSub1 = r.PositionPTVSub1;
            model.UIdPTVSub1 = r.UIdPTVSub1 ?? 0;

            model.NamePTVSub2 = r.NamePTVSub2;
            model.EmailPTVSub2 = r.EmailPTVSub2;
            model.PhonePTVSub2 = r.PhonePTVSub2;
            model.PositionPTVSub2 = r.PositionPTVSub2;
            model.UIdPTVSub2 = r.UIdPTVSub2 ?? 0;

            model.NamePTVSub3 = r.NamePTVSub3;
            model.EmailPTVSub3 = r.EmailPTVSub3;
            model.PhonePTVSub3 = r.PhonePTVSub3;
            model.PositionPTVSub3 = r.PositionPTVSub3;
            model.UIdPTVSub3 = r.UIdPTVSub3 ?? 0;

            model.NamePTVSub4 = r.NamePTVSub4;
            model.EmailPTVSub4 = r.EmailPTVSub4;
            model.PhonePTVSub4 = r.PhonePTVSub4;
            model.PositionPTVSub4 = r.PositionPTVSub4;
            model.UIdPTVSub4 = r.UIdPTVSub4 ?? 0;

            model.NamePTVSub5 = r.NamePTVSub5;
            model.EmailPTVSub5 = r.EmailPTVSub5;
            model.PhonePTVSub5 = r.PhonePTVSub5;
            model.PositionPTVSub5 = r.PositionPTVSub5;
            model.UIdPTVSub5 = r.UIdPTVSub5 ?? 0;

            model.NamePTVSub6 = r.NamePTVSub6;
            model.EmailPTVSub6 = r.EmailPTVSub6;
            model.PhonePTVSub6 = r.PhonePTVSub6;
            model.PositionPTVSub6 = r.PositionPTVSub6;
            model.UIdPTVSub6 = r.UIdPTVSub6 ?? 0;

            model.NamePTVSub7 = r.NamePTVSub7;
            model.EmailPTVSub7 = r.EmailPTVSub7;
            model.PhonePTVSub7 = r.PhonePTVSub7;
            model.PositionPTVSub7 = r.PositionPTVSub7;
            model.UIdPTVSub7 = r.UIdPTVSub7 ?? 0;

            model.NamePTVSub8 = r.NamePTVSub8;
            model.EmailPTVSub8 = r.EmailPTVSub8;
            model.PhonePTVSub8 = r.PhonePTVSub8;
            model.PositionPTVSub8 = r.PositionPTVSub8;
            model.UIdPTVSub8 = r.UIdPTVSub8 ?? 0;

            model.NameCECDoctor = r.NameCECDoctor;
            model.EmailCECDoctor = r.EmailCECDoctor;
            model.PhoneCECDoctor = r.PhoneCECDoctor;
            model.PositionCECDoctor = r.PositionCECDoctor;
            model.UIdCECDoctor = r.UIdCECDoctor ?? 0;

            model.NameNurseTool1 = r.NameNurseTool1;
            model.EmailNurseTool1 = r.EmailNurseTool1;
            model.PhoneNurseTool1 = r.PhoneNurseTool1;
            model.PositionNurseTool1 = r.PositionNurseTool1;
            model.UIdNurseTool1 = r.UIdNurseTool1 ?? 0;

            model.NameNurseTool2 = r.NameNurseTool2;
            model.EmailNurseTool2 = r.EmailNurseTool2;
            model.PhoneNurseTool2 = r.PhoneNurseTool2;
            model.PositionNurseTool2 = r.PositionNurseTool2;
            model.UIdNurseTool2 = r.UIdNurseTool2 ?? 0;

            model.NameNurseOutRun1 = r.NameNurseOutRun1;
            model.EmailNurseOutRun1 = r.EmailNurseOutRun1;
            model.PhoneNurseOutRun1 = r.PhoneNurseOutRun1;
            model.PositionNurseOutRun1 = r.PositionNurseOutRun1;
            model.UIdNurseOutRun1 = r.UIdNurseOutRun1 ?? 0;

            model.NameNurseOutRun2 = r.NameNurseOutRun2;
            model.EmailNurseOutRun2 = r.EmailNurseOutRun2;
            model.PhoneNurseOutRun2 = r.PhoneNurseOutRun2;
            model.PositionNurseOutRun2 = r.PositionNurseOutRun2;
            model.UIdNurseOutRun2 = r.UIdNurseOutRun2 ?? 0;

            model.NameNurseOutRun3 = r.NameNurseOutRun3;
            model.EmailNurseOutRun3 = r.EmailNurseOutRun3;
            model.PhoneNurseOutRun3 = r.PhoneNurseOutRun3;
            model.PositionNurseOutRun3 = r.PositionNurseOutRun3;
            model.UIdNurseOutRun3 = r.UIdNurseOutRun3 ?? 0;

            model.NameNurseOutRun4 = r.NameNurseOutRun4;
            model.EmailNurseOutRun4 = r.EmailNurseOutRun4;
            model.PhoneNurseOutRun4 = r.PhoneNurseOutRun4;
            model.PositionNurseOutRun4 = r.PositionNurseOutRun4;
            model.UIdNurseOutRun4 = r.UIdNurseOutRun4 ?? 0;

            model.NameNurseOutRun5 = r.NameNurseOutRun5;
            model.EmailNurseOutRun5 = r.EmailNurseOutRun5;
            model.PhoneNurseOutRun5 = r.PhoneNurseOutRun5;
            model.PositionNurseOutRun5 = r.PositionNurseOutRun5;
            model.UIdNurseOutRun5 = r.UIdNurseOutRun5 ?? 0;

            model.NameNurseOutRun6 = r.NameNurseOutRun6;
            model.EmailNurseOutRun6 = r.EmailNurseOutRun6;
            model.PhoneNurseOutRun6 = r.PhoneNurseOutRun6;
            model.PositionNurseOutRun6 = r.PositionNurseOutRun6;
            model.UIdNurseOutRun6 = r.UIdNurseOutRun6 ?? 0;

            model.HospitalName = r.HospitalName;
            model.HospitalPhone = r.HospitalPhone;
            model.PatientPhone = r.PatientPhone;
            model.HospitalCode = r.HospitalCode;
            model.State = r.State;
            model.TimeAnesth = r.TimeAnesth ?? 0;
            model.SurgeryDescription = r.SurgeryDescription;
            model.ORRoomName = r.ORRoomName;
            model.HpServiceCode = r.HpServiceCode;
            model.HpServiceName = r.HpServiceName;

            model.CreatedBy = r.CreatedBy;
            model.NameCreatedBy = r.NameCreatedBy;
            model.EmailCreatedBy = r.EmailCreatedBy;
            model.listUserEkips = r.listUsers.Where(c => c.TypePageId == 1).ToList();
            //vutv7
            model.ChargeDate = r.ChargeDate;
            model.ChargeBy = r.ChargeBy;

            model.NameKTVSubSurgery = r.NameKTVSubSurgery;
            model.EmailKTVSubSurgery = r.EmailKTVSubSurgery;
            model.PhoneKTVSubSurgery = r.PhoneKTVSubSurgery;
            model.PositionKTVSubSurgery = r.PositionKTVSubSurgery;
            model.UIdKTVSubSurgery = r.UIdKTVSubSurgery ?? 0;

            model.NameKTVDiagnose = r.NameKTVDiagnose;
            model.EmailKTVDiagnose = r.EmailKTVDiagnose;
            model.PhoneKTVDiagnose = r.PhoneKTVDiagnose;
            model.PositionKTVDiagnose = r.PositionKTVDiagnose;
            model.UIdKTVDiagnose = r.UIdKTVDiagnose ?? 0;

            model.NameKTVCEC = r.NameKTVCEC;
            model.EmailKTVCEC = r.EmailKTVCEC;
            model.PhoneKTVCEC = r.PhoneKTVCEC;
            model.PositionKTVCEC = r.PositionKTVCEC;
            model.UIdKTVCEC = r.UIdKTVCEC ?? 0;

            model.NameDoctorDiagnose = r.NameDoctorDiagnose;
            model.EmailDoctorDiagnose = r.EmailDoctorDiagnose;
            model.PhoneDoctorDiagnose = r.PhoneDoctorDiagnose;
            model.PositionDoctorDiagnose = r.PositionDoctorDiagnose;
            model.UIdDoctorDiagnose = r.UIdDoctorDiagnose ?? 0;

            model.NameDoctorNewBorn = r.NameDoctorNewBorn;
            model.EmailDoctorNewBorn = r.EmailDoctorNewBorn;
            model.PhoneDoctorNewBorn = r.PhoneDoctorNewBorn;
            model.PositionDoctorNewBorn = r.PositionDoctorNewBorn;
            model.UIdDoctorNewBorn = r.UIdDoctorNewBorn ?? 0;

            model.NameMidwives = r.NameMidwives;
            model.EmailMidwives = r.EmailMidwives;
            model.PhoneMidwives = r.PhoneMidwives;
            model.PositionMidwives = r.PositionMidwives;
            model.UIdMidwives = r.UIdMidwives ?? 0;

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveORManagementByJson(int UIdPTVMain, int UIdPTVSub1, int UIdPTVSub2, int UIdPTVSub3, int UIdPTVSub4, int UIdPTVSub5, int UIdPTVSub6, int UIdPTVSub7, int UIdPTVSub8, int UIdCECDoctor, int UIdNurseTool1, int UIdNurseTool2, int UIdNurseOutRun1, int UIdNurseOutRun2, int UIdNurseOutRun3, int UIdNurseOutRun4, int UIdNurseOutRun5, int UIdNurseOutRun6, int UIdKTVSubSurgery, int UIdKTVDiagnose, int UIdKTVCEC, int UIdDoctorDiagnose, int UIdDoctorNewBorn, int UIdMidwives, string EmailPTVMain
            , string EmailPTVSub1, string EmailPTVSub2, string EmailPTVSub3, string EmailPTVSub4, string EmailPTVSub5, string EmailPTVSub6, string EmailPTVSub7, string EmailPTVSub8, string EmailCECDoctor, string EmailNurseTool1, string EmailNurseTool2, string EmailNurseOutRun1, string EmailNurseOutRun2, string EmailNurseOutRun3, string EmailNurseOutRun4, string EmailNurseOutRun5, string EmailNurseOutRun6
            , string EmailKTVSubSurgery, string EmailKTVDiagnose, string EmailKTVCEC, string EmailDoctorDiagnose, string EmailDoctorNewBorn, string EmailMidwives
            , string PhonePTVMain, string PhonePTVSub1, string PhonePTVSub2, string PhonePTVSub3, string PhonePTVSub4, string PhonePTVSub5, string PhonePTVSub6, string PhonePTVSub7, string PhonePTVSub8, string PhoneCECDoctor, string PhoneNurseTool1, string PhoneNurseTool2
            , string PhoneNurseOutRun1, string PhoneNurseOutRun2, string PhoneNurseOutRun3, string PhoneNurseOutRun4, string PhoneNurseOutRun5, string PhoneNurseOutRun6
            , string PhoneKTVSubSurgery, string PhoneKTVDiagnose, string PhoneKTVCEC, string PhoneDoctorDiagnose, string PhoneDoctorNewBorn, string PhoneMidwives
            , int State, int Id, string PId, string SurgeryDescription, int surgeryType, string OrderID, string ChargeDetailId, string DepartmentCode, string AdmissionWard, int actionType = 1, string ChargeDate = "", string ChargeBy = "")
        {
            ActionMessage actionMessage;
            var emailRegex = @"^[a-zA-Z]+[a-zA-Z0-9._-]+@[a-zA-Z0-9]{2,}(\.[a-zA-Z0-9]{2,4}){1,2}$";
            if (ModelState.IsValid && (State == (int)ORProgressStateEnum.NoApproveSurgeryManager || Constant.ListStateAllowCoordinator.Contains(State)))
            {
                //Truong hop phe duyet moi kiem tra va cap nhat thong tin email, phone mumber
                if (State == 3 || Constant.ListStateAllowCoordinator.Contains(State))
                {
                    if (string.IsNullOrEmpty(ChargeDetailId) && State != (int)ORProgressStateEnum.AssignEkip)
                    {
                        return Json(new ActionMessage(-1, "Vui lòng chọn chỉ định trước khi ghi nhận ekip!"));
                    }
                    if (IsEmail(true, emailRegex, EmailPTVMain, "Email PTV chính") != null)
                    {
                        return IsEmail(true, emailRegex, EmailPTVMain, "Email PTV chính");
                    };

                    if (IsEmail(false, emailRegex, EmailPTVSub1, "Email PTV phụ 1") != null)
                    {
                        return IsEmail(false, emailRegex, EmailPTVSub1, "Email PTV phụ 1");
                    };

                    if (IsEmail(false, emailRegex, EmailPTVSub2, "Email PTV phụ 2") != null)
                    {
                        return IsEmail(false, emailRegex, EmailPTVSub2, "Email PTV phụ 2");
                    };

                    if (IsEmail(false, emailRegex, EmailPTVSub3, "Email PTV phụ 3") != null)
                    {
                        return IsEmail(false, emailRegex, EmailPTVSub3, "Email PTV phụ 3");
                    };
                    if (IsEmail(false, emailRegex, EmailPTVSub4, "Email PTV phụ 4") != null)
                    {
                        return IsEmail(false, emailRegex, EmailPTVSub4, "Email PTV phụ 4");
                    };
                    if (IsEmail(false, emailRegex, EmailPTVSub5, "Email PTV phụ 5") != null)
                    {
                        return IsEmail(false, emailRegex, EmailPTVSub5, "Email PTV phụ 5");
                    };
                    if (IsEmail(false, emailRegex, EmailPTVSub6, "Email PTV phụ 6") != null)
                    {
                        return IsEmail(false, emailRegex, EmailPTVSub6, "Email PTV phụ 6");
                    };
                    if (IsEmail(false, emailRegex, EmailPTVSub7, "Email PTV phụ 7") != null)
                    {
                        return IsEmail(false, emailRegex, EmailPTVSub7, "Email PTV phụ 7");
                    };
                    if (IsEmail(false, emailRegex, EmailPTVSub8, "Email PTV phụ 8") != null)
                    {
                        return IsEmail(false, emailRegex, EmailPTVSub8, "Email PTV phụ 8");
                    };

                    if (IsEmail(false, emailRegex, EmailCECDoctor, "Email PTV CEC") != null)
                    {
                        return IsEmail(false, emailRegex, EmailCECDoctor, "Email PTV CEC");
                    };

                    if (IsEmail(true, emailRegex, EmailNurseTool1, "Email điều dưỡng dụng cụ 1") != null)
                    {
                        return IsEmail(true, emailRegex, EmailNurseTool1, "Email điều dưỡng dụng cụ 1");
                    };

                    if (IsEmail(false, emailRegex, EmailNurseTool2, "Email điều dưỡng dụng cụ 2") != null)
                    {
                        return IsEmail(false, emailRegex, EmailNurseTool2, "Email điều dưỡng dụng cụ 2");
                    };

                    if (IsEmail(true, emailRegex, EmailNurseOutRun1, "Email điều dưỡng chạy ngoài 1") != null)
                    {
                        return IsEmail(true, emailRegex, EmailNurseOutRun1, "Email điều dưỡng chạy ngoài 1");
                    };

                    if (IsEmail(false, emailRegex, EmailNurseOutRun2, "Email điều dưỡng chạy ngoài 2") != null)
                    {
                        return IsEmail(false, emailRegex, EmailNurseOutRun2, "Email điều dưỡng chạy ngoài 2");
                    };
                    if (IsEmail(false, emailRegex, EmailNurseOutRun3, "Email điều dưỡng chạy ngoài 3") != null)
                    {
                        return IsEmail(false, emailRegex, EmailNurseOutRun3, "Email điều dưỡng chạy ngoài 3");
                    };
                    if (IsEmail(false, emailRegex, EmailNurseOutRun4, "Email điều dưỡng chạy ngoài 4") != null)
                    {
                        return IsEmail(false, emailRegex, EmailNurseOutRun4, "Email điều dưỡng chạy ngoài 4");
                    };
                    if (IsEmail(false, emailRegex, EmailNurseOutRun5, "Email điều dưỡng chạy ngoài 5") != null)
                    {
                        return IsEmail(false, emailRegex, EmailNurseOutRun5, "Email điều dưỡng chạy ngoài 5");
                    };
                    if (IsEmail(false, emailRegex, EmailNurseOutRun6, "Email điều dưỡng chạy ngoài 6") != null)
                    {
                        return IsEmail(false, emailRegex, EmailNurseOutRun6, "Email điều dưỡng chạy ngoài 6");
                    };
                    //vutv7
                    if (IsEmail(false, emailRegex, EmailKTVSubSurgery, "Email KTV phụ mổ") != null)
                    {
                        return IsEmail(false, emailRegex, EmailKTVSubSurgery, "Email KTV phụ mổ");
                    };

                    if (IsEmail(false, emailRegex, EmailKTVDiagnose, "Email KTV CĐHA") != null)
                    {
                        return IsEmail(false, emailRegex, EmailKTVDiagnose, "Email KTV CĐHA");
                    };
                    if (IsEmail(false, emailRegex, EmailKTVCEC, "Email KTV chạy máy CEC") != null)
                    {
                        return IsEmail(false, emailRegex, EmailKTVCEC, "Email KTV chạy máy CEC");
                    };
                    if (IsEmail(false, emailRegex, EmailDoctorDiagnose, "Email BS CĐHA") != null)
                    {
                        return IsEmail(false, emailRegex, EmailDoctorDiagnose, "Email BS CĐHA");
                    };
                    if (IsEmail(false, emailRegex, EmailDoctorNewBorn, "Email BS sơ sinh") != null)
                    {
                        return IsEmail(false, emailRegex, EmailDoctorNewBorn, "Email BS sơ sinh");
                    };
                    if (IsEmail(false, emailRegex, EmailMidwives, "Email Nữ hộ sinh") != null)
                    {
                        return IsEmail(false, emailRegex, EmailMidwives, "Email Nữ hộ sinh");
                    };
                    var ptvMain = _userMngtCaching.UpdateEmailOrPhone(UIdPTVMain, Sanitizer.GetSafeHtmlFragment(EmailPTVMain).ToLower(), Sanitizer.GetSafeHtmlFragment(PhonePTVMain));
                    if (ptvMain != null)
                    {
                        if (ptvMain.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                            return Json(new ActionMessage(-1, "Không tìm thấy PTV chính"));
                        else
                            return Json(ptvMain);
                    }

                    if (UIdPTVSub1 > 0)
                    {
                        var ptvSub1 = _userMngtCaching.UpdateEmailOrPhone(UIdPTVSub1, Sanitizer.GetSafeHtmlFragment(EmailPTVSub1).ToLower(), Sanitizer.GetSafeHtmlFragment(PhonePTVSub1));
                        if (ptvSub1 != null)
                        {
                            if (ptvSub1.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy PTV phụ 1"));
                            else
                                return Json(ptvSub1);
                        }
                    }

                    if (UIdPTVSub2 > 0)
                    {
                        var ptvSub2 = _userMngtCaching.UpdateEmailOrPhone(UIdPTVSub2, Sanitizer.GetSafeHtmlFragment(EmailPTVSub2).ToLower(), Sanitizer.GetSafeHtmlFragment(PhonePTVSub2));
                        if (ptvSub2 != null)
                        {
                            if (ptvSub2.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy PTV phụ 2"));
                            else
                                return Json(ptvSub2);
                        }
                    }

                    if (UIdPTVSub3 > 0)
                    {
                        var ptvSub3 = _userMngtCaching.UpdateEmailOrPhone(UIdPTVSub3, Sanitizer.GetSafeHtmlFragment(EmailPTVSub3).ToLower(), Sanitizer.GetSafeHtmlFragment(PhonePTVSub3));
                        if (ptvSub3 != null)
                        {
                            if (ptvSub3.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy PTV phụ 3"));
                            else
                                return Json(ptvSub3);
                        }
                    }
                    if (UIdPTVSub4 > 0)
                    {
                        var ptvSub4 = _userMngtCaching.UpdateEmailOrPhone(UIdPTVSub4, Sanitizer.GetSafeHtmlFragment(EmailPTVSub4).ToLower(), Sanitizer.GetSafeHtmlFragment(PhonePTVSub4));
                        if (ptvSub4 != null)
                        {
                            if (ptvSub4.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy PTV phụ 4"));
                            else
                                return Json(ptvSub4);
                        }
                    }
                    if (UIdPTVSub5 > 0)
                    {
                        var ptvSub5 = _userMngtCaching.UpdateEmailOrPhone(UIdPTVSub5, Sanitizer.GetSafeHtmlFragment(EmailPTVSub5).ToLower(), Sanitizer.GetSafeHtmlFragment(PhonePTVSub5));
                        if (ptvSub5 != null)
                        {
                            if (ptvSub5.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy PTV phụ 5"));
                            else
                                return Json(ptvSub5);
                        }
                    }
                    if (UIdPTVSub6 > 0)
                    {
                        var ptvSub6 = _userMngtCaching.UpdateEmailOrPhone(UIdPTVSub6, Sanitizer.GetSafeHtmlFragment(EmailPTVSub6).ToLower(), Sanitizer.GetSafeHtmlFragment(PhonePTVSub6));
                        if (ptvSub6 != null)
                        {
                            if (ptvSub6.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy PTV phụ 6"));
                            else
                                return Json(ptvSub6);
                        }
                    }
                    if (UIdPTVSub7 > 0)
                    {
                        var ptvSub7 = _userMngtCaching.UpdateEmailOrPhone(UIdPTVSub7, Sanitizer.GetSafeHtmlFragment(EmailPTVSub7).ToLower(), Sanitizer.GetSafeHtmlFragment(PhonePTVSub6));
                        if (ptvSub7 != null)
                        {
                            if (ptvSub7.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy PTV phụ 7"));
                            else
                                return Json(ptvSub7);
                        }
                    }
                    if (UIdPTVSub8 > 0)
                    {
                        var ptvSub8 = _userMngtCaching.UpdateEmailOrPhone(UIdPTVSub8, Sanitizer.GetSafeHtmlFragment(EmailPTVSub8).ToLower(), Sanitizer.GetSafeHtmlFragment(PhonePTVSub8));
                        if (ptvSub8 != null)
                        {
                            if (ptvSub8.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy PTV phụ 8"));
                            else
                                return Json(ptvSub8);
                        }
                    }
                    if (UIdCECDoctor > 0)
                    {
                        var ptvCEC = _userMngtCaching.UpdateEmailOrPhone(UIdCECDoctor, Sanitizer.GetSafeHtmlFragment(EmailCECDoctor).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneCECDoctor));
                        if (ptvCEC != null)
                        {
                            if (ptvCEC.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy PTV CEC"));
                            else
                                return Json(ptvCEC);
                        }
                    }

                    var nurseTool1 = _userMngtCaching.UpdateEmailOrPhone(UIdNurseTool1, Sanitizer.GetSafeHtmlFragment(EmailNurseTool1).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneNurseTool1));
                    if (nurseTool1 != null)
                    {
                        if (nurseTool1.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                            return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng dụng cụ 1"));
                        else
                            return Json(nurseTool1);
                    }

                    if (UIdNurseTool2 > 0)
                    {
                        var nurseTool2 = _userMngtCaching.UpdateEmailOrPhone(UIdNurseTool2, Sanitizer.GetSafeHtmlFragment(EmailNurseTool2).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneNurseTool2));
                        if (nurseTool2 != null)
                        {
                            if (nurseTool2.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng dụng cụ 2"));
                            else
                                return Json(nurseTool2);
                        }
                    }

                    var nurseOutRun1 = _userMngtCaching.UpdateEmailOrPhone(UIdNurseOutRun1, Sanitizer.GetSafeHtmlFragment(EmailNurseOutRun1).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneNurseOutRun1));
                    if (nurseOutRun1 != null)
                    {
                        if (nurseOutRun1.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                            return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng chạy ngoài 1"));
                        else
                            return Json(nurseOutRun1);
                    }

                    if (UIdNurseOutRun2 > 0)
                    {
                        var nurseOutRun2 = _userMngtCaching.UpdateEmailOrPhone(UIdNurseOutRun2, Sanitizer.GetSafeHtmlFragment(EmailNurseOutRun2).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneNurseOutRun2));
                        if (nurseOutRun2 != null)
                        {
                            if (nurseOutRun2.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng chạy ngoài 2"));
                            else
                                return Json(nurseOutRun2);
                        }
                    }
                    if (UIdNurseOutRun3 > 0)
                    {
                        var nurseOutRun3 = _userMngtCaching.UpdateEmailOrPhone(UIdNurseOutRun3, Sanitizer.GetSafeHtmlFragment(EmailNurseOutRun3).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneNurseOutRun3));
                        if (nurseOutRun3 != null)
                        {
                            if (nurseOutRun3.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng chạy ngoài 3"));
                            else
                                return Json(nurseOutRun3);
                        }
                    }
                    if (UIdNurseOutRun4 > 0)
                    {
                        var nurseOutRun4 = _userMngtCaching.UpdateEmailOrPhone(UIdNurseOutRun4, Sanitizer.GetSafeHtmlFragment(EmailNurseOutRun4).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneNurseOutRun4));
                        if (nurseOutRun4 != null)
                        {
                            if (nurseOutRun4.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng chạy ngoài 4"));
                            else
                                return Json(nurseOutRun4);
                        }
                    }
                    if (UIdNurseOutRun5 > 0)
                    {
                        var nurseOutRun5 = _userMngtCaching.UpdateEmailOrPhone(UIdNurseOutRun5, Sanitizer.GetSafeHtmlFragment(EmailNurseOutRun5).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneNurseOutRun5));
                        if (nurseOutRun5 != null)
                        {
                            if (nurseOutRun5.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng chạy ngoài 5"));
                            else
                                return Json(nurseOutRun5);
                        }
                    }

                    if (UIdNurseOutRun6 > 0)
                    {
                        var nurseOutRun6 = _userMngtCaching.UpdateEmailOrPhone(UIdNurseOutRun6, Sanitizer.GetSafeHtmlFragment(EmailNurseOutRun6).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneNurseOutRun6));
                        if (nurseOutRun6 != null)
                        {
                            if (nurseOutRun6.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng chạy ngoài 6"));
                            else
                                return Json(nurseOutRun6);
                        }
                    }
                    //vutv7 update các trường mới thêm 
                    if (UIdKTVSubSurgery > 0)
                    {
                        var ktvSubSurgery = _userMngtCaching.UpdateEmailOrPhone(UIdKTVSubSurgery, Sanitizer.GetSafeHtmlFragment(EmailKTVSubSurgery).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneKTVSubSurgery));
                        if (ktvSubSurgery != null)
                        {
                            if (ktvSubSurgery.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy KTV phụ mổ"));
                            else
                                return Json(ktvSubSurgery);
                        }
                    }
                    if (UIdKTVDiagnose > 0)
                    {
                        var ktvDiagnose = _userMngtCaching.UpdateEmailOrPhone(UIdKTVDiagnose, Sanitizer.GetSafeHtmlFragment(EmailKTVDiagnose).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneKTVDiagnose));
                        if (ktvDiagnose != null)
                        {
                            if (ktvDiagnose.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy KTV CĐHA"));
                            else
                                return Json(ktvDiagnose);
                        }
                    }
                    if (UIdKTVCEC > 0)
                    {
                        var ktvCEC = _userMngtCaching.UpdateEmailOrPhone(UIdKTVCEC, Sanitizer.GetSafeHtmlFragment(EmailKTVCEC).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneKTVCEC));
                        if (ktvCEC != null)
                        {
                            if (ktvCEC.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy KTV chạy máy CEC"));
                            else
                                return Json(ktvCEC);
                        }
                    }
                    if (UIdDoctorDiagnose > 0)
                    {
                        var doctorDiagnose = _userMngtCaching.UpdateEmailOrPhone(UIdDoctorDiagnose, Sanitizer.GetSafeHtmlFragment(EmailDoctorDiagnose).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneDoctorDiagnose));
                        if (doctorDiagnose != null)
                        {
                            if (doctorDiagnose.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy BS CĐHA"));
                            else
                                return Json(doctorDiagnose);
                        }
                    }
                    if (UIdDoctorNewBorn > 0)
                    {
                        var doctorNewBorn = _userMngtCaching.UpdateEmailOrPhone(UIdDoctorNewBorn, Sanitizer.GetSafeHtmlFragment(EmailDoctorNewBorn).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneDoctorNewBorn));
                        if (doctorNewBorn != null)
                        {
                            if (doctorNewBorn.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy BS sơ sinh"));
                            else
                                return Json(doctorNewBorn);
                        }
                    }
                    if (UIdMidwives > 0)
                    {
                        var midwives = _userMngtCaching.UpdateEmailOrPhone(UIdMidwives, Sanitizer.GetSafeHtmlFragment(EmailMidwives).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneMidwives));
                        if (midwives != null)
                        {
                            if (midwives.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                                return Json(new ActionMessage(-1, "Không tìm thấy Nữ hộ sinh"));
                            else
                                return Json(midwives);
                        }
                    }
                }
                //vutv7 xử lý trường hợp chỉ định quá hạn
                var splitChargeDate = ChargeDate.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
                if (splitChargeDate.Count() == 1)
                {
                    int ExprireMonth = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ExprireMonth"].ToString());
                    int months = (DateTime.Now.Year - Convert.ToDateTime(ChargeDate).Year) * 12 + (DateTime.Now.Month - Convert.ToDateTime(ChargeDate).Month);
                    if (months > ExprireMonth)
                    {
                        return Json(new ActionMessage(-1, "Chỉ định cho ca mổ này đã vượt quá 2 tháng cho phép để ghi nhận doanh thu theo quy đinh."));
                    }
                }

                var contract = new ORAnesthProgressContract();
                contract.Id = Id;
                contract.HospitalCode = CurrentLoc.NameEN;
                contract.PId = PId;
                if (actionType == 2 && contract.Id <= 0)
                {
                    //get from model
                    ORVisitModel v = _visitService.GetCurrentVisit();
                    if (v == null)
                    {
                        return Json(new ActionMessage(-1, "tham số không hợp lệ"));
                    }
                    SetCoordinatorSurgicalFromVisitCurrent(ref contract, v);
                    #region GetServiceId
                    //if (!string.IsNullOrEmpty(v.PatientService))
                    //{
                    //    var oCode = v.PatientService.Split(new[] { "//" }, StringSplitOptions.None);
                    //    if (oCode.Count() > 1)
                    //    {
                    //        var svEntity = _orService.GetServiceByCode(oCode[1]);
                    //        if (svEntity != null)
                    //        {
                    //            contract.HpServiceId = svEntity.Id;
                    //            contract.HpServiceName = svEntity.Name;
                    //            contract.dtStart = DateTime.Now.AddMinutes(-svEntity.AnesthesiaTime);
                    //            contract.dtEnd = DateTime.Now.AddMinutes(svEntity.CleaningTime);
                    //            contract.dtOperation = DateTime.Now;
                    //            contract.TimeAnesth = svEntity.AnesthesiaTime;
                    //            contract.VisitCode = Guid.NewGuid().ToString();
                    //        }
                    //        contract.OrderID = oCode[0];
                    //        if (oCode.Length >= 3)
                    //            contract.ChargeDetailId = oCode[2];
                    //        if (oCode.Length >= 4)
                    //            contract.DepartmentCode = oCode[3];
                    //        if (oCode.Length >= 5)
                    //            contract.AdmissionWard = oCode[4];
                    //        if (oCode.Length >= 7)
                    //        {
                    //            contract.ChargeDate = Convert.ToDateTime(oCode[6]);
                    //        }
                    //    }
                    //}
                    //vutv7 - ghi nhận ekip theo lô
                    if (!string.IsNullOrEmpty(v.PatientService))
                    {
                        var lstPatienSr = v.PatientService.Split(new[] { ";" }, System.StringSplitOptions.RemoveEmptyEntries);
                        if (lstPatienSr.Count() > 0)
                        {
                            foreach (var item in lstPatienSr)
                            {
                                var oCode = item.Split(new[] { "//" }, System.StringSplitOptions.RemoveEmptyEntries);
                                if (oCode.Count() > 1)
                                {
                                    var svEntity = _orService.GetServiceByCode(oCode[1]);
                                    if (svEntity != null)
                                    {
                                        contract.HpServiceId = svEntity.Id;
                                        contract.HpServiceName = svEntity.Name;
                                        contract.dtStart = DateTime.Now.AddMinutes(-svEntity.AnesthesiaTime);
                                        contract.dtEnd = DateTime.Now.AddMinutes(svEntity.CleaningTime);
                                        contract.dtOperation = DateTime.Now;
                                        contract.TimeAnesth = svEntity.AnesthesiaTime;
                                        contract.VisitCode = Guid.NewGuid().ToString();
                                    }
                                    contract.OrderID = oCode[0];
                                    if (oCode.Length >= 3)
                                        contract.ChargeDetailId += string.IsNullOrEmpty(contract.ChargeDetailId) ? oCode[2] : "," + oCode[2];
                                    if (oCode.Length >= 4)
                                        contract.DepartmentCode = oCode[3];
                                    if (oCode.Length >= 5)
                                        contract.AdmissionWard = oCode[4];
                                    if (oCode.Length >= 7)
                                    {
                                        contract.ChargeDateStr += string.IsNullOrEmpty(contract.ChargeDateStr) ? oCode[6] : "," + oCode[6];
                                        contract.ChargeDate = Convert.ToDateTime(oCode[6]);
                                    }
                                    if (oCode.Length >= 9)
                                        contract.ChargeBy += string.IsNullOrEmpty(contract.ChargeBy) ? oCode[8] : "," + oCode[8];
                                }
                            }
                        }
                    }
                    #endregion
                    contract.SurgeryType = surgeryType;
                    contract.CreatedBy = CurrentUserId;
                }
                else if (Constant.ListStateAllowCoordinator.Contains(State))
                {
                    contract.OrderID = OrderID;
                    contract.ChargeDetailId = ChargeDetailId;
                    contract.DepartmentCode = DepartmentCode;
                    contract.AdmissionWard = AdmissionWard;
                    //vutv7
                    if (!string.IsNullOrEmpty(ChargeDate))
                    {
                        contract.ChargeDate = Convert.ToDateTime(ChargeDate);
                        //vutv7 - thêm trường hợp điều phối mổ
                        contract.ChargeDateStr = ChargeDate;
                    }
                    contract.ChargeBy = ChargeBy;
                }

                //ext
                contract.UIdPTVMain = UIdPTVMain;
                contract.UIdPTVSub1 = UIdPTVSub1;
                contract.UIdPTVSub2 = UIdPTVSub2;
                contract.UIdPTVSub3 = UIdPTVSub3;
                contract.UIdPTVSub4 = UIdPTVSub4;
                contract.UIdPTVSub5 = UIdPTVSub5;
                contract.UIdPTVSub6 = UIdPTVSub6;
                contract.UIdPTVSub7 = UIdPTVSub7;
                contract.UIdPTVSub8 = UIdPTVSub8;
                contract.UIdCECDoctor = UIdCECDoctor;
                contract.UIdNurseTool1 = UIdNurseTool1;
                contract.UIdNurseTool2 = UIdNurseTool2;
                contract.UIdNurseOutRun1 = UIdNurseOutRun1;
                contract.UIdNurseOutRun2 = UIdNurseOutRun2;
                contract.UIdNurseOutRun3 = UIdNurseOutRun3;
                contract.UIdNurseOutRun4 = UIdNurseOutRun4;
                contract.UIdNurseOutRun5 = UIdNurseOutRun5;
                contract.UIdNurseOutRun6 = UIdNurseOutRun6;
                //vutv7
                contract.UIdKTVSubSurgery = UIdKTVSubSurgery;
                contract.UIdKTVDiagnose = UIdKTVDiagnose;
                contract.UIdKTVCEC = UIdKTVCEC;
                contract.UIdDoctorDiagnose = UIdDoctorDiagnose;
                contract.UIdDoctorNewBorn = UIdDoctorNewBorn;
                contract.UIdMidwives = UIdMidwives;

                contract.State = State;
                contract.SurgeryDescription = SurgeryDescription;

                contract.NamePTVMain = _userMngtCaching.GetFullNameById(UIdPTVMain)?.Name;
                contract.NamePTVSub1 = _userMngtCaching.GetFullNameById(UIdPTVSub1)?.Name;
                contract.NamePTVSub2 = _userMngtCaching.GetFullNameById(UIdPTVSub2)?.Name;
                contract.NamePTVSub3 = _userMngtCaching.GetFullNameById(UIdPTVSub3)?.Name;
                contract.NamePTVSub4 = _userMngtCaching.GetFullNameById(UIdPTVSub4)?.Name;
                contract.NamePTVSub5 = _userMngtCaching.GetFullNameById(UIdPTVSub5)?.Name;
                contract.NamePTVSub6 = _userMngtCaching.GetFullNameById(UIdPTVSub6)?.Name;
                contract.NamePTVSub7 = _userMngtCaching.GetFullNameById(UIdPTVSub7)?.Name;
                contract.NamePTVSub8 = _userMngtCaching.GetFullNameById(UIdPTVSub8)?.Name;
                contract.NameCECDoctor = _userMngtCaching.GetFullNameById(UIdCECDoctor)?.Name;
                contract.NameNurseTool1 = _userMngtCaching.GetFullNameById(UIdNurseTool1)?.Name;
                contract.NameNurseTool2 = _userMngtCaching.GetFullNameById(UIdNurseTool2)?.Name;
                contract.NameNurseOutRun1 = _userMngtCaching.GetFullNameById(UIdNurseOutRun1)?.Name;
                contract.NameNurseOutRun2 = _userMngtCaching.GetFullNameById(UIdNurseOutRun2)?.Name;
                contract.NameNurseOutRun3 = _userMngtCaching.GetFullNameById(UIdNurseOutRun3)?.Name;
                contract.NameNurseOutRun4 = _userMngtCaching.GetFullNameById(UIdNurseOutRun4)?.Name;
                contract.NameNurseOutRun5 = _userMngtCaching.GetFullNameById(UIdNurseOutRun5)?.Name;
                contract.NameNurseOutRun6 = _userMngtCaching.GetFullNameById(UIdNurseOutRun6)?.Name;

                contract.NameKTVSubSurgery = _userMngtCaching.GetFullNameById(UIdKTVSubSurgery)?.Name;
                contract.NameKTVDiagnose = _userMngtCaching.GetFullNameById(UIdKTVDiagnose)?.Name;
                contract.NameKTVCEC = _userMngtCaching.GetFullNameById(UIdKTVCEC)?.Name;
                contract.NameDoctorDiagnose = _userMngtCaching.GetFullNameById(UIdDoctorDiagnose)?.Name;
                contract.NameDoctorNewBorn = _userMngtCaching.GetFullNameById(UIdDoctorNewBorn)?.Name;
                contract.NameMidwives = _userMngtCaching.GetFullNameById(UIdMidwives)?.Name;
                //Check Role permission for location data
                #region Check Role permission for location data
                if (contract.HospitalCode != CurrentLoc.NameEN)
                {
                    //Return Invalid current location with Data Location
                    return Json(new ActionMessage(new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied)));
                }
                #endregion .Check Role permission for location data
                //vutv7
                var lstChargeDetailId = !string.IsNullOrEmpty(contract.ChargeDetailId) ? contract.ChargeDetailId.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries) : new string[0];
                var lstChargeDate = !string.IsNullOrEmpty(contract.ChargeDateStr) ? contract.ChargeDateStr.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries) : new string[0];
                var lstChargeBy = !string.IsNullOrEmpty(contract.ChargeBy) ? contract.ChargeBy.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries) : new string[0];
                if (lstChargeDetailId.Count() > 1 && (lstChargeDetailId.Count() == lstChargeDate.Count()))
                {
                    for (int i = 0; i < lstChargeDetailId.Count(); i++)
                    {
                        contract.Id = Id;
                        contract.ChargeDetailId = lstChargeDetailId[i];
                        contract.ChargeDate = Convert.ToDateTime(lstChargeDate[i]);
                        contract.ChargeBy = lstChargeBy[i];
                        CUDReturnMessage response = _orService.SaveORManagementByJson(contract, CurrentUserId, actionType);

                        if (response.Id > 0)
                        {
                            if (response.Id != (int)ResponseCode.AdminRole_Accessdenied)
                            {
                                //Cập nhật sang DIMS để thực hiện tính toán
                                #region Cập nhật sang DIMS để tính toán lại
                                _syncData.UpdateDimsToReCalculate(contract.ChargeDetailId);
                                #endregion
                                actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.OR_SuccessUpdate));
                            }
                            else
                                actionMessage = new ActionMessage(response.Id, response.Message);
                            if (actionType == 2 && contract.Id <= 0)
                            {
                                _visitService.SetCurrentVisit(null);
                            }
                        }
                        else
                        {
                            //actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.Error));
                            actionMessage = new ActionMessage(response.Id, response.Message);
                        }
                        //var actionMessageExt = new ActionMessage(response);
                        //return Json(actionMessageExt);
                    }
                    return Json(new ActionMessage(1, "Success!"));
                }
                else
                {
                    CUDReturnMessage response = _orService.SaveORManagementByJson(contract, CurrentUserId, actionType);

                    if (response.Id > 0)
                    {
                        if (response.Id != (int)ResponseCode.AdminRole_Accessdenied)
                        {
                            //Cập nhật sang DIMS để thực hiện tính toán
                            #region Cập nhật sang DIMS để tính toán lại
                            _syncData.UpdateDimsToReCalculate(contract.ChargeDetailId);
                            #endregion
                            actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.OR_SuccessUpdate));
                        }
                        else
                            actionMessage = new ActionMessage(response.Id, response.Message);
                        #region sendmail 
                        //var model = _orService.GetORAnesthProgress(response.Id.ToString(), (int)TypeSearchEnum.Id);
                        //if (model != null) { 
                        //    if (State == (int)ORProgressStateEnum.NoApproveSurgeryManager)
                        //    {
                        //        #region send mail black to registor doctor


                        //        string hrefLink = MailHelper.DomainMailUri + "dang-ky-phong-mo" + "?Id=" + response.Id;
                        //        var dataLink = new ORLinkContract()
                        //        {
                        //            GuidCode = Guid.NewGuid(),
                        //            Code = hrefLink,
                        //            LimitDate = DateTime.Now.AddDays(30),
                        //            IsActive = false,
                        //            IpActive = string.Empty
                        //        };
                        //        _orService.CUDOperationLink(dataLink, CurrentUserId);
                        //        string hrefActiveCode = MailHelper.DomainMailUri + "kich-hoat-link" + "?code=" + dataLink.GuidCode;

                        //        var contentMail = string.Format(@"Dear Anh/Chị Bác Sĩ {0}. <br/> 
                        //                                         Hiện tại thông tin đăng ký ca mổ của anh chị với thông tin sau : <br/>
                        //                                          - Họ tên bệnh nhân: {1} <br/>
                        //                                          - Ngày tháng năm sinh: {2} <br/> 
                        //                                          - Tên dịch vụ mổ: {3} <br/>
                        //                                          - Thời gian gây mê: {4} <br/>
                        //                                          - Phòng mổ yêu cầu: {5} <br/>
                        //                                          - Ngày thực hiện yêu cầu: {6} <br/>
                        //                                          - Thời gian bắt đầu: {7} <br/>
                        //                                          - Thời gian kết thúc: {8} <br/>
                        //        Thông tin ca mổ này đã được quản lý phòng mổ {10} từ chối,<br/>
                        //        Anh/Chị click vào link sau đây <a href={9}> Click xem chi tiết </a> để xem đầy đủ thông tin . <br/>
                        //        Trân trọng.<br/>", model.NameCreatedBy, model.HoTen, (model.NgaySinh.ToVEShortDate() + " " + model.NgaySinh.ToVEShortTime()), model.HpServiceName, model.TimeAnesth, model.ORRoomName, ((model.dtOperation??DateTime.Now).ToVEShortDate() + " " + (model.dtOperation??DateTime.Now).ToVEShortTime()) , ((model.dtStart ?? DateTime.Now).ToVEShortDate() + " " + (model.dtStart ?? DateTime.Now).ToVEShortTime()), ((model.dtEnd ?? DateTime.Now).ToVEShortDate() + " " + (model.dtEnd ?? DateTime.Now).ToVEShortTime()), hrefActiveCode, model.NameSurgeryDoctorManager);
                        //        if (MailHelper.IsEmail(model.EmailCreatedBy))
                        //            MailHelper.SendMail(model.EmailCreatedBy, string.Empty, string.Empty, "Quản lý phòng mổ từ chối phê duyệt", contentMail);

                        //        #endregion

                        //    }
                        //    else if(State == (int)ORProgressStateEnum.ApproveSurgeryManager)
                        //    {
                        //        #region send to anesth doctor manager
                        //        string hrefLink = MailHelper.DomainMailUri + "dieu-phoi-nhan-su-gay-me" + "?Id=" + response.Id;


                        //        var dataLink = new ORLinkContract()
                        //        {
                        //            GuidCode = Guid.NewGuid(),
                        //            Code = hrefLink,
                        //            LimitDate = DateTime.Now.AddDays(30),
                        //            IsActive = false,
                        //            IpActive = string.Empty
                        //        };
                        //        _orService.CUDOperationLink(dataLink, CurrentUserId);
                        //        string hrefActiveCode = MailHelper.DomainMailUri + "kich-hoat-link" + "?code=" + dataLink.GuidCode;


                        //        var contentMail = string.Format(@"Dear Anh/Chị điều phối gây mê {11}. <br/> 
                        //                                         Hiện tại thông tin đăng ký ca mổ của bác sĩ {0} đăng ký với thông tin sau : <br/>
                        //                                          - Họ tên bệnh nhân: {1} <br/>
                        //                                          - Ngày tháng năm sinh: {2} <br/> 
                        //                                          - Tên dịch vụ mổ: {3} <br/>
                        //                                          - Thời gian gây mê: {4} <br/>
                        //                                          - Phòng mổ yêu cầu: {5} <br/>
                        //                                          - Ngày thực hiện yêu cầu: {6} <br/>
                        //                                          - Thời gian bắt đầu: {7} <br/>
                        //                                          - Thời gian kết thúc: {8} <br/>
                        //        Thông tin ca mổ này đã được quản lý phòng mổ {10} phê duyệt,<br/>
                        //        Anh chị quản lý điều phối gây mê vào link sau đây <a href={9}> Click xem chi tiết </a>  để tiến hành điều phối nhân sự gây mê . <br/>
                        //        Trân trọng.<br/>", model.NameCreatedBy, model.HoTen, (model.NgaySinh.ToVEShortDate() + " " + model.NgaySinh.ToVEShortTime()), model.HpServiceName, model.TimeAnesth, model.ORRoomName, ((model.dtOperation ?? DateTime.Now).ToVEShortDate() + " " + (model.dtOperation ?? DateTime.Now).ToVEShortTime()), ((model.dtStart ?? DateTime.Now).ToVEShortDate() + " " + (model.dtStart ?? DateTime.Now).ToVEShortTime()), ((model.dtEnd ?? DateTime.Now).ToVEShortDate() + " " + (model.dtEnd ?? DateTime.Now).ToVEShortTime()), hrefActiveCode, model.NameSurgeryDoctorManager,model.NameAnesthManager);
                        //        if (MailHelper.IsEmail(model.EmailAnesthManager))
                        //            MailHelper.SendMail(model.EmailAnesthManager, string.Empty, string.Empty, "Thông báo điều phối nhân sự gây mê ", contentMail);
                        //        #endregion

                        //    }
                        //}
                        #endregion
                        if (actionType == 2 && contract.Id <= 0)
                        {
                            _visitService.SetCurrentVisit(null);
                        }
                    }
                    else
                    {
                        //actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.Error));
                        actionMessage = new ActionMessage(response.Id, response.Message);
                    }
                    //var actionMessageExt = new ActionMessage(response);
                    //return Json(actionMessageExt);
                    return Json(actionMessage);
                }
            }
            return Json(new ActionMessage(-1, MessageResource.Shared_ModelState_InValid));
        }


        #endregion

        #region ORView Anesth

        public ActionResult ViewORAnesth(int Id = 0, int actionType = 1)
        {
            var model = new ORAnesthesiaModel();
            model.listUserEkips = new List<ORMappingEkipContract>();
            model.dtStart = DateTime.Now;
            model.dtOperation = DateTime.Now;
            model.dtEnd = DateTime.Now;
            ORVisitModel v = null;
            if (Id == 0)
            {
                #region Process when direct approve with out book room
                if (actionType == 2)
                {
                    v = _visitService.GetCurrentVisit();
                }
                if (v == null)
                    return RedirectToAction("SearchPatientOR", "OR");
                model = GetCoordinatorAnesthFromVisitCurrent(model, v);
                model.CreatedBy = CurrentUserId;
                #region GetServiceId
                //if (!string.IsNullOrEmpty(v.PatientService))
                //{
                //    var oCode = v.PatientService.Split(new[] { "//" }, StringSplitOptions.None);
                //    if (oCode.Count() > 1)
                //    {
                //        var svEntity = _orService.GetServiceByCode(oCode[1]);
                //        if (svEntity != null)
                //        {
                //            model.HpServiceId = svEntity.Id;
                //            model.HpServiceName = svEntity.Name;
                //        }
                //        model.OrderID = oCode[0];
                //        if (oCode.Length >= 3)
                //            model.ChargeDetailId = oCode[2];
                //        if (oCode.Length >= 4)
                //            model.DepartmentCode = oCode[3];
                //        if (oCode.Length >= 5)
                //            model.AdmissionWard = oCode[4];
                //    }
                //}

                if (!string.IsNullOrEmpty(v.PatientService))
                {
                    List<PatientService> lstPatientService = new List<PatientService>();
                    var lstPatienSr = v.PatientService.Split(new[] { ";" }, System.StringSplitOptions.RemoveEmptyEntries);
                    if (lstPatienSr.Count() > 0)
                    {
                        foreach (var item in lstPatienSr)
                        {
                            var oCode = item.Split(new[] { "//" }, System.StringSplitOptions.RemoveEmptyEntries);
                            if (oCode.Count() > 1)
                            {
                                var svEntity = _orService.GetServiceByCode(oCode[1]);
                                if (svEntity != null)
                                {
                                    model.HpServiceId = svEntity.Id;
                                    model.HpServiceName = svEntity.Name;
                                }
                                model.OrderID = oCode[0];
                                if (oCode.Length >= 3)
                                    model.ChargeDetailId += String.IsNullOrEmpty(model.ChargeDetailId) ? oCode[2] : "," + oCode[2];
                                if (oCode.Length >= 4)
                                    model.DepartmentCode = oCode[3];
                                if (oCode.Length >= 5)
                                    model.AdmissionWard = oCode[4];
                                if (oCode.Length >= 7)
                                    //model.ChargeDate = Convert.ToDateTime(oCode[6]);
                                    model.ChargeDateStr += String.IsNullOrEmpty(model.ChargeDateStr) ? oCode[6] : "," + oCode[6];
                                if (oCode.Length >= 9)
                                    model.ChargeBy += String.IsNullOrEmpty(model.ChargeBy) ? oCode[8] : "," + oCode[8];
                            }
                        }
                    }
                }
                else
                {
                    return RedirectToAction("SearchPatientOR", "OR");
                }
                #endregion
                #endregion
            }
            else
            {
                var orProgress = _orService.GetORAnesthProgress(Id.ToString(), (int)TypeSearchEnum.Id);
                if (orProgress != null)
                {
                    SetDataModel(ref model, orProgress);
                    #region Check access denied on location
                    if ((orProgress.HospitalCode != memberExtendedInfo.CurrentLocaltion.NameEN)
                        || (orProgress.CreatedBy != CurrentUserId && !memberExtendedInfo.IsSuperAdmin && !memberExtendedInfo.IsManageAdminSurgery && !memberExtendedInfo.IsManagAnes))
                    {
                        //Redirect to site access denied
                        return new RedirectResult(new UrlHelper(HttpContext.Request.RequestContext).RouteUrl("ErrorHandler", new { id = 1001 }));
                    }
                    #endregion .Check access denied on location
                    #region Get charges/service from core OH
                    if (string.IsNullOrEmpty(model.ChargeDetailId))
                    {
                        model.Charges = _orService.GetChargesFromOH(model.HospitalCode, model.PId, CurrentUserId);
                        if (model.Charges?.Count > 0)
                            model.Charges = model.Charges.Where(x => x.ItemCode == model.HpServiceCode)?.ToList();
                    }
                    //vutv7 check trường hợp chỉ định quá hạn muốn thay thế chỉ định khác
                    else if (!string.IsNullOrEmpty(model.ChargeDetailId) && model.ChargeDate != null)
                    {
                        int ExprireMonth = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ExprireMonth"].ToString());
                        int months = (DateTime.Now.Year - model.ChargeDate.Value.Year) * 12 + (DateTime.Now.Month - model.ChargeDate.Value.Month);
                        if (months > ExprireMonth)
                        {
                            var lstChargeOH = _orService.GetChargesFromOH(model.HospitalCode, model.PId, CurrentUserId);
                            if (lstChargeOH.Count > 0)
                            {
                                foreach (var item in lstChargeOH.Where(x => x.ItemName == model.HpServiceName))
                                {
                                    months = (DateTime.Now.Year - item.ChargeDate.Year) * 12 + (DateTime.Now.Month - item.ChargeDate.Month);
                                    if (months <= ExprireMonth)
                                    {
                                        model.ChargesReplace.Add(item);
                                    }
                                }
                            }
                        }

                    }
                    #endregion
                }
                else
                {
                    return RedirectToAction("SearchPatientOR", "OR");
                }
            }

            #region master data
            var listEkipTypes = new List<int>()
            {
                (int)ORPositionEnum.MainAnesthDoctor,
                (int)ORPositionEnum.SubAnesthDoctor,
                (int)ORPositionEnum.AnesthNurse1,
                (int)ORPositionEnum.AnesthNurse2,
                (int)ORPositionEnum.AnesthNurseRecovery,
                (int)ORPositionEnum.SubAnesthDoctor2,
                (int)ORPositionEnum.Anesthesiologist,

            };
            var dataEkips = _orService.GetListORUsers(model.HospitalCode, listEkipTypes, 0, model.dtStart, model.dtEnd);
            dataEkips.Insert(0, new ORUserInfoContract()
            {
                Id = 0,
                Name = string.Empty,
                Email = string.Empty,
                PositionId = 0,
                Phone = string.Empty,
                PositionName = string.Empty,
                //linhht
                Username = string.Empty
            });

            if (dataEkips != null && dataEkips.Any())
                model.listEkips = dataEkips;
            #endregion

            //#region master data
            ////doctor
            //var listAnesthDoctors = _orService.GetListORUsers(orProgress.HospitalCode, (int)ORPositionEnum.AnesthDoctor, 0);
            //if (listAnesthDoctors != null && listAnesthDoctors.Any())
            //    model.listAnesthDoctors = listAnesthDoctors;
            ////anesth nurse
            //var listAnesthNurses = _orService.GetListORUsers(orProgress.HospitalCode, (int)ORPositionEnum.AnesthNurse, 0);
            //if (listAnesthNurses != null && listAnesthNurses.Any())
            //    model.listAnesthNurses = listAnesthNurses;
            //#endregion
            model.ShowdtEnd = model.dtEnd.ToVEShortTime();
            model.ShowdtStart = model.dtStart.ToVEShortTime();
            ViewBag.ActionType = actionType;
            return View(model);
        }

        private ORAnesthesiaModel GetCoordinatorAnesthFromVisitCurrent(ORAnesthesiaModel model, ORVisitModel v)
        {
            if (v == null) return model;
            model.PId = v.MA_BN;
            model.Address = v.DIA_CHI;
            model.Sex = v.GIOI_TINH;
            model.NgaySinh = v.NGAY_SINH;
            model.HoTen = v.HO_TEN;
            model.VisitCode = v.VISIT_CODE;
            model.HospitalCode = v.HospitalCode;
            model.HospitalName = v.HospitalName;
            model.HospitalPhone = v.HospitalPhone;
            model.PatientPhone = v.PatientPhone;
            model.Email = v.Email;
            model.Ages = v.Age;
            model.PatientPhone = v.PatientPhone;
            model.SurgeryType = v.SurgeryType;
            return model;
        }
        private void SetDataModel(ref ORAnesthesiaModel model, ORAnesthProgressContract r)
        {
            model.Id = r.Id;
            model.PId = r.PId;
            model.VisitCode = r.VisitCode;
            model.HoTen = r.HoTen;
            model.NgaySinh = r.NgaySinh;
            model.Sex = r.Sex;
            model.Address = r.Address;
            model.Email = r.Email;
            model.Ages = r.Ages;
            model.NameProject = r.NameProject;
            model.NameProject = r.NameProject;
            model.HpServiceId = r.HpServiceId;
            model.ORRoomId = r.ORRoomId;
            model.SurgeryType = r.SurgeryType == 0 ? 1 : r.SurgeryType;
            model.dtStart = r.dtStart ?? DateTime.Now;
            model.dtEnd = r.dtEnd ?? DateTime.Now;
            model.dtOperation = r.dtOperation ?? DateTime.Now;

            model.OrderID = r.OrderID;
            model.ChargeDetailId = r.ChargeDetailId;
            model.DepartmentCode = r.DepartmentCode;
            model.AdmissionWard = r.AdmissionWard;

            model.NameMainAnesthDoctor = r.NameMainAnesthDoctor;
            model.EmailMainAnesthDoctor = r.EmailMainAnesthDoctor;
            model.PhoneMainAnesthDoctor = r.PhoneMainAnesthDoctor;
            model.PositionMainAnesthDoctor = r.PositionMainAnesthDoctor;
            model.UIdMainAnesthDoctor = r.UIdMainAnesthDoctor ?? 0;

            model.NameCreatedBy = r.NameCreatedBy;
            model.EmailCreatedBy = r.EmailCreatedBy;
            model.PhoneCreatedBy = r.PhoneCreatedBy;
            model.PositionCreatedBy = r.PositionCreatedBy;
            model.CreatedBy = r.CreatedBy;


            model.NameAnesthNurse1 = r.NameAnesthNurse1;
            model.EmailAnesthNurse1 = r.EmailAnesthNurse1;
            model.PhoneAnesthNurse1 = r.PhoneAnesthNurse1;
            model.PositionAnesthNurse1 = r.PositionAnesthNurse1;
            model.UIdAnesthNurse1 = r.UIdAnesthNurse1 ?? 0;

            model.NameAnesthNurse2 = r.NameAnesthNurse2;
            model.EmailAnesthNurse2 = r.EmailAnesthNurse2;
            model.PhoneAnesthNurse2 = r.PhoneAnesthNurse2;
            model.PositionAnesthNurse2 = r.PositionAnesthNurse2;
            model.UIdAnesthNurse2 = r.UIdAnesthNurse2 ?? 0;

            model.NameAnesthNurseRecovery = r.NameAnesthNurseRecovery;
            model.EmailAnesthNurseRecovery = r.EmailAnesthNurseRecovery;
            model.PhoneAnesthNurseRecovery = r.PhoneAnesthNurseRecovery;
            model.PositionAnesthNurseRecovery = r.PositionAnesthNurseRecovery;
            model.UIdAnesthNurseRecovery = r.UIdAnesthNurseRecovery ?? 0;

            model.NameSubAnesthDoctor = r.NameSubAnesthDoctor;
            model.EmailSubAnesthDoctor = r.EmailSubAnesthDoctor;
            model.PhoneSubAnesthDoctor = r.PhoneSubAnesthDoctor;
            model.PositionSubAnesthDoctor = r.PositionSubAnesthDoctor;
            model.UIdSubAnesthDoctor = r.UIdSubAnesthDoctor ?? 0;
            model.HospitalName = r.HospitalName;
            model.HospitalPhone = r.HospitalPhone;
            model.PatientPhone = r.PatientPhone;
            model.HospitalCode = r.HospitalCode;

            model.State = r.State;
            model.TimeAnesth = r.TimeAnesth ?? 0;
            model.AnesthDescription = r.AnesthDescription;
            model.ORRoomName = r.ORRoomName;
            model.HpServiceCode = r.HpServiceCode;
            model.HpServiceName = r.HpServiceName;
            model.HospitalName = r.HospitalName;
            model.HospitalPhone = r.HospitalPhone;
            model.PatientPhone = r.PatientPhone;
            model.HospitalCode = r.HospitalCode;
            model.listUserEkips = r.listUsers.Where(c => c.TypePageId == 2).ToList();
            model.ChargeDate = r.ChargeDate;
            model.ChargeBy = r.ChargeBy;
            //vutv7
            model.NameSubAnesthDoctor2 = r.NameSubAnesthDoctor2;
            model.EmailSubAnesthDoctor2 = r.EmailSubAnesthDoctor2;
            model.PhoneSubAnesthDoctor2 = r.PhoneSubAnesthDoctor2;
            model.PositionSubAnesthDoctor2 = r.PositionSubAnesthDoctor2;
            model.UIdSubAnesthDoctor2 = r.UIdSubAnesthDoctor2 ?? 0;

            model.NameAnesthesiologist = r.NameAnesthesiologist;
            model.EmailAnesthesiologist = r.EmailAnesthesiologist;
            model.PhoneAnesthesiologist = r.PhoneAnesthesiologist;
            model.PositionAnesthesiologist = r.PositionAnesthesiologist;
            model.UIdAnesthesiologist = r.UIdAnesthesiologist ?? 0;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveORAnesthByJson(int UIdMainAnesthDoctor, int UIdSubAnesthDoctor, int UIdAnesthNurse1, int UIdAnesthNurse2, int UIdAnesthNurseRecovery, string EmailMainAnesthDoctor, string EmailSubAnesthDoctor, string EmailAnesthNurse1, string EmailAnesthNurse2, string EmailAnesthNurseRecovery, string PhoneMainAnesthDoctor, string PhoneSubAnesthDoctor, string PhoneAnesthNurse1, string PhoneAnesthNurse2, string PhoneAnesthNurseRecovery
            , int UIdSubAnesthDoctor2, string EmailSubAnesthDoctor2, string PhoneSubAnesthDoctor2, int UIdAnesthesiologist, string EmailAnesthesiologist, string PhoneAnesthesiologist
            , int State, int Id, string PId, string AnesthDescription
            , int surgeryType, string OrderID, string ChargeDetailId, string DepartmentCode, string AdmissionWard, int actionType = 1, string ChargeDate = "", string ChargeBy = ""
            )
        {
            ActionMessage actionMessage;

            if (ModelState.IsValid && (State == (int)ORProgressStateEnum.CancelAnesthManager || Constant.ListStateAllowCoordinator.Contains(State)))
            {
                if (string.IsNullOrEmpty(ChargeDetailId) && State != (int)ORProgressStateEnum.AssignEkip && State != (int)ORProgressStateEnum.CancelAnesthManager)
                {
                    return Json(new ActionMessage(-1, "Vui lòng chọn chỉ định trước khi ghi nhận ekip!"));
                }
                var emailRegex = @"^[a-zA-Z]+[a-zA-Z0-9._-]+@[a-zA-Z0-9]{2,}(\.[a-zA-Z0-9]{2,4}){1,2}$";
                if (IsEmail(true, emailRegex, EmailMainAnesthDoctor, "Email bác sỹ gâp mê") != null)
                {
                    return IsEmail(true, emailRegex, EmailMainAnesthDoctor, "Email bác sỹ gâp mê");
                };

                if (IsEmail(false, emailRegex, EmailSubAnesthDoctor, "Email bác sỹ phụ mê 1") != null)
                {
                    return IsEmail(false, emailRegex, EmailSubAnesthDoctor, "Email bác sỹ phụ mê 1");
                };

                if (IsEmail(true, emailRegex, EmailAnesthNurse1, "Email điều dưỡng phụ mê 1") != null)
                {
                    return IsEmail(true, emailRegex, EmailAnesthNurse1, "Email điều dưỡng phụ mê 1");
                };
                if (IsEmail(false, emailRegex, EmailSubAnesthDoctor2, "Email bác sỹ phụ mê 2") != null)
                {
                    return IsEmail(false, emailRegex, EmailSubAnesthDoctor2, "Email bác sỹ phụ mê 2");
                };
                if (IsEmail(false, emailRegex, EmailAnesthesiologist, "Email BS khám gây mê") != null)
                {
                    return IsEmail(false, emailRegex, EmailAnesthesiologist, "Email BS khám gây mê");
                };
                if (IsEmail(false, emailRegex, EmailAnesthNurse2, "Email điều dưỡng phụ mê 2") != null)
                {
                    return IsEmail(false, emailRegex, EmailAnesthNurse2, "Email điều dưỡng phụ mê 2");
                };
                if (IsEmail(false, emailRegex, EmailAnesthNurseRecovery, "Email điều dưỡng hồi tỉnh") != null)
                {
                    return IsEmail(false, emailRegex, EmailAnesthNurseRecovery, "Email điều dưỡng hồi tỉnh");
                };
                var mainAnesthDoctor = _userMngtCaching.UpdateEmailOrPhone(UIdMainAnesthDoctor, Sanitizer.GetSafeHtmlFragment(EmailMainAnesthDoctor).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneMainAnesthDoctor));
                if (mainAnesthDoctor != null)
                {
                    if (mainAnesthDoctor.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                        return Json(new ActionMessage(-1, "Không tìm thấy bác sỹ gây mê"));
                    else
                        return Json(mainAnesthDoctor);
                }

                if (UIdSubAnesthDoctor > 0)
                {
                    var subAnesthDoctor = _userMngtCaching.UpdateEmailOrPhone(UIdSubAnesthDoctor, Sanitizer.GetSafeHtmlFragment(EmailSubAnesthDoctor).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneSubAnesthDoctor));
                    if (subAnesthDoctor != null)
                    {
                        if (subAnesthDoctor.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                            return Json(new ActionMessage(-1, "Không tìm thấy bác sỹ phụ mê 1"));
                        else
                            return Json(subAnesthDoctor);
                    }
                }

                var anesthNurse1 = _userMngtCaching.UpdateEmailOrPhone(UIdAnesthNurse1, Sanitizer.GetSafeHtmlFragment(EmailAnesthNurse1).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneAnesthNurse1));
                if (anesthNurse1 != null)
                {
                    if (anesthNurse1.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                        return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng phụ mê 1"));
                    else
                        return Json(anesthNurse1);
                }

                if (UIdAnesthNurse2 > 0)
                {
                    var anesthNurse2 = _userMngtCaching.UpdateEmailOrPhone(UIdAnesthNurse2, Sanitizer.GetSafeHtmlFragment(EmailAnesthNurse2).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneAnesthNurse2));
                    if (anesthNurse2 != null)
                    {
                        if (anesthNurse2.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                            return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng phụ mê 2"));
                        else
                            return Json(anesthNurse2);
                    }
                }

                if (UIdAnesthNurseRecovery > 0)
                {
                    var anesthNurseRecory = _userMngtCaching.UpdateEmailOrPhone(UIdAnesthNurseRecovery, Sanitizer.GetSafeHtmlFragment(EmailAnesthNurseRecovery).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneAnesthNurseRecovery));
                    if (anesthNurseRecory != null)
                    {
                        if (anesthNurseRecory.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                            return Json(new ActionMessage(-1, "Không tìm thấy điều dưỡng hồi tỉnh"));
                        else
                            return Json(anesthNurseRecory);
                    }
                }
                if (UIdSubAnesthDoctor2 > 0)
                {
                    var subAnesthDoctor2 = _userMngtCaching.UpdateEmailOrPhone(UIdSubAnesthDoctor2, Sanitizer.GetSafeHtmlFragment(EmailSubAnesthDoctor2).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneSubAnesthDoctor2));
                    if (subAnesthDoctor2 != null)
                    {
                        if (subAnesthDoctor2.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                            return Json(new ActionMessage(-1, "Không tìm thấy bác sỹ phụ mê 2"));
                        else
                            return Json(subAnesthDoctor2);
                    }
                }
                if (UIdAnesthesiologist > 0)
                {
                    var anesthesiologist = _userMngtCaching.UpdateEmailOrPhone(UIdAnesthesiologist, Sanitizer.GetSafeHtmlFragment(EmailAnesthesiologist).ToLower(), Sanitizer.GetSafeHtmlFragment(PhoneAnesthesiologist));
                    if (anesthesiologist != null)
                    {
                        if (anesthesiologist.Id == (int)ResponseCode.UserMngt_UsernameNotExisted)
                            return Json(new ActionMessage(-1, "Không tìm thấy BS khám gây mê"));
                        else
                            return Json(anesthesiologist);
                    }
                }
                //vutv7 xử lý trường hợp chỉ định quá hạn
                var splitChargeDate = ChargeDate.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
                if (splitChargeDate.Count() == 1)
                {
                    int ExprireMonth = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["ExprireMonth"].ToString());
                    int months = (DateTime.Now.Year - Convert.ToDateTime(ChargeDate).Year) * 12 + (DateTime.Now.Month - Convert.ToDateTime(ChargeDate).Month);
                    if (months > ExprireMonth)
                    {
                        return Json(new ActionMessage(-1, "Chỉ định cho ca mổ này đã vượt quá 2 tháng cho phép để ghi nhận doanh thu theo quy đinh."));
                    }
                }

                var contract = new ORAnesthProgressContract();
                contract.Id = Id;
                contract.HospitalCode = CurrentLoc.NameEN;
                contract.PId = PId;
                if (actionType == 2 && contract.Id <= 0)
                {
                    //get from model
                    ORVisitModel v = _visitService.GetCurrentVisit();
                    if (v == null)
                    {
                        return Json(new ActionMessage(-1, "tham số không hợp lệ"));
                    }
                    SetCoordinatorSurgicalFromVisitCurrent(ref contract, v);
                    #region GetServiceId
                    //if (!string.IsNullOrEmpty(v.PatientService))
                    //{
                    //    var oCode = v.PatientService.Split(new[] { "//" }, StringSplitOptions.None);
                    //    if (oCode.Count() > 1)
                    //    {
                    //        var svEntity = _orService.GetServiceByCode(oCode[1]);
                    //        if (svEntity != null)
                    //        {
                    //            contract.HpServiceId = svEntity.Id;
                    //            contract.HpServiceName = svEntity.Name;
                    //            contract.dtStart = DateTime.Now.AddMinutes(-svEntity.AnesthesiaTime);
                    //            contract.dtEnd = DateTime.Now.AddMinutes(svEntity.CleaningTime);
                    //            contract.dtOperation = DateTime.Now;
                    //            contract.TimeAnesth = svEntity.AnesthesiaTime;
                    //            contract.VisitCode = Guid.NewGuid().ToString();
                    //        }
                    //        contract.OrderID = oCode[0];
                    //        if (oCode.Length >= 3)
                    //            contract.ChargeDetailId = oCode[2];
                    //        if (oCode.Length >= 4)
                    //            contract.DepartmentCode = oCode[3];
                    //        if (oCode.Length >= 5)
                    //            contract.AdmissionWard = oCode[4];
                    //        //vutv7
                    //        if (oCode.Length >= 7)
                    //            contract.ChargeDate = Convert.ToDateTime(oCode[6]);
                    //        if (oCode.Length >= 9)
                    //            contract.ChargeBy = oCode[8];
                    //    }
                    //}

                    //vutv7 - ghi nhận ekip theo lô
                    if (!string.IsNullOrEmpty(v.PatientService))
                    {
                        var lstPatienSr = v.PatientService.Split(new[] { ";" }, System.StringSplitOptions.RemoveEmptyEntries);
                        if (lstPatienSr.Count() > 0)
                        {
                            foreach (var item in lstPatienSr)
                            {
                                var oCode = item.Split(new[] { "//" }, System.StringSplitOptions.RemoveEmptyEntries);
                                if (oCode.Count() > 1)
                                {
                                    var svEntity = _orService.GetServiceByCode(oCode[1]);
                                    if (svEntity != null)
                                    {
                                        contract.HpServiceId = svEntity.Id;
                                        contract.HpServiceName = svEntity.Name;
                                        contract.dtStart = DateTime.Now.AddMinutes(-svEntity.AnesthesiaTime);
                                        contract.dtEnd = DateTime.Now.AddMinutes(svEntity.CleaningTime);
                                        contract.dtOperation = DateTime.Now;
                                        contract.TimeAnesth = svEntity.AnesthesiaTime;
                                        contract.VisitCode = Guid.NewGuid().ToString();
                                    }
                                    contract.OrderID = oCode[0];
                                    if (oCode.Length >= 3)
                                        contract.ChargeDetailId += string.IsNullOrEmpty(contract.ChargeDetailId) ? oCode[2] : "," + oCode[2];
                                    if (oCode.Length >= 4)
                                        contract.DepartmentCode = oCode[3];
                                    if (oCode.Length >= 5)
                                        contract.AdmissionWard = oCode[4];
                                    if (oCode.Length >= 7)
                                    {
                                        contract.ChargeDateStr += string.IsNullOrEmpty(contract.ChargeDateStr) ? oCode[6] : "," + oCode[6];
                                        contract.ChargeDate = Convert.ToDateTime(oCode[6]);
                                    }
                                    if (oCode.Length >= 9)
                                        contract.ChargeBy += string.IsNullOrEmpty(contract.ChargeBy) ? oCode[8] : "," + oCode[8];
                                }
                            }
                        }
                    }
                    #endregion
                    contract.SurgeryType = surgeryType;
                    contract.CreatedBy = CurrentUserId;
                }
                else if (Constant.ListStateAllowCoordinator.Contains(State))
                {
                    contract.OrderID = OrderID;
                    contract.ChargeDetailId = ChargeDetailId;
                    contract.DepartmentCode = DepartmentCode;
                    contract.AdmissionWard = AdmissionWard;
                    //vutv7
                    if (!string.IsNullOrEmpty(ChargeDate))
                    {
                        contract.ChargeDate = Convert.ToDateTime(ChargeDate);
                        //vutv7 - thêm trường hợp điều phối gây mê
                        contract.ChargeDateStr = ChargeDate;
                    }
                    contract.ChargeBy = ChargeBy;
                }
                //ext
                contract.UIdMainAnesthDoctor = UIdMainAnesthDoctor;
                contract.UIdSubAnesthDoctor = UIdSubAnesthDoctor;
                contract.UIdAnesthNurse1 = UIdAnesthNurse1;
                contract.UIdAnesthNurse2 = UIdAnesthNurse2;
                contract.UIdAnesthNurseRecovery = UIdAnesthNurseRecovery;
                contract.AnesthDescription = Sanitizer.GetSafeHtmlFragment(AnesthDescription);
                contract.State = State;

                contract.NameMainAnesthDoctor = _userMngtCaching.GetFullNameById(UIdMainAnesthDoctor)?.Name;
                contract.NameSubAnesthDoctor = _userMngtCaching.GetFullNameById(UIdSubAnesthDoctor)?.Name;
                contract.NameAnesthNurse1 = _userMngtCaching.GetFullNameById(UIdAnesthNurse1)?.Name;
                contract.NameAnesthNurse2 = _userMngtCaching.GetFullNameById(UIdAnesthNurse2)?.Name;
                contract.NameAnesthNurseRecovery = _userMngtCaching.GetFullNameById(UIdAnesthNurseRecovery)?.Name;

                contract.UIdSubAnesthDoctor2 = UIdSubAnesthDoctor2;
                contract.UIdAnesthesiologist = UIdAnesthesiologist;
                contract.NameSubAnesthDoctor2 = _userMngtCaching.GetFullNameById(UIdSubAnesthDoctor2)?.Name;
                contract.NameAnesthesiologist = _userMngtCaching.GetFullNameById(UIdAnesthesiologist)?.Name;

                //Check Role permission for location data
                #region Check Role permission for location data
                if (contract.HospitalCode != CurrentLoc.NameEN)
                {
                    //Return Invalid current location with Data Location
                    return Json(new ActionMessage(new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied)));
                }
                #endregion .Check Role permission for location data
                //vutv7
                var lstChargeDetailId = !string.IsNullOrEmpty(contract.ChargeDetailId) ? contract.ChargeDetailId.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries) : new string[0];
                var lstChargeDate = !string.IsNullOrEmpty(contract.ChargeDateStr) ? contract.ChargeDateStr.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries) : new string[0];
                var lstChargeBy = !string.IsNullOrEmpty(contract.ChargeBy) ? contract.ChargeBy.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries) : new string[0];
                if (lstChargeDetailId.Count() > 1 && (lstChargeDetailId.Count() == lstChargeDate.Count()))
                {
                    for (int i = 0; i < lstChargeDetailId.Count(); i++)
                    {
                        contract.Id = Id;
                        contract.ChargeDetailId = lstChargeDetailId[i];
                        contract.ChargeDate = Convert.ToDateTime(lstChargeDate[i]);
                        contract.ChargeBy = lstChargeBy[i];
                        CUDReturnMessage response = _orService.SaveORManagementByJson(contract, CurrentUserId, actionType);

                        if (response.Id > 0)
                        {
                            if (response.Id != (int)ResponseCode.AdminRole_Accessdenied)
                            {
                                //Cập nhật sang DIMS để thực hiện tính toán
                                #region Cập nhật sang DIMS để tính toán lại
                                _syncData.UpdateDimsToReCalculate(contract.ChargeDetailId);
                                #endregion
                                actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.OR_SuccessUpdate));
                            }
                            else
                                actionMessage = new ActionMessage(response.Id, response.Message);
                            if (actionType == 2 && contract.Id <= 0)
                            {
                                _visitService.SetCurrentVisit(null);
                            }
                        }
                        else
                        {
                            //actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.Error));
                            actionMessage = new ActionMessage(response.Id, response.Message);
                        }
                        //var actionMessageExt = new ActionMessage(response);
                        //return Json(actionMessageExt);
                    }
                    return Json(new ActionMessage(1, "Success!"));
                }
                else
                {
                    CUDReturnMessage response = _orService.SaveORManagementByJson(contract, CurrentUserId, actionType);
                    if (response.Id > 0)
                    {
                        if (response.Id != (int)ResponseCode.AdminRole_Accessdenied)
                        {
                            //Cập nhật sang DIMS để thực hiện tính toán
                            #region Cập nhật sang DIMS để tính toán lại
                            _syncData.UpdateDimsToReCalculate(contract.ChargeDetailId);
                            #endregion
                            actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.OR_SuccessUpdate));
                        }
                        else
                            actionMessage = new ActionMessage(response.Id, response.Message);
                        #region sendmail 
                        //var model = _orService.GetORAnesthProgress(response.Id.ToString(), (int)TypeSearchEnum.Id);
                        //if (model != null)
                        //{
                        //    if (State == (int)ORProgressStateEnum.ApproveAnesthManager)
                        //    {
                        //        #region send mail black to registor doctor
                        //        var contentMail = string.Format(@"Dear Anh/Chị . <br/> 
                        //                                         Hiện tại thông tin đăng ký ca mổ của bác sĩ {0} với thông tin sau : <br/>
                        //                                          - Họ tên bệnh nhân: {1} <br/>
                        //                                          - Ngày tháng năm sinh: {2} <br/> 
                        //                                          - Tên dịch vụ mổ: {3} <br/>
                        //                                          - Thời gian gây mê: {4} <br/>
                        //                                          - Phòng mổ yêu cầu: {5} <br/>
                        //                                          - Ngày thực hiện yêu cầu: {6} <br/>
                        //                                          - Thời gian bắt đầu: {7} <br/>
                        //                                          - Thời gian kết thúc: {8} <br/>
                        //        Thông tin ca mổ này đã được điều phối phòng mổ {9} phê duyệt và điều phối gây mê  {10} yêu cầu bác sĩ  {11} thực hiện gây mê,<br/>
                        //        Vậy các anh/chị bác sĩ liên quan đến thông tin ca mổ nắm thông tin <br/>
                        //        Trân trọng.<br/>", model.NameCreatedBy, model.HoTen, (model.NgaySinh.ToVEShortDate() + " " + model.NgaySinh.ToVEShortTime()), model.HpServiceName, model.TimeAnesth, model.ORRoomName, ((model.dtOperation ?? DateTime.Now).ToVEShortDate() + " " + (model.dtOperation ?? DateTime.Now).ToVEShortTime()), ((model.dtStart ?? DateTime.Now).ToVEShortDate() + " " + (model.dtStart ?? DateTime.Now).ToVEShortTime()), ((model.dtEnd ?? DateTime.Now).ToVEShortDate() + " " + (model.dtEnd ?? DateTime.Now).ToVEShortTime()), model.NameSurgeryDoctorManager, model.NameAnesthManager,model.NameAnesthDoctor);
                        //        string EmailAnesthDoctor = (model!=null && MailHelper.IsEmail(model.EmailAnesthDoctor)) ? model.EmailAnesthDoctor : string.Empty;
                        //        List<string> lstSendEmail = new List<string>();
                        //        if (MailHelper.IsEmail(model.EmailCreatedBy))
                        //            lstSendEmail.Add(model.EmailCreatedBy);
                        //        if (MailHelper.IsEmail(model.EmailSurgeryDoctorManager))
                        //            lstSendEmail.Add(model.EmailSurgeryDoctorManager);
                        //        if (MailHelper.IsEmail(model.EmailSurgeryDoctor))
                        //            lstSendEmail.Add(model.EmailSurgeryDoctor);
                        //        if (MailHelper.IsEmail(model.EmailAnesthManager))
                        //            lstSendEmail.Add(model.EmailAnesthManager);
                        //        if (MailHelper.IsEmail(model.EmailToolTech))
                        //            lstSendEmail.Add(model.EmailToolTech);
                        //        if (MailHelper.IsEmail(model.EmailAnesthNurse))
                        //            lstSendEmail.Add(model.EmailAnesthNurse);                            
                        //        if (lstSendEmail.Any())
                        //            MailHelper.SendMail(string.Join(",",lstSendEmail), string.Empty, string.Empty, "Hoàn tất phiếu đăng ký mổ", contentMail);
                        //        #endregion
                        //    }                       
                        //}
                        #endregion
                        if (actionType == 2 && contract.Id <= 0)
                        {
                            _visitService.SetCurrentVisit(null);
                        }
                    }
                    else
                    {
                        //actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.Error));
                        actionMessage = new ActionMessage(response.Id, response.Message);
                    }
                    //var actionMessageExt = new ActionMessage(response);
                    //return Json(actionMessageExt);
                    return Json(actionMessage);
                }
            }
            return Json(new ActionMessage(-1, MessageResource.Shared_ModelState_InValid));
        }
        #endregion



        public ActionResult SearchAnesthInfo(DateTime? FromDate, DateTime? ToDate, int State = 0, string kw = "", int p = 1, int ps = 0, int HpServiceId = 0, int ORRoomId = 0, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh, int IsAll = 0/*linhht*/, string username = "", bool export = false)
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;
            if (siteId != memberExtendedInfo.CurrentLocaltion.NameEN)
            {
                //Redirect to site access denied
                return new RedirectResult(new UrlHelper(HttpContext.Request.RequestContext).RouteUrl("ErrorHandler", new { id = 1001 }));
            }

            if (!FromDate.HasValue) FromDate = DateTime.Now.AddDays(-7);
            if (!ToDate.HasValue) ToDate = DateTime.Now;
            FromDate = FromDate.Value.AddTimeToTheStartOfDay();
            ToDate = ToDate.Value.AddTimeToTheEndOfDay();
            kw = kw.Trim();


            var listStates = new List<SelectListItem>();
            var listHpServices = new List<SelectListItem>();
            var listORRooms = new List<SelectListItem>();
            var listUserFileter = new List<SelectListItem>() { new SelectListItem() { Text = "All", Value = "0" }, new SelectListItem() { Text = "Only me", Value = "1" } };
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;


            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion


            listStates =
                EnumExtension.ToListOfValueAndDesc<ORProgressStateEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);


            var dataService = _orService.GetListHpServices(siteId, (int)SourceClientEnum.Oh);
            if (dataService != null && dataService.Any())
            {
                dataService = dataService.Where(x => x.Type == 2).ToList();
                dataService.Insert(0, new HpServiceContract() { Id = 0, Name = "Tất cả" });
                listHpServices = dataService.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToList();
            }

            var dataORRooms = _orService.GetListRoom(siteId, string.Empty, (int)SourceClientEnum.Oh, -1, "1", true);
            if (dataORRooms != null && dataORRooms.Any())
            {
                dataORRooms.Insert(0, new ORRoomContract() { Id = 0, Name = "Tất cả" });
                listORRooms = dataORRooms.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToList();
            }
            int userFilter = IsAll == 0 ? 0 : CurrentUserId;
            //linhht
            SearchORProgress data = _orService.FindAnesthInfo(FromDate ?? DateTime.Now, ToDate ?? DateTime.Now, State, kw, p, ps, HpServiceId, ORRoomId, siteId, sourceClientId, userFilter, 0, username);

            #region action export data file
            if (export)
            {
                var data1 = _orService.FindAnesthInfo(FromDate ?? DateTime.Now, ToDate ?? DateTime.Now, State, kw, 1, 1000, HpServiceId, ORRoomId, siteId, sourceClientId, userFilter, 0, username);

                if (data1 != null && data1.Data != null && data1.Data?.Count > 0)
                {
                    return ExportORReceiptSurgeryList(data1.Data,
                   "OR-receipt-surgery-list-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"), (DateTime)FromDate, (DateTime)ToDate);
                }
            }
            #endregion .action export data file

            //----
            var model = new SearchORAnesthInfoModel()
            {
                listData = data.Data,
                TotalCount = data.TotalRows,
                listHpServices = listHpServices,
                listORRooms = listORRooms,
                listSites = listSites,
                PageCount = ps,
                PageNumber = p,
                listStates = listStates,
                State = State,
                kw = kw,
                FromDate = FromDate ?? DateTime.Now,
                ToDate = ToDate ?? DateTime.Now,
                siteId = siteId,
                sourceClientId = sourceClientId,
                HpServiceId = HpServiceId,
                ORRoomId = ORRoomId,
                currentUserId = CurrentUserId,
                listUserFilter = listUserFileter,
                IsAll = IsAll
            };
            model.currentUserId = CurrentUserId;
            return View(model);
        }

        #region Ekip
        public ActionResult CUDAnesthEkip(int Id = 0, int ORAnesthProgessId = 0, int TypePageId = 1, string HospitalCode = "", int positionId = 0)
        {
            //set default current location
            HospitalCode = (string.IsNullOrEmpty(HospitalCode)) ? memberExtendedInfo.CurrentLocaltion.NameEN : HospitalCode;
            if (HospitalCode != memberExtendedInfo.CurrentLocaltion.NameEN)
            {
                //Redirect to site access denied
                return new RedirectResult(new UrlHelper(HttpContext.Request.RequestContext).RouteUrl("ErrorHandler", new { id = 1001 }));
            }

            DateTime dt = DateTime.Now;
            var model = new ORMappingEkipModel();
            model.ORAnesthProgessId = ORAnesthProgessId;
            model.Id = Id;
            model.TypePageId = TypePageId;
            model.listEkips = new List<ORUserInfoContract>();
            model.HospitalCode = HospitalCode;

            #region postion
            if (TypePageId == (int)ORAnesthManagerEnum.SurgeryDoctorManager)
            {
                model.listPositions = new List<SelectListItem>()
                {
                    new SelectListItem   {  Text = "PTV chính",Value = "1"},
                    new SelectListItem   {  Text = "PTV phụ 1",Value = "2"},
                    new SelectListItem   {  Text = "PTV phụ 2",Value = "3"},
                    new SelectListItem   {  Text = "PTV phụ 3",Value = "4"},
                    new SelectListItem   {  Text = "Bác sĩ CEC",Value = "5"},
                    new SelectListItem   {  Text = "Điều dưỡng dụng cụ 1",Value = "6"},
                    new SelectListItem   {  Text = "Điều dưỡng dụng cụ 2",Value = "7"},
                    new SelectListItem   {  Text = "Điều dưỡng chạy ngoài 1",Value = "8"},
                    new SelectListItem   {  Text = "Điều dưỡng chạy ngoài 2",Value = "9"}
                };
                if (positionId == 0) positionId = (int)ORPositionEnum.PTVMain;
            }
            else if (TypePageId == (int)ORAnesthManagerEnum.AnesthDoctorManager)
            {
                model.listPositions = new List<SelectListItem>()
                {
                    new SelectListItem   {  Text = "Bác sĩ gây mê",Value = "10"},
                    new SelectListItem   {  Text = "Bác sĩ phụ mê",Value = "11"},
                    new SelectListItem   {  Text = "Điều dưỡng phụ mê 1",Value = "12"},
                    new SelectListItem   {  Text = "Điều dưỡng gây mê",Value = "13"},
                };
                if (positionId == 0) positionId = (int)ORPositionEnum.MainAnesthDoctor;
            }


            //doctor
            var dataEkips = _orService.GetListORUsers(HospitalCode, positionId, 0);
            if (dataEkips != null && dataEkips.Any())
                model.listEkips = dataEkips;
            model.PositionId = positionId;
            #endregion

            if (Id > 0)
            {
                var data = _orService.GetInfoEkip(Id);
                if (data != null)
                {
                    model.Id = data.Id;
                    //model.PositionId = data.PositionId;
                    model.HospitalCode = data.HospitalCode;
                    model.NameEkip = data.UserName;
                    model.EmailEkip = data.Email;
                    model.PhoneEkip = data.Phone;
                    model.PositionId = data.PositionId;
                    model.PositionEkip = data.PositionName;
                    model.TypePageId = data.TypePageId;
                    model.UIdEkip = data.UId;
                    model.ORAnesthProgessId = data.ORAnesthProgessId;
                }
            }
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveCUDEkip(ORMappingEkipModel model)
        {
            if (ModelState.IsValid)
            {
                var contract = new ORMappingEkipContract();
                contract.Id = model.Id;
                contract.HospitalCode = Sanitizer.GetSafeHtmlFragment(model.HospitalCode);
                contract.UId = model.UIdEkip;
                contract.Email = Sanitizer.GetSafeHtmlFragment(model.EmailEkip);
                contract.Phone = Sanitizer.GetSafeHtmlFragment(model.PhoneEkip);
                contract.PositionId = model.PositionId;

                contract.ORAnesthProgessId = model.ORAnesthProgessId;
                contract.TypePageId = model.TypePageId;

                ActionMessage actionMessage;
                #region Check access denied on location
                if (memberExtendedInfo.CurrentLocaltion.NameEN != contract.HospitalCode)
                {
                    actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied));
                    return Json(actionMessage);
                }
                #endregion .Check access denied on location
                CUDReturnMessage response = _orService.SaveCUDEkip(contract, CurrentUserId);

                if (response.Id > 0)
                {
                    actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.OR_SuccessCreate));
                }
                else
                {
                    actionMessage = new ActionMessage(response.Id, response.Message);
                }
                TempData[AdminGlobal.ActionMessage] = actionMessage;
                response.Id = (int)contract.ORAnesthProgessId;
                var actionMessageExt = new ActionMessage(response);
                actionMessageExt.Message = model.TypePageId.ToString();
                return Json(actionMessageExt);
            }
            return Json(new ActionMessage(-1, MessageResource.Shared_ModelState_InValid));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteAnesthEkip(long Id)
        {
            CUDReturnMessage response = _orService.DeleteAnesthEkip(Id, CurrentUserId);
            var actionMessageExt = new ActionMessage(response);
            return Json(actionMessageExt);
        }
        [HttpGet]
        public JsonResult GetListUserForEKip(int positionId, string kw)
        {
            var dataEkips = _orService.GetListORUsers(CurrentLoc.NameEN, new List<int> { positionId }, 0, DateTime.Now, DateTime.Now);
            if (!string.IsNullOrEmpty(kw))
            {
                dataEkips = dataEkips?.Where(x => x.Name.ToLower().Contains(kw.ToLower()))?.ToList();
            }
            return Json(dataEkips, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteSurgery(long Id, int Type)
        {
            var item = _orService.GetORAnesthProgress(Id.ToString(), 1);
            if (item != null && (item.CreatedBy == CurrentUserId || memberExtendedInfo.IsSuperAdmin || memberExtendedInfo.IsManageAdminSurgery))
            {
                CUDReturnMessage response = null;
                switch ((ORStepEnum)Type)
                {
                    case ORStepEnum.Registor:
                        if (item.State == (int)ORProgressStateEnum.Registor || item.State == (int)ORLogStateEnum.CancelCharge)
                        {
                            response = _orService.DeleteSurgery(Id, CurrentUserId);
                            if (item.State == (int)ORLogStateEnum.CancelCharge)
                            {
                                //Cập nhật sang DIMS để tính toán lại
                                #region Cập nhật sang DIMS để tính toán lại
                                var resUpdateDims = _syncData.UpdateDimsToReCalculate(item.ChargeDetailId);
                                if (resUpdateDims == 0)
                                {
                                    //Rollback delete
                                    _orService.RollbackDeleteSurgery(Id, CurrentUserId);
                                    return Json(new CUDReturnMessage() { Id = 0, Message = "Xóa ghi nhận doanh thu không thành công (DIMS). Vui lòng thử lại sau!" });
                                }
                                #endregion
                            }
                            var actionMessageExt = new ActionMessage(response);
                            return Json(actionMessageExt);
                        }
                        else
                        {
                            return Json(new ActionMessage(-1, "Dữ liệu không hợp lệ hoặc bạn không có quyền xóa"));
                        }
                    case ORStepEnum.CoordinatorSurgery:
                        if (item.State == (int)ORProgressStateEnum.ApproveAnesthManager || item.State == (int)ORProgressStateEnum.ApproveSurgeryManager || item.State == (int)ORLogStateEnum.CancelCharge)
                        {
                            response = _orService.DeleteSurgery(Id, CurrentUserId);
                            //Cập nhật sang DIMS để tính toán lại
                            #region Cập nhật sang DIMS để tính toán lại
                            var resUpdateDims = _syncData.UpdateDimsToReCalculate(item.ChargeDetailId);
                            if (resUpdateDims == 0)
                            {
                                //Rollback delete
                                _orService.RollbackDeleteSurgery(Id, CurrentUserId);
                                return Json(new CUDReturnMessage() { Id = 0, Message = "Xóa ghi nhận doanh thu không thành công (DIMS). Vui lòng thử lại sau!" });
                            }
                            #endregion
                            var actionMessageExt = new ActionMessage(response);
                            return Json(actionMessageExt);
                        }
                        else
                        {
                            return Json(new ActionMessage(-1, "Dữ liệu không hợp lệ hoặc bạn không có quyền xóa"));
                        }
                    default:
                        return Json(new ActionMessage(-1, "Dữ liệu không hợp lệ hoặc bạn không có quyền xóa"));
                }
            }
            else
            {
                return Json(new ActionMessage(-1, "Dữ liệu không hợp lệ hoặc bạn không có quyền xóa"));
            }
        }

        private ActionResult SearchORInfo(DateTime? FromDate, DateTime? ToDate, int State = 0, string kw = "", int p = 1, int ps = 0, int HpServiceId = 0, int ORRoomId = 0, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh, int? serviceType = -1, string export = "")
        {
            if (!FromDate.HasValue) FromDate = DateTime.Now.AddDays(-7);
            if (!ToDate.HasValue) ToDate = DateTime.Now;
            FromDate = FromDate.Value.AddTimeToTheStartOfDay();
            ToDate = ToDate.Value.AddTimeToTheEndOfDay();


            var listStates = new List<SelectListItem>();
            var listHpServices = new List<SelectListItem>();
            var listORRooms = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;


            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion


            listStates =
                EnumExtension.ToListOfValueAndDesc<ORProgressStateEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);


            var dataService = _orService.GetListHpServices(siteId, (int)SourceClientEnum.Oh);
            if (dataService != null && dataService.Any())
            {
                dataService = dataService.Where(x => x.Type == 2).ToList();
                dataService.Insert(0, new HpServiceContract() { Id = 0, Name = "Tất cả" });
                listHpServices = dataService.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToList();
            }

            var dataORRooms = _orService.GetListRoom(siteId, string.Empty, (int)SourceClientEnum.Oh, -1, "1", true);
            if (dataORRooms != null && dataORRooms.Any())
            {
                dataORRooms.Insert(0, new ORRoomContract() { Id = 0, Name = "Tất cả" });
                listORRooms = dataORRooms.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }).ToList();
            }
            SearchORProgress data = null;
            #region action export data file
            if (!string.IsNullOrEmpty(export) && export == "True")
            {
                data = _orService.FindAnesthInfo(FromDate ?? DateTime.Now, ToDate ?? DateTime.Now, State, kw, 1, 1000000, HpServiceId, ORRoomId, siteId, sourceClientId, 0, 0);
                if (data != null && data.Data != null && data.Data?.Count > 0)
                {
                    return ExportORReceiptSurgeryList(data.Data,
                    "OR-receipt-surgery-list-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"), (DateTime)FromDate, (DateTime)ToDate);
                }
                goto StepReturnFormSearch;
            }
            #endregion .action export data file
            data = _orService.FindAnesthInfo(FromDate ?? DateTime.Now, ToDate ?? DateTime.Now, State, kw, p, ps, HpServiceId, ORRoomId, siteId, sourceClientId, 0, 0, "", serviceType);
        StepReturnFormSearch:
            var model = new SearchORAnesthInfoModel()
            {
                listData = data.Data,
                TotalCount = data.TotalRows,
                listHpServices = listHpServices,
                listORRooms = listORRooms,
                listSites = listSites,
                PageCount = ps,
                PageNumber = p,
                listStates = listStates,
                State = State,
                kw = kw,
                FromDate = FromDate ?? DateTime.Now,
                ToDate = ToDate ?? DateTime.Now,
                siteId = siteId,
                sourceClientId = sourceClientId,
                HpServiceId = HpServiceId,
                ORRoomId = ORRoomId,
                currentUserId = CurrentUserId
            };
            return View(model);
        }

        public ActionResult SearchAnesthRole(DateTime? FromDate, DateTime? ToDate, int State = (int)ORProgressStateEnum.ApproveSurgeryManager, string kw = "", int p = 1, int ps = 0, int HpServiceId = 0, int ORRoomId = 0, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;
            return this.SearchORInfo(FromDate, ToDate, State, kw, p, ps, HpServiceId, ORRoomId, siteId, sourceClientId);
        }
        public ActionResult SearchSurgeryRole(DateTime? FromDate, DateTime? ToDate, int State = (int)ORProgressStateEnum.Registor, string kw = "", int p = 1, int ps = 0, int HpServiceId = 0, int ORRoomId = 0, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh, string export = "")
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;
            return this.SearchORInfo(FromDate, ToDate, State, kw, p, ps, HpServiceId, ORRoomId, siteId, sourceClientId, 1, export);
        }
        public ActionResult ViewPlan(string HospitalCode = "")
        {
            try
            {
                #region Check access denied on location
                if (memberExtendedInfo.CurrentLocaltion.NameEN != HospitalCode)
                {
                    var actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied));
                    ViewBag.listData = new JavaScriptSerializer().Serialize(actionMessage);
                    return View();
                }
                #endregion .Check access denied on location
                #region master data
                var listSites = new List<SelectListItem>();
                if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
                {
                    foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                    {
                        listSites.Add(new SelectListItem()
                        {
                            Text = item.NameVN,
                            Value = item.NameEN
                        });
                    }
                }
                if (!listSites.Any()) return RedirectToAction("Login", "Authen");

                if (listSites.Any() && !listSites.Any(x => x.Value.Equals(HospitalCode)))
                {
                    HospitalCode = listSites.FirstOrDefault().Value;
                }
                var site = listSites.FirstOrDefault(s => s.Value.Equals(HospitalCode));
                if (site == null) return RedirectToAction("Login", "Authen");
                #endregion

                //ds room theo site
                var listRoom = _orService.GetListRoom(HospitalCode, string.Empty, 0, -1, "1", true).Select(c => new { id = c.Id, title = c.Name }).ToList();
                ViewBag.listRoom = new JavaScriptSerializer().Serialize(listRoom).ToString();
                //ds data 
                DateTime dtstart = new DateTime();
                DateTime stend = new DateTime();
                DateTime now = DateTime.Now;
                dtstart = new DateTime(now.Year, now.Month, 1);
                dtstart = now.AddDays(-15);
                stend = dtstart.AddMonths(1).AddDays(-1);
                var data = _orService.FindAnesthInfo(dtstart, stend, 0, string.Empty, 1, 5000000, 0, 0, HospitalCode, 2, 0, 1);

                // var data = _orService.FindAnesthInfo(now.AddDays(-1), now.AddDays(1), 0, string.Empty, 1, 5000000, 0, 0, HospitalCode, 2, 0, 1);
                List<Object> listData = new List<object>();
                foreach (var c in data.Data.OrderBy(c => c.ORRoomId))
                {
                    string colorFormat = "green";
                    if (c.State == (int)ORProgressStateEnum.Registor)
                        colorFormat = "orange";
                    if (c.State == (int)ORProgressStateEnum.NoApproveSurgeryManager || c.State == (int)ORProgressStateEnum.CancelAnesthManager)
                        colorFormat = "red";
                    string urlClick = "";
                    if ((c.State == (int)ORProgressStateEnum.Registor || c.State == (int)ORProgressStateEnum.NoApproveSurgeryManager)
                        && (c.CreatedBy == CurrentUserId && c.CreatedBy > 0)
                       )
                    {
                        urlClick = Url.Action("ViewORRegistor", new { Id = c.Id });
                    }
                    //QuangBH20190422: Fix the issues that the time will be in other row because they add -1 minutes
                    //listData.Add(new { id = c.Id, title = c.AnesthTitle, resourceId = c.ORRoomId, start = (c.dtAnesthStart.AddMinutes(-1)).ToString("o"), end = (c.dtStart ?? DateTime.Now).ToString("o"), color = "blue", infopatient = "" });
                    if (c.dtEnd.Value.Subtract(c.dtStart.Value) <= TimeSpan.FromMinutes(15))
                    {
                        listData.Add(new { id = c.Id, title = c.AnesthTitle.Trim(), resourceId = c.ORRoomId, start = (c.dtAnesthStart).ToString("o"), end = (c.dtStart ?? DateTime.Now).AddMinutes(5).ToString("o"), color = "blue", infopatient = "" });
                        listData.Add(new { id = c.Id, title = c.PId + " " + c.HpServiceName, resourceId = c.ORRoomId, start = (c.dtStart ?? DateTime.Now).AddMinutes(5).ToString("o"), end = (c.dtEnd ?? DateTime.Now).AddMinutes(-5).ToString("o"), color = colorFormat, infopatient = "", url = urlClick });
                        string addClassName = "";
                        if ((c.CreatedBy == CurrentUserId || memberExtendedInfo.IsSuperAdmin) && c.State == (int)ORProgressStateEnum.Registor)
                        {
                            addClassName = "fc-clear";
                        }
                        listData.Add(new { id = c.Id, title = c.CleanTitle.Trim(), resourceId = c.ORRoomId, start = (c.dtEnd ?? DateTime.Now).AddMinutes(-5).ToString("o"), end = (c.dtCleanEnd).ToString("o"), color = "blue", infopatient = "", className = addClassName });
                    }
                    else
                    {
                        if (c.TimeAnesth == 0)
                        {
                            listData.Add(new { id = c.Id, title = c.AnesthTitle.Trim(), resourceId = c.ORRoomId, start = (c.dtAnesthStart).ToString("o"), end = (c.dtStart ?? DateTime.Now).AddMinutes(15).ToString("o"), color = "blue", infopatient = "" });
                        }
                        else
                        {
                            listData.Add(new { id = c.Id, title = c.AnesthTitle.Trim(), resourceId = c.ORRoomId, start = (c.dtAnesthStart).ToString("o"), end = (c.dtStart ?? DateTime.Now).ToString("o"), color = "blue", infopatient = "" });
                        }

                        if (c.TimeAnesth == 0)
                        {
                            listData.Add(new { id = c.Id, title = c.PId + " " + c.HpServiceName, resourceId = c.ORRoomId, start = (c.dtStart ?? DateTime.Now).AddMinutes(15).ToString("o"), end = (c.dtEnd ?? DateTime.Now).ToString("o"), color = colorFormat, infopatient = "", url = urlClick });
                        }
                        else
                        {
                            listData.Add(new { id = c.Id, title = c.PId + " " + c.HpServiceName, resourceId = c.ORRoomId, start = (c.dtStart ?? DateTime.Now).ToString("o"), end = (c.dtEnd ?? DateTime.Now).ToString("o"), color = colorFormat, infopatient = "", url = urlClick });
                        }

                        string addClassName = "";
                        if ((c.CreatedBy == CurrentUserId || memberExtendedInfo.IsSuperAdmin) && c.State == (int)ORProgressStateEnum.Registor)
                        {
                            addClassName = "fc-clear";
                        }
                        listData.Add(new { id = c.Id, title = c.CleanTitle.Trim(), resourceId = c.ORRoomId, start = (c.dtEnd ?? DateTime.Now).ToString("o"), end = (c.dtCleanEnd).ToString("o"), color = "blue", infopatient = "", className = addClassName });
                    }
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                ViewBag.listData = serializer.Serialize(listData);

                return View();
            }
            catch (Exception ex)
            {
                //log.Debug(string.Format("OR.ViewPlan Ex: {0}", ex));
                return View();
            }

        }
        [HttpGet]
        public JsonResult GetFullPhongBan(string HospitalCode = "")
        {
            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }


            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(HospitalCode)))
            {
                HospitalCode = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(HospitalCode));
            #endregion

            var response = _orService.GetListRoom(HospitalCode, string.Empty, 0, -1, string.Empty, false);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetFullPlanByMonth(string HospitalCode = "")
        {
            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(HospitalCode)))
            {
                HospitalCode = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(HospitalCode));
            #endregion

            DateTime dtstart = new DateTime();
            DateTime stend = new DateTime();
            DateTime now = DateTime.Now;
            dtstart = new DateTime(now.Year, now.Month, 1);
            dtstart = now.AddDays(-15);
            stend = dtstart.AddMonths(2).AddDays(-1);
            var data = _orService.FindAnesthInfo(dtstart, stend, 0, string.Empty, 1, 5000000, 0, 0, HospitalCode, 2, 0, 1);
            var response = data.Data;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetFullPlanByMonthByeource(string HospitalCode = "", int RoomId = 0)
        {
            //set default current location
            HospitalCode = (string.IsNullOrEmpty(HospitalCode)) ? memberExtendedInfo.CurrentLocaltion.NameEN : HospitalCode;

            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(HospitalCode)))
            {
                HospitalCode = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(HospitalCode));
            #endregion

            DateTime dtstart = new DateTime();
            DateTime stend = new DateTime();
            DateTime now = DateTime.Now;
            dtstart = new DateTime(now.Year, now.Month, 1);
            dtstart = now.AddDays(-15);
            stend = dtstart.AddMonths(1).AddDays(-1);
            var response = _orService.FindAnesthInfo(dtstart, stend, 0, string.Empty, 1, 5000000, 0, RoomId, HospitalCode, 2, 0, 1);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region View Public 

        public ActionResult OHViewPatientPublic(DateTime? FromDate, DateTime? ToDate, int State = 0, string kw = "", int p = 1, int ps = 0, int r = 0, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;
            if (siteId != memberExtendedInfo.CurrentLocaltion.NameEN)
            {
                //Redirect to site access denied
                return new RedirectResult(new UrlHelper(HttpContext.Request.RequestContext).RouteUrl("ErrorHandler", new { id = 1001 }));
            }
            //FromDate = DateTime.Now.AddDays(-30);//test
            //ToDate = DateTime.Now.AddDays(30);//test
            //siteId = "HCP";//test


            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion
            var listStates = new List<SelectListItem>();
            var listCheckListTypes = new List<SelectListItem>();
            var listSystems = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (!FromDate.HasValue) FromDate = DateTime.Now;
            FromDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
            if (!ToDate.HasValue) ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
            #region state
            listStates =
                EnumExtension.ToListOfValueAndDesc<OHPatientStateEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
            #endregion state

            var data = _orService.FindAnesthPublicInfo(FromDate ?? DateTime.Now, ToDate ?? DateTime.Now, State, kw, p, ps, 0, r, siteId, sourceClientId, 0, false);

            var model = new OHViewPatientPublicModel()
            {
                listData = data.Data.OrderBy(x => x.ORRoomName).Select(x => x).ToList(),
                TotalCount = data.TotalRows,
                PageCount = ps,
                PageNumber = p,
                listStates = listStates,
                State = State,
                kw = kw,
                FromDate = FromDate ?? DateTime.Now,
                ToDate = ToDate ?? DateTime.Now,
                siteId = siteId,
                sourceClientId = sourceClientId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OHViewPatientsPublicByJson(int pageIndex, int totalPage, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;

            List<ORAnesthProgressExtContract> listPatient = new List<ORAnesthProgressExtContract>();
            pageIndex = pageIndex + 1;
            pageIndex = pageIndex > totalPage ? 1 : pageIndex;

            var model = new OHViewPatientPublicContractModel()
            {
                listData = listPatient,
                PageCount = AdminConfiguration.Paging_PageSize,
                PageNumber = pageIndex,
                kw = string.Empty,
                siteId = siteId,
                sourceClientId = sourceClientId
            };
            try
            {
                //test,nhớ chạy chinh thuc thi bo ra
                var data = _orService.FindAnesthPublicInfo(Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00"), Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59"), 0, string.Empty, pageIndex, model.PageCount, 0, 0, siteId, sourceClientId, 0, false);
                //var data = _orService.FindAnesthPublicInfo(Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00"), Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59"), 0, string.Empty, pageIndex, AdminConfiguration.Paging_PageSize, 0, 0, siteId, sourceClientId, 0);
                if (data.TotalRows == 0) return Json(new List<ORAnesthProgressContract>());
                foreach (var item in data.Data)
                {
                    listPatient.Add(new ORAnesthProgressExtContract()
                    {
                        #region data
                        Id = item.Id,
                        dtStart = (item.dtStart ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat),
                        dtEnd = (item.dtEnd ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat),
                        PId = item.PId,
                        HoTen = item.HoTen,
                        Ages = item.Ages,
                        NamePTVMain = item.NamePTVMain,
                        HpServiceName = item.HpServiceName,
                        State = item.State,
                        CreatedBy = item.CreatedBy,
                        NgaySinh = item.NgaySinh,
                        Email = item.Email,
                        HospitalCode = item.HospitalCode,
                        HpServiceId = item.HpServiceId,
                        Sex = item.Sex,
                        dtOperation = (item.dtOperation ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat),
                        TimeAnesth = item.TimeAnesth ?? 0,
                        IsEmergence = item.IsEmergence,
                        RegDescription = string.IsNullOrEmpty(item.RegDescription) ? string.Empty : item.RegDescription,
                        ORRoomId = item.ORRoomId,
                        ORRoomName = item.ORRoomName,
                        StateName = EnumExtension.GetDescription((OHPatientStateEnum)item.State),
                        Color = HtmlExtensions.OHStateBackgroundColorPatient(item.State),
                        //Color = HtmlExtensions.StateBackgroundColorPatient(item.State),
                        UIdPTVMain = item.UIdPTVMain,
                        UIdPTVSub1 = item.UIdPTVSub1,
                        UIdPTVSub2 = item.UIdPTVSub2,
                        UIdCECDoctor = item.UIdCECDoctor,
                        UIdNurseTool1 = item.UIdNurseTool1,
                        UIdNurseTool2 = item.UIdNurseTool2,
                        UIdNurseOutRun1 = item.UIdNurseOutRun1,
                        UIdNurseOutRun2 = item.UIdNurseOutRun2,
                        EmailPTVMain = item.EmailPTVMain,
                        PhonePTVMain = item.PhonePTVMain,
                        PositionPTVMain = item.PositionPTVMain,
                        NamePTVSub1 = item.NamePTVSub1,
                        EmailPTVSub1 = item.EmailPTVSub1,
                        PhonePTVSub1 = item.PhonePTVSub1,
                        PositionPTVSub1 = item.PositionPTVSub1,
                        NamePTVSub2 = item.NamePTVSub2,
                        EmailPTVSub2 = item.EmailPTVSub2,
                        PhonePTVSub2 = item.PhonePTVSub2,
                        PositionPTVSub2 = item.PositionPTVSub2,
                        NamePTVSub3 = item.NamePTVSub3,
                        EmailPTVSub3 = item.EmailPTVSub3,
                        PhonePTVSub3 = item.PhonePTVSub3,
                        PositionPTVSub3 = item.PositionPTVSub3,
                        NameCECDoctor = item.NameCECDoctor,
                        EmailCECDoctor = item.EmailCECDoctor,
                        PhoneCECDoctor = item.PhoneCECDoctor,
                        PositionCECDoctor = item.PositionCECDoctor,
                        NameNurseTool1 = item.NameNurseTool1,
                        EmailNurseTool1 = item.EmailNurseTool1,
                        PhoneNurseTool1 = item.PhoneNurseTool1,
                        PositionNurseTool1 = item.PositionNurseTool1,
                        NameNurseTool2 = item.NameNurseTool2,
                        EmailNurseTool2 = item.EmailNurseTool2,
                        PhoneNurseTool2 = item.PhoneNurseTool2,
                        PositionNurseTool2 = item.PositionNurseTool2,
                        NameNurseOutRun1 = item.NameNurseOutRun1,
                        EmailNurseOutRun1 = item.EmailNurseOutRun1,
                        PhoneNurseOutRun1 = item.PhoneNurseOutRun1,
                        PositionNurseOutRun1 = item.PositionNurseOutRun1,
                        NameNurseOutRun2 = item.NameNurseOutRun2,
                        EmailNurseOutRun2 = item.EmailNurseOutRun2,
                        PhoneNurseOutRun2 = item.PhoneNurseOutRun2,
                        PositionNurseOutRun2 = item.PositionNurseOutRun2,
                        AnesthDescription = item.AnesthDescription,
                        NameProject = item.NameProject,
                        VisitCode = item.VisitCode,
                        ProjectName = item.ProjectName,
                        NameCreatedBy = item.NameCreatedBy,
                        EmailCreatedBy = item.EmailCreatedBy,
                        PhoneCreatedBy = item.PhoneCreatedBy,
                        PositionCreatedBy = item.PositionCreatedBy,

                        UIdMainAnesthDoctor = item.UIdMainAnesthDoctor,
                        UIdSubAnesthDoctor = item.UIdSubAnesthDoctor,
                        UIdAnesthNurse1 = item.UIdAnesthNurse1,
                        UIdAnesthNurse2 = item.UIdAnesthNurse2,
                        NameMainAnesthDoctor = item.NameMainAnesthDoctor,
                        EmailMainAnesthDoctor = item.EmailMainAnesthDoctor,
                        PhoneMainAnesthDoctor = item.PhoneMainAnesthDoctor,
                        PositionMainAnesthDoctor = item.PositionMainAnesthDoctor,
                        NameSubAnesthDoctor = item.NameSubAnesthDoctor,
                        EmailSubAnesthDoctor = item.EmailSubAnesthDoctor,
                        PhoneSubAnesthDoctor = item.PhoneSubAnesthDoctor,
                        PositionSubAnesthDoctor = item.PositionSubAnesthDoctor,
                        NameAnesthNurse1 = item.NameAnesthNurse1,
                        EmailAnesthNurse1 = item.EmailAnesthNurse1,
                        PhoneAnesthNurse1 = item.PhoneAnesthNurse1,
                        PositionAnesthNurse1 = item.PositionAnesthNurse1,
                        NameAnesthNurse2 = item.NameAnesthNurse2,
                        EmailAnesthNurse2 = item.EmailAnesthNurse2,
                        PhoneAnesthNurse2 = item.PhoneAnesthNurse2,
                        PositionAnesthNurse2 = item.PositionAnesthNurse2,
                        AnesthTitle = item.AnesthTitle,
                        dtAnesthStart = item.dtAnesthStart,
                        CleanTitle = item.CleanTitle,
                        dtCleanEnd = item.dtCleanEnd,

                        #endregion
                    });
                }
                model.TotalCount = data.TotalRows;
                model.listData = listPatient;
                model.listRoom = listPatient.Select(c => c.ORRoomId).Distinct().ToList();
                model.LastPage = EnumExtension.GetLastPage(model.TotalCount, AdminConfiguration.Paging_PageSize);
                model.HtmlPaging = Helper.StringExtensions.HtmlPaging(model.TotalCount, model.PageNumber, true, model.PageCount).Replace("/OHViewPatientsPublicByJson/?p", ("/OHViewPatientPublic?siteId=" + siteId + "&p")).Replace("/OHViewPatientsPublicByJson", ("/OHViewPatientPublic?siteId=" + siteId));
            }
            catch (Exception ex)
            {
                model.listData = new List<ORAnesthProgressExtContract>();
                model.TotalCount = 0;
            }
            return Json(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult OHViewPublicByJson(int pageIndex, int totalPage, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;

            List<ORAnesthProgressExtContract> listPatient = new List<ORAnesthProgressExtContract>();
            pageIndex = pageIndex + 1;
            pageIndex = pageIndex > totalPage ? 1 : pageIndex;

            var model = new OHViewPatientPublicContractModel()
            {
                listData = listPatient,
                //PageCount = 2,
                PageCount = AdminConfiguration.Paging_PageSize,
                PageNumber = pageIndex,
                kw = string.Empty,
                siteId = siteId,
                sourceClientId = sourceClientId
            };
            try
            {
                //test,nhớ chạy chinh thuc thi bo ra
                // var data = _orService.FindAnesthPublicInfo(Convert.ToDateTime(DateTime.Now.AddDays(-30).ToShortDateString() + " 00:00:00"), Convert.ToDateTime(DateTime.Now.AddDays(30).ToShortDateString() + " 23:59:59"), 0, string.Empty, pageIndex, model.PageCount, 0, 0, siteId, sourceClientId, 0);
                var data = _orService.FindAnesthPublicInfo(Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00"), Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59"), 0, string.Empty, pageIndex, AdminConfiguration.Paging_PageSize, 0, 0, siteId, sourceClientId, 0, true);
                if (data.TotalRows == 0) return Json(new List<ORAnesthProgressContract>());
                foreach (var item in data.Data)
                {
                    listPatient.Add(new ORAnesthProgressExtContract()
                    {
                        #region data
                        Id = item.Id,
                        dtStart = (item.dtStart ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat),
                        dtEnd = (item.dtEnd ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat),
                        PId = item.PId,
                        HoTen = item.HoTen,
                        Ages = item.Ages,
                        NamePTVMain = item.NamePTVMain,
                        HpServiceName = item.HpServiceName,
                        State = item.State,
                        CreatedBy = item.CreatedBy,
                        NgaySinh = item.NgaySinh,
                        Email = item.Email,
                        HospitalCode = item.HospitalCode,
                        HpServiceId = item.HpServiceId,
                        Sex = item.Sex,
                        dtOperation = (item.dtOperation ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat),
                        TimeAnesth = item.TimeAnesth ?? 0,
                        IsEmergence = item.IsEmergence,
                        RegDescription = string.IsNullOrEmpty(item.RegDescription) ? string.Empty : item.RegDescription,
                        ORRoomId = item.ORRoomId,
                        ORRoomName = item.ORRoomName,
                        StateName = EnumExtension.GetDescription((OHPatientStateEnum)item.State),
                        Color = HtmlExtensions.OHStateBackgroundColorPatient(item.State),
                        UIdPTVMain = item.UIdPTVMain,
                        UIdPTVSub1 = item.UIdPTVSub1,
                        UIdPTVSub2 = item.UIdPTVSub2,
                        UIdCECDoctor = item.UIdCECDoctor,
                        UIdNurseTool1 = item.UIdNurseTool1,
                        UIdNurseTool2 = item.UIdNurseTool2,
                        UIdNurseOutRun1 = item.UIdNurseOutRun1,
                        UIdNurseOutRun2 = item.UIdNurseOutRun2,
                        EmailPTVMain = item.EmailPTVMain,
                        PhonePTVMain = item.PhonePTVMain,
                        PositionPTVMain = item.PositionPTVMain,
                        NamePTVSub1 = item.NamePTVSub1,
                        EmailPTVSub1 = item.EmailPTVSub1,
                        PhonePTVSub1 = item.PhonePTVSub1,
                        PositionPTVSub1 = item.PositionPTVSub1,
                        NamePTVSub2 = item.NamePTVSub2,
                        EmailPTVSub2 = item.EmailPTVSub2,
                        PhonePTVSub2 = item.PhonePTVSub2,
                        PositionPTVSub2 = item.PositionPTVSub2,
                        NamePTVSub3 = item.NamePTVSub3,
                        EmailPTVSub3 = item.EmailPTVSub3,
                        PhonePTVSub3 = item.PhonePTVSub3,
                        PositionPTVSub3 = item.PositionPTVSub3,
                        NameCECDoctor = item.NameCECDoctor,
                        EmailCECDoctor = item.EmailCECDoctor,
                        PhoneCECDoctor = item.PhoneCECDoctor,
                        PositionCECDoctor = item.PositionCECDoctor,
                        NameNurseTool1 = item.NameNurseTool1,
                        EmailNurseTool1 = item.EmailNurseTool1,
                        PhoneNurseTool1 = item.PhoneNurseTool1,
                        PositionNurseTool1 = item.PositionNurseTool1,
                        NameNurseTool2 = item.NameNurseTool2,
                        EmailNurseTool2 = item.EmailNurseTool2,
                        PhoneNurseTool2 = item.PhoneNurseTool2,
                        PositionNurseTool2 = item.PositionNurseTool2,
                        NameNurseOutRun1 = item.NameNurseOutRun1,
                        EmailNurseOutRun1 = item.EmailNurseOutRun1,
                        PhoneNurseOutRun1 = item.PhoneNurseOutRun1,
                        PositionNurseOutRun1 = item.PositionNurseOutRun1,
                        NameNurseOutRun2 = item.NameNurseOutRun2,
                        EmailNurseOutRun2 = item.EmailNurseOutRun2,
                        PhoneNurseOutRun2 = item.PhoneNurseOutRun2,
                        PositionNurseOutRun2 = item.PositionNurseOutRun2,
                        AnesthDescription = item.AnesthDescription,
                        NameProject = item.NameProject,
                        VisitCode = item.VisitCode,
                        ProjectName = item.ProjectName,
                        NameCreatedBy = item.NameCreatedBy,
                        EmailCreatedBy = item.EmailCreatedBy,
                        PhoneCreatedBy = item.PhoneCreatedBy,
                        PositionCreatedBy = item.PositionCreatedBy,

                        UIdMainAnesthDoctor = item.UIdMainAnesthDoctor,
                        UIdSubAnesthDoctor = item.UIdSubAnesthDoctor,
                        UIdAnesthNurse1 = item.UIdAnesthNurse1,
                        UIdAnesthNurse2 = item.UIdAnesthNurse2,
                        NameMainAnesthDoctor = item.NameMainAnesthDoctor,
                        EmailMainAnesthDoctor = item.EmailMainAnesthDoctor,
                        PhoneMainAnesthDoctor = item.PhoneMainAnesthDoctor,
                        PositionMainAnesthDoctor = item.PositionMainAnesthDoctor,
                        NameSubAnesthDoctor = item.NameSubAnesthDoctor,
                        EmailSubAnesthDoctor = item.EmailSubAnesthDoctor,
                        PhoneSubAnesthDoctor = item.PhoneSubAnesthDoctor,
                        PositionSubAnesthDoctor = item.PositionSubAnesthDoctor,
                        NameAnesthNurse1 = item.NameAnesthNurse1,
                        EmailAnesthNurse1 = item.EmailAnesthNurse1,
                        PhoneAnesthNurse1 = item.PhoneAnesthNurse1,
                        PositionAnesthNurse1 = item.PositionAnesthNurse1,
                        NameAnesthNurse2 = item.NameAnesthNurse2,
                        EmailAnesthNurse2 = item.EmailAnesthNurse2,
                        PhoneAnesthNurse2 = item.PhoneAnesthNurse2,
                        PositionAnesthNurse2 = item.PositionAnesthNurse2,
                        AnesthTitle = item.AnesthTitle,
                        dtAnesthStart = item.dtAnesthStart,
                        CleanTitle = item.CleanTitle,
                        dtCleanEnd = item.dtCleanEnd,
                        // Sorting=item.Sorting

                        #endregion
                    });
                }
                model.TotalCount = data.TotalRows;
                model.listData = listPatient;
                model.listRoom = listPatient.Select(c => c.ORRoomId).Distinct().ToList();
                model.LastPage = EnumExtension.GetLastPage(model.TotalCount, AdminConfiguration.Paging_PageSize);
                model.HtmlPaging = Helper.StringExtensions.HtmlPaging(model.TotalCount, model.PageNumber, true, model.PageCount).Replace("/OHViewPublicByJson/?p", ("/OHViewPublic?siteId=" + siteId + "&p")).Replace("/OHViewPublicByJson", ("/OHViewPublic?siteId=" + siteId));
                model.HtmlPaging = model.HtmlPaging.Replace(@"""", "'");
                model.HtmlPaging = model.HtmlPaging.Replace(@"/'", @"'");
            }
            catch (Exception ex)
            {
                model.listData = new List<ORAnesthProgressExtContract>();
                model.TotalCount = 0;
            }
            return Json(model);
        }

        public ActionResult OHViewPublic(DateTime? FromDate, DateTime? ToDate, int State = 0, string kw = "", int p = 1, int ps = 0, int r = 0, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;
            if (siteId != memberExtendedInfo.CurrentLocaltion.NameEN)
            {
                //Redirect to site access denied
                return new RedirectResult(new UrlHelper(HttpContext.Request.RequestContext).RouteUrl("ErrorHandler", new { id = 1001 }));
            }
            //FromDate = DateTime.Now.AddDays(-30);//test
            //ToDate = DateTime.Now.AddDays(30);//test
            //siteId = "HCP";//test
            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion
            var listStates = new List<SelectListItem>();
            var listCheckListTypes = new List<SelectListItem>();
            var listSystems = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (!FromDate.HasValue) FromDate = DateTime.Now;
            FromDate = Convert.ToDateTime((FromDate ?? DateTime.Now).ToShortDateString() + " 00:00:00");
            if (!ToDate.HasValue) ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
            #region state
            listStates =
                EnumExtension.ToListOfValueAndDesc<OHPatientStateEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
            #endregion state

            var data = _orService.FindAnesthPublicInfo(FromDate ?? DateTime.Now, ToDate ?? DateTime.Now, State, kw, p, ps, 0, r, siteId, sourceClientId, 0, true);

            var model = new OHViewPatientPublicModel()
            {
                listData = data.Data,
                TotalCount = data.TotalRows,
                PageCount = ps,
                PageNumber = p,
                listStates = listStates,
                State = State,
                kw = kw,
                FromDate = FromDate ?? DateTime.Now,
                ToDate = ToDate ?? DateTime.Now,
                siteId = siteId,
                sourceClientId = sourceClientId
            };
            return View(model);
        }
        #endregion

        public ActionResult OHSearchPatients(OHQueuePatientSearchParam param)
        {
            //set default current location
            param.siteId = (string.IsNullOrEmpty(param.siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : param.siteId;

            ViewBag.SearchParam = param;
            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(param.siteId)))
            {
                param.siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(param.siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion

            if (param.Keyword == null)
                param.Keyword = string.Empty;
            if (param.export)
            {
                param.p = 1;
                param.PageSize = Int16.MaxValue;
                var searchResult = _orService.FindAnesthPublicInfo(param.FromDate, param.ToDate, param.StateId, param.Keyword, param.p, param.PageSize, 0, param.RoomId, param.siteId, param.sourceClientId, 0, false);
                if (searchResult != null && searchResult.Data != null && searchResult.Data.Any())
                {
                    return ExportORPatient(searchResult.Data,
                    "danh-sach-benh-nhan" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"), (DateTime)param.FromDate, (DateTime)param.ToDate);
                }
                return null;
            }
            else
            {

                var listStates = new List<SelectListItem>();
                var listHpServices = new List<SelectListItem>();
                var listORRooms = new List<SelectListItem>();

                listStates =
                    EnumExtension.ToListOfValueAndDesc<OHPatientStateEnum>().Select(e => new SelectListItem
                    {
                        Text = e.Description,
                        Value = e.Value.ToString()
                    }).ToList();
                listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
                var dataService = _orService.GetListHpServices(param.siteId, (int)SourceClientEnum.Oh);
                if (dataService != null && dataService.Any())
                {
                    dataService.Insert(0, new HpServiceContract() { Id = 0, Name = "Tất cả" });
                    listHpServices = dataService.Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Id.ToString()
                    }).ToList();
                }

                var dataORRooms = _orService.GetListRoom(param.siteId, string.Empty, (int)SourceClientEnum.Oh, -1, "1", true);
                if (dataORRooms != null && dataORRooms.Any())
                {
                    dataORRooms.Insert(0, new ORRoomContract() { Id = 0, Name = "Tất cả" });
                    listORRooms = dataORRooms.Select(r => new SelectListItem
                    {
                        Text = r.Name,
                        Value = r.Id.ToString()
                    }).ToList();
                }


                ViewBag.States = listStates;
                ViewBag.Rooms = listORRooms;
                ViewBag.Sites = listSites;
                var searchResult = _orService.FindAnesthPublicInfo(param.FromDate, param.ToDate, param.StateId, param.Keyword, param.p, param.PageSize, 0, param.RoomId, param.siteId, param.sourceClientId, 0, false);

                return View(searchResult);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSortting(long Id, int value)
        {
            CUDReturnMessage updateResult = _orService.UpdateSorting(Id, value, CurrentUserId);
            if (updateResult == null)
                return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Fail, message = MessageResource.CMS_SystemError_Message });
            if (updateResult.Id == (int)ResponseCode.Successed)
                return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Success, message = MessageResource.CMS_UpdateSuccessed });
            else if (updateResult.Id == (int)ResponseCode.AdminRole_Accessdenied)
            {
                return Json(updateResult);
            }
            return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Fail, message = MessageResource.CMS_UpdateFailed });
        }

        public ActionResult CUDPatientOH(int Id = 0)
        {
            var model = new ORAnesthCUDProgressModel();
            model.dtStart = DateTime.Now;
            model.dtOperation = DateTime.Now;
            model.dtEnd = DateTime.Now;
            if (Id == 0)
                return RedirectToAction("OHSearchPatients", "OR");
            var orProgress = _orService.GetORAnesthProgress(Id.ToString(), (int)TypeSearchEnum.Id);
            if (orProgress != null)
            {
                SetDataModel(ref model, orProgress);
            }
            else
            {
                return RedirectToAction("OHSearchPatients", "OR");
            }

            var listStates =
                EnumExtension.ToListOfValueAndDesc<OHPatientStateEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listStates.Remove(listStates.Where(x => new List<int> { 3 }.Contains(Convert.ToInt32(x.Value))).SingleOrDefault());
            listStates.Where(x => x.Value == "5").Select(x => { x.Text = "Vừa duyệt đăng ký xong (Create)"; return x; }).ToList();
            model.listStates = listStates;
            return View(model);
        }

        #region private method
        private ActionResult ExportORPatient(List<ORAnesthProgressContract> datatable, string filename, DateTime fromDate, DateTime toDate)
        {

            datatable = datatable.OrderBy(s => s.ORRoomId).ThenBy(p => p.Sorting).ThenBy(d => d.State).ToList();
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("patient infos");
            int maxColumnIndex = 14;
            int startGridRowsIndex = 6;
            int excelRowCount = datatable.Count();

            #region format header

            //  Row report name
            using (var reportHeader = workSheet.Cells[1, 1, 1, maxColumnIndex])
            {
                reportHeader.Merge = true;
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Style.Font.Bold = true;
                reportHeader.Style.Font.Size = 16;
                reportHeader.Value = "LIST PATIENTS (DANH SÁCH BỆNH NHÂN)";
            }

            //Ngày xuất từ
            using (var reportHeader = workSheet.Cells[2, 1, 2, 2])
            {
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Style.Font.Bold = true;
                reportHeader.Merge = true;
                reportHeader.Value = "FromDate (Từ ngày)";
            }

            using (var reportHeader = workSheet.Cells[2, 3, 2, 4])
            {
                reportHeader.Merge = true;
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Value = fromDate.ToString("dd/MM/yyyy");
            }
            //Ngày xuất đến
            using (var reportHeader = workSheet.Cells[3, 1, 3, 2])
            {
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Style.Font.Bold = true;
                reportHeader.Merge = true;
                reportHeader.Value = "Todate (Đến ngày)";
            }
            using (var reportHeader = workSheet.Cells[3, 3, 3, 4])
            {
                reportHeader.Merge = true;
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Value = toDate.ToString("dd/MM/yyyy");
            }


            #endregion format header

            #region format grid header

            using (var header = workSheet.Cells[startGridRowsIndex, 1, startGridRowsIndex, maxColumnIndex])
            {
                header.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                header.Style.Font.Bold = true;
                header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                var headerBgColor = System.Drawing.ColorTranslator.FromHtml("#dbdbdb");
                header.Style.Fill.BackgroundColor.SetColor(headerBgColor);
                header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                header.Style.WrapText = true;
            }
            // Border cho lưới data
            using (var header = workSheet.Cells[startGridRowsIndex, 1, startGridRowsIndex + excelRowCount, maxColumnIndex])
            {
                header.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                header.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                header.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                header.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                header.Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
                header.Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
                header.Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
                header.Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
            }

            //  Bind grid header name
            workSheet.Cells[startGridRowsIndex, 1].Value = "STT";
            workSheet.Cells[startGridRowsIndex, 2].Value = "Operation Room(Phòng mổ)";
            workSheet.Cells[startGridRowsIndex, 3].Value = "Start time(Thời gian bắt đầu)";
            workSheet.Cells[startGridRowsIndex, 4].Value = "End time(Thời gian kết thúc)";
            workSheet.Cells[startGridRowsIndex, 5].Value = "PID (Mã bệnh nhân)";
            workSheet.Cells[startGridRowsIndex, 6].Value = "Patient Name(Tên bệnh nhân)";
            workSheet.Cells[startGridRowsIndex, 7].Value = "Sex(Giới tính)";
            workSheet.Cells[startGridRowsIndex, 8].Value = "Age(Tuổi)";
            workSheet.Cells[startGridRowsIndex, 9].Value = "Ward(Khu vực)";
            workSheet.Cells[startGridRowsIndex, 10].Value = "Surgeon/Anesth (Ekip phẩu thuật)";
            workSheet.Cells[startGridRowsIndex, 11].Value = "Procedure(Tên phẩu thuật)";
            workSheet.Cells[startGridRowsIndex, 12].Value = "Readline(Trạng thái)";
            workSheet.Cells[startGridRowsIndex, 13].Value = "Remarks(Ghi nhận)";
            //  Bind grid data
            int k = 1;
            foreach (var it in datatable)
            {

                #region Thông tin mổ
                string htmlPatient = string.Empty;

                if (it.UIdPTVMain > 0 || it.UIdPTVSub1 > 0 || it.UIdPTVSub2 > 0 || it.UIdPTVSub3 > 0 || it.UIdCECDoctor > 0 || it.UIdNurseTool1 > 0
                               || it.UIdNurseTool2 > 0 || it.UIdNurseOutRun1 > 0 || it.UIdNurseOutRun2 > 0)
                {
                    htmlPatient += " Thông tin mổ \r\n";
                }

                if (it.UIdPTVMain > 0)
                {
                    htmlPatient += "Bác sĩ Mổ chính: " + it.NamePTVMain + " \r\n";
                }
                if (it.UIdPTVSub1 > 0)
                {
                    htmlPatient += "Bác sĩ Mổ phụ 1:" + it.NamePTVSub1 + "\r\n";
                }
                if (it.UIdPTVSub2 > 0)
                {
                    htmlPatient += "Bác sĩ Mổ phụ 2: " + it.NamePTVSub2 + "\r\n";
                }
                if (it.UIdPTVSub3 > 0)
                {
                    htmlPatient += "Bác sĩ Mổ phụ 3: " + it.NamePTVSub3 + "\r\n";
                }

                if (it.UIdCECDoctor > 0)
                {
                    htmlPatient += "Bác sĩ CEC: " + it.NameCECDoctor + "\r\n";
                }
                if (it.UIdNurseTool1 > 0)
                {
                    htmlPatient += "Điều dưỡng dụng cụ 1: " + it.NameNurseTool1 + "\r\n";
                }

                if (it.UIdNurseTool2 > 0)
                {
                    htmlPatient += "Điều dưỡng dụng cụ 2: " + it.NameNurseTool2 + "\r\n";
                }

                if (it.UIdNurseOutRun1 > 0)
                {
                    htmlPatient += "Điều dưỡng chạy ngoài 1: " + it.NameNurseOutRun1 + "\r\n";
                }

                if (it.UIdNurseOutRun2 > 0)
                {
                    htmlPatient += "Điều dưỡng chạy ngoài 2: " + it.NameNurseOutRun2 + "\r\n";
                }


                if (it.UIdMainAnesthDoctor > 0 || it.UIdSubAnesthDoctor > 0 || it.UIdAnesthNurse1 > 0 || it.UIdAnesthNurse2 > 0)
                {
                    htmlPatient += " Thông tin gây mê \r\n";
                }


                if (it.UIdMainAnesthDoctor > 0)
                {
                    htmlPatient += "Bác sĩ gây mê 1: " + it.NameMainAnesthDoctor + "\r\n";
                }
                if (it.UIdSubAnesthDoctor > 0)
                {
                    htmlPatient += "Bác sĩ gây mê 2: " + it.NameSubAnesthDoctor + "\r\n";
                }
                if (it.UIdAnesthNurse1 > 0)
                {
                    htmlPatient += "Điều dưỡng phụ mê 1: " + it.NameAnesthNurse1 + "\r\n";
                }
                if (it.UIdAnesthNurse2 > 0)
                {
                    htmlPatient += "Điều dưỡng phụ mê 2: " + it.NameAnesthNurse2 + "\r\n";
                }

                #endregion
                workSheet.Cells[startGridRowsIndex + k, 1].Value = k;
                workSheet.Cells[startGridRowsIndex + k, 2].Value = it.ORRoomName;
                workSheet.Cells[startGridRowsIndex + k, 3].Value = (it.dtStart ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat);
                workSheet.Cells[startGridRowsIndex + k, 4].Value = (it.dtEnd ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat);
                workSheet.Cells[startGridRowsIndex + k, 5].Value = it.PId;
                workSheet.Cells[startGridRowsIndex + k, 6].Value = it.HoTen;
                workSheet.Cells[startGridRowsIndex + k, 7].Value = it.Sex == 1 ? "Nam" : it.Sex == 2 ? "Nữ" : "Chư xác định";
                workSheet.Cells[startGridRowsIndex + k, 8].Value = it.Ages;
                workSheet.Cells[startGridRowsIndex + k, 9].Value = string.Empty;
                workSheet.Cells[startGridRowsIndex + k, 10].Value = htmlPatient;
                workSheet.Cells[startGridRowsIndex + k, 11].Value = it.HpServiceName;
                workSheet.Cells[startGridRowsIndex + k, 12].Value = EnumExtension.GetDescription((OHPatientStateEnum)(it.State));
                workSheet.Cells[startGridRowsIndex + k, 13].Value = it.RegDescription;
                k++;
            }
            workSheet.Cells[6, 1, excelRowCount, maxColumnIndex].AutoFitColumns();

            #endregion format grid header

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;
        }


        #endregion


        private void SetDataModel(ref ORAnesthCUDProgressModel model, ORAnesthProgressContract r)
        {
            model.Id = r.Id;
            model.PId = r.PId;
            model.VisitCode = r.VisitCode;
            model.HoTen = r.HoTen;
            model.NgaySinh = r.NgaySinh;
            model.Sex = r.Sex;
            model.Address = r.Address;
            model.Email = r.Email;
            model.Ages = r.Ages;
            model.NameProject = r.NameProject;
            model.HpServiceId = r.HpServiceId;
            //Get Service detail
            #region .Get Service detail
            model.Service = _orService.GetServiceById(r.HpServiceId);
            #endregion .Get Service detail
            model.ORRoomId = r.ORRoomId;
            model.dtStart = r.dtStart ?? DateTime.Now;
            model.dtEnd = r.dtEnd ?? DateTime.Now;
            model.dtOperation = r.dtOperation ?? DateTime.Now;

            model.HospitalName = r.HospitalName;
            model.HospitalPhone = r.HospitalPhone;
            model.PatientPhone = r.PatientPhone;

            model.HospitalCode = r.HospitalCode;

            model.State = r.State;
            model.TimeAnesth = r.TimeAnesth ?? 0;

            model.NameCreatedBy = !(string.IsNullOrEmpty(r.NameCreatedBy)) ? r.NameCreatedBy : CurrentUserDisplayName;
            model.EmailCreatedBy = !(string.IsNullOrEmpty(r.EmailCreatedBy)) ? r.EmailCreatedBy : CurrentEmail;
            model.CreatedBy = r.CreatedBy != 0 ? r.CreatedBy : CurrentUserId;


            model.NameMainAnesthDoctor = r.NameMainAnesthDoctor;
            model.EmailMainAnesthDoctor = r.EmailMainAnesthDoctor;
            model.PhoneMainAnesthDoctor = r.PhoneMainAnesthDoctor;
            model.PositionMainAnesthDoctor = r.PositionMainAnesthDoctor;
            model.UIdMainAnesthDoctor = r.UIdMainAnesthDoctor ?? 0;

            model.NameCreatedBy = r.NameCreatedBy;
            model.EmailCreatedBy = r.EmailCreatedBy;
            model.PhoneCreatedBy = r.PhoneCreatedBy;
            model.PositionCreatedBy = r.PositionCreatedBy;
            model.CreatedBy = r.CreatedBy;


            model.NameAnesthNurse1 = r.NameAnesthNurse1;
            model.EmailAnesthNurse1 = r.EmailAnesthNurse1;
            model.PhoneAnesthNurse1 = r.PhoneAnesthNurse1;
            model.PositionAnesthNurse1 = r.PositionAnesthNurse1;
            model.UIdAnesthNurse1 = r.UIdAnesthNurse1 ?? 0;

            model.NameAnesthNurse2 = r.NameAnesthNurse2;
            model.EmailAnesthNurse2 = r.EmailAnesthNurse2;
            model.PhoneAnesthNurse2 = r.PhoneAnesthNurse2;
            model.PositionAnesthNurse2 = r.PositionAnesthNurse2;
            model.UIdAnesthNurse2 = r.UIdAnesthNurse2 ?? 0;

            model.NameSubAnesthDoctor = r.NameSubAnesthDoctor;
            model.EmailSubAnesthDoctor = r.EmailSubAnesthDoctor;
            model.PhoneSubAnesthDoctor = r.PhoneSubAnesthDoctor;
            model.PositionSubAnesthDoctor = r.PositionSubAnesthDoctor;
            model.UIdSubAnesthDoctor = r.UIdSubAnesthDoctor ?? 0;
            model.HospitalName = r.HospitalName;
            model.HospitalPhone = r.HospitalPhone;
            model.PatientPhone = r.PatientPhone;
            model.HospitalCode = r.HospitalCode;

            model.State = r.State;
            model.TimeAnesth = r.TimeAnesth ?? 0;
            model.AnesthDescription = r.AnesthDescription;
            model.ORRoomName = r.ORRoomName;
            model.HpServiceName = r.HpServiceName;
            model.HospitalName = r.HospitalName;
            model.HospitalPhone = r.HospitalPhone;
            model.PatientPhone = r.PatientPhone;
            model.ShowdtEnd = model.dtEnd.ToVEShortTime();
            model.ShowdtStart = model.dtStart.ToVEShortTime();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeORProgressState(long Id, int State)
        {
            try
            {
                int iTrueState = 0;
                var orProgress = _orService.GetORAnesthProgress(Id.ToString(), (int)TypeSearchEnum.Id);
                if (orProgress != null)
                {
                    //Set TH state that, Trong TH chi duyet Mo? thi state that=3, da duyet gay me=5
                    iTrueState = (State == 5) ? ((orProgress.UIdMainAnesthDoctor > 0) ? State : (int)OHPatientStateEnum.DuyetMo) : State;
                }
                CUDReturnMessage response = _orService.ChangeState(Id, iTrueState, CurrentUserId);
                //log.Debug("Store log change state of OR Progress: " + response.Message);
                var actionMessageExt = new ActionMessage(response);
                return Json(actionMessageExt);
            }
            catch (Exception ex)
            {
                //log.Debug("Error when change state of OR Progress: " + ex);
                return Json(new CUDReturnMessage() { Id = 0, Message = "Có lỗi trong quá trình xử lý! Liên hệ Hỗ trợ VH để biết thêm thông tin." });
            }

        }

        public ActionResult SearchHpService(int State = -1, string kw = "", int p = 1, int ps = 0, int HpServiceId = 0, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;

            var listStates = new List<SelectListItem>();
            kw = HttpUtility.UrlDecode(kw);
            var listHpServices = new List<SelectListItem>();
            var listORRooms = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;

            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion
            listStates =
                EnumExtension.ToListOfValueAndDesc<SearchStateEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listStates.Insert(0, AdminGlobal.DefaultValue.DefaultStateSelectListItem);

            var dataService = _orService.SearchHpServiceInfo(State, kw, p, ps, HpServiceId, siteId, sourceClientId);
            int TotalRows = (dataService != null && dataService.Any()) ? dataService.FirstOrDefault().TotalRecords : 1;
            var model = new SearchHpServiceModel()
            {
                listData = (dataService != null) ? dataService.ToList() : new List<HpServiceSite>(),
                TotalCount = TotalRows,
                listSites = listSites,
                PageCount = ps,
                PageNumber = p,
                listStates = listStates,
                State = State,
                kw = kw,
                siteId = siteId,
                sourceClientId = sourceClientId,
                HpServiceId = HpServiceId,
                currentUserId = CurrentUserId
            };
            return View(model);
        }
        public ActionResult CUDHpService(int Id = 0, string siteId = "")
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;

            HpServiceModel model;
            var listLocation = _locationApi.Get();

            #region master data site
            var listSites = new List<SelectListItem>();
            if (listLocation != null && listLocation.IsNotNullAndNotEmpty() && listLocation.Where(l => l.LevelNo > 1 && !l.IsDeleted).ToList().Any())
            {
                foreach (var item in listLocation.Where(l => l.LevelNo > 1 && !l.NameEN.Equals("HTC")))
                {

                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion

            #region init data model
            if (Id > 0)
            {
                var response = _orService.SearchHpServiceInfo(-1, string.Empty, 1, 100, Id, string.Empty).FirstOrDefault();
                if (response == null)
                {
                    model = new HpServiceModel()
                    {
                        Id = Id
                    };
                    ViewBag.ErrorMessage = MessageResource.Shared_SystemErrorMessage;
                }
                else
                {
                    model = new HpServiceModel()
                    {
                        Id = response.Id,
                        Name = response.Name,
                        Oh_Code = response.Oh_Code,
                        CleaningTime = response.CleaningTime,
                        PreparationTime = response.PreparationTime,
                        AnesthesiaTime = response.AnesthesiaTime,
                        OtherTime = response.OtherTime,
                        Description = response.Description,
                        lstSiteId = response.listSites == null ? new List<string>() : response.listSites.Select(r => r.HospitalCode).ToList(),
                        listSites = listSites,
                        siteId = siteId
                    };
                }
            }
            else
            {
                model = new HpServiceModel()
                {
                    Id = 0,
                    Name = "",
                    Oh_Code = "",
                    CleaningTime = 0,
                    PreparationTime = 0,
                    AnesthesiaTime = 0,
                    OtherTime = 0,
                    Description = "",
                    lstSiteId = new List<string>(),
                    listSites = listSites,
                    siteId = siteId
                };
            }
            #endregion
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CUDHpService(HpServiceModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new HpServiceSite
                {
                    Id = model.Id,
                    Oh_Code = model.Oh_Code,
                    Name = model.Name,
                    CleaningTime = model.CleaningTime,
                    PreparationTime = model.PreparationTime,
                    AnesthesiaTime = model.AnesthesiaTime,
                    OtherTime = model.OtherTime,
                    Description = model.Description,
                    IdMapping = model.IdMapping,
                    SourceClientId = (int)SourceClientEnum.Oh,
                    Type = 2,
                    listSites = model.lstSiteId != null ? model.lstSiteId.Select(r => new SiteShortContract() { Id = r, SiteName = string.Empty, HospitalCode = r }).ToList() : new List<SiteShortContract>()
                };
                if (service.listSites == null) service.listSites = new List<SiteShortContract>();
                var response = _orService.CUDHpService(service, CurrentUserId);
                if (response == null)
                {
                    response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                else if (response.Id > 0)
                {
                    response.SystemMessage = "Shared_SaveSuccess";
                    var msg = new ActionMessage(1, response.SystemMessage);
                    TempData[AdminGlobal.ActionMessage] = msg;

                    return Json(msg);
                }
                else if (response.Id == (int)ResponseCode.AdminRole_Accessdenied)
                {
                    response.SystemMessage = "AdminRole_Accessdenied";
                }
                else
                {
                    response.SystemMessage = "CMS_GetRuntimeErrorMsg";
                }
                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                return Json(response);
            }
            var cudMsg = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
            return Json(new ActionMessage(cudMsg));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CheckExistPositionByScheduler(long Id, int UId)
        {
            CUDReturnMessage response = _orService.CheckExistPositionByScheduler(Id, UId);
            var actionMessageExt = new ActionMessage(response);
            return Json(actionMessageExt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ClearCachingAll()
        {
            var result = _orService.ClearCachingAll();
            var response = new CUDReturnMessage() { Id = result ? 1 : 0, Message = string.Empty };
            var actionMessageExt = new ActionMessage(response);
            return Json(actionMessageExt);
        }

        #region OR tracking

        public ActionResult ListORTracking(DateTime? fromDate, DateTime? toDate, string kw = "", int p = 1, int ps = 20, string siteId = "", string export = "")
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;

            var listLocation = _locationApi.Get();
            #region master data site
            var listSites = new List<SelectListItem>();
            if (listLocation != null && listLocation.IsNotNullAndNotEmpty() && listLocation.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in listLocation.Where(l => l.LevelNo > 1 && !l.NameEN.Equals("HTC")))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion


            kw = HttpUtility.UrlDecode(kw).Trim();
            if (fromDate.HasValue == false) fromDate = DateTime.Now.AddDays(-6);
            if (toDate.HasValue == false) toDate = DateTime.Now;

            fromDate = fromDate.Value.AddTimeToTheStartOfDay();
            toDate = toDate.Value.AddTimeToTheEndOfDay();

            var data = _orService.ListORTracking(fromDate.Value, toDate.Value, kw, p, ps, siteId);
            var model = new ORTrackingModel
            {
                TotalCount = data.Count,
                PageCount = ps,
                PageNumber = p,
                FromDate = fromDate.Value,
                ToDate = toDate.Value,
                ListTracking = data == null ? null : data.List,
                siteId = siteId,
                listSites = listSites
            };

            if (!string.IsNullOrEmpty(export) && export == "True")
            {
                if (data != null && data.List != null)
                {
                    return ExportTrackingORUser(data.List,
                    "tracking-or-user-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"), (DateTime)fromDate, (DateTime)toDate);
                }
                return null;
            }
            return View(model);
        }
        #endregion

        #region Search HP Service 
        public ActionResult SearchHpServiceExt(string kw = "", int page = 1)
        {
            var listSites = new List<SelectListItem>();
            //var siteId = "HCP";
            var siteId = memberExtendedInfo.CurrentLocaltion.NameEN;
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");

            var data = _orService.SearchHpServiceExt(siteId, kw, page, 10, (int)SourceClientEnum.Oh);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private JsonResult IsEmail(bool isRequired, string emailRegex, string email, string field)
        {
            if (string.IsNullOrEmpty(email))
            {
                if (isRequired)
                    return Json(new ActionMessage(-1, $"{field} chưa cung cấp"));
                else
                    return null;
            }
            else
            {
                Regex regex = new Regex(emailRegex);
                Match match = regex.Match(email);
                if (!match.Success)
                    return Json(new ActionMessage(-1, $"{field} sai định dạng"));
            }
            return null;
        }

        private ActionResult ExportTrackingORUser(List<ORTrackingContract> datatable, string filename, DateTime fromDate, DateTime toDate)
        {
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Tracking OR User");
            int maxColumnIndex = 6;
            int startGridRowsIndex = 6;
            int excelRowCount = datatable.Count();

            #region format header

            //  Row report name
            using (var reportHeader = workSheet.Cells[1, 1, 1, maxColumnIndex])
            {
                reportHeader.Merge = true;
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Style.Font.Bold = true;
                reportHeader.Style.Font.Size = 16;
                reportHeader.Value = "DANH SÁCH Tracking OR User";
            }

            //Ngày xuất từ
            using (var reportHeader = workSheet.Cells[2, 1, 2, 2])
            {
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Style.Font.Bold = true;
                reportHeader.Merge = true;
                reportHeader.Value = "FromDate (Từ ngày)";
            }

            using (var reportHeader = workSheet.Cells[2, 3, 2, 4])
            {
                reportHeader.Merge = true;
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Value = fromDate.ToString("dd/MM/yyyy");
            }
            //Ngày xuất đến
            using (var reportHeader = workSheet.Cells[3, 1, 3, 2])
            {
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Style.Font.Bold = true;
                reportHeader.Merge = true;
                reportHeader.Value = "Todate (Đến ngày)";
            }
            using (var reportHeader = workSheet.Cells[3, 3, 3, 4])
            {
                reportHeader.Merge = true;
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Value = toDate.ToString("dd/MM/yyyy");
            }


            #endregion format header

            #region format grid header

            using (var header = workSheet.Cells[startGridRowsIndex, 1, startGridRowsIndex, maxColumnIndex])
            {
                header.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                header.Style.Font.Bold = true;
                header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                var headerBgColor = System.Drawing.ColorTranslator.FromHtml("#dbdbdb");
                header.Style.Fill.BackgroundColor.SetColor(headerBgColor);
                header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                header.Style.WrapText = true;
            }
            // Border cho lưới data
            using (var header = workSheet.Cells[startGridRowsIndex, 1, startGridRowsIndex + excelRowCount, maxColumnIndex])
            {
                header.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                header.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                header.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                header.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                header.Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
                header.Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
                header.Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
                header.Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
            }

            //  Bind grid header name
            workSheet.Cells[startGridRowsIndex, 1].Value = "STT";
            workSheet.Cells[startGridRowsIndex, 2].Value = "User Id";
            workSheet.Cells[startGridRowsIndex, 3].Value = "Họ và tên";
            workSheet.Cells[startGridRowsIndex, 4].Value = "Email";
            workSheet.Cells[startGridRowsIndex, 5].Value = "Thông tin";
            workSheet.Cells[startGridRowsIndex, 6].Value = "Nội dung";

            //  Bind grid data
            int k = 1;
            foreach (var it in datatable)
            {

                #region Thông tin Tracking OR User
                string htmlContent = string.Empty;
                var trackings = it.ContentTracking.Split(new string[] { "\"" }, StringSplitOptions.RemoveEmptyEntries);

                var pid = ReturnContent(trackings, "PId", "Patient Id");
                var hoTen = ReturnContent(trackings, "HoTen", "Họ và tên");
                var ngaySinh = ReturnContent(trackings, "NgaySinh", "Ngày sinh");
                var diaChi = ReturnContent(trackings, "Address", "Địa chỉ");
                var ptvChinh = ReturnContent(trackings, "NamePTVMain", "PTV chính");
                var ptvPhu1 = ReturnContent(trackings, "NamePTVSub1", "PTV phụ 1");
                var ptvPhu2 = ReturnContent(trackings, "NamePTVSub2", "PTV phụ 2");
                var ptvPhu3 = ReturnContent(trackings, "NamePTVSub3", "PTV phụ 3");
                var ptvPhu4 = ReturnContent(trackings, "NamePTVSub4", "PTV phụ 4");
                var ptvPhu5 = ReturnContent(trackings, "NamePTVSub5", "PTV phụ 5");
                var ptvPhu6 = ReturnContent(trackings, "NamePTVSub6", "PTV phụ 6");
                var ptvPhu7 = ReturnContent(trackings, "NamePTVSub7", "PTV phụ 7");
                var ptvPhu8 = ReturnContent(trackings, "NamePTVSub8", "PTV phụ 8");
                var ptvCEC = ReturnContent(trackings, "NameCECDoctor", "PTV CEC");
                var dungCu1 = ReturnContent(trackings, "NameNurseTool1", "Điều dưỡng dụng cụ 1");
                var dungCu2 = ReturnContent(trackings, "NameNurseTool2", "Điều dưỡng dụng cụ 2:");
                var chayNgoai1 = ReturnContent(trackings, "NameNurseOutRun1", "Điều dưỡng chạy ngoài 1");
                var chayNgoai2 = ReturnContent(trackings, "NameNurseOutRun2", "Điều dưỡng chạy ngoài 2");
                var chayNgoai3 = ReturnContent(trackings, "NameNurseOutRun3", "Điều dưỡng chạy ngoài 3");
                var chayNgoai4 = ReturnContent(trackings, "NameNurseOutRun4", "Điều dưỡng chạy ngoài 4");
                var chayNgoai5 = ReturnContent(trackings, "NameNurseOutRun5", "Điều dưỡng chạy ngoài 5");
                var chayNgoai6 = ReturnContent(trackings, "NameNurseOutRun6", "Điều dưỡng chạy ngoài 6");
                //vutv7
                var ktvPhuMo = ReturnContent(trackings, "NameKTVSubSurgery", "KTV phụ mổ");
                var ktvCDHA = ReturnContent(trackings, "NameKTVDiagnose", "KTV CĐHA");
                var ktvCEC = ReturnContent(trackings, "NameKTVCEC", "KTV chạy máy CEC");
                var bsCDHA = ReturnContent(trackings, "NameDoctorDiagnose", "BS CĐHA");
                var bsSoSinh = ReturnContent(trackings, "NameDoctorNewBorn", "BS sơ sinh");
                var nuHoSinh = ReturnContent(trackings, "NameMidwives", "Nữ hộ sinh");

                var bsGayMe = ReturnContent(trackings, "NameMainAnesthDoctor", "Bác sỹ gây mê");
                var bsPhuMe = ReturnContent(trackings, "NameSubAnesthDoctor", "Bác sỹ phụ mê 1");
                var bsPhuMe2 = ReturnContent(trackings, "NameSubAnesthDoctor2", "Bác sỹ phụ mê 2");
                var bsKhamGayMe = ReturnContent(trackings, "NameAnesthesiologist", "BS khám gây mê");
                var phuMe1 = ReturnContent(trackings, "NameAnesthNurse1", "Điều dưỡng phụ mê 1");
                var phuMe2 = ReturnContent(trackings, "NameAnesthNurse2", "Điều dưỡng phụ mê 2");
                var ddHoitinh = ReturnContent(trackings, "NameAnesthNurseRecovery", "Điều dưỡng hồi tỉnh");

                htmlContent += $"Ngày tạo: {it.CreatedDate.ToVEFullDateTime()} \r\n";
                if (!string.IsNullOrEmpty(pid))
                    htmlContent += pid;
                if (!string.IsNullOrEmpty(hoTen))
                    htmlContent += hoTen;
                if (!string.IsNullOrEmpty(ngaySinh))
                    htmlContent += ngaySinh;
                if (!string.IsNullOrEmpty(diaChi))
                    htmlContent += diaChi;
                if (!string.IsNullOrEmpty(ptvChinh))
                    htmlContent += ptvChinh;
                if (!string.IsNullOrEmpty(ptvPhu1))
                    htmlContent += ptvPhu1;
                if (!string.IsNullOrEmpty(ptvPhu2))
                    htmlContent += ptvPhu2;
                if (!string.IsNullOrEmpty(ptvPhu3))
                    htmlContent += ptvPhu3;
                if (!string.IsNullOrEmpty(ptvPhu4))
                    htmlContent += ptvPhu4;
                if (!string.IsNullOrEmpty(ptvPhu5))
                    htmlContent += ptvPhu5;
                if (!string.IsNullOrEmpty(ptvPhu6))
                    htmlContent += ptvPhu6;
                if (!string.IsNullOrEmpty(ptvPhu7))
                    htmlContent += ptvPhu7;
                if (!string.IsNullOrEmpty(ptvPhu8))
                    htmlContent += ptvPhu8;
                if (!string.IsNullOrEmpty(ptvCEC))
                    htmlContent += ptvCEC;
                if (!string.IsNullOrEmpty(dungCu1))
                    htmlContent += dungCu1;
                if (!string.IsNullOrEmpty(dungCu2))
                    htmlContent += dungCu2;
                if (!string.IsNullOrEmpty(chayNgoai1))
                    htmlContent += chayNgoai1;
                if (!string.IsNullOrEmpty(chayNgoai2))
                    htmlContent += chayNgoai2;
                if (!string.IsNullOrEmpty(chayNgoai3))
                    htmlContent += chayNgoai3;
                if (!string.IsNullOrEmpty(chayNgoai4))
                    htmlContent += chayNgoai4;
                if (!string.IsNullOrEmpty(chayNgoai5))
                    htmlContent += chayNgoai5;
                if (!string.IsNullOrEmpty(chayNgoai6))
                    htmlContent += chayNgoai6;
                //VUTV7
                if (!string.IsNullOrEmpty(ktvPhuMo))
                    htmlContent += ktvPhuMo;
                if (!string.IsNullOrEmpty(ktvCDHA))
                    htmlContent += ktvCDHA;
                if (!string.IsNullOrEmpty(ktvCEC))
                    htmlContent += ktvCEC;
                if (!string.IsNullOrEmpty(bsCDHA))
                    htmlContent += bsCDHA;
                if (!string.IsNullOrEmpty(bsSoSinh))
                    htmlContent += bsSoSinh;
                if (!string.IsNullOrEmpty(nuHoSinh))
                    htmlContent += nuHoSinh;

                if (!string.IsNullOrEmpty(bsGayMe))
                    htmlContent += bsGayMe;
                if (!string.IsNullOrEmpty(bsPhuMe))
                    htmlContent += bsPhuMe;
                if (!string.IsNullOrEmpty(bsPhuMe2))
                    htmlContent += bsPhuMe2;
                if (!string.IsNullOrEmpty(bsKhamGayMe))
                    htmlContent += bsKhamGayMe;
                if (!string.IsNullOrEmpty(phuMe1))
                    htmlContent += phuMe1;
                if (!string.IsNullOrEmpty(phuMe2))
                    htmlContent += phuMe2;
                if (!string.IsNullOrEmpty(ddHoitinh))
                    htmlContent += ddHoitinh;

                #endregion
                workSheet.Cells[startGridRowsIndex + k, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[startGridRowsIndex + k, 1].Value = k;
                workSheet.Cells[startGridRowsIndex + k, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[startGridRowsIndex + k, 2].Value = it.UserId;
                workSheet.Cells[startGridRowsIndex + k, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[startGridRowsIndex + k, 3].Value = it.FullName;
                workSheet.Cells[startGridRowsIndex + k, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[startGridRowsIndex + k, 4].Value = it.Email;
                var cell5 = workSheet.Cells[startGridRowsIndex + k, 5];
                cell5.Style.WrapText = true;
                cell5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell5.RichText.Add($"ORId: {it.ORId} \r\n Trạng thái: {EnumExtension.GetDescription((ORLogStateEnum)it.State)}");

                var cell6 = workSheet.Cells[startGridRowsIndex + k, 6];
                cell6.Style.WrapText = true;
                cell6.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell6.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                cell6.RichText.Add(htmlContent);
                k++;
            }
            workSheet.Column(5).Width = 50;
            workSheet.Column(6).Width = 100;
            //workSheet.Cells[6, 1, excelRowCount, maxColumnIndex].AutoFitColumns();

            #endregion format grid header

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;
        }

        private string ReturnContent(string[] trackings, string key, string label)
        {
            var idx = Array.IndexOf(trackings, key);
            if (idx > -1)
            {
                var str = string.Empty;

                if (!string.IsNullOrEmpty(trackings[idx + 1]) && trackings[idx + 1].Equals(":"))
                {
                    if (!string.IsNullOrEmpty(trackings[idx + 1]) && !trackings[idx + 1].Equals(":null,"))
                    {
                        str = trackings[idx + 2];
                    }
                }
                else if (!string.IsNullOrEmpty(trackings[idx + 1]) && trackings[idx + 1].Equals(":null,"))
                {
                    str = string.Empty;
                }
                else
                {
                    str = trackings[idx + 1];
                }

                Regex r = new Regex(@"\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}");
                Match m = r.Match(str);
                if (m.Success)
                {
                    if (!string.IsNullOrEmpty(str))
                    {

                        str = DateTime.Parse(str).Year >= 1900 ? DateTime.Parse(str).ToString("dd/MM/yyyy") : string.Empty;
                    }
                }

                if (str != string.Empty)
                {
                    return $"{label}: {str}\r\n";
                }
            }
            return null;
        }

        #region ORRoom Management
        public ActionResult RoomList(int State = -1, string kw = "", int p = 1, int ps = 0, int roomType = -1, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;

            var listRoomTypes = new List<SelectListItem>();
            var listStates = new List<SelectListItem>();
            kw = HttpUtility.UrlDecode(kw);
            var listORRooms = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;

            #region master data
            var listSites = new List<SelectListItem>();
            if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
            {
                foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                {
                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion
            listRoomTypes =
                EnumExtension.ToListOfValueAndDesc<RoomTypeEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listRoomTypes.Insert(0, AdminGlobal.DefaultValue.DefaultStateSelectListItem);

            listStates =
                EnumExtension.ToListOfValueAndDesc<DisPlayEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listStates.Insert(0, AdminGlobal.DefaultValue.DefaultStateSelectListItem);

            var dataRoom = _orService.SearchORRoom(siteId, kw, sourceClientId, roomType, State.ToString(), true, p, ps);
            var model = new SearchORRoomModel()
            {
                listData = dataRoom.Data,
                TotalCount = dataRoom.TotalRows,
                listSites = listSites,
                PageCount = ps,
                PageNumber = p,
                listRoomTypes = listRoomTypes,
                listStates = listStates,
                State = State,
                kw = kw,
                siteId = siteId,
                sourceClientId = sourceClientId
            };
            return View(model);
        }
        /// <summary>
        /// Get rooms via surgery type
        /// </summary>
        /// <param name="surgerytype"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRoomsViaSurgeryType(int surgerytype)
        {
            var currentLoc = CurrentUserId;
            var listORRooms = _orService.GetDataWithMemCache<ORCaching, List<ORRoomContract>>(_orService, ConfigUrl.PreFixMemmoryCache, "GetListRoom",
                         new object[] { CurrentLoc?.NameEN, string.Empty, (int)SourceClientEnum.Oh, -1, "1", true }, CacheTimeout.Medium);
            // var listORRooms = _orService.GetListRoom(orProgress.HospitalCode, 0, string.Empty);
            if (listORRooms != null && listORRooms.Any())
            {
                if (surgerytype == 2)
                {
                    listORRooms = listORRooms?.Where(x => x.TypeRoom == (int)RoomTypeEnum.Emergency)?.ToList();
                }
                else
                {
                    listORRooms = listORRooms?.Where(x => x.TypeRoom != (int)RoomTypeEnum.Emergency && x.TypeRoom != (int)RoomTypeEnum.Approve4Pay)?.ToList();
                }
            }
            return Json(listORRooms, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CUDORRoom(int Id = 0, string siteId = "")
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;

            ORRoomModel model;
            var listLocation = _locationApi.Get();

            #region master data site
            var listSites = new List<SelectListItem>();
            if (listLocation != null && listLocation.IsNotNullAndNotEmpty() && listLocation.Where(l => l.LevelNo > 1 && !l.IsDeleted).ToList().Any())
            {
                foreach (var item in listLocation.Where(l => l.LevelNo > 1 && !l.NameEN.Equals("HTC")))
                {

                    listSites.Add(new SelectListItem()
                    {
                        Text = item.NameVN,
                        Value = item.NameEN
                    });
                }
            }
            if (!listSites.Any()) return RedirectToAction("Login", "Authen");

            if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
            {
                siteId = listSites.FirstOrDefault().Value;
            }
            var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
            if (site == null) return RedirectToAction("Login", "Authen");
            #endregion

            #region init data model
            var listRoomTypes =
                EnumExtension.ToListOfValueAndDesc<RoomTypeEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listRoomTypes.Insert(0, AdminGlobal.DefaultValue.RequireSelectListItem);
            if (Id > 0)
            {
                var response = _orService.GetORRoom(Id);
                if (response == null)
                {
                    model = new ORRoomModel()
                    {
                        Id = Id
                    };
                    ViewBag.ErrorMessage = MessageResource.Shared_SystemErrorMessage;
                }
                else
                {
                    model = new ORRoomModel()
                    {
                        Id = response.Id,
                        Name = response.Name,
                        Description = response.Description,
                        IsDeleted = response.IsDeleted,
                        Sorting = response.Sorting,
                        CreatedBy = response.CreatedBy,
                        CreatedByName = response.CreatedByName,
                        CreatedDate = response.CreatedDate,
                        UpdatedBy = response.UpdatedBy,
                        UpdatedByName = response.UpdatedByName,
                        UpdatedDate = response.UpdatedDate,
                        Id_Mapping = response.Id_Mapping,
                        Name_Mapping = response.Name_Mapping,
                        IsDisplay = response.IsDisplay,
                        HospitalCode = response.HospitalCode,
                        SourceClientId = response.SourceClientId,
                        TypeRoom = response.TypeRoom,
                        listTypeRooms = listRoomTypes
                    };
                }
            }
            else
            {
                model = new ORRoomModel()
                {
                    Id = 0,
                    Name = "",
                    listTypeRooms = listRoomTypes
                };
            }
            #endregion
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CUDORRoom(ORRoomModel model)
        {
            //set default current location
            if (ModelState.IsValid)
            {
                if (model.TypeRoom == 0)
                {
                    return Json(new ActionMessage(-1, "Vui lòng chọn Type Room!"));
                }
                var data = new ORRoomContract
                {
                    Id = model.Id,
                    HospitalCode = model.Id > 0 ? model.HospitalCode : memberExtendedInfo.CurrentLocaltion.NameEN,
                    Name = model.Name,
                    Description = model.Description,
                    Id_Mapping = model.Id_Mapping,
                    SourceClientId = (int)SourceClientEnum.Oh,
                    TypeRoom = model.TypeRoom,
                    Sorting = model.Sorting,
                    CreatedBy = (model.Id > 0) ? model.CreatedBy : CurrentUserId,
                    CreatedDate = model.CreatedDate,
                    IsDeleted = model.IsDeleted,
                    UpdatedBy = CurrentUserId,
                    UpdatedDate = model.UpdatedDate,
                    IsDisplay = model.IsDisplay == "on" ? "1" : "0",
                    Name_Mapping = model.Name_Mapping
                };
                ActionMessage actionMessage;
                #region Check access denied on location
                if (memberExtendedInfo.CurrentLocaltion.NameEN != data.HospitalCode)
                {
                    actionMessage = new ActionMessage(new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied));
                    return Json(actionMessage);
                }
                #endregion .Check access denied on location
                var response = _orService.CUDRoom(data, CurrentUserId);
                if (response == null)
                {
                    response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                else if (response.Id > 0)
                {
                    response.SystemMessage = "Shared_SaveSuccess";
                    var msg = new ActionMessage(1, response.SystemMessage);
                    TempData[AdminGlobal.ActionMessage] = msg;
                    #region remove cache
                    BaseCaching.Instant.ClearCachingContainKey("GetListRoom");
                    #endregion
                    return Json(msg);
                }
                else if (response.Id == (int)ResponseCode.AdminRole_Accessdenied)
                {
                    response.SystemMessage = "AdminRole_Accessdenied";
                }
                else
                {
                    response.SystemMessage = "CMS_GetRuntimeErrorMsg";
                }
                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                return Json(response);
            }
            var cudMsg = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
            return Json(new ActionMessage(cudMsg));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult HideOrShowRoom(int id, string actiontype)
        {
            CUDReturnMessage response = null;
            response = _orService.HideOrShowRoom(CurrentUserId, id, actiontype.ToUpper());
            if (response == null)
                return Json(new ActionMessage(new CUDReturnMessage(ResponseCode.Error)));

            var actionMessage = new ActionMessage(response);

            if (response.Id == (int)ResponseCode.OR_Room_SuccessHide || response.Id == (int)ResponseCode.OR_Room_SuccessShow)
            {
                actionMessage.ID = 1;
                TempData[AdminGlobal.ActionMessage] = actionMessage;
            }

            return Json(actionMessage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteRoom(int id)
        {
            var response = _orService.RoomDelete(CurrentUserId, id);

            if (response == null)
                return Json(new ActionMessage(new CUDReturnMessage(ResponseCode.Error)));

            var actionMessage = new ActionMessage(response);

            if (response.Id == (int)ResponseCode.OR_Room_SuccessDelete)
            {
                actionMessage.ID = 1;
                TempData[AdminGlobal.ActionMessage] = actionMessage;
            }

            return Json(actionMessage);
        }
        #endregion .ORRoom Management
        [HttpGet]
        public JsonResult GetDiagnosisViaChargeId(string chargeid)
        {
            string Diagnosis = string.Empty;
            if (!string.IsNullOrEmpty(chargeid))
            {
                Diagnosis = _syncData.GetDiagnosisByCharge(chargeid);
            }
            return Json(Diagnosis, JsonRequestBehavior.AllowGet);
        }
        #region Print Receipt surgery
        [HttpGet]
        public JsonResult GetReceiptSurgeryInfo(int pgid)
        {
            var entity = _orService.GetORAnesthProgress(pgid.ToString(), (int)TypeSearchEnum.Id);
            if (entity != null)
            {
                #region Get Charge Unit Price
                //set mặc định bằng tiền phòng 5* Vip=3.500.000
                double? advanceAmount = 3500000;
                if (!string.IsNullOrEmpty(entity.ChargeDetailId))
                {
                    var dataCharges = _syncData.GetCharges(entity.ChargeDetailId);
                    if (dataCharges?.Count > 0)
                    {
                        advanceAmount += dataCharges[0].UnitPrice;
                    }
                }
                #endregion
                #region Get Diagnosis from EMR
                string Diagnosis = string.Empty;
                if (!string.IsNullOrEmpty(entity.ChargeDetailId))
                {
                    Diagnosis = _syncData.GetDiagnosisByCharge(entity.ChargeDetailId);
                }
                #endregion
                System.Globalization.CultureInfo vnFormat = new System.Globalization.CultureInfo("vi-VN");
                var response = new
                {
                    entity.Id,
                    entity.PId,
                    entity.HoTen,
                    Sex = EnumExtension.GetDescription((SexTypeEnum)entity.Sex),
                    Dob = entity.NgaySinh != null ? entity.NgaySinh.Value.ToString("dd/MM/yyyy") : "",
                    entity.HpServiceName,
                    Diagnosis = !string.IsNullOrEmpty(entity.Diagnosis) ? entity.Diagnosis : Diagnosis,
                    NamePTVMain = entity.NamePTVMain.ToFullName(),
                    ShowdtOperation = entity.dtOperation.HasValue ? entity.dtOperation.Value.ToVEShortDate() : string.Empty,
                    ShowdtStart = entity.dtStart.HasValue ? entity.dtStart.Value.AddMinutes(-entity.TimeAnesth.Value).ToVEShortTime() : string.Empty,
                    ShowdtAdmission = entity.dtAdmission.HasValue ? entity.dtAdmission.Value.ToVEShortDate() : string.Empty,
                    ShowTimeAdmission = entity.dtAdmission.HasValue ? entity.dtAdmission.Value.ToVEShortTime() : string.Empty,
                    NoFoodFrom = entity.dtStart.HasValue ? entity.dtStart.Value.AddMinutes(-(entity.TimeAnesth.Value)).AddHours(-6).ToVEShortDateTimeNotSecondV3() : string.Empty,
                    NoDrinkFrom = entity.dtStart.HasValue ? entity.dtStart.Value.AddMinutes(-(entity.TimeAnesth.Value)).AddHours(-2).ToVEShortDateTimeNotSecondV3() : string.Empty,
                    AdvanceAmount = advanceAmount != null ? advanceAmount.Value.ToString("#,##0", vnFormat) + " VND" : string.Empty,
                    entity.RegDescription,
                    AdmissionWard = !string.IsNullOrEmpty(entity.AdmissionWard) ? entity.AdmissionWard : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                };
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateReceiptSurgery(int pgid, string key, string value)
        {
            CUDReturnMessage updateResult = _orService.UpdateReceiptSurgeryInfo(pgid, key, value.Trim(), CurrentUserId);

            if (updateResult == null)
                return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Fail, message = MessageResource.CMS_SystemError_Message });

            if (updateResult.Id == (int)ResponseCode.Successed)
                return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Success, message = MessageResource.CMS_UpdateSuccessed });

            return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Fail, message = MessageResource.CMS_UpdateFailed });
        }
        #endregion
        #region Function helper
        private ActionResult ExportORReceiptSurgeryList(List<ORAnesthProgressContract> datatable, string filename, DateTime fromDate, DateTime toDate)
        {
            datatable = datatable.OrderBy(s => s.ORRoomId).ThenBy(p => p.Sorting).ThenBy(d => d.State).ToList();
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Receipt surgery list");
            int maxColumnIndex = 16;
            int startGridRowsIndex = 6;
            int excelRowCount = datatable.Count();

            #region format header

            //  Row report name
            using (var reportHeader = workSheet.Cells[1, 1, 1, maxColumnIndex])
            {
                reportHeader.Merge = true;
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Style.Font.Bold = true;
                reportHeader.Style.Font.Size = 16;
                reportHeader.Value = "LIST RECEIPT SURGERY (DANH SÁCH HẸN MỔ)";
            }

            //Ngày xuất từ
            using (var reportHeader = workSheet.Cells[2, 1, 2, 2])
            {
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Style.Font.Bold = true;
                reportHeader.Merge = true;
                reportHeader.Value = "FromDate (Từ ngày)";
            }

            using (var reportHeader = workSheet.Cells[2, 3, 2, 4])
            {
                reportHeader.Merge = true;
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Value = fromDate.ToString("dd/MM/yyyy");
            }
            //Ngày xuất đến
            using (var reportHeader = workSheet.Cells[3, 1, 3, 2])
            {
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Style.Font.Bold = true;
                reportHeader.Merge = true;
                reportHeader.Value = "Todate (Đến ngày)";
            }
            using (var reportHeader = workSheet.Cells[3, 3, 3, 4])
            {
                reportHeader.Merge = true;
                reportHeader.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                reportHeader.Value = toDate.ToString("dd/MM/yyyy");
            }


            #endregion format header

            #region format grid header

            using (var header = workSheet.Cells[startGridRowsIndex, 1, startGridRowsIndex, maxColumnIndex])
            {
                header.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                header.Style.Font.Bold = true;
                header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                var headerBgColor = System.Drawing.ColorTranslator.FromHtml("#dbdbdb");
                header.Style.Fill.BackgroundColor.SetColor(headerBgColor);
                header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                header.Style.WrapText = true;
            }
            // Border cho lưới data
            using (var header = workSheet.Cells[startGridRowsIndex, 1, startGridRowsIndex + excelRowCount, maxColumnIndex])
            {
                header.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                header.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                header.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                header.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                header.Style.Border.Top.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
                header.Style.Border.Left.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
                header.Style.Border.Right.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
                header.Style.Border.Bottom.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#9b9b9b"));
            }

            //  Bind grid header name
            workSheet.Cells[startGridRowsIndex, 1].Value = "STT";
            workSheet.Cells[startGridRowsIndex, 2].Value = "Operation Room(Phòng mổ)";
            workSheet.Cells[startGridRowsIndex, 3].Value = "Start time(Thời gian bắt đầu)";
            workSheet.Cells[startGridRowsIndex, 4].Value = "End time(Thời gian kết thúc)";
            workSheet.Cells[startGridRowsIndex, 5].Value = "PID (Mã bệnh nhân)";
            workSheet.Cells[startGridRowsIndex, 6].Value = "Patient Name(Tên bệnh nhân)";
            workSheet.Cells[startGridRowsIndex, 7].Value = "Sex(Giới tính)";
            workSheet.Cells[startGridRowsIndex, 8].Value = "Age(Tuổi)";
            workSheet.Cells[startGridRowsIndex, 9].Value = "Thời gian nhập viện dự kiến";
            workSheet.Cells[startGridRowsIndex, 10].Value = "Chỉ định dịch vụ";
            workSheet.Cells[startGridRowsIndex, 11].Value = "Chẩn đoán";
            workSheet.Cells[startGridRowsIndex, 12].Value = "Địa điểm nhập viện";
            workSheet.Cells[startGridRowsIndex, 13].Value = "Nhịn ăn từ";
            workSheet.Cells[startGridRowsIndex, 14].Value = "Nhịn uống từ";
            //workSheet.Cells[startGridRowsIndex, 15].Value = "Tạm ứng";
            workSheet.Cells[startGridRowsIndex, 15].Value = "Bác sĩ thực hiện";
            workSheet.Cells[startGridRowsIndex, 16].Value = "Ghi chú";
            //  Bind grid data
            int k = 1;
            foreach (var it in datatable)
            {

                workSheet.Cells[startGridRowsIndex + k, 1].Value = k;
                workSheet.Cells[startGridRowsIndex + k, 2].Value = it.ORRoomName;
                workSheet.Cells[startGridRowsIndex + k, 3].Value = (it.dtStart ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat);
                workSheet.Cells[startGridRowsIndex + k, 4].Value = (it.dtEnd ?? DateTime.Now).ToString(StringHelper.DateTimeNoSecondFormat);
                workSheet.Cells[startGridRowsIndex + k, 5].Value = it.PId;
                workSheet.Cells[startGridRowsIndex + k, 6].Value = it.HoTen;
                workSheet.Cells[startGridRowsIndex + k, 7].Value = it.Sex == 1 ? "Nam" : it.Sex == 2 ? "Nữ" : "Chư xác định";
                workSheet.Cells[startGridRowsIndex + k, 8].Value = it.Ages;
                workSheet.Cells[startGridRowsIndex + k, 9].Value = it.dtAdmission;
                workSheet.Cells[startGridRowsIndex + k, 10].Value = it.HpServiceName;
                workSheet.Cells[startGridRowsIndex + k, 11].Value = it.Diagnosis;
                workSheet.Cells[startGridRowsIndex + k, 12].Value = it.AdmissionWard;
                workSheet.Cells[startGridRowsIndex + k, 13].Value = it.dtStart.HasValue ? it.dtStart.Value.AddMinutes(-(it.TimeAnesth.Value)).AddHours(-6).ToVEShortDateTimeNotSecondV3() : string.Empty;
                workSheet.Cells[startGridRowsIndex + k, 14].Value = it.dtStart.HasValue ? it.dtStart.Value.AddMinutes(-(it.TimeAnesth.Value)).AddHours(-2).ToVEShortDateTimeNotSecondV3() : string.Empty;
                //workSheet.Cells[startGridRowsIndex + k, 15].Value = string.Empty;
                workSheet.Cells[startGridRowsIndex + k, 15].Value = it.NamePTVMain.ToFullName();
                //workSheet.Cells[startGridRowsIndex + k, 12].Value = EnumExtension.GetDescription((ORLogStateEnum)(it.State));
                workSheet.Cells[startGridRowsIndex + k, 16].Value = it.RegDescription;
                k++;
            }
            workSheet.Cells[6, 1, excelRowCount, maxColumnIndex].AutoFitColumns();

            #endregion format grid header

            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            return null;
        }
        #endregion
        #region linhht tìm kiếm bệnh nhân
        public ActionResult GetPatientInfo(string kw = "", string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            try
            {
                //Check role access on location
                siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;
                if (siteId != memberExtendedInfo.CurrentLocaltion.NameEN)
                {
                    //Redirect to site access denied
                    return Json(new ResponseBase()
                    {
                        Type = "ERR",
                        Message = "Bạn chưa thuộc site nào"
                    });
                    //return PartialView("_PatientInfo");
                }
                //show list site allows
                #region master data
                var listSites = new List<SelectListItem>();
                if (memberExtendedInfo != null && memberExtendedInfo.Locations.IsNotNullAndNotEmpty() && memberExtendedInfo.Locations.Where(l => l.LevelNo > 1).ToList().Any())
                {
                    foreach (var item in memberExtendedInfo.Locations.Where(l => l.LevelNo > 1))
                    {
                        listSites.Add(new SelectListItem()
                        {
                            Text = item.NameVN,
                            Value = item.NameEN
                        });
                    }
                }
                if (!listSites.Any())
                {
                    //return PartialView("_PatientInfo");
                    return Json(new ResponseBase()
                    {
                        Type = "ERR",
                        Message = "Bạn không có quyền truy cập dữ liệu"
                    });
                }
                //if (listSites.Any() && !listSites.Any(x => x.Value.Equals(siteId)))
                //{
                //    siteId = listSites.FirstOrDefault().Value;
                //}
                //var site = listSites.FirstOrDefault(s => s.Value.Equals(siteId));
                // collect data
                #endregion

                //string LastSiteName = string.Empty;
                //string LastVistDate = string.Empty;
                kw = HttpUtility.UrlDecode(kw).Trim();
                BenhNhanOR data = null;
                if (!string.IsNullOrEmpty(kw))
                {
                    data = _syncData.GetBenhNhanORInfo(kw, sourceClientId);
                    if (data == null)
                    {
                        //Tìm kiếm theo thông tin Patient
                        data = _syncData.GetPatientInfo(kw);
                    }
                }
                //if (data != null && data.VisitSyncs != null && data.VisitSyncs.VisitSync != null && data.VisitSyncs.VisitSync.Any())
                //{
                //    var lastSiteVisit = data.VisitSyncs.VisitSync.OrderByDescending(c => c.NGAY_VAO).FirstOrDefault();
                //    LastVistDate = lastSiteVisit.NGAY_VAO != null ? lastSiteVisit.NGAY_VAO.ToString("dd/MM/yyyy") : string.Empty;
                //    if (lastSiteVisit != null && !string.IsNullOrEmpty(lastSiteVisit.MA_BENH_VIEN))
                //    {
                //        var lastSite = _orService.GetHospitalSite(lastSiteVisit.MA_BENH_VIEN);
                //        LastSiteName = lastSite != null ? lastSite.SiteNameFull : string.Empty;
                //    }
                //}

                #region Insert new service
                if (data != null && data.ListServices != null)
                {
                    //log.Debug(string.Format("OR.GetBenhNhanORInfo ListService return: {0}", JsonConvert.SerializeObject(data.ListServices)));
                    foreach (var model in data.ListServices.Services)
                    {
                        var response = _orService.InsertNewService(model, CurrentUserId);
                    }
                    var newList = _orService.RemoveExistingServices(data.ListServices.Services, siteId);
                    data.ListServices.Services = newList;
                }
                #endregion

                //log.Debug(string.Format("OR.SearchPatientOR return: {0}", JsonConvert.SerializeObject(data)));
                //return PartialView("_PatientInfo", data);
                return Json(new ResponseBase()
                {
                    Type = "success",
                    Message = "success",
                    data = data
                });

            }
            catch (Exception ex)
            {
                //log.Debug(ex);
                //return PartialView("_PatientInfo");
                return Json(new ResponseBase()
                {
                    Type = "ERR",
                    Message = "Có lỗi xảy ra"
                });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePatientInfo(PatientReq patientReq)
        {
            if (!ModelState.IsValid) return Json(new ResponseBase()
            {
                Type = "ERR",
                Message = "Không cập nhật được thông tin bệnh nhân"
            });
            try
            {
                var patient = new ReportPatient()
                {
                    Id = patientReq.pMa,
                    Address = patientReq.pAddress,
                    Age = patientReq.pAge,
                    National = patientReq.pNational,
                    Sex = patientReq.pSex,
                    HoTen = patientReq.pName,
                    Phone = patientReq.pPhone,
                    NgaySinh = DateTime.Parse(patientReq.pBirthday)
                };
                var isUpdate = await _orService.UpdatePatientInfo(patient, CurrentUserId);
                if (isUpdate)
                    return Json(new ResponseBase()
                    {
                        Type = "success",
                        Message = "success"
                    });

            }
            catch (Exception ex)
            {
                //log.Debug(ex);
                //return PartialView("_PatientInfo");
                return Json(new ResponseBase()
                {
                    Type = "ERR",
                    Message = "Có lỗi xảy ra"
                });
            }
            return Json(new ResponseBase()
            {
                Type = "ERR",
                Message = "Cập nhật không thành công"
            });
        }
        #endregion
    }
}