using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Admin.Resource;
using Contract.Microsite;

namespace Admin.Models.MicrositeMngt
{
    public class MicrositeDetailModel
    {
        [Display(Name = "MicrositeMngt_ListMicrosite_Label_ReferenceCode", ResourceType = typeof(LayoutResource))]
        [StringLength(50)]
        [RegularExpression(pattern: Helper.AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string ReferenceCode { get; set; }

        [Required]
        [Display(Name = "MicrositeMngt_ListMicrosite_Label_MstId", ResourceType = typeof(LayoutResource))]
        public int MstId { get; set; }
        

        public MicrositeDetailContract Details { get; set; }

        [Required]
        public int MsId { get; set; }
    }
}