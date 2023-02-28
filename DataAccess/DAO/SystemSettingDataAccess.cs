using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Shared;
using Contract.SystemSetting;
using DataAccess.Models;

namespace DataAccess
{
    public interface ISystemSettingDataAccess
    {
        /// <summary>
        /// Lấy tất cả cấu hình
        /// </summary>
        /// <returns></returns>
        IEnumerable<SystemSetting> Get();

        /// <summary>
        /// Tìm cấu hình theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        SystemSetting Find(string key);

        /// <summary>
        /// Cập nhật cấu hình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        CUDReturnMessage Update(string key, string value, int userId);
    }

    public class SystemSettingDataAccess : BaseDataAccess, ISystemSettingDataAccess
    {
        public SystemSettingDataAccess(string appid, int uid) : base(appid, uid)
        {
        }

        /// <summary>
        /// Lấy tất cả cấu hình
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SystemSetting> Get()
        {
            var settings = DbContext.SystemSettings.Where(r => r.IsDeleted != true);

            if (settings != null && settings.Any())
            {
                return settings;
            }

            return null;
        }

        /// <summary>
        /// Tìm cấu hình theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SystemSetting Find(string key)
        {
            var setting = DbContext.SystemSettings.SingleOrDefault(r => r.Key == key && r.IsDeleted != true);

            if (setting != null)
            {
                return setting;
            }

            return null;
        }

        /// <summary>
        /// Cập nhật cấu hình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CUDReturnMessage Update(string key, string value, int userId)
        {
            var setting = DbContext.SystemSettings.SingleOrDefault(r => r.Key == key && r.IsDeleted != true);

            if (setting != null)
            {
                setting.Value = value;
                setting.LastUpdatedBy = userId;
                setting.LastUpdatedDate = DateTime.Now;

                int affectedRow = DbContext.SaveChanges();
                if (affectedRow > 0)
                {
                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.Successed,
                        Message = ""
                    };
                }
                else
                {
                    return new CUDReturnMessage()
                    {
                        Id = (int)ResponseCode.NoChanged,
                        Message = ""
                    };
                }
            }
            else
            {
                return new CUDReturnMessage()
                {
                    Id = (int)ResponseCode.KeyNotExist,
                    Message = ""
                };
            }
        }
    }
}