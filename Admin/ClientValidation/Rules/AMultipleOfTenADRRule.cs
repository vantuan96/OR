using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.ClientValidation.Rules
{
    public class AMultipleOfTenADRRule : ModelClientValidationRule
    {
        public AMultipleOfTenADRRule(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ValidationType = "amultipleoftenadr";
        }
    }
}