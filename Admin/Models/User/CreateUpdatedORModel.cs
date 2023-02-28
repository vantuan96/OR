using Admin.Helper;
using Admin.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Admin.Models.User
{
    public class CreateUpdatedORModel
    {
        public int UserId { get; set; }
        public List<SelectListItem> ListPositions { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "UserMngt_ListUser_Label_Username", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        [AllowHtml]
        public string Username { get; set; }
        //[Required]
        public List<int> PositionId { get; set; }

        public string Email { get; set; }
        [AllowHtml]
        public string Phone { get; set; }
        public List<SelectListItem> ListStates { get; set; }
        public int StateId { get; set; }

    }
}
