using Business.Core;
using Contract.Log;
using Contract.Shared;
using System;
using System.Collections.Generic;

namespace Caching.Core
{
    public interface ILogObjectCaching
    {
        CUDReturnMessage AddLog(LogObjectContract data);
        List<LogObjectContract> FindLog(SearchLogParam data);
    }
    public class LogObjectCaching : BaseCaching, ILogObjectCaching
    {
        private readonly Lazy<ILogObjectBusiness> lazyBusiness;
        public LogObjectCaching()
        {
            lazyBusiness = new Lazy<ILogObjectBusiness>(() => new LogObjectBusiness(appid, uid));
        }
        public CUDReturnMessage AddLog(LogObjectContract data)
        {
            return lazyBusiness.Value.AddLog(data);
        }
        public List<LogObjectContract> FindLog(SearchLogParam data)
        {
            return lazyBusiness.Value.FindLog(data);
        }
    }
}
