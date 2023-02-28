using Business.BMSMaster;
using Contract.BMSMaster;
using System;
using System.Collections.Generic;

namespace Caching.BMSMaster
{
    public interface IBMSMasterCaching
    {
        /// <summary>
        /// Lấy tất cả PnL
        /// </summary>
        /// <returns></returns>
        List<PlanPnLContract> GetAllPnL();

        /// <summary>
        /// Lấy tất cả site
        /// </summary>
        /// <returns></returns>
        List<PlanSiteContract> GetAllSite();

        /// <summary>
        /// Lấy tất cả phòng ban
        /// </summary>
        /// <returns></returns>
        List<DepartmentContract> GetAllDepartment();
    }

    public class BMSMasterCaching : BaseCaching, IBMSMasterCaching
    {
        private Lazy<IBMSMasterBusiness> lazyBMSMasterBusiness;
        private string typeName = "BMSMasterCaching";

        public BMSMasterCaching(/*string appid, int uid*/)
        {
            lazyBMSMasterBusiness = new Lazy<IBMSMasterBusiness>(() => new BMSMasterBusiness(appid, uid));
        }

        /// <summary>
        /// Lấy tất cả PnL
        /// </summary>
        /// <returns></returns>
        public List<PlanPnLContract> GetAllPnL()
        {
            return base.ProcessCache<List<PlanPnLContract>>(() =>
            {
                return base.GetDataWithMemCache<IBMSMasterBusiness, List<PlanPnLContract>>(
                    lazyBMSMasterBusiness.Value,
                    "GetAllPnL",
                    new object[] { },
                    CacheTimeout.Medium);
            }, "ReportCaching", "GetAllPnL");

            //return lazyBMSMasterBusiness.Value.GetAllPnL();
        }

        /// <summary>
        /// Lấy tất cả site
        /// </summary>
        /// <returns></returns>
        public List<PlanSiteContract> GetAllSite()
        {
            return base.ProcessCache<List<PlanSiteContract>>(() =>
            {
                return base.GetDataWithMemCache<IBMSMasterBusiness, List<PlanSiteContract>>(
                    lazyBMSMasterBusiness.Value,
                    "GetAllSite",
                    new object[] { },
                    CacheTimeout.Medium);
            }, "ReportCaching", "GetAllSite");

            //return lazyBMSMasterBusiness.Value.GetAllSite();
        }

        /// <summary>
        /// Lấy tất cả phòng ban
        /// </summary>
        /// <returns></returns>
        public List<DepartmentContract> GetAllDepartment()
        {
            return base.ProcessCache<List<DepartmentContract>>(() =>
            {
                return base.GetDataWithMemCache<IBMSMasterBusiness, List<DepartmentContract>>(
                    lazyBMSMasterBusiness.Value,
                    "GetAllDepartment",
                    new object[] { },
                    CacheTimeout.Medium);
            }, "ReportCaching", "GetAllDepartment");

            //return lazyBMSMasterBusiness.Value.GetAllDepartment();
        }
    }
}