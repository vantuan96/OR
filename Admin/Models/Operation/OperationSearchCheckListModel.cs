using Admin.Resource;
using Contract.OperationCheckList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Admin.Models.Operation
{
    public class OperationSearchCheckListModel
    {
        [Display(Name = "AdminTools_Template_KeyWord", ResourceType = typeof(LayoutResource))]
        [Required]
        public string kw { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public List<OperationCheckListContract> listData { get; set; }
        public List<SelectListItem> listStates { get; set; }
        public List<SelectListItem> listCheckListTypes { get; set; }
        public List<SelectListItem> listSystems { get; set; }


        public int state { get; set; }
        public int systemId { get; set; }
        public int checkListTypeId { get; set; }
    }
}
