using System;
using System.Collections.Generic;
using System.Linq;
using BMS.Contract.Language;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;
using BMS.DataAccess.Models;

namespace BMS.Business.Core
{
    public interface ILanguageBusiness
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

    public class LanguageBusiness : BaseBusiness, ILanguageBusiness
    {
        private Lazy<ILanguageDataAccess> lazyAccess;

        public LanguageBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyAccess = new Lazy<ILanguageDataAccess>(() =>
            {
                var instance = new LanguageDataAccess(appid, uid);
                return instance;
            });
        }

        /// <summary>
        /// Lấy tất cả ngôn ngữ
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LanguageContract> Get()
        {
            var data = lazyAccess.Value.Get();
            if (data != null)
            {
                return data.Select(x => new LanguageContract()
                {
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                    ShortName = x.ShortName,
                    IsDefault = x.IsDefault,
                    IconUrl = x.IconUrl,
                    IsActive = x.IsActive,
                    Status = x.Status,
                    IsOnsite = x.IsOnsite,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    LastUpdatedBy = x.LastUpdatedBy,
                    LastUpdatedDate = x.LastUpdatedDate
                });
            }
            return null;
        }

        /// <summary>
        /// Tìm ngôn ngữ theo key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public LanguageContract Find(int id)
        {
            var data = lazyAccess.Value.Find(id);
            if (data != null)
            {
                return new LanguageContract()
                {
                    LanguageId = data.LanguageId,
                    Name = data.Name,
                    ShortName = data.ShortName,
                    IsDefault = data.IsDefault,
                    IconUrl = data.IconUrl,
                    IsActive = data.IsActive,
                    Status = data.Status,
                    IsOnsite = data.IsOnsite,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    LastUpdatedBy = data.LastUpdatedBy,
                    LastUpdatedDate = data.LastUpdatedDate
                };
            }
            return null;
        }

        /// <summary>
        /// Thêm mới hoặc cập nhật ngôn ngữ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertOrUpdate(int langId, string name, string shortName, bool isDefault,
            string iconUrl, bool isActive, int status, bool isOnsite, int userId)
        {
            return lazyAccess.Value.InsertOrUpdate(langId, name, shortName, isDefault, iconUrl, isActive, 
                status, isOnsite, userId);
        }

        /// <summary>
        /// Lấy danh sách ngôn ngữ onsite
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LanguageContract> GetOnsiteLanguage()
        {
            var data = lazyAccess.Value.Get((int)ObjectStatus.Onsite);
            if (data != null)
            {
                return data.Select(x => new LanguageContract()
                {
                    LanguageId = x.LanguageId,
                    Name = x.Name,
                    ShortName = x.ShortName,
                    IsDefault = x.IsDefault,
                    IconUrl = x.IconUrl,
                    IsActive = x.IsActive,
                    Status = x.Status,
                    IsOnsite = x.IsOnsite,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    LastUpdatedBy = x.LastUpdatedBy,
                    LastUpdatedDate = x.LastUpdatedDate
                });
            }
            return null;
        }
    }
}