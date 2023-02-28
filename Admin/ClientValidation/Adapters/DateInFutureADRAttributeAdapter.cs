using Admin.ClientValidation.Rules;
using System.Collections.Generic;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation.Adapters
{
    public class DateInFutureADRAttributeAdapter : DataAnnotationsModelValidator<DateInFutureADRAttribute>
    {
        public DateInFutureADRAttributeAdapter(ModelMetadata metadata, ControllerContext context, DateInFutureADRAttribute attribute)
            : base(metadata, context, attribute)
        {
        }


        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new DateInFutureADRRule(ErrorMessage) };
        }
    }
}