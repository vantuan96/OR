using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    public class TaxCodeADRAttribute : RegularExpressionAttribute
    {
        public TaxCodeADRAttribute(string errorMessage = "")
            : base(@"^[0-9]+(\-[0-9]+)?$")
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
