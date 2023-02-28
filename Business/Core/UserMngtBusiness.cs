using System;
using System.Linq;
using System.Collections.Generic;
using Contract.User;
using DataAccess;
using Contract.Shared;
using Contract.Microsite;
using VG.Common;
using DataAccess.DAO;
using Contract.OR;
using VG.EncryptLib.EncryptLib;
using DataAccess.Models;

namespace Business.Core
{
    public interface IUserMngtBusiness
    {
        List<AdminRoleContract> GetListRole(int currentRoleSort);

        PagedList<UserItemContract> GetListUser(List<int> locationIds, int roleId,  string searchText, int pageSize, int page);

        UserItemContract FetchUserToUpdate(int userId, int currentUserId);

        CUDReturnMessage CreateUpdateUser(CreateUpdateUserContract user);

        CUDReturnMessage ResetPassword(int userId);

        CUDReturnMessage LockUnLockUser(int userId, bool lockStatus);

        CUDReturnMessage DeleteUser(int userId);
        List<AdminUserInfo> GetAllUser(int userId);
        CUDReturnMessage CreateUpdateORUserInfo(ORUserInfoContract data, int uid);
        CUDReturnMessage UpdateEmailOrPhone(int id, string email, string phone);
        CUDReturnMessage CreateUpdateUserInfoPosition(int userId, List<int> positionIds);
        List<ORUserInforPositionContract> GetPositionByUserId(int userId);
        List<ORPositionType> GetPositions();

        AdminUser GetFullNameById(int id);
    }

    public class UserMngtBusiness : BaseBusiness, IUserMngtBusiness
    {
        private Lazy<IAdminUsersDataAccess> lazyAdminUsersDataAccess;
        private Lazy<IAdminRoleDataAccess> lazyAdminRoleDataAccess;
        private Lazy<ISystemSettingDataAccess> lazySystemSettingDataAccess;
        private Lazy<ILocationDataAccess> lazyLocation;
        //private Lazy<IBMSMasterDataAccess> lazyMaster;

        public UserMngtBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyAdminUsersDataAccess = new Lazy<IAdminUsersDataAccess>(() => new AdminUsersDataAccess(appid, uid));
            lazySystemSettingDataAccess = new Lazy<ISystemSettingDataAccess>(() => new SystemSettingDataAccess(appid, uid));
            lazyAdminRoleDataAccess = new Lazy<IAdminRoleDataAccess>(() => new AdminRoleDataAccess(appid, uid));
            lazyLocation = new Lazy<ILocationDataAccess>(() => new LocationDataAccess(appid, uid));
        }

        public PagedList<UserItemContract> GetListUser(List<int> locationIds, int roleId,string searchText, int pageSize, int page)
        {
            var query = lazyAdminUsersDataAccess.Value.GetListUser(locationIds, roleId, searchText);
            //var queryAllPnL = lazyMaster.Value.GetAllPnL();
            //var queryAllSite = lazyMaster.Value.GetAllSite();

            var result = new PagedList<UserItemContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(r => r.UId).Skip((page - 1) * pageSize).Take(pageSize).Select(r => 
                new UserItemContract
                {
                    UserId = r.UId,
                    Email = r.Email,
                    PhoneNumber = r.PhoneNumber,
                    FullName = r.Name,
                    CreatedDate = r.CreatedDate,
                    IsActive = r.IsActive,
                    DeptId = r.DeptId ?? 0,
                    //DeptName = r.AdminDepartment == null ? "" : r.AdminDepartment.DeptName,
                    IsADAccount = r.IsADAccount,
                    Username = r.Username,
                    Roles = r.AdminUser_Role.Where(ur => ur.IsDeleted == false && ur.AdminRole.IsDeleted == false).Select(ur => new AdminRoleContract
                    {
                        RoleId = ur.AdminRole.RId,
                        RoleName = ur.AdminRole.RoleName,
                        Sort = ur.AdminRole.Sort
                    }).ToList(),
                    MicroSites = r.AdminUser_Microsite.Where(us => us.IsDeleted == false && us.Microsite.IsDeleted == false).Select(us => new MicrositeItemContract
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
                        IsDeleted = l.Location.IsDeleted,
                        QuestionGroupId = l.Location.QuestionGroupId
                    }).Distinct().ToList()
                }).ToList();
            }

            return result;
        }

        public UserItemContract FetchUserToUpdate(int userId, int currentUserId)
        {
            var query = lazyAdminUsersDataAccess.Value.GetUser(userId: userId);
            //var queryAllPnL = lazyMaster.Value.GetAllPnL();
            //var queryAllSite = lazyMaster.Value.GetAllSite();

            var data = query.Select(r => new UserItemContract
            {
                UserId = r.UId,
                Email = r.Email,
                FullName = r.Name,
                PhoneNumber = r.PhoneNumber,
                DeptId = r.DeptId ?? 0,
                //DeptName = r.AdminDepartment == null ? "" : r.AdminDepartment.DeptName,
                CreatedDate = r.CreatedDate,
                IsADAccount = r.IsADAccount,
                Username = r.Username,
                LineManager=r.LineManagerId,
                Roles = r.AdminUser_Role.Where(ur => ur.IsDeleted == false && ur.AdminRole.IsDeleted == false).Select(ur => new AdminRoleContract
                {
                    RoleId = ur.AdminRole.RId,
                    RoleName = ur.AdminRole.RoleName,
                    Sort = ur.AdminRole.Sort
                }).ToList(),
                MicroSites = r.AdminUser_Microsite.Where(us => us.IsDeleted == false && us.Microsite.IsDeleted == false).Select(us => new MicrositeItemContract
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
                        IsDeleted = l.Location.IsDeleted,
                        QuestionGroupId = l.Location.QuestionGroupId
                    }).Distinct().ToList()
                //PnLs = r.AdminUser_PnL.Where(p => p.IsDeleted == false)
                //    .Select(
                //        p => new AdminUserPnLContract()
                //        {
                //            UId = p.UId,
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
                //    ).ToList()
            }).SingleOrDefault();

            //if (queryAllPnL != null && queryAllPnL.Count() > 0)
            //{
            //    var allPnL = queryAllPnL.ToList();
            //    var listPnL = new List<AdminUserPnLContract>();

            //    foreach (var item in data.PnLs)
            //    {
            //        if (allPnL.Exists(p => p.ID == item.PnLId))
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
            //            item.PnLId = allSite.Single(p => p.ID == item.SiteId).PlanPnLID ?? 0;
            //            listSite.Add(item);
            //        }
            //    }
            //    data.Sites = listSite;
            //}

            return data;
        }

        public CUDReturnMessage CreateUpdateUser(CreateUpdateUserContract user)
        {
            var locations = lazyLocation.Value.Get();
            //var listPnL = new List<int>();

            if (locations != null && locations.Count() > 0)
            {
                //var allSite = queryAllSite.ToList();
                //foreach (var item in user.Locations)
                //{
                //    if (allSite.Exists(s => s.ID == item))
                //    {
                //        listPnL.Add(allSite.Single(s => s.ID == item).PlanPnLID ?? 0);
                //    }
                //}

                //listPnL = listPnL.Distinct().ToList();
                //user.PnLs = listPnL;

                if (user.UserId == 0)
                {
                    if (lazyAdminUsersDataAccess.Value.IsExistUsername(user.Username))
                        return new CUDReturnMessage(ResponseCode.UserMngt_UsernameExisted);

                    //if (lazyAdminUsersDataAccess.Value.IsExistEmail(user.Email))
                    //    return new CUDReturnMessage(ResponseCode.UserMngt_EmailExisted);

                    //if (user.IsADAccount)
                    //{
                    var defaultPasswordSettingKey = lazySystemSettingDataAccess.Value.Find("User.DefaultPassword");
                    var desDefaultPassword = defaultPasswordSettingKey != null ? Security.Decrypt(AppUtils.SecuKey, defaultPasswordSettingKey.Value) : "12345678";
                    string defaultPassword = desDefaultPassword;

                    string passwordSalt = StringUtil.RandomString(5);
                    string passwordHash = (defaultPassword + passwordSalt).MD5Hash();
                    return lazyAdminUsersDataAccess.Value.CreateUser(user, passwordHash, passwordSalt);
                    //}
                    //else
                    //{
                    //    return lazyAdminUsersDataAccess.Value.CreateUser(user);
                    //}
                }
                else
                {
                    return lazyAdminUsersDataAccess.Value.UpdateUser(user);
                }
            }

            return new CUDReturnMessage(ResponseCode.UserMngt_InvalidSite);
        }

        public CUDReturnMessage ResetPassword(int userId)
        {
            var defaultPasswordSettingKey = lazySystemSettingDataAccess.Value.Find("User.DefaultPassword");
            var desDefaultPassword = defaultPasswordSettingKey != null ? Security.Decrypt(AppUtils.SecuKey, defaultPasswordSettingKey.Value) : "12345678";
            string defaultPassword = desDefaultPassword;

            string passwordSalt = StringUtil.RandomString(5);
            string passwordHash = (defaultPassword + passwordSalt).MD5Hash();
            return lazyAdminUsersDataAccess.Value.ResetPassword(userId, passwordHash, passwordSalt);
        }

        public CUDReturnMessage LockUnLockUser(int userId, bool lockStatus)
        {
            if (lockStatus)
            {
                return lazyAdminUsersDataAccess.Value.LockUser(userId);
            }
            else
            {
                return lazyAdminUsersDataAccess.Value.UnLockUser(userId);
            }
        }

        public CUDReturnMessage DeleteUser(int userId)
        {
            return lazyAdminUsersDataAccess.Value.DeleteUser(userId);
        }

        public List<AdminRoleContract> GetListRole(int currentRoleSort)
        {
            var query = lazyAdminRoleDataAccess.Value.GetListRole(sort: currentRoleSort);

            return query.Select(r => new AdminRoleContract
            {
                RoleId = r.RId,
                RoleName = r.RoleName,
                Sort = r.Sort,
                LastUpdatedBy = r.LastUpdatedBy,
                LastUpdatedDate = r.LastUpdatedDate
            }).ToList();
        }

        public List<AdminUserInfo> GetAllUser(int userId)
        {
            var query = lazyAdminUsersDataAccess.Value.GetAllUser(userId);

            return query.Select(r => new AdminUserInfo
            {
                UserId = r.UId,
                UserName = r.Username,
            }).ToList();
        }

        public CUDReturnMessage CreateUpdateORUserInfo(ORUserInfoContract data, int uid)
        {
            return lazyAdminUsersDataAccess.Value.CreateUpdateORUserInfo(data,uid);
        }
        
        public CUDReturnMessage UpdateEmailOrPhone(int id, string email, string phone)
        {
            return lazyAdminUsersDataAccess.Value.UpdateEmailOrPhone(id, email, phone);
        } 
        
        public CUDReturnMessage CreateUpdateUserInfoPosition(int userId, List<int> positionIds)
        {
            return lazyAdminUsersDataAccess.Value.CreateUpdateUserInfoPosition(userId, positionIds);

        }
        public List<ORUserInforPositionContract> GetPositionByUserId(int userId)
        {
            var query = lazyAdminUsersDataAccess.Value.GetPositionByUserId(userId);

            if (query == null)
                return null;
            return query.Select(r => new ORUserInforPositionContract
            {
                UserId = r.UserInforId,
                PositionId = r.PositionId,
            }).ToList();
        }

        public List<ORPositionType> GetPositions()
        {
            var query = lazyAdminUsersDataAccess.Value.GetPositions();
            if (query == null)
                return null;
            return query.ToList();
        }

        public AdminUser GetFullNameById(int id)
        {
            var query = lazyAdminUsersDataAccess.Value.GetFullNameById(id);

            if (query == null)
                return null;
            return query;
        }
    }
}
