using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Core;
using Contract.Shared;
using Contract.SystemSetting;

namespace Caching.Core
{
    public interface ISystemSettingCaching
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

    public class SystemSettingCaching : BaseCaching, ISystemSettingCaching
    {
        private Lazy<SystemSettingBusiness> lazySystemSetting;

        public SystemSettingCaching(/*string appid, int uid*/)  
        {
            lazySystemSetting = new Lazy<SystemSettingBusiness>(() =>
            {
                var instance = new SystemSettingBusiness(appid, uid);
                return instance;
            });
        }

        /// <summary>
        /// Lấy tất cả cấu hình
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SystemSettingContract> Get()
        {
            return lazySystemSetting.Value.Get();
        }

        /// <summary>
        /// Tìm cấu hình theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SystemSettingContract Find(string key)
        {
            return lazySystemSetting.Value.Find(key);
        }

        /// <summary>
        /// Cập nhật cấu hình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CUDReturnMessage Update(string key, string value, int userId)
        {            
            return lazySystemSetting.Value.Update(key, value, userId);
        }
    }
}
