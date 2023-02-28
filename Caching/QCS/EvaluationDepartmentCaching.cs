using System;
using System.Collections.Generic;
using BMS.Business.QCS;
using BMS.Contract.Qcs;
using BMS.Contract.Shared;

namespace BMS.Caching.QCS
{
    public interface IEvaluationDepartmentCaching
    {
        /// <summary>
        /// Lấy danh sách bộ phận đánh giá
        /// </summary>
        /// <returns></returns>
        IEnumerable<AdminDepartmentContract> GetListDepartments();
        /// <summary>
        /// Lấy thông tin bộ phận đánh giá theo mã
        /// </summary>
        /// <param name="deptId">Mã bộ phận đánh giá</param>
        /// <returns></returns>
        AdminDepartmentContract GetDepartmentById(int deptId);

        /// <summary>
        /// Lấy danh sách bộ phận 
        /// </summary>
        /// <returns></returns>
        PagedList<AdminDepartmentContract> GetListDepartmentMasterSite(int page, int pageSize);

        /// <summary>
        /// Them/hieu chinh bo phan
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateAdminDepartment(AdminDepartmentContract model);

        /// <summary>
        /// Xoa bo phan
        /// </summary>
        CUDReturnMessage DeleteAdminDepartment(int deptId);
    }

    public class EvaluationDepartmentCaching : BaseCaching, IEvaluationDepartmentCaching
    {
        private readonly Lazy<IEvaluationDepartmentBusiness> _evaluationDepartmentBusiness;

        public EvaluationDepartmentCaching(/*string appid, int uid*/)  
        {
            _evaluationDepartmentBusiness = new Lazy<IEvaluationDepartmentBusiness>(() => new EvaluationDepartmentBusiness(appid, uid));
        }

        public IEnumerable<AdminDepartmentContract> GetListDepartments()
        {
            return _evaluationDepartmentBusiness.Value.GetListDepartments();
        }

        public AdminDepartmentContract GetDepartmentById(int deptId)
        {
            return _evaluationDepartmentBusiness.Value.GetDepartmentById(deptId);
        }

        public PagedList<AdminDepartmentContract> GetListDepartmentMasterSite(int page, int pageSize)
        {
            return _evaluationDepartmentBusiness.Value.GetListDepartmentMasterSite(page, pageSize);
        }

        public CUDReturnMessage InsertUpdateAdminDepartment(AdminDepartmentContract model)
        {
            return _evaluationDepartmentBusiness.Value.InsertUpdateAdminDepartment(model);
        }

        public CUDReturnMessage DeleteAdminDepartment(int deptId)
        {
            return _evaluationDepartmentBusiness.Value.DeleteAdminDepartment(deptId);
        }
    }
}