using Admin.Resource;
using Contract.OR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VG.Common;

namespace Admin.Models.OR
{
    public class SearchORRoomModel
    {
        [Display(Name = "AdminTools_Template_KeyWord", ResourceType = typeof(LayoutResource))]
        [Required]
        public string kw { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public List<ORRoomContract> listData { get; set; }
        public List<SelectListItem> listRoomTypes { get; set; }
        public List<SelectListItem> listStates { get; set; }
        public List<SelectListItem> listSites { get; set; }

        public string siteId { get; set; }
        public int sourceClientId { get; set; }
        public int State { get; set; }
        public int IsAll { get; set; }
    }
}
