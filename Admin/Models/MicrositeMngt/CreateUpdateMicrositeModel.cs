using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Admin.Helper;
using Admin.Resource;

namespace Admin.Models.MicrositeMngt
{
    public class CreateUpdateMicrositeModel
    {
        public int Id { get; set; }

        public int MsId { get; set; }

        [Required]
        [Display(Name = "MicrositeMngt_ListMicrosite_Label_Title", ResourceType = typeof(LayoutResource))]
        [StringLength(512)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string Title { get; set; }

       
        [Display(Name = "MicrositeMngt_ListMicrosite_Label_ShortDescription", ResourceType = typeof(LayoutResource))]
        [StringLength(512)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string ShortDescription { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_Rewrite", ResourceType = typeof(LayoutResource))]
        [StringLength(256)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string Rewrite { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_Description", ResourceType = typeof(LayoutResource))]
        [StringLength(2048)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string Description { get; set; }



        [Display(Name = "MicrositeMngt_ListMicrosite_Label_ImageUrl", ResourceType = typeof(LayoutResource))]
        public string ImageUrl { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_TargetLinkImage", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.UrlParamReg, ErrorMessageResourceName = "Shared_ValidAttrMsg_InvalidLink", ErrorMessageResourceType = typeof(MessageResource))]
        [StringLength(50)]
        public string TargetLinkImage { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_ImageMobileUrl", ResourceType = typeof(LayoutResource))]
        public string ImageMobileUrl { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_BannerUrl", ResourceType = typeof(LayoutResource))]
        public string BannerUrl { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_TargetLinkImageMobile", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.UrlParamReg, ErrorMessageResourceName = "Shared_ValidAttrMsg_InvalidLink", ErrorMessageResourceType = typeof(MessageResource))]
        public string TargetLinkImageMobile { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_LanguageShortName", ResourceType = typeof(LayoutResource))]
        public string LangShortName { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_MetaTitle", ResourceType = typeof(LayoutResource))]
        [StringLength(1024)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string MetaTitle { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_MetaDescription", ResourceType = typeof(LayoutResource))]
        [StringLength(1024)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string MetaDescription { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_MetaKeyword", ResourceType = typeof(LayoutResource))]
        [StringLength(1024)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string MetaKeyword { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_Status", ResourceType = typeof(LayoutResource))]
        public int ApprovalStatus { get; set; }

        [Display(Name = "MicrositeMngt_ListMicrosite_Label_ReferenceCode", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string ReferenceCode { get; set; }

        public List<SelectListItem> ListStatus { get; set; }

      
        [Display(Name = "MicrositeMngt_ListMicrosite_Label_MstId", ResourceType = typeof(LayoutResource))]
        public int MstId { get; set; }
    }
}