using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Business.Core;
using Contract.Microsite;
using Contract.Shared;

namespace Caching.Microsite
{
    public interface IMicrositeMngtCaching
    {
        List<MicrositeContract> GetListMicrosite(int userId);
        MicrositeDetailContract GetMicrositeDetail(int msId);
        CUDReturnMessage CreateUpdateMicrosite(MicrositeContentContract ms);
        CUDReturnMessage UpdateMicrositeStatus(int msId, int status);
        CUDReturnMessage DeleteMicrosite(int msId);

     
    }

    public class MicrositeMngtCaching : BaseCaching, IMicrositeMngtCaching
    {
        private Lazy<IMicrositeMngtBusiness> lazyMicrositeMngtBusiness;
        private string typeName = "MicrositeMngtCaching";

        public MicrositeMngtCaching(/*string appid, int uid*/)  
        {
            lazyMicrositeMngtBusiness = new Lazy<IMicrositeMngtBusiness>(() => new MicrositeMngtBusiness(appid, uid));
        }

        public List<MicrositeContract> GetListMicrosite(int userId)
        {
            return lazyMicrositeMngtBusiness.Value.GetListMicrosite(userId);

            //return base.ProcessCache<List<MicrositeContract>>(()=>{
            //    return base.GetDataWithMemCache<MicrositeMngtBusiness, List<MicrositeContract>>(
            //        lazyMicrositeMngtBusiness.Value,
            //        "GetListMicrosite",
            //        new object[] { status },
            //        CacheTimeout.Medium);
            //},typeName ,"GetListMicrosite");
        }

        public MicrositeDetailContract GetMicrositeDetail(int msId)
        {
            return lazyMicrositeMngtBusiness.Value.GetMicrositeDetail(msId);
        }
        public CUDReturnMessage CreateUpdateMicrosite(MicrositeContentContract ms)
        {
            return lazyMicrositeMngtBusiness.Value.CreateUpdateMicrosite(ms);
        }

        public CUDReturnMessage UpdateMicrositeStatus(int msId, int status)
        {
            return lazyMicrositeMngtBusiness.Value.UpdateMicrositeStatus(msId, status);
        }

        public CUDReturnMessage DeleteMicrosite(int msId)
        {
            return lazyMicrositeMngtBusiness.Value.DeleteMicrosite(msId);
        }
        
     
    }
}
