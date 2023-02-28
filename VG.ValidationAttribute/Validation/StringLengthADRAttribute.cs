using System;
using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class StringLengthADRAttribute : StringLengthAttribute
    {
        public StringLengthADRAttribute(int min = 6, int max = 200) : base(maximumLength: max)
        {
            this.MinimumLength = min;
        }
    }
}