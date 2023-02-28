using System;
using System.Collections.Generic;
using BMS.Business.QCS;
using BMS.Contract.Qcs.Master;

namespace BMS.Caching.QCS
{
    public interface IViolDepartmentCaching
    {
        IEnumerable<ViolDepartmentContract> GetListDepartments();
    }

    public class ViolDepartmentCaching: BaseCaching, IViolDepartmentCaching
    {
        private readonly Lazy<IViolDepartmentBusiness> _violDeptBusiness;
        public ViolDepartmentCaching(/*string appid, int uid*/)
             
        {
            _violDeptBusiness = new Lazy<IViolDepartmentBusiness>(() => new ViolDepartmentBusiness(appid, uid));
        }
        /// <summary>
        /// Lấy bộ phận của nhân viên
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ViolDepartmentContract> GetListDepartments()
        {
            return _violDeptBusiness.Value.GetListDepartments();
        }
    }
}
