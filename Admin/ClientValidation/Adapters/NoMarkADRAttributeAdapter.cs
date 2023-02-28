using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class NoMarkADRAttributeAdapter : DataAnnotationsModelValidator<NoMarkADRAttribute>
    {
        private string _RegexNotMark;

        public NoMarkADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, NoMarkADRAttribute attribute)
            : base(metadata, context, attribute)
        {
            _RegexNotMark = attribute.RegexNotMark;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "notmarkadr"
            };
            rule.ValidationParameters.Add("mark", _RegexNotMark);
            return new[] { rule };
        }
    }
}