using System;
using System.Collections.Generic;
using System.Linq;
using BMS.Contract.AdminTag;
using BMS.Contract.Shared;
using BMS.DataAccess.DAO;

namespace BMS.Business.Core
{
    public interface IAdminTagBusiness
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

    public class AdminTagBusiness : BaseBusiness, IAdminTagBusiness
    {
        private Lazy<AdminTagDataAccess> lazy;

        public AdminTagBusiness(string appid, int uid) : base(appid, uid)
        {
            lazy = new Lazy<AdminTagDataAccess>(() =>
            {
                var instance = new AdminTagDataAccess(appid, uid);
                return instance;
            });
        }

        /// <summary>
        /// Lấy tất cả tag
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdminTagContract> Get(string name)
        {
            var query = lazy.Value.Get(name, null);

            return query.Select(x => new AdminTagContract()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IsPredefined = x.IsPredefined,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdatedDate = x.LastUpdatedDate
            });
        }

        /// <summary>
        /// Tìm tag theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdminTagContract Find(int id)
        {
            var data = lazy.Value.Find(id);
            if (data != null)
            {
                return new AdminTagContract()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    IsPredefined = data.IsPredefined,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    LastUpdatedBy = data.LastUpdatedBy,
                    LastUpdatedDate = data.LastUpdatedDate
                };
            }
            return null;
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