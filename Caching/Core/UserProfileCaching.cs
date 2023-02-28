using System;
using Business.Core;
using Contract.Shared;
using Contract.User;

namespace Caching.Core
{
    public interface IUserProfileCaching
    {
        UserProfileContract GetUserProfile(int userId);

        CUDReturnMessage ChangePassword(int userId, string oldPassword, string newPassword);
    }

    public class UserProfileCaching : BaseCaching, IUserProfileCaching
    {
        private Lazy<IUserProfileBusiness> lazyUserProfileBusiness;

        public UserProfileCaching(/*string appid, int uid*/)  
        {
            lazyUserProfileBusiness = new Lazy<IUserProfileBusiness>(() => new UserProfileBusiness(appid, uid));
        }

        public UserProfileContract GetUserProfile(int userId)
        {
            return lazyUserProfileBusiness.Value.GetUserProfile(userId);
        }

        public CUDReturnMessage ChangePassword(int userId, string oldPassword, string newPassword)
        {
            return lazyUserProfileBusiness.Value.ChangePassword(userId, oldPassword, newPassword);
        }
    }
}
