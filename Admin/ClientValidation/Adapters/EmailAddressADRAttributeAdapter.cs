using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class EmailAddressADRAttributeAdapter : DataAnnotationsModelValidator<EmailAddressADRAttribute>
    {
        private string _RegexEmail;

        public EmailAddressADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, EmailAddressADRAttribute attribute)
            : base(metadata, context, attribute)
        {
            _RegexEmail = attribute.RegexEmail;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "emailaddressadr"
            };
            rule.ValidationParameters.Add("email", _RegexEmail);
            return new[] { rule };
        }
    }
}