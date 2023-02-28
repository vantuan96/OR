using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace VG.ValidAttribute.Validation
{
    //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    sealed public class RangeNumberADRAttribute : ValidationAttribute
    {
        #region Fields

        private readonly int _min;
        private readonly int _max;
        private readonly ValidationResult failure;
        private readonly ValidationResult success;

        #endregion

        #region Properties

        public int Min
        {
            get
            {
                return _min;
            }
        }

        public int Max
        {
            get
            {
                return _max;
            }
        }

        #endregion

        #region Ctor

        public RangeNumberADRAttribute() : base()
        {
            this._min = int.MinValue;
            this._max = int.MaxValue;
            this.failure = new ValidationResult(String.Empty);
            this.success = ValidationResult.Success;
        }

        public RangeNumberADRAttribute(int Min, int Max, string Message = null) : base()
        {
            this._min = Min;
            this._max = Max;

            if (string.IsNullOrEmpty(Message))
                Message = String.Empty;

            this.failure = new ValidationResult(Message);
            this.success = ValidationResult.Success;
        }

        #endregion

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                decimal _value = decimal.Parse(value.ToString());

                if (_value < _min || _value > _max)
                {
                    return failure;
                }
            }

            return success;
        }

        public override string FormatErrorMessage(string name)
        {
            string min = String.Format(CultureInfo.InvariantCulture, "{0:#,0}", _min);
            string max = String.Format(CultureInfo.InvariantCulture, "{0:#,0}", _max);
            min = min.Replace(',', '.');
            max = max.Replace(',', '.');

            return string.Format(this.ErrorMessageString, name, min, max);
        }

        #endregion

    }
}
