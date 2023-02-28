using Admin.Helper;
using Admin.Models.OR;
using Admin.Models.QueuePatients;
using Admin.Models.Shared;
using Admin.Resource;
using Admin.Shared;
using Caching;
using Caching.Core;
using Caching.OR;
using Contract.Enum;
using Contract.OR;
using Contract.QueuePatient;
using Contract.Shared;
using DataAccess.Models;
using Microsoft.Security.Application;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Controllers
{
    [CheckUserCaching]
    [AuthorizeEhos]
    public class QueuePatientController : BaseController
    {
        private readonly IQueuePatientCaching _queuePatientCaching;
        private readonly IMasterCaching masterCaching;
        private readonly ORCaching _orService;
        private readonly ILocationCaching _locationApi;
        public QueuePatientController(IAuthenCaching authenCaching, ISystemSettingCaching systemSettingApi, IQueuePatientCaching queuePatientCaching, IMasterCaching masterCaching, ORCaching orService, ILocationCaching locationApi) : base(authenCaching, systemSettingApi)
        {
            this.masterCaching = masterCaching;
            _orService = orService;
            _queuePatientCaching = queuePatientCaching;
            _locationApi = locationApi;
        }
        #region Block time for OR
        public ActionResult ListTimeBlock()
        {
            var listMs = masterCaching.GetListBlocktime().ToList();
            ViewBag.ListMs = listMs;
            return View(listMs);
        } 

        public ActionResult CreateUpdateBlocktime(int id = 0)
        {
            var model = new Blocktime_view();

            if (id > 0)
            {
                var lstBlocktime = masterCaching.GetListBlocktime();
                if (lstBlocktime != null && lstBlocktime.Exists(m => m.id == id))
                {
                    model.id = lstBlocktime.Single(m => m.id == id).id;
                    model.MaDV = lstBlocktime.Single(m => m.id == id).MaDV;
                    model.TenDv = lstBlocktime.Single(m => m.id == id).TenDv;
                    model.AnesthesiaTime = lstBlocktime.Single(m => m.id == id).AnesthesiaTime;
                    model.CleaningTime = lstBlocktime.Single(m => m.id == id).CleaningTime;
                    model.PreparationTime = 0;
                    model.OtherTime = lstBlocktime.Single(m => m.id == id).OtherTime;
                    model.Ehos_Iddv = lstBlocktime.Single(m => m.id == id).Ehos_Iddv;
                }
            }
            else
            {
                model.id = 0;
                model.MaDV = "";
                model.TenDv = "";
                model.Ehos_Iddv = 0;
                model.AnesthesiaTime = 0;
                model.CleaningTime = 0;
                model.PreparationTime = 0;
                model.OtherTime = 0;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateBlocktime(Blocktime_view model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new Blocktime_view
                {
                    id = model.id,
                    MaDV = Sanitizer.GetSafeHtmlFragment(model.MaDV),
                    TenDv = Sanitizer.GetSafeHtmlFragment(model.TenDv),
                    AnesthesiaTime = model.AnesthesiaTime,
                    CleaningTime = model.CleaningTime,
                    PreparationTime = model.PreparationTime,
                    OtherTime = model.OtherTime,
                    Ehos_Iddv = model.Ehos_Iddv
                };

                var response = masterCaching.InsertUpdateBlocktime(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.Successed)
                    {
                        response.SystemMessage = "TypeDoctor_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Successed)
                    {
                        response.SystemMessage = "TypeDoctor_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Successed)
                    {
                        response.SystemMessage = "TypeDoctor_DuplicateCode";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else
                    {
                        response.SystemMessage = "CMS_GetRuntimeErrorMsg";
                    }
                }
                else
                {
                    response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                return Json(response);
            }

            var cudMsg = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
            return Json(new ActionMessage(cudMsg));
        }
        #endregion
        public ActionResult SearchPatients(QueuePatientSearchParam param)
        {
            ViewBag.SearchParam = param;
            if (param.Keyword == null)
                param.Keyword = string.Empty;
            if (param.export)
            {
                param.p = 1;
                param.PageSize = Int16.MaxValue;
                var searchResult = _queuePatientCaching.SearchPatients(param);
                if(searchResult!=null && searchResult.Data!=null && searchResult.Data.Any())
                {
                    return ExportPatientQueue(searchResult.Data,
                "danh-sach-benh-nhan" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"), (DateTime)param.FromDate, (DateTime)param.ToDate);
                }
                return null;
            }else
            {
                ViewBag.States = _queuePatientCaching.GetStates();
                ViewBag.Rooms = _queuePatientCaching.GetRooms();
                var searchResult = _queuePatientCaching.SearchPatients(param);

                return View(searchResult);
            }
            
        }
        #region private method
        private ActionResult ExportPatientQueue(List<Patient> datatable,
        string filename, DateTime fromDate, DateTime toDate)
        {

            datatable = datatable.OrderBy(s => s.RoomId).ThenBy(p => p.Sorting).ThenBy(d => d.State).ToList();
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
                reportHeader.Value ="Todate (Đến ngày)";
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
            workSheet.Cells[startGridRowsIndex, 2].Value = "Room number(Phòng mổ)";
            workSheet.Cells[startGridRowsIndex, 3].Value = "Start time(Thời gian bắt đầu)";
            workSheet.Cells[startGridRowsIndex, 4].Value = "End time(Thời gian kết thúc)";
            workSheet.Cells[startGridRowsIndex, 5].Value = "PID (Mã bệnh nhân)";
            workSheet.Cells[startGridRowsIndex, 6].Value = "Patient Name(Tên bệnh nhân)";
            workSheet.Cells[startGridRowsIndex, 7].Value = "Sex(Giới tính)";
            workSheet.Cells[startGridRowsIndex, 8].Value = "Age(Tuổi)";
            workSheet.Cells[startGridRowsIndex, 9].Value = "Ward(Khu vực)";
            workSheet.Cells[startGridRowsIndex, 10].Value = "Surgeon/Anesth (Ekip phẩu thuật)";
            workSheet.Cells[startGridRowsIndex, 11].Value = "Type of anesth(Loại gây mê)";
            workSheet.Cells[startGridRowsIndex, 12].Value = "Procedure(Tên phẩu thuật)";
            workSheet.Cells[startGridRowsIndex, 13].Value = "Readline(Trạng thái)";
            workSheet.Cells[startGridRowsIndex, 14].Value = "Remarks(Ghi nhận)";
            //  Bind grid data
            int k = 1;
            foreach (var it in datatable)
            {
                workSheet.Cells[startGridRowsIndex + k, 1].Value = k;
                workSheet.Cells[startGridRowsIndex + k, 2].Value = it.RoomHospital.Name;
                workSheet.Cells[startGridRowsIndex + k, 3].Value = it.StartDate.ToString(StringHelper.DateTimeNoSecondFormat);
                workSheet.Cells[startGridRowsIndex + k, 4].Value = it.EndDate.ToString(StringHelper.DateTimeNoSecondFormat);
                workSheet.Cells[startGridRowsIndex + k, 5].Value = it.PId ;
                workSheet.Cells[startGridRowsIndex + k, 6].Value = it.PatientName;
                workSheet.Cells[startGridRowsIndex + k, 7].Value = it.Sex==1?"Nam": it.Sex == 2 ?"Nữ":"Chư xác định";
                workSheet.Cells[startGridRowsIndex + k, 8].Value = it.Age;
                workSheet.Cells[startGridRowsIndex + k, 9].Value = it.AreaName;
                workSheet.Cells[startGridRowsIndex + k, 10].Value = it.EkipName+"/"+it.EKipAnesth;
                workSheet.Cells[startGridRowsIndex + k, 11].Value = it.TypeName;
                workSheet.Cells[startGridRowsIndex + k, 12].Value = it.ServiceName;
                workSheet.Cells[startGridRowsIndex + k, 13].Value = EnumExtension.GetDescription((PatientStateEnum)it.State);
                workSheet.Cells[startGridRowsIndex + k, 14].Value = it.Description;
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

        public ActionResult InsertUpdatePatient(int patientId = 0)
        {
            string siteId = memberExtendedInfo.Locations.FirstOrDefault()?.NameEN;

            ViewBag.States = _queuePatientCaching.GetStates();
            ViewBag.Rooms = _queuePatientCaching.GetRooms();
            var listMs = masterCaching.GetListBlocktime().ToList();
            if (listMs == null)
                listMs = new List<Blocktime_view>();
            ViewBag.ListBlockTime = listMs;
            //ViewBag.ListHpServiceContract = _orService.GetDataWithMemCache<ORCaching, List<HpServiceContract>>(_orService, ConfigUrl.PreFixMemmoryCache, "GetListHpServices",
            //           new object[] { siteId, 0 }, CacheTimeout.Medium) ?? new List<HpServiceContract>();
            
            ViewBag.ListHpServiceContract = _orService.GetListHpServices(siteId, (int)SourceClientEnum.Ehos);
            if (patientId == 0)
                return View(new Patient());

            var dbPatient = _queuePatientCaching.GetPatientById(patientId);
            return View(dbPatient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult InsertUpdatePatient(Patient patient)
        {
            ViewBag.States = _queuePatientCaching.GetStates();
            ViewBag.Rooms = _queuePatientCaching.GetRooms();
            var listMs = masterCaching.GetListBlocktime().ToList();
            if (listMs == null)
                listMs = new List<Blocktime_view>();
            ViewBag.ListBlockTime = listMs;
            #region fix datetime
            //patient.StartDate = Convert.ToDateTime(patient.StartDate.ToString(), CultureInfo.GetCultureInfo("vi-VN").DateTimeFormat);
            //patient.EndDate = Convert.ToDateTime(patient.EndDate.ToString(), CultureInfo.GetCultureInfo("vi-VN").DateTimeFormat);
            #endregion fix datetime
            if (patient.StartDate < new DateTime(1970, 1, 1, 0, 0, 0) || patient.EndDate < new DateTime(1970, 1, 1, 0, 0, 0))
            {
                ViewBag.SaveFailMessage = "Thời gian không hợp lệ!";
                return View(patient);
            }

            string siteId = memberExtendedInfo.Locations.FirstOrDefault()?.NameEN;
            ViewBag.ListHpServiceContract = _orService.GetListHpServices(siteId, (int)SourceClientEnum.Ehos);

            #region Valid data
            //if (patient.ServiceName.Equals("-1"))
            if (string.IsNullOrEmpty(patient.ServiceName))
            {
                ViewBag.SaveFailMessage = "Vui lòng lựa chọn dịch vụ!";
                return View(patient);
            }
            patient.ServiceName = Sanitizer.GetSafeHtmlFragment(patient.ServiceName);
            #endregion Valid data
            patient.CreatedBy = CurrentUserId;
            patient.CreatedDate = DateTime.Now;
            patient.UpdatedDate = DateTime.Now;
            patient.UpdatedBy = CurrentUserId;
            Patient newPatient = _queuePatientCaching.InsertUpdatePatient(patient);
            ViewBag.SaveSuccessMessage = "Lưu thành công";
            return View(newPatient);
        }

        public ActionResult ViewPatientsPublic(DateTime? FromDate, DateTime? ToDate, int State = 0, string kw = "", int p = 1, int ps = 0, int r = 0)
        {
            var listStates = new List<SelectListItem>();
            var listCheckListTypes = new List<SelectListItem>();
            var listSystems = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (!FromDate.HasValue) FromDate = DateTime.Now;
            FromDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() +" 00:00:00");
            if (!ToDate.HasValue) ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
            #region state
            listStates =
                EnumExtension.ToListOfValueAndDesc<PatientStatePublicEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
            #endregion state


            var data = _queuePatientCaching.ViewPatientsPublic(new QueuePatientSearchParam()
            {
                Keyword = kw,
                StateId = State,
                FromDate = FromDate ?? DateTime.Now,
                ToDate = ToDate ?? DateTime.Now,
                RoomId = r,
                p = p,
                PageSize = ps

            });
            var model = new PatientModel()
            {
                listData = data.Data,
                TotalCount = data.TotalRecords,
                PageCount = ps,
                PageNumber = p,
                listStates = listStates,
                State = State,
                kw = kw,
                FromDate = FromDate ?? DateTime.Now,
                ToDate = ToDate ?? DateTime.Now
            };
            return View(model);
        }
        public ActionResult ViewPublic(DateTime? FromDate, DateTime? ToDate, int State = 0, string kw = "", int p = 1, int ps = 0, int r = 0)
        {
            var listStates = new List<SelectListItem>();
            var listCheckListTypes = new List<SelectListItem>();
            var listSystems = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (!FromDate.HasValue) FromDate = DateTime.Now;
            FromDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
            if (!ToDate.HasValue) ToDate = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
            #region state
            listStates =
                EnumExtension.ToListOfValueAndDesc<PatientStatePublicEnum>().Select(e => new SelectListItem
                {
                    Text = e.Description,
                    Value = e.Value.ToString()
                }).ToList();
            listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
            #endregion state


            var data = _queuePatientCaching.ViewPatientsPublic(new QueuePatientSearchParam()
            {
                Keyword = kw,
                StateId = State,
                FromDate = FromDate ?? DateTime.Now,
                ToDate = ToDate ?? DateTime.Now,
                RoomId = r,
                p = p,
                PageSize = ps

            });
            var model = new PatientModel()
            {
                listData = data.Data,
                TotalCount = data.TotalRecords,
                PageCount = ps,
                PageNumber = p,
                listStates = listStates,
                State = State,
                kw = kw,
                FromDate = FromDate ?? DateTime.Now,
                ToDate = ToDate ?? DateTime.Now
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ViewPatientsPublicByJson(int pageIndex,int totalPage)
        {
            List<PatientContract> listPatient = new List<PatientContract>();
            pageIndex = pageIndex + 1;
            pageIndex = pageIndex > totalPage ? 1 : pageIndex;

            var model = new PatientContractModel()
            {
                listData = listPatient,
                PageCount = AdminConfiguration.Paging_PageSize,
                PageNumber = pageIndex,
                kw = string.Empty
            };
            try
            {
                var data = _queuePatientCaching.ViewPatientsPublic(new QueuePatientSearchParam()
                {
                    Keyword = string.Empty,
                    StateId = 0,
                    FromDate =DateTime.Now,
                    ToDate =  DateTime.Now,
                    RoomId = 0,
                    p = pageIndex,
                    PageSize = AdminConfiguration.Paging_PageSize

                });
                if (data.TotalRecords == 0) return Json(new List<PatientContract>());

                foreach(var item in data.Data)
                {
                    listPatient.Add(new PatientContract()
                    {
                        Id = item.Id,
                        StartDate = item.StartDate.ToString(StringHelper.DateTimeNoSecondFormat),
                        EndDate = item.EndDate.ToString(StringHelper.DateTimeNoSecondFormat),
                        PId = item.PId,
                        Age = item.Age ?? 1,
                        AreaName = item.AreaName,
                        EkipName = string.IsNullOrEmpty(item.EkipName) ? string.Empty : item.EkipName,
                        TypeName = string.IsNullOrEmpty(item.TypeName)?string.Empty:item.TypeName,
                        ServiceName = item.ServiceName,
                        State = item.State,
                        Description = string.IsNullOrEmpty(item.Description)?string.Empty: item.Description,
                        CreatedBy = item.CreatedBy ?? 0,
                        CreatedDate = item.CreatedDate,
                        UpdatedBy = item.UpdatedBy ?? 0,
                        UpdatedDate = item.UpdatedDate,
                        Sorting = item.Sorting,
                        RoomId = item.RoomId ?? 1,
                        PatientName = item.PatientName,
                        IsDeleted = item.IsDeleted,
                        TypeKcbId = item.TypeKcbId ?? 0,
                        Sex = item.Sex ?? 0,
                        IntendTime = item.IntendTime ?? DateTime.Now,
                        LichHen_Id = item.LichHen_Id ?? 0,
                        IsEmergence = item.IsEmergence,
                        EKipAnesth = string.IsNullOrEmpty(item.EKipAnesth) ? string.Empty : item.EKipAnesth,
                        RoomHospitalName = item.RoomHospital.Name,
                        Statename = EnumExtension.GetDescription((PatientStateEnum)item.State),
                        Color = HtmlExtensions.StateBackgroundColorPatient(item.State)
                    });
                }
                model.TotalCount = data.TotalRecords;
                model.listData = listPatient;
                model.listRoom = listPatient.Select(c => c.RoomId).Distinct().ToList();
                model.LastPage = EnumExtension.GetLastPage(model.TotalCount, AdminConfiguration.Paging_PageSize);
                model.HtmlPaging =Helper.StringExtensions.HtmlPaging(model.TotalCount, model.PageNumber, true, model.PageCount).Replace("/ViewPatientsPublicByJson/?p", "/ViewPatientsPublic?p").Replace("/ViewPatientsPublicByJson", "/ViewPatientsPublic");
            }
            catch (Exception ex)
            {
                model.listData = new List<PatientContract>();
                model.TotalCount = 0;
            }
            return Json(model);            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSortting(long Id,int value)
        {
            CUDReturnMessage updateResult = _queuePatientCaching.UpdateSorting(Id, value, CurrentUserId);
            if (updateResult == null)
                return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Fail, message = MessageResource.CMS_SystemError_Message });
            if (updateResult.Id == (int)ResponseCode.Successed)
                return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Success, message = MessageResource.CMS_UpdateSuccessed });

            return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Fail, message = MessageResource.CMS_UpdateFailed });
        }

        [HttpGet]
        public JsonResult GetFullPhongBan()
        {
            var response = _queuePatientCaching.GetORRooms(); 
            return Json(response, JsonRequestBehavior.AllowGet);  
        }
        [HttpGet]
        public JsonResult GetFullPlanByMonth()
        {
            DateTime dtstart = new DateTime();
            DateTime stend = new DateTime();
            DateTime now = DateTime.Now;
            dtstart = new DateTime(now.Year, now.Month, 1);
            dtstart = now.AddDays(-15);
            stend = dtstart.AddMonths(2).AddDays(-1);
            var response = _queuePatientCaching.GetPlanORRooms(dtstart, stend);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetFullPlanByMonthByeource(int idresource)
        {
            DateTime dtstart = new DateTime();
            DateTime stend = new DateTime();
            DateTime now = DateTime.Now;
            dtstart = new DateTime(now.Year, now.Month, 1);
            dtstart = now.AddDays(-15);
            stend = dtstart.AddMonths(1).AddDays(-1);
            //var response = _queuePatientCaching.GetPlanORRooms(dtstart, stend);
            var response = _queuePatientCaching.GetPlanORRoomsByID(dtstart, stend, idresource);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewPlanOR()
        { 
            return View();
        }

        public ActionResult SearchHpServiceByName(string kw = "", int page = 1)
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

            var data = _orService.SearchHpServiceExt(siteId, kw, page, 10, 1); //1: Type of Ehos
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Quan ly dich vu
        public ActionResult SearchHpService(int State = -1, string kw = "", int p = 1, int ps = 0, int HpServiceId = 0, string siteId = "", int sourceClientId = (int)SourceClientEnum.Ehos)
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
                foreach (var item in listLocation.Where(l => l.LevelNo > 1 && l.NameEN.Equals("HTC")))
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
        #endregion
    }
}