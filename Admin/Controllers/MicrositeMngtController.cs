using Caching.Core;
using Caching.Microsite;
using Contract.Microsite;
using Contract.Shared;
using Admin.Helper;
using Admin.Models.MicrositeMngt;
using Admin.Models.Shared;
using Admin.Resource;
using System.Linq;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Controllers
{
    [CheckUserCaching]
    public class MicrositeMngtController : BaseController
    {
        private readonly IMicrositeMngtCaching micrositeMngtApi;

        public MicrositeMngtController(IAuthenCaching authenApi,IMicrositeMngtCaching micrositeMngtApi,ISystemSettingCaching systemSettingApi) : base(authenApi, systemSettingApi)
        {
            this.micrositeMngtApi = micrositeMngtApi;
        }

        #region microsite management

        public ActionResult ListMicrosite()
        {
            var listMs = micrositeMngtApi.GetListMicrosite(CurrentUserId).ToList();
            ViewBag.ListMs = listMs;
            return View(listMs);
        }

        /// <summary>
        /// thêm và chỉnh sửa microsite content
        /// </summary>
        /// <param name="msId"></param>
        /// <param name="micrositeContentId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public ActionResult CreateUpdateMicrosite(int msId = 0, int micrositeContentId = 0, string lang = "vi")
        {
            CreateUpdateMicrositeModel model = new CreateUpdateMicrositeModel();
            string referenceCode = "";

            #region tạo ddl

            var ListApprovalStatus = EnumExtension.ToListOfValueAndDesc<ApprovalStatus>().Select(r => new SelectListItem
            {
                Text = r.Description,
                Value = r.Value.ToString()
            }).ToList();

         

            #endregion tạo ddl

            if (msId > 0)
            {
                var microsite = micrositeMngtApi.GetListMicrosite(CurrentUserId);
                if (microsite != null && microsite.Exists(m => m.MsId == msId))
                {
                    model.ReferenceCode = microsite.Single(m => m.MsId == msId).ReferenceCode;
                    model.Id = microsite.Single(m => m.MsId == msId).MsId;
                    model.MsId = microsite.Single(m => m.MsId == msId).MsId;
                    model.Title = microsite.Single(m => m.MsId == msId).Title;

                }
            }          
            else
            {
                model.Id = 0;
                model.MsId = msId;
                model.LangShortName = lang;
                model.ReferenceCode = referenceCode;
            }
                        
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateMicrositeContentStatus(int msId, int status)
        {
            var response = micrositeMngtApi.UpdateMicrositeStatus(msId, status);
            if (response.Id == (int)ResponseCode.MicrositeMngt_SuccessUpdate)
            {
                var msg = new ActionMessage(1, response.SystemMessage);
                TempData[AdminGlobal.ActionMessage] = msg;

                return Json(msg);
            }
            else
            {
                return Json(new ActionMessage(response));
            }
        }

        /// <summary>
        /// Thêm và chỉnh sửa microsite content
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdateMicrosite(CreateUpdateMicrositeModel model)
        {
            if (ModelState.IsValid)
            {
                model.ApprovalStatus = (int)ApprovalStatus.Approved;
                var msContent = lazyMapper.Value.Map<MicrositeContentContract>(model);
                var response = micrositeMngtApi.CreateUpdateMicrosite(msContent);
                if (response != null)
                {
                    if (response.Id == (int)ResponseCode.MicrositeMngt_SuccessCreate || response.Id == (int)ResponseCode.MicrositeMngt_SuccessUpdate)
                    {
                        response.SystemMessage = "Shared_SaveSuccess";
                        TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
                        //ViewBag.ActionMessage = new ActionMessage(response);
                        //ViewBag.IsSuccess = true;
                    }
                    else
                    {
                        //ModelState.AddModelError(string.Empty, Resource.MessageResource.CMS_GetRuntimeErrorMsg);
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

            #region tạo ddl

            //var ListApprovalStatus = EnumExtension.ToListOfValueAndDesc<ApprovalStatus>().Select(r => new SelectListItem
            //{
            //    Text = r.Description,
            //    Value = r.Value.ToString()
            //}).ToList();
            //model.ListStatus = ListApprovalStatus;

            #endregion tạo ddl

            var cudMsg = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
            return Json(new ActionMessage(cudMsg));
        }

        /// <summary>
        /// update thông tin chính của microsite
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMicrosite(MicrositeDetailModel model)
        {
            if (ModelState.IsValid)
            {
                MicrositeContentContract data = new MicrositeContentContract();
                data.MsId = model.MsId;
                data.MstId = model.MstId;
                data.ReferenceCode = model.ReferenceCode;
                data.Id = -1;//Update microsite

                var response = micrositeMngtApi.CreateUpdateMicrosite(data);

                if (response == null)
                {
                    response = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                }
                if (response.Id == (int)ResponseCode.MicrositeMngt_SuccessUpdate)
                {
                    response.SystemMessage = "Shared_SaveSuccess";
                }
                else
                {
                    response.SystemMessage = "CMS_GetRuntimeErrorMsg";
                }

                response.Message = StringUtil.GetResourceString(typeof(MessageResource), response.SystemMessage);
                TempData[AdminGlobal.ActionMessage] = new ActionMessage(response);
            }
            else
            {
                var cudMsg = new CUDReturnMessage() { Id = (int)ResponseCode.Error, SystemMessage = "CMS_GetRuntimeErrorMsg" };
                TempData[AdminGlobal.ActionMessage] = new ActionMessage(cudMsg);
            }

            return Redirect(Url.RouteUrl("ViewMicrositeDetail", new { msId = model.MsId }));
        }

        public ActionResult ViewMicrositeDetail(int msId)
        {
            var data = micrositeMngtApi.GetMicrositeDetail(msId);
            MicrositeDetailModel model = new MicrositeDetailModel();
            model.Details = data;
            model.MstId = data.MstId;
            model.ReferenceCode = data.ReferenceCode;

            #region tạo ddl status

            var listStatus = EnumExtension.ToListOfValueAndDesc<ObjectStatus>().Select(r => new SelectListItem
            {
                Text = StringUtil.GetResourceString(typeof(LayoutResource), r.Description),
                Value = r.Value.ToString(),
                Selected = r.Value == data.Status
            }).ToList();           

            #endregion tạo ddl status

            ViewBag.ListStatus = listStatus;
            return View(model);
        }
        [ValidateAntiForgeryToken]
        public JsonResult DeleteMicrosite(int id)
        {
            var response = micrositeMngtApi.DeleteMicrosite(id);
            return CUDToJson(response, (int)ResponseCode.MicrositeMngt_SuccessDelete);
        }

        #endregion microsite management
    }
}