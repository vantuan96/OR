using Admin.Resource;
using Contract.OR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Models.OR
{
    public class OHViewPatientPublicModel
    {
        [Display(Name = "AdminTools_Template_KeyWord", ResourceType = typeof(LayoutResource))]
        [Required]
        public string kw { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public List<ORAnesthProgressContract> listData { get; set; }
        public List<SelectListItem> listStates { get; set; }


        public int State { get; set; }
        public int RoomId { get; set; }

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
        public string siteId { get; set; }
        public int sourceClientId { get; set; }
    }

    public class OHViewPatientPublicContractModel
    {
        public string kw { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public List<ORAnesthProgressExtContract> listData { get; set; }
        public List<int> listRoom { get; set; }
        public int LastPage { get; set; }
        public string HtmlPaging { get; set; }
        public string siteId { get; set; }
        public int sourceClientId { get; set; }


    }
}
