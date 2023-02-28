using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class AMultipleOfTenADRAttributeAdapter: DataAnnotationsModelValidator<AMultipleOfTenADRAttribute>
    {
        public AMultipleOfTenADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, AMultipleOfTenADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new AMultipleOfTenADRRule(ErrorMessage) };
        }
    }
}