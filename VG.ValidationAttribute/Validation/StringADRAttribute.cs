using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class StringADRAttribute : DataTypeAttribute
    {
        private static Regex _regex;

        public StringADRAttribute(string regex, DataType type, string errorMessage = "")
            : base(type)
        {
            ErrorMessage = !string.IsNullOrEmpty(errorMessage) ? errorMessage : "Không đúng định dạng";

            _regex = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
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
    }
}
