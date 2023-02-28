using System;
using System.Linq;
using VG.Common;
using Contract.Microsite;
using Contract.Shared;
using Contract.User;
using DataAccess;

namespace Business.Core
{
    public interface IAuthenBusiness
    {
        LoginContract CheckLoginByForCheckLogin(string key);
        LoginContract CheckLoginByUsername(string username);

        LoginContract CheckLogin(string username, string password);
        CUDReturnMessage UpdateLoginLogout(LoginContract entity, bool isLogon=false);

        MemberExtendContract GetMemberDetail(int userId);

        CUDReturnMessage InsertUserTracking(string log, int userId);
        CUDReturnMessage UpdateLogout(LoginContract entity, bool isLogon = false);

    }

    public class AuthenBusiness : BaseBusiness, IAuthenBusiness
    {
        public readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Lazy<IAdminUsersDataAccess> lazyAdminUsersDataAccess;
        private Lazy<IAdminRoleDataAccess> lazyAdminRoleDataAccess;
        private Lazy<IAdminActionDataAccess> lazyAdminActionDataAccess;

        public AuthenBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyAdminUsersDataAccess = new Lazy<IAdminUsersDataAccess>(() => new AdminUsersDataAccess(appid, uid));
            lazyAdminRoleDataAccess = new Lazy<IAdminRoleDataAccess>(() => new AdminRoleDataAccess(appid, uid));
            lazyAdminActionDataAccess = new Lazy<IAdminActionDataAccess>(() => new AdminActionDataAccess(appid, uid));
        }
        public LoginContract CheckLoginByForCheckLogin(string key)
        {
            try {
                var userInfo = lazyAdminUsersDataAccess.Value.GetUserForCheckLogin(username: key).SingleOrDefault();
                int iUserId = 0;
                int.TryParse(key, out iUserId);
                userInfo = (userInfo == null && iUserId>0) ? lazyAdminUsersDataAccess.Value.GetUserForCheckLogin(userId: iUserId).SingleOrDefault() : userInfo;
                userInfo = (userInfo == null) ? lazyAdminUsersDataAccess.Value.GetUserForCheckLogin(email: key).SingleOrDefault() : userInfo;
                userInfo = (userInfo == null) ? lazyAdminUsersDataAccess.Value.GetUserForCheckLogin(tokenKey: key).SingleOrDefault() : userInfo;
                //userInfo = (userInfo == null) ? lazyAdminUsersDataAccess.Value.GetUserForCheckLogin(clientIp: key).SingleOrDefault() : userInfo;

                if (userInfo == null)
                    return new LoginContract(ResponseCode.Login_HaveNotLogonPermit);

                if (userInfo.IsADAccount == false)
                    return new LoginContract(ResponseCode.Login_Failed);

                if (userInfo.IsActive == false)
                    return new LoginContract(ResponseCode.Login_UserNotActived);
                if (userInfo.IsLogon ==null || (userInfo.IsLogon!=null) && !userInfo.IsLogon.Value)
                    return new LoginContract(ResponseCode.Login_HaveNotLogonPermit);

                return new LoginContract
                {
                    Email = userInfo.Email,
                    FullName = userInfo.Name,
                    NickName = userInfo.Username,
                    UserId = userInfo.UId,
                    ClientIp = userInfo.LastLoginIp != null ? userInfo.LastLoginIp : string.Empty,
                    CurrentLoginFail = userInfo.FailedPasswordAttemptCount != null ? userInfo.FailedPasswordAttemptCount.Value : 0,
                    IsLogon = userInfo.IsLogon != null ? userInfo.IsLogon.Value : false,
                    TokenKey = userInfo.Token ?? userInfo.Token,
                    ResCode=ResponseCode.Successed
                };
            } catch(Exception ex)
            {
                log.Debug(string.Format("CheckLoginByForCheckLogin with key {0}",key));
                log.Debug(string.Format("Error CheckLoginByForCheckLogin: {0}",ex));
                return null;
            }
        }
        public LoginContract CheckLoginByUsername(string username)
        {
            var userInfo = lazyAdminUsersDataAccess.Value.GetUser(username: username).FirstOrDefault();
                        

            if (userInfo == null)
                return new LoginContract(ResponseCode.Login_HaveNotLogonPermit);

            if (userInfo.IsADAccount == false)
                return new LoginContract(ResponseCode.Login_Failed);

            if (userInfo.IsActive == false)
                return new LoginContract(ResponseCode.Login_UserNotActived);

            return new LoginContract
            {
                Email = userInfo.Email,
                FullName = userInfo.Name,
                NickName = userInfo.Username,
                UserId = userInfo.UId,
            };
        }

        public LoginContract CheckLogin(string username, string password)
        {
            //var userInfo = lazyAdminUsersDataAccess.Value.GetUser(username: username).FirstOrDefault();
            var userInfo = lazyAdminUsersDataAccess.Value.GetUser(email: username).FirstOrDefault();

            if (userInfo == null)
                return new LoginContract(ResponseCode.Login_Failed);

            if (userInfo.IsADAccount || string.IsNullOrEmpty(userInfo.PaswordSalt) || string.IsNullOrEmpty(userInfo.Password))
                return new LoginContract(ResponseCode.Login_Failed);
            
            string md5 = (password + userInfo.PaswordSalt).MD5Hash();
            if (userInfo.Password.Equals(md5) == false)
                return new LoginContract(ResponseCode.Login_Failed);

            if (userInfo.IsActive == false)
                return new LoginContract(ResponseCode.Login_UserNotActived);

            return new LoginContract
            {
                Email = userInfo.Email,
                FullName = userInfo.Name,
                NickName = userInfo.Username,
                UserId = userInfo.UId,
            };
        }
        public CUDReturnMessage UpdateLoginLogout(LoginContract entity, bool isLogon=false)
        {
            return lazyAdminUsersDataAccess.Value.UpdateLoginLogout(entity,isLogon);
        }
        public CUDReturnMessage UpdateLogout(LoginContract entity, bool isLogon = false)
        {
            return lazyAdminUsersDataAccess.Value.UpdateLogout(entity, isLogon);
        }
        public MemberExtendContract GetMemberDetail(int userId)
        {
            var query = lazyAdminUsersDataAccess.Value.GetUser(userId: userId, isActive: true);
            var userInfo = query.Select(r => new MemberExtendContract
            {
                UserId = r.UId,
                UserAccount=r.Username,
                Email = r.Email,
                PhoneNumber = r.PhoneNumber,
                DisplayName = r.Name,
                IsRequireChangePass = r.IsRequireChangePass,
                LineManagerId=r.LineManagerId,
                ListMicrosites = r.AdminUser_Microsite.Where(us => us.IsDeleted == false && us.Microsite.IsDeleted == false)
                    .Select(us => new MicrositeItemContract
                    {
                        MsId = us.MsId,
                        IsRootSite = us.Microsite.IsRootSite,
                        ReferenceCode = us.Microsite.ReferenceCode,
                        Title = us.Microsite.Name,
                        StatusId = us.Microsite.Status
                    }).ToList(),
                Locations = r.AdminUser_Location.Where(l => l.IsDeleted == false && l.Location.IsDeleted == false)
                    .Select(l => new LocationContract()
                    {
                        LocationId = l.Location.LocationId,
                        NameVN = l.Location.NameVN,
                        NameEN = l.Location.NameEN,
                        SloganVN = l.Location.SloganVN,
                        SloganEN = l.Location.SloganEN,
                        LogoName = l.Location.LogoName,
                        BackgroundName = l.Location.BackgroundName,
                        ColorCode = l.Location.ColorCode,
                        ParentId = l.Location.ParentId,
                        LevelNo = l.Location.LevelNo,
                        LevelPath = l.Location.LevelPath,
                        RootId = l.Location.RootId,
                        LayoutTypeId = l.Location.LayoutTypeId,
                        CreatedBy = l.Location.CreatedBy,
                        CreatedDate = l.Location.CreatedDate,
                        LastUpdatedBy = l.Location.LastUpdatedBy,
                        LastUpdatedDate = l.Location.LastUpdatedDate,
                        IsCurrent=l.IsCurrent,
                        IsDeleted = l.Location.IsDeleted,
                        QuestionGroupId = l.Location.QuestionGroupId,
                        ORUnitEmail=l.Location.ORUnitEmail,
                        ORUnitName=l.Location.ORUnitName
                    }).Distinct().ToList()
                //PnLs = r.AdminUser_PnL.Where(p => p.IsDeleted == false)
                //    .Select(
                //        p=> new AdminUserPnLContract()
                //        {
                //            UId=p.UId,
                //            PnLId = p.PnLId
                //        }
                //    ).ToList(),
                //Sites = r.AdminUser_PnL_Site.Where(p => p.IsDeleted == false)
                //    .Select(
                //        p => new AdminUserPnLSiteContract()
                //        {
                //            UId = p.UId,
                //            SiteId = p.SiteId
                //        }
                //    ).ToList(),
            }).SingleOrDefault();

            if (userInfo == null)
                return new MemberExtendContract { UserId = -1 };
            
            var listRoles = lazyAdminRoleDataAccess.Value.GetGrantedRoleByUserId(userId).Select(r => new AdminRoleContract
            {
                RoleId = r.AdminRole.RId,
                RoleName = r.AdminRole.RoleName,
                Sort = r.AdminRole.Sort
            }).ToList();

            if (listRoles == null || listRoles.Count == 0) 
                return new MemberExtendContract { UserId = -1 };

            var listActions = lazyAdminActionDataAccess.Value.GetListActionByListRoleId(listRoles.Select(r => r.RoleId).ToList());

            userInfo.Roles = listRoles;
            userInfo.ListMemberAllowedActions = listActions;
            if(userInfo!=null && userInfo.LineManagerId > 0)
            {
                var querymanagerInfo = lazyAdminUsersDataAccess.Value.GetUser(userId: userInfo.LineManagerId, isActive: true);
                if (querymanagerInfo != null && querymanagerInfo.Any())
                    userInfo.LineManagerEmail = querymanagerInfo.FirstOrDefault().Email;
            }
            

            return userInfo;
        }

        public CUDReturnMessage InsertUserTracking(string log, int userId)
        {
            return lazyAdminUsersDataAccess.Value.InsertUserTracking(log, userId);
        }
    }
}
