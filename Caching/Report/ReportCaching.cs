using System;
using Business.Report;
using System.Data;
using Contract.Shared;
using Contract.Report;
using System.Collections.Generic;

namespace Caching.Report
{
    public interface IReportCaching
    {
        List<CheckListDashboard> GetDashboardInMonth();
        List<SystemDashBoard> GetDashboardBySystem();
        List<CheckListInfoDashboard> GetReportCheckListSummary();
    }             
        
    public class ReportCaching : BaseCaching, IReportCaching
    {
        private const string CachingTypeName = "ReportCaching";
        private readonly Lazy<IReportBusiness> lazyReportBusiness;        
        public ReportCaching()
             
        {
            lazyReportBusiness = new Lazy<IReportBusiness>(() => new ReportBusiness(appid, uid));
        }

        public List<CheckListDashboard> GetDashboardInMonth()
        {
            return lazyReportBusiness.Value.GetDashboardInMonth();
        }
        public List<SystemDashBoard> GetDashboardBySystem()
        {
            return lazyReportBusiness.Value.GetDashboardBySystem();
        }

        public List<CheckListInfoDashboard> GetReportCheckListSummary()
        {
            return lazyReportBusiness.Value.GetReportCheckListSummary();
        }
    }
}
