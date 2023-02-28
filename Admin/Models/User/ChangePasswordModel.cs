using System.ComponentModel.DataAnnotations;

namespace Admin.Models.User
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "{0} phải từ {2} đến {1} ký tự")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và {0} không trùng nhau.")]
        public string ConfirmPassword { get; set; }
    }
}