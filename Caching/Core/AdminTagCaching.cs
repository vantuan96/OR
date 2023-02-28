using System;
using System.Collections.Generic;
using BMS.Business.Core;
using BMS.Contract.AdminTag;
using BMS.Contract.Shared;

namespace BMS.Caching.Core
{
    public interface IAdminTagCaching
    {
        /// <summary>
        /// Lấy tất cả tag
        /// </summary>
        /// <returns></returns>
        IEnumerable<AdminTagContract> Get(string name);

        /// <summary>
        /// Tìm tag theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AdminTagContract Find(int id);

        /// <summary>
        /// Cập nhật tag
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        CUDReturnMessage InsertUpdate(AdminTagContract model, int userId);
    }

    public class AdminTagCaching : BaseCaching, IAdminTagCaching
    {
        private Lazy<AdminTagBusiness> lazy;

        public AdminTagCaching(/*string appid, int uid*/)  
        {
            lazy = new Lazy<AdminTagBusiness>(() =>
            {
                var instance = new AdminTagBusiness(appid, uid);
                return instance;
            });
        }

        /// <summary>
        /// Lấy tất cả tag
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdminTagContract> Get(string name)
        {
            return lazy.Value.Get(name);
        }

        /// <summary>
        /// Tìm tag theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdminTagContract Find(int id)
        {
            return lazy.Value.Find(id);
        }

        /// <summary>
        /// Cập nhật tag
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CUDReturnMessage InsertUpdate(AdminTagContract model, int userId)
        {
            return lazy.Value.InsertUpdate(model, userId);
        }
    }
}