using Admin.Resource;
using Contract.OR.SyncData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Admin.Models.OR
{
    public class PatientORSyncModel
    {
        [Display(Name = "AdminTools_Template_KeyWord", ResourceType = typeof(LayoutResource))]
        [Required]
        public string kw { get; set; }
        public BenhNhanOR Data { get; set; }
        public int sourceClientId { get; set; }
        public string siteId { get; set; }
        public string siteName { get; set; }

        public string LastSiteName { get; set; }
        public string LastVistDate { get; set; }
        public List<SelectListItem> listSites { get; set; }
    }
}
