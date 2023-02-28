using System.ComponentModel.DataAnnotations;
using Admin.Resource;

namespace Admin.Models.User
{
    public class CreateUpdateRoleModel
    {
        public int RoleId { get; set; }
        
        [Required]
        [Display(Name = "UserMngt_ListRole_Label_RoleName", ResourceType = typeof(LayoutResource))]
        public string RoleName { get; set; }
        
        [Required]
        [Display(Name = "UserMngt_ListRole_Label_Sort", ResourceType = typeof(LayoutResource))]
        public int Sort { get; set; }
    }
};