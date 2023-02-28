using System;
using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLengthADRAttribute : StringLengthAttribute
    {
        public MaxLengthADRAttribute(int max = 200) : base(max) {}
    }
}
