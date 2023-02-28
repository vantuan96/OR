using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BMS.Contract.Qcs;
using BMS.Contract.Qcs.EvaluationCategoryGroups;
using BMS.Contract.Qcs.Sites;
using BMS.Contract.Qcs.SiteZones;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;
using BMS.DataAccess.DAO.QCS;

namespace BMS.Business.Qcs
{
    public interface ISiteBusiness
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
        /// <returns></returns>
        CUDReturnMessage InsertUpdateSiteZone(SiteZoneContract model);

        /// <summary>
        ///  Xoa thong tin zone
        /// </summary>
        /// <param name="SiteZoneId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage DeleteZone(int SiteZoneId);

        List<EvaluationCategoryGroupContract> GetListCateGroup(int sourceClientId);


        SiteZoneContract GetSiteZoneInfo(int SiteZoneId);

    }

    public class SiteBusiness : BaseBusiness, ISiteBusiness
    {
        private Lazy<ISiteAccess> lazySiteAccess;
        private Lazy<ISiteZoneAccess> lazySiteZone;
        private Lazy<IEvaluationCategoryGroupAccess> lazyGroupCate;
        public SiteBusiness(string appid, int uid) : base(appid, uid)
        {
            lazySiteAccess = new Lazy<ISiteAccess>(() => new SiteAccess(appid, uid));
            lazySiteZone = new Lazy<ISiteZoneAccess>(() => new SiteZoneAccess(appid, uid));
            lazyGroupCate = new Lazy<IEvaluationCategoryGroupAccess>(() => new EvaluationCategoryGroupAccess(appid, uid));
        }
        /// <summary>
        /// Xoa thong tin zone
        /// </summary>
        /// <param name="SiteZoneId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteZone(int SiteZoneId)
        {
            return lazySiteZone.Value.DeleteZone(SiteZoneId);
        }

        public PagedList<SiteZoneInfoContract> FindSiteZones(int siteId, string keyWord, int groupCateId, int page, int pageSize, int sourceClientId)
        {
            var query = lazySiteAccess.Value.FindSiteZones(new int[] { siteId }, keyWord, groupCateId, sourceClientId);
            var result = new PagedList<SiteZoneInfoContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(r => r.SiteZoneName).Skip((page - 1) * pageSize).Take(pageSize).Select(r => new SiteZoneInfoContract
                {
                    SiteId = r.SiteId,
                    SiteName = r.Microsite.MicrositeContents.Where(c => c.IsDeleted == false && c.LangShortName == defaultLanguageCode).Select(c => c.Title).FirstOrDefault(),
                    SiteCode = r.Microsite.ReferenceCode,
                    SiteZoneId = r.SiteZoneId,
                    SiteZoneName = r.SiteZoneName,
                    SiteZoneCode = r.SiteZoneCode,
                    CateGroups = r.EvaluationZoneCateGroups.Where(m => m.IsDeleted == false).Select(m => new EvaluationCateGroupItemContract
                    {
                        EvaluationCateGroupId = m.EvaluationCategoryGroup.EvaluationCateGroupId,
                        EvaluationCateGroupCode = m.EvaluationCategoryGroup.EvaluationCateGroupCode,
                        EvaluationCateGroupName = m.EvaluationCategoryGroup.EvaluationCateGroupName,
                    }).ToList()
                }).ToList();
            }

            return result;
        }

        /// <summary>
        /// Them moi /dieu chinh thong tin zone
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateSiteZone(SiteZoneContract model)
        {
            if (model.SiteZoneId == 0)
            {
                return lazySiteZone.Value.InsertSiteZone(model);
            }
            else
            {
                return lazySiteZone.Value.UpdateSiteZone(model);
            }
        }

        /// <summary>
        /// Lay ds group cate dua vao clientSource
        /// </summary>
        /// <param name="sourceClientId"></param>
        /// <returns></returns>
        public List<EvaluationCategoryGroupContract> GetListCateGroup(int sourceClientId)
        {
            return
                lazyGroupCate.Value.GetListCateGroup(sourceClientId).Select(g => new EvaluationCategoryGroupContract()
                {
                    CreatedDate = g.CreatedDate,
                    CreatedBy = g.CreatedBy,
                    EvaluationCateGroupId = g.EvaluationCateGroupId,
                    EvaluationCateGroupCode = g.EvaluationCateGroupCode,
                    EvaluationCateGroupName = g.EvaluationCateGroupName,
                    IsDeleted = g.IsDeleted,
                    LastUpdatedBy = g.LastUpdatedBy,
                    LastUpdatedDate = g.LastUpdatedDate,
                    SourceClientId = g.SourceClientId
                }).ToList();
        }

        /// <summary>
        /// Lay thong tin sizeZone
        /// </summary>
        /// <param name="SiteZoneId"></param>
        /// <returns></returns>
        public SiteZoneContract GetSiteZoneInfo(int SiteZoneId)
        {
            var query = lazySiteZone.Value.GetSiteZoneInfo(SiteZoneId);

            return query.Select(r => new SiteZoneContract
            {
                SiteId = r.SiteId,
                SiteZoneName = r.SiteZoneName,
                SiteZoneCode = r.SiteZoneCode,
                SiteZoneId = r.SiteZoneId,
                EvaluationCateGroupId = r.EvaluationZoneCateGroups.Where(m => m.IsDeleted == false).Select(m => m.EvaluationCateGroupId).ToList()
            }).SingleOrDefault();
        }
    }
}
