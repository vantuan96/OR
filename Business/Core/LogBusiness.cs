using System;
using System.Collections.Generic;
using System.Linq;
using Contract.Core;
using Contract.Shared;
using Contract.SystemLog;
using DataAccess;
using DataAccess.Models;

namespace Business.Core
{
    public interface ILogBusiness
    {
        void InsertLog(string content, SystemLogType logType, SourceType sourceType);

        PagedList<LogDbContract> GetListLogDb(int logTypeId, int sourceId, DateTime fromDate, DateTime toDate, int pageSize, int page);
    }

    public class LogBusiness : BaseBusiness, ILogBusiness
    {
        private Lazy<ILogDataAccess> lazyAccess;
        public LogBusiness(string appid, int uid) : base(appid, uid)
        {
            lazyAccess = new Lazy<ILogDataAccess>(() =>
            {
                return new LogDataAccess(appid, uid);
            });
        }

        public void InsertLog(string content, SystemLogType logType, SourceType sourceType)
        {
            lazyAccess.Value.InsertRuntimeLogging(appid, uid, content, logType, sourceType);
        }

        public PagedList<LogDbContract> GetListLogDb(int logTypeId, int sourceId, DateTime fromDate, DateTime toDate, int pageSize, int page)
        {
            var query = lazyAccess.Value.GetListLogDb(logTypeId, sourceId, fromDate, toDate);
            var result = new PagedList<LogDbContract>(query.Count());

            if (result.Count > 0)
            {
                result.List = query.OrderByDescending(r => r.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize).Select(r => new LogDbContract
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    LogContent = r.LogContent,
                    CreatedDate = r.CreatedDate,
                    LogTypeId = r.LogType,
                    SourceId = r.SourceId
                }).ToList();
            }

            return result;
        }
    }
}
