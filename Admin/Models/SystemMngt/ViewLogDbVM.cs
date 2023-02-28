using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.Common;
using Admin.Helper;
using Contract.SystemLog;

namespace Admin.Models.SystemMngt
{
    public class ViewLogDbVM
    {
        public List<LogDbContract> ListRuntimeError { get; set; }

        public SelectList Clients { get; set; }

        /// <summary>
        /// lỗi từ DB hay Client
        /// </summary>
        public SelectList SourceError { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }

        public string FromDateToDateText
        {
            get
            {
                return string.Concat(FromDate.ToVEShortDate(), " - ", ToDate.ToVEShortDate());
            }
        }

        public string FromDateHidden
        {
            get
            {
                return FromDate.ToString(AdminGlobal.EnglishDateShortFormat);
            }
        }

        public string ToDateHidden
        {
            get
            {
                return ToDate.ToString(AdminGlobal.EnglishDateShortFormat);
            }
        }
    }
}