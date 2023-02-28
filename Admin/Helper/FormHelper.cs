using Admin.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace Admin.Helper
{
   
    public class SelectListItemCustom : System.Web.Mvc.SelectListItem
    {
    public IDictionary<string, string> itemsHtmlAttributes { get; set; }
    }

    public static class FormHelper
    {
        public static MvcHtmlString DropDownListForCustom(this System.Web.Mvc.HtmlHelper htmlHelper, string id, List<SelectListItemCustom> selectListItems , IDictionary<string, object> htmlAttributes)
        {
            var selectListHtml = "";
            var attrHtml = "";
            

            foreach (var item in selectListItems)
            {
                var attributes = new List<string>();
                foreach (KeyValuePair<string, string> dictItem in item.itemsHtmlAttributes)
                {
                    attributes.Add(string.Format("{0}='{1}'", dictItem.Key, dictItem.Value));
                }
                // do this or some better way of tag building
                selectListHtml += string.Format(
                    "<option value='{0}' {1} {2}>{3}</option>", item.Value,item.Selected ? "selected" : string.Empty,string.Join(" ", attributes.ToArray()),item.Text);

            }

            var attributesControll = new List<string>();
            foreach (var itemAttribute in htmlAttributes)
            {
                attributesControll.Add(string.Format("{0}='{1}'", itemAttribute.Key, itemAttribute.Value));
            }
            attrHtml = string.Join(" ", attributesControll.ToArray());

                    // do this or some better way of tag building
                var html = string.Format("<select id='{0}' name='{0}' {2}>{1}</select>", id, selectListHtml, attrHtml);

            return new MvcHtmlString(html);
        }

        public static MvcHtmlString DropDownListForCustomExt(this System.Web.Mvc.HtmlHelper htmlHelper, string id, List<SelectListItemCustom> selectListItems,  object  htmlAttributes)
        {
            var selectListHtml = "";         
            foreach (var item in selectListItems)
            {
                var attributes = new List<string>();
                foreach (KeyValuePair<string, string> dictItem in item.itemsHtmlAttributes)
                {
                    attributes.Add(string.Format("{0}='{1}'", dictItem.Key, dictItem.Value));
                }
                selectListHtml += string.Format(
                    "<option value='{0}' {1} {2}>{3}</option>", item.Value, item.Selected ? "selected" : string.Empty, string.Join(" ", attributes.ToArray()), item.Text);
            }
            var html = string.Format("<select id='{0}' name='{0}' {2} >{1}</select>", id, selectListHtml, ToClassString(htmlAttributes)); //multiple
            return new MvcHtmlString(html);
        }
        public static string ToClassString(this object value)
        {
            if (value == null)
                return string.Empty;
            var attrHtml = "";
            var attributesControll = new List<string>();
            foreach (var prop in value.GetType().GetProperties())
            {
                attributesControll.Add(string.Format("{0}='{1}'", prop.Name, prop.GetValue(value)));
            }
            attrHtml = string.Join(" ", attributesControll.ToArray());
            return attrHtml;
        }


    }

   
}
