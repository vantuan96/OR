using System;
using System.Collections.Generic;
using System.Linq;
using Caching.Core;
using Admin.Helper;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Admin.Models.Master;
using Admin.Models.Shared;
using Admin.Resource;
using Contract.MasterData;
using Contract.Shared;
using Contract.User;
using VG.Common;
using Contract.Enum;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using Microsoft.Security.Application;

namespace Admin.Controllers
{
    [CheckUserCaching]
    public class MasterController : BaseController
    {
        private readonly IMasterCaching masterCaching;
        private readonly ILocationCaching locationApi;
        private readonly IUserMngtCaching userMngtCaching;
        public MasterController(IAuthenCaching authenApi, IMasterCaching masterCaching, ILocationCaching locationApi, ISystemSettingCaching systemSettingApi, IUserMngtCaching userMngtCaching)
            : base(authenApi, systemSettingApi)
        {
            this.masterCaching = masterCaching;
            this.locationApi = locationApi;
            this.userMngtCaching = userMngtCaching;
        }

        #region PnL List
        /// <summary>
        /// Get list
        /// </summary>
        /// <returns></returns>
        public ActionResult ListPnLList()
        {
            var model = masterCaching.GetListPnLList();
            return View(model);
        }

        /// <summary>
        /// Delete PnL List
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePnLList(int id)
        {
            var response = masterCaching.DeletePnLList(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.PnLList_SuccessDelete);
        }

        public ActionResult CreateUpdatePnLList(int id = 0)
        {
            var model = new CreateUpdatePnLListModel();

            #region tạo ddl
            var listPnLListStatus = masterCaching.GetListPnLListStatus();
            #endregion tạo ddl

            if (id > 0)
            {
                var pnlList = masterCaching.GetListPnLList();
                if (pnlList != null && pnlList.Exists(m => m.PnLListId == id))
                {
                    model.Id = pnlList.Single(m => m.PnLListId == id).PnLListId;
                    model.PnLListId = pnlList.Single(m => m.PnLListId == id).PnLListId;
                    model.PnLListCode = pnlList.Single(m => m.PnLListId == id).PnLListCode;
                    model.PnLListName = pnlList.Single(m => m.PnLListId == id).PnLListName;
                    model.Description = pnlList.Single(m => m.PnLListId == id).Description;
                    model.FullAddress = pnlList.Single(m => m.PnLListId == id).FullAddress;
                    model.ListPnLsStatus = listPnLListStatus;
                }
            }
            else
            {
                model.Id = 0;
                model.PnLListId = id;
                model.PnLListCode = "";
                model.PnLListName = "";
                model.Description = "";
                model.FullAddress = "";
                model.ListPnLsStatus = listPnLListStatus;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdatePnLList(CreateUpdatePnLListModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new PnLListContract
                {
                    PnLListId = model.PnLListId,
                    PnLListCode = Sanitizer.GetSafeHtmlFragment(model.PnLListCode),
                    PnLListName = Sanitizer.GetSafeHtmlFragment(model.PnLListName),
                    Description = Sanitizer.GetSafeHtmlFragment(model.Description),
                    FullAddress = Sanitizer.GetSafeHtmlFragment(model.FullAddress),
                    StatusId = model.StatusId,
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdatePnLList(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.PnLList_SuccessCreate)
                    {
                        response.SystemMessage = "PnLList_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PnLList_SuccessUpdate)
                    {
                        response.SystemMessage = "PnLList_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PnLList_DuplicateName)
                    {
                        response.SystemMessage = "PnLList_DuplicateName";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PnLList_DuplicateCode)
                    {
                        response.SystemMessage = "PnLList_DuplicateCode";
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

        #region PnL BU List

        public ActionResult ListPnLBuList()
        {
            var listMs = masterCaching.GetListPnLBuList().ToList();
            ViewBag.ListMs = listMs;
            return View(listMs);
        }

        [ValidateAntiForgeryToken]
        public JsonResult DeletePnLBuList(int id)
        {
            var response = masterCaching.DeletePnLBuList(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.PnLBuList_SuccessDelete);
        }

        public ActionResult CreateUpdatePnLBuList(int id = 0)
        {
            var model = new CreateUpdatePnLBuListModel();

            #region tạo ddl
            var listPnLList = masterCaching.GetListPnLList();
            #endregion tạo ddl

            if (id > 0)
            {
                var pnlBuList = masterCaching.GetListPnLBuList();
                if (pnlBuList != null && pnlBuList.Exists(m => m.PnLBuListId == id))
                {
                    model.PnLBUListCode = pnlBuList.Single(m => m.PnLBuListId == id).PnLBuListCode;
                    model.Id = pnlBuList.Single(m => m.PnLBuListId == id).PnLBuListId;
                    model.PnLBUListId = pnlBuList.Single(m => m.PnLBuListId == id).PnLBuListId;
                    model.Description = pnlBuList.Single(m => m.PnLBuListId == id).Description;
                    model.PnLListId = pnlBuList.Single(m => m.PnLBuListId == id).PnLListId;
                    model.ListPnLList = listPnLList;
                }
            }
            else
            {
                model.Id = 0;
                model.PnLBUListId = id;
                model.PnLBUListCode = "";
                model.Description = "";
                model.PnLListId = 0;
                model.ListPnLList = listPnLList;
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdatePnLBuList(CreateUpdatePnLBuListModel model)
        {
            if (ModelState.IsValid)
            {
                //var msContent = lazyMapper.Value.Map<PnLBuListContract>(model);
                var msContent = new PnLBuListContract
                {
                    PnLListId = model.PnLListId,
                    PnLBuListId = model.PnLBUListId,
                    PnLBuListCode = Sanitizer.GetSafeHtmlFragment(model.PnLBUListCode),
                    Description = Sanitizer.GetSafeHtmlFragment(model.Description),
                    Sort = 1,
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    PnLListCode = ""
                };

                var response = masterCaching.InsertUpdatePnLBuList(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.PnLBuList_SuccessCreate)
                    {
                        response.SystemMessage = "PnLBuList_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PnLBuList_SuccessUpdate)
                    {
                        response.SystemMessage = "PnLBuList_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PnLBuList_DuplicateCode)
                    {
                        response.SystemMessage = "PnLBuList_DuplicateCode";
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
        #endregion microsite management

        #region Danh mục phòng ban bộ phận
        public ActionResult ListDepartmentList()
        {
            var model = masterCaching.GetDepartmentList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDepartmentList(int id)
        {
            var response = masterCaching.DeleteDepartmentList(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.DepartmentList_SuccessDelete);
        }

        public ActionResult CreateUpdateDepartmentList(int id = 0)
        {
            var model = new CreateUpdateDepartmentListModel();

            #region tạo ddl
            var listDepartmentType = masterCaching.GetDepartmentListType();
            #endregion tạo ddl

            if (id > 0)
            {
                var departmentList = masterCaching.GetDepartmentList();
                if (departmentList != null && departmentList.Exists(m => m.DepartmentListId == id))
                {
                    model.ListDepartmentType = listDepartmentType;
                    model.Id = departmentList.Single(m => m.DepartmentListId == id).DepartmentListId;
                    model.DepartmentListId = departmentList.Single(m => m.DepartmentListId == id).DepartmentListId;
                    model.DepartmentListCode = departmentList.Single(m => m.DepartmentListId == id).DepartmentListCode;
                    model.Description = departmentList.Single(m => m.DepartmentListId == id).Description;
                    model.Type = departmentList.Single(m => m.DepartmentListId == id).Type;
                    model.ParentCode = departmentList.Single(m => m.DepartmentListId == id).ParentCode;
                    model.Level = departmentList.Single(m => m.DepartmentListId == id).Level;
                }
            }
            else
            {
                model.Id = 0;
                model.DepartmentListId = id;
                model.DepartmentListCode = "";
                model.Description = "";
                model.ParentCode = "";
                model.Level = 0;
                model.ListDepartmentType = listDepartmentType;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateDepartmentList(CreateUpdateDepartmentListModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new DepartmentListContract
                {
                    DepartmentListId = model.DepartmentListId,
                    DepartmentListCode = Sanitizer.GetSafeHtmlFragment(model.DepartmentListCode),
                    Description = Sanitizer.GetSafeHtmlFragment(model.Description),
                    Type = model.Type,
                    ParentCode = Sanitizer.GetSafeHtmlFragment(model.ParentCode),
                    Sort = model.Sort,
                    Level = model.Level,
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdateDepartmentList(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.DepartmentList_SuccessCreate)
                    {
                        response.SystemMessage = "DepartmentList_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.DepartmentList_SuccessUpdate)
                    {
                        response.SystemMessage = "DepartmentList_SuccessUpdate";
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

        #region Danh mục nhân viên
        public ActionResult ListStaffList()
        {
            var model = masterCaching.GetStaffList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStaffList(int id)
        {
            var response = masterCaching.DeleteStaffList(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.StaffList_SuccessDelete);
        }

        public ActionResult CreateUpdateStaffList(int id = 0)
        {
            var model = new CreateUpdateStaffListModel();

            #region tạo ddl
            var listDepartmentTitle = masterCaching.GetDepartmentTitle();
            var listDepartmentStatus = masterCaching.GetDepartmentStatus();
            var listDepartmentGeneral = masterCaching.GetDepartmentGeneral();
            var listDeList = locationApi.Get();
            var listDepartmentUnit = listDeList.Where(x => x.LayoutTypeId == 1 && !x.IsDeleted).ToList();
            var listDepartmentCentre = listDeList.Where(x => x.LayoutTypeId == 2 && !x.IsDeleted).ToList();
            var listDepartmentDepartment = listDeList.Where(x => x.LayoutTypeId == 3 && !x.IsDeleted).ToList();
            var listDepartmentGroup = listDeList.Where(x => x.LayoutTypeId == 4 && !x.IsDeleted).ToList();

            //ViewBag.StateList = listDepartmentUnit;
            #endregion tạo ddl

            if (id > 0)
            {
                var staffList = masterCaching.GetStaffList();
                if (staffList != null && staffList.Exists(m => m.StaffListId == id))
                {
                    model.ListDepartmentTitle = listDepartmentTitle;
                    model.ListDepartmentStatus = listDepartmentStatus;
                    model.ListDepartmentGeneral = listDepartmentGeneral;
                    model.ListDepartmentUnit = listDepartmentUnit;
                    model.ListDepartmentCentre = listDepartmentCentre;
                    model.ListDepartmentDepartment = listDepartmentDepartment;
                    model.ListDepartmentGroup = listDepartmentGroup;

                    model.Id = staffList.Single(m => m.StaffListId == id).StaffListId;
                    model.StaffListId = staffList.Single(m => m.StaffListId == id).StaffListId;
                    model.StaffListCode = staffList.Single(m => m.StaffListId == id).StaffListCode;
                    model.FullName = staffList.Single(m => m.StaffListId == id).FullName;
                    model.General = staffList.Single(m => m.StaffListId == id).General;
                    model.Email = staffList.Single(m => m.StaffListId == id).Email;
                    model.PhoneNo = staffList.Single(m => m.StaffListId == id).PhoneNo;
                    model.UnitCodeId = staffList.Single(m => m.StaffListId == id).UnitCodeId;
                    model.CentreCodeId = staffList.Single(m => m.StaffListId == id).CentreCodeId;
                    model.DepartmentCodeId = staffList.Single(m => m.StaffListId == id).DepartmentCodeId;
                    model.GroupCodeId = staffList.Single(m => m.StaffListId == id).GroupCodeId;
                    model.OfficeLocation = staffList.Single(m => m.StaffListId == id).OfficeLocation;
                    model.CityCode = staffList.Single(m => m.StaffListId == id).CityCode;
                    model.TitleCodeId = staffList.Single(m => m.StaffListId == id).TitleCodeId;
                    model.LevelCode = staffList.Single(m => m.StaffListId == id).LevelCode;
                    model.StaffListId = staffList.Single(m => m.StaffListId == id).StaffListId;
                    model.ManagerCode = staffList.Single(m => m.StaffListId == id).ManagerCode;
                    model.BirthDate = staffList.Single(m => m.StaffListId == id).BirthDate;
                    model.JoinCompanyDate = staffList.Single(m => m.StaffListId == id).JoinCompanyDate;
                }
            }
            else
            {
                model.Id = 0;
                model.StaffListId = 0;
                model.StaffListCode = "";
                model.FullName = "";
                model.General = 0;
                model.Email = "";
                model.PhoneNo = "";
                model.UnitCodeId = 0;
                model.CentreCodeId = 0;
                model.DepartmentCodeId = 0;
                model.GroupCodeId = 0;
                model.OfficeLocation = "";
                model.CityCode = "";
                model.TitleCodeId = 0;
                model.LevelCode = "";
                model.StaffListId = 0;
                model.ManagerCode = "";
                model.BirthDate = new DateTime(1900, 1, 1);
                model.JoinCompanyDate = new DateTime(1900, 1, 1);
                model.ListDepartmentTitle = listDepartmentTitle;
                model.ListDepartmentStatus = listDepartmentStatus;
                model.ListDepartmentGeneral = listDepartmentGeneral;
                model.ListDepartmentUnit = listDepartmentUnit;
                model.ListDepartmentCentre = new List<LocationContract>();
                model.ListDepartmentDepartment = new List<LocationContract>();
                model.ListDepartmentGroup = new List<LocationContract>();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateStaffList(CreateUpdateStaffListModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new StaffListContract
                {
                    StaffListId = model.StaffListId,
                    StaffListCode = Sanitizer.GetSafeHtmlFragment(model.StaffListCode),
                    FullName = Sanitizer.GetSafeHtmlFragment(model.FullName),
                    General = model.General,
                    Email = Sanitizer.GetSafeHtmlFragment(model.Email),
                    PhoneNo = Sanitizer.GetSafeHtmlFragment(model.PhoneNo),
                    UnitCodeId = model.UnitCodeId,
                    CentreCodeId = model.CentreCodeId,
                    DepartmentCodeId = model.DepartmentCodeId,
                    GroupCodeId = model.GroupCodeId,
                    OfficeLocation = Sanitizer.GetSafeHtmlFragment(model.OfficeLocation),
                    CityCode = Sanitizer.GetSafeHtmlFragment(model.CityCode),
                    TitleCodeId = model.TitleCodeId,
                    LevelCode = Sanitizer.GetSafeHtmlFragment(model.LevelCode),
                    StatusId = model.StatusId,
                    ManagerCode = Sanitizer.GetSafeHtmlFragment(model.ManagerCode),
                    BirthDate = model.BirthDate,
                    JoinCompanyDate = model.JoinCompanyDate,
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdateStaffList(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.StaffList_SuccessCreate)
                    {
                        response.SystemMessage = "StaffList_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.StaffList_SuccessUpdate)
                    {
                        response.SystemMessage = "StaffList_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.StaffList_DuplicateCode)
                    {
                        response.SystemMessage = "StaffList_DuplicateCode";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetCityList(string stateID)
        {
            List<LocationContract> lstcity = new List<LocationContract>();
            int stateiD = Convert.ToInt32(stateID);
            var lst = locationApi.Get();
            lstcity = lst.Where(x => x.ParentId == stateiD).ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstcity);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Danh mục nhóm thuộc tính BU
        public ActionResult ListPnLBuAttributeGroup()
        {
            var model = masterCaching.GetPnLBuAttributeGroup();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePnLBuAttributeGroup(int id)
        {
            var response = masterCaching.DeletePnLBuAttributeGroup(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.PBAG_SuccessDelete);
        }

        public ActionResult CreateUpdatePnLBuAttributeGroup(int id = 0)
        {
            var model = new CreateUpdatePnLBuAttributeGroupModel();
            if (id > 0)
            {
                var pnlList = masterCaching.GetPnLBuAttributeGroup();
                if (pnlList != null && pnlList.Exists(m => m.PnLBuAttributeGroupId == id))
                {
                    model.Id = pnlList.Single(m => m.PnLBuAttributeGroupId == id).PnLBuAttributeGroupId;
                    model.PnLBuAttributeGroupId = pnlList.Single(m => m.PnLBuAttributeGroupId == id).PnLBuAttributeGroupId;
                    model.PnLBuAttributeGroupCode = pnlList.Single(m => m.PnLBuAttributeGroupId == id).PnLBuAttributeGroupCode;
                    model.PnLBuAttributeGroupName = pnlList.Single(m => m.PnLBuAttributeGroupId == id).PnLBuAttributeGroupName;
                    model.Description = pnlList.Single(m => m.PnLBuAttributeGroupId == id).Description;
                }
            }
            else
            {
                model.Id = 0;
                model.PnLBuAttributeGroupId = id;
                model.PnLBuAttributeGroupCode = "";
                model.PnLBuAttributeGroupName = "";
                model.Description = "";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdatePnLBuAttributeGroup(CreateUpdatePnLBuAttributeGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new PnLBuAttributeGroupContract
                {
                    PnLBuAttributeGroupId = model.PnLBuAttributeGroupId,
                    PnLBuAttributeGroupCode = Sanitizer.GetSafeHtmlFragment(model.PnLBuAttributeGroupCode),
                    PnLBuAttributeGroupName = Sanitizer.GetSafeHtmlFragment(model.PnLBuAttributeGroupName),
                    Description = Sanitizer.GetSafeHtmlFragment(model.Description),
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdatePnLBuAttributeGroup(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.PBAG_SuccessCreate)
                    {
                        response.SystemMessage = "PBAG_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PBAG_SuccessUpdate)
                    {
                        response.SystemMessage = "PBAG_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PBAG_DuplicateCode)
                    {
                        response.SystemMessage = "PBAG_DuplicateCode";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PBAG_DuplicateName)
                    {
                        response.SystemMessage = "PBAG_DuplicateName";
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

        #region Thuộc tính BU
        public ActionResult ListPnLBuAttribute()
        {
            var model = masterCaching.GetPnLBuAttribute();
            return View(model);
        }
        [ValidateAntiForgeryToken]
        public ActionResult DeletePnLBuAttribute(int id)
        {
            var response = masterCaching.DeletePnLBuAttribute(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.PnLBuAttribute_SuccessDelete);
        }
        public ActionResult CreateUpdatePnLBuAttribute(int id = 0)
        {
            var model = new CreateUpdatePnLBuAttributeModel();

            #region tạo ddl
            var listAttributeGroup = masterCaching.GetPnLBuAttributeGroup();
            var listPnLList = masterCaching.GetListPnLList();
            var listPnLBuList = masterCaching.GetListPnLBuList();
            #endregion tạo ddl

            if (id > 0)
            {
                var pnlBuAttribute = masterCaching.GetPnLBuAttribute();
                if (pnlBuAttribute != null && pnlBuAttribute.Exists(m => m.PnLBuAttributeId == id))
                {
                    model.ListPnLAttributeGroup = listAttributeGroup;
                    model.ListPnLList = listPnLList;
                    model.ListPnLBuList = listPnLBuList;

                    model.Id = pnlBuAttribute.Single(m => m.PnLBuAttributeId == id).PnLBuAttributeId;
                    model.PnLBuAttributeId = pnlBuAttribute.Single(m => m.PnLBuAttributeId == id).PnLBuAttributeId;
                    model.PnLBuAttributeCode = pnlBuAttribute.Single(m => m.PnLBuAttributeId == id).PnLBuAttributeCode;
                    model.PnLBuAttributeName = pnlBuAttribute.Single(m => m.PnLBuAttributeId == id).PnLBuAttributeName;
                    model.PnLListId = pnlBuAttribute.Single(m => m.PnLBuAttributeId == id).PnLListId;
                    model.PnLBUListId = pnlBuAttribute.Single(m => m.PnLBuAttributeId == id).PnLBUListId;
                    model.PnLAttributeGroupId = pnlBuAttribute.Single(m => m.PnLBuAttributeId == id).PnLAttributeGroupId;
                    model.Description = pnlBuAttribute.Single(m => m.PnLBuAttributeId == id).Description;
                }
            }
            else
            {
                //model.Id = 0;
                model.PnLBuAttributeId = id;
                model.PnLBuAttributeCode = "";
                model.PnLBuAttributeName = "";
                model.PnLAttributeGroupId = 0;
                model.PnLListId = 0;
                model.PnLBUListId = 0;
                model.Description = "";
                model.ListPnLAttributeGroup = listAttributeGroup;
                model.ListPnLList = listPnLList;
                model.ListPnLBuList = new List<PnLBuListContract>();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdatePnLBuAttribute(CreateUpdatePnLBuAttributeModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new PnLBuAttributeContract
                {
                    PnLBuAttributeId = model.PnLBuAttributeId,
                    PnLBuAttributeCode = Sanitizer.GetSafeHtmlFragment(model.PnLBuAttributeCode),
                    PnLBuAttributeName = Sanitizer.GetSafeHtmlFragment(model.PnLBuAttributeName),
                    Description = Sanitizer.GetSafeHtmlFragment(model.Description),
                    Visible = true,
                    PnLAttributeGroupId = model.PnLAttributeGroupId,
                    PnLListId = model.PnLListId,
                    PnLBUListId = model.PnLBUListId,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdatePnLBuAttribute(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.PnLBuAttribute_SuccessCreate)
                    {
                        response.SystemMessage = "PnLBuAttribute_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PnLBuAttribute_SuccessUpdate)
                    {
                        response.SystemMessage = "PnLBuAttribute_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PnLBuAttribute_DuplicateName)
                    {
                        response.SystemMessage = "PnLBuAttribute_DuplicateName";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.PnLBuAttribute_DuplicateCode)
                    {
                        response.SystemMessage = "PnLBuAttribute_DuplicateCode";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPnLBuByPnL(string stateID)
        {
            List<PnLBuListContract> lstPnLBu = new List<PnLBuListContract>();
            int stateiD = Convert.ToInt32(stateID);
            var lst = masterCaching.GetListPnLBuList().ToList();
            lstPnLBu = lst.Where(x => x.PnLListId == stateiD).ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstPnLBu);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Vùng miền
        public ActionResult ListRegion()
        {
            var model = masterCaching.GetRegion();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRegion(int id)
        {
            var response = masterCaching.DeleteRegion(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.Region_SuccessDelete);
        }

        public ActionResult CreateUpdateRegion(int id = 0)
        {
            var model = new CreateUpdateRegionModel();
            if (id > 0)
            {
                var region = masterCaching.GetRegion();
                if (region != null && region.Exists(m => m.RegionId == id))
                {
                    model.Id = region.Single(m => m.RegionId == id).RegionId;
                    model.RegionId = region.Single(m => m.RegionId == id).RegionId;
                    model.RegionCode = region.Single(m => m.RegionId == id).RegionCode;
                    model.RegionName = region.Single(m => m.RegionId == id).RegionName;
                    model.Description = region.Single(m => m.RegionId == id).Description;
                }
            }
            else
            {
                model.Id = 0;
                model.RegionId = id;
                model.RegionCode = "";
                model.RegionName = "";
                model.Description = "";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateRegion(CreateUpdateRegionModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new RegionContract
                {
                    RegionId = model.RegionId,
                    RegionCode = Sanitizer.GetSafeHtmlFragment(model.RegionCode),
                    RegionName = Sanitizer.GetSafeHtmlFragment(model.RegionName),
                    Description = Sanitizer.GetSafeHtmlFragment(model.Description),
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdateRegion(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.Region_SuccessCreate)
                    {
                        response.SystemMessage = "Region_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Region_SuccessUpdate)
                    {
                        response.SystemMessage = "Region_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Region_DuplicateName)
                    {
                        response.SystemMessage = "Region_DuplicateName";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Region_DuplicateCode)
                    {
                        response.SystemMessage = "Region_DuplicateCode";
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

        #region Đơn vị hành chính
        public ActionResult DvhcList()
        {
            var listDvhc = masterCaching.GetDvhc();
            return View(listDvhc.OrderBy(x => x.AdministrativeUnitsId).ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteDvhc(int id)
        {
            var response = masterCaching.Delete(id, CurrentUserId);

            if (response == null)
                return Json(new CUDReturnMessage(ResponseCode.Error));
            else
            {
                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
            }
            return Json(response);
        }

        [HttpGet]
        public ActionResult CreateUpdateDvhc(int id = 0, int pid = 0)
        {
            CreateUpdateDvhcModel model;
            var listprefix = masterCaching.GetPrefix();
            var listprefixBy = new List<PrefixContract>();
            string parentName = "";

            if (id > 0)
            {
                listprefixBy = new List<PrefixContract>();
                var response = masterCaching.Find(id);

                if (response == null)
                {
                    model = new CreateUpdateDvhcModel()
                    {
                        AdministrativeUnitsId = id,
                        ParentId = pid
                    };

                    ViewBag.ErrorMessage = MessageResource.Shared_SystemErrorMessage;
                }
                else
                {
                    pid = response.ParentId ?? 0;

                    if (pid > 0)
                    {
                        var parent = masterCaching.Find(pid);
                        if (parent != null)
                        {
                            parentName = parent.AdministrativeUnitsVN;
                        }
                    }
                    listprefixBy = listprefix.Where(x => x.PrefixGroup == response.LevelNo).ToList();
                    model = new CreateUpdateDvhcModel()
                    {
                        AdministrativeUnitsId = id,
                        ParentId = response.ParentId ?? pid,
                        AdministrativeUnitsVN = response.AdministrativeUnitsVN,
                        AdministrativeUnitsEN = response.AdministrativeUnitsEN,
                        LevelNo = response.LevelNo,
                        Prefix = response.Prefix,
                        ParentName = parentName,
                        ListPrefix = listprefixBy
                    };
                }
            }
            else
            {
                listprefixBy = new List<PrefixContract>();
                int levelNo = 1;
                if (pid > 0)
                {
                    var parent = masterCaching.Find(pid);
                    if (parent != null)
                    {
                        parentName = parent.AdministrativeUnitsVN;
                        levelNo = parent.LevelNo + 1;
                    }
                }
                listprefixBy = listprefix.Where(x => x.PrefixGroup == levelNo).ToList();
                model = new CreateUpdateDvhcModel()
                {
                    AdministrativeUnitsId = id,
                    ParentId = pid,
                    AdministrativeUnitsVN = "",
                    AdministrativeUnitsEN = "",
                    LevelNo = pid == 0 ? 1 : levelNo,//Tạo node root (k có pid) thì level = 1 ngược lại là level = parentLevel + 1
                    ParentName = parentName,
                    Prefix = "",
                    ListPrefix = listprefixBy
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public JsonResult CreateUpdateDvhc(CreateUpdateDvhcModel model)
        {
            if (ModelState.IsValid)
            {
                DvhcContract data = new DvhcContract();
                data.AdministrativeUnitsId = model.AdministrativeUnitsId;
                data.AdministrativeUnitsVN = Sanitizer.GetSafeHtmlFragment(model.AdministrativeUnitsVN);
                data.ParentId = model.ParentId;
                data.LevelNo = model.LevelNo;
                data.Prefix = model.Prefix;
                data.CreatedBy = CurrentUserId;
                data.UpdatedBy = CurrentUserId;
                data.CreatedDate = DateTime.Now;
                data.UpdatedDate = DateTime.Now;
                var response = masterCaching.InsertUpdateDvhc(data);

                if (response == null)
                {
                    response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                if (response.Id == (int)ResponseCode.Dvhc_SuccessCreate
                        || response.Id == (int)ResponseCode.Dvhc_SuccessUpdate)
                {
                    response.SystemMessage = "Shared_SaveSuccess";
                    TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                }
                else if (response.Id == (int)ResponseCode.Dvhc_DuplicateName)
                {
                    response.SystemMessage = "Dvhc_DuplicateName";
                }
                else
                {
                    response.SystemMessage = "CMS_GetRuntimeErrorMsg";
                }

                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                return Json(response);
            }

            var cudMsg = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
            cudMsg.Message = StringUtil.GetResourceString(typeof(MessageResource), cudMsg.SystemMessage);
            return Json(cudMsg);
        }
        #endregion

        #region Danh mục nhóm cơ sở
        public ActionResult GetBasisGroup()
        {
            var model = masterCaching.GetBasisGroup();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBasisGroup(int id)
        {
            var response = masterCaching.DeleteBasisGroup(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.BasisGroup_SuccessDelete);
        }

        public ActionResult CreateUpdateBasisGroup(int id = 0)
        {
            var model = new CreateUpdateBasisGroupModel();
            if (id > 0)
            {
                var basisGroup = masterCaching.GetBasisGroup();
                if (basisGroup != null && basisGroup.Exists(m => m.BasisGroupId == id))
                {
                    model.Id = basisGroup.Single(m => m.BasisGroupId == id).BasisGroupId;
                    model.BasisGroupId = basisGroup.Single(m => m.BasisGroupId == id).BasisGroupId;
                    model.BasisGroupCode = basisGroup.Single(m => m.BasisGroupId == id).BasisGroupCode;
                    model.BasisGroupName = basisGroup.Single(m => m.BasisGroupId == id).BasisGroupName;
                    model.Address = basisGroup.Single(m => m.BasisGroupId == id).Address;
                    model.Longitude = basisGroup.Single(m => m.BasisGroupId == id).Longitude;
                    model.Latitude = basisGroup.Single(m => m.BasisGroupId == id).Latitude;
                }
            }
            else
            {
                model.Id = 0;
                model.BasisGroupId = id;
                model.BasisGroupCode = "";
                model.BasisGroupName = "";
                model.Address = "";
                model.Longitude = 0;
                model.Latitude = 0;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateBasisGroup(CreateUpdateBasisGroupModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new BasisGroupContract
                {
                    BasisGroupId = model.BasisGroupId,
                    BasisGroupCode = Sanitizer.GetSafeHtmlFragment(model.BasisGroupCode),
                    BasisGroupName = Sanitizer.GetSafeHtmlFragment(model.BasisGroupName),
                    Address = Sanitizer.GetSafeHtmlFragment(model.Address),
                    Longitude = model.Longitude,
                    Latitude = model.Latitude,
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdateBasisGroup(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.BasisGroup_SuccessCreate)
                    {
                        response.SystemMessage = "BasisGroup_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.BasisGroup_SuccessUpdate)
                    {
                        response.SystemMessage = "BasisGroup_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.BasisGroup_DuplicateName)
                    {
                        response.SystemMessage = "BasisGroup_DuplicateName";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.BasisGroup_DuplicateCode)
                    {
                        response.SystemMessage = "BasisGroup_DuplicateCode";
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

        #region Danh mục cơ sở
        public ActionResult ListBasis()
        {
            var model = masterCaching.GetBasis();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBasis(int id)
        {
            var response = masterCaching.DeleteBasis(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.Basis_SuccessDelete);
        }
        public ActionResult CreateUpdateBasis(int id = 0)
        {
            var model = new CreateUpdateBasisModel();

            #region tạo ddl
            var listListBasisGroup = masterCaching.GetBasisGroup();
            var listPnList = masterCaching.GetListPnLList();
            var listPnLBuList = masterCaching.GetListPnLBuList();
            var listDvhc = masterCaching.GetDvhc();
            var listCity = listDvhc.Where(x => x.ParentId == 0 && x.LevelNo == 1).ToList();
            var listDistrict = listDvhc.Where(x => x.LevelNo == 2).ToList();
            var listWard = listDvhc.Where(x => x.LevelNo == 3).ToList();
            var listListBasisStatus = masterCaching.GetBasisStatus();
            var listDeList = locationApi.Get();
            var listDepartment = listDeList.Where(x => x.LayoutTypeId == 1 && !x.IsDeleted).ToList();
            var listStaff = masterCaching.GetStaffList();
            #endregion tạo ddl

            if (id > 0)
            {
                var basis = masterCaching.GetBasis();
                if (basis != null && basis.Exists(m => m.BasisId == id))
                {
                    model.ListBasisGroup = listListBasisGroup;
                    model.ListPnLList = listPnList;
                    model.ListPnLBuList = listPnLBuList;
                    model.ListCity = listCity;
                    model.ListDistrict = listDistrict;
                    model.ListWard = listWard;
                    model.ListBasisStatus = listListBasisStatus;
                    model.ListDepartment = listDepartment;
                    model.ListBasisStatus = listListBasisStatus;
                    model.ListStaff = listStaff;

                    model.Id = basis.Single(m => m.BasisId == id).BasisId;
                    model.BasisId = basis.Single(m => m.BasisId == id).BasisId;
                    model.BasisCode = basis.Single(m => m.BasisId == id).BasisCode;
                    model.BasisName = basis.Single(m => m.BasisId == id).BasisName;
                    model.BasisGroupId = basis.Single(m => m.BasisId == id).BasisGroupId;
                    model.PnLListId = basis.Single(m => m.BasisId == id).PnLListId;
                    model.PnLBUListId = basis.Single(m => m.BasisId == id).PnLBUListId;
                    model.Description = basis.Single(m => m.BasisId == id).Description;
                    model.CityId = basis.Single(m => m.BasisId == id).CityId;
                    model.DistrictId = basis.Single(m => m.BasisId == id).DistrictId;
                    model.WardId = basis.Single(m => m.BasisId == id).WardId;
                    model.RefCode = basis.Single(m => m.BasisId == id).RefCode;
                    model.StatusId = basis.Single(m => m.BasisId == id).StatusId;
                    model.FullName = basis.Single(m => m.BasisId == id).FullName;
                    model.StatusDescription = basis.Single(m => m.BasisId == id).StatusDescription;
                    model.OpeningDate = basis.Single(m => m.BasisId == id).OpeningDate;
                    model.Latitude = basis.Single(m => m.BasisId == id).Latitude;
                    model.Longitude = basis.Single(m => m.BasisId == id).Longitude;
                    model.Address = basis.Single(m => m.BasisId == id).Address;
                    model.Manager = basis.Single(m => m.BasisId == id).Manager;
                    model.ManagerPhone = basis.Single(m => m.BasisId == id).ManagerPhone;
                    model.SitePhone = basis.Single(m => m.BasisId == id).SitePhone;
                    model.SiteEmail = basis.Single(m => m.BasisId == id).SiteEmail;
                    model.AreaManager = basis.Single(m => m.BasisId == id).AreaManager;
                    model.AreaManagerPhone = basis.Single(m => m.BasisId == id).AreaManagerPhone;
                    model.AreaManagerEmail = basis.Single(m => m.BasisId == id).AreaManagerEmail;
                    model.DepartmentId = basis.Single(m => m.BasisId == id).DepartmentId;
                    model.StaffId = basis.Single(m => m.BasisId == id).StaffId;
                }
            }
            else
            {
                model.Id = 0;
                model.BasisId = 0;
                model.BasisCode = "";
                model.BasisName = "";
                model.BasisGroupId = 0;
                model.PnLListId = 0;
                model.PnLBUListId = 0;
                model.Description = "";
                model.CityId = 0;
                model.DistrictId = 0;
                model.WardId = 0;
                model.RefCode = "";
                model.StatusId = 0;
                model.FullName = "";
                model.StatusDescription = "";
                model.OpeningDate = DateTime.Today;
                model.Latitude = 0;
                model.Longitude = 0;
                model.Address = "";
                model.Manager = "";
                model.ManagerPhone = "";
                model.SitePhone = "";
                model.SiteEmail = "";
                model.AreaManager = "";
                model.AreaManagerPhone = "";
                model.AreaManagerEmail = "";
                model.DepartmentId = 0;
                model.StaffId = 0;


                model.ListBasisGroup = listListBasisGroup;
                model.ListPnLList = listPnList;
                model.ListPnLBuList = new List<PnLBuListContract>();
                model.ListCity = listCity;
                model.ListDistrict = new List<DvhcContract>();
                model.ListWard = new List<DvhcContract>();
                model.ListBasisStatus = listListBasisStatus;
                model.ListDepartment = listDepartment;
                model.ListDepartmentView = new List<LocationContract>();
                model.ListBasisStatus = listListBasisStatus;
                model.ListStaff = listStaff;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateBasis(CreateUpdateBasisModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    //go on as normal
            //}
            //else
            //{
            //    var errors = ModelState.Select(x => x.Value.Errors)
            //                           .Where(y => y.Count > 0)
            //                           .ToList();
            //}
            if (ModelState.IsValid)
            {
                var msContent = new BasisContract
                {
                    BasisCode = Sanitizer.GetSafeHtmlFragment(model.BasisCode),
                    BasisName = Sanitizer.GetSafeHtmlFragment(model.BasisName),
                    BasisGroupId = model.BasisGroupId,
                    PnLListId = model.PnLListId,
                    PnLBUListId = model.PnLBUListId,
                    Description = Sanitizer.GetSafeHtmlFragment(model.Description),
                    CityId = model.CityId,
                    DistrictId = model.DistrictId,
                    WardId = model.WardId,
                    RefCode = Sanitizer.GetSafeHtmlFragment(model.RefCode),
                    StatusId = model.StatusId,
                    FullName = Sanitizer.GetSafeHtmlFragment(model.FullName),
                    StatusDescription = Sanitizer.GetSafeHtmlFragment(model.StatusDescription),
                    OpeningDate = model.OpeningDate,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Address = Sanitizer.GetSafeHtmlFragment(model.Address),
                    Manager = Sanitizer.GetSafeHtmlFragment(model.Manager),
                    ManagerPhone = Sanitizer.GetSafeHtmlFragment(model.ManagerPhone),
                    SitePhone = Sanitizer.GetSafeHtmlFragment(model.SitePhone),
                    SiteEmail = Sanitizer.GetSafeHtmlFragment(model.SiteEmail),
                    AreaManager = Sanitizer.GetSafeHtmlFragment(model.AreaManager),
                    AreaManagerPhone = Sanitizer.GetSafeHtmlFragment(model.AreaManagerPhone),
                    AreaManagerEmail = Sanitizer.GetSafeHtmlFragment(model.AreaManagerEmail),
                    DepartmentId = model.DepartmentId,
                    StaffId = model.StaffId,
                    Visible = true,
                    CreateBy = CurrentUserId,
                    UpdateBy = CurrentUserId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdateBasis(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.Basis_SuccessCreate)
                    {
                        response.SystemMessage = "Basis_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Basis_SuccessUpdate)
                    {
                        response.SystemMessage = "Basis_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Basis_DuplicateCode)
                    {
                        response.SystemMessage = "Basis_DuplicateCode";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Basis_DuplicateName)
                    {
                        response.SystemMessage = "Basis_DuplicateName";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDvhcBy(string stateID)
        {
            List<DvhcContract> lstDvhc = new List<DvhcContract>();
            int stateiD = Convert.ToInt32(stateID);
            var lst = masterCaching.GetDvhc();
            lstDvhc = lst.Where(x => x.ParentId == stateiD).ToList();
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(lstDvhc);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Title
        public ActionResult ListDepartmentTitle()
        {
            var model = masterCaching.GetDepartmentTitle();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDepartmentTitle(int id)
        {
            var response = masterCaching.DeleteDepartmentTitle(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.DepartmentTitle_SuccessDelete);
        }

        public ActionResult CreateUpdateDepartmentTitle(int id = 0)
        {
            var model = new CreateUpdateDepartmentTitleModel();
            if (id > 0)
            {
                var departmentTitle = masterCaching.GetDepartmentTitle();
                if (departmentTitle != null && departmentTitle.Exists(m => m.DepartmentTitleId == id))
                {
                    model.Id = departmentTitle.Single(m => m.DepartmentTitleId == id).DepartmentTitleId;
                    model.DepartmentTitleId = departmentTitle.Single(m => m.DepartmentTitleId == id).DepartmentTitleId;
                    model.DepartmentTitleCode = departmentTitle.Single(m => m.DepartmentTitleId == id).DepartmentTitleCode;
                    model.DepartmentTitleName = departmentTitle.Single(m => m.DepartmentTitleId == id).DepartmentTitleName;
                    model.Description = departmentTitle.Single(m => m.DepartmentTitleId == id).Description;
                }
            }
            else
            {
                model.Id = 0;
                model.DepartmentTitleId = id;
                model.DepartmentTitleCode = "";
                model.DepartmentTitleName = "";
                model.Description = "";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateDepartmentTitle(CreateUpdateDepartmentTitleModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new DepartmentTitleContract
                {
                    DepartmentTitleId = model.DepartmentTitleId,
                    DepartmentTitleCode = Sanitizer.GetSafeHtmlFragment(model.DepartmentTitleCode),
                    DepartmentTitleName = Sanitizer.GetSafeHtmlFragment(model.DepartmentTitleName),
                    Description = Sanitizer.GetSafeHtmlFragment(model.Description),
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdateDepartmentTitle(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.DepartmentTitle_SuccessCreate)
                    {
                        response.SystemMessage = "DepartmentTitle_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.DepartmentTitle_SuccessUpdate)
                    {
                        response.SystemMessage = "DepartmentTitle_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.DepartmentTitle_DuplicateName)
                    {
                        response.SystemMessage = "DepartmentTitle_DuplicateName";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.DepartmentTitle_DuplicateCode)
                    {
                        response.SystemMessage = "DepartmentTitle_DuplicateCode";
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

        #region Level
        public ActionResult ListLevel()
        {
            var model = masterCaching.GetLevel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteLevel(int id)
        {
            var response = masterCaching.DeleteLevel(id, CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.Level_SuccessDelete);
        }

        public ActionResult CreateUpdateLevel(int id = 0)
        {
            var model = new CreateUpdateLevelModel();
            if (id > 0)
            {
                var level = masterCaching.GetLevel();
                if (level != null && level.Exists(m => m.LevelId == id))
                {
                    model.Id = level.Single(m => m.LevelId == id).LevelId;
                    model.LevelId = level.Single(m => m.LevelId == id).LevelId;
                    model.LevelCode = level.Single(m => m.LevelId == id).LevelCode;
                    model.LevelName = level.Single(m => m.LevelId == id).LevelName;
                    model.Description = level.Single(m => m.LevelId == id).Description;
                }
            }
            else
            {
                model.Id = 0;
                model.LevelId = id;
                model.LevelCode = "";
                model.LevelName = "";
                model.Description = "";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateLevel(CreateUpdateLevelModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new LevelContract
                {
                    LevelId = model.LevelId,
                    LevelCode = Sanitizer.GetSafeHtmlFragment(model.LevelCode),
                    LevelName = Sanitizer.GetSafeHtmlFragment(model.LevelName),
                    Description = Sanitizer.GetSafeHtmlFragment(model.Description),
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = masterCaching.InsertUpdateLevel(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.Level_SuccessCreate)
                    {
                        response.SystemMessage = "Level_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Level_SuccessUpdate)
                    {
                        response.SystemMessage = "Level_SuccessUpdate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Level_DuplicateName)
                    {
                        response.SystemMessage = "Level_DuplicateName";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.Level_DuplicateCode)
                    {
                        response.SystemMessage = "Level_DuplicateCode";
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

        #region System 

        public ActionResult GetSystemCheckList(int state = 0, string kw = "", int p = 1, int ps = 0, Boolean export = false)
        {
            var listStates = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (export)
            {
                var pageSize = int.MaxValue;
                p = 1;
                var listData = masterCaching.GetSystemCheckList(CurrentUserId, state, kw, p, ps, 0);
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    return ExportTemplate(MakeSystemContentExcell(listData.Data), LayoutResource.AdminTools_System_ExportExcel_TitleFile,
                  "danh-sach-he-thong-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                }
                return null;
            }
            else
            {
                #region state
                listStates =
                    EnumExtension.ToListOfValueAndDesc<SystemStateEnum>().Select(r => new SelectListItem
                    {
                        Text = r.Description,
                        Value = r.Value.ToString()
                    }).ToList();
                listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
                #endregion state

                var data = masterCaching.GetSystemCheckList(CurrentUserId, state, kw, p, ps, 0);

                var model = new ListSystemCheckListModel()
                {
                    listData = data.Data,
                    TotalCount = data.TotalRows,
                    PageCount = ps,
                    PageNumber = p,
                    listStates = listStates,
                    state = state,
                    kw = kw
                };
                return View(model);
            }


        }
        #region export excell
        private ActionResult ExportTemplate(DataTable datatable, string title, string filename)
        {
            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add(title);
            var rowStartLoadData = 2;
            #region Formart column
            workSheet.Column(1).Width = 50;
            for (var icol = 2; icol <= datatable.Columns.Count; icol++)
            {
                workSheet.Column(icol).Width = 35;
            }
            /* Format cho header */

            using (var header = workSheet.Cells[rowStartLoadData - 1, 1, rowStartLoadData, datatable.Columns.Count])
            {
                header.AutoFitColumns(30);
                header.Style.Font.Color.SetColor(System.Drawing.Color.White);

                header.Style.Font.Bold = true;
                header.Style.Fill.PatternType = ExcelFillStyle.Solid;
                var headerBgColor = System.Drawing.ColorTranslator.FromHtml("#5e9bd3");
                header.Style.Fill.BackgroundColor.SetColor(headerBgColor);
                header.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                header.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            #endregion
            workSheet.Cells[rowStartLoadData, 1].LoadFromDataTable(datatable, true);

            using (var memoryStream = new MemoryStream())
            {
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename.UnicodeToKoDauAndGach() + ".xlsx");

                Response.Flush();
                Response.End();
            }

            return null;
        }
        private DataTable MakeSystemContentExcell(List<SystemCheckListContract> list)
        {
            var datatable = new DataTable("tblData");
            #region group header content
            datatable.Columns.Add(new DataColumn(LayoutResource.Shared_Label_SortNumber));
            datatable.Columns.Add(new DataColumn(LayoutResource.Master_SystemCheckList_SystemName));
            datatable.Columns.Add(new DataColumn(LayoutResource.Master_SystemCheckList_Description));
            datatable.Columns.Add(new DataColumn(LayoutResource.Master_SystemCheckList_CreatedDate));
            datatable.Columns.Add(new DataColumn(LayoutResource.Master_SystemCheckList_State));
            #endregion
            int i = 0;
            foreach (var detail in list)
            {
                var row = datatable.NewRow();
                i++;
                //content
                row[LayoutResource.Shared_Label_SortNumber] = i;
                row[LayoutResource.Master_SystemCheckList_SystemName] = detail.SystemName;
                row[LayoutResource.Master_SystemCheckList_Description] = detail.Description;
                row[LayoutResource.Master_SystemCheckList_CreatedDate] = detail.CreatedDate;
                row[LayoutResource.Master_SystemCheckList_State] = detail.StateName;
                datatable.Rows.Add(row);
            }
            return datatable;
        }
        #endregion


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteSystemCheckList(int SystemId)
        {
            CUDReturnMessage result;
            if (SystemId > 0)
            {
                result = masterCaching.DeleteSystemCheckList(SystemId, CurrentUserId);
            }
            else
            {
                result = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
            }
            return Json(new ActionMessage(result));
        }

        /// <summary>
        /// Them,chinh sua thong tin system 
        /// </summary>
        /// <param name="TemplateId"></param>
        /// <returns></returns>
        public ActionResult InsertUpdateSystemCheckList(int SystemId = 0)
        {
            var model = new InsertUpdatetSystemModel();
            var listStates = new List<SelectListItem>();

            var listCates = new List<SelectListItem>();
            var listSubCates = new List<SelectListItem>();
            var listOrginTypes = new List<SelectListItem>();
            var listAppTypes = new List<SelectListItem>();
            var listStatus = new List<SelectListItem>();
            var listAuthenticationMethods = new List<SelectListItem>();
            var listRanks = new List<SelectListItem>();
            var listRTOTypes = new List<SelectListItem>();
            var listRPOTypes = new List<SelectListItem>();
            var listDRs = new List<SelectListItem>();
            var listServiceContinutys = new List<SelectListItem>();
            var listStabilitys = new List<SelectListItem>();
            var listSystemTypes = new List<SelectListItem>();
            var listSecuritys = new List<SelectListItem>();
            var listPerformances = new List<SelectListItem>();
            var listPLs = new List<SelectListItem>();
            model.LastDateDRTest = DateTime.Now;

            if (SystemId > 0)
            {
                var infoSystem = masterCaching.GetSystemCheckList(CurrentUserId, 0, string.Empty, 1, 20, SystemId).Data.FirstOrDefault();
                if (infoSystem != null)
                {
                    model.State = infoSystem.State;
                    model.SystemId = infoSystem.SystemId;
                    model.SystemName = infoSystem.SystemName;
                    model.Description = infoSystem.Description;
                    model.Priority = infoSystem.Priority;


                    //update add more info
                    model.Code = infoSystem.Code;
                    model.PlId = infoSystem.PlId;
                    model.SystemNameEn = infoSystem.SystemNameEn;
                    model.CateId = infoSystem.CateId;
                    model.SubCateId = infoSystem.SubCateId;
                    model.FunctionOverview = infoSystem.FunctionOverview;
                    model.ProviderName = infoSystem.ProviderName;
                    model.OriginTypeId = infoSystem.OriginTypeId;
                    model.Platform = infoSystem.Platform;
                    model.AppTypeId = infoSystem.AppTypeId;
                    model.IsSAP = infoSystem.IsSAP;
                    model.Status = infoSystem.State;
                    model.UrlSystem = infoSystem.UrlSystem;
                    model.AuthenticationMethodId = infoSystem.AuthenticationMethodId;
                    model.RankId = infoSystem.RankId;
                    model.RTOTypeId = infoSystem.RTOTypeId;
                    model.RPOTypeId = infoSystem.RPOTypeId;
                    model.RLO = infoSystem.RLO ?? 0;
                    model.DRTypeId = infoSystem.DRTypeId;
                    model.LastDateDRTest = infoSystem.LastDateDRTest ?? DateTime.Now;
                    model.ScStateId = infoSystem.ScStateId;
                    model.SME = infoSystem.SME;
                    model.OwingBusinessUnit = infoSystem.OwingBusinessUnit;
                    model.ITContact = infoSystem.ITContact;
                    model.YearImplement = infoSystem.YearImplement;
                    model.QuantityUserActive = infoSystem.QuantityUserActive;
                    model.ConCurrentUser = infoSystem.ConCurrentUser;
                    model.BusinessHour = infoSystem.BusinessHour;
                    model.BusinessIssue = infoSystem.BusinessIssue;
                    model.TechIssue = infoSystem.TechIssue;
                    model.SystemMaintainTime = infoSystem.SystemMaintainTime;
                    model.IsDevTest = infoSystem.IsDevTest;
                    model.HostingLocation = infoSystem.HostingLocation;
                    model.IsReplace = infoSystem.IsReplace;
                    model.ReplaceBy = infoSystem.ReplaceBy;
                    model.DetailReplaceBy = infoSystem.DetailReplaceBy;
                    model.IsRequirementSecurity = infoSystem.IsRequirementSecurity;
                    model.IsRequirementSecurityDesign = infoSystem.IsRequirementSecurityDesign;
                    model.IsCheckCertification = infoSystem.IsCheckCertification;
                    model.IsCheckSecurityByGolive = infoSystem.IsCheckSecurityByGolive;
                    model.IsCheckRisk = infoSystem.IsCheckRisk;
                    model.SecurityStateId = infoSystem.SecurityStateId;
                    model.PerformanceId = infoSystem.PerformanceId;
                    model.PerformanceNote = infoSystem.PerformanceNote;
                }
            }
            #region master data

            #region state
            listStates =
                EnumExtension.ToListOfValueAndDesc<SystemStateEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.State)
                }).ToList();
            #endregion state

            #region PL
            var dataPLs = masterCaching.GetListPnLList();
            if (dataPLs != null && dataPLs != null && dataPLs.Any())
            {
                listPLs = dataPLs.Select(c => new SelectListItem
                {
                    Text = c.PnLListName,
                    Value = c.PnLListId.ToString(),
                    Selected = (c.PnLListId == model.PlId)
                }).ToList();
            }
            #endregion


            #region cate
            var dataCate = masterCaching.GetCate((int)SystemStateEnum.Active, string.Empty, 0, 1, int.MaxValue);
            if (dataCate != null && dataCate.Data != null && dataCate.Data.Any())
            {
                listCates = dataCate.Data.Select(c => new SelectListItem
                {
                    Text = c.CateName,
                    Value = c.CateId.ToString(),
                    Selected = (c.CateId == model.CateId)
                }).ToList();
            }
            #endregion

            #region subcate
            var dataSubCate = masterCaching.GetSubCate((int)SystemStateEnum.Active, string.Empty, 0, 1, int.MaxValue);
            if (dataSubCate != null && dataSubCate.Data != null && dataSubCate.Data.Any())
            {
                listSubCates = dataSubCate.Data.Select(c => new SelectListItem
                {
                    Text = c.SubCateName,
                    Value = c.SubCateId.ToString(),
                    Selected = (c.SubCateId == model.SubCateId)
                }).ToList();
            }
            #endregion

            #region Orgin Type
            listOrginTypes =
                EnumExtension.ToListOfValueAndDesc<OrginTypeEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.OriginTypeId)
                }).ToList();
            #endregion

            #region App Type
            listAppTypes =
                EnumExtension.ToListOfValueAndDesc<AppTypeEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.AppTypeId)
                }).ToList();
            #endregion

            #region Status
            listStatus =
                EnumExtension.ToListOfValueAndDesc<SystemStatusEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.Status)
                }).ToList();
            #endregion

            #region AuthenticationMethod
            listAuthenticationMethods =
                EnumExtension.ToListOfValueAndDesc<AuthenticationMethodEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.AuthenticationMethodId)
                }).ToList();
            #endregion

            #region Rank
            listRanks =
                EnumExtension.ToListOfValueAndDesc<RankEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.RankId)
                }).ToList();
            #endregion

            #region RTO
            listRTOTypes =
                EnumExtension.ToListOfValueAndDesc<RTOTypeEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.RTOTypeId)
                }).ToList();
            #endregion

            #region RPO
            listRPOTypes =
                EnumExtension.ToListOfValueAndDesc<RPOTypeEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.RPOTypeId)
                }).ToList();
            #endregion

            #region DRs
            listDRs =
                EnumExtension.ToListOfValueAndDesc<DREnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.DRTypeId)
                }).ToList();
            #endregion


            #region Service continuetys
            listServiceContinutys =
                EnumExtension.ToListOfValueAndDesc<ServiceContinutyEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.ScStateId)
                }).ToList();
            #endregion

            #region Stability
            listStabilitys =
                EnumExtension.ToListOfValueAndDesc<StabilityEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.StabilityId)
                }).ToList();
            #endregion

            #region System Types
            listSystemTypes =
                EnumExtension.ToListOfValueAndDesc<SystemTypeEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.SystemId)
                }).ToList();
            #endregion

            #region Security Types
            listSecuritys =
                EnumExtension.ToListOfValueAndDesc<SecurityStateEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.SecurityStateId)
                }).ToList();
            #endregion

            #region performance Types
            listPerformances =
                EnumExtension.ToListOfValueAndDesc<PerformanceEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.PerformanceId)
                }).ToList();
            #endregion

            #endregion

            model.listStates = listStates;
            model.listCates = listCates;
            model.listSubCates = listSubCates;
            model.listOrginTypes = listOrginTypes;
            model.listAppTypes = listAppTypes;
            model.listStatus = listStatus;
            model.listAuthenticationMethods = listAuthenticationMethods;
            model.listRanks = listRanks;
            model.listRTOTypes = listRTOTypes;
            model.listRPOTypes = listRPOTypes;
            model.listDRs = listDRs;
            model.listServiceContinutys = listServiceContinutys;
            model.listStabilitys = listStabilitys;
            model.listSystemTypes = listSystemTypes;
            model.listSecuritys = listSecuritys;
            model.listPerformances = listPerformances;
            model.listPLs = listPLs;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertUpdateSystemByAjax(InsertUpdatetSystemModel model)
        {
            if (ModelState.IsValid || (model.SystemId > 0))
            {
                model.SystemName = Sanitizer.GetSafeHtmlFragment(model.SystemName);
                model.SystemNameEn= Sanitizer.GetSafeHtmlFragment(model.SystemNameEn);
                model.PerformanceNote= Sanitizer.GetSafeHtmlFragment(model.PerformanceNote);
                var response = new CUDReturnMessage();
                SystemCheckListContract param = lazyMapper.Value.Map<SystemCheckListContract>(model);
                param.Visible = model.Status == (int)SystemStatusEnum.Active ? true : false;
                param.State = model.Status;
                response = masterCaching.InsertUpdateSystemCheckList(param,CurrentUserId);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.SystemMngt_SuccessCreated || response.Id == (int)ResponseCode.SystemMngt_SuccessUpdated)
                    {
                        response.SystemMessage = "Shared_SaveSuccess";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                }
                else
                {
                    response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                return Json(response);
            }
            var error = ModelState.Select(x => x.Value.Errors)
                         .Where(y => y.Count > 0)
                         .ToList();
            var systemMessage = "CMS_GetRuntimeErrorMsg";
            var cudMsg = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = systemMessage };
            return Json(new ActionMessage(cudMsg));
        }
        /// <summary>
        /// Gán/ chỉnh sửa thông tin owner
        /// </summary>
        /// <param name="SystemId"></param>
        /// <returns></returns>
        public ActionResult InsertUpdateOwnerSystem(int SystemId = 0)
        {
            var model = new AssignOwnerSystemModel();
            var listUser = new List<SelectListItem>();
            #region user
            var listRoles = userMngtCaching.GetListRole(CurrentUserRoles.Min(r => r.Sort));
            var allLocation = locationApi.Get();

            var validLoc = new List<LocationContract>();
            foreach (var item in allLocation)
            {
                if (memberExtendedInfo.Locations.Exists(x => x.LocationId == item.LocationId))
                {
                    validLoc.Add(item);
                }
            }
            validLoc.ForEach(l =>
            {
                l.NameVN = StringHelper.GenInheritSpace(l.LevelNo) + l.NameVN;
            });

            List<int> searchLoc = new List<int>();
            searchLoc = validLoc.Select(x => x.LocationId).ToList();
            var dataUsers = new UserMngtCaching().GetListUser(searchLoc, 0, string.Empty,int.MaxValue,1).List;
            if (dataUsers != null && dataUsers.Any())
            {
                listUser = dataUsers.Select(c => new SelectListItem
                {
                    Text = c.FullName,
                    Value = c.UserId.ToString(),
                }).ToList();
            }
            model.listUsers = listUser;
            model.SystemId = SystemId;
            #endregion
            if (SystemId > 0)
            {
                var infoSystem = masterCaching.GetSystemCheckList(CurrentUserId, 0, string.Empty, 1, 20, SystemId).Data.FirstOrDefault();
                if (infoSystem != null)
                {
                    model.UIds = infoSystem.Users.Select(c => c.UserId).ToList();
                    model.SystemName = infoSystem.SystemName;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AssignUserToSystemCheckList(AssignOwnerSystemModel model)
        {
            if (ModelState.IsValid || (model.SystemId > 0))
            {
                model.SystemName = Sanitizer.GetSafeHtmlFragment(model.SystemName);
                var response = new CUDReturnMessage();
                AssignOwnerSystemContract param = lazyMapper.Value.Map<AssignOwnerSystemContract>(model);
                response = masterCaching.AssignUserToSystemCheckList(param,CurrentUserId);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.SystemMngt_AssignOwnerSuccessed)
                    {
                        response.SystemMessage = "Shared_SaveSuccess";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                }
                else
                {
                    response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                return Json(response);
            }
            var error = ModelState.Select(x => x.Value.Errors)
                         .Where(y => y.Count > 0)
                         .ToList();
            var systemMessage = "CMS_GetRuntimeErrorMsg";
            var cudMsg = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = systemMessage };
            return Json(new ActionMessage(cudMsg));
        }
        #endregion
    }
}