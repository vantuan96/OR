using System;
using BMS.Business.Subscribe;
using BMS.Contract.Shared;

namespace BMS.Caching.Subscribe
{
    public interface ISubscribeCaching
    {
        CUDReturnMessage Subscribe(string email, int msid);

        CUDReturnMessage SubmitContact(string name, string email, string message, int msId);
    }

    public class SubscribeCaching : BaseCaching, ISubscribeCaching
    {
        Lazy<ISubscribeBusiness> lazySubscribeBusiness;

        public SubscribeCaching(/*string appid, int uid*/)  
        {
            lazySubscribeBusiness = new Lazy<ISubscribeBusiness>(() => new SubscribeBusiness(appid, uid));
        }

        public CUDReturnMessage Subscribe(string email, int msid)
        {
            return lazySubscribeBusiness.Value.Subscribe(email, msid);
        }

        public CUDReturnMessage SubmitContact(string name, string email, string message, int msId)
        {
            return lazySubscribeBusiness.Value.SubmitContact(name, email, message, msId);
        }
    }
}
