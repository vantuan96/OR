using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EqualLengthADRAttribute : ValidationAttribute
    {
        public int _lengthValue { get; set; }

        public EqualLengthADRAttribute(int lengthValue, string errorMessage = "")
        {
            _lengthValue = lengthValue;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                this.ErrorMessage = errorMessage;
            }
        }

        public override bool IsValid(object value)
        {
            return value == null || (value is string && (((string)value).Length == 0 || ((string)value).Length == _lengthValue));
        }

        public override string FormatErrorMessage(string name)
        {
            string min = String.Format(CultureInfo.InvariantCulture, "{0:#,#}", _lengthValue);

            return string.Format(this.ErrorMessageString, name, min);
        }

    }
}
