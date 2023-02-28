using System.Web.Mvc;

namespace Admin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleExecuteAction());
        }
        public class HandleExecuteAction : IActionFilter, IResultFilter
        {
            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
            }

            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
            }

            public void OnResultExecuted(ResultExecutedContext filterContext)
            {
            }

            public void OnResultExecuting(ResultExecutingContext filterContext)
            {
            }
        }
    }
}