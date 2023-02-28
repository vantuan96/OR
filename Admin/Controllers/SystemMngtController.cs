using Caching.Core;
using Contract.AdminAction;
using Contract.Core;
using Contract.Shared;
using Contract.SystemLog;
using Contract.SystemSetting;
using Admin.Helper;
using Admin.Models.Shared;
using Admin.Models.SystemMngt;
using Admin.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using VG.Common;
using System.DirectoryServices.AccountManagement;
using VG.EncryptLib.EncryptLib;

namespace Admin.Controllers
{
    [CheckUserCaching]
    public class SystemMngtController : BaseController
    {
        private readonly ISystemSettingCaching _systemApi;
        private readonly ILogCaching _logApi;
        private readonly ISystemCaching systemMngtApi;

        public SystemMngtController(
            IAuthenCaching authenApi,
            ISystemSettingCaching systemApi,
            ILogCaching logApi,
            ISystemCaching systemMngtApi
        ) : base(authenApi, systemApi)
        {
            this._systemApi = systemApi;
            this._logApi = logApi;
            this.systemMngtApi = systemMngtApi;
        }

        #region Logging

        /// <summary>
        /// Xem log lỗi trên file của hệ thống
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="logLocation">The log location.</param>
        /// <param name="isDelete">The is delete.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewLogClient(int p = 1, int isDelete = 0, string folderName = "", string fileName = "")
        {
            var vm = new ViewLogClientVM();
            var path = Common.SettingValues.LoggingDir;
            var logFilename = string.Empty;
            var logLocation = FileLogLocation.Presentation;

            if (!string.IsNullOrEmpty(fileName))
            {
                Regex reg = new Regex(@"[\.\\/]*");
                fileName = reg.Replace(fileName, "");
                folderName = reg.Replace(folderName, "");
                logFilename = path + "\\" + fileName + ".log";
            }

            if (isDelete.Equals(1) && !string.IsNullOrEmpty(fileName))
            {
                var msg = new CUDReturnMessage();

                switch (logLocation)
                {
                    case FileLogLocation.Presentation:
                        {
                            if (System.IO.File.Exists(logFilename))
                            {
                                System.IO.File.Delete(logFilename);
                                msg.Id = 1;
                                msg.Message = "Xóa thành công";
                            }
                            else
                            {
                                msg.Id = 0;
                                msg.Message = "Xóa thất bại";
                            }
                        }
                        break;
                }

                TempData["AdminErrorLog_Alert"] = msg;

                return RedirectToAction("ViewLogClient", new { p = 1, logLocation = logLocation });
            }
            else
            {
                int pageCount = 10;

                int skipRecord = (p - 1) * pageCount;

                switch (logLocation)
                {
                    case FileLogLocation.Presentation:
                        {
                            if (!path.IsNullOrEmpty() && System.IO.Directory.Exists(path))
                            {
                                DirectoryInfo folderInfo = new DirectoryInfo(path);
                                var result = folderInfo.GetFiles("*.log").OrderByDescending(x => x.CreationTime).Select(x => x.FullName);
                                vm.LogFiles = result != null && result.Count() > 0 ? result.ToList() : new List<string>();
                            }
                            else
                            {
                                vm.LogFiles = new List<string>();
                            }
                        }
                        break;
                }

                vm.LogLocation = logLocation;
                vm.PageNumber = p;
                vm.PageCount = pageCount;
                vm.TotalCount = vm.LogFiles.IsNotNullAndNotEmpty() ? vm.LogFiles.Count : 0;
                vm.LogFiles = vm.LogFiles.IsNotNullAndNotEmpty() ? vm.LogFiles.Skip(skipRecord).Take(pageCount).ToList() : null;

                if (!string.IsNullOrEmpty(fileName) && vm.LogFiles.IsNotNullAndNotEmpty() && vm.LogFiles.Count(x => !x.IndexOf(fileName).Equals(-1)) > 0)
                {
                    switch (logLocation)
                    {
                        case FileLogLocation.Presentation:
                            {
                                if (System.IO.File.Exists(logFilename))
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.Load(logFilename);
                                    foreach (XmlNode node in doc.SelectSingleNode("errorlog").SelectNodes("exception"))
                                    {
                                        vm.ContentNodes += "<div style='border:solid 1px #ddd;padding: 8px;margin: 5px 0 5px 0;'>";
                                        vm.ContentNodes += "<div><b> Ngày Tạo :</b>" + (node.SelectSingleNode("time") != null ? node.SelectSingleNode("time").InnerText : "") + "</div>";
                                        vm.ContentNodes += "<div><b> Mô Tả :</b>" + (node.SelectSingleNode("description") != null ? node.SelectSingleNode("description").InnerText : "") + "</div>";
                                        vm.ContentNodes += "<div><b> Phương thức:</b>" + (node.SelectSingleNode("method") != null ? node.SelectSingleNode("method").InnerText : "") + "</div>";
                                        vm.ContentNodes += "<div><b> Trace :</b>" + (node.SelectSingleNode("trace") != null ? node.SelectSingleNode("trace").InnerText : "") + "</div>";
                                        vm.ContentNodes += "</div>";
                                    }
                                }
                            }
                            break;
                    }

                    vm.FileName = fileName;
                }

                return View(vm);
            }
        }

