using System;
using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IntNumberADRAttribute : ValidationAttribute
    {
        #region Fields

        private readonly ValidationResult failure;
        private readonly ValidationResult success;

        #endregion

        #region Ctor

        public IntNumberADRAttribute(string Message = null) : base()
        {
            if (Message != null)
            {
                ErrorMessage = Message;
            }

            if (string.IsNullOrEmpty(ErrorMessage))
            {
                this.failure = new ValidationResult(String.Empty);
            }
            else
            {
                this.failure = new ValidationResult(ErrorMessage);
            }

            this.success = ValidationResult.Success;
        }

        #endregion

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int _value;

                if (int.TryParse(value.ToString(), out _value) == false)
                {
                    return failure;
                }
            }

            return success;
        }
        
        #endregion
    }
}
