using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Contract.Qcs.Master;
using BMS.DataAccess.DAO.QCS;

namespace BMS.Business.QCS
{
    public interface IViolGroupBusiness 
    {
        IEnumerable<ViolGroupContract> GetViolationGroups();
    }

    public class ViolGroupBusiness : BaseBusiness, IViolGroupBusiness
    {
        private readonly Lazy<IViolGroupAccess> _violGroupAccess;
        public ViolGroupBusiness(string appid, int uid) : base(appid, uid) 
        {
            _violGroupAccess = new Lazy<IViolGroupAccess>(()=> new ViolGroupAccess(appid,uid));
        }
        /// <summary>
        /// Lấy danh sách nhóm lỗi
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ViolGroupContract> GetViolationGroups()
        {
            return _violGroupAccess.Value.GetViolationGroups();
        }
    }
}
