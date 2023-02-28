using System;
using System.Collections.Generic;
using BMS.Business.QCS;
using BMS.Contract.Qcs;
using BMS.Contract.Shared;

namespace BMS.Caching.QCS
{
    public interface IEvaluationCategoryCaching
    {
        IEnumerable<EvaluationCategoryContract> GetListEvaluationCategory(int iEvalTypeId = 0, int iEvalCateGroup = -1);

        EvaluationCategoryContract FindEvalCategory(int iEvalCateId);

        CUDReturnMessage AddEvalCategory(EvaluationCategoryContract objEvalCategory);

        CUDReturnMessage UpdateEvalCategory(EvaluationCategoryContract objEvalCategory);

        CUDReturnMessage DeleteEvalCategory(int iEvalCategoryId);
    }

    public class EvaluationCategoryCaching : BaseCaching, IEvaluationCategoryCaching
    {
        private readonly Lazy<IEvaluationCategoryBusiness> _evalCategoryBusiness;

        public EvaluationCategoryCaching(/*string appid, int uid*/)  
        {
            _evalCategoryBusiness= new Lazy<IEvaluationCategoryBusiness>(()=>new EvaluationCategoryBusiness(appid,uid));
        }
        /// <summary>
        /// Lấy danh sách hạng mục đánh giá
        /// </summary>
        /// <param name="iEvalCateGroup">Mã nhóm hạng mục đánh giá nếu truyền vào -1 là lấy tất cả</param>
        /// <returns>Danh sách EvaluationCategoryContract</returns>
        public IEnumerable<EvaluationCategoryContract> GetListEvaluationCategory(int iEvalTypeId = 0, int iEvalCateGroup = -1)
        {
            return _evalCategoryBusiness.Value.GetListEvaluationCategory(iEvalTypeId, iEvalCateGroup);
        }
        /// <summary>
        /// Thêm mới nhóm hạng mục đánh giá
        /// </summary>
        /// <param name="objEvalCategory">Thông tin nhóm hạng mục đánh giá cần tạo</param>
        /// <returns></returns>
        public CUDReturnMessage AddEvalCategory(EvaluationCategoryContract objEvalCategory)
        {
            return _evalCategoryBusiness.Value.AddEvalCategory(objEvalCategory);
        }
        /// <summary>
        /// Cập nhật thông tin nhóm hạng mục đánh giá
        /// </summary>
        /// <param name="objEvalCategory">Thông tin nhóm hạng mục đánh giá cần cập nhật</param>
        /// <returns></returns>
        public CUDReturnMessage UpdateEvalCategory(EvaluationCategoryContract objEvalCategory)
        {
            return _evalCategoryBusiness.Value.UpdateEvalCategory(objEvalCategory);
        }
        /// <summary>
        /// Xóa nhóm hạng mục đánh giá
        /// </summary>
        /// <param name="iEvalCategoryId">Mã nhóm hạng mục đánh giá</param>
        /// <returns></returns>
        public CUDReturnMessage DeleteEvalCategory(int iEvalCategoryId)
        {
            return _evalCategoryBusiness.Value.DeleteEvalCategory(iEvalCategoryId);
        }
        /// <summary>
        /// Tìm thông tin nhóm chuyên mục đánh giá
        /// </summary>
        /// <param name="iEvalCateId"></param>
        /// <returns></returns>
        public EvaluationCategoryContract FindEvalCategory(int iEvalCateId)
        {
            return _evalCategoryBusiness.Value.FindEvalCategory(iEvalCateId);
        }
    }
}