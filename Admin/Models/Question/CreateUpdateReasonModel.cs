using Admin.Helper;
using Admin.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.Models.Question
{
    public class CreateUpdateReasonModel
    {
        public int QuestionAnswerMappingId { get; set; }

        public int ReasonId { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Question_ReasonName", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
           ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
           ErrorMessageResourceType = typeof(LayoutResource))]
        /// <summary>
        /// Title chọn các check box sau khi chấm điểm
        /// </summary>
        public string NameVN { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Question_ReasonNameEN", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
           ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
           ErrorMessageResourceType = typeof(LayoutResource))]
        public string NameEN { get; set; }

        [Required]
        [Display(Name = "Shared_SortLabel", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.OnlyNumberReg,
           ErrorMessageResourceName = "Shared_RequiredInputNumber",
           ErrorMessageResourceType = typeof(LayoutResource))]
        public int Sort { get; set; }

        [Display(Name = "Question_ReasonType", ResourceType = typeof(LayoutResource))]
        public int Type { get; set; }

        public List<SelectListItem> ListType { get; set; }
    }
}