using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.ClientValidation.Rules
{
    public class RangeNumberADRRule : ModelClientValidationRule
    {
        public RangeNumberADRRule(string errorMessage, int min, int max)
        {
            ErrorMessage = errorMessage;
            ValidationType = "rangenumberadr";
            ValidationParameters["min"] = min;
            ValidationParameters["max"] = max;
        }
    }
}