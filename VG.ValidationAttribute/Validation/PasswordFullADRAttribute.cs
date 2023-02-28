using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    public class PasswordFullADRAttribute : RegularExpressionAttribute
    {
        public PasswordFullADRAttribute(string errorMessage = "")
            : base(@"^((?=.*\d)(?=.*[a-zA-Z])(?!.*\s)(?=.*[\W]).{12,50})?$") // Không có viết hoa
            //: base(@"^((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s)(?=.*[\W]).{6,64})?$") // Có chữ viết hoa
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
