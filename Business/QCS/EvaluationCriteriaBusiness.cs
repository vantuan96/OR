using System;
using System.Linq;
using System.Collections.Generic;
using BMS.Contract.Qcs;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO.QCS;

namespace BMS.Business.QCS
{
    public interface IEvaluationCriteriaBusiness
    {
        IEnumerable<EvaluationCriteriaContract> GetListEvaluationCriteria(int iEvalTypeId = 0, int iDeptId = 0, int iEvaluationCateGroupId = 0, int iEvaluationCateId = 0, int iEvalCriteriaId = 0);
        
        CUDReturnMessage AddNewEvaluationCriteria(EvaluationCriteriaContract evaluationCriteria);
        
        CUDReturnMessage UpdateEvaluationCriteria(EvaluationCriteriaContract evaluationCriteria);

        CUDReturnMessage DeleteEvaluationCriteria(int iEvalCriteriaId);
    }

    public class EvaluationCriteriaBusiness : BaseBusiness, IEvaluationCriteriaBusiness
    {        
        private readonly Lazy<IEvaluationCriteriaAccess> _evalCriteriaAccess;
     
        public EvaluationCriteriaBusiness(string appid, int uid) : base(appid, uid)
        {
            _evalCriteriaAccess = new Lazy<IEvaluationCriteriaAccess>(()=>new EvaluationCriteriaAccess(appid,uid));            
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
            //return _evalCriteriaAccess.Value.GetListEvaluationCriteria(iDeptId, iEvaluationCateGroupId, iEvaluationCateId, iEvalCriteriaId);
            var query = _evalCriteriaAccess.Value.GetListEvalCriteria(iEvalTypeId, iDeptId, iEvaluationCateGroupId, iEvaluationCateId, iEvalCriteriaId);
            return query.Select(criteria => new EvaluationCriteriaContract
            {
                Id = criteria.EvaluationCriteriaId,
                Name = criteria.EvaluationCriteriaName,
                Point = (int)criteria.Point,

                CriteriaGroupId = criteria.EvaluationCriteriaGroup.EvaluationCriteriaGroupId,
                CriteriaGroupName = criteria.EvaluationCriteriaGroup.EvaluationCriteriaGroupName,

                DeptId = criteria.AdminDepartment.DeptId,
                DeptName = criteria.AdminDepartment.DeptName,

                EvaluationCateId = criteria.EvaluationCategory.EvaluationCateId,
                EvaluationCateName = criteria.EvaluationCategory.EvaluationCateName,
                EvaluationCateCode = criteria.EvaluationCategory.EvaluationCateCode,

                EvaluationCateGroupId = criteria.EvaluationCategory.EvaluationCategoryGroup.EvaluationCateGroupId,
                EvaluationCateGroupName = criteria.EvaluationCategory.EvaluationCategoryGroup.EvaluationCateGroupName                
            });
        }
        /// <summary>
        /// Thêm mới tiêu chuẩn đánh giá
        /// </summary>
        /// <param name="evaluationCriteria">Đối tượng chứa thông tin tiêu chuẩn đánh giá</param>
        /// <returns></returns>
        public CUDReturnMessage AddNewEvaluationCriteria(EvaluationCriteriaContract evaluationCriteria)
        {
            return _evalCriteriaAccess.Value.AddNewEvaluationCriteria(evaluationCriteria);
        }
        /// <summary>
        /// Cập nhật thông tin tiêu chuẩn đánh giá
        /// </summary>
        /// <param name="evaluationCriteria">Đối tượng chứa thông tin tiêu chuẩn đánh giá</param>
        /// <returns></returns>
        public CUDReturnMessage UpdateEvaluationCriteria(EvaluationCriteriaContract evaluationCriteria)
        {
            return _evalCriteriaAccess.Value.UpdateEvaluationCriteria(evaluationCriteria);
        }

        /// <summary>
        /// Xóa tiêu chí đánh giá
        /// </summary>
        /// <param name="iEvalCriteriaId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteEvaluationCriteria(int iEvalCriteriaId)
        {
            return _evalCriteriaAccess.Value.DeleteEvaluationCriteria(iEvalCriteriaId);
        }
    }
}