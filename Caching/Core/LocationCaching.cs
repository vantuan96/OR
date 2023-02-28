using Business.Core;
using Contract.Shared;
using Contract.User;
using System;
using System.Collections.Generic;

namespace Caching.Core
{
    public interface ILocationCaching
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

    public class LocationCaching : BaseCaching, ILocationCaching

    {
        private Lazy<ILocationBusiness> lazy;

        public LocationCaching(/*string appid, int uid*/)
        {
            lazy = new Lazy<ILocationBusiness>(() => new LocationBusiness(appid, uid));
        }

        /// <summary>
        /// Lấy ds tất cả vị trí (pnl, cơ sở, chi nhánh, phòng ban)
        /// </summary>
        /// <returns></returns>
        public List<LocationContract> Get()
        {
            return lazy.Value.Get();
        }

        /// <summary>
        /// Lấy thông tin phòng ban
        /// </summary>
        /// <returns></returns>
        public LocationContract Find(int id)
        {
            return lazy.Value.Find(id);
        }

        /// <summary>
        /// Xóa phòng ban
        /// </summary>
        /// <returns></returns>
        public CUDReturnMessage Delete(int id, int userId)
        {
            return lazy.Value.Delete(id, userId);
        }

        /// <summary>
        /// Thêm hoặc update phòng ban
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdateLocation(LocationContract data)
        {
            return lazy.Value.InsertUpdateLocation(data);
        }
        public CUDReturnMessage UpdateLocationUser(int uId, List<LocationContract> listData)
        {
            return lazy.Value.UpdateLocationUser(uId,listData);
        }
        public List<LocationContract> GetSiteExistDepartment(int userId)
        {
            return lazy.Value.GetSiteExistDepartment(userId);
        }
    }
}