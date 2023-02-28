using Admin.Resource;
using Contract.MasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Admin.Models.CheckList
{
    public class ListItemModel
    {
        [Display(Name = "AdminTools_Template_KeyWord", ResourceType = typeof(LayoutResource))]
        [Required]
        public string kw { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public List<ItemContract> listData { get; set; }
        public List<SelectListItem> listStates { get; set; }

        
        public int state { get; set; }
    }
}