        [HttpGet]
        public ActionResult ViewLogDB(DateTime? frda, DateTime? toda, int p = 1, int ps = 50, int logTypeId = 0, int sourceId = 0)
        {
            var fromDate = frda.HasValue ? frda.Value : DateTime.Now.Date;
            var toDate = toda.HasValue ? toda.Value : DateTime.Now;
            toDate = toDate.AddTimeToTheEndOfDay();
            int pageCount = ps;

            var data = _logApi.GetListLogDb(logTypeId, sourceId, fromDate, toDate, pageCount, p);
            if (data != null && data.List != null)
            {
                data.List.ForEach(x =>
                {
                    x.Title = x.LogContent.Length > 150 ? x.LogContent.Substring(0, 150) : x.LogContent;
                    x.LogContent = HttpUtility.HtmlEncode(x.LogContent);
                    x.LogContent = x.LogContent.Replace("\r\n", "<br/>");
                    x.LogContent = x.LogContent.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
                });
            }

            var listSource = new List<SelectListItem>();
            listSource.AddRange(EnumExtension.ToListOfValueAndDesc<SourceType>().Select(x => new SelectListItem { Value = x.Value.ToString(), Text = x.Description }));

            var listClient = new List<SelectListItem>();
            listClient.AddRange(EnumExtension.ToListOfValueAndDesc<SystemLogType>().Select(x => new SelectListItem { Value = x.Value.ToString(), Text = x.Description }));

            var vm = new ViewLogDbVM()
            {
                ListRuntimeError = data.List,
                SourceError = new SelectList(listSource, "Value", "Text"),
                Clients = new SelectList(listClient, "Value", "Text"),
                FromDate = fromDate,
                ToDate = toDate,
                PageNumber = p,
                PageCount = pageCount,
                TotalCount = data.Count
            };
            return View(vm);
        }

        #endregion Logging

        #region Setting

        public ActionResult SettingList()
        {
            ViewBag.CanUpdateSetting = CheckAllowAction("SystemMngt", "UpdateSetting") ? "1" : "0";
            ViewBag.CanUpdateAdminSetting = CheckAllowAction("SystemMngt", "UpdateAdminSetting") ? "1" : "0";

            IEnumerable<SystemSettingContract> systemSettings = _systemApi.Get();
            return View(systemSettings.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSetting(string key, string value)
        {
            if (key.Equals("User.DefaultPassword"))
            {
                value = Security.Encrypt(AppUtils.SecuKey, value);
            }
            CUDReturnMessage updateResult = _systemApi.Update(key, value.Trim(), CurrentUserId);

            if (updateResult == null)
                return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Fail, message = MessageResource.CMS_SystemError_Message });

            if (updateResult.Id == (int)ResponseCode.Successed)
                return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Success, message = MessageResource.CMS_UpdateSuccessed });

            return Json(new { status = LayoutResource.CMS_AjaxResponseStatus_Fail, message = MessageResource.CMS_UpdateFailed });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReCheckCurrentUserPass(string password)
        {
            bool validLogin = false;
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
            {
                validLogin = context.ValidateCredentials(memberExtendedInfo.UserAccount, password);
            }
            if (validLogin)
            {
                var defaultPasswordSettingKey = _systemApi.Find("User.DefaultPassword");
                var desDefaultPassword = defaultPasswordSettingKey != null ? Security.Decrypt(AppUtils.SecuKey, defaultPasswordSettingKey.Value) : "12345678";
                return Json(new { IsSuccess = 1, Message = desDefaultPassword }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { IsSuccess = 0, Message = "Mật khẩu không đúng!" }, JsonRequestBehavior.AllowGet);
            }
      
        }
        #endregion Setting

        #region Upload

        //public ActionResult UploadedFile()
        //{
        //    //1-Ok, 2-File ext not allowed, 3-Failed, 4-Size Exceed
        //    int status = 1;
        //    string fName = "";
        //    string path = "";
        //    try
        //    {
        //        foreach (string fileName in Request.Files)
        //        {
        //            HttpPostedFileBase file = Request.Files[fileName];

        //            if (CheckAllowedFileExt(Path.GetExtension(file.FileName), UploadType.Image))
        //            {
        //                if (file.ContentLength <= AdminGlobal.MaxImageSize)
        //                {
        //                    path = SaveFileUploaded(Request.Files, fileName, UploadType.Image);
        //                }
        //                else
        //                {
        //                    status = 4;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                status = 2;
        //                break;
        //            }

        //            //HttpPostedFileBase file = Request.Files[fileName];
        //            ////Save file content goes here
        //            //fName = file.FileName;
        //            //string ext = Path.GetExtension(fName);

        //            //if (file != null && file.ContentLength > 0)
        //            //{
        //            //    MemoryStream target = new MemoryStream();
        //            //    file.InputStream.CopyTo(target);
        //            //    byte[] data = target.ToArray();

        //            //    path = UploadHelper.UploadByteDataFile(ConfigurationManager.AppSettings["UploadPhotoURI"],
        //            //       Guid.NewGuid().ToString() + ext,
        //            //       data);
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = 3;
        //    }

        //    if (status == 1)
        //    {
        //        return Json(new ActionMessage(1, path));
        //    }
        //    else if (status == 2)
        //    {
        //        return Json(new ActionMessage(0, MessageResource.UploadFile_FileExtNotAllowed));
        //    }
        //    else if (status == 4)
        //    {
        //        return Json(new ActionMessage(0, string.Format(MessageResource.UploadFile_FileSizeExceeded, AdminGlobal.MaxImageSize / 1024 / 1024)));
        //    }
        //    else
        //    {
        //        return Json(new ActionMessage(0, MessageResource.CMS_GetRuntimeErrorMsg));
        //    }
        //}

        //public ActionResult UploadedMediaFile()
        //{
        //    //1-Ok, 2-File ext not allowed, 3-Failed, 4-Size Exceed
        //    int status = 1;
        //    string fName = "";
        //    string path = "";
        //    try
        //    {
        //        foreach (string fileName in Request.Files)
        //        {
        //            HttpPostedFileBase file = Request.Files[fileName];

        //            if (CheckAllowedFileExt(Path.GetExtension(file.FileName), UploadType.Document))
        //            {
        //                if (file.ContentLength <= AdminGlobal.MaxMediaFileSize)
        //                {
        //                    path = SaveFileUploaded(Request.Files, fileName, UploadType.Document);
        //                }
        //                else
        //                {
        //                    status = 4;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                status = 2;
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = 3;
        //    }

        //    if (status == 1)
        //    {
        //        return Json(new ActionMessage(1, path));
        //    }
        //    else if (status == 2)
        //    {
        //        return Json(new ActionMessage(0, MessageResource.UploadFile_FileExtNotAllowed));
        //    }
        //    else if (status == 4)
        //    {
        //        return Json(new ActionMessage(0, string.Format(MessageResource.UploadFile_FileSizeExceeded, AdminGlobal.MaxMediaFileSize / 1024 / 1024)));
        //    }
        //    else
        //    {
        //        return Json(new ActionMessage(0, MessageResource.CMS_GetRuntimeErrorMsg));
        //    }
        //}

        //public ActionResult UploadedFileFromCKEditor(string CKEditorFuncNum)
        //{
        //    //1-Ok, 2-File ext not allowed, 3-Failed, 4-Size Exceed
        //    int status = 1;
        //    string fName = "";
        //    string path = "";
        //    try
        //    {
        //        foreach (string fileName in Request.Files)
        //        {
        //            HttpPostedFileBase file = Request.Files[fileName];

        //            if (CheckAllowedFileExt(Path.GetExtension(file.FileName), UploadType.Image))
        //            {
        //                if (file.ContentLength <= AdminGlobal.MaxImageSize)
        //                {
        //                    path = SaveFileUploaded(Request.Files, fileName, UploadType.Image);
        //                }
        //                else
        //                {
        //                    status = 4;
        //                    break;
        //                }
        //            }
        //            else
        //            {
        //                status = 2;
        //                break;
        //            }

        //            //HttpPostedFileBase file = Request.Files[fileName];
        //            ////Save file content goes here
        //            //fName = file.FileName;
        //            //string ext = Path.GetExtension(fName);

        //            //if (file != null && file.ContentLength > 0)
        //            //{
        //            //    MemoryStream target = new MemoryStream();
        //            //    file.InputStream.CopyTo(target);
        //            //    byte[] data = target.ToArray();

        //            //    path = UploadHelper.UploadByteDataFile(ConfigurationManager.AppSettings["UploadPhotoURI"],
        //            //       Guid.NewGuid().ToString() + ext,
        //            //       data);
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = 3;
        //    }

        //    if (status == 1)
        //    {
        //        //return Json(new ActionMessage(1, path));
        //        //return Json(new { uploaded = 1, fileName = path, url = path });

        //        return Content("<script type=\"text/javascript\">" +
        //                        "    window.parent.CKEDITOR.tools.callFunction(\"" + CKEditorFuncNum + "\", '" + AdminConfiguration.DomainPhotoURI + path + "',\"\")" +
        //                        "</script>");
        //    }
        //    if (status == 2)
        //    {
        //        return Content("<script type=\"text/javascript\">" +
        //                        "    window.parent.CKEDITOR.tools.callFunction(\"" + CKEditorFuncNum + "\", '',\"" + MessageResource.UploadFile_FileExtNotAllowed + "\")" +
        //                        "</script>");
        //    }
        //    else if (status == 4)
        //    {
        //        return Content("<script type=\"text/javascript\">" +
        //                       "    window.parent.CKEDITOR.tools.callFunction(\"" + CKEditorFuncNum + "\", '',\"" + string.Format(MessageResource.UploadFile_FileSizeExceeded, AdminGlobal.MaxImageSize / 1024 / 1024) + "\")" +
        //                       "</script>");
        //    }
        //    else
        //    {
        //        return Content("<script type=\"text/javascript\">" +
        //                        "    window.parent.CKEDITOR.tools.callFunction(\"" + CKEditorFuncNum + "\", '',\"" + MessageResource.CMS_GetRuntimeErrorMsg + "\")" +
        //                        "</script>");
        //    }
        //}

        //private string SaveFileUploaded(HttpFileCollectionBase files, string fileUploadName, UploadType type)
        //{
        //    string typePath = "Upload";

        //    if (type == UploadType.Image)
        //    {
        //        typePath = "Upload";
        //    }
        //    else if (type == UploadType.Document)
        //    {
        //        typePath = "Document";
        //    }

        //    typePath += "\\" + DateTime.Now.ToString("yyyyMM");

        //    HttpPostedFileBase file = Request.Files[fileUploadName];
        //    //Save file content goes here
        //    string fName = file.FileName;
        //    string ext = Path.GetExtension(fName);
        //    string fNameKd = Path.GetFileNameWithoutExtension(fName);
        //    if (fNameKd.Length > 30)
        //    {
        //        fNameKd = fNameKd.Substring(0, 30);
        //    }
        //    fNameKd = fNameKd.UnicodeToKoDauAndGach();

        //    string fileName = DateTime.Now.ToString("ddHHmmss") + "-" + fNameKd + ext;
        //    string folderPath = Path.Combine(AdminConfiguration.UploadFolderPath, typePath);

        //    //Check folder exists
        //    if (!Directory.Exists(folderPath))
        //    {
        //        Directory.CreateDirectory(folderPath);
        //    }

        //    string savePath = string.Format("{0}\\{1}", folderPath, fileName);
        //    string rePath = string.Format("{0}\\{1}", typePath, fileName);

        //    if (file != null && file.ContentLength > 0)
        //    {
        //        file.SaveAs(savePath);
        //    }

        //    return rePath.Replace('\\', '/');
        //}

        //private bool CheckAllowedFileExt(string fileExt, UploadType type)
        //{
        //    var allowExt = AdminGlobal.AllowUploadImageExt;

        //    if (type == UploadType.Document)
        //    {
        //        allowExt = AdminGlobal.AllowUploadFileExt;
        //    }

        //    return ("," + allowExt + ",").IndexOf("," + fileExt.ToLower() + ",") >= 0;
        //}

        #endregion Upload

      

        #region Quản lý controller & action

        public ActionResult ImportActions(bool iframe = false)
        {
            var data = systemMngtApi.GetAllController();

            if (iframe)
            {
                return View("ImportActions_Popup", data);
            }
            else
            {
                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ImportActions(string data)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var list = serializer.Deserialize<List<AdminControllerContract>>(data);

            CUDReturnMessage response = systemMngtApi.ImportActions(list);
            return CUDToJson(response, (int)ResponseCode.Successed);
        }

        [HttpGet]
        public ActionResult ImportGroupActions(bool iframe = false)
        {
            var data = systemMngtApi.GetListGroupAction(null, 0);

            if (iframe)
            {
                return View("ImportGroupActions_Popup", data);
            }
            else
            {
                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ImportGroupActions(string data)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var list = serializer.Deserialize<List<AdminGroupActionContract>>(data);

            CUDReturnMessage response = systemMngtApi.ImportGroupActions(list);
            return CUDToJson(response, (int)ResponseCode.Successed);
        }

        public ActionResult ImportGroupActionMaps(bool iframe = false)
        {
            var data = systemMngtApi.GetAllGroupActionMap();

            if (iframe)
            {
                return View("ImportGroupActionMaps_Popup", data);
            }
            else
            {
                return View(data);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ImportGroupActionMaps(string data)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var list = serializer.Deserialize<List<AdminGroupActionMapContract>>(data);

            CUDReturnMessage response = systemMngtApi.ImportGroupActionMaps(list);
            return CUDToJson(response, (int)ResponseCode.Successed);
        }

        #endregion Quản lý controller & action

        #region User tracking

        public ActionResult ListUserTracking(DateTime? fromDate, DateTime? toDate, string kw = "", int p = 1, int ps = 20)
        {
            kw = HttpUtility.UrlDecode(kw).Trim();
            if (fromDate.HasValue == false) fromDate = DateTime.Now.AddDays(-6);
            if (toDate.HasValue == false) toDate = DateTime.Now;

            fromDate = fromDate.Value.AddTimeToTheStartOfDay();
            toDate = toDate.Value.AddTimeToTheEndOfDay();

            var data = systemMngtApi.GetListUserTracking(fromDate.Value, toDate.Value, kw, p, ps);
            var model = new UserTrackingModel
            {
                TotalCount = data.Count,
                PageCount = ps,
                PageNumber = p,
                FromDate = fromDate.Value,
                ToDate = toDate.Value,
                ListTracking = data == null ? null : data.List
            };
            return View(model);
        }

        #endregion

        public ActionResult ToolAbout()
        {
            var ToolDescriptionKey = _systemApi.Find("ToolDescription");
            ViewBag.ToolDescription = ToolDescriptionKey == null ? "" : ToolDescriptionKey.Value;
            return View();
        }
    }
}