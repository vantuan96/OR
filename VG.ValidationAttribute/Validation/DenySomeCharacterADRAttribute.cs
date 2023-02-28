using System;
using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DenySomeCharacterADRAttribute : ValidationAttribute
    {
        public string StrDenied { get; set; }

        public DenySomeCharacterADRAttribute(string strDenied = "")
        {
            this.StrDenied = strDenied;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string strValue = value.ToString();

                foreach (var c in this.StrDenied)
                {
                    if (strValue.Contains(c.ToString()))
                        return false;
                }
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessageString, name, this.StrDenied);
        }
    }
}