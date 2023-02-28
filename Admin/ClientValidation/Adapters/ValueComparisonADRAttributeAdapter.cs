using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class ValueComparisonADRAttributeAdapter : DataAnnotationsModelValidator<ValueComparisonADRAttribute>
    {
        public ValueComparisonADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, ValueComparisonADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new ValueComparisonADRRule(ErrorMessage, Attribute.OtherPropertyName, Attribute.Comparison) };
        }
    }
}