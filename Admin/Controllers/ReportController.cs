using Admin.Helper;
using Admin.Models.Report;
using Caching.Core;
using Caching.Report;
using System.Web.Mvc;

namespace Admin.Controllers
{
    [CheckUserCaching]
    public class ReportController : BaseController
    {
        private readonly IReportCaching reportCaching;
        private readonly ILocationCaching locationApi;
        private readonly ISystemSettingCaching systemSettingApi;

        int defaultPageSize = AdminConfiguration.Paging_PageSize;

        public ReportController(IAuthenCaching authenCaching,
            ISystemSettingCaching systemSettingApi,
            ILocationCaching locationApi,
            IReportCaching reportCaching
        ) : base(authenCaching, systemSettingApi)
        {
            this.reportCaching = reportCaching;
            this.locationApi = locationApi;            
            this.systemSettingApi = systemSettingApi;
        }
        
        public ActionResult DashBoard()
        {
            var listCheckList = reportCaching.GetDashboardInMonth();
            var listSystem = reportCaching.GetDashboardBySystem();
            var listSummary = reportCaching.GetReportCheckListSummary();
            var model = new DashboardModel();
            model.listSystems = listSystem;
            model.listCheckLists = listCheckList;
            model.listSummarys = listSummary;
            return View(model);
        }        
    }
}