using System;
using System.Linq;
using VG.Common;
using Contract.Microsite;
using Contract.Shared;
using Contract.User;
using DataAccess;
using DataAccess.DAO;
using System.Collections.Generic;
using DataAccess.Models;

namespace Business.Core
{
    public interface IUserProfileBusiness
    {
        UserProfileContract GetUserProfile(int userId);

        CUDReturnMessage ChangePassword(int userId, string oldPassword, string newPassword);
    }

    public class UserProfileBusiness : BaseBusiness, IUserProfileBusiness
    {
        private Lazy<IAdminUsersDataAccess> lazyAdminUsersDataAccess;
        //private Lazy<IBMSMasterDataAccess> lazyMaster;

        public UserProfileBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyAdminUsersDataAccess = new Lazy<IAdminUsersDataAccess>(() => new AdminUsersDataAccess(appid, uid));
            //lazyMaster = new Lazy<IBMSMasterDataAccess>(() => new BMSMasterDataAccess(appid, uid));
        }

        public UserProfileContract GetUserProfile(int userId)
        {
            var query = lazyAdminUsersDataAccess.Value.GetUser(userId: userId);
            //var queryAllPnL =  lazyMaster.Value.GetAllPnL();
            //var queryAllSite = lazyMaster.Value.GetAllSite().ToList();

            var data = query.Select(userInfo => new UserProfileContract
            {
                UserId = userInfo.UId,
                Email = userInfo.Email,
                FullName = userInfo.Name,
                CreatedDate = userInfo.CreatedDate,
                Roles = userInfo.AdminUser_Role.Where(ur => ur.IsDeleted == false && ur.AdminRole.IsDeleted == false)
                    .Select(ur => new AdminRoleContract
                {
                    RoleId = ur.AdminRole.RId,
                    RoleName = ur.AdminRole.RoleName
                }).ToList(),
                MicroSites = userInfo.AdminUser_Microsite.Where(us => us.IsDeleted == false && us.Microsite.IsDeleted == false)
                    .Select(us => new MicrositeItemContract
                {
                    MsId = us.MsId,
                    IsRootSite = us.Microsite.IsRootSite,
                    ReferenceCode = us.Microsite.ReferenceCode,
                    Title = us.Microsite.Name,
                    StatusId = us.Microsite.Status
                }).ToList(),
                Locations = userInfo.AdminUser_Location.Where(l=>l.IsDeleted == false && l.Location.IsDeleted == false)
                    .Select(l=> new LocationContract()
                    {
                        LocationId        = l.Location.LocationId         ,
                        NameVN            = l.Location.NameVN             ,
                        NameEN            = l.Location.NameEN             ,
                        SloganVN          = l.Location.SloganVN           ,
                        SloganEN          = l.Location.SloganEN           ,
                        LogoName          = l.Location.LogoName           ,
                        BackgroundName    = l.Location.BackgroundName     ,
                        ColorCode         = l.Location.ColorCode          ,
                        ParentId          = l.Location.ParentId           ,
                        LevelNo           = l.Location.LevelNo            ,
                        LevelPath         = l.Location.LevelPath          ,
                        RootId            = l.Location.RootId             ,
                        LayoutTypeId      = l.Location.LayoutTypeId       ,
                        CreatedBy         = l.Location.CreatedBy          ,
                        CreatedDate       = l.Location.CreatedDate        ,
                        LastUpdatedBy     = l.Location.LastUpdatedBy      ,
                        LastUpdatedDate   = l.Location.LastUpdatedDate    ,
                        IsDeleted         = l.Location.IsDeleted          ,
                        QuestionGroupId   = l.Location.QuestionGroupId
                    }).ToList()
                //PnLs = userInfo.AdminUser_PnL.Where(p => p.IsDeleted == false)
                //    .Select(
                //        p => new AdminUserPnLContract()
                //        {
                //            UId = p.UId,
                //            PnLId = p.PnLId
                //        }
                //    ).ToList(),
                //Sites = userInfo.AdminUser_PnL_Site.Where(p => p.IsDeleted == false)
                //    .Select(
                //        p => new AdminUserPnLSiteContract()
                //        {
                //            UId = p.UId,
                //            SiteId = p.SiteId
                //        }
                //    ).ToList()
            }).SingleOrDefault();

            //if(queryAllPnL!=null && queryAllPnL.Count() > 0)
            //{
            //    var allPnL = queryAllPnL.ToList();
            //    var listPnL = new List<AdminUserPnLContract>();

            //    foreach (var item in data.PnLs)
            //    {
            //        if(allPnL.Exists(p=>p.ID== item.PnLId))
            //        {
            //            item.PnLName = allPnL.Single(p => p.ID == item.PnLId).Name;
            //            listPnL.Add(item);
            //        }                   
            //    }
            //    data.PnLs = listPnL;
            //}

            //if (queryAllSite != null && queryAllSite.Count() > 0)
            //{
            //    var allSite = queryAllSite.ToList();
            //    var listSite = new List<AdminUserPnLSiteContract>();
            //    foreach (var item in data.Sites)
            //    {
            //        if (allSite.Exists(p => p.ID == item.SiteId))
            //        {
            //            item.SiteName = allSite.Single(p => p.ID == item.SiteId).Name;
            //            item.PnLId = allSite.Single(p => p.ID == item.SiteId).PlanPnLID??0;
            //            listSite.Add(item);
            //        }                    
            //    }
            //    data.Sites = listSite;
            //}

            return data;
        }

        public CUDReturnMessage ChangePassword(int userId, string oldPassword, string newPassword)
        {
            var userInfo = lazyAdminUsersDataAccess.Value.GetUser(userId: userId).FirstOrDefault();
            if (userInfo == null)
                return new CUDReturnMessage(ResponseCode.Error);

            string md5 = (oldPassword + userInfo.PaswordSalt).MD5Hash();
            if (userInfo.Password.Equals(md5) == false)
                return new CUDReturnMessage(ResponseCode.ChangePassword_WrongOldPassword);

            string newPasswordSalt = StringUtil.RandomString(5);
            string passwordHash = (newPassword + newPasswordSalt).MD5Hash();

            return lazyAdminUsersDataAccess.Value.ChangePassword(userId, passwordHash, newPasswordSalt);
        }

    }
}
