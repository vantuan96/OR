using Contract.Log;
using Contract.Shared;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Core
{
    public interface ILogObjectBusiness
    {
        CUDReturnMessage AddLog(LogObjectContract data);
        List<LogObjectContract> FindLog(SearchLogParam data);
    }
    public class LogObjectBusiness : BaseBusiness, ILogObjectBusiness
    {
        private readonly Lazy<ILogObjectDataAccess> lazyAccess;
        public LogObjectBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyAccess = new Lazy<ILogObjectDataAccess>(() =>
            {
                return new LogObjectDataAccess(appid, uid);
            });
        }

        public CUDReturnMessage AddLog(LogObjectContract data)
        {
            return lazyAccess.Value.AddLog(data);
        }
        public List<LogObjectContract> FindLog(SearchLogParam param)
        {
            var result = new List<LogObjectContract>();
            var data=lazyAccess.Value.FindLog(param).ToList();
            if (data == null) return result;
            result = data.Select(c => new LogObjectContract()
            {
                Id = c.Id,
                ObjectId = c.ObjectId,
                OldState = c.OldState??0,
                NewState = c.NewState??0,
                ObjectTypeId = c.ObjectTypeId,
                ActionId = c.ActionId,
                CreatedBy = c.CreatedBy??0,
                CreatedDate = c.CreatedDate??DateTime.Now,
                UserAssigned = c.UserAssigned??0,
                OldInformation = c.OldInformation,
                NewInformation = c.NewInformation,
                CreatedByName= c.AdminUser!=null?c.AdminUser.Name:string.Empty
            }).ToList();
            return result;
        }
    }
}
