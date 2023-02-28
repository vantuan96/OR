using Admin.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Admin.Models.Master
{
    public class AssignOwnerSystemModel
    {

        public List<SelectListItem> listUsers { get; set; }
        [Display(Name = "Master_SystemCheckList_OwnerName", ResourceType = typeof(LayoutResource))]
        [Required]
        public List<int> UIds { get; set; }
        public int SystemId { get; set; }
        [Display(Name = "Master_SystemCheckList_SystemName", ResourceType = typeof(LayoutResource))]
        [Required]
        public string SystemName { get; set; }
        public int TypeRule { get; set; }
    }
}
