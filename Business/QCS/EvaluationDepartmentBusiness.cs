using System;
using System.Collections.Generic;
using BMS.Contract.Qcs;
using BMS.DataAccess.DAO.QCS;
using BMS.Contract.Shared;
using System.Linq;
namespace BMS.Business.QCS
{
    public interface IEvaluationDepartmentBusiness
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
        /// Them/hieu chinh bộp phận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateAdminDepartment(AdminDepartmentContract model);
        /// <summary>
        /// Xoa bộp phận
        /// </summary>
        CUDReturnMessage DeleteAdminDepartment(int deptId);

    }

    public class EvaluationDepartmentBusiness : BaseBusiness, IEvaluationDepartmentBusiness
    {
        private readonly Lazy<IEvaluationDepartmentAccess> _evaluationDepartmentAccess;

        public EvaluationDepartmentBusiness(string appid, int uid) : base(appid, uid)
        {
            _evaluationDepartmentAccess = new Lazy<IEvaluationDepartmentAccess>(() => new EvaluationDepartmentAccess(appid, uid));
        }

        public IEnumerable<AdminDepartmentContract> GetListDepartments()
        {
            var query = _evaluationDepartmentAccess.Value.GetListDepartmentMasterSite();
            var result = query.Select(dept => new AdminDepartmentContract
            {
                DeptId = dept.DeptId,
                DeptName = dept.DeptName,
                DeptCode = dept.DeptCode,
                DeptParentId = dept.DeptParentId ?? 0,
                CreatedDate = dept.CreatedDate,
                CreatedBy = dept.CreatedBy,
                LastUpdatedDate = dept.LastUpdatedDate,
                LastUpdatedBy = dept.LastUpdatedBy
            });
            return result;
        }

        public AdminDepartmentContract GetDepartmentById(int deptId)
        {
            return _evaluationDepartmentAccess.Value.GetDepartmentById(deptId);
        }


        public PagedList<AdminDepartmentContract> GetListDepartmentMasterSite(int page, int pageSize)
        {
            var query = _evaluationDepartmentAccess.Value.GetListDepartmentMasterSite();
            var result = new PagedList<AdminDepartmentContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderBy(dept => dept.DeptName).Skip((page - 1) * pageSize).Take(pageSize).Select(dept => new AdminDepartmentContract
                {
                    DeptId = dept.DeptId,
                    DeptName = dept.DeptName,
                    DeptCode = dept.DeptCode,
                    DeptParentId = dept.DeptParentId ?? 0,
                    CreatedDate = dept.CreatedDate,
                    CreatedBy = dept.CreatedBy,
                    LastUpdatedDate = dept.LastUpdatedDate,
                    LastUpdatedBy = dept.LastUpdatedBy
                }).ToList();
            }

            return result;
        }
        public CUDReturnMessage InsertUpdateAdminDepartment(AdminDepartmentContract model)
        {
            if (model.DeptId == 0)
            {
                return _evaluationDepartmentAccess.Value.InsertDepartment(model);
            }
            else
            {
                return _evaluationDepartmentAccess.Value.UpdateDepartment(model);
            }
        }

        public CUDReturnMessage DeleteAdminDepartment(int deptId)
        {
            return _evaluationDepartmentAccess.Value.DeleteAdminDepartment(deptId);
        }

    }
}