using System;
using System.Collections.Generic;
using BMS.Business.QCS;
using BMS.Contract.Qcs;
using BMS.Contract.Shared;

namespace BMS.Caching.QCS
{
    public interface IEvaluationCriteriaCaching
    {
        IEnumerable<EvaluationCriteriaContract> GetListEvaluationCriteria(int iEvalTypeId = 0, int iDeptId = 0, int iEvaluationCateGroupId = 0, int iEvaluationCateId = 0, int iEvalCriteriaId = 0);

        CUDReturnMessage AddNewEvaluationCriteria(EvaluationCriteriaContract evaluationCriteria);

        CUDReturnMessage UpdateEvaluationCriteria(EvaluationCriteriaContract evaluationCriteria);
        
        CUDReturnMessage DeleteEvaluationCriteria(int evalCriteriaId);
    }

    public class EvaluationCriteriaCaching : BaseCaching, IEvaluationCriteriaCaching
    {
        private readonly Lazy<IEvaluationCriteriaBusiness> _evalCriteriatBusiness;

        public EvaluationCriteriaCaching(/*string appid, int uid*/)  
        {
            _evalCriteriatBusiness = new Lazy<IEvaluationCriteriaBusiness>(()=>new EvaluationCriteriaBusiness(appid,uid));
        }

        /// <summary>
        /// Lấy danh sách tiêu chuẩn đánh giá
        /// </summary>
        /// <param name="iDeptId">lọc theo mã bộ phận đánh giá nếu mã lớn hơn 0</param>
        /// <param name="iEvaluationCateGroupId">lọc theo mã nhóm chuyên mục đánh giá nếu mã lớn hơn 0</param>
        /// <param name="iEvaluationCateId">lọc theo mã chuyên mục đánh giá nếu mã lớn hơn 0</param>
        /// <returns>Trả về danh sách đối tượng EvaluationCriteriaContract nếu thỏa điều kiện lọc</returns>
        public IEnumerable<EvaluationCriteriaContract> GetListEvaluationCriteria(int iEvalTypeId = 0, int iDeptId = 0, int iEvaluationCateGroupId = 0, int iEvaluationCateId = 0, int iEvalCriteriaId = 0)
        {
            return _evalCriteriatBusiness.Value.GetListEvaluationCriteria(iEvalTypeId, iDeptId, iEvaluationCateGroupId, iEvaluationCateId, iEvalCriteriaId);
        }
        /// <summary>
        /// Thêm mới tiêu chuẩn đánh giá
        /// </summary>
        /// <param name="evaluationCriteria">Đối tượng chứa thông tin tiêu chuẩn đánh giá</param>
        /// <returns></returns>
        public CUDReturnMessage AddNewEvaluationCriteria(EvaluationCriteriaContract evaluationCriteria)
        {
            return _evalCriteriatBusiness.Value.AddNewEvaluationCriteria(evaluationCriteria);
        }
        /// <summary>
        /// Cập nhật thông tin tiêu chuẩn đánh giá
        /// </summary>
        /// <param name="evaluationCriteria">Đối tượng chứa thông tin tiêu chuẩn đánh giá</param>
        /// <returns></returns>
        public CUDReturnMessage UpdateEvaluationCriteria(EvaluationCriteriaContract evaluationCriteria)
        {
            return _evalCriteriatBusiness.Value.UpdateEvaluationCriteria(evaluationCriteria);
        }

        /// <summary>
        /// Xóa tiêu chí đánh giá
        /// </summary>
        /// <param name="evaluationCriteria">Đối tượng chứa thông tin tiêu chuẩn đánh giá</param>
        /// <returns></returns>
        public CUDReturnMessage DeleteEvaluationCriteria(int evalCriteriaId)
        {
            return _evalCriteriatBusiness.Value.DeleteEvaluationCriteria(evalCriteriaId);
        }
    }
}