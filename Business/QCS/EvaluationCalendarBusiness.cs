using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VG.Common;
using BMS.Contract.Qcs;
using BMS.Contract.Qcs.EvaluationCategoryGroups;
using BMS.Contract.Qcs.EvaluationSiteZones;
using BMS.Contract.Qcs.SiteZones;
using BMS.Contract.Qcs.SiteZoneViolations;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO.QCS;

namespace BMS.Business.QCS
{
    public interface IEvaluationCalendarBusiness
    {
        /// <summary>
        /// Them moi ky/cuoc kiem tra
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateEvaluationCalendar(EvaluationCalendarContract model);

        /// <summary>
        /// Tạo tự động nhiều kỳ kiểm tra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage InsertManyEvaluationCalendar(InsertManyEvaluationCalendarContract model);


        /// <summary>
        /// xóa 1 kỳ /cuộc kiểm tra
        /// </summary>
        /// <param name="EvaluationCalendarId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage DeleteEvaluationCalendar(int EvaluationCalendarId, int userId);
        /// <summary>
        /// tim kiem ds ky kiem tra
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sourceClientId"></param>
        /// <returns></returns>
        PagedList<EvaluationCalendarInfoContract> FindEvaluationCalendar(int siteId, string keyWord, int page, int pageSize, int sourceClientId,int state);
        /// <summary>
        /// lay thong tin EvaluationCalendar
        /// </summary>
        /// <param name="evalCalendarId"></param>
        /// <returns></returns>
        EvaluationCalendarContract GetEvaluationCalendar(int evalCalendarId);

        /// <summary>
        /// lấy thống kê số lượng kết quả đánh giá theo từng Zone
        /// </summary>
        /// <param name="evaluationCalendarId"></param>
        /// <returns></returns>
        List<EvaluationGroupContract> GetEvaluateResult(int msId, int sourceClientId, int evaluationCalendarId);

        /// <summary>
        /// lấy thông tin chi tiết kết quả kiểm tra
        /// </summary>
        /// <param name="siteZoneId"></param>
        /// <param name="evaluationCalendarId"></param>
        /// <returns></returns>
        List<EvaluationSiteZoneDetailContract> GetEvaluateDetail(int siteZoneId, int evaluationCalendarId);

        /// <summary>
        /// cập nhật thông tin chi tiết
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        CUDReturnMessage UpdateCheckListDetail(List<EvaluationSiteZoneDetailContract> detail);

    }

    public class EvaluationCalendarBusiness : BaseBusiness, IEvaluationCalendarBusiness
    {
        private Lazy<IEvaluationCalendarAccess> evaluationCalendarAccess;
        private Lazy<ISiteAccess> lazySiteAccess;
        private Lazy<IEvaluationCriteriaAccess> lazyEvaluationCriteriaAccess;
        private Lazy<IEvaluationCategoryGroupAccess> lazyEvaluationCategoryGroupAccess;

        public EvaluationCalendarBusiness(string appid, int uid) : base(appid, uid)
        {
            evaluationCalendarAccess = new Lazy<IEvaluationCalendarAccess>(() => new EvaluationCalendarAccess(appid, uid));
            lazySiteAccess = new Lazy<ISiteAccess>(() => new SiteAccess(appid, uid));
            lazyEvaluationCriteriaAccess = new Lazy<IEvaluationCriteriaAccess>(() => new EvaluationCriteriaAccess(appid, uid));
            lazyEvaluationCategoryGroupAccess = new Lazy<IEvaluationCategoryGroupAccess>(() => new EvaluationCategoryGroupAccess(appid, uid));
        }
        /// <summary>
        /// xóa 1 kỳ/cuộc kiểm tra
        /// </summary>
        /// <param name="EvaluationCalendarId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteEvaluationCalendar(int EvaluationCalendarId, int userId)
        {
            return evaluationCalendarAccess.Value.DeleteEvaluationCalendar(EvaluationCalendarId, userId);
        }
        /// <summary>
        /// tim kiếm 1 kỳ /cuộc kiểm tra
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sourceClientId"></param>
        /// <returns></returns>
        public PagedList<EvaluationCalendarInfoContract> FindEvaluationCalendar(int siteId, string keyWord, int page, int pageSize, int sourceClientId,int state)
        {
            var query = evaluationCalendarAccess.Value.FindEvaluationCalendar(siteId, keyWord, sourceClientId, null, null, null,state: state);
            var result = new PagedList<EvaluationCalendarInfoContract>(query.Count());
            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(r => r.EvaluationStartDate).Skip((page - 1) * pageSize).Take(pageSize).Select(r => new EvaluationCalendarInfoContract
                {
                    EvaluationCalendarId = r.EvaluationCalendarId,
                    EvaluationCalendarName = r.EvaluationCalendarName,
                    EvaluationStartDate = r.EvaluationStartDate,
                    EvaluationEndDate = r.EvaluationEndDate,
                    SiteId = r.SiteId,
                    SiteName = r.Microsite.MicrositeContents.Where(s => s.IsDeleted == false && s.LangShortName == defaultLanguageCode).Select(s => s.Title).FirstOrDefault(),
                    EvaluationCalendarStatusId = r.EvaluationCalendarStatusId,
                    EvaluationCalendarStatusName = r.EvaluationCalendarStatu.EvaluationCalendarStatusName
                }).ToList();
            }

            return result;

        }

        /// <summary>
        /// lay thong tin eval calendar
        /// </summary>
        /// <param name="evalCalendarId"></param>
        /// <returns></returns>
        public EvaluationCalendarContract GetEvaluationCalendar(int evalCalendarId)
        {
            var data = evaluationCalendarAccess.Value.GetEvaluationCalendar(evalCalendarId);
            if (data == null) return null;
            return new EvaluationCalendarContract
            {
                EvaluationCalendarId = data.EvaluationCalendarId,
                EvaluationCalendarName = data.EvaluationCalendarName,
                EvaluationStartDate = data.EvaluationStartDate,
                EvaluationEndDate = data.EvaluationEndDate,
                SiteId = data.SiteId,
                EvaluationCalendarStatusId = data.EvaluationCalendarStatusId,
                LastUpdatedBy = data.LastUpdatedBy,
                IsDeleted = data.IsDeleted
            };
        }

        /// <summary>
        /// thêm 1 kỳ/cuộc kiểm tra
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateEvaluationCalendar(EvaluationCalendarContract model)
        {
            return evaluationCalendarAccess.Value.InsertUpdateEvaluationCalendar(model);
        }


        /// <summary>
        /// tạo nhiều kỳ kiểm tra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertManyEvaluationCalendar(InsertManyEvaluationCalendarContract model)
        {
            return evaluationCalendarAccess.Value.InsertManyEvaluationCalendar(model);
        }

        public List<EvaluationGroupContract> GetEvaluateResult(int msId, int sourceClientId, int evaluationCalendarId)
        {
            var querySiteZone = lazySiteAccess.Value.FindSiteZones( new int[] { msId }, string.Empty, 0, sourceClientId);
            var queryEvaluateResult = evaluationCalendarAccess.Value.GetEvaluateResult(exceptStatusId: (int)EvaluationStatus.NotYetEvaluated, evaluationCalendarId: evaluationCalendarId);
            var queryCateGroup = lazyEvaluationCategoryGroupAccess.Value.GetListCateGroup(sourceClientId);
            var queryEvaluationCriteria = lazyEvaluationCriteriaAccess.Value.GetListEvalCriteria();

            // lấy danh sách khu vực
            var listSiteZone = querySiteZone.Select(r => new ZoneItemContract
            {
                SiteZoneId = r.SiteZoneId,
                SiteZoneCode = r.SiteZoneCode,
                SiteZoneName = r.SiteZoneName,
                EvaluationCateGroupId = r.EvaluationZoneCateGroups.Where(m => m.IsDeleted == false).Select(m => m.EvaluationCateGroupId).ToList()
            }).ToList();

            // lấy số lượng tiêu chí đánh giá theo mỗi chủ đề
            var dicEvaluationCriteria = queryEvaluationCriteria.GroupBy(r => r.EvaluationCategory.EvaluationCateGroupId).Select(gr => new
            {
                EvaluationCateGroupId = gr.Key,
                Count = gr.Count()
            }).ToDictionary(r => r.EvaluationCateGroupId);

            // lấy số lượng đã đánh giá theo từng khu vực và chủ đề
            var dictEvaluateResult = queryEvaluateResult.GroupBy(r => new { r.SiteZoneId, r.EvaluationCriteria.EvaluationCategory.EvaluationCateGroupId }).Select(gr => new
            {
                Key = gr.Key,
                Count = gr.Count()
            }).ToDictionary(r => r.Key);

            // lấy danh sách chủ đề
            var listCateGroup = queryCateGroup.Select(r => new EvaluationCateGroupItemContract
            {
                EvaluationCateGroupId = r.EvaluationCateGroupId,
                EvaluationCateGroupCode = r.EvaluationCateGroupCode,
                EvaluationCateGroupName = r.EvaluationCateGroupName
            }).ToList();

            List<EvaluationGroupContract> result = new List<EvaluationGroupContract>();
            foreach (var cateGroup in listCateGroup)
            {
                var group = new EvaluationGroupContract(cateGroup);
                var needToEvaluate = dicEvaluationCriteria.ContainsKey(cateGroup.EvaluationCateGroupId) ? dicEvaluationCriteria[cateGroup.EvaluationCateGroupId].Count : 0;

                foreach (var zone in listSiteZone.Where(r => r.EvaluationCateGroupId.Contains(cateGroup.EvaluationCateGroupId)))
                {
                    group.ListSiteZone.Add(new EvaluationZoneGroupContract
                    {
                        SiteZone = zone,
                        NeedToEvaluate = needToEvaluate,
                        EvaluatedCount = dictEvaluateResult.ContainsKey(new { zone.SiteZoneId, cateGroup.EvaluationCateGroupId }) ? dictEvaluateResult[new { zone.SiteZoneId, cateGroup.EvaluationCateGroupId }].Count : 0,
                    });
                }

                result.Add(group);
            }

            return result;
        }

        public List<EvaluationSiteZoneDetailContract> GetEvaluateDetail(int siteZoneId, int evaluationCalendarId)
        {
            var queryEvaluateResult = evaluationCalendarAccess.Value.GetEvaluateResult(siteZoneId: siteZoneId, evaluationCalendarId: evaluationCalendarId);

            return queryEvaluateResult.Select(r => new EvaluationSiteZoneDetailContract
            {
                EvaluationSiteZoneId = r.EvaluationSiteZoneId,
                EvaluationStatusId = r.EvaluationStatusId,
                EvaluationCalendarId = r.EvaluationCalendarId,
                EvaluationCriteriaId = r.EvaluationCriteriaId,
                SiteZoneId = r.SiteZoneId,
                ListViolations = r.SiteZoneViolations.Where(v => v.IsDeleted == false).Select(v => new SiteZoneViolationItemContract
                {
                    ViolationId = v.ViolationId,
                    UriImageViolationError = v.UriImageViolationError,
                    StatusId = v.ViolationStatusId
                }).ToList()
            }).ToList();
        }

        public CUDReturnMessage UpdateCheckListDetail(List<EvaluationSiteZoneDetailContract> detail)
        {
            var listToInsert = detail.Where(r => r.EvaluationSiteZoneId == 0 && r.EvaluationStatusId != (int)EvaluationStatus.NotYetEvaluated).ToList();
            var listToUpdate = detail.Where(r => r.EvaluationSiteZoneId > 0).ToList();

            if (listToInsert != null && listToInsert.Count > 0)
            {
                var result = evaluationCalendarAccess.Value.InsertEvaluationSiteZone(listToInsert);
                if (result.Id != (int)ResponseCode.Admin_Qcs_EvaluationCalendar_UpdateSuccess)
                    return result;
            }

            if (listToUpdate != null && listToUpdate.Count > 0)
            {
                var result = evaluationCalendarAccess.Value.UpdateEvaluationSiteZone(listToUpdate);
                if (result.Id != (int)ResponseCode.Admin_Qcs_EvaluationCalendar_UpdateSuccess)
                    return result;
            }

            return new CUDReturnMessage(ResponseCode.Admin_Qcs_EvaluationCalendar_UpdateSuccess);
        }
    }
}
