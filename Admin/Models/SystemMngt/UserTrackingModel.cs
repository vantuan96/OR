using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VG.Common;
using Contract.User;

namespace Admin.Models.SystemMngt
{
    public class UserTrackingModel
    {
        public List<UserTrackingContract> ListTracking { get; set; }

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