using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.ClientValidation.Rules
{
    public class MinNumberADRRule : ModelClientValidationRule
    {
        public MinNumberADRRule(string errorMessage, int min)
        {
            ErrorMessage = errorMessage;
            ValidationType = "minnumberadr";
            ValidationParameters["min"] = min;
        }
    }
}