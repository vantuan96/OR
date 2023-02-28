using Admin.Helper;
using Admin.Models.User;
using Admin.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VG.ValidAttribute.Validation;

namespace Admin.Models.Device
{
    public class CreateUpdateDeviceModel
    {
        public int DeviceId { get; set; }

        [Required]
        [StringLength(2000)]
        [Display(Name = "Device_Label_ImeiNo", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string Imei { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Device_Label_Name", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Shared_Label_Location", ResourceType = typeof(LayoutResource))]        
        public int Location { get; set; }
        
        [Display(Name = "Shared_Label_Activated", ResourceType = typeof(LayoutResource))]
        public bool Status { get; set; }

        public List<LocationTreeViewModel> ListLocations { get; set; }
    }
}