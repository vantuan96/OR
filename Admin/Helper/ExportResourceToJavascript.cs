using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using Admin.Resource;

namespace Admin.Helper
{
    public static class ExportResourceToJavascript
    {
        public static void Export(string filepath)
        {
            try
            {
                ResourceSet resourceSet = MessageResource.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                IDictionary a = resourceSet.Cast<DictionaryEntry>().ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());
                string strMessageResource = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(a);

                ResourceSet resourceLayoutSet = LayoutResource.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                IDictionary b = resourceLayoutSet.Cast<DictionaryEntry>().ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());
                string strFormResource = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(b);

                string content = "var msg_lang = " + strMessageResource + "; var layout_lang =" + strFormResource;
                File.WriteAllText(filepath, content, Encoding.UTF8);
            }
            catch { }
        }
    }
}