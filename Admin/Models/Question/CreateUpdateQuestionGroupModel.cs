using Admin.Helper;
using Admin.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models.Question
{
    public class CreateUpdateQuestionGroupModel
    {
        public int QuestionGroupId { get; set; }

        [Required]
        [Display(Name = "Device_LayoutTypeName", ResourceType = typeof(LayoutResource))]
        public int? LayoutTypeId { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Device_QuestionGroupName", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
           ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
           ErrorMessageResourceType = typeof(LayoutResource))]
        public string NameVN { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Device_QuestionGroupNameEN", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
           ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
           ErrorMessageResourceType = typeof(LayoutResource))]
        public string NameEN { get; set; }

        [Display(Name = "Shared_Label_Status", ResourceType = typeof(LayoutResource))]        
        public bool Status { get; set; }

        public List<SelectListItem> LayoutTypes { get; set; }

        public List<SelectListItem> Statuses { get; set; }
    }
}