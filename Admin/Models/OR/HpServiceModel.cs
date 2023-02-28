using Admin.Helper;
using Admin.Resource;
using Contract.OR;
using Contract.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.Models.OR
{
    public class HpServiceModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [StringLength(100)]
        public string Oh_Code { get; set; }
        public int CleaningTime { get; set; }
        public int PreparationTime { get; set; }
        public int AnesthesiaTime { get; set; }
        public int OtherTime { get; set; }
        public string Description { get; set; }
        public string IdMapping { get; set; }
        public List<string> lstSiteId { get; set; }
        public string siteId { get; set; }        
        public List<SelectListItem> listSites { get; set; }        
    }
}
