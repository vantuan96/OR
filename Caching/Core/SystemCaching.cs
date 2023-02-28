using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Core;
using Contract.AdminAction;
using Contract.Shared;
using Contract.User;

namespace Caching.Core
{
    public interface ISystemCaching
    {
        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        /// <returns></returns>
        bool HealthCheckDbConnection();


        /// <summary>
        /// lấy danh sách toàn bộ controller
        /// </summary>
        List<AdminControllerContract> GetAllController();

        /// <summary>
        /// import controllers và actions
        /// </summary>
        /// <returns></returns>
        CUDReturnMessage ImportActions(List<AdminControllerContract> data);

        /// <summary>
        /// lấy danh sách tất cả group action
        /// </summary>
        /// <returns></returns>
        List<AdminGroupActionContract> GetListGroupAction(bool? isDeleted, int rId);

        /// <summary>
        ///  lấy danh sách tất cả group action map
        /// </summary>
        /// <returns></returns>
        List<AdminGroupActionMapContract> GetAllGroupActionMap();

        /// <summary>
        /// nhập group action
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        CUDReturnMessage ImportGroupActions(List<AdminGroupActionContract> data);

        /// <summary>
        /// nhập group action map
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        CUDReturnMessage ImportGroupActionMaps(List<AdminGroupActionMapContract> data);

        /// <summary>
        /// thêm và chỉnh sửa role
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateRole(AdminRoleContract data);

        /// <summary>
        /// map role và group action
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateRoleGroupAction(InsertUpdateAdminRoleGroupActionContract data);
        
        /// <summary>
        /// lấy danh sách tracking
        /// </summary>
        /// <returns></returns>
        PagedList<UserTrackingContract> GetListUserTracking(DateTime fromDate, DateTime toDate, string keyword, int page, int pageSize);

        /// <summary>
        /// Xóa role
        /// </summary>
        /// <param name="rId"></param>
        /// <returns></returns>
        CUDReturnMessage DeleteRole(int rId);

        
    }

    public class SystemCaching : BaseCaching, ISystemCaching
    {
        private Lazy<ISystemBusiness> lazy;

        public SystemCaching(/*string appid, int uid*/)  
        {
            lazy = new Lazy<ISystemBusiness>(() => new SystemBusiness(appid, uid));
        }

        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        /// <returns></returns>
        public bool HealthCheckDbConnection()
        {
            var data = lazy.Value.HealthCheckDbConnection();

            return data;
        }

        public List<AdminControllerContract> GetAllController()
        {
            return lazy.Value.GetAllController();
        }

        public CUDReturnMessage ImportActions(List<AdminControllerContract> data)
        {
            return lazy.Value.ImportActions(data);
        }

        public List<AdminGroupActionContract> GetListGroupAction(bool? isDeleted, int rId)
        {
            return lazy.Value.GetListGroupAction(isDeleted, rId);
        }

        public List<AdminGroupActionMapContract> GetAllGroupActionMap()
        {
            return lazy.Value.GetAllGroupActionMap();
        }

        public CUDReturnMessage ImportGroupActions(List<AdminGroupActionContract> data)
        {
            return lazy.Value.ImportGroupActions(data);
        }

        public CUDReturnMessage ImportGroupActionMaps(List<AdminGroupActionMapContract> data)
        {
            return lazy.Value.ImportGroupActionMaps(data);
        }

        /// <summary>
        /// thêm và chỉnh sửa role
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateRole(AdminRoleContract data)
        {
            return lazy.Value.InsertUpdateRole(data);
        }

        /// <summary>
        /// map role và group action
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateRoleGroupAction(InsertUpdateAdminRoleGroupActionContract data)
        {
            return lazy.Value.InsertUpdateRoleGroupAction(data);
        }

        public PagedList<UserTrackingContract> GetListUserTracking(DateTime fromDate, DateTime toDate, string keyword, int page, int pageSize)
        {
            return lazy.Value.GetListUserTracking(fromDate, toDate, keyword, page, pageSize);
        }

        public CUDReturnMessage DeleteRole(int rId) {
            return lazy.Value.DeleteRole(rId);
        }
    }
}
