using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;
using Admin.Helper;
using Admin.Resource;
using Contract.User;

namespace Admin.Models.User
{
    public class CreateUpdateUserModel
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "UserMngt_ListUser_Label_Username", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: @"^(?=[A-Za-z0-9])(?!.*[._\[\]-]{2})[A-Za-z0-9._\[\]-]{3,15}$",
            ErrorMessageResourceName = "Validate_User_InvalidFormat",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string Username { get; set; }
                
        [EmailAddressADR]
        [StringLength(100)]
        [Display(Name = "UserMngt_ListUser_Label_EmailAddress", ResourceType = typeof(LayoutResource))]
        public string Email { get; set; }

        [PhoneNumberADR]
        [StringLength(100)]
        [Display(Name = "UserMngt_ListUser_Label_PhoneNumber", ResourceType = typeof(LayoutResource))]
        public string PhoneNumber { get; set; }
                
        [StringLength(100)]
        [Display(Name = "UserMngt_ListUser_Label_UserFullName", ResourceType = typeof(LayoutResource))]
        [RegularExpression(pattern: AdminGlobal.SpacialCharReg,
            ErrorMessageResourceName = "CMS_Layout_Category_NotHtmlChar",
            ErrorMessageResourceType = typeof(LayoutResource))]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "UserMngt_ListUser_Label_AccountRole", ResourceType = typeof(LayoutResource))]
        public List<int> RoleId { get; set; }
        
        [Display(Name = "UserMngt_ListUser_Label_Microsite", ResourceType = typeof(LayoutResource))]
        public List<int> MicroSite { get; set; }
        
        [Display(Name = "Shared_Label_PnL", ResourceType = typeof(LayoutResource))]
        public List<int> PnLs { get; set; }
        
        [Display(Name = "Shared_Label_Site", ResourceType = typeof(LayoutResource))]
        public List<int> Sites { get; set; }

        [Required]
        [Display(Name = "Shared_Label_Location", ResourceType = typeof(LayoutResource))]
        public string Location { get; set; }

        [Display(Name = "UserMngt_ListUser_Label_DeptId", ResourceType = typeof(LayoutResource))]
        public int? DeptId { get; set; }

        [Display(Name = "UserMngt_ListUser_Label_IsADAccount", ResourceType = typeof(LayoutResource))]
        public bool IsADAccount { get; set; }

        public List<AdminRoleContract> ListRoles { get; set; }

        public List<SelectListItem> ListMicroSites { get; set; }

        public List<SelectListItem> ListDepartments { get; set; }

        public List<SelectListItem> ListPnLs { get; set; }

        public List<SelectListItem> ListSites { get; set; }

        public List<LocationTreeViewModel> ListLocations { get; set; }        

        public List<SelectListItem> ListUsers { get; set; }

        [Display(Name = "UserMngt_ListUser_Label_LineManager", ResourceType = typeof(LayoutResource))]
        public int LineManagerUser { get; set;}

    }
}