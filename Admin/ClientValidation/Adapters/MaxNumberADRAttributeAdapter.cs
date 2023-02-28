using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class MaxNumberADRAttributeAdapter: DataAnnotationsModelValidator<MaxNumberADRAttribute>
    {
        public MaxNumberADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, MaxNumberADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new MaxNumberADRRule(ErrorMessage, Attribute.Max) };
        }
    }
}