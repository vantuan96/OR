using Admin.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Admin.Models.CheckList
{
    public class InsertUpdateItemModel
    {
        public int ClItemId { get; set; }
        [Display(Name = "Master_ItemCheckList_ItemName", ResourceType = typeof(LayoutResource))]
        [Required]
        public string ItemName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Master_ItemCheckList_State", ResourceType = typeof(LayoutResource))]
        [Required]
        public int State { get; set; }
        public int Sort { get; set; }
        public List<SelectListItem> listStates { get; set; }
    }
}
