using Admin.Helper;
using Admin.Resource;
using Contract.Question;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Contract.MasterData;

namespace Admin.Models.User
{
    public class CreateUpdateLocationModel
    {
        public int LocationId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Location_NameVN", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string NameVN { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Location_NameEN", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string NameEN { get; set; }

        [StringLength(200)]
        [Display(Name = "Location_Slogan", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string SloganVN { get; set; }

        [StringLength(200)]
        [Display(Name = "Location_SloganEN", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string SloganEN { get; set; }

        [StringLength(200)]
        [Display(Name = "Location_Logo", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string LogoName { get; set; }

        [StringLength(50)]
        [Display(Name = "Location_BgName", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string BackgroundName { get; set; }

        [StringLength(10)]
        [Display(Name = "Location_ColorCode", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string ColorCode { get; set; }

        public int LevelNo { get; set; }

        public int ParentId { get; set; }

        [Display(Name = "Location_Parent_Name", ResourceType = typeof(LayoutResource))]
        public string ParentName { get; set; }

        [Display(Name = "Device_QuestionGroupName", ResourceType = typeof(LayoutResource))]
        public int QuestionGroupId { get; set; }

        public List<SelectListItem> QuestionGroups { get; set; }
        public int DepartmentTypeId { get; set; }
        public List<DepartmentListTypeContract> ListDepartmentType { get; set; }
    }
}