using System;
using System.Linq;
using System.Collections.Generic;
using BMS.Contract.Qcs;
using BMS.DataAccess.DAO.QCS;
using BMS.Contract.Shared;

namespace BMS.Business.QCS
{
    public interface IEvalCriteriaGroupBusiness
    {
        IEnumerable<EvalCriteriaGroupContract> GetListCriteriaGroup(int iEvalCriteriaGrpId = 0);

        PagedList<EvalCriteriaGroupContract> FindCriteriaGroups(string kw = "", int iEvalCriteriaGrpId = 0, int iPage = 1, int iPageSize = 20);

        CUDReturnMessage AddEvalCriteriaGroup(EvalCriteriaGroupContract objCriteriaGroupInfo);

        CUDReturnMessage UpdateEvalCriteriaGroup(EvalCriteriaGroupContract objCriteriaGroupInfo);

        CUDReturnMessage DeleteEvalCriteriaGroup(int evalCriteriaGroupId);
    }

    public class EvalCriteriaGroupBusiness : BaseBusiness, IEvalCriteriaGroupBusiness
    {
        private readonly Lazy<IEvalCriteriaGroupAccess> _evalCriteriaGrpAccess;

        public EvalCriteriaGroupBusiness(string appid, int uid)
            : base(appid, uid)
        {
            _evalCriteriaGrpAccess = new Lazy<IEvalCriteriaGroupAccess>(() => new EvalCriteriaGroupAccess(appid, uid));
        }
        /// <summary>
        /// Lấy danh sách Hạng mục của tiểu chí đánh giá
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvalCriteriaGroupContract> GetListCriteriaGroup(int iEvalCriteriaGrpId = 0)
        {
            return _evalCriteriaGrpAccess.Value.GetListCriteriaGroup(iEvalCriteriaGrpId).Select(c => new EvalCriteriaGroupContract
            {
                EvaluationCriteriaGroupId = c.EvaluationCriteriaGroupId,
                EvaluationCriteriaGroupName = c.EvaluationCriteriaGroupName,
                EvaluationCriteriaGroupCode = c.EvaluationCriteriaGroupCode
            });
        }

        /// <summary>
        /// Thêm mới nhóm tiêu chí đánh giá
        /// </summary>
        /// <param name="objCriteriaGroupInfo"></param>
        /// <returns></returns>
        public CUDReturnMessage AddEvalCriteriaGroup(EvalCriteriaGroupContract objCriteriaGroupInfo)
        {
            return _evalCriteriaGrpAccess.Value.AddEvalCriteriaGroup(objCriteriaGroupInfo);
        }

        /// <summary>
        /// Cập nhật lại thông tin nhóm tiêu chí đánh giá
        /// </summary>
        /// <param name="objCriteriaGroupInfo"></param>
        /// <returns></returns>
        public CUDReturnMessage UpdateEvalCriteriaGroup(EvalCriteriaGroupContract objCriteriaGroupInfo)
        {
            return _evalCriteriaGrpAccess.Value.UpdateEvalCriteriaGroup(objCriteriaGroupInfo);
        }
        /// <summary>
        /// Xóa nhóm tiêu chí đánh giá
        /// </summary>
        /// <param name="evalCriteriaGroupId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteEvalCriteriaGroup(int evalCriteriaGroupId)
        {
            return _evalCriteriaGrpAccess.Value.DeleteEvalCriteriaGroup(evalCriteriaGroupId);
        }

        public PagedList<EvalCriteriaGroupContract> FindCriteriaGroups(string kw = "", int iEvalCriteriaGrpId = 0, int iPage = 1, int iPageSize = 20)
        {
            var query = _evalCriteriaGrpAccess.Value.GetListCriteriaGroup(iEvalCriteriaGrpId);
            if (!string.IsNullOrEmpty(kw)){
                query = query.Where(eg => eg.EvaluationCriteriaGroupName.Contains(kw));
            }
            var result = new PagedList<EvalCriteriaGroupContract>(query.Count());
            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(g => g.EvaluationCriteriaGroupName).Skip((iPage -1) * iPageSize).Take(iPageSize).Select(c => new EvalCriteriaGroupContract
                {
                    EvaluationCriteriaGroupId = c.EvaluationCriteriaGroupId,
                    EvaluationCriteriaGroupName = c.EvaluationCriteriaGroupName,
                    EvaluationCriteriaGroupCode = c.EvaluationCriteriaGroupCode
                }).ToList();
            }
            return result;
        }
    }
}
