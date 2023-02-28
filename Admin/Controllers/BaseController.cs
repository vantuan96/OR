using AutoMapper;
using Caching.Core;
using Contract.Microsite;
using Contract.Shared;
using Contract.User;
using Admin.Helper;
using Admin.Models.MicrositeMngt;
using Admin.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VG.Common;
using VG.Framework.Mvc;
using VG.General.ExceptionHandling;
using Admin.Models.Master;
using Contract.MasterData;
using Admin.Models.CheckList;
using Admin.Models.Operation;
using System.Web.Security;
using Newtonsoft.Json;
using Admin.App_Start;

namespace Admin.Controllers
{
    public class BaseController : Controller
    {
        public readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected MemberExtendContract _memberExtendedInfo;
        protected MemberExtendContract memberExtendedInfo
        {
            get
            {
                if (this._memberExtendedInfo != null)
                    return this._memberExtendedInfo;
                else
                {
                    if (System.Web.HttpContext.Current != null)
                        return MvcHelper.GetUserSession(System.Web.HttpContext.Current?.ApplicationInstance.Context);
                    else return null;
                }
            }
            set
            {
                this._memberExtendedInfo = value;
            }
        }

        private ISystemSettingCaching systemSettingCaching;
        protected IAuthenCaching authenCaching;
        private string urlReferrer = null;

        protected string DomainName = "vingroup.local";
        /// <summary>
        /// UserId
        /// </summary>
        protected int CurrentUserId
        {
            get { return memberExtendedInfo.UserId; }
        }

        protected string CurrentUserDisplayName
        {
            get { return memberExtendedInfo.DisplayName; }
        }
        protected string CurrentUserName
        {
            get
            {
                return memberExtendedInfo?.UserAccount;
            }
        }
        protected LocationContract CurrentLoc
        {
            get
            {
                var SessionUserInfo = MvcHelper.GetUserSession(HttpContext.ApplicationInstance.Context);
                return SessionUserInfo.CurrentLocaltion;
            }
        }


        //protected int CurrentMsId
        //{
        //    get { return memberExtendedInfo.CurrentMicrosite == null ? 0 : memberExtendedInfo.CurrentMicrosite.MsId; }
        //}

        //protected bool IsCurrentRootSite
        //{
        //    get { return memberExtendedInfo.CurrentMicrosite == null ? false : memberExtendedInfo.CurrentMicrosite.IsRootSite; }
        //}

        protected string CurrentEmail { get { return memberExtendedInfo.Email; } }
        protected string LineManagerEmail { get { return memberExtendedInfo.LineManagerEmail; } }

        protected List<int> AllowMsId
        {
            get
            {
                return memberExtendedInfo.ListMicrosites.Select(r => r.MsId).ToList();
            }
        }

        protected List<MicrositeItemContract> AllowMicrosite
        {
            get
            {
                return memberExtendedInfo.ListMicrosites.OrderByDescending(r => r.IsRootSite).ThenBy(r => r.Title).Distinct().ToList();
            }
        }

        protected List<MicrositeItemContract> AllowMicrositeExceptRootSite
        {
            get
            {
                return memberExtendedInfo.ListMicrosites.Where(r => r.IsRootSite == false).OrderByDescending(r => r.IsRootSite).ThenBy(r => r.Title).ToList();
            }
        }

        protected string EmailLogin
        {
            get { return memberExtendedInfo.Email; }
        }

        protected List<AdminRoleContract> CurrentUserRoles
        {
            get { return memberExtendedInfo.Roles; }
        }

        protected string UrlReferrer
        {
            get { return urlReferrer; }
        }

        protected Lazy<IMapper> lazyMapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MicrositeContentContract, CreateUpdateMicrositeModel>();
                cfg.CreateMap<CreateUpdateMicrositeModel, MicrositeContentContract>();
                cfg.CreateMap<SystemCheckListContract, InsertUpdatetSystemModel>();
                cfg.CreateMap<InsertUpdatetSystemModel, SystemCheckListContract>();
                cfg.CreateMap<InsertUpdateItemModel, ItemContract>();
                cfg.CreateMap<ItemContract, InsertUpdateItemModel>();
                cfg.CreateMap<AssignOwnerSystemModel, AssignOwnerSystemContract>();
                cfg.CreateMap<AssignOwnerSystemContract, AssignOwnerSystemModel>();

                cfg.CreateMap<UpdateItemModel, UpdateItemContract>();
                cfg.CreateMap<UpdateItemContract, UpdateItemModel>();




            });

            IMapper mapper = config.CreateMapper();
            return mapper;
        });

        public BaseController(IAuthenCaching authenCaching, ISystemSettingCaching systemSettingCaching)
        {
            try
            {
                this.authenCaching = authenCaching;
                this.systemSettingCaching = systemSettingCaching;

                var domainNameConfig = systemSettingCaching.Find("Account.ADDomainName");

                if (domainNameConfig != null)
                {
                    DomainName = domainNameConfig.Value;
                }
            }
            catch (Exception ex)
            {
                //log.Debug(string.Format("Exception when create BaseController constructor.Detail: {0}", ex));
            }

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string controllerName = filterContext.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            string curRequestUrl = HttpUtility.UrlEncode(Request.Url.ToString());

            const string lostSession = "&stcode=302";

            LoginContract memberBaseInfo = VinAuthentication.CurrentUser(filterContext.HttpContext);
            bool isLogon = VinAuthentication.IsLogon(filterContext.HttpContext);
            ViewBag.IsAuthen = isLogon;
            string email = (string)filterContext.HttpContext.Session["Email"];
            //var isSigned = AuthenController.IsLogonAsync(email);
            if (!isLogon || memberBaseInfo == null)
            {
                OwinStartup.IsLogin = 1;
                VinAuthentication.SignOutLocal(filterContext.HttpContext);
                string url = new UrlHelper(filterContext.HttpContext.Request.RequestContext).Action("Login", "Authen") + "?refurl=" + curRequestUrl + lostSession;

                if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.HttpContext.Request.HttpMethod == "POST")
                {
                    url = new UrlHelper(filterContext.HttpContext.Request.RequestContext).Action("Login", "Authen");
                    filterContext.Result = new JsonResult
                    {
                        Data = new { redirect = url, status = 401 },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new RedirectResult(url);
                }
            }
            else
            {
                var ToolNameKey = systemSettingCaching.Find("ToolName");
                ViewBag.ToolName = ToolNameKey == null ? "" : ToolNameKey.Value;
                var LogoKey = systemSettingCaching.Find("Logo");
                ViewBag.Logo = LogoKey == null ? "" : LogoKey.Value;

                var request = filterContext.HttpContext.Request;
                if (request.UrlReferrer != null && request.UrlReferrer.PathAndQuery != request.Url.PathAndQuery && request.UrlReferrer.AbsolutePath != request.Url.AbsolutePath)
                    urlReferrer = HttpContext.Server.UrlEncode(request.UrlReferrer.PathAndQuery);

                CUDReturnMessage reMsg = new CUDReturnMessage();

                var memberExtInfo = authenCaching.GetMemberDetail(memberBaseInfo.UserId);

                //VinAuthentication.UserSignIn(memberBaseInfo, HttpContext, memberExtInfo);

                if (memberExtInfo == (default(MemberExtendContract)) || memberExtInfo == null)
                {
                    // loi ket noi deal service
                    filterContext.Result = new RedirectResult(Url.RouteUrl("ErrorHandler", new { id = (int)EnumErrorCode.CouldNotLoadDataFromDealService }));
                }
                else if (memberExtInfo.UserId == -2)
                {
                    // khong co quyen truy cap
                    filterContext.Result = new RedirectResult(Url.RouteUrl("ErrorHandler", new { id = (int)EnumErrorCode.IsConstructing }));
                }
                else if (memberExtInfo.UserId == -1 || (memberExtInfo.ListMemberAllowedActions != null && !memberExtInfo.ListMemberAllowedActions.Where(n => n.IsShowMenu == true).Any()))
                {
                    // khong co quyen truy cap
                    filterContext.Result = new RedirectResult(Url.RouteUrl("ErrorHandler", new { id = (int)EnumErrorCode.DeniedSystem, emailLogin = memberBaseInfo.Email }));
                }
                //else if (memberExtInfo.ListMicrosites.IsNullOrEmpty())
                //{
                //    // khong co quyen truy cap
                //    filterContext.Result = new RedirectResult(Url.RouteUrl("ErrorHandler", new { id = (int)EnumErrorCode.MicrositeNotFounded, emailLogin = memberBaseInfo.Email }));
                //}
                else if (!CheckAllowAction(controllerName, actionName, memberExtInfo))
                {
                    //reMsg = _dealToolClient.CheckUserAvailable(memberBaseInfo.Id);
                    if (memberExtInfo.ListMemberAllowedActions != null &&
                        memberExtInfo.ListMemberAllowedActions.Count(x => x.IsShowMenu) > 0 && !Request.IsAjaxRequest())
                    {
                        // khong co quyen truy cap he thong
                        filterContext.Result = new RedirectResult(Url.RouteUrl("ErrorHandler", new { id = (int)EnumErrorCode.Denied/*, userlogin = memberBaseInfo.Email*/ }));
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(Url.RouteUrl("ErrorHandler", new { id = (int)EnumErrorCode.Denied, stCode = 302/*, userlogin = memberBaseInfo.Email*/ }));
                    }
                }
                else
                {
                    memberExtendedInfo = memberExtInfo;
                    //memberExtendedInfo.CurrentMicrosite = memberBaseInfo.CurrentMicrosite;

                    filterContext.HttpContext.Session[AdminConfiguration.CurrentUserSessionKey] = memberExtendedInfo;

                    //if (memberExtendedInfo.IsRequireChangePass)
                    //{
                    //    // yeu cau doi mat khau
                    //    if ((actionName != "Index" && actionName != "ChangePassword") || controllerName != "UserProfile")
                    //        filterContext.Result = new RedirectResult(Url.Action("Index", "UserProfile"));
                    //}
                    //else if (memberExtendedInfo.CurrentMicrosite == null)
                    //{
                    //    if (memberExtendedInfo.ListMicrosites.Count == 1)
                    //    {
                    //        memberExtendedInfo.CurrentMicrosite = memberExtendedInfo.ListMicrosites[0];
                    //        //ViewBag.MicrositeName = memberBaseInfo.CurrentMicrosite.Title;
                    //    }
                    //    else
                    //    {
                    //        // yêu cầu chọn microsite
                    //        if (actionName != "ChangeMicrosite" || controllerName != "UserProfile")
                    //        {
                    //            var changeMcrositeUrl = new UrlHelper(filterContext.HttpContext.Request.RequestContext).Action("ChangeMicrosite", "UserProfile") + "?refurl=" + curRequestUrl;
                    //            filterContext.Result = new RedirectResult(changeMcrositeUrl);
                    //        }
                    //    }
                    //}
                }
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            // Get last error
            Exception ex = filterContext.Exception;
            VGException vgex;

            string actionName = filterContext.HttpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            string linkRequest = filterContext.HttpContext.Request.Url.PathAndQuery;
            string ipAddress = filterContext.HttpContext.Request.UserHostAddress;

            //Exception is AdayroiException already
            if (filterContext.Exception is VGException)
            {
                vgex = (VGException)filterContext.Exception;
            }
            else
            {
                vgex = new VGException(ErrorSeverity.Error,
                    ErrorCode.RuntimeError,
                    string.Format(ConstValue.ErrorSourceFormat, filterContext.Controller.ToString(), actionName),
                    ex.Message,
                    ex.StackTrace);
            }

            // TODO: Invoke log error function
            string message = CommonHelper.ExceptionToMessage(vgex) + " \r\n "
                + " ActionName: " + actionName + " \r\n "
                + " URL: " + linkRequest;
            CommonHelper.CatchExceptionToLogLocal(message, memberExtendedInfo);
            filterContext.Result = new RedirectResult(Url.RouteUrl("ErrorHandler", new { id = (int)EnumErrorCode.Exception, stCode = vgex.ErrorCode }));
            base.OnException(filterContext);
        }

        /// <summary>
        /// kiểm tra quyền truy cập của user hiện tại
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="memberExtendedInfo"></param>
        /// <returns></returns>
        public bool CheckAllowAction(string controllerName, string actionName, MemberExtendContract memberExtendedInfo = null)
        {
            memberExtendedInfo = memberExtendedInfo ??
                (MemberExtendContract)HttpContext.Session[AdminConfiguration.CurrentUserSessionKey];

            if (memberExtendedInfo != null)
            {
                if (controllerName.Equals("UserMngt") && actionName.Equals("Default")
                    || controllerName.Equals("UserMngt") && actionName.Equals("UnderConstruction")
                    )
                {
                    return true;
                }
                else
                {
                    return memberExtendedInfo != null
                        && memberExtendedInfo.ListMemberAllowedActions.Count(x =>
                            x.ControllerName == controllerName &&
                            x.ActionName == actionName) > 0;
                }
            }
            else
            {
                return memberExtendedInfo != null
                    && memberExtendedInfo.ListMemberAllowedActions.Count(x =>
                        x.ControllerName == controllerName &&
                        x.ActionName == actionName) > 0;
            }
        }

        /// <summary>
        /// chuyển response từ api sang json
        /// </summary>
        /// <param name="cud">Response từ API</param>
        /// <param name="statusId">ID của những trạng thái thành công</param>
        /// <returns></returns>
        protected JsonResult CUDToJson(CUDReturnMessage cud, List<int> statusId)
        {
            if (cud == null) return Json(new ActionMessage(new CUDReturnMessage(ResponseCode.Error)));

            if (statusId.Contains(cud.Id))
            {
                TempData[AdminGlobal.ActionMessage] = new ActionMessage(1, cud.SystemMessage);
                return Json(new ActionMessage(1, ""));
            }
            else
            {
                return Json(new ActionMessage(cud));
            }
        }

        /// <summary>
        /// chuyển response từ api sang json
        /// </summary>
        /// <param name="cud">Response từ API</param>
        /// <param name="statusId">ID của trạng thái thành công</param>
        /// <returns></returns>
        protected JsonResult CUDToJson(CUDReturnMessage cud, int statusId)
        {
            if (cud == null) return Json(new ActionMessage(new CUDReturnMessage(ResponseCode.Error)));

            if (statusId == cud.Id)
            {
                TempData[AdminGlobal.ActionMessage] = new ActionMessage(1, cud.SystemMessage);
                return Json(new ActionMessage(1, ""));
            }
            else
            {
                return Json(new ActionMessage(cud));
            }
        }

    }
}