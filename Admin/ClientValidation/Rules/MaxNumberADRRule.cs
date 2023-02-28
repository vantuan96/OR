using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.ClientValidation.Rules
{
    public class MaxNumberADRRule : ModelClientValidationRule
    {
        public MaxNumberADRRule(string errorMessage, int max)
        {
            ErrorMessage = errorMessage;
            ValidationType = "maxnumberadr";
            ValidationParameters["max"] = max;
        }
    }
}