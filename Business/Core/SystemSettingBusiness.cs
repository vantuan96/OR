using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Shared;
using Contract.SystemSetting;
using DataAccess;

namespace Business.Core
{
    public interface ISystemSettingBusiness
    {
        /// <summary>
        /// Lấy tất cả cấu hình
        /// </summary>
        /// <returns></returns>
        IEnumerable<SystemSettingContract> Get();

        /// <summary>
        /// Tìm cấu hình theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        SystemSettingContract Find(string key);

        /// <summary>
        /// Cập nhật cấu hình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        CUDReturnMessage Update(string key, string value, int userId);
    }

    public class SystemSettingBusiness : BaseBusiness, ISystemSettingBusiness
    {
        private Lazy<ISystemSettingDataAccess> lazyDataAccess;

        public SystemSettingBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyDataAccess = new Lazy<ISystemSettingDataAccess>(() =>
            {
                var instance = new SystemSettingDataAccess(appid, uid);
                return instance;
            });
        }

        /// <summary>
        /// Lấy tất cả cấu hình
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SystemSettingContract> Get()
        {
            var data = lazyDataAccess.Value.Get();
            if (data != null)
            {
                return data.Select(x => new SystemSettingContract()
                {
                    Id = x.Id,
                    Key = x.Key,
                    Name = x.Name,
                    Value = x.Value,
                    Description = x.Description,
                    IsAdminConfig = x.IsAdminConfig,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    LastUpdatedBy = x.LastUpdatedBy,
                    LastUpdatedDate = x.LastUpdatedDate
                });
            }
            return null;
        }

        /// <summary>
        /// Tìm cấu hình theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SystemSettingContract Find(string key)
        {
            var data = lazyDataAccess.Value.Find(key);
            if (data != null)
            {
                return new SystemSettingContract()
                {
                    Id = data.Id,
                    Key = data.Key,
                    Name = data.Name,
                    Value = data.Value,
                    Description = data.Description,
                    IsAdminConfig = data.IsAdminConfig,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    LastUpdatedBy = data.LastUpdatedBy,
                    LastUpdatedDate = data.LastUpdatedDate
                };
            }
            return null;
        }

        /// <summary>
        /// Tìm cấu hình theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CUDReturnMessage Update(string key, string value, int userId)
        {
            var data = lazyDataAccess.Value.Update(key, value, userId);
            
            return data;
        }
    }
}