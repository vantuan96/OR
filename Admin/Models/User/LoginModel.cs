using System.ComponentModel.DataAnnotations;
using VG.ValidAttribute.Validation;
using Admin.Resource;
using Admin.Helper;

namespace Admin.Models.User
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Login_Username", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string Username { get; set; }

        //[Required]
        //[EmailAddressADR]
        //[Display(Name = "Login_LabelEmail", ResourceType = typeof(LayoutResource))]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Login_LabelPassword", ResourceType =  typeof(LayoutResource))]
        public string Password { get; set; }

        [Display(Name = "Login_LabelCapcha", ResourceType =  typeof(LayoutResource))]
        public string Captcha { get; set; }
    }
}