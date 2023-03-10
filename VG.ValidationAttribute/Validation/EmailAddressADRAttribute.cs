using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class EmailAddressADRAttribute : ValidationAttribute
    {
        private static Regex _regex;// = new Regex(@"^(([a-zA-Z0-9_\.])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z]{2,4})+)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const string _regexValidation = @"^(([a-zA-Z0-9-_\.])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z]{2,4})+)?$";

        public string RegexEmail { get; set; }
        public string ErrorMessageCustom { get; set; }

        public EmailAddressADRAttribute()
        {
            //RegexEmail = !string.IsNullOrWhiteSpace(RegexEmail) ? RegexEmail : _regexValidation;
            RegexEmail = RegexEmail ?? _regexValidation;
            _regex = new Regex(RegexEmail, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (!string.IsNullOrEmpty(ErrorMessageCustom))
            {
                this.ErrorMessage = ErrorMessageCustom;
            }
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;

            return !string.IsNullOrWhiteSpace(valueAsString) && _regex.Match(valueAsString).Length > 0 && valueAsString.Length <= 250;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessageString, name);
        }
    }
}
