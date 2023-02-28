using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class IntNumberADRAttributeAdapter: DataAnnotationsModelValidator<IntNumberADRAttribute>
    {
        public IntNumberADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, IntNumberADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new IntNumberADRRule(ErrorMessage) };
        }
    }
}