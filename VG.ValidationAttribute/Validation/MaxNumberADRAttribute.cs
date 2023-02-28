using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MaxNumberADRAttribute : ValidationAttribute
    {
        #region Fields

        private readonly int _max;
        private readonly ValidationResult failure;
        private readonly ValidationResult success;

        #endregion

        #region Properties

        public int Max
        {
            get { return _max; }
        }

        #endregion

        #region Ctor

        public MaxNumberADRAttribute(int Max, string Message = null)
        {
            this._max = Max;

            if (string.IsNullOrEmpty(Message))
                Message = String.Empty;
            else
                this.ErrorMessage = Message;

            this.failure = new ValidationResult(Message);
            this.success = ValidationResult.Success;
        }

        #endregion

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                int _value = int.Parse(value.ToString());

                if (_value > _max)
                {
                    return failure;
                }
            }

            return success;
        }

        public override string FormatErrorMessage(string name)
        {
            string max = String.Format(CultureInfo.InvariantCulture, "{0:#,0}", _max);
            max = max.Replace(',', '.');

            return string.Format(this.ErrorMessageString, name, max);
        }

        #endregion

    }
}
