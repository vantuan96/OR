using System;
using System.Collections.Generic;
using Business.Core;
using Contract.Shared;
using Contract.User;
using Contract.OR;
using DataAccess.Models;

namespace Caching.Core
{
    public interface IUserMngtCaching
    {
        List<AdminRoleContract> GetListRole(int currentRoleSort);

        PagedList<UserItemContract> GetListUser( List<int> locationIds,int roleId, string searchText, int pageSize, int page);

        UserItemContract FetchUserToUpdate(int userId, int currentUserId);

        CUDReturnMessage CreateUpdateUser(CreateUpdateUserContract user);

        CUDReturnMessage ResetPassword(int userId);

        CUDReturnMessage LockUnLockUser(int userId, bool lockStatus);

        CUDReturnMessage DeleteUser(int userId);
        List<AdminUserInfo> GetAllUser(int userId);
        CUDReturnMessage CreateUpdateORUserInfo(ORUserInfoContract data,int uId);
        CUDReturnMessage UpdateEmailOrPhone(int id, string email, string phone);
        CUDReturnMessage CreateUpdateUserInfoPosition(int userId, List<int> positionIds);

        List<ORUserInforPositionContract> GetPositionByUserId(int userId);

        List<ORPositionType> GetPositions();
        AdminUser GetFullNameById(int id);
    }

    public class UserMngtCaching : BaseCaching, IUserMngtCaching
    {
        private Lazy<IUserMngtBusiness> lazyUserMngtBusiness;

        public UserMngtCaching(/*string appid, int uid*/)  
        {
            lazyUserMngtBusiness = new Lazy<IUserMngtBusiness>(() => new UserMngtBusiness(appid, uid));
        }

        public List<AdminRoleContract> GetListRole(int currentRoleSort)
        {
            return lazyUserMngtBusiness.Value.GetListRole(currentRoleSort);
        }

        public PagedList<UserItemContract> GetListUser(List<int> locationIds, int roleId,  string searchText, int pageSize, int page)
        {
            return lazyUserMngtBusiness.Value.GetListUser(locationIds, roleId,  searchText, pageSize, page);
        }

        public UserItemContract FetchUserToUpdate(int userId, int currentUserId)
        {
            return lazyUserMngtBusiness.Value.FetchUserToUpdate(userId, currentUserId);
        }

        public CUDReturnMessage CreateUpdateUser(CreateUpdateUserContract user)
        {
            return lazyUserMngtBusiness.Value.CreateUpdateUser(user);
        }

        public CUDReturnMessage ResetPassword(int userId)
        {
            return lazyUserMngtBusiness.Value.ResetPassword(userId);
        }

        public CUDReturnMessage LockUnLockUser(int userId, bool lockStatus)
        {
            return lazyUserMngtBusiness.Value.LockUnLockUser(userId, lockStatus);
        }

        public CUDReturnMessage DeleteUser(int userId)
        {
            return lazyUserMngtBusiness.Value.DeleteUser(userId);
        }

        public List<AdminUserInfo> GetAllUser(int userId)
        {
            return lazyUserMngtBusiness.Value.GetAllUser(userId);
        }

        public CUDReturnMessage CreateUpdateORUserInfo(ORUserInfoContract data, int uId)
        {
            return lazyUserMngtBusiness.Value.CreateUpdateORUserInfo(data,uId);
        }
        public CUDReturnMessage UpdateEmailOrPhone(int id, string email, string phone)
        {
            return lazyUserMngtBusiness.Value.UpdateEmailOrPhone(id, email, phone);
        }
        
        public CUDReturnMessage CreateUpdateUserInfoPosition(int userId, List<int> positionIds)
        {
            return lazyUserMngtBusiness.Value.CreateUpdateUserInfoPosition(userId, positionIds);
        }

        public List<ORUserInforPositionContract> GetPositionByUserId(int userId)
        {
            return lazyUserMngtBusiness.Value.GetPositionByUserId(userId);
        }

        public List<ORPositionType> GetPositions()
        {
            return lazyUserMngtBusiness.Value.GetPositions();
        }

        public AdminUser GetFullNameById(int id)
        {
            return lazyUserMngtBusiness.Value.GetFullNameById(id);
        }
    }
}
