using System.Web.Mvc;
using Caching.Core;

namespace Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(ISystemSettingCaching systemSettingApi,
            IAuthenCaching authenCaching
            ) : base(authenCaching, systemSettingApi)
        {

        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
    }
}
