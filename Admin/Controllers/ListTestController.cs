using System.Web.Mvc;
using Caching.Core;

namespace Admin.Controllers
{
    public class ListTestController : BaseController
    {
        private ISystemSettingCaching _systemSettingApi;
        private IAuthenCaching _authenCaching;
        public ListTestController(ISystemSettingCaching systemSettingApi,
            IAuthenCaching authenCaching)
            : base(authenCaching, systemSettingApi)
        {
            this._systemSettingApi = systemSettingApi;
            this._authenCaching = authenCaching;
        }
        // GET: ListTest
        public ActionResult Index()
        {
            
            return View();
        }


    }
}