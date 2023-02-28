using System.Web.Mvc;

namespace Admin.ClientValidation.Rules
{
    public class DateInFutureADRRule : ModelClientValidationRule
    {
        public DateInFutureADRRule(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ValidationType = "dateinfutureadr";
        }
    }
}