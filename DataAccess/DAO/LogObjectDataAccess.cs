using Contract.Log;
using Contract.Shared;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public interface ILogObjectDataAccess
    {
        CUDReturnMessage AddLog(LogObjectContract data);
        IQueryable<LogObject> FindLog(SearchLogParam data);
    }
    public class LogObjectDataAccess : BaseDataAccess, ILogObjectDataAccess
    {
        public LogObjectDataAccess(string appid, int uid) : base(appid, uid)
        {
        }

        public CUDReturnMessage AddLog(LogObjectContract data)
        {
            try
            {
                var log = new LogObject()
                {
                    Id = data.Id,
                    ObjectId = data.ObjectId,
                    OldState = data.OldState,
                    NewState = data.NewState,
                    ObjectTypeId = data.ObjectTypeId,
                    ActionId = data.ActionId,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = DateTime.Now,
                    UserAssigned = data.UserAssigned,
                    OldInformation = data.OldInformation,
                    NewInformation = data.NewInformation,
                    
                };
                DbContext.LogObjects.Add(log);
                DbContext.SaveChanges();
                return new CUDReturnMessage(ResponseCode.LogObject_CreatedSuccess);
            }catch (Exception ex)
            {
                return new CUDReturnMessage(ResponseCode.LogObject_Error);
            }            
        }
        public IQueryable<LogObject> FindLog(SearchLogParam param)
        {
            var query = DbContext.LogObjects.AsQueryable();
            if (param == null) return query;
            if (param.ObjectId > 0)
                query = query.Where(c => c.ObjectId == param.ObjectId);
            if (param.ObjectTypeId > 0)
                query = query.Where(c => c.ObjectTypeId == (int)param.ObjectTypeId);
            return query;
        }
    }
}
