using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class RequiredIfADRAttributeAdapter : DataAnnotationsModelValidator<RequiredIfADRAttribute>
    {
        public RequiredIfADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredIfADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new RequiredIfADRRule(ErrorMessage, Attribute.OtherPropertyName, Attribute.Comparison, Attribute.Value) };
        }
    }
}