using System.Web.Mvc;
using VG.ValidAttribute.Enums;

namespace Admin.ClientValidation.Rules
{
    public class RequiredIfADRRule : ModelClientValidationRule
    {
        public RequiredIfADRRule(string errorMessage, string otherPropertyName, ValueComparison comparison, object value)
        {
            ErrorMessage = errorMessage;
            ValidationType = "requiredifadr";
            ValidationParameters["otherpropertyname"] = otherPropertyName;
            ValidationParameters["comparison"] = comparison;
            ValidationParameters["value"] = value;
        }
    }
}