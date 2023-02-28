using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Contract.Qcs.Master;
using BMS.DataAccess.DAO.QCS;

namespace BMS.Business.QCS
{
    public interface IViolResolveTypeBusiness
    {
        IEnumerable<ViolResolveTypeContract> GetListViolResolveType();
    }

    public class ViolResolveTypeBusiness : BaseBusiness, IViolResolveTypeBusiness
    {
        private readonly Lazy<IViolResolveTypeAccess> _violResolveTypeAccess;
        public ViolResolveTypeBusiness(string appid, int uid) : base(appid, uid) 
        {
            _violResolveTypeAccess = new Lazy<IViolResolveTypeAccess>(() => new ViolResolveTypeAccess(appid, uid));
        }
        /// <summary>
        /// Lấy danh sách hình thức xử lý
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ViolResolveTypeContract> GetListViolResolveType()
        {
            return _violResolveTypeAccess.Value.GetListViolResolveType();
        }
    }
}
