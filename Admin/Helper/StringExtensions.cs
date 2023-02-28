using System.Text.RegularExpressions;
using System;
using Admin.Models;

namespace Admin.Helper
{
    public static class StringExtensions
    {
        private static readonly string[] AllowedHTMLTags = new string[]
        {
            "a",
            "b",
            "i",
            "strong",
            "p",
        };

        public static string RemoveUnwantedTags(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return data;



            data = data.Replace("<", "&lt;");
            data = data.Replace(">", "&gt;");

            foreach (var s in AllowedHTMLTags)
            {
                //data = data.Replace(""
            }

            return data;
        }
        
        public static int TryParseToInt(this string str)
        {
            int output = 0;
            if (!string.IsNullOrEmpty(str))
                int.TryParse(str, out output);
            return output;
        }

        /// <summary>
        /// parse string value to integer with -1 is default value
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int TryParseToIntWithGlosbeValueDefault(this string str)
        {
            try
            {
                return int.Parse(str.Trim());
            }
            catch (Exception)
            {
                return -2;
            }
        }

        public static short TryParseToShort(this string str)
        {
            short output = 0;
            if (!string.IsNullOrEmpty(str))
                short.TryParse(str, out output);
            return output;
        }

        public static DateTime TryParseToDate(this string str)
        {
            try
            {
                return DateTime.Parse(str);
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        public static string RemoveScriptTag(this string str)
        {
            try
            {
                return new Regex("<script*>|</script>").Replace(str," ");
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string HtmlPaging(int TotalRow, int PageIndex, bool IsShowCount = true, int PageSize = 10)
        {
            string html = string.Empty;
                var pagingObj = new PagingModel(TotalRow, PageIndex, PageSize);
            if (pagingObj.LastPage > 1)
            {
                if (IsShowCount)
                {
                    html += "<div class=\"col -md-5 clearfix col-sm-5\">";
                    html += "<div class=\"dataTables_info\">Show items from " + pagingObj.StartRowIndex + " to " + @pagingObj.EndRowIndex + " in " + @TotalRow + " patients (Hiển thị từ " + pagingObj.StartRowIndex + " đến " + @pagingObj.EndRowIndex + " trong " + @TotalRow + " bản ghi)</div>";
                    html += "</div>";
                }

                html += "<div class=\"col -md-7 clearfix col-sm-7\">";
                html += "       <div class=\"dataTables_paginate paging_simple_numbers\">";
                html += "          <ul class=\"pagination pagination-sm\">";
                foreach (var r in pagingObj.ListButton)
                {
                    html += "<li class=\"" + r.CssClass + "\">";
                    html += "        <a href =\"" + r.Href + "\">" + r.InnerText + "</a>";//href =\"" + r.Href + "\"
                    html += "    </li>";
                }
                html += "</ul> </div> </div>"; 
            }             
            return html;
        }
    }
}