using System;
using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class DateCompareADRAttribute : ValidationAttribute
    {
        private ValidationType _validationType;
        private DateTime? _fromDate;
        private DateTime _toDate;
        private string _defaultErrorMessage;
        public string _propertyNameToCompare;

        public DateCompareADRAttribute(ValidationType validationType, string message, string compareWith = "", string fromDate = "")  
        {  
            _validationType = validationType;  
            switch (validationType)  
            {  
                case ValidationType.Compare:  
                {  
                    _propertyNameToCompare = compareWith;  
                    _defaultErrorMessage = message;  
                }
                break;  
                case ValidationType.RangeValidation:  
                {  
                    _fromDate = new DateTime(2009,1,1);  
                    _toDate = DateTime.Today;  
                    _defaultErrorMessage = message;  
                }
                break;  
            }  
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            switch (_validationType)
            {
                case ValidationType.Compare:
                    {
                        var baseProperyInfo = validationContext.ObjectType.GetProperty(_propertyNameToCompare);
                        var startDate = (DateTime)baseProperyInfo.GetValue(validationContext.ObjectInstance, null);
                        if (value != null)
                        {
                            DateTime enteredDate = (DateTime)value;
                            if (enteredDate <= startDate)
                            {
                                return new ValidationResult(_defaultErrorMessage);
                            }
                        }
                    }
                    break;
                case ValidationType.RangeValidation:
                    {
                        if (value != null)
                        {
                            DateTime enteredDate = (DateTime)value;
                            if (!(enteredDate >= _fromDate && enteredDate <= _toDate))
                            {
                                return new ValidationResult(_defaultErrorMessage);
                            }
                        }
                    }
                    break;
            }
            return null;
        }

        public override string FormatErrorMessage(string name)
        {
            return _defaultErrorMessage;
        }
    }


    public enum ValidationType
    {
        RangeValidation,
        Compare
    }
}
