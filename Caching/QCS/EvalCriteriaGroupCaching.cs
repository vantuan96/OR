using System;
using System.Collections.Generic;
using BMS.Business.QCS;
using BMS.Contract.Qcs;
using BMS.Contract.Shared;

namespace BMS.Caching.QCS
{
    public interface IEvalCriteriaGroupCaching
    {
        IEnumerable<EvalCriteriaGroupContract> GetListCriteriaGroup(int iEvalCriteriaGrpId = 0);

        PagedList<EvalCriteriaGroupContract> FindCriteriaGroups(string kw = "", int iEvalCriteriaGrpId = 0, int iPage = 1, int iPageSize = 20);

        CUDReturnMessage AddEvalCriteriaGroup(EvalCriteriaGroupContract objCriteriaGroupInfo);

        CUDReturnMessage UpdateEvalCriteriaGroup(EvalCriteriaGroupContract objCriteriaGroupInfo);

        CUDReturnMessage DeleteEvalCriteriaGroup(int evalCriteriaGroupId);
    }
    public class EvalCriteriaGroupCaching: BaseCaching,IEvalCriteriaGroupCaching
    {
        private readonly Lazy<IEvalCriteriaGroupBusiness> _evalCriteriaGrpBusiness;

        public EvalCriteriaGroupCaching(/*string appid, int uid*/)
             
        {
            _evalCriteriaGrpBusiness = new Lazy<IEvalCriteriaGroupBusiness>(() => new EvalCriteriaGroupBusiness(appid, uid));
        }
        /// <summary>
        /// Lấy danh sách Hạng mục của tiểu chí đánh giá
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EvalCriteriaGroupContract> GetListCriteriaGroup(int iEvalCriteriaGrpId = 0)
        {
            return _evalCriteriaGrpBusiness.Value.GetListCriteriaGroup(iEvalCriteriaGrpId);
        }

        /// <summary>
        /// Thêm mới nhóm tiêu chí đánh giá
        /// </summary>
        /// <param name="objCriteriaGroupInfo"></param>
        /// <returns></returns>
        public CUDReturnMessage AddEvalCriteriaGroup(EvalCriteriaGroupContract objCriteriaGroupInfo)
        {
            return _evalCriteriaGrpBusiness.Value.AddEvalCriteriaGroup(objCriteriaGroupInfo);
        }

        /// <summary>
        /// Cập nhật lại thông tin nhóm tiêu chí đánh giá
        /// </summary>
        /// <param name="objCriteriaGroupInfo"></param>
        /// <returns></returns>
        public CUDReturnMessage UpdateEvalCriteriaGroup(EvalCriteriaGroupContract objCriteriaGroupInfo)
        {
            return _evalCriteriaGrpBusiness.Value.UpdateEvalCriteriaGroup(objCriteriaGroupInfo);
        }
        /// <summary>
        /// Xóa nhóm tiêu chí đánh giá
        /// </summary>
        /// <param name="evalCriteriaGroupId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteEvalCriteriaGroup(int evalCriteriaGroupId)
        {
            return _evalCriteriaGrpBusiness.Value.DeleteEvalCriteriaGroup(evalCriteriaGroupId);
        }

        public PagedList<EvalCriteriaGroupContract> FindCriteriaGroups(string kw = "", int iEvalCriteriaGrpId = 0, int iPage = 1, int iPageSize = 20)
        {
            return _evalCriteriaGrpBusiness.Value.FindCriteriaGroups(kw, iEvalCriteriaGrpId, iPage, iPageSize);
        }
    }
}
