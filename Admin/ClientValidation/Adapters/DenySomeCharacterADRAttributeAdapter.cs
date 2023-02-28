using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class DenySomeCharacterADRAttributeAdapter: DataAnnotationsModelValidator<DenySomeCharacterADRAttribute>
    {
        public DenySomeCharacterADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, DenySomeCharacterADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new DenySomeCharacterADRRule(ErrorMessage, Attribute.StrDenied) };
        }
    }
}