using System;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Models.Report
{
    public class BaseReportModel<T> : BaseReportModel
    {
        /// <summary>
        /// du lieu report
        /// </summary>
        public T ReportData { get; set; }

    }

    public class BaseReportModel
    {
        /// <summary>
        /// json du lieu report
        /// </summary>
        public string TableContent { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

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
                return FromDate.ToString("yyyy-MM-dd");
            }
        }

        public string ToDateHidden
        {
            get
            {
                return ToDate.ToString("yyyy-MM-dd");
            }
        }

    }
}