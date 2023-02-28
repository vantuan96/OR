using System;
using System.Collections.Generic;
using BMS.Business.QCS;
using BMS.Contract.Qcs.Master;

namespace BMS.Caching.QCS
{
    public interface IViolGroupCaching
    {
        IEnumerable<ViolGroupContract> GetViolationGroups();
    }

    public class ViolGroupCaching : BaseCaching, IViolGroupCaching
    {
        private readonly Lazy<IViolGroupBusiness> _violGroupBusiness;
        public ViolGroupCaching(/*string appid, int uid*/)
             
        {
            _violGroupBusiness = new Lazy<IViolGroupBusiness>(() => new ViolGroupBusiness(appid, uid));
        }
        /// <summary>
        /// Lấy danh sách nhóm lỗi
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ViolGroupContract> GetViolationGroups()
        {
            return _violGroupBusiness.Value.GetViolationGroups();
        }
    }
}
