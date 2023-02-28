using System;
using System.Collections.Generic;
using BMS.Business.QCS;
using BMS.Contract.Qcs.Master;

namespace BMS.Caching.QCS
{
    public interface IViolResolveTypeCaching
    {
        IEnumerable<ViolResolveTypeContract> GetListViolResolveType();
    }
    public class ViolResolveTypeCaching : BaseCaching, IViolResolveTypeCaching
    {
        private readonly Lazy<IViolResolveTypeBusiness> _violResolveTypeBusiness;
        public ViolResolveTypeCaching(/*string appid, int uid*/)   
        {
            _violResolveTypeBusiness = new Lazy<IViolResolveTypeBusiness>(()=> new ViolResolveTypeBusiness(appid,uid));
        }
        /// <summary>
        /// Lấy danh sách hình thức xử lý
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ViolResolveTypeContract> GetListViolResolveType()
        {
            return _violResolveTypeBusiness.Value.GetListViolResolveType();
        }
    }
}
