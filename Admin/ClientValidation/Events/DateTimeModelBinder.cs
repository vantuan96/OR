using System;
using System.Globalization;
using System.Web.Mvc;
using Admin.Resource;

namespace Admin.ClientValidation.Events
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        public DateTimeModelBinder() { }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var displayFormat = bindingContext.ModelMetadata.DisplayFormatString;
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (!string.IsNullOrEmpty(displayFormat) && value != null)
            {
                DateTime date;
                displayFormat = displayFormat.Replace("{0:", string.Empty).Replace("}", string.Empty);
                if (DateTime.TryParseExact(value.AttemptedValue, displayFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }
                else
                {
                    bindingContext.ModelState.AddModelError(
                        bindingContext.ModelName,
                        string.Format(MessageResource.Shared_ValidAttrMsg_DateADR, bindingContext.ModelMetadata.DisplayName)
                    );
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}