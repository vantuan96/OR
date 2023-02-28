using System;
using Caching.Core;
using Contract.Shared;
using Admin.Helper;
using Admin.Models.Shared;
using Admin.Resource;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contract.CheckList;
using Admin.Models.CheckList;
using VG.Common;
using Contract.Enum;
using System.Data;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.IO;
using Contract.MasterData;
using System.Configuration;
using Admin.SMTPMail;

namespace Admin.Controllers
{
    [CheckUserCaching]
    public class CheckListController : BaseController
    {
        private int defaultPageSize = 10;
        private readonly ICheckListCaching checkListCaching;
        private readonly ISystemSettingCaching systemSettingApi;
        private readonly ISystemCaching systemMngtApi;
        private readonly IMasterCaching masterCaching;
        public CheckListController(IAuthenCaching authenCaching, ICheckListCaching checkListCaching, ISystemCaching systemMngtApi, ISystemSettingCaching systemSettingApi, IMasterCaching masterCaching) : base(authenCaching, systemSettingApi)
        {
            this.checkListCaching = checkListCaching;
            this.systemSettingApi = systemSettingApi;
            this.systemMngtApi = systemMngtApi;
            this.masterCaching = masterCaching;

            defaultPageSize = AdminConfiguration.Paging_PageSize;
        }

        #region quản lý checklist
        public ActionResult CreateUpdateCheckList(int CheckListId = 0)
        {
            DateTime dt = DateTime.Now;
            var model = new CreateUpdateCheckListModel();
            model.CLItemIds = new List<int>() { };
            
            
            model.SetupDateFrom = dt;

            #region master data
            List<SelectListItem> listStates = new List<SelectListItem>();
            List<SelectListItem> listSystems = new List<SelectListItem>();
            List<SelectListItem> listCheckListTypes = new List<SelectListItem>();
            List<SelectListItem> listClItems = new List<SelectListItem>();
            #region state
            listStates =
                EnumExtension.ToListOfValueAndDesc<SystemStateEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString()
                }).OrderByDescending(c => c.Value).ToList();
            #endregion state
            #region system

            var dataSystems = masterCaching.GetSystemCheckList(CurrentUserId,(int)SystemStatusEnum.Active, string.Empty, 1, 1000, 0, false);
            if (dataSystems != null && dataSystems.Data.Any())

                listSystems = dataSystems.Data.Select(c => new SelectListItem
                {
                    Text = c.SystemName,
                    Value = c.SystemId.ToString()
                }).ToList();
            #endregion system

            #region checkListTypeId

            listCheckListTypes =
              EnumExtension.ToListOfValueAndDesc<CheckListTypeEnum>().Select(r => new SelectListItem
              {
                  Text = r.Description,
                  Value = r.Value.ToString()
              }).ToList();

            #endregion checkListTypeId

            #region items

            var dataItems = checkListCaching.GetItem((int)SystemStateEnum.Active, string.Empty, 1, 1000, 0);
            if (dataItems != null && dataItems.Data.Any())

                listClItems = dataItems.Data.Select(c => new SelectListItem
                {
                    Text = c.ItemName,
                    Value = c.CLItemId.ToString()
                }).ToList();
            #endregion items
            #endregion
            
            if (CheckListId>0)
            {
                var listData = checkListCaching.ListCheckList(0,0,0,CheckListId,string.Empty,1,1);
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    var data = listData.Data.FirstOrDefault();
                    if (data != null)
                    {
                        model.CheckListId = data.CheckListId;
                        model.CheckListName = data.CheckListName;
                        model.Description = data.Description;
                        model.Visible = data.Visible;
                        model.CreatedBy = data.CreatedBy;
                        model.CreateDate = data.CreatedDate;
                        model.UpdatedBy = data.UpdatedBy;
                        model.CreateName = data.CreateName;
                        model.State = data.Visible?1:0;
                        model.CheckListTypeId = data.CheckListTypeId;
                        model.SystemId = data.SystemId;
                        model.CLItemIds = data.lstItemIds ;
                        model.SetupDateFrom = data.SetupDateFrom;
                    }
                }
            }
            model.listStates = listStates;
            model.listSystems = listSystems;
            model.listClItems = listClItems;
            model.listCheckListTypes = listCheckListTypes;
            return View(model);           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateUpdateCheckList(CreateUpdateCheckListModel model)
        {
            if (ModelState.IsValid)
            {
                var contract = new CheckListContract
                {
                    CheckListId = model.CheckListId,
                    CheckListName = model.CheckListName,
                    Description = model.Description,
                    Visible = model.State==1,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    Priority = model.Priority,
                    CheckListTypeId = model.CheckListTypeId,
                    SystemId = model.SystemId,
                    lstItemIds = model.CLItemIds,
                    SetupDateFrom = model.SetupDateFrom,

            };

            CUDReturnMessage response = checkListCaching.InsertUpdateCheckList(contract,CurrentUserId);
          
            var  actionMessage = new ActionMessage(response);
            TempData[AdminGlobal.ActionMessage] = actionMessage;
            return Json(actionMessage);
            }

            return Json(new ActionMessage(-1, MessageResource.Shared_ModelState_InValid));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ActiveCheckList(int CheckListId)
        {
            var response = checkListCaching.ActiveCheckList(CheckListId,CurrentUserId);
            return Json(new ActionMessage(response));
        }

        #endregion quản lý checklist

        #region Check List Item

        public ActionResult ListCheckListItem()
        {
            var model = checkListCaching.GetCheckListIitem();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCheckListDetail(int id)
        {
            var response = checkListCaching.DeleteCheckListItem(id,CurrentUserId);
            return CUDToJson(response, (int)ResponseCode.CheckListDetail_SuccessDelete);
        }

        public ActionResult CreateUpdateCheckListDetail(int id = 0)
        {
            var model = new CreateUpdateCheckListDetailModel();

            #region tạo ddl
            var checkList = checkListCaching.GetListCheckList(0);
            #endregion tạo ddl

            if (id > 0)
            {
                var checkListItem = checkListCaching.GetCheckListIitem();
                if (checkListItem != null && checkListItem.Exists(m => m.CheckListDetailId == id))
                {
                    model.Id = checkListItem.Single(m => m.CheckListDetailId == id).CheckListDetailId;
                    model.CheckListDetailId = checkListItem.Single(m => m.CheckListDetailId == id).CheckListDetailId;
                    model.CheckListDetailName = checkListItem.Single(m => m.CheckListDetailId == id).CheckListDetailName;
                    model.CheckListId = checkListItem.Single(m => m.CheckListDetailId == id).CheckListId;
                    model.Description = checkListItem.Single(m => m.CheckListDetailId == id).Description;
                    model.Priority = checkListItem.Single(m => m.CheckListDetailId == id).Priority;
                    model.ListCheckList = checkList;
                }
            }
            else
            {
                model.Id = 0;
                model.CheckListDetailId = id;
                model.CheckListDetailName = "";
                model.CheckListId = 0;
                model.Description = "";
                model.Priority = 1;
                model.ListCheckList = checkList;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateCheckListDetail(CreateUpdateCheckListDetailModel model)
        {
            if (ModelState.IsValid)
            {
                var msContent = new CheckListItemContract()
                {
                    CheckListDetailId = model.CheckListDetailId,
                    CheckListDetailName = model.CheckListDetailName,
                    CheckListId = model.CheckListId,
                    Description = model.Description,
                    Priority = model.Priority,
                    Visible = true,
                    CreatedBy = CurrentUserId,
                    UpdatedBy = CurrentUserId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };

                var response = checkListCaching.InsertUpdateCheckListItem(msContent,CurrentUserId);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.CheckListDetail_SuccessCreate)
                    {
                        response.SystemMessage = "CheckListDetail_SuccessCreate";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                    }
                    else if (response.Id == (int)ResponseCode.CheckListDetail_SuccessUpdate)
                    {
                        response.SystemMessage = "CheckListDetail_SuccessUpdate";
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


        #region Item

        public ActionResult GetItemCheckList(int state = 0, string kw = "", int p = 1, int ps = 0, Boolean export = false)
        {
            var listStates = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (export)
            {
                var pageSize = int.MaxValue;
                p = 1;
                var listData = checkListCaching.GetItem(state, kw, p, ps, 0);
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    return ExportTemplate(MakeItemContentExcell(listData.Data), LayoutResource.AdminTools_Item_ExportExcel_TitleFile,
                  "danh-sach-hang-muc-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
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

                var data = checkListCaching.GetItem(state, kw, p, ps, 0);

                var model = new ListItemModel()
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
        private DataTable MakeItemContentExcell(List<ItemContract> list)
        {
            var datatable = new DataTable("tblData");
            #region group header content
            datatable.Columns.Add(new DataColumn(LayoutResource.Shared_Label_SortNumber));
            datatable.Columns.Add(new DataColumn(LayoutResource.Master_ItemCheckList_ItemName));
            datatable.Columns.Add(new DataColumn(LayoutResource.Master_ItemCheckList_Description));
            datatable.Columns.Add(new DataColumn(LayoutResource.Master_ItemCheckList_CreatedDate));
            datatable.Columns.Add(new DataColumn(LayoutResource.Master_ItemCheckList_State));
            #endregion
            int i = 0;
            foreach (var detail in list)
            {
                var row = datatable.NewRow();
                i++;
                //content
                row[LayoutResource.Shared_Label_SortNumber] = i;
                row[LayoutResource.Master_ItemCheckList_ItemName] = detail.ItemName;
                row[LayoutResource.Master_ItemCheckList_Description] = detail.Description;
                row[LayoutResource.Master_ItemCheckList_CreatedDate] = detail.CreatedDate;
                row[LayoutResource.Master_ItemCheckList_State] = detail.StateName;
                datatable.Rows.Add(row);
            }
            return datatable;
        }
        #endregion


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteItem(int ClItemId)
        {
            CUDReturnMessage result;
            if (ClItemId > 0)
            {
                result = checkListCaching.DeleteItem(ClItemId,CurrentUserId);
            }
            else
            {
                result = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
            }
            return Json(new ActionMessage(result));
        }

        /// <summary>
        /// Them,chinh sua thong tin hang muc 
        /// </summary>
        /// <param name="TemplateId"></param>
        /// <returns></returns>
        public ActionResult InsertUpdateItem(int ClItemId = 0)
        {
            var model = new InsertUpdateItemModel();
            var listStates = new List<SelectListItem>();

            if (ClItemId > 0)
            {
                var infoSystem = checkListCaching.GetItem(0, string.Empty, 1, 20, ClItemId).Data.FirstOrDefault();
                if (infoSystem != null)
                {
                    model.State = infoSystem.Visible ? (int)SystemStateEnum.Active : (int)SystemStateEnum.NoActive;
                    model.ClItemId = infoSystem.CLItemId;
                    model.ItemName = infoSystem.ItemName;
                    model.Description = infoSystem.Description;
                    model.Sort = infoSystem.Sort;
                }
            }
            #region source client
            listStates =
                EnumExtension.ToListOfValueAndDesc<SystemStateEnum>().Select(r => new SelectListItem
                {
                    Text = r.Description,
                    Value = r.Value.ToString(),
                    Selected = (r.Value == model.State)
                }).ToList();
            #endregion source client
            model.listStates = listStates;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertUpdateItemByAjax(InsertUpdateItemModel model)
        {
            if (ModelState.IsValid || (model.ClItemId > 0))
            {

                var response = new CUDReturnMessage();
                ItemContract param = lazyMapper.Value.Map<ItemContract>(model);
                param.Visible = model.State == (int)SystemStateEnum.Active ? true : false;
                response = checkListCaching.InsertUpdateItem(param,CurrentUserId);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.ItemMngt_SuccessCreated || response.Id == (int)ResponseCode.ItemMngt_SuccessUpdated)
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

        #region new checklist
        public ActionResult ListCheckList(int state = 0,int systemId=0,int checkListTypeId=0,string kw = "", int p = 1, int ps = 0, Boolean export = false)
        {
            var listStates = new List<SelectListItem>();
            var listCheckListTypes = new List<SelectListItem>();
            var listSystems = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (export)
            {
                var listData = checkListCaching.ListCheckList(state, systemId, checkListTypeId, 0, kw, 1, int.MaxValue);
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    return ExportTemplate(MakeCheckListContentExcell(listData.Data), LayoutResource.AdminTools_CheckList_ExportExcel_TitleFile,
                  "danh-sach-check-list-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
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

                #region system
                
                var dataSystems= masterCaching.GetSystemCheckList(CurrentUserId, (int)SystemStateEnum.Active, string.Empty,1,1000,0,false);
                if (dataSystems != null && dataSystems.Data.Any())

                    listSystems = dataSystems.Data.Select(c => new SelectListItem
                    {
                        Text = c.SystemName,
                        Value = c.SystemId.ToString()
                    }).ToList();

                listSystems.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
                #endregion system

                #region checkListTypeId

                listCheckListTypes =
                  EnumExtension.ToListOfValueAndDesc<CheckListTypeEnum>().Select(r => new SelectListItem
                  {
                      Text = r.Description,
                      Value = r.Value.ToString()
                  }).ToList();
                listCheckListTypes.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);

                #endregion checkListTypeId
                var data = checkListCaching.ListCheckList(state,systemId,checkListTypeId,0,kw, p, ps);
                var model = new SearchCheckListModel()
                {
                    listData = data.Data,
                    TotalCount = data.TotalRows,
                    PageCount = ps,
                    PageNumber = p,
                    listStates = listStates,
                    state = state,
                    kw = kw,
                    checkListTypeId=checkListTypeId,
                    listCheckListTypes= listCheckListTypes,
                    systemId=systemId,
                    listSystems= listSystems
                };
                return View(model);
            }


        }


        private DataTable MakeCheckListContentExcell(List<CheckListContract> list)
        {
            var datatable = new DataTable("tblData");
            #region group header content
            datatable.Columns.Add(new DataColumn(LayoutResource.Shared_Label_SortNumber));
            datatable.Columns.Add(new DataColumn(LayoutResource.CheckList_Label_Title_Name));
            datatable.Columns.Add(new DataColumn(LayoutResource.CheckList_Label_Title_Description));
            datatable.Columns.Add(new DataColumn(LayoutResource.CheckList_Label_Title_CreateDate));
            datatable.Columns.Add(new DataColumn(LayoutResource.Share_State_Lable_StateName));
            #endregion
            int i = 0;
            foreach (var detail in list)
            {
                var row = datatable.NewRow();
                i++;
                //content
                row[LayoutResource.Shared_Label_SortNumber] = i;
                row[LayoutResource.CheckList_Label_Title_Name] = detail.CheckListName;
                row[LayoutResource.CheckList_Label_Title_Description] = detail.Description;
                row[LayoutResource.CheckList_Label_Title_CreateDate] = detail.CreatedDate;
                row[LayoutResource.Share_State_Lable_StateName] = detail.StateName;
                datatable.Rows.Add(row);
            }
            return datatable;
        }
        #endregion
    }
}