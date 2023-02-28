using System;
using System.Collections.Generic;
using BMS.Business.QCS;
using BMS.Contract.Qcs;
using BMS.Contract.Shared;

namespace BMS.Caching.QCS
{
    public interface IEvaluationCategoryGroupCaching
    {
        /// <summary>
        /// Lấy danh sách bộ phận đánh giá
        /// </summary>
        /// <returns></returns>
        //IEnumerable<EvalCategoryGroupContract> GetListDepartments();
        /// <summary>
        /// Lấy thông tin bộ phận đánh giá theo mã
        /// </summary>
        /// <param name = "deptId" > Mã bộ phận đánh giá</param>
        /// <returns></returns>

        EvalCategoryGroupContract GetCategoryGroupById(int iCateGroup);

        /// <summary>
        /// Lấy danh sách bộ phận 
        /// </summary>
        /// <returns></returns>
        PagedList<EvalCategoryGroupContract> GetListCateGroupMasterSite(int page, int pageSize, int SourceClientId);

        /// <summary>
        /// Them/hieu chinh loai hang muc
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateCateGroup(EvalCategoryGroupContract model);

        /// <summary>
        /// Xoa loai hang muc
        /// </summary>
        CUDReturnMessage DeleteCateGroup(int iCateGroup);
    }

    public class EvaluationCategoryGroupCaching : BaseCaching, IEvaluationCategoryGroupCaching
    {
        private readonly Lazy<IEvaluationCategoryGroupBusiness> _evaluationCategoryGroupBusiness;

        public EvaluationCategoryGroupCaching(/*string appid, int uid*/)  
        {
            _evaluationCategoryGroupBusiness = new Lazy<IEvaluationCategoryGroupBusiness>(() => new EvaluationCategoryGroupBusiness(appid, uid));
        }

        public EvalCategoryGroupContract GetCategoryGroupById(int iCateGroup)
        {
            return _evaluationCategoryGroupBusiness.Value.GetCategoryGroupById(iCateGroup);
        }

        public PagedList<EvalCategoryGroupContract> GetListCateGroupMasterSite(int page, int pageSize, int SourceClientId)
        {
            return _evaluationCategoryGroupBusiness.Value.GetListCateGroupMasterSite(page, pageSize, SourceClientId);
        }

        public CUDReturnMessage InsertUpdateCateGroup(EvalCategoryGroupContract model)
        {
            return _evaluationCategoryGroupBusiness.Value.InsertUpdateCateGroup(model);
        }

        public CUDReturnMessage DeleteCateGroup(int iCateGroup)
        {
            return _evaluationCategoryGroupBusiness.Value.DeleteCateGroup(iCateGroup);
        }
    }
}