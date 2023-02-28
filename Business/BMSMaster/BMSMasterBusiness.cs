using Contract.BMSMaster;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BMSMaster
{
    public interface IBMSMasterBusiness
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

    public class BMSMasterBusiness : BaseBusiness, IBMSMasterBusiness
    {
        private Lazy<IBMSMasterDataAccess> lazyBMSMasterDataAccess;


        public BMSMasterBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyBMSMasterDataAccess = new Lazy<IBMSMasterDataAccess>(() => new BMSMasterDataAccess(appid, uid));

        }

        /// <summary>
        /// Lấy tất cả PnL
        /// </summary>
        /// <returns></returns>
        public List<PlanPnLContract> GetAllPnL()
        {
            var query = lazyBMSMasterDataAccess.Value.GetAllPnL();

            return query.OrderBy(x => x.Name).Select(r => new PlanPnLContract
            {
                ID = r.ID,
                Name = r.Name,
                Inactive = r.Inactive,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                UpdatedBy = r.UpdatedBy,
                UpdatedDate = r.UpdatedDate
            }).ToList();
        }

        /// <summary>
        /// Lấy tất cả site
        /// </summary>
        /// <returns></returns>
        public List<PlanSiteContract> GetAllSite()
        {
            var query = lazyBMSMasterDataAccess.Value.GetAllSite();

            return query.OrderBy(x => x.Name).Select(r => new PlanSiteContract
            {
                ID = r.ID,
                Name = r.Name,
                PlanPnLID = r.PlanPnLID,
                Inactive = r.Inactive,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                UpdatedBy = r.UpdatedBy,
                UpdatedDate = r.UpdatedDate
            }).ToList();
        }

        /// <summary>
        /// Lấy tất cả phòng ban
        /// </summary>
        /// <returns></returns>
        public List<DepartmentContract> GetAllDepartment()
        {
            var query = lazyBMSMasterDataAccess.Value.GetAllDepartment();
            return query.OrderBy(x => x.Name).Select(r => new DepartmentContract
            {
                ID = r.ID,
                Name = r.Name,
                Description = r.Description,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                UpdatedBy = r.UpdatedBy,
                UpdatedDate = r.UpdatedDate,
                Status = r.Status,
                Email = r.Email,
                HeadofDepartment = r.HeadofDepartment,
                Code = r.Code,
                IsShowHotline = r.IsShowHotline,
                IsShowInventory = r.IsShowInventory,
                IsShowOther = r.IsShowOther
            }).ToList();
        }
    }
}
