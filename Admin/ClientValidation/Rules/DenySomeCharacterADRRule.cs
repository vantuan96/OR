using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.ClientValidation.Rules
{
    public class DenySomeCharacterADRRule : ModelClientValidationRule
    {
        public DenySomeCharacterADRRule(string errorMessage, string strDenied)
        {
            ErrorMessage = errorMessage;
            ValidationType = "denysomecharacteradr";
            ValidationParameters["strdenied"] = strDenied;
        }
    }
}