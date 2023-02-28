using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class MinNumberADRAttributeAdapter: DataAnnotationsModelValidator<MinNumberADRAttribute>
    {
        public MinNumberADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, MinNumberADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new MinNumberADRRule(ErrorMessage, Attribute.Min) };
        }
    }
}