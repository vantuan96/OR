using Contract.Shared;
using Contract.User;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DAO
{
    public interface ILocationDataAccess
    {
        /// <summary>
        /// Lấy ds tất cả vị trí (pnl, cơ sở, chi nhánh, phòng ban)
        /// </summary>
        /// <returns></returns>
        IQueryable<Location> Get();

        /// <summary>
        /// Tìm phòng ban
        /// </summary>
        /// <returns></returns>
        Location Find(int id);

        /// <summary>
        /// Xóa phòng ban
        /// </summary>
        /// <returns></returns>
        CUDReturnMessage Delete(int id, int userId);

        /// <summary>
        /// Thêm hoặc update phòng ban
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdateLocation(LocationContract data);
        CUDReturnMessage UpdateLocationUser(int uId, List<LocationContract> listData);
        List<DepartmentType> GetDepartmentListType();
        IQueryable<string> GetSiteExistDepartment(int userId);
    }

    public class LocationDataAccess : BaseDataAccess, ILocationDataAccess
    {
        public LocationDataAccess(string appid, int uid) : base(appid, uid)
        {
        }

        /// <summary>
        /// Lấy ds tất cả vị trí (pnl, cơ sở, chi nhánh, phòng ban)
        /// </summary>
        /// <returns></returns>
        public IQueryable<Location> Get()
        {
            IQueryable<Location> query = DbContext.Locations.Where(r => r.IsDeleted == false).OrderBy(x=>x.NameEN);
            return query;
        }

        /// <summary>
        /// Tìm phòng ban
        /// </summary>
        /// <returns></returns>
        public Location Find(int id)
        {
            Location query = DbContext.Locations.SingleOrDefault(r => r.LocationId == id && r.IsDeleted == false);

            return query;
        }

        /// <summary>
        /// Xóa phòng ban
        /// </summary>
        /// <returns></returns>
        public CUDReturnMessage Delete(int id, int userId)
        {
            //Location query = DbContext.Locations.SingleOrDefault(r => r.LocationId == id && r.IsDeleted == false);
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var allChildLocation = DbContext.Locations.Where(x => ("." + x.LevelPath + ".").Contains("." + id + "."));

                    if (allChildLocation.Any())
                    {
                        var relRow = DbContext.Devices
                            .Any(x => allChildLocation.Select(r => r.LocationId).Contains(x.LocationId ?? 0) && !x.IsDeleted);
                        if (relRow)
                        {
                            return new CUDReturnMessage(ResponseCode.LocationMngt_IsUsing);
                        }
                    }

                    Location query = DbContext.Locations.SingleOrDefault(r => r.LocationId == id && r.IsDeleted == false);

                    query.IsDeleted = true;
                    query.LastUpdatedBy = userId;
                    query.LastUpdatedDate = DateTime.Now;
                    DbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return new CUDReturnMessage(ResponseCode.LocationMngt_SuccessDelete);
        }

        /// <summary>
        /// Thêm hoặc update phòng ban
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateLocation(LocationContract data)
        {
            if (data == null)
            {
                return new CUDReturnMessage(ResponseCode.Error);
            }

            var checkRowName
                = DbContext.Locations
                    .SingleOrDefault(x => x.NameVN == data.NameVN 
                                            && x.LocationId != data.LocationId 
                                            && x.ParentId==data.ParentId);
            if (checkRowName != null)
            {
                return new CUDReturnMessage(ResponseCode.LocationMngt_DuplicateName);
            }
            var checkRowCode
                = DbContext.Locations
                    .SingleOrDefault(x => x.NameEN == data.NameEN
                                          && x.LocationId != data.LocationId
                                          && x.ParentId == data.ParentId);
            if (checkRowCode != null)
            {
                return new CUDReturnMessage(ResponseCode.LocationMngt_DuplicateCode);
            }

            if (data.LocationId == 0)
            {
                Models.Location location = new Models.Location()
                {
                    NameVN = data.NameVN,
                    NameEN = data.NameEN,
                    SloganVN = data.SloganVN,
                    SloganEN = data.SloganEN,
                    LogoName = data.LogoName,
                    BackgroundName = data.BackgroundName,
                    ColorCode = data.ColorCode,
                    ParentId = data.ParentId,
                    LevelNo = data.LevelNo,
                    LevelPath = data.LevelPath,
                    RootId = data.RootId,
                    LayoutTypeId = data.LayoutTypeId,
                    QuestionGroupId = data.QuestionGroupId,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = data.LastUpdatedBy,
                    LastUpdatedDate = DateTime.Now,
                    IsDeleted = false
                };
                DbContext.Locations.Add(location);
                DbContext.SaveChanges();

                if (location.LevelNo == 1)
                {
                    location.RootId = location.LocationId;
                    location.LevelPath = "" + location.LocationId + "";
                }
                else
                {
                    location.LevelPath = location.LevelPath + "." + location.LocationId + "";
                }

                //Add location cho tat ca adminuser
                var superAdmins = DbContext.AdminUsers.Where(x => x.AdminUser_Role.Select(r => r.RId).Contains(1));
                if (superAdmins.Any())
                {
                    foreach (var item in superAdmins)
                    {
                        var userLocation = new Models.AdminUser_Location()
                        {
                            UId = item.UId,
                            LocationId = location.LocationId,
                            CreatedBy = data.CreatedBy,
                            CreatedDate = DateTime.Now,
                            LastUpdatedBy = data.LastUpdatedBy,
                            LastUpdatedDate = DateTime.Now,
                            IsDeleted = false
                        };
                        DbContext.AdminUser_Location.Add(userLocation);
                    }
                }

                //Add quyen cho user tao ra location
                var uL = new AdminUser_Location() {
                    UId = data.CreatedBy,
                    LocationId = location.LocationId,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = data.LastUpdatedBy,
                    LastUpdatedDate = DateTime.Now,
                    IsDeleted = false
                };
                DbContext.AdminUser_Location.Add(uL);

                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.LocationMngt_SuccessCreate);
            }
            else
            {
                var location = DbContext.Locations.SingleOrDefault(r => r.LocationId == data.LocationId);
                if (location == null)
                {
                    return new CUDReturnMessage(ResponseCode.Error);
                }

                location.NameVN = data.NameVN;
                location.NameEN = data.NameEN;
                location.SloganVN = data.SloganVN;
                location.SloganEN = data.SloganEN;
                location.LogoName = data.LogoName;
                location.BackgroundName = data.BackgroundName;
                location.ColorCode = data.ColorCode;
                //location.ParentId = data.ParentId;
                //location.LevelNo = data.LevelNo;
                //location.LevelPath = data.LevelPath;
                //location.RootId = data.RootId;
                location.LayoutTypeId = data.LayoutTypeId;
                location.QuestionGroupId = data.QuestionGroupId;
                location.LastUpdatedBy = data.LastUpdatedBy;
                location.LastUpdatedDate = DateTime.Now;

                //Update các device
                var devices = DbContext.Devices.Where(d => d.LocationId == location.LocationId);
                if (devices.Any())
                {
                    foreach (var item in devices)
                    {
                        item.LayoutTypeId = location.LayoutTypeId;
                        item.QuestionGroupId = location.QuestionGroupId;
                    }
                }

                DbContext.SaveChanges();

                return new CUDReturnMessage(ResponseCode.LocationMngt_SuccessUpdate);
            }
        }
        public CUDReturnMessage UpdateLocationUser(int uId,List<LocationContract> listData)
        {
            try
            {
                if (listData == null)
                {
                    return new CUDReturnMessage(ResponseCode.Error);
                }
                if (listData.Count > 0 && uId > 0)
                {
                    foreach (var item in listData)
                    {
                        //Start ujpdate Location_User
                        var data = DbContext.AdminUser_Location.Where(x => x.LocationId == item.LocationId && x.UId == uId).SingleOrDefault();
                        if (data != null)
                        {
                            data.LocationId = item.LocationId;
                            data.UId = uId;
                            data.IsCurrent = item.IsCurrent;
                            data.IsDeleted = item.IsDeleted;
                            data.LastUpdatedBy = uId;
                            data.LastUpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            data = new AdminUser_Location()
                            {
                                LocationId = item.LocationId,
                                UId = uId,
                                CreatedBy = uId,
                                CreatedDate = DateTime.Now,
                                LastUpdatedBy = uId,
                                LastUpdatedDate = DateTime.Now,
                                IsCurrent = item.IsCurrent,
                                IsDeleted = item.IsDeleted
                            };
                            DbContext.AdminUser_Location.Add(data);
                        }
                        DbContext.SaveChanges();
                    }
                    return new CUDReturnMessage(ResponseCode.LocationMngt_SuccessUpdate);
                }
                else
                    return new CUDReturnMessage(ResponseCode.ItemMngt_NoExists);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<DepartmentType> GetDepartmentListType()
        {
            var query = DbContext.DepartmentTypes.ToList();
            return query;
        }
        public IQueryable<string> GetSiteExistDepartment(int userId)
        {
            IQueryable<string> query = DbContext.AdminUser_PnL_DepartmentSite.Where(r => r.IsDeleted == false && r.UId == userId).Select(x => x.SiteId).Distinct();
            return query;
        }
    }
}