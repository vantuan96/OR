using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class DateCompareADRAttributeAdapter : DataAnnotationsModelValidator<DateCompareADRAttribute>
    {
        public DateCompareADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, DateCompareADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new DateCompareADRRule(ErrorMessage, Attribute._propertyNameToCompare) };
        }
    }
}