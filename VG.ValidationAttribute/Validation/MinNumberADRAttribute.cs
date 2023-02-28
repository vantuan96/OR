using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MinNumberADRAttribute : ValidationAttribute
    {
        #region Fields

        private readonly int _min;
        private readonly ValidationResult failure;
        private readonly ValidationResult success;

        #endregion

        #region Properties

        public int Min
        {
            get { return _min; }
        }

        #endregion

        #region Ctor

        public MinNumberADRAttribute(int Min, string Message = null)
        {
            this._min = Min;

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

                if (_value < _min)
                {
                    return failure;
                }
            }

            return success;
        }

        public override string FormatErrorMessage(string name)
        {
            string min = String.Format(CultureInfo.InvariantCulture, "{0:#,0}", _min);
            min = min.Replace(',', '.');

            return string.Format(this.ErrorMessageString, name, min);
        }

        #endregion

    }
}
