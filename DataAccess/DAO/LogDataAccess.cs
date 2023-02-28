using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Core;
using Contract.Shared;
using DataAccess.Models;

namespace DataAccess
{
    public interface ILogDataAccess
    {

        void InsertRuntimeLogging(string appId, int uid, string content, SystemLogType logType, SourceType SourceType);

        IQueryable<SystemRuntimeLog> GetListLogDb(int logTypeId, int sourceId, DateTime fromDate, DateTime toDate);
    }

    public class LogDataAccess : BaseDataAccess, ILogDataAccess
    {
        public LogDataAccess(string appid, int uid) : base(appid, uid)
        {

        }

        public void InsertRuntimeLogging(string appId, int uid, string content, SystemLogType logType, SourceType SourceType)
        {
            var newRow = new SystemRuntimeLog()
            {
                CreatedDate = DateTime.Now,
                LogContent = content,
                LogType = (byte)logType,
                SourceId = (int)SourceType,
                UserId = uid,
                Id = Guid.NewGuid()
            };

            DbContext.SystemRuntimeLogs.Add(newRow);

            int affectedRow = DbContext.SaveChanges();


            //if (affectedRow > 0)
            //{
            //    return new CUDReturnMessage()
            //    {
            //        Id = (int)ResponseCode.Successed,
            //        Message = ""
            //    };
            //}
            //else
            //{
            //    return new CUDReturnMessage()
            //    {
            //        Id = (int)ResponseCode.NoChanged,
            //        Message = ""
            //    };
            //}


        }

        public IQueryable<SystemRuntimeLog> GetListLogDb(int logTypeId, int sourceId, DateTime fromDate, DateTime toDate)
        {
            IQueryable<Models.SystemRuntimeLog> query = DbContext.SystemRuntimeLogs;

            query = query.Where(r => r.CreatedDate > fromDate && r.CreatedDate < toDate);

            if (logTypeId > 0)
            {
                query = query.Where(r => r.LogType == logTypeId);
            }

            if (sourceId > 0)
            {
                query = query.Where(r => r.SourceId == sourceId);
            }

            return query;
        }


       
    }
}
