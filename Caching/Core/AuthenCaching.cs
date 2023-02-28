using System;
using Business.Core;
using Contract.Shared;
using Contract.User;

namespace Caching.Core
{
    public interface IAuthenCaching
    {
        LoginContract CheckLoginByForCheckLogin(string key);
        LoginContract CheckLoginByUsername(string username);

        LoginContract CheckLogin(string username, string password);
        CUDReturnMessage UpdateLoginLogout(LoginContract entity,bool isLogon=false);
        MemberExtendContract GetMemberDetail(int userId);

        CUDReturnMessage InsertUserTracking(string log, int userId);

        CUDReturnMessage UpdateLogout(LoginContract entity, bool isLogon = false);
    }

    public class AuthenCaching : BaseCaching, IAuthenCaching
    {
        private Lazy<IAuthenBusiness> lazyAuthenBusiness;
        
        public AuthenCaching(/*string appid, int uid*/)  
        {
            lazyAuthenBusiness = new Lazy<IAuthenBusiness>(() => new AuthenBusiness(appid, uid));
        }
        public LoginContract CheckLoginByForCheckLogin(string key)
        {
            return lazyAuthenBusiness.Value.CheckLoginByForCheckLogin(key);
        }
        public LoginContract CheckLoginByUsername(string username)
        {
            return lazyAuthenBusiness.Value.CheckLoginByUsername(username);
        }

        public LoginContract CheckLogin(string username, string password)
        {
            return lazyAuthenBusiness.Value.CheckLogin(username, password);
        }
        public CUDReturnMessage UpdateLoginLogout(LoginContract entity, bool isLogon=false)
        {
            return lazyAuthenBusiness.Value.UpdateLoginLogout(entity,isLogon);
        }

        public CUDReturnMessage UpdateLogout(LoginContract entity, bool isLogon = false)
        {
            return lazyAuthenBusiness.Value.UpdateLogout(entity, isLogon);
        }
        public MemberExtendContract GetMemberDetail(int userId)
        {
            return lazyAuthenBusiness.Value.GetMemberDetail(userId);
        }

        public CUDReturnMessage InsertUserTracking(string log, int userId)
        {
            return lazyAuthenBusiness.Value.InsertUserTracking(log, userId);
        }
    }
}
