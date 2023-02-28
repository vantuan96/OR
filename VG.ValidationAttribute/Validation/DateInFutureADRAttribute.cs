using System;
using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class DateInFutureADRAttribute : ValidationAttribute
    {
        private readonly ValidationResult failure;
        private readonly ValidationResult success;

        public DateInFutureADRAttribute()
        {
            this.failure = new ValidationResult(string.Empty);
            this.success = ValidationResult.Success;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if ((DateTime)value <= DateTime.Now)
                {
                    return failure;
                }
            }

            return success;
        }
    }
}
