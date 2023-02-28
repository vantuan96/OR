using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Admin.ClientValidation.Adapters
{
    public class DataTypeAttributeAdapter : DataAnnotationsModelValidator<DataTypeAttribute>
    {
        public DataTypeAttributeAdapter(ModelMetadata metadata, ControllerContext context, DataTypeAttribute attribute)
            : base(metadata, context, attribute) { }

        public override System.Collections.Generic.IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = this.ErrorMessage;

            switch (this.Attribute.DataType)
            {
                case DataType.EmailAddress:
                    rule.ValidationType = "email";
                    break;
                case DataType.Url:
                    rule.ValidationType = "url";
                    break;
                default:
                    return new ModelClientValidationRule[0];
            }
            return new[] { rule };
        }
    }
}