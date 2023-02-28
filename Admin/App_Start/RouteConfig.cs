using System.Web.Mvc;
using System.Web.Routing;

namespace Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region authen

            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Authen", action = "Login" }
            );

            routes.MapRoute(
                name: "ErrorHandler",
                url: "error",
                defaults: new { controller = "Authen", action = "ErrorHandler" }
            );

            #endregion authen

            #region user profile

            routes.MapRoute(
                name: "UserProfile",
                url: "thong-tin-ca-nhan",
                defaults: new { controller = "UserProfile", action = "Index" }
            );

            routes.MapRoute(
                name: "ChangeMicrosite",
                url: "thay-doi-microsite",
                defaults: new { controller = "UserProfile", action = "ChangeMicrosite" }
            );

            #endregion user profile

            #region user management

            routes.MapRoute(
                name: "ListUser",
                url: "danh-sach-nguoi-dung",
                defaults: new { controller = "UserMngt", action = "ListUser" }
            );

            routes.MapRoute(
                name: "ListRole",
                url: "danh-sach-quyen",
                defaults: new { controller = "UserMngt", action = "ListRole" }
            );

            routes.MapRoute(
                name: "CreateUpdateRole",
                url: "them-sua-quyen",
                defaults: new { controller = "UserMngt", action = "CreateUpdateRole" }
            );

            routes.MapRoute(
                name: "DeleteRole",
                url: "xoa-quyen",
                defaults: new { controller = "UserMngt", action = "DeleteRole" }
            );

            routes.MapRoute(
                name: "UpdateRoleGroupActionMap",
                url: "cap-nhat-phan-quyen",
                defaults: new { controller = "UserMngt", action = "UpdateRoleGroupActionMap" }
            );

            routes.MapRoute(
                name: "LocationList",
                url: "quan-ly-phong-ban",
                defaults: new { controller = "UserMngt", action = "LocationList" }
            );

            #endregion user management

            #region system mngt

            routes.MapRoute(
                name: "SettingList",
                url: "cau-hinh-he-thong",
                defaults: new { controller = "SystemMngt", action = "SettingList" }
            );

            routes.MapRoute(
                name: "LanguageList",
                url: "danh-sach-ngon-ngu",
                defaults: new { controller = "SystemMngt", action = "LanguageList" }
            );

            routes.MapRoute(
                name: "ImportActions",
                url: "import-actions",
                defaults: new { controller = "SystemMngt", action = "ImportActions" }
            );

            routes.MapRoute(
                name: "ImportGroupActions",
                url: "import-group-actions",
                defaults: new { controller = "SystemMngt", action = "ImportGroupActions" }
            );

            routes.MapRoute(
                name: "ImportGroupActionMaps",
                url: "import-group-action-maps",
                defaults: new { controller = "SystemMngt", action = "ImportGroupActionMaps" }
            );

            routes.MapRoute(
               name: "ViewLogClient",
               url: "xem-file-loi",
               defaults: new { controller = "SystemMngt", action = "ViewLogClient" }
           );

            routes.MapRoute(
               name: "ViewLogDB",
               url: "xem-loi-he-thong",
               defaults: new { controller = "SystemMngt", action = "ViewLogDB" }
           );

            #endregion system mngt

            #region microsite

            routes.MapRoute(
                 name: "ListMicrosite",
                 url: "microsite",
                 defaults: new { controller = "MicrositeMngt", action = "ListMicrosite" }
             );

            routes.MapRoute(
                name: "CreateUpdateMicrosite",
                url: "tao-microsite",
                defaults: new { controller = "MicrositeMngt", action = "CreateUpdateMicrosite" }
            );

            routes.MapRoute(
                 name: "ViewMicrositeDetail",
                 url: "chi-tiet-microsite-{msId}",
                 defaults: new { controller = "MicrositeMngt", action = "ViewMicrositeDetail" }
             );

            routes.MapRoute(
                name: "ListAddress",
                url: "quan-ly-dia-chi",
                defaults: new { controller = "MicrositeMngt", action = "ListAddress" }
            );

            routes.MapRoute(
                name: "AddressDetail",
                url: "chi-tiet-dia-chi",
                defaults: new { controller = "MicrositeMngt", action = "AddressDetail" }
            );

            routes.MapRoute(
                name: "ListRoute",
                url: "quan-ly-lo-trinh",
                defaults: new { controller = "MicrositeMngt", action = "ListRoute" }
            );

            routes.MapRoute(
                name: "RouteDetail",
                url: "chi-tiet-lo-trinh",
                defaults: new { controller = "MicrositeMngt", action = "RouteDetail" }
            );

            #endregion microsite

            #region ListUserTracking

            routes.MapRoute(
                name: "ListUserTracking",
                url: "list-user-tracking",
                defaults: new { controller = "SystemMngt", action = "ListUserTracking" }
            );

            #endregion ListUserTracking

            #region Report

            routes.MapRoute(
               name: "MaintainScheduleReport",
               url: "bao-cao-ke-hoach-bao-tri",
               defaults: new { controller = "Report", action = "MaintainScheduleReport" }
           );

            #endregion Report

            #region Device
            routes.MapRoute(
                name: "DeviceList",
                url: "quan-ly-thiet-bi",
                defaults: new { controller = "DeviceMngt", action = "List" }
            );
            #endregion

            #region Question
            routes.MapRoute(
                name: "QuestionGroupList",
                url: "quan-ly-bo-cau-hoi",
                defaults: new { controller = "QuestionMngt", action = "QuestionGroupList" }
            );

            routes.MapRoute(
                name: "CreateUpdateQuestionGroup",
                url: "chi-tiet-bo-cau-hoi",
                defaults: new { controller = "QuestionMngt", action = "CreateUpdateQuestionGroup" }
            );

            routes.MapRoute(
                name: "QuestionGroupDetail",
                url: "noi-dung-bo-cau-hoi",
                defaults: new { controller = "QuestionMngt", action = "QuestionGroupDetail" }
            );
            #endregion

            #region Report
            routes.MapRoute(
                name: "SurveyResultDetail",
                url: "bao-cao-ket-qua-danh-gia-chat-luong",
                defaults: new { controller = "Report", action = "SurveyResultDetail" }
            );
            #endregion

            #region CheckList

            #endregion

            #region PnL List
            routes.MapRoute(
                name: "ListPnLList",
                url: "pnl-list",
                defaults: new { controller = "Master", action = "ListPnLList" }
            );
            routes.MapRoute(
                name: "CreateUpdatePnLList",
                url: "tao-pnllist",
                defaults: new { controller = "Master", action = "CreateUpdatePnLList" }
            );

          

            #endregion

            #region PnL BU List
            routes.MapRoute(
                name: "ListPnLBuList",
                url: "pnl-bu-list",
                defaults: new { controller = "Master", action = "ListPnLBuList" }
            );
            routes.MapRoute(
                name: "CreateUpdatePnLBuList",
                url: "tao-pnlbulist",
                defaults: new { controller = "Master", action = "CreateUpdatePnLBuList" }
            );
            #endregion

            #region system checklist
            routes.MapRoute(
                name: "GetSystemCheckList",
                url: "danh-muc-he-thong",
                defaults: new { controller = "Master", action = "GetSystemCheckList" }
                );

            routes.MapRoute(
              name: "DeleteSystemCheckList",
              url: "xoa-he-thong-check-list",
              defaults: new { controller = "Master", action = "DeleteSystemCheckList" }
            );
            routes.MapRoute(
              name: "InsertUpdateSystemCheckList",
              url: "them-moi-hoac-chinh-sua-thong-tin-he-thong",
              defaults: new { controller = "Master", action = "InsertUpdateSystemCheckList" }
             );
            routes.MapRoute(
             name: "InsertUpdateOwnerSystem",
             url: "set-quyen-owner-he-thong",
             defaults: new { controller = "Master", action = "InsertUpdateOwnerSystem" }
            );

            #endregion

            #region item checklist
            routes.MapRoute(
                name: "GetItemCheckList",
                url: "hang-muc-check-list",
                defaults: new { controller = "CheckList", action = "GetItemCheckList" }
                );

            routes.MapRoute(
              name: "DeleteItem",
              url: "xoa-hang-muc-check-list",
              defaults: new { controller = "CheckList", action = "DeleteItem" }
            );
            routes.MapRoute(
              name: "InsertUpdateItem",
              url: "them-moi-hoac-chinh-sua-hang-muc-check-list",
              defaults: new { controller = "CheckList", action = "InsertUpdateItem" }
             );

            #endregion

            #region new checklist
            routes.MapRoute(
                name: "ListCheckList",
                url: "danh-sach-check-list",
                defaults: new { controller = "CheckList", action = "ListCheckList" }
            );
            routes.MapRoute(
                name: "CreateUpdateCheckList",
                url: "tao-hoac-chinh-sua-check-list",
                defaults: new { controller = "CheckList", action = "CreateUpdateCheckList" }
            );



            #endregion
          //  #region Call queue
          //  routes.MapRoute(
          //   name: "ListPhongBan",
          //   url: "danh-sach-phong-ban",
          //   defaults: new { controller = "QueuePatient", action = "GetFullPhongBan" }
          //);
          //  #endregion
            #region operation checklist
            routes.MapRoute(
               name: "ListOperationCheckList",
               url: "danh-sach-check-list-can-thuc-hien",
               defaults: new { controller = "OperationCheckList", action = "ListOperationCheckList" }
            );
            routes.MapRoute(
               name: "UpdateOperationCheckList",
               url: "thuc-hien-check-list",
               defaults: new { controller = "OperationCheckList", action = "UpdateOperationCheckList" }
            );
            routes.MapRoute(
              name: "ListApproveCheckList",
              url: "phe-duyet-check-list",
              defaults: new { controller = "OperationCheckList", action = "ListApproveCheckList" }
           );
            routes.MapRoute(
            name: "ViewCheckListDetail",
            url: "chi-tiet-thuc-hien-checklist",
            defaults: new { controller = "OperationCheckList", action = "ViewCheckListDetail" }
          );
            //history checklist
            routes.MapRoute(
                name: "HistoryCheckList",
                url: "lich-su-checklist",
                defaults: new { controller = "OperationCheckList", action = "HistoryCheckList" }
            );
            //chi tiet log
            routes.MapRoute(
                  name: "ViewDetailHistory",
                  url: "chi-tiet-log",
                  defaults: new { controller = "OperationCheckList", action = "ViewDetailHistory" }
            );

            #endregion

            #region report
            routes.MapRoute(
              name: "Dashboard",
              url: "dash-board",
              defaults: new { controller = "Report", action = "Dashboard" }
            );
            #endregion

            #region 
            #region OR 
            routes.MapRoute(
                name: "SearchPatientOR",
                url: "tra-cuu-benh-nhan",
                defaults: new { controller = "OR", action = "SearchPatientOR" }
            );
            routes.MapRoute(
                name: "ViewVisit",
                url: "thong-tin-luot-kham",
                defaults: new { controller = "OR", action = "ViewVisit" }
            );
            routes.MapRoute(
               name: "ViewORRegistor",
               url: "dang-ky-phong-mo",
               defaults: new { controller = "OR", action = "ViewORRegistor" }
           );
            routes.MapRoute(
               name: "ViewORManagement",
               url: "phan-cong-va-ghi-nhan-ekip-mo",
               defaults: new { controller = "OR", action = "ViewORManagement" }
           );

            routes.MapRoute(
               name: "ViewORAnesth",
               url: "dieu-phoi-nhan-su-gay-me",
               defaults: new { controller = "OR", action = "ViewORAnesth" }
           );
            routes.MapRoute(
             name: "CheckActiveLink",
             url: "kich-hoat-link",
             defaults: new { controller = "Temp", action = "CheckActiveLink" }
            );

            routes.MapRoute(
                name: "SearchAnesthInfo",
                url: "tra-cuu-thong-tin-dang-ky",
                defaults: new { controller = "OR", action = "SearchAnesthInfo" }
            );

            routes.MapRoute(
                 name: "SearchSurgeryRole",
                 url: "tra-cuu-thong-tin-mo",
                 defaults: new { controller = "OR", action = "SearchSurgeryRole" }
            );
            routes.MapRoute(
                 name: "SearchAnesthRole",
                 url: "tra-cuu-thong-tin-gay-me",
                 defaults: new { controller = "OR", action = "SearchAnesthRole" }
            );
            routes.MapRoute(
               name: "ViewPlan",
               url: "dashboard",
               defaults: new { controller = "OR", action = "ViewPlan" }
          );
           routes.MapRoute(
               name: "GetFullPhongBan",
               url: "get-full-phong-ban-oh",
               defaults: new { controller = "OR", action = "GetFullPhongBan" }
           );
            routes.MapRoute(
              name: "GetFullPlanByMonth",
              url: "get-full-phong-ban-oh-by-month",
              defaults: new { controller = "OR", action = "GetFullPlanByMonth" }
           );

          routes.MapRoute(
             name: "OHSearchPatients",
             url: "thong-tin-phong-mo",
             defaults: new { controller = "OR", action = "OHSearchPatients" }
          );
            routes.MapRoute(
             name: "SearchHpService",
             url: "danh-muc-ky-thuat",
             defaults: new { controller = "OR", action = "SearchHpService" }
          );
            routes.MapRoute(
             name: "DeleteSurgery",
             url: "huy-ca-mo",
             defaults: new { controller = "OR", action = "DeleteSurgery" }
          );

            #endregion

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}