using System;
using System.Collections.Generic;
using System.Linq;
using Contract.AdminAction;
using Contract.Shared;
using Contract.User;
using DataAccess;
using DataAccess.DAO;

namespace Business.Core
{
    public interface ISystemBusiness
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

    public class SystemBusiness : BaseBusiness, ISystemBusiness
    {
        private Lazy<ISystemDataAccess> lazyAccess;
        private Lazy<IAdminActionDataAccess> lazyAdminActionAccess;
        private Lazy<IAdminGroupActionDataAccess> lazyAdminGroupActionAccess;
        private Lazy<IAdminRoleDataAccess> lazyAdminRoleDataAccess;
        private Lazy<IAdminUsersDataAccess> lazyAdminUsersDataAccess;

        public SystemBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyAccess = new Lazy<ISystemDataAccess>(() => new SystemDataAccess(appid, uid));
            lazyAdminActionAccess = new Lazy<IAdminActionDataAccess>(() => new AdminActionDataAccess(appid, uid));
            lazyAdminGroupActionAccess = new Lazy<IAdminGroupActionDataAccess>(() => new AdminGroupActionDataAccess(appid, uid));
            lazyAdminRoleDataAccess = new Lazy<IAdminRoleDataAccess>(() => new AdminRoleDataAccess(appid, uid));
            lazyAdminUsersDataAccess = new Lazy<IAdminUsersDataAccess>(() => new AdminUsersDataAccess(appid, uid));
        }

        /// <summary>
        /// Kiểm tra kết nối database
        /// </summary>
        /// <returns></returns>
        public bool HealthCheckDbConnection()
        {
            var data = lazyAccess.Value.HealthCheckDbConnection();

            return data;
        }

        public List<AdminControllerContract> GetAllController()
        {
            var query = lazyAdminActionAccess.Value.GetAllController();

            return query.Select(r => new AdminControllerContract
            {
                CId = r.CId,
                ControllerDisplayName = r.ControllerDisplayName,
                ControllerName = r.ControllerName,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                CssIcon = r.CssIcon,
                IsDeleted = r.IsDeleted,
                LastUpdatedBy = r.LastUpdatedBy,
                LastUpdatedDate = r.LastUpdatedDate,
                Sort = r.Sort,
                Visible = r.Visible,
                ListActions = r.AdminActions.Select(a => new AdminActionFullContract
                {
                    ActionDisplayName = a.ActionDisplayName,
                    ActionName = a.ActionName,
                    AId = a.AId,
                    CreatedBy = a.CreatedBy,
                    CreatedDate = a.CreatedDate,
                    CssIcon = a.CssIcon,
                    IsDefault = a.IsDefault,
                    IsDeleted = a.IsDeleted,
                    LastUpdatedBy = a.LastUpdatedBy,
                    LastUpdatedDate = a.LastUpdatedDate,
                    PublicStatus = a.PublicStatus,
                    ShowMenuStatus = a.ShowMenuStatus,
                    Sort = a.Sort,
                    Visible = a.Visible
                }).ToList()
            }).ToList();
        }

        public CUDReturnMessage ImportActions(List<AdminControllerContract> data)
        {
            return lazyAdminActionAccess.Value.ImportActions(data);
        }

        public List<AdminGroupActionContract> GetListGroupAction(bool? isDeleted, int rId)
        {
            var query = lazyAdminGroupActionAccess.Value.GetListGroupAction(isDeleted, rId);

            return query.Select(r => new AdminGroupActionContract
            {
                GaId = r.GaId,
                Name = r.Name,
                Description = r.Description,
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                LastUpdatedBy = r.LastUpdatedBy,
                LastUpdatedDate = r.LastUpdatedDate,
                IsDeleted = r.IsDeleted
            }).ToList();
        }


        public List<AdminGroupActionMapContract> GetAllGroupActionMap()
        {
            var query = lazyAdminGroupActionAccess.Value.GetAllGroupActionMap();

            return query.Select(r => new AdminGroupActionMapContract
            {
                GaId = r.GaId,
                AId = r.AId,                
                CreatedBy = r.CreatedBy,
                CreatedDate = r.CreatedDate,
                LastUpdatedBy = r.LastUpdatedBy,
                LastUpdatedDate = r.LastUpdatedDate,
                IsDeleted = r.IsDeleted
            }).ToList();
        }

        public CUDReturnMessage ImportGroupActions(List<AdminGroupActionContract> data)
        {
            return lazyAdminGroupActionAccess.Value.ImportGroupActions(data);
        }

        public CUDReturnMessage ImportGroupActionMaps(List<AdminGroupActionMapContract> data)
        {
            return lazyAdminGroupActionAccess.Value.ImportGroupActionMaps(data);
        }

        
        public PagedList<UserTrackingContract> GetListUserTracking(DateTime fromDate, DateTime toDate, string keyword, int page, int pageSize)
        {
            var query = lazyAdminUsersDataAccess.Value.GetListUserTracking(fromDate, toDate, keyword);
            var result = new PagedList<UserTrackingContract>(query.Count());
            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(r => r.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).Select(r => new UserTrackingContract
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    Email = r.AdminUser.Email,
                    FullName = r.AdminUser.Name,
                    CreatedDate = r.CreatedDate,
                    ContentTracking = r.ContentTracking
                }).ToList();
            }
            return result;
        }

        public CUDReturnMessage InsertUpdateRole(AdminRoleContract data)
        {
            return lazyAdminRoleDataAccess.Value.InsertUpdateRole(data);
        }

        public CUDReturnMessage InsertUpdateRoleGroupAction(InsertUpdateAdminRoleGroupActionContract data)
        {
            return lazyAdminRoleDataAccess.Value.InsertUpdateRoleGroupAction(data);
        }

        public CUDReturnMessage DeleteRole(int rId)
        {
            return lazyAdminRoleDataAccess.Value.DeleteRole(rId);
        }
    }
}