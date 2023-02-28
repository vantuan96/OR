using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MinLengthADRAttribute : ValidationAttribute
    {
        public int _minValue { get; set; }

        public MinLengthADRAttribute(int min = 6, string errorMessage = "")
        {
            _minValue = min;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                this.ErrorMessage = errorMessage;
            }
        }

        public override bool IsValid(object value)
        {
            return value == null || (value is string && (((string)value).Length == 0 || ((string)value).Length >= _minValue));
        }

        public override string FormatErrorMessage(string name)
        {
            string min = String.Format(CultureInfo.InvariantCulture, "{0:#,#}", _minValue);

            return string.Format(this.ErrorMessageString, name, min);
        }

    }
}
