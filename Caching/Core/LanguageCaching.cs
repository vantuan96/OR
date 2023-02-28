using System;
using System.Collections.Generic;
using BMS.Business.Core;
using BMS.Contract.Language;
using BMS.Contract.Shared;

namespace BMS.Caching.Core
{
    public interface ILanguageCaching
    {
        /// <summary>
        /// Lấy tất cả ngôn ngữ
        /// </summary>
        /// <returns></returns>
        IEnumerable<LanguageContract> Get();

        /// <summary>
        /// Tìm ngôn ngữ theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        LanguageContract Find(int id);

        /// <summary>
        /// Thêm mới hoặc cập nhật ngôn ngữ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        CUDReturnMessage InsertOrUpdate(int langId, string name, string shortName, bool isDefault,
            string iconUrl, bool isActive, int status, bool isOnsite, int userId);

        /// <summary>
        /// Lấy danh sách ngôn ngữ onsite
        /// </summary>
        /// <returns></returns>
        IEnumerable<LanguageContract> GetOnsiteLanguage();
    }

    public class LanguageCaching : BaseCaching, ILanguageCaching
    {
        private Lazy<LanguageBusiness> lazy;

        public LanguageCaching(/*string appid, int uid*/)  
        {
            lazy = new Lazy<LanguageBusiness>(() =>
            {
                var instance = new LanguageBusiness(appid, uid);
                return instance;
            });
        }

        /// <summary>
        /// Lấy tất cả ngôn ngữ
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LanguageContract> Get()
        {
            return lazy.Value.Get();
        }

        /// <summary>
        /// Tìm ngôn ngữ theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public LanguageContract Find(int id)
        {
            return lazy.Value.Find(id);
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật ngôn ngữ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdate(int langId, string name, string shortName, bool isDefault,
            string iconUrl, bool isActive, int status, bool isOnsite, int userId)
        {
            return lazy.Value.InsertOrUpdate(langId, name, shortName, isDefault, iconUrl, isActive,
                status, isOnsite, userId);
        }


        /// <summary>
        /// Lấy danh sách ngôn ngữ onsite
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LanguageContract> GetOnsiteLanguage()
        {
            return lazy.Value.GetOnsiteLanguage();
        }
    }
}