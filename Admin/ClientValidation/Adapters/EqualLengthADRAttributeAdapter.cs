using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class EqualLengthADRAttributeAdapter : DataAnnotationsModelValidator<EqualLengthADRAttribute>
    {
        private int _lengthValue;

        public EqualLengthADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, EqualLengthADRAttribute attribute)
            : base(metadata, context, attribute)
        {
            _lengthValue = attribute._lengthValue;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "equallengthadr"
            };
            rule.ValidationParameters.Add("equal", _lengthValue);
            return new[] { rule };
        }
    }
}