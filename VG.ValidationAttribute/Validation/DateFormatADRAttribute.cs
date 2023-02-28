using System;
using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DateFormatADRAttribute : DisplayFormatAttribute
    {
        public DateFormatADRAttribute(string formatType = "dd/MM/yyyy")
        {
            ApplyFormatInEditMode = true;
            DataFormatString = "{0:" + formatType + "}";
        }
    }
}
