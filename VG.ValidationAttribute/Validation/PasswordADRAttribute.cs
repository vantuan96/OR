using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    public class PasswordADRAttribute : RegularExpressionAttribute
    {
        public PasswordADRAttribute(string errorMessage = "")
            : base(@"^((?=.*\d)(?=.*[a-zA-Z])(?=.*[\W]).{12,50})?$") // Không có viết hoa
            //: base(@"^((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).{6,64})?$") // Có chữ viết hoa
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorMessage = errorMessage;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessageString, name);
        }
    }
}
