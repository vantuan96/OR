using Admin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Contract.User;

public class MvcHelper
{
    private static List<Type> GetSubClasses<T>()
    {
        return Assembly.GetCallingAssembly().GetTypes().Where(
            type => type.IsSubclassOf(typeof(T))).ToList();
    }

    public List<string> GetControllerNames()
    {
        List<string> controllerNames = new List<string>();
        GetSubClasses<Controller>().ForEach(type => controllerNames.Add(type.Name.Substring(0, type.Name.Length - 10)));
        return controllerNames;
    }

    public static bool IsIEBrowser(HttpContext context)
    {
        //return false;

        if (context.Request.Browser.Browser == "IE")
        {
            return context.Request.Browser.MajorVersion <= 8;
            //if (context.Request.Browser.MajorVersion == 7) { ieClass = "ie7"; }
            //else if (context.Request.Browser.MajorVersion == 8) { ieClass = "ie8"; }
            //else if (Request.Browser.MajorVersion == 9) { ieClass = "ie9"; }
        }
        else { return false; }
    }

    public static T GetSession<T>(HttpContext context, string key)
    {
        T retSession = default(T);
        try
        {
            retSession = (T)context.Session[key];
        }
        catch (Exception ex)
        {
            throw new Exception("Could not convert Session[\"" + key + "\"] to type " + retSession.GetType().Name);
        }

        return retSession;
    }

    public static MemberExtendContract GetUserSession(HttpContext context)
    {
        return GetSession<MemberExtendContract>(context, AdminConfiguration.CurrentUserSessionKey);
    }
}