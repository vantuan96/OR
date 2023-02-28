using System.Web.Mvc;

namespace Admin.ClientValidation.Rules
{
    public class DateCompareADRRule : ModelClientValidationRule
    {
        public DateCompareADRRule(string errorMessage, string otherPropertyName)
        {
            ErrorMessage = errorMessage;
            ValidationType = "datecompareadr";
            ValidationParameters["otherpropertyname"] = otherPropertyName;
        }
    }
}