
using System.Web.Mvc;
using Caching.Core;
using Admin.Models.User;
using Admin.Models.Shared;
using Admin.Helper;
using Admin.Resource;
using Contract.Shared;
using System.Linq;
using Contract.User;
using System.Collections;

namespace Admin.Controllers
{
    [CheckUserCaching]
    public class UserProfileController : BaseController
    {
        private IAuthenCaching authenCaching;
        private IUserProfileCaching userProfileCaching;
        private readonly ILocationCaching locationApi;

        public UserProfileController(IAuthenCaching authenCaching, 
            ISystemSettingCaching systemSettingApi,
            IUserProfileCaching userProfileCaching,
            ILocationCaching locationApi) : base(authenCaching, systemSettingApi)
        {
            this.authenCaching = authenCaching;
            this.userProfileCaching = userProfileCaching;
            this.locationApi = locationApi;
        }

        // GET: UserProfile
        public ActionResult Index()
        {
            var result = userProfileCaching.GetUserProfile(CurrentUserId);
            var listLocation = locationApi.Get();

            ViewBag.IsRequireChangePass = memberExtendedInfo.IsRequireChangePass;
            ViewBag.AllLocation = listLocation;
            return View(result);
        }

        public ActionResult ChangePassword(bool isFillDefaultPass = false)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePassword(ChangePasswordModel model)
        {
            if (model.OldPassword != model.NewPassword)
            {
                var result = userProfileCaching.ChangePassword(CurrentUserId, model.OldPassword, model.NewPassword);
                if (result == null)
                    return Json(new ActionMessage(-1, MessageResource.Shared_SystemErrorMessage));

                var actionMessage = new ActionMessage(result);

                if (result.Id == (int)ResponseCode.ChangePassword_Successed)
                {
                    actionMessage.ID = 1;
                    TempData[AdminGlobal.ActionMessage] = actionMessage;

                    return Json(actionMessage);
                }
                else
                {
                    actionMessage.ID = -1;
                    return Json(actionMessage);
                }
            }
            else
            {
                return Json(new ActionMessage(-1, MessageResource.ChangePassword_PasswordNotChanged));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeSite(int siteId)
        {
            if(memberExtendedInfo!=null && memberExtendedInfo.Locations!=null && memberExtendedInfo.Locations.Count > 0)
            {
                for (var i = 0; i < memberExtendedInfo.Locations.Count; i++)
                {
                    if (memberExtendedInfo.Locations[i].LocationId == siteId)
                    {
                        memberExtendedInfo.Locations[i].IsCurrent = true;
                    }
                    else
                    {
                        memberExtendedInfo.Locations[i].IsCurrent = false;
                    }
                }
                var result=locationApi.UpdateLocationUser(CurrentUserId, memberExtendedInfo.Locations);
                ////Clear all caches
                //foreach (DictionaryEntry entry in HttpContext.Cache)
                //{
                //    HttpContext.Cache.Remove((string)entry.Key);
                //}
                if(result!=null&& result.Id== (int)ResponseCode.LocationMngt_SuccessUpdate)
                {
                    ////Đóng tạm thời (30/03/2022)
                    if (Shared.ORVisitBase.Instance != null)
                        Shared.ORVisitBase.Instance.SetCurrentVisit(null);

                    return Json(new { IsSuccess = 1, Message = "Đổi Site thành công" }, JsonRequestBehavior.AllowGet);
                }
                    
                else
                    return Json(new { IsSuccess = 0, Message = VG.Common.StringUtil.GetResourceString(typeof(MessageResource), result.Message) }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { IsSuccess = 0, Message = "Đổi Site thành công" }, JsonRequestBehavior.AllowGet);

        }
    }
}