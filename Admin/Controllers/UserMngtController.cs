//using Caching.BMSMaster;

using System;
using Caching.Core;
using Caching.Microsite;
using Contract.AdminAction;
using Contract.Shared;
using Contract.User;
using Admin.Helper;
using Admin.Models.Shared;
using Admin.Models.User;
using Admin.Resource;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VG.Common;
using Contract.OR;
using Caching.OR;
using System.Text.RegularExpressions;
using VG.EncryptLib.EncryptLib;
using Microsoft.Security.Application;
using System.Data.Entity.Core.Metadata.Edm;
using Admin.Models;

namespace Admin.Controllers
{
    [CheckUserCaching]
    public class UserMngtController : BaseController
    {
        private int defaultPageSize = 10;
        private readonly IUserMngtCaching userMngtCaching;
        private readonly ISystemSettingCaching systemSettingApi;
        private readonly IMicrositeMngtCaching micrositeMngtApi;
        private readonly ISystemCaching systemMngtApi;
        private readonly ILocationCaching locationApi;
        private readonly IMasterCaching masterCaching;
        private readonly IORCaching orService;
        public UserMngtController(IAuthenCaching authenCaching,
            IUserMngtCaching userMngtCaching,
            IMasterCaching masterCaching,
            IMicrositeMngtCaching micrositeMngtApi,
            ISystemCaching systemMngtApi,
            ILocationCaching locationApi,
            IORCaching _orService,
        ISystemSettingCaching systemSettingApi) : base(authenCaching, systemSettingApi)
        {
            this.userMngtCaching = userMngtCaching;
            this.masterCaching = masterCaching;
            this.systemSettingApi = systemSettingApi;
            this.micrositeMngtApi = micrositeMngtApi;
            this.systemMngtApi = systemMngtApi;
            this.locationApi = locationApi;
            this.orService = _orService;

            defaultPageSize = AdminConfiguration.Paging_PageSize;
        }

        #region quản lý người dùng

        public ActionResult ListUser(int role = 0, int? locationId = null, string kw = "", int ps = 10, int p = 1)
        {
            //var tresting = Security.Encrypt(AppUtils.SecuKey, "vinmec@2018");

            //set default current location
            locationId = (locationId == null) ? memberExtendedInfo.CurrentLocationId : locationId;
            kw = HttpUtility.UrlDecode(kw).Trim();
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
            if (validLoc != null && validLoc.Count > 1)
            {
                validLoc.Insert(0, new LocationContract() { LocationId = 0, NameVN = LayoutResource.Shared_SelectOpt_All });
            }
            List<int> searchLoc = new List<int>();
            if (locationId != 0)
            {
                searchLoc = new List<int> { locationId.Value };
            }
            else if (validLoc.Any())
            {
                searchLoc = validLoc.Select(x => x.LocationId).ToList();
            }

            var data = userMngtCaching.GetListUser(searchLoc, role, kw, ps, p);

            ListUserModel model = new ListUserModel
            {
                ListUser = data.List,
                SelectMicrosite = new SelectList(AllowMicrosite, "MsId", "Title"),
                SelectRoles = new SelectList(listRoles, "RoleId", "RoleName"),
                SelectLocation = new SelectList(validLoc.OrderBy(x => x.LevelPath), "LocationId", "NameVN", locationId),
                TotalCount = data.Count,
                PageCount = ps,
                PageNumber = p,
                RoleId = role,
                SearchText = kw
            };

            var defaultPasswordSettingKey = systemSettingApi.Find("User.DefaultPassword");
            var desDefaultPassword = defaultPasswordSettingKey != null ? Security.Decrypt(AppUtils.SecuKey, defaultPasswordSettingKey.Value) : "12345678";
            ViewBag.DefaultPassword = desDefaultPassword;
            ViewBag.CurrentUserId = CurrentUserId;
            return View(model);
        }

        public ActionResult CreateUpdateUser(int userId = 0)
        {
            CreateUpdateUserModel model;

            var listRoles = userMngtCaching.GetListRole(CurrentUserRoles.Min(r => r.Sort));
            var listLocation = locationApi.Get();

            var validLoc = new List<LocationContract>();
            foreach (var item in listLocation)
            {
                if (memberExtendedInfo.Locations.Exists(x => x.LocationId == item.LocationId))
                {
                    validLoc.Add(item);
                }
            }

            #region tạo ddl department

            var listDepartments = new List<SelectListItem>() {
                new SelectListItem
                {
                    Text = LayoutResource.Shared_SelectOpt_All,
                    Value = "0"
                }
            };

            #endregion tạo ddl department

            if (userId > 0)
            {
                var response = userMngtCaching.FetchUserToUpdate(userId, CurrentUserId);
                var listUserOthers = userMngtCaching.GetAllUser(userId);

                if (response == null)
                {
                    model = new CreateUpdateUserModel()
                    {
                        UserId = userId
                    };
                    ViewBag.ErrorMessage = MessageResource.Shared_SystemErrorMessage;
                }
                else
                {
                    model = new CreateUpdateUserModel()
                    {
                        IsADAccount = response.IsADAccount,
                        Username = response.Username,
                        UserId = response.UserId,
                        Email = response.Email,
                        PhoneNumber = response.PhoneNumber,
                        FullName = response.FullName,
                        DeptId = response.DeptId,
                        RoleId = response.Roles == null ? new List<int>() : response.Roles.Select(r => r.RoleId).ToList(),
                        MicroSite = response.MicroSites == null ? new List<int>() : response.MicroSites.Select(r => r.MsId).ToList(),
                        ListRoles = listRoles,
                        ListMicroSites = AllowMicrosite.Select(r => new SelectListItem { Value = r.MsId.ToString(), Text = r.Title }).ToList(),
                        ListDepartments = listDepartments,
                        Location = response.Locations == null ? "" : string.Join(",", response.Locations.Select(r => r.LocationId).ToList()),
                        ListLocations = CommonHelper.ConvertToListLocationTreeViewModel(validLoc, response.Locations == null ? null : response.Locations.Select(r => r.LocationId).ToList()),
                        ListUsers = listUserOthers != null ? listUserOthers.Select(r => new SelectListItem { Value = r.UserId.ToString(), Text = r.UserName }).ToList() : new List<SelectListItem>(),
                        LineManagerUser = response.LineManager
                    };
                }
            }
            else
            {
                model = new CreateUpdateUserModel()
                {
                    IsADAccount = true,
                    Username = "",
                    UserId = 0,
                    Email = "",
                    FullName = "",
                    PhoneNumber = "",
                    DeptId = 0,
                    RoleId = new List<int>(),
                    MicroSite = new List<int>(),
                    ListRoles = listRoles,
                    ListMicroSites = AllowMicrosite.Select(r => new SelectListItem { Value = r.MsId.ToString(), Text = r.Title }).ToList(),
                    ListDepartments = listDepartments,
                    Location = "",
                    ListLocations = CommonHelper.ConvertToListLocationTreeViewModel(validLoc, null)
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateUser(CreateUpdateUserModel model)
        {
            var emailRegex = @"^[a-zA-Z]+[a-zA-Z0-9._-]+@[a-zA-Z0-9]{2,}(\.[a-zA-Z0-9]{2,4}){1,2}$";

            if (model.RoleId != null && model.RoleId.Contains((int)AdminRole.SuperAdmin) && CurrentUserRoles.Exists(r => r.RoleId == (int)AdminRole.SuperAdmin) == false)
            {
                ModelState.AddModelError(string.Empty, MessageResource.UserMngt_HaveNoPermission);
            }

            if (ModelState.IsValid)
            {
                var adInfo = VinAuthentication.GetADUserInfo(model.Username, DomainName);
                if (adInfo == null)
                {
                    CUDReturnMessage res = new CUDReturnMessage();
                    res.Id = (int)ResponseCode.UserMngt_UsernameNotExisted;
                    res.SystemMessage = "UserMngt_UsernameNotExisted";
                    res.Message = StringUtil.GetResourceString(typeof(MessageResource), res.SystemMessage);
                    return Json(res);
                }

                string email = adInfo.Properties["mail"].Value != null ? adInfo.Properties["mail"].Value.ToString() : "";
                //if (string.IsNullOrEmpty(email))
                //{
                //    return Json(new ActionMessage(-1, $"Email chưa cung cấp"));
                //}
                //else
                //{
                //    Regex regex = new Regex(emailRegex);
                //    Match match = regex.Match(email);
                //    if (!match.Success)
                //        return Json(new ActionMessage(-1, $"Email sai định dạng"));
                //}

                if (!string.IsNullOrEmpty(model.Email))
                {
                    Regex regex = new Regex(emailRegex);
                    Match match = regex.Match(model.Email);
                    if (!match.Success)
                        return Json(new ActionMessage(-1, $"Email sai định dạng"));
                }
                string telephoneNumber = adInfo.Properties["telephoneNumber"].Value != null ? adInfo.Properties["telephoneNumber"].Value.ToString() : "";
                string displayName = adInfo.Properties["displayName"].Value != null ? adInfo.Properties["displayName"].Value.ToString() : "";
                //linhht lẩy tên tự viết vào
                var user = new CreateUpdateUserContract
                {
                    Username = Sanitizer.GetSafeHtmlFragment(model.Username),
                    IsADAccount = true,
                    UserId = model.UserId,
                    Email = model.Email != null ? Sanitizer.GetSafeHtmlFragment(model.Email) : email,
                    PhoneNumber = telephoneNumber,
                    Fullname = model.FullName,//displayName,
                    Roles = model.RoleId,
                    MicroSites = model.MicroSite,
                    DeptId = model.DeptId,
                    Locations = model.Location.Split(',').Select(r => int.Parse(r)).ToList(),
                    LineManagerId = model.LineManagerUser
                };

                var response = userMngtCaching.CreateUpdateUser(user);

                if (response == null)
                {
                    response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                else if (response.Id == (int)ResponseCode.UserMngt_SuccessCreated || response.Id == (int)ResponseCode.UserMngt_SuccessUpdated)
                {
                    var msg = new ActionMessage(1, response.SystemMessage);
                    TempData[AdminGlobal.ActionMessage] = msg;
                    TempData["SettingNotiOneSecond"] = 1000;

                    return Json(msg);
                }
                else if (response.Id == (int)ResponseCode.UserMngt_EmailExisted)
                {
                    response.SystemMessage = "UserMngt_EmailExisted";
                }
                else if (response.Id == (int)ResponseCode.UserMngt_UsernameExisted)
                {
                    response.SystemMessage = "UserMngt_UsernameExisted";
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
        #region 13/03/2020 Comment old code by Phubq.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult GetADAccountInfo(string username)
        //{
        //    using (var context = new PrincipalContext(ContextType.Domain))
        //    {
        //        UserPrincipal user = UserPrincipal.FindByIdentity(context, IdentityType.DistinguishedName, username);

        //        if (user != null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult ResetPassword(int userId)
        //{
        //    var response = userMngtCaching.ResetPassword(userId);

        //    if (response == null)
        //        return Json(new ActionMessage(new CUDReturnMessage(ResponseCode.Error)));

        //    var actionMessage = new ActionMessage(response);

        //    if (response.Id == (int)ResponseCode.UserMngt_SuccessResetPassword)
        //    {
        //        actionMessage.ID = 1;
        //        TempData[AdminGlobal.ActionMessage] = actionMessage;
        //    }

        //    return Json(actionMessage);
        //}
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult LockUnLockUser(int userId, bool lockStatus)
        {
            var response = userMngtCaching.LockUnLockUser(userId, lockStatus);

            if (response == null)
                return Json(new ActionMessage(new CUDReturnMessage(ResponseCode.Error)));

            var actionMessage = new ActionMessage(response);

            if (response.Id == (int)ResponseCode.UserMngt_SuccessLock || response.Id == (int)ResponseCode.UserMngt_SuccessUnLock)
            {
                actionMessage.ID = 1;
                TempData[AdminGlobal.ActionMessage] = actionMessage;
            }

            return Json(actionMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteUser(int userId)
        {
            var response = userMngtCaching.DeleteUser(userId);

            if (response == null)
                return Json(new ActionMessage(new CUDReturnMessage(ResponseCode.Error)));

            var actionMessage = new ActionMessage(response);

            if (response.Id == (int)ResponseCode.UserMngt_SuccessDelete)
            {
                actionMessage.ID = 1;
                TempData[AdminGlobal.ActionMessage] = actionMessage;
            }

            return Json(actionMessage);
        }

        #endregion quản lý người dùng

        #region quản lý role & phân quyền

        public ActionResult ListRole()
        {
            var model = userMngtCaching.GetListRole(currentRoleSort: 0);
            ViewBag.ListGroupAction = systemMngtApi.GetListGroupAction(false, 0);
            return View(model);
        }

        public ActionResult CreateUpdateRole(int id = 0)
        {
            if (id == 0)
            {
                return View(new CreateUpdateRoleModel());
            }
            else
            {
                var listRole = userMngtCaching.GetListRole(currentRoleSort: 0);
                var role = listRole.Where(r => r.RoleId == id).FirstOrDefault();
                if (role == null) return View(new CreateUpdateRoleModel());

                var model = new CreateUpdateRoleModel
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    Sort = role.Sort
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateUpdateRole(CreateUpdateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var contract = new AdminRoleContract
                {
                    RoleId = model.RoleId,
                    RoleName = Sanitizer.GetSafeHtmlFragment(model.RoleName),
                    Sort = model.Sort
                };

                CUDReturnMessage response = systemMngtApi.InsertUpdateRole(contract);
                return CUDToJson(response, new List<int> { (int)ResponseCode.AdminRole_SuccessCreate, (int)ResponseCode.AdminRole_SuccessUpdate });
            }

            return Json(new ActionMessage(-1, MessageResource.Shared_ModelState_InValid));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRole(int id)
        {
            var response = systemMngtApi.DeleteRole(id);
            return CUDToJson(response, (int)ResponseCode.AdminRole_SuccessUpdate);
        }

        [HttpGet]
        public JsonResult UpdateRoleGroupActionMap(int roleId)
        {
            var model = systemMngtApi.GetListGroupAction(null, roleId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateRoleGroupActionMap(int roleId, List<int> listGroupId)
        {
            var contract = new InsertUpdateAdminRoleGroupActionContract
            {
                RId = roleId,
                GroupActions = listGroupId
            };

            var response = systemMngtApi.InsertUpdateRoleGroupAction(contract);
            return CUDToJson(response, new List<int> { (int)ResponseCode.AdminRoleGroupAction_SuccessUpdate });
        }

        #endregion quản lý role & phân quyền

        #region quản lý bộ phận
        public ActionResult LocationList()
        {
            var listLocation = locationApi.Get();
            var lstType = masterCaching.GetDepartmentListType();
            foreach (var item in listLocation)
            {
                foreach (var type in lstType)
                {
                    if (type.DepartmentTypeId == item.LayoutTypeId)
                    {
                        item.LayoutTypeName = type.DepartmentTypeName;
                    }
                }
            }

            return View(listLocation.OrderBy(x => x.NameEN).ToList());
        }

        [HttpGet]
        public ActionResult CreateUpdateLocation(int id = 0, int pid = 0)
        {
            CreateUpdateLocationModel model;
            //var questionGroups = questionApi.Get("", 
            //    new List<int>() { (int)Status.Active}, 
            //    new List<int>());
            var listDepartmentType = masterCaching.GetDepartmentListType();
            string parentName = "";

            if (id > 0)
            {
                var response = locationApi.Find(id);

                if (response == null)
                {
                    model = new CreateUpdateLocationModel()
                    {
                        LocationId = id,
                        ParentId = pid
                    };

                    ViewBag.ErrorMessage = MessageResource.Shared_SystemErrorMessage;
                }
                else
                {
                    pid = response.ParentId ?? 0;

                    if (pid > 0)
                    {
                        var parent = locationApi.Find(pid);
                        if (parent != null)
                        {
                            parentName = parent.NameVN;
                        }
                    }

                    model = new CreateUpdateLocationModel()
                    {
                        LocationId = id,
                        ParentId = response.ParentId ?? pid,
                        NameVN = response.NameVN,
                        NameEN = response.NameEN,
                        SloganVN = response.SloganVN,
                        SloganEN = response.SloganEN,
                        BackgroundName = response.BackgroundName,
                        LogoName = response.LogoName,
                        ColorCode = response.ColorCode,
                        LevelNo = response.LevelNo,
                        QuestionGroupId = response.QuestionGroupId ?? 0,
                        //QuestionGroups = questionGroups.List.Select(r => new SelectListItem { Value = r.QuestionGroupId.ToString(), Text = r.NameVN }).ToList(),
                        ParentName = parentName,
                        DepartmentTypeId = Convert.ToInt32(response.LayoutTypeId),
                        ListDepartmentType = listDepartmentType
                    };
                }
            }
            else
            {
                int levelNo = 1;
                if (pid > 0)
                {
                    var parent = locationApi.Find(pid);
                    if (parent != null)
                    {
                        parentName = parent.NameVN;
                        levelNo = parent.LevelNo + 1;
                    }
                }

                model = new CreateUpdateLocationModel()
                {
                    LocationId = id,
                    ParentId = pid,
                    NameVN = "",
                    NameEN = "",
                    SloganVN = "",
                    SloganEN = "",
                    BackgroundName = "",
                    LogoName = "",
                    ColorCode = "",
                    LevelNo = pid == 0 ? 1 : levelNo,//Tạo node root (k có pid) thì level = 1 ngược lại là level = parentLevel + 1
                    QuestionGroupId = 0,
                    ParentName = parentName,
                    //QuestionGroups = questionGroups.List.Select(r => new SelectListItem { Value = r.QuestionGroupId.ToString(), Text = r.NameVN }).ToList()
                    DepartmentTypeId = 0,
                    ListDepartmentType = listDepartmentType
                };
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteLocation(int id)
        {
            var response = locationApi.Delete(id, CurrentUserId);

            if (response == null)
                return Json(new CUDReturnMessage(ResponseCode.Error));
            else
            {
                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
            }
            return Json(response);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public JsonResult CreateUpdateLocation(CreateUpdateLocationModel model)
        {
            if (ModelState.IsValid)
            {
                LocationContract data = new LocationContract();
                data.LocationId = model.LocationId;
                data.NameVN = Sanitizer.GetSafeHtmlFragment(model.NameVN);
                data.NameEN = Sanitizer.GetSafeHtmlFragment(model.NameEN);
                data.SloganVN = Sanitizer.GetSafeHtmlFragment(model.SloganVN);
                data.SloganEN = Sanitizer.GetSafeHtmlFragment(model.SloganEN);
                data.LogoName = Sanitizer.GetSafeHtmlFragment(model.LogoName);
                data.BackgroundName = Sanitizer.GetSafeHtmlFragment(model.BackgroundName);
                data.ColorCode = Sanitizer.GetSafeHtmlFragment(model.ColorCode);
                data.ParentId = model.ParentId;
                data.LevelNo = model.LevelNo;
                //data.LevelPath         = model.LevelPath         ;
                //data.RootId            = model.RootId            ;
                data.LayoutTypeId = model.DepartmentTypeId;
                //data.LayoutTypeName    = model.LayoutTypeName    ;
                data.CreatedBy = CurrentUserId;
                //data.CreatedDate       = model.CreatedDate       ;
                data.LastUpdatedBy = CurrentUserId;
                //data.LastUpdatedDate   = model.LastUpdatedDate   ;
                //data.IsDeleted         = model.IsDeleted         ;
                //data.QuestionGroupId   = model.QuestionGroupId   ;
                //data.QuestionGroupName = model.QuestionGroupName;

                var response = locationApi.InsertUpdateLocation(data);

                if (response == null)
                {
                    response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                if (response.Id == (int)ResponseCode.LocationMngt_SuccessCreate
                        || response.Id == (int)ResponseCode.LocationMngt_SuccessUpdate)
                {
                    response.SystemMessage = "Shared_SaveSuccess";
                    TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                }
                else if (response.Id == (int)ResponseCode.LocationMngt_DuplicateCode)
                {
                    response.SystemMessage = "LocationMngt_DuplicateCode";
                }
                else if (response.Id == (int)ResponseCode.LocationMngt_DuplicateName)
                {
                    response.SystemMessage = "LocationMngt_DuplicateName";
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

        public ActionResult SetAccountOR(int userId = 0, string siteId = "", int sourceClientId = (int)SourceClientEnum.Oh)
        {
            //set default current location
            siteId = (string.IsNullOrEmpty(siteId)) ? memberExtendedInfo.CurrentLocaltion.NameEN : siteId;

            CreateUpdatedORModel model = new CreateUpdatedORModel();
            model.ListStates = new List<SelectListItem>() { new SelectListItem() { Value = "0", Text = "Kích hoạt" }, new SelectListItem() { Value = "1", Text = "Xóa" } };
            //model.ListPositions= EnumExtension.ToListOfValueAndDesc<ORPositionEnum>().Select(r => new SelectListItem
            // {
            //     Text = r.Description,
            //     Value = r.Value.ToString()
            // }).OrderByDescending(c => c.Value).ToList();
            model.ListPositions = userMngtCaching.GetPositions()
                                    .Where(x => x.ParentId == null)
                                    .Select(x => new SelectListItem
                                    {
                                        Text = x.GroupName,
                                        Value = x.Id.ToString()
                                    }).ToList();
            //model.ListPositions.Insert(0, new SelectListItem() { Value ="0", Text = "" });
            if (userId == 0) RedirectToAction("ListUser", "UserMngt");
            ORUserInfoContract userInfo = orService.GetORUserInfo(userId);
            model.UserId = userId;
            if (userInfo != null)
            {
                model.Username = userInfo.Name;
                model.Phone = string.IsNullOrEmpty(userInfo.Phone) ? " " : userInfo.Phone;
                model.Email = string.IsNullOrEmpty(userInfo.Email) ? " " : userInfo.Email;
                model.PositionId = userMngtCaching.GetPositionByUserId(model.UserId).Select(x => x.PositionId).ToList();
                model.StateId = userInfo.IsDeleted ? 1 : 0;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SetAccountOR(CreateUpdatedORModel model)
        {
            if (ModelState.IsValid) //&& model.PositionId != 0
            {
                var emailRegex = @"^[a-zA-Z]+[a-zA-Z0-9._-]+@[a-zA-Z0-9]{2,}(\.[a-zA-Z0-9]{2,4}){1,2}$";
                if (string.IsNullOrEmpty(model.Email))
                {
                    return Json(new ActionMessage(-1, $"Email chưa cung cấp"));
                }
                else
                {
                    Regex regex = new Regex(emailRegex);
                    Match match = regex.Match(model.Email);
                    if (!match.Success)
                        return Json(new ActionMessage(-1, $"Email sai định dạng"));
                }
                if (model.PositionId.Count <= 0 || model.PositionId.Contains(0))
                {
                    return Json(new ActionMessage(new CUDReturnMessage() { Id = -1, SystemMessage = "CMS_OR_NotPosition" }));
                }

                var data = new ORUserInfoContract()
                {
                    Id = model.UserId,
                    Name = Sanitizer.GetSafeHtmlFragment(model.Username),
                    Email = Sanitizer.GetSafeHtmlFragment(model.Email),
                    Phone = Sanitizer.GetSafeHtmlFragment(model.Phone),
                    //PositionId=model.PositionId,
                    IsDeleted = model.StateId == 0 ? false : true,
                };
                var response = userMngtCaching.CreateUpdateORUserInfo(data, CurrentUserId);

                if (response == null)
                {
                    response = new CUDReturnMessage() { Id = -1, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                else if (response.Id == (int)ResponseCode.UserMngt_SuccessUpdated || response.Id == (int)ResponseCode.UserMngt_SuccessCreated)
                {
                    var resUserInfoPosition = userMngtCaching.CreateUpdateUserInfoPosition(data.Id, model.PositionId);
                    var msg = new ActionMessage(1, response.SystemMessage);
                    //TempData[AdminGlobal.ActionMessage] = msg;
                    return Json(msg);
                }
                else if (response.Id > 0)
                {
                    return Json(new ActionMessage(response));
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
            var cudMsg = new CUDReturnMessage() { Id = -1, SystemMessage = "CMS_OR_NotPosition" };
            return Json(new ActionMessage(cudMsg));
        }

        #region linhht lấy dữ liệu từ AD update vào DB
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult GetUInfoFromAD(CreateUpdateUserModel model)
        {
            var adInfo = VinAuthentication.GetADUserInfo(model.Username, DomainName);
            if (adInfo == null)
            {
                CUDReturnMessage res = new CUDReturnMessage();
                res.Id = (int)ResponseCode.UserMngt_UsernameNotExisted;
                res.SystemMessage = "UserMngt_UsernameNotExisted";
                res.Message = StringUtil.GetResourceString(typeof(MessageResource), res.SystemMessage);
                //return Json(res);
                return Json(new ResponseBase()
                {
                    Status = "ERR",
                    Type = "ERR",
                    Message = StringUtil.GetResourceString(typeof(MessageResource), res.SystemMessage),
                });
            }

            string email = adInfo.Properties["mail"].Value != null ? adInfo.Properties["mail"].Value.ToString() : "";

            //if (!string.IsNullOrEmpty(model.Email))
            //{
            //    Regex regex = new Regex(emailRegex);
            //    Match match = regex.Match(model.Email);
            //    if (!match.Success)
            //        //return Json(new ActionMessage(-1, $"Email sai định dạng"));
            //        return Json(new ResponseBase()
            //        {
            //            Status = "ERR",
            //            Type = "ERR",
            //            Message = StringUtil.GetResourceString(typeof(MessageResource), res.SystemMessage),
            //        });
            //}
            string telephoneNumber = adInfo.Properties["telephoneNumber"].Value != null ? adInfo.Properties["telephoneNumber"].Value.ToString() : "";
            string displayName = adInfo.Properties["displayName"].Value != null ? adInfo.Properties["displayName"].Value.ToString() : "";

            var user = new CreateUpdateUserContract
            {
                Username = Sanitizer.GetSafeHtmlFragment(model.Username),
                IsADAccount = true,
                UserId = model.UserId,
                Email =  email,
                PhoneNumber = telephoneNumber,
                Fullname = displayName,
                Roles = model.RoleId,
                MicroSites = model.MicroSite,
                DeptId = model.DeptId,
                Locations = model.Location.Split(',').Select(r => int.Parse(r)).ToList(),
                LineManagerId = model.LineManagerUser
            };

            return Json(new ResponseBase()
            {
                Status = "success",
                Type = "success",
                Message = "success",
                data = user
            });

        }
        #endregion
    }
}