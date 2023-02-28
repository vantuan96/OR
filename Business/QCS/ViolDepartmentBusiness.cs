using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Contract.Qcs.Master;
using BMS.DataAccess.DAO.QCS;

namespace BMS.Business.QCS
{
    public interface IViolDepartmentBusiness 
    {
        IEnumerable<ViolDepartmentContract> GetListDepartments();
    }
    public class ViolDepartmentBusiness : BaseBusiness, IViolDepartmentBusiness
    {
        private readonly Lazy<IViolDepartmentAccess> _violDeptAccess;
        public ViolDepartmentBusiness(string appid, int uid) : base(appid, uid) {
            _violDeptAccess = new Lazy<IViolDepartmentAccess>(() => new ViolDepartmentAccess(appid, uid));
        }
        /// <summary>
        /// Lấy bộ phận của nhân viên
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ViolDepartmentContract> GetListDepartments()
        {
            return _violDeptAccess.Value.GetListDepartments();
        }
    }
}
