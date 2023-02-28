using Admin.Helper;
using Admin.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Admin.Models.Question
{
    public class UpdateAnswerMappingModel
    {
        public int QuestionAnswerMappingId { get; set; }

        [Display(Name = "Question_AnswerName", ResourceType = typeof(LayoutResource))]
        public string AnswerTextVN { get; set; }

        [Display(Name = "Question_AnswerNameEN", ResourceType = typeof(LayoutResource))]
        public string AnswerTextEN { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "AnswerMapping_FeedbackText", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
           ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
           ErrorMessageResourceType = typeof(LayoutResource))]
        /// <summary>
        /// Title chọn các check box sau khi chấm điểm
        /// </summary>
        public string FeedbackTitleVN { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "AnswerMapping_FeedbackTextEN", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
           ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
           ErrorMessageResourceType = typeof(LayoutResource))]
        public string FeedbackTitleEN { get; set; }

        public int UserId { get; set; }
    }
}