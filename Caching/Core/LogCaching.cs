using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Core;
using Contract.Core;
using Contract.Shared;
using Contract.SystemLog;

namespace Caching.Core
{
    public interface ILogCaching
    {
        void InsertLog(string content, SystemLogType logType, SourceType sourceType);

        PagedList<LogDbContract> GetListLogDb(int logTypeId, int sourceId, DateTime fromDate, DateTime toDate, int pageSize, int page);

    }

    public class LogCaching : BaseCaching, ILogCaching
    {
        private Lazy<LogBusiness> lazyLogBusiness;

        public LogCaching(/*string appid, int uid*/)  
        {
            lazyLogBusiness = new Lazy<LogBusiness>(() =>
            {
                var instance = new LogBusiness(appid, uid);
                return instance;
            });
        }

        public void InsertLog(string content, SystemLogType logType, SourceType sourceType)
        {
            lazyLogBusiness.Value.InsertLog(content, logType, sourceType);
        }

        public PagedList<LogDbContract> GetListLogDb(int logTypeId, int sourceId, DateTime fromDate, DateTime toDate, int pageSize, int page)
        {
            return lazyLogBusiness.Value.GetListLogDb(logTypeId, sourceId, fromDate, toDate, pageSize, page);
        }

    }
}
