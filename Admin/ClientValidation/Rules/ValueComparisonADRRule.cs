using System.Web.Mvc;
using VG.ValidAttribute.Enums;

namespace Admin.ClientValidation.Rules
{
    public class ValueComparisonADRRule : ModelClientValidationRule
    {
        public ValueComparisonADRRule(string errorMessage, string otherPropertyName, ValueComparison comparison)
        {
            ErrorMessage = errorMessage;
            ValidationType = "valuecomparisonadr";
            ValidationParameters["otherpropertyname"] = otherPropertyName;
            ValidationParameters["comparison"] = comparison;
        }
    }
}