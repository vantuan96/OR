using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using VG.Framework.Mvc.Conventions;
using Admin.Helper;
using Admin.Resource;
using Admin.ClientValidation;
using System.Web.Http;
using System.Web;

namespace Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            
            // xuất resource ra javascript
            // chỉ cần chạy ở local, lên server có thể không chạy cũng không sao
            ExportResourceToJavascript.Export(Server.MapPath("~/assets/js/adr_global_lang.js"));

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Bootstrapper.Initialise();

            ModelMetadataProviders.Current = new ConventionalModelMetadataProvider(false, typeof(MessageResource));
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            ModelBinders.Binders.DefaultBinder = new Admin.Helper.AdminGlobal.TrimModelBinder();
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                var context = Context;
                var request = Request;
                if (context != null && context.Request.HttpMethod == "POST")
                {
                    var objUser = VinAuthentication.CurrentUser(request.RequestContext.HttpContext); //Object user logged on
                    if (objUser != null)
                    {
                        var requestPath = context.Request.CurrentExecutionFilePath;

                        if (requestPath.Equals("/Auth/DoLogin")) return;

                        string textInputStream = string.Empty;
                        if (context.Request.InputStream != null) // Project su dung angularjs, post stream
                        {
                            Stream temp_stream = new MemoryStream();
                            context.Request.InputStream.CopyTo(temp_stream);
                            temp_stream.Seek(0, SeekOrigin.Begin);
                            using (StreamReader reader = new StreamReader(temp_stream))
                            {
                                textInputStream = reader.ReadToEnd();
                            }
                            context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                        }
                        else //Project post form
                        {
                            textInputStream = context.Request.Form.ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            HttpContext.Current.Response.Headers.Remove("Server");
        }
    }
}