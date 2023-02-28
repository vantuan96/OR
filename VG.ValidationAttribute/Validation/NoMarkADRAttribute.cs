using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NoMarkADRAttribute : ValidationAttribute
    {
        private static Regex _regex = new Regex(@"^(([a-zA-Z0-9])+)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        public string RegexNotMark { get; set; }

        public NoMarkADRAttribute(string errorMessage = "")
        {
            RegexNotMark = _regex.ToString();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                this.ErrorMessage = errorMessage;
            }
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;

            return valueAsString != null && _regex.Match(valueAsString).Length > 0;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessageString, name);
        }
    }
}
