using Admin.Helper;
using Admin.Models.CheckList;
using Admin.Models.Operation;
using Admin.Models.Shared;
using Admin.Resource;
using Admin.SMTPMail;
using Caching.Core;
using Contract.Enum;
using Contract.Log;
using Contract.MasterData;
using Contract.OperationCheckList;
using Contract.Shared;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Controllers
{
    [CheckUserCaching]
    public class OperationCheckListController : BaseController
    {

        private int defaultPageSize = 10;
        private readonly IOperationCheckListCaching checkListCaching;
        private readonly ISystemSettingCaching systemSettingApi;
        private readonly ISystemCaching systemMngtApi;
        private readonly IMasterCaching masterCaching;
        private readonly ILogObjectCaching logCaching;
        public OperationCheckListController(IAuthenCaching authenCaching, IOperationCheckListCaching checkListCaching, ISystemCaching systemMngtApi, ISystemSettingCaching systemSettingApi, IMasterCaching masterCaching, ILogObjectCaching logCaching) : base(authenCaching, systemSettingApi)
        {
            this.checkListCaching = checkListCaching;
            this.systemSettingApi = systemSettingApi;
            this.systemMngtApi = systemMngtApi;
            this.masterCaching = masterCaching;
            this.logCaching = logCaching;
            defaultPageSize = AdminConfiguration.Paging_PageSize;
        }
        
        public ActionResult ListOperationCheckList(int state = 0, int systemId = 0, int checkListTypeId = 0, string kw = "", int p = 1, int ps = 0, Boolean export = false)
        {
            var listStates = new List<SelectListItem>();
            var listCheckListTypes = new List<SelectListItem>();
            var listSystems = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (export)
            {

                var listData = checkListCaching.ListCheckList(state, systemId, checkListTypeId, 0, kw, 1, int.MaxValue,CurrentUserId,IsPermissionAprove:false);
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    return ExportTemplate(MakeCheckListContentExcell(listData.Data), LayoutResource.AdminTools_CheckList_ExportExcel_TitleFile,
                            "danh-sach-operation-check-list-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                }
                return null;
            }
            else
            {
                #region state
                listStates =
                    EnumExtension.ToListOfValueAndDesc<CheckListStateEnum>().Select(r => new SelectListItem
                    {
                        Text = r.Description,
                        Value = r.Value.ToString()
                    }).ToList();
                listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
                #endregion state

                #region system

                var dataSystems = masterCaching.GetSystemCheckList(CurrentUserId, (int)SystemStateEnum.Active, string.Empty, 1, 1000, 0, true);
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
                var data = checkListCaching.ListCheckList(state, systemId, checkListTypeId, 0, kw, p, ps,CurrentUserId, IsPermissionAprove: false);
                var model = new OperationSearchCheckListModel()
                {
                    listData = data.Data,
                    TotalCount = data.TotalRows,
                    PageCount = ps,
                    PageNumber = p,
                    listStates = listStates,
                    state = state,
                    kw = kw,
                    checkListTypeId = checkListTypeId,
                    listCheckListTypes = listCheckListTypes,
                    systemId = systemId,
                    listSystems = listSystems
                };
                return View(model);
            }
        }


        private DataTable MakeCheckListContentExcell(List<OperationCheckListContract> list)
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

        public ActionResult UpdateOperationCheckList(long InstanceId)
        {
            DateTime dt = DateTime.Now;
            var model = new UpdateOperationCheckListModel();

            if (InstanceId > 0)
            {
                var listData = checkListCaching.ListCheckList(0, 0, 0,InstanceId, string.Empty, 1,10,CurrentUserId, IsPermissionAprove: false);
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    var data = listData.Data.FirstOrDefault();
                    if (data != null)
                    {
                        model.CheckListId = data.CheckListId;
                        model.InstanceId = data.InstanceId;
                        model.SystemName = data.SystemName;
                        model.CheckListName = data.CheckListName;
                        model.CheckListTypeId = data.CheckListTypeId;
                        model.CheckListTypeName = data.CheckListTypeName;
                        model.Description = data.Description;
                        model.State = data.CheckListStatusId;
                        model.SystemId = data.SystemId;
                        model.DeadLine = data.Deadline;
                        model.listClItems = data.Items.Select(c => new UpdateItemModel()
                        {
                            ClItemId = c.CLItemId,
                            ItemName = c.ItemName,
                            Description = c.Description,
                            State = c.State,
                            Comment = c.Comment,
                            Sort = c.Sort
                          
                        }).ToList();
                    
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveOperationCheckListDetail(List<UpdateItemModel> listClItems)
        {
            if (ModelState.IsValid)
            {

                List<UpdateItemContract> param = listClItems.Select(c => lazyMapper.Value.Map<UpdateItemContract>(c)).ToList();
                var response = checkListCaching.SaveOperationCheckListDetail(param, CurrentUserId);

                if (response.Id == (int)ResponseCode.OperationCheckList_UpdateSuccess)
                {
                    response.SystemMessage = "Shared_SaveSuccess";
                    TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult RequestApproveCheckList(UpdateOperationCheckListContract info)
        {
            info.State = (int)CheckListStateEnum.Execute;
            var response = checkListCaching.ChangeStateOperationCheckList(info, CurrentUserId);
            if (response.Id == (int)ResponseCode.OperationCheckList_ChangeStatus)
            {
                var contentMail = string.Format("Dear Anh/Chị Quản lý. <br/> Hiện tại checklist {0} đã được thực hiện hoàn tất , kính nhờ anh/chị quản lý kiểm tra và xác nhận hoàn thành .<br/> Trân trọng", info.Comment);
                SendMail(LineManagerEmail, string.Empty, string.Empty, "Xin phê duyệt hoàn thành checklist", contentMail);

                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                var message = new ActionMessage(response);
               return Json(response);               
            }
            else
            {
                response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
            }
            response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
            return Json(response);
        }

        #region line manager

        public ActionResult ListApproveCheckList(int state = 0, int systemId = 0, int checkListTypeId = 0, string kw = "", int p = 1, int ps = 0, Boolean export = false)
        {
            state = (int)CheckListStateEnum.Execute;
            var listStates = new List<SelectListItem>();
            var listCheckListTypes = new List<SelectListItem>();
            var listSystems = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (export)
            {
                var listData = checkListCaching.ListCheckList((int)CheckListStateEnum.Execute, systemId, checkListTypeId, 0, kw, 1, int.MaxValue, CurrentUserId, IsPermissionAprove:true);
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    return ExportTemplate(MakeCheckListContentExcell(listData.Data), LayoutResource.AdminTools_CheckList_ExportExcel_TitleFile,
                            "danh-sach-check-list-can-approve" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                }
                return null;
            }
            else
            {
                //#region state
                //listStates =
                //    EnumExtension.ToListOfValueAndDesc<CheckListStateEnum>().Select(r => new SelectListItem
                //    {
                //        Text = r.Description,
                //        Value = r.Value.ToString()
                //    }).ToList();
                //listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
                //#endregion state

                #region system

                var dataSystems = masterCaching.GetSystemCheckList(CurrentUserId, (int)SystemStateEnum.Active, string.Empty, 1, 1000, 0, true);
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
                var data = checkListCaching.ListCheckList((int)CheckListStateEnum.Execute, systemId, checkListTypeId, 0, kw, p, ps, CurrentUserId, IsPermissionAprove:true);
                var model = new OperationSearchCheckListModel()
                {
                    listData = data.Data,
                    TotalCount = data.TotalRows,
                    PageCount = ps,
                    PageNumber = p,
                    listStates = listStates,
                    state = state,
                    kw = kw,
                    checkListTypeId = checkListTypeId,
                    listCheckListTypes = listCheckListTypes,
                    systemId = systemId,
                    listSystems = listSystems
                };
                return View(model);
            }
        }
        /// <summary>
        /// manager xem thong tin checklist da thuc hien
        /// </summary>
        /// <param name="InstanceId"></param>
        /// <returns></returns>
        public ActionResult ViewCheckListDetail(long InstanceId)
        {
            DateTime dt = DateTime.Now;
            var model = new UpdateOperationCheckListModel();

            if (InstanceId > 0)
            {
                var listData = checkListCaching.ListCheckList(0, 0, 0, InstanceId, string.Empty, 1, 10, CurrentUserId, IsPermissionAprove: false);
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    var data = listData.Data.FirstOrDefault();
                    if (data != null)
                    {
                        model.CheckListId = data.CheckListId;
                        model.InstanceId = data.InstanceId;
                        model.SystemName = data.SystemName;
                        model.CheckListName = data.CheckListName;
                        model.CheckListTypeId = data.CheckListTypeId;
                        model.CheckListTypeName = data.CheckListTypeName;
                        model.Description = data.Description;
                        model.State = data.CheckListStatusId;
                        model.SystemId = data.SystemId;
                        model.DeadLine = data.Deadline;
                        model.OwnerEmail = data.OwnerEmail;
                        model.listClItems = data.Items.Select(c => new UpdateItemModel()
                        {
                            ClItemId = c.CLItemId,
                            ItemName = c.ItemName,
                            Description = c.Description,
                            State = c.State,
                            Comment = c.Comment,
                            Sort = c.Sort

                        }).ToList();

                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveApproveCheckListDetail(string InstanceId,int TypeId,string Description,string OwnerEmail,string CheckListName)
        {
            var info = new UpdateOperationCheckListContract();
            info.InstanceId = Int64.Parse(InstanceId);
            info.State=TypeId==1 ? (int)CheckListStateEnum.ApproveOk:(int)CheckListStateEnum.Reject;
            info.Description = Description;
            var response = checkListCaching.ChangeStateOperationCheckList(info, CurrentUserId);
            if (response.Id == (int)ResponseCode.OperationCheckList_ChangeStatus)
            {
                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                var message = new ActionMessage(response);
                TempData[AdminGlobal.ActionMessage] = message;

                var contentMail = string.Format("Dear Anh/chị . <br/> Hiện tại checklist {0} đã bị từ chối, cần thực hiện lại,anh/chị vui lòng kiểm tra/ thực hiện lại .<br/> Trân trọng", CheckListName);
                SendMail(OwnerEmail, string.Empty, string.Empty, "Checklist bị không được duyệt", contentMail);

                return Json(response);
            }
            else
            {
                response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
            }
            response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
            return Json(response);
        }




        #endregion

        #region history checklist

        public ActionResult HistoryCheckList(int state = 0, int systemId = 0, int checkListTypeId = 0, string kw = "", int p = 1, int ps = 0, Boolean export = false)
        {
            var listStates = new List<SelectListItem>();
            var listCheckListTypes = new List<SelectListItem>();
            var listSystems = new List<SelectListItem>();
            if (ps == 0) ps = AdminConfiguration.Paging_PageSize;
            if (export)
            {
                var listData = checkListCaching.ListCheckList(state, systemId, checkListTypeId, 0, kw, 1, int.MaxValue, 0, IsPermissionAprove: true);
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    return ExportTemplate(MakeCheckListContentExcell(listData.Data), LayoutResource.AdminTools_CheckList_ExportExcel_TitleFile,
                            "danh-sach-check-list-can-tim-kiem" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss"));
                }
                return null;
            }
            else
            {
                #region state
                listStates =
                    EnumExtension.ToListOfValueAndDesc<CheckListStateEnum>().Select(r => new SelectListItem
                    {
                        Text = r.Description,
                        Value = r.Value.ToString()
                    }).ToList();
                listStates.Insert(0, AdminGlobal.DefaultValue.DefaultSelectListItem);
                #endregion state

                #region system

                var dataSystems = masterCaching.GetSystemCheckList(0,state, string.Empty, 1, 1000, 0, true);
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
                var data = checkListCaching.ListCheckList(state, systemId, checkListTypeId, 0, kw, p, ps, 0, IsPermissionAprove: true);//
                var model = new OperationSearchCheckListModel()
                {
                    listData = data.Data,
                    TotalCount = data.TotalRows,
                    PageCount = ps,
                    PageNumber = p,
                    listStates = listStates,
                    state = state,
                    kw = kw,
                    checkListTypeId = checkListTypeId,
                    listCheckListTypes = listCheckListTypes,
                    systemId = systemId,
                    listSystems = listSystems
                };
                return View(model);
            }
        }
     
        /// <summary>
        /// Xem thông tin lịch sử thay đổi checklist
        /// </summary>
        /// <param name="InstanceId"></param>
        /// <returns></returns>
        public ActionResult ViewDetailHistory(long InstanceId)
        {
            DateTime dt = DateTime.Now;
            var model = new ViewHistoryLogModel();

            if (InstanceId > 0)
            {
                var listData = checkListCaching.ListCheckList(0, 0, 0, InstanceId, string.Empty, 1, 10, CurrentUserId, IsPermissionAprove: false);

                var listDataChange = logCaching.FindLog(new SearchLogParam()
                {
                    ObjectId=InstanceId,
                    ObjectTypeId=ObjectTypeEnum.CheckList
                });
                if (listData != null && listData.Data != null && listData.Data.Any())
                {
                    var data = listData.Data.FirstOrDefault();
                    if (data != null)
                    {
                        model.CheckListId = data.CheckListId;
                        model.InstanceId = data.InstanceId;
                        model.SystemName = data.SystemName;
                        model.CheckListName = data.CheckListName;
                        model.CheckListTypeId = data.CheckListTypeId;
                        model.CheckListTypeName = data.CheckListTypeName;
                        model.Description = data.Description;
                        model.State = data.CheckListStatusId;
                        model.SystemId = data.SystemId;
                        model.DeadLine = data.Deadline;
                        model.listClItems = data.Items.Select(c => new UpdateItemModel()
                        {
                            ClItemId = c.CLItemId,
                            ItemName = c.ItemName,
                            Description = c.Description,
                            State = c.State,
                            Comment = c.Comment,
                            Sort = c.Sort

                        }).ToList();
                        model.LogChange = listDataChange;
                    }
                }
            }
        
            return View(model);
        }

        #endregion

        #region smtp mail
        private void SendMail(string toEmail,string ccMail,string bccMail, string subJect, string Description)
        {
            try
            {
                Boolean IsConfigSendMail = Boolean.Parse(ConfigurationManager.AppSettings["IsConfigSendMail"]!=null?ConfigurationManager.AppSettings["IsConfigSendMail"].ToString():"false");
                if (!IsConfigSendMail || string.IsNullOrEmpty(toEmail)) return;
                string AlliasCompanyName = ConfigurationManager.AppSettings["AlliasCompanyName"].ToString();
                MailCore info = new MailCore();
                info.HtmlBody = Description;
                info.FromAddress =new FromEmail().getNoReplyEmail();
                info.Subject = subJect;
                if (!String.IsNullOrEmpty(toEmail))
                {
                    info.Recipients = toEmail.Split(',').ToList();
                }
                info.FileAttachments = new List<FileAttachment>() { };
                info.FromName = AlliasCompanyName;
                if (!String.IsNullOrEmpty(ccMail))
                {
                    info.CC = ccMail.Split(',').ToList();
                }
                if (!String.IsNullOrEmpty(bccMail))
                {
                    info.BCC = bccMail.Split(',').ToList();
                }
                #region "Load File Attach"
                try
                {
                    string attachContent = string.Empty;
                    string jsonAttachFile = String.IsNullOrEmpty(attachContent)
                        ? "[]"
                        : attachContent;
                    info.FileAttachments =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileAttachment>>(jsonAttachFile);
                }
                catch (Exception) { }
                #endregion
                
                var resultMail = new SMTPMailClient().SendSMTPMail(info);               
            }
            catch (Exception ex)
            {               
            }
        }

    

        #endregion
    }
}