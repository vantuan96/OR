using System;
using System.Linq;
using System.Collections.Generic;
using BMS.Contract.Qcs;
using BMS.DataAccess.DAO.QCS;
using BMS.Contract.Shared;

namespace BMS.Business.QCS
{
    public interface IEvaluationCategoryBusiness
    {
        IEnumerable<EvaluationCategoryContract> GetListEvaluationCategory(int iEvalTypeId = 0, int iEvalCateGroup = -1);

        EvaluationCategoryContract FindEvalCategory(int iEvalCateId);

        CUDReturnMessage AddEvalCategory(EvaluationCategoryContract objEvalCategory);

        CUDReturnMessage UpdateEvalCategory(EvaluationCategoryContract objEvalCategory);

        CUDReturnMessage DeleteEvalCategory(int iEvalCategoryId);
    }

    public class EvaluationCategoryBusiness : BaseBusiness, IEvaluationCategoryBusiness
    {
        private readonly Lazy<IEvaluationCategoryAccess> _evalCategoryAccess;

        public EvaluationCategoryBusiness(string appId, int uid) : base(appId, uid)
        {
            _evalCategoryAccess = new Lazy<IEvaluationCategoryAccess>(()=> new EvaluationCategoryAccess(appId,uid));
        }

        /// <summary>
        /// Lấy danh sách hạng mục đánh giá
        /// </summary>
        /// <param name="iEvalCateGroup">Mã nhóm hạng mục đánh giá nếu truyền vào -1 là lấy tất cả</param>
        /// <returns>Danh sách EvaluationCategoryContract</returns>
        public IEnumerable<EvaluationCategoryContract> GetListEvaluationCategory(int iEvalTypeId = 0, int iEvalCateGroup = -1)
        {
            return _evalCategoryAccess.Value.GetListEvaluationCategory(iEvalTypeId, iEvalCateGroup)
                    .Select(c => new EvaluationCategoryContract
                    {
                        CateId = c.EvaluationCateId,
                        CateName = c.EvaluationCateName,
                        CateCode = c.EvaluationCateCode,
                        CateGroupId = c.EvaluationCateId,
                        CateGroupName = c.EvaluationCategoryGroup.EvaluationCateGroupName,
                        EvalTypeId = c.EvaluationCategoryGroup.SourceClientId
                    });
        }

        /// <summary>
        /// Thêm mới nhóm hạng mục đánh giá
        /// </summary>
        /// <param name="objEvalCategory">Thông tin nhóm hạng mục đánh giá cần tạo</param>
        /// <returns></returns>
        public CUDReturnMessage AddEvalCategory(EvaluationCategoryContract objEvalCategory)
        {
            return _evalCategoryAccess.Value.AddEvalCategory(objEvalCategory);
        }
        /// <summary>
        /// Cập nhật thông tin nhóm hạng mục đánh giá
        /// </summary>
        /// <param name="objEvalCategory">Thông tin nhóm hạng mục đánh giá cần cập nhật</param>
        /// <returns></returns>
        public CUDReturnMessage UpdateEvalCategory(EvaluationCategoryContract objEvalCategory)
        {
            return _evalCategoryAccess.Value.UpdateEvalCategory(objEvalCategory);
        }
        /// <summary>
        /// Xóa nhóm hạng mục đánh giá
        /// </summary>
        /// <param name="iEvalCategoryId">Mã nhóm hạng mục đánh giá</param>
        /// <returns></returns>
        public CUDReturnMessage DeleteEvalCategory(int iEvalCategoryId)
        {
            return _evalCategoryAccess.Value.DeleteEvalCategory(iEvalCategoryId);
        }

        /// <summary>
        /// Tìm thông tin nhóm chuyên mục theo mã
        /// </summary>
        /// <param name="iEvalCateId"></param>
        /// <returns></returns>
        public EvaluationCategoryContract FindEvalCategory(int iEvalCateId)
        {
            var evalCategory = _evalCategoryAccess.Value.GetListEvaluationCategory().Where(c => c.EvaluationCateId == iEvalCateId).Select(c => new EvaluationCategoryContract
            {
                CateId = c.EvaluationCateId,
                CateName = c.EvaluationCateName,
                CateCode = c.EvaluationCateCode,
                CateGroupId = c.EvaluationCateId,
                CateGroupName = c.EvaluationCategoryGroup.EvaluationCateGroupName,
                EvalTypeId = c.EvaluationCategoryGroup.SourceClientId
            }).FirstOrDefault();

            return evalCategory;
        }
    }
}