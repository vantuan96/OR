using System;
using System.Collections.Generic;
using System.Linq;
using Contract.AdminAction;
using Contract.Shared;
using Contract.User;
using DataAccess.Models;

namespace DataAccess
{
    public interface IAdminRoleDataAccess
    {        
        IQueryable<Models.AdminUser_Role> GetGrantedRoleByUserId(int userId);

        IQueryable<Models.AdminRole> GetListRole(int sort);

        /// <summary>
        /// Thêm và chỉnh sửa Role
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
        /// Xóa role
        /// </summary>
        /// <param name="rId"></param>
        /// <returns></returns>
        CUDReturnMessage DeleteRole(int rId);
    }

    public class AdminRoleDataAccess : BaseDataAccess, IAdminRoleDataAccess
    {
        public AdminRoleDataAccess(string appid, int uid) : base(appid, uid)
        {
        }

        public IQueryable<AdminUser_Role> GetGrantedRoleByUserId(int userId)
        {
            var query = DbContext.AdminUser_Role.Where(r => r.IsDeleted == false && r.AdminRole.IsDeleted == false);

            if (userId > 0)
            {
                query = query.Where(r => r.UId == userId);
            }

            return query;
        }

        public IQueryable<Models.AdminRole> GetListRole(int sort)
        {
            var query = DbContext.AdminRoles.Where(r => r.IsDeleted == false);

            if (sort > 0)
            {
                query = query.Where(r => r.Sort >= sort);
            }

            return query;
        }

        public CUDReturnMessage InsertUpdateRole(AdminRoleContract data) {

            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            if (DbContext.AdminRoles.Where(r => r.RoleName == data.RoleName && r.RId != data.RoleId && !r.IsDeleted).Any())
            {
                return new CUDReturnMessage(ResponseCode.AdminRole_DuplicateKey);
            }

            if (data.RoleId == 0)
            {
                Models.AdminRole newRole = new Models.AdminRole()
                {
                    RoleName = data.RoleName,
                    Sort = data.Sort,
                    IsDeleted = false,
                    Visible = true,
                    CreatedBy = uid,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = uid,
                    LastUpdatedDate = DateTime.Now
                };
                DbContext.AdminRoles.Add(newRole);
                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.AdminRole_SuccessCreate);
            }
            else {
                var role = DbContext.AdminRoles.SingleOrDefault(r => r.RId == data.RoleId);
                if (role == null)
                    return new CUDReturnMessage(ResponseCode.Error);

                role.RoleName = data.RoleName;
                role.LastUpdatedBy = uid;
                role.Sort = data.Sort;
                role.LastUpdatedDate = DateTime.Now;
                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.AdminRole_SuccessUpdate);
            }

        }

        public CUDReturnMessage InsertUpdateRoleGroupAction(InsertUpdateAdminRoleGroupActionContract data) {
            //bỏ map
            var deleteRoleGroupActions = DbContext.AdminRole_GroupAction.Where(r => r.RId == data.RId && !data.GroupActions.Contains(r.GaId) && !r.IsDeleted).ToList();

            if (deleteRoleGroupActions != null)
            {
                foreach (var item in deleteRoleGroupActions)
                {
                    item.IsDeleted = true;
                    item.LastUpdatedBy = uid;
                    item.LastUpdatedDate = DateTime.Now;
                }
            }

            //lay danh sách đã bị xóa
            var addRoleGroupActions = DbContext.AdminRole_GroupAction.Where(r => r.RId == data.RId && data.GroupActions.Contains(r.GaId) && r.IsDeleted).ToList();


            foreach (var gaId in data.GroupActions)
            {
                if (addRoleGroupActions != null && addRoleGroupActions.Any(r=> r.GaId == gaId))
                {
                    var updateData = addRoleGroupActions.Where(r => r.GaId == gaId).FirstOrDefault();
                    updateData.IsDeleted = false;
                    updateData.LastUpdatedBy = uid;
                    updateData.LastUpdatedDate = DateTime.Now;
                }
                else
                {
                    var newRoleGroupActions = DbContext.AdminRole_GroupAction.FirstOrDefault(r => r.RId == data.RId && r.GaId == gaId);

                    if (newRoleGroupActions == null)
                    {
                        newRoleGroupActions = new Models.AdminRole_GroupAction()
                        {
                            RId = data.RId,
                            GaId = gaId,
                            CreatedBy = uid,
                            CreatedDate = DateTime.Now,
                            LastUpdatedBy = uid,
                            LastUpdatedDate = DateTime.Now
                        };
                        DbContext.AdminRole_GroupAction.Add(newRoleGroupActions);
                    }
                    
                }
            }
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.AdminRoleGroupAction_SuccessUpdate);
        }


        public CUDReturnMessage DeleteRole(int rId)
        {
            var role = DbContext.AdminRoles.FirstOrDefault(r => r.RId == rId);
            if (role == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            role.IsDeleted = true;
            role.LastUpdatedBy = uid;
            role.LastUpdatedDate = DateTime.Now;
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.AdminRole_SuccessUpdate);
        }
    }
}
