using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMS.Contract.Shared;
using BMS.DataAccess;
using BMS.DataAccess.DAO;

namespace BMS.Business.Subscribe
{
    public interface ISubscribeBusiness
    {
        CUDReturnMessage Subscribe(string email, int msid);

        CUDReturnMessage SubmitContact(string name, string email, string message, int msId);
    }

    public class SubscribeBusiness : BaseBusiness, ISubscribeBusiness
    {
        private Lazy<ISubscriberDataAccess> lazySubscribeDataAccess;
        private Lazy<IContactMessageDataAccess> lazyContactMessageDataAccess;

        public SubscribeBusiness(string appid, int uid) : base(appid, uid)
        {
            lazySubscribeDataAccess = new Lazy<ISubscriberDataAccess>(() => new SubscriberDataAccess(appid, uid));
            lazyContactMessageDataAccess = new Lazy<IContactMessageDataAccess>(() => new ContactMessageDataAccess(appid, uid));

        }

        public CUDReturnMessage Subscribe(string email, int msid)
        {
            return lazySubscribeDataAccess.Value.InsertSubscriber(email, msid);
        }

        public CUDReturnMessage SubmitContact(string name, string email, string message, int msId)
        {
            return lazyContactMessageDataAccess.Value.CreateContactMessage(name, email, message, msId);
        }
    }
}
