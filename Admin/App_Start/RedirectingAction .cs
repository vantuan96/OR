using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.App_Start
{
    public class RedirectingAction : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (true)
            {
                //context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                //{
                //    controller = "Home",
                //    action = "Index"
                //}));
            }
        }
    }
}