using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Business.Qcs;
using BMS.Contract.Qcs.EvaluationCategoryGroups;
using BMS.Contract.Qcs.Sites;
using BMS.Contract.Qcs.SiteZones;
using BMS.Contract.Shared;

namespace BMS.Caching.Qcs
{
    public interface ISiteCaching
    {
        /// <summary>
        /// tim kiem danh sach zone cua thuoc trung tam thuong mai
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="groupCateId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedList<SiteZoneInfoContract> FindSiteZones(int siteId, string keyWord, int groupCateId, int page, int pageSize, int sourceClientId);
        /// <summary>
        /// Them/hieu chinh zone
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateSiteZone(SiteZoneContract model);
        /// <summary>
        ///  Xoa thong tin zone
        /// </summary>
        /// <param name="SiteZoneId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage DeleteZone(int SiteZoneId);
        /// <summary>
        /// Lay ds group cate
        /// </summary>
        /// <param name="sourceClientId"></param>
        /// <returns></returns>
        List<EvaluationCategoryGroupContract> GetListCateGroup(int sourceClientId);
    }

    public class SiteCaching : BaseCaching, ISiteCaching
    {
        private Lazy<SiteBusiness> siteBusiness;

        public SiteCaching(/*string appid, int uid*/)  
        {
            siteBusiness = new Lazy<SiteBusiness>(() =>
            {
                var instance = new SiteBusiness(appid, uid);
                return instance;
            });
        }
        /// <summary>
        /// xoa zone
        /// </summary>
        /// <param name="SiteZoneId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteZone(int SiteZoneId)
        {
            return siteBusiness.Value.DeleteZone(SiteZoneId); 
        }
        /// <summary>
        /// tim kiem thong tin zone
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="groupCateId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sourceClientId"></param>
        /// <returns></returns>
        public PagedList<SiteZoneInfoContract> FindSiteZones(int siteId, string keyWord, int groupCateId, int page, int pageSize, int sourceClientId)
        {
            return siteBusiness.Value.FindSiteZones(siteId,keyWord, groupCateId,page,pageSize,sourceClientId);
        }
        /// <summary>
        /// Lay ds group cate
        /// </summary>
        /// <param name="sourceClientId"></param>
        /// <returns></returns>
        public List<EvaluationCategoryGroupContract> GetListCateGroup(int sourceClientId)
        {
            return siteBusiness.Value.GetListCateGroup(sourceClientId);
        }
        
        /// <summary>
        /// Lay thong tin size zone
        /// </summary>
        /// <param name="SiteZoneId"></param>
        /// <returns></returns>
        public SiteZoneContract GetSiteZoneInfo(int siteZoneId)
        {
            return siteBusiness.Value.GetSiteZoneInfo(siteZoneId);
        }

        /// <summary>
        /// cap nhat thong tin zone
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateSiteZone(SiteZoneContract model)
        {
            return siteBusiness.Value.InsertUpdateSiteZone(model);
        }
    }
}
