using Contract.OR;
using Contract.Shared;
using Contract.User;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Core
{
    public interface ILocationBusiness
    {
        /// <summary>
        /// Lấy ds tất cả vị trí (pnl, cơ sở, chi nhánh, phòng ban)
        /// </summary>
        /// <returns></returns>
        List<LocationContract> Get();

        /// <summary>
        /// Lấy thông tin phòng ban
        /// </summary>
        /// <returns></returns>
        LocationContract Find(int id);

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
        List<LocationContract> GetSiteExistDepartment(int userId);
    }

    public class LocationBusiness : BaseBusiness, ILocationBusiness
    {
        private Lazy<ILocationDataAccess> lazyLocation;

        public LocationBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyLocation = new Lazy<ILocationDataAccess>(() => new LocationDataAccess(appid, uid));
        }

        /// <summary>
        /// Lấy ds tất cả vị trí (pnl, cơ sở, chi nhánh, phòng ban)
        /// </summary>
        /// <returns></returns>
        public List<LocationContract> Get()
        {
            var data = lazyLocation.Value.Get();
            //var departmentType = lazyLocation.Value.GetDepartmentListType().ToList();
            //var lstdp = new List<DepartmentTypeV01>();
            //foreach (var item in departmentType)
            //{
            //    lstdp.Add(new DepartmentTypeV01 { DepartmentTypeId = item.DepartmentTypeId, DepartmentTypeName = item.DepartmentTypeName });
            //}

            return data.Select(x => new LocationContract()
            {
                LocationId = x.LocationId,
                NameVN = x.NameVN,
                NameEN = x.NameEN,
                SloganVN = x.SloganVN,
                SloganEN = x.SloganEN,
                LogoName = x.LogoName,
                BackgroundName = x.BackgroundName,
                ColorCode = x.ColorCode,
                ParentId = x.ParentId,
                LevelNo = x.LevelNo,
                LevelPath = x.LevelPath,
                RootId = x.RootId,
                LayoutTypeId = x.LayoutTypeId,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdatedDate = x.LastUpdatedDate,
                IsDeleted = x.IsDeleted,
                QuestionGroupId = x.QuestionGroupId,
                //LayoutTypeName = lstdp.FirstOrDefault(d => d.DepartmentTypeId == x.LayoutTypeId).DepartmentTypeName
            }).ToList();
        }

        /// <summary>
        /// Lấy thông tin phòng ban
        /// </summary>
        /// <returns></returns>
        public LocationContract Find(int id)
        {
            var x = lazyLocation.Value.Find(id);
            return new LocationContract()
            {
                LocationId = x.LocationId,
                NameVN = x.NameVN,
                NameEN = x.NameEN,
                SloganVN = x.SloganVN,
                SloganEN = x.SloganEN,
                LogoName = x.LogoName,
                BackgroundName = x.BackgroundName,
                ColorCode = x.ColorCode,
                ParentId = Convert.ToInt32(x.ParentId),
                LevelNo = x.LevelNo,
                LevelPath = x.LevelPath,
                RootId = Convert.ToInt32(x.RootId),
                LayoutTypeId = Convert.ToInt32(x.LayoutTypeId),
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdatedDate = x.LastUpdatedDate,
                IsDeleted = x.IsDeleted,
                QuestionGroupId = Convert.ToInt32(x.QuestionGroupId),
            };
        }

        /// <summary>
        /// Xóa phòng ban
        /// </summary>
        /// <returns></returns>
        public CUDReturnMessage Delete(int id, int userId)
        {
            return lazyLocation.Value.Delete(id, userId);
        }

        /// <summary>
        /// Thêm hoặc update phòng ban
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateLocation(LocationContract data)
        {
            //Root node
            if (data.ParentId == 0)
            {
                data.LevelNo = 1;
                data.ParentId = 0;
            }
            else
            {
                var parent = this.Find(data.ParentId ?? 0);
                if (parent != null)
                {
                    data.RootId = parent.RootId;
                    data.SloganEN = parent.SloganEN;
                    data.SloganVN = parent.SloganVN;
                    data.LogoName = parent.LogoName;
                    data.BackgroundName = parent.BackgroundName;
                    data.ColorCode = parent.ColorCode;
                    data.LevelNo = parent.LevelNo + 1;
                    data.RootId = parent.RootId;
                    data.LevelPath = parent.LevelPath;
                    //data.LayoutTypeId = parent.LayoutTypeId;
                }
                else
                {
                    return new CUDReturnMessage(ResponseCode.Error);
                }
            }

            return lazyLocation.Value.InsertUpdateLocation(data);

        }
        public CUDReturnMessage UpdateLocationUser(int uId, List<LocationContract> listData)
        {
            return lazyLocation.Value.UpdateLocationUser(uId,listData);
        }
        public List<LocationContract> GetSiteExistDepartment(int userId)
        {
            var data = lazyLocation.Value.Get();
            var dataSite = lazyLocation.Value.GetSiteExistDepartment(userId);
            if (dataSite == null || !dataSite.Any()) return new List<LocationContract>();
            return data.Where(d => dataSite.Contains(d.NameEN)).Select(x => new LocationContract()
            {
                LocationId = x.LocationId,
                NameVN = x.NameVN,
                NameEN = x.NameEN,
                SloganVN = x.SloganVN,
                SloganEN = x.SloganEN,
                LogoName = x.LogoName,
                BackgroundName = x.BackgroundName,
                ColorCode = x.ColorCode,
                ParentId = x.ParentId,
                LevelNo = x.LevelNo,
                LevelPath = x.LevelPath,
                RootId = x.RootId,
                LayoutTypeId = x.LayoutTypeId,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdatedDate = x.LastUpdatedDate,
                IsDeleted = x.IsDeleted,
                QuestionGroupId = x.QuestionGroupId,
                SourceClientId = x.SourceClientId ?? (int)SourceClientEnum.Oh
            }).ToList();
        }
    }
}