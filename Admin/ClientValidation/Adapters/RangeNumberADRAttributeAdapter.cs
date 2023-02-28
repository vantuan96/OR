using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class RangeNumberADRAttributeAdapter : DataAnnotationsModelValidator<RangeNumberADRAttribute>
    {
        public RangeNumberADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, RangeNumberADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new RangeNumberADRRule(ErrorMessage, Attribute.Min, Attribute.Max) };
        }
    }
}