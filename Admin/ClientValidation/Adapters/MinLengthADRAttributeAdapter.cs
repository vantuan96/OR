using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class MinLengthADRAttributeAdapter : DataAnnotationsModelValidator<MinLengthADRAttribute>
    {
        private int minLength;

        public MinLengthADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, MinLengthADRAttribute attribute)
            : base(metadata, context, attribute)
        {
            minLength = attribute._minValue;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "minlengthadr"
            };
            rule.ValidationParameters.Add("min", minLength);
            return new[] { rule };
        }
    }
}