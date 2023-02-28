using System;
using System.Collections.Generic;
using BMS.Business.QCS;
using BMS.Contract.Qcs.EvaluationSiteZones;
using BMS.Contract.Shared;

namespace BMS.Caching.QCS
{
    public interface IEvaluationCalendarCaching
    {
        /// <summary>
        /// Them moi ky/cuoc kiem tra
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateEvaluationCalendar(EvaluationCalendarContract model);

        /// <summary>
        /// Thêm mới hàng loạt kỳ kiem tra
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

    public class EvaluationCalendarCaching : BaseCaching, IEvaluationCalendarCaching
    {
        private Lazy<IEvaluationCalendarBusiness> evaluationCalendarBusiness;

        public EvaluationCalendarCaching(/*string appid, int uid*/)  
        {
            evaluationCalendarBusiness = new Lazy<IEvaluationCalendarBusiness>(() =>
            {
                var instance = new EvaluationCalendarBusiness(appid, uid);
                return instance;
            });
        }

        public CUDReturnMessage DeleteEvaluationCalendar(int EvaluationCalendarId, int userId)
        {
            return evaluationCalendarBusiness.Value.DeleteEvaluationCalendar(EvaluationCalendarId, userId);
        }

        public PagedList<EvaluationCalendarInfoContract> FindEvaluationCalendar(int siteId, string keyWord, int page, int pageSize, int sourceClientId,int state)
        {
            return evaluationCalendarBusiness.Value.FindEvaluationCalendar(siteId, keyWord, page, pageSize, sourceClientId,state);
        }

        /// <summary>
        /// lay thong tin ky kiem tra
        /// </summary>
        /// <param name="evalCalendarId"></param>
        /// <returns></returns>
        public EvaluationCalendarContract GetEvaluationCalendar(int evalCalendarId)
        {
            return evaluationCalendarBusiness.Value.GetEvaluationCalendar(evalCalendarId);
        }

        public CUDReturnMessage InsertUpdateEvaluationCalendar(EvaluationCalendarContract model)
        {
            return evaluationCalendarBusiness.Value.InsertUpdateEvaluationCalendar(model);
        }

        public CUDReturnMessage InsertManyEvaluationCalendar(InsertManyEvaluationCalendarContract model)
        {
            return evaluationCalendarBusiness.Value.InsertManyEvaluationCalendar(model);
        }

        public List<EvaluationGroupContract> GetEvaluateResult(int msId, int sourceClientId, int evaluationCalendarId)
        {
            return evaluationCalendarBusiness.Value.GetEvaluateResult(msId, sourceClientId, evaluationCalendarId);
        }

        public List<EvaluationSiteZoneDetailContract> GetEvaluateDetail(int siteZoneId, int evaluationCalendarId)
        {
            return evaluationCalendarBusiness.Value.GetEvaluateDetail(siteZoneId, evaluationCalendarId);
        }

        public CUDReturnMessage UpdateCheckListDetail(List<EvaluationSiteZoneDetailContract> detail)
        {
            return evaluationCalendarBusiness.Value.UpdateCheckListDetail(detail);
        }
    }
}