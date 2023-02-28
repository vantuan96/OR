using Contract.OR;
using Contract.Shared;
using Contract.User;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public interface IAdminUsersDataAccess
    {
        IQueryable<Models.AdminUser> GetUser(string email = "", string username = "", int userId = 0, bool? isActive = null);
        IQueryable<Models.AdminUser> GetUserForCheckLogin(string email = "", string username = "", int userId = 0, string clientIp = "", string tokenKey = "");

        IQueryable<Models.AdminUser> GetListUser(List<int> locIds, int roleId = 0, string searchText = "");

        CUDReturnMessage ChangePassword(int userId, string password, string salt);

        bool IsExistEmail(string email);

        bool IsExistUsername(string username);

        CUDReturnMessage CreateUser(CreateUpdateUserContract user, string passwordHash = null, string passwordSalt = null);

        CUDReturnMessage UpdateUser(CreateUpdateUserContract user);

        CUDReturnMessage ResetPassword(int userId, string passwordHash, string passwordSalt);

        CUDReturnMessage LockUser(int userId);

        CUDReturnMessage UnLockUser(int userId);

        CUDReturnMessage DeleteUser(int userId);

        CUDReturnMessage UpdateLoginLogout(LoginContract entity, bool isLogon = false);
        CUDReturnMessage InsertUserTracking(string log, int userId);

        IQueryable<Models.AdminUserTracking> GetListUserTracking(DateTime fromDate, DateTime toDate, string keyword);
        IQueryable<Models.AdminUser> GetAllUser(int userId);
        CUDReturnMessage CreateUpdateORUserInfo(ORUserInfoContract data, int uid);
        CUDReturnMessage UpdateEmailOrPhone(int id, string email, string phone);

        CUDReturnMessage CreateUpdateUserInfoPosition(int userId, List<int> positionIds);

        IQueryable<Models.ORUserInfor_Position> GetPositionByUserId(int userId);
        IQueryable<ORPositionType> GetPositions();
        AdminUser GetFullNameById(int id);
        CUDReturnMessage UpdateLogout(LoginContract entity, bool isLogon = false);
        
    }

    public class AdminUsersDataAccess : BaseDataAccess, IAdminUsersDataAccess
    {
        public AdminUsersDataAccess(string appid, int uid) : base(appid, uid)
        {
        }

        public IQueryable<Models.AdminUser> GetUser(string email = "", string username = "", int userId = 0, bool? isActive = null)
        {
            var query = DbContext.AdminUsers.Where(r => r.IsDeleted == false);

            if (string.IsNullOrEmpty(email) == false)
            {
                query = query.Where(r => r.Email == email);
            }

            if (string.IsNullOrEmpty(username) == false)
            {
                query = query.Where(r => r.Username == username);
            }

            if (userId > 0)
            {
                query = query.Where(r => r.UId == userId);
            }

            if (isActive.HasValue)
            {
                query = query.Where(r => r.IsActive == isActive.Value);
            }

            return query;
        }
        public IQueryable<Models.AdminUser> GetUserForCheckLogin(string email = "", string username = "", int userId = 0, string clientIp = "", string tokenKey = "")
        {
            var query = DbContext.AdminUsers.Where(r => r.IsDeleted == false && (r.IsLogon != null && r.IsLogon.Value));

            if (string.IsNullOrEmpty(email) == false)
            {
                query = query.Where(r => r.Email == email);
            }

            if (string.IsNullOrEmpty(username) == false)
            {
                query = query.Where(r => r.Username == username);
            }

            if (userId > 0)
            {
                query = query.Where(r => r.UId == userId);
            }
            if (!string.IsNullOrEmpty(tokenKey))
            {
                query = query.Where(r => r.Token == tokenKey);
            }
            if (!string.IsNullOrEmpty(clientIp))
            {
                query = query.Where(r => r.LastLoginIp == clientIp);
            }

            return query;
        }
        public IQueryable<Models.AdminUser> GetListUser(List<int> locIds, int roleId = 0, string searchText = "")
        {
            IQueryable<Models.AdminUser> query = DbContext.AdminUsers.Where(r => r.IsDeleted == false);

            if (roleId > 0)
            {
                query = query.Where(r => r.AdminUser_Role.Any(ur => ur.RId == roleId && ur.IsDeleted == false));
            }
            //tam thoi off
            //else
            //{
            //    query = query.Where(r => r.AdminUser_Role.Any(ur => ur.RId == (int)Contract.User.AdminRole.SuperAdmin && ur.IsDeleted == false) == false);
            //}


            query = query.Where(r => r.AdminUser_Location.Any(r2 => r2.IsDeleted == false && locIds.Contains(r2.LocationId)));


            if (string.IsNullOrEmpty(searchText) == false)
            {
                query = query.Where(r => r.Username.Contains(searchText)
                || (!string.IsNullOrEmpty(r.Email) && r.Email.Contains(searchText))
                || (!string.IsNullOrEmpty(r.Name) && r.Name.Contains(searchText)));
                //query = query.Where(r => r.Username.Contains(searchText));
            }

            return query;
        }

        public IQueryable<Models.AdminUser> GetAllUser(int userId)
        {
            var query = DbContext.AdminUsers.Where(r => r.IsDeleted == false);
            if (userId > 0)
                query = query.Where(c => c.UId != userId);
            return query;
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public CUDReturnMessage ChangePassword(int userId, string password, string salt)
        {
            var user = DbContext.AdminUsers.Find(userId);

            if (user != null)
            {
                //Check Have role on location
                var CheckExistLoc = user.AdminUser_Location.Where(x => !x.IsDeleted).Select(x => x.LocationId).Except(MemberExtInfo.Locations.Where(x => !x.IsDeleted).Select(x => x.LocationId).ToList()).ToList();
                if (CheckExistLoc.Any())
                {
                    //Return Invalid location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                user.Password = password;
                user.PaswordSalt = salt;
                user.IsRequireChangePass = false;
                user.LastUpdatedDate = DateTime.Now;
                user.LastUpdatedBy = uid;

                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.ChangePassword_Successed);
            }

            return new CUDReturnMessage(ResponseCode.Error);
        }

        public bool IsExistEmail(string email)
        {
            return DbContext.AdminUsers.Any(r => r.Email.Equals(email) && r.IsDeleted == false);
        }

        public bool IsExistUsername(string username)
        {
            return DbContext.AdminUsers.Any(r => r.Username.Equals(username) && r.IsDeleted == false);
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        public CUDReturnMessage CreateUser(CreateUpdateUserContract user, string passwordHash = null, string passwordSalt = null)
        {
            //Check Have role on location
            var CheckExistLoc = user.Locations.Except(MemberExtInfo.Locations.Where(x => !x.IsDeleted).Select(x => x.LocationId).ToList()).ToList();
            if (CheckExistLoc.Any())
            {
                //Return Invalid location with Data Location
                return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
            }

            Models.AdminUser adminuser = new Models.AdminUser
            {
                Username = user.Username,
                IsADAccount = user.IsADAccount,
                Name = user.Fullname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = passwordHash,
                PaswordSalt = passwordSalt,
                DeptId = user.DeptId,
                IsActive = true,
                IsRequireChangePass = false,
                CreatedBy = uid,
                CreatedDate = DateTime.Now,
                LastUpdatedBy = uid,
                LastUpdatedDate = DateTime.Now,
                IsDeleted = false
            };

            foreach (var role in user.Roles)
            {
                Models.AdminUser_Role usrrole = new Models.AdminUser_Role
                {
                    RId = role,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = uid,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = uid
                };

                adminuser.AdminUser_Role.Add(usrrole);
            }
            #region Comment Code
            //foreach (var site in user.MicroSites)
            //{
            //    Models.AdminUser_Microsite usrsite = new Models.AdminUser_Microsite
            //    {
            //        MsId = site,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.Now,
            //        CreatedBy = uid
            //    };

            //    adminuser.AdminUser_Microsite.Add(usrsite);
            //}

            //foreach (var pnl in user.PnLs)
            //{
            //    Models.AdminUser_PnL p = new Models.AdminUser_PnL
            //    {
            //        PnLId = pnl,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.Now,
            //        CreatedBy = uid,
            //        LastUpdatedDate = DateTime.Now,
            //        LastUpdatedBy = uid
            //    };

            //    adminuser.AdminUser_PnL.Add(p);
            //}

            //foreach (var pnl in user.Sites)
            //{
            //    Models.AdminUser_PnL_Site p = new Models.AdminUser_PnL_Site
            //    {
            //        SiteId = pnl,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.Now,
            //        CreatedBy = uid,
            //        LastUpdatedDate = DateTime.Now,
            //        LastUpdatedBy = uid
            //    };

            //    adminuser.AdminUser_PnL_Site.Add(p);
            //}
            #endregion .Comment Code

            foreach (var loc in user.Locations)
            {
                AdminUser_Location l = new AdminUser_Location()
                {
                    LocationId = loc,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = uid,
                    LastUpdatedDate = DateTime.Now,
                    LastUpdatedBy = uid
                };
                adminuser.AdminUser_Location.Add(l);
            }

            DbContext.AdminUsers.Add(adminuser);
            DbContext.SaveChanges();

            return new CUDReturnMessage(ResponseCode.UserMngt_SuccessCreated);
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public CUDReturnMessage UpdateUser(CreateUpdateUserContract user)
        {
            var adminuser = DbContext.AdminUsers.Find(user.UserId);
            if (adminuser != null)
            {
                //Check Have role on location
                var CheckExistLoc = user.Locations.Except(MemberExtInfo.Locations.Select(x => x.LocationId).ToList()).ToList();
                if (CheckExistLoc.Any())
                {
                    //Return Invalid location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                adminuser.Name = user.Fullname;
                adminuser.PhoneNumber = user.PhoneNumber;
                adminuser.Email = user.Email;
                adminuser.DeptId = user.DeptId;
                adminuser.LastUpdatedBy = uid;
                adminuser.LastUpdatedDate = DateTime.Now;
                adminuser.Username = user.Username;
                adminuser.IsADAccount = user.IsADAccount;
                adminuser.LineManagerId = user.LineManagerId;

                UpdateRoleUser(adminuser, user.Roles);
                //UpdateMicrositeUser(adminuser, user.MicroSites);
                //UpdatePnLUser(adminuser, user.PnLs);
                //UpdatePnLSiteUser(adminuser, user.Sites);
                UpdateLocationUser(adminuser, user.Locations);
                //linhht update ORUserInfo
                UpdateORUser(adminuser);
                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.UserMngt_SuccessUpdated);
            }
            else
            {
                //Ko ton tai
                return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
            }
        }

        private void UpdateRoleUser(Models.AdminUser user, List<int> newRoleIds)
        {
            // đánh dấu xóa role
            var deleteRoles = DbContext.AdminUser_Role.Where(r => r.IsDeleted == false && r.UId == user.UId && newRoleIds.Contains(r.RId) == false);
            if (deleteRoles.Any())
            {
                foreach (var role in deleteRoles)
                {
                    role.IsDeleted = true;
                    role.LastUpdatedBy = uid;
                    role.LastUpdatedDate = DateTime.Now;
                }
            }

            // bỏ đánh dấu xóa role
            var restoreRoles = DbContext.AdminUser_Role.Where(r => r.IsDeleted == true && r.UId == user.UId && newRoleIds.Contains(r.RId));
            if (restoreRoles.Any())
            {
                foreach (var role in restoreRoles)
                {
                    role.IsDeleted = false;
                    role.LastUpdatedBy = uid;
                    role.LastUpdatedDate = DateTime.Now;
                }
            }

            // thêm role mới
            var insertRoles = newRoleIds.Where(r => user.AdminUser_Role.Any(ur => ur.RId == r) == false);
            if (insertRoles.Any())
            {
                foreach (var role in insertRoles)
                {
                    Models.AdminUser_Role usrrole = new Models.AdminUser_Role
                    {
                        UId = user.UId,
                        RId = role,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        CreatedBy = uid,
                        LastUpdatedBy = uid,
                        LastUpdatedDate = DateTime.Now
                    };

                    DbContext.AdminUser_Role.Add(usrrole);
                }
            }
        }

        //private void UpdateMicrositeUser(Models.AdminUser user, List<int> newSiteIds)
        //{
        //    // đánh dấu xóa site
        //    var deleteSites = DbContext.AdminUser_Microsite.Where(r => r.IsDeleted == false && r.UId == user.UId && newSiteIds.Contains(r.MsId) == false);
        //    if (deleteSites.Any())
        //    {
        //        foreach (var site in deleteSites)
        //        {
        //            site.IsDeleted = true;
        //        }
        //    }

        //    // bỏ đánh dấu xóa site
        //    var restoreSites = DbContext.AdminUser_Microsite.Where(r => r.IsDeleted == true && r.UId == user.UId && newSiteIds.Contains(r.MsId));
        //    if (restoreSites.Any())
        //    {
        //        foreach (var site in restoreSites)
        //        {
        //            site.IsDeleted = false;
        //        }
        //    }

        //    // thêm site mới
        //    var insertSites = newSiteIds.Where(r => user.AdminUser_Microsite.Any(ur => ur.MsId == r) == false);
        //    if (insertSites.Any())
        //    {
        //        foreach (var site in insertSites)
        //        {
        //            Models.AdminUser_Microsite usrsite = new Models.AdminUser_Microsite
        //            {
        //                UId = user.UId,
        //                MsId = site,
        //                IsDeleted = false,
        //                CreatedDate = DateTime.Now,
        //                CreatedBy = uid
        //            };

        //            DbContext.AdminUser_Microsite.Add(usrsite);
        //        }
        //    }
        //}

        /*
        private void UpdatePnLUser(Models.AdminUser user, List<int> newPnL)
        {
            // đánh dấu xóa site
            var delete = DbContext.AdminUser_PnL.Where(r => r.IsDeleted == false && r.UId == user.UId && newPnL.Contains(r.PnLId) == false);
            if (delete.Any())
            {
                foreach (var item in delete)
                {
                    item.IsDeleted = true;
                    item.LastUpdatedDate = DateTime.Now;
                    item.LastUpdatedBy = uid;
                }
            }

            // bỏ đánh dấu xóa site
            var restore = DbContext.AdminUser_PnL.Where(r => r.IsDeleted == true && r.UId == user.UId && newPnL.Contains(r.PnLId));
            if (restore.Any())
            {
                foreach (var item in restore)
                {
                    item.IsDeleted = false;
                    item.LastUpdatedDate = DateTime.Now;
                    item.LastUpdatedBy = uid;
                }
            }

            // thêm site mới
            var insert = newPnL.Where(r => user.AdminUser_PnL.Any(ur => ur.PnLId == r) == false);
            if (insert.Any())
            {
                foreach (var item in insert)
                {
                    Models.AdminUser_PnL p = new Models.AdminUser_PnL
                    {
                        UId = user.UId,
                        PnLId = item,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        CreatedBy = uid,
                        LastUpdatedDate = DateTime.Now,
                        LastUpdatedBy = uid
                    };

                    DbContext.AdminUser_PnL.Add(p);
                }
            }
        }

        private void UpdatePnLSiteUser(Models.AdminUser user, List<int> newSite)
        {
            // đánh dấu xóa site
            var delete = DbContext.AdminUser_PnL_Site.Where(r => r.IsDeleted == false && r.UId == user.UId && newSite.Contains(r.SiteId) == false);
            if (delete.Any())
            {
                foreach (var item in delete)
                {
                    item.IsDeleted = true;
                    item.LastUpdatedDate = DateTime.Now;
                    item.LastUpdatedBy = uid;
                }
            }

            // bỏ đánh dấu xóa site
            var restore = DbContext.AdminUser_PnL_Site.Where(r => r.IsDeleted == true && r.UId == user.UId && newSite.Contains(r.SiteId));
            if (restore.Any())
            {
                foreach (var item in restore)
                {
                    item.IsDeleted = false;
                    item.LastUpdatedDate = DateTime.Now;
                    item.LastUpdatedBy = uid;
                }
            }

            // thêm site mới
            var insert = newSite.Where(r => user.AdminUser_PnL_Site.Any(ur => ur.SiteId == r) == false);
            if (insert.Any())
            {
                foreach (var item in insert)
                {
                    Models.AdminUser_PnL_Site p = new Models.AdminUser_PnL_Site
                    {
                        UId = user.UId,
                        SiteId = item,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        CreatedBy = uid,
                        LastUpdatedDate = DateTime.Now,
                        LastUpdatedBy = uid
                    };

                    DbContext.AdminUser_PnL_Site.Add(p);
                }
            }
        }
        */

        private void UpdateLocationUser(Models.AdminUser user, List<int> newLocation)
        {
            // đánh dấu xóa site
            var delete = DbContext.AdminUser_Location.Where(r => r.IsDeleted == false && r.UId == user.UId && newLocation.Contains(r.LocationId) == false);
            if (delete.Any())
            {
                foreach (var item in delete)
                {
                    item.IsDeleted = true;
                    item.LastUpdatedDate = DateTime.Now;
                    item.LastUpdatedBy = uid;
                }
            }

            // bỏ đánh dấu xóa site
            var restore = DbContext.AdminUser_Location.Where(r => r.IsDeleted == true && r.UId == user.UId && newLocation.Contains(r.LocationId));
            if (restore.Any())
            {
                foreach (var item in restore)
                {
                    item.IsDeleted = false;
                    item.LastUpdatedDate = DateTime.Now;
                    item.LastUpdatedBy = uid;
                }
            }

            // thêm site mới
            var insert = newLocation.Where(r => user.AdminUser_Location.Any(ur => ur.LocationId == r) == false);
            if (insert.Any())
            {
                foreach (var item in insert)
                {
                    Models.AdminUser_Location p = new Models.AdminUser_Location
                    {
                        UId = user.UId,
                        LocationId = item,
                        IsDeleted = false,
                        CreatedDate = DateTime.Now,
                        CreatedBy = uid,
                        LastUpdatedDate = DateTime.Now,
                        LastUpdatedBy = uid
                    };

                    DbContext.AdminUser_Location.Add(p);
                }
            }
        }
        //linhht update ORUserInfo
        private void UpdateORUser(Models.AdminUser user)
        {

            // thêm site mới
            var updated = DbContext.ORUserInfoes.FirstOrDefault(x => x.Id == user.UId);
            if (updated != null)
            {
                updated.Name = user.Name;
                if (string.IsNullOrEmpty(updated.Email))
                {
                    updated.Email = user.Email;
                }
                if (string.IsNullOrEmpty(updated.Phone))
                {
                    updated.Phone = user.PhoneNumber;
                }
                updated.UpdatedDate = DateTime.Now;
                updated.UpdatedBy = user.LastUpdatedBy;
                DbContext.Entry(updated).State = System.Data.Entity.EntityState.Modified;
            }
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="memberExtInfo"></param>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        public CUDReturnMessage ResetPassword(int userId, string passwordHash, string passwordSalt)
        {
            var adminuser = DbContext.AdminUsers.Find(userId);
            if (adminuser != null)
            {
                if (!adminuser.AdminUser_Location.Any(x => x.LocationId == MemberExtInfo.CurrentLocationId && !x.IsDeleted))
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                adminuser.Password = passwordHash;
                adminuser.PaswordSalt = passwordSalt;
                adminuser.IsRequireChangePass = true;
                adminuser.LastUpdatedBy = uid;
                adminuser.LastUpdatedDate = DateTime.Now;

                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.UserMngt_SuccessResetPassword);
            }
            else
            {
                //Ko ton tai
                return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
            }
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage LockUser(int userId)
        {
            var adminuser = DbContext.AdminUsers.Find(userId);
            if (adminuser != null)
            {
                if (!adminuser.AdminUser_Location.Any(x => x.LocationId == MemberExtInfo.CurrentLocationId && !x.IsDeleted))
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                adminuser.IsActive = false;
                adminuser.LastUpdatedBy = uid;
                adminuser.LastUpdatedDate = DateTime.Now;

                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.UserMngt_SuccessLock);
            }
            else
            {
                //Ko ton tai
                return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
            }

        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage UnLockUser(int userId)
        {
            var adminuser = DbContext.AdminUsers.Find(userId);
            if (adminuser != null)
            {
                if (!adminuser.AdminUser_Location.Any(x => x.LocationId == MemberExtInfo.CurrentLocationId && !x.IsDeleted))
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                adminuser.IsActive = true;
                adminuser.LastUpdatedBy = uid;
                adminuser.LastUpdatedDate = DateTime.Now;

                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.UserMngt_SuccessUnLock);
            }
            else
            {
                //Ko ton tai
                return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
            }

        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage DeleteUser(int userId)
        {
            var adminuser = DbContext.AdminUsers.Find(userId);
            if (adminuser != null)
            {
                if (!adminuser.AdminUser_Location.Any(x => x.LocationId == MemberExtInfo.CurrentLocationId && !x.IsDeleted))
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
                adminuser.IsDeleted = true;
                adminuser.LastUpdatedBy = uid;
                adminuser.LastUpdatedDate = DateTime.Now;

                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.UserMngt_SuccessDelete);
            }
            else
            {
                //Ko ton tai
                return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
            }
        }

        public CUDReturnMessage UpdateLogout(LoginContract entity, bool isLogon = false)
        {
            var adminuser = DbContext.AdminUsers.Where(x => x.Username.Equals(entity.NickName)).SingleOrDefault();
            if (adminuser != null)
            {
                adminuser.LastLoginIp = entity.ClientIp;
                adminuser.Token = null;
                adminuser.FailedPasswordAttemptCount = 0;
                adminuser.IsLogon = false;
                adminuser.LastActivityDate = DateTime.Now;
                adminuser.LastUpdatedBy = uid;
                adminuser.LastUpdatedDate = DateTime.Now;
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.UserMngt_SuccessUpdated);
            }
            else
            {
                //Ko ton tai
                return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
            }
        }
        public CUDReturnMessage UpdateLoginLogout(LoginContract entity, bool isLogon = false)
        {
            var adminuser = DbContext.AdminUsers.Find(entity.UserId);
            adminuser = (adminuser == null) ? DbContext.AdminUsers.Where(x => x.Username.Equals(entity.NickName)).SingleOrDefault() : adminuser;
            //adminuser = (adminuser == null) ? DbContext.AdminUsers.Where(x => x.LastLoginIp.Equals(entity.ClientIp)).SingleOrDefault() : adminuser;
            adminuser = (adminuser == null) ? DbContext.AdminUsers.Where(x => x.Token.Equals(entity.TokenKey)).SingleOrDefault() : adminuser;
            if (adminuser != null)
            {
                adminuser.LastLoginIp = entity.ClientIp;
                if (!isLogon)
                    adminuser.Token = entity.TokenKey;
                adminuser.FailedPasswordAttemptCount = entity.CurrentLoginFail;
                adminuser.IsLogon = entity.IsLogon;
                adminuser.LastActivityDate = DateTime.Now;
                adminuser.LastUpdatedBy = uid;
                adminuser.LastUpdatedDate = DateTime.Now;

                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.UserMngt_SuccessUpdated);
            }
            else
            {
                //Ko ton tai
                return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
            }
        }

        public CUDReturnMessage InsertUserTracking(string log, int userId)
        {
            Models.AdminUserTracking track = new Models.AdminUserTracking
            {
                UserId = userId,
                ContentTracking = log,
                CreatedDate = DateTime.Now,
                TypeId = 0
            };
            DbContext.AdminUserTrackings.Add(track);
            DbContext.SaveChanges();
            return new CUDReturnMessage(ResponseCode.UserMngt_SuccessUpdated);
        }
        public IQueryable<AdminUserTracking> GetListUserTracking(DateTime fromDate, DateTime toDate, string keyword)
        {
            var query = DbContext.AdminUserTrackings.Where(r => r.CreatedDate >= fromDate && r.CreatedDate <= toDate);
            if (string.IsNullOrEmpty(keyword) == false)
            {
                query = query.Where(r => r.ContentTracking.Contains(keyword) || r.AdminUser.Name.Contains(keyword) || r.AdminUser.Email.Contains(keyword));
            }
            return query;
        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="data"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public CUDReturnMessage CreateUpdateORUserInfo(ORUserInfoContract data, int uid)
        {
            // thêm dept moi
            var currentUser = DbContext.ORUserInfoes.FirstOrDefault(c => c.Id == data.Id);
            var adminuser = DbContext.AdminUsers.Find(data.Id);
            if (currentUser == null)
            {
                #region Check Role of location
                if (adminuser != null)
                {
                    if (!adminuser.AdminUser_Location.Any(x => x.LocationId == MemberExtInfo.CurrentLocationId && !x.IsDeleted))
                    {
                        //Return Invalid current location with Data Location
                        return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                    }
                }
                else
                {
                    //Ko ton tai
                    return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
                }
                #endregion .Check Role of location

                DbContext.ORUserInfoes.Add(new ORUserInfo()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Phone = data.Phone,
                    Email = data.Email,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    CreatedBy = uid,
                    UpdatedBy = uid,
                    UpdatedDate = DateTime.Now
                });
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.UserMngt_SuccessCreated);
            }
            else
            {
                #region Check Role of location
                if (adminuser != null)
                {
                    if (!adminuser.AdminUser_Location.Any(x => x.LocationId == MemberExtInfo.CurrentLocationId && !x.IsDeleted))
                    {
                        //Return Invalid current location with Data Location
                        return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                    }
                }
                else
                {
                    //Ko ton tai
                    return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
                }
                #endregion .Check Role of location

                currentUser.Phone = data.Phone;
                currentUser.Email = data.Email;
                currentUser.UpdatedBy = uid;
                currentUser.UpdatedDate = DateTime.Now;
                currentUser.IsDeleted = data.IsDeleted;
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.UserMngt_SuccessUpdated);
            }

        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>

        public CUDReturnMessage UpdateEmailOrPhone(int id, string email, string phone)
        {
            // thêm dept moi
            var currentUser = DbContext.ORUserInfoes.FirstOrDefault(c => c.Id == id);
            if (currentUser == null)
            {
                return new CUDReturnMessage(ResponseCode.UserMngt_UsernameNotExisted);
            }
            else
            {
                var adminuser = DbContext.AdminUsers.Find(id);
                #region Check Role of location
                if (adminuser != null)
                {
                    if (!adminuser.AdminUser_Location.Any(x => x.LocationId == MemberExtInfo.CurrentLocationId && !x.IsDeleted))
                    {
                        //Return Invalid current location with Data Location
                        return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                    }
                }
                else
                {
                    //Ko ton tai
                    return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
                }
                #endregion .Check Role of location
                currentUser.Email = email;
                currentUser.Phone = phone;
                currentUser.UpdatedBy = uid;
                currentUser.UpdatedDate = DateTime.Now;
                DbContext.SaveChanges();
                return null;
            }

        }
        /// <summary>
        /// Da fix phan quyen du lieu theo site
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="positionIds"></param>
        /// <returns></returns>
        public CUDReturnMessage CreateUpdateUserInfoPosition(int userId, List<int> positionIds)
        {
            if (userId == 0)
            {
                return new CUDReturnMessage(ResponseCode.UserMngt_UsernameNotExisted);
            }

            var adminUser = DbContext.AdminUsers.Find(userId);
            #region Check Role of location
            if (adminUser != null)
            {
                if (!adminUser.AdminUser_Location.Any(x => x.LocationId == MemberExtInfo.CurrentLocationId && !x.IsDeleted))
                {
                    //Return Invalid current location with Data Location
                    return new CUDReturnMessage(ResponseCode.AdminRole_Accessdenied);
                }
            }
            else
            {
                //Ko ton tai
                return new CUDReturnMessage(ResponseCode.UserMngt_NotExisted);
            }
            #endregion .Check Role of location

            var userPositions = DbContext.ORUserInfor_Position.Where(c => c.UserInforId == userId).ToList();

            //var positionInsert = positionIds.Except(userPositions.Select(x => x.PositionId).ToList());
            //var positionUpdate = positionIds.Intersect(userPositions.Select(x => x.PositionId).ToList());
            //var positionDelete = userPositions.Select(x => x.PositionId).ToList().Except(positionIds);

            //foreach (var item in positionDelete)
            //{
            //    var objUP = userPositions.FirstOrDefault(x => x.UserInforId == userId && x.PositionId == item);
            //    if(objUP != null)
            //    {
            //        objUP.IsDeleted = true;
            //    }
            //    DbContext.SaveChanges();
            //}

            //foreach (var item in positionUpdate)
            //{
            //    var objUP = userPositions.FirstOrDefault(x => x.UserInforId == userId && x.PositionId == item);
            //    if (objUP != null)
            //    {
            //        if(objUP.IsDeleted)
            //            objUP.IsDeleted = !objUP.IsDeleted;
            //    }
            //    DbContext.SaveChanges();
            //}

            //foreach (var item in positionInsert)
            //{
            //    var obj = new ORUserInfor_Position
            //    {
            //        UserInforId = userId,
            //        PositionId = item,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.Now,
            //        CreatedBy = uid,
            //        LastUpdatedBy = uid,
            //        LastUpdatedDate = DateTime.Now
            //    };
            //    DbContext.ORUserInfor_Position.Add(obj);
            //    DbContext.SaveChanges();
            //}

            //set các vị trí về trạng thái xóa mềm nếu có
            userPositions.ForEach(x => x.IsDeleted = true);
            DbContext.SaveChanges();

            //Tìm kiếm các vị trí con
            if (positionIds.Count > 0)
            {
                foreach (var item in positionIds)
                {
                    var childrenPositions = DbContext.ORPositionTypes.Where(x => x.Id == item || x.ParentId == item).ToList();
                    if (childrenPositions.Count > 0)
                    {
                        foreach (var children in childrenPositions)
                        {
                            var objUP = DbContext.ORUserInfor_Position.FirstOrDefault(x => x.PositionId == children.Id && x.UserInforId == userId);
                            if (objUP != null)
                            {
                                objUP.IsDeleted = !objUP.IsDeleted;
                                DbContext.SaveChanges();
                            }
                            else
                            {
                                var obj = new ORUserInfor_Position
                                {
                                    UserInforId = userId,
                                    PositionId = children.Id,
                                    IsDeleted = false,
                                    CreatedDate = DateTime.Now,
                                    CreatedBy = uid,
                                    LastUpdatedBy = uid,
                                    LastUpdatedDate = DateTime.Now
                                };
                                DbContext.ORUserInfor_Position.Add(obj);
                                DbContext.SaveChanges();
                            }
                        }
                    }

                }
            }
            return null;
        }

        public IQueryable<ORUserInfor_Position> GetPositionByUserId(int userId)
        {
            if (userId == 0)
                return null;
            var query = DbContext.ORUserInfor_Position.Where(r => !r.IsDeleted && r.UserInforId == userId);
            return query;
        }

        public IQueryable<ORPositionType> GetPositions()
        {
            var query = DbContext.ORPositionTypes.Where(r => !r.IsDeleted ?? false);
            return query;
        }

        public AdminUser GetFullNameById(int id)
        {
            if (id <= 0)
                return null;
            var query = DbContext.AdminUsers.FirstOrDefault(r => !r.IsDeleted && r.UId == id);
            if (query == null)
            {
                query = DbContext.ORUserInfoes.Where(r => r.Id == id && (!r.IsDeleted ?? false))?.Select(x => new AdminUser
                {
                    UId = x.Id,
                    Name = x.Name
                }).FirstOrDefault();
            }

            return query;
        }
    }
}