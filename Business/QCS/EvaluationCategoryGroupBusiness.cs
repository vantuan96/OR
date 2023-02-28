using System;
using System.Collections.Generic;
using BMS.Contract.Qcs;
using BMS.DataAccess.DAO.QCS;
using BMS.Contract.Shared;
using System.Linq;
namespace BMS.Business.QCS
{
    public interface IEvaluationCategoryGroupBusiness
    {


        /// <summary>
        /// Lấy thông tin loai theo mã
        /// </summary>
        /// <param name="iCateGroup">Mã loai hang muc</param>
        /// <returns></returns>
        EvalCategoryGroupContract GetCategoryGroupById(int iCateGroup);

        /// <summary>
        /// Them/hieu chinh loai hang muc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateCateGroup(EvalCategoryGroupContract model);
        /// <summary>
        /// Xoa loai hang muc
        /// </summary>
        CUDReturnMessage DeleteCateGroup(int iCateGroup);
        ///// <summary>
        ///// Lấy danh sách loai hang muc
        ///// </summary>
        ///// <returns></returns>
        PagedList<EvalCategoryGroupContract> GetListCateGroupMasterSite(int page, int pageSize, int SourceClientId);
    }

    public class EvaluationCategoryGroupBusiness : BaseBusiness, IEvaluationCategoryGroupBusiness
    {
        private readonly Lazy<IEvaluationCategoryGroupAccess> _evaluationCategoryGroupAccess;

        public EvaluationCategoryGroupBusiness(string appid, int uid) : base(appid, uid)
        {
            _evaluationCategoryGroupAccess = new Lazy<IEvaluationCategoryGroupAccess>(() => new EvaluationCategoryGroupAccess(appid, uid));
        }

        public EvalCategoryGroupContract GetCategoryGroupById(int iCateGroup)
        {
            return _evaluationCategoryGroupAccess.Value.GetCategoryGroupById(iCateGroup);
        }


        public PagedList<EvalCategoryGroupContract> GetListCateGroupMasterSite(int page, int pageSize, int SourceClientId)
        {
            var query = _evaluationCategoryGroupAccess.Value.GetListCateGroupMasterSite(SourceClientId);
            var result = new PagedList<EvalCategoryGroupContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderBy(cateGroup => cateGroup.EvaluationCateGroupName).Skip((page - 1) * pageSize).Take(pageSize).Select(cateGroup => new EvalCategoryGroupContract
                {
                    EvaluationCateGroupId = cateGroup.EvaluationCateGroupId,
                    EvaluationCateGroupCode = cateGroup.EvaluationCateGroupCode,
                    EvaluationCateGroupName = cateGroup.EvaluationCateGroupName,
                    SourceClientId = cateGroup.SourceClientId,
                    CreatedDate = cateGroup.CreatedDate,
                    CreatedBy = cateGroup.CreatedBy,
                    LastUpdatedDate = cateGroup.LastUpdatedDate,
                    LastUpdatedBy = cateGroup.LastUpdatedBy
                }).ToList();
            }

            return result;
        }
        public CUDReturnMessage InsertUpdateCateGroup(EvalCategoryGroupContract model)
        {
            if (model.EvaluationCateGroupId == 0)
            {
                return _evaluationCategoryGroupAccess.Value.InsertCateGroup(model);
            }
            else
            {
                return _evaluationCategoryGroupAccess.Value.UpdateCateGroup(model);
            }
        }

        public CUDReturnMessage DeleteCateGroup(int iCateGroup)
        {
            return _evaluationCategoryGroupAccess.Value.DeleteCateGroup(iCateGroup);
        }

    }
}