using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    public class MobileADRAttribute : RegularExpressionAttribute
    {
        public MobileADRAttribute(string errorMessage = "")
            //: base(@"^([(]?\+?\d{2,4}[)]?[\s|\-|\.]?\d{2,3}[\s|\-|\.]?\d{3}[\s|\-|\.]?\d{3})?$")
            //: base(@"^(([(](\+?(?=.*[1-9])\d{2,4})[)]|(\+?(?=.*[1-9])\d{2,4}))[\s|\-|\.]?\d{2,3}[\s|\-|\.]?\d{3}[\s|\-|\.]?\d{3})?$")
            : base(@"^((?:(?!\n)\s*)?([(](\+?(?=.*[1-9])\d{2,4})[)]|(\+?(?=.*[1-9])\d{2,4}))[\s|\-|\.]?\d{2,3}[\s|\-|\.]?\d{3}[\s|\-|\.]?\d{3}(?:(?!\n)\s*)?)?$")
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
