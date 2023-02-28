using Admin.Helper;
using Admin.Resource;
using Contract.Question;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Admin.Models.Question
{
    public class CreateUpdateQuestionModel
    {
        public int QuestionGroupId { get; set; }

        public int QuestionId { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Question_Name", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
           ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
           ErrorMessageResourceType = typeof(LayoutResource))]
        public string NameVN { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Question_NameEN", ResourceType = typeof(LayoutResource))]
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

       
        [Display(Name = "Question_AnswerName", ResourceType = typeof(LayoutResource))]
        public List<int> SubmittedAnswers { get; set; }

        public List<SelectListItem> ListAllAnswer { get; set; }
    }
}