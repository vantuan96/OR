using System.Web;

namespace Business.Share
{
    public class ShareFunctions
    {
        public static T GetSession<T>(string sessionName)
        {
            var session = HttpContext.Current.Session;
            if (session == null || session[sessionName] == null)
                return default(T);
            else
                return (T)session[sessionName];
        }

        public static void SetSession(string sessionName, object value)
        {
            var session = HttpContext.Current.Session;
            session[sessionName] = value;
        }
    }
}
