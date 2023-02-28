using Admin.ClientValidation.Adapters;
using System.Web.Mvc;
using VG.ValidAttribute.Validation;

namespace Admin.ClientValidation
{
    public class DataAnnotationsModelValidatorProviderExtensions
    {
        public static void RegisterValidationExtensions()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(PhoneNumberADRAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MobileADRAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAddressADRAttribute), typeof(EmailAddressADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(TelephoneADRAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(FaxADRAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MaxLengthADRAttribute), typeof(StringLengthAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringLengthADRAttribute), typeof(StringLengthAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DateADRAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(StringADRAttribute), typeof(DataTypeAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RangeNumberADRAttribute), typeof(RangeNumberADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinNumberADRAttribute), typeof(MinNumberADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MaxNumberADRAttribute), typeof(MaxNumberADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(ValueComparisonADRAttribute), typeof(ValueComparisonADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredIfADRAttribute), typeof(RequiredIfADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(AMultipleOfTenADRAttribute), typeof(AMultipleOfTenADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(MinLengthADRAttribute), typeof(MinLengthADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(NoMarkADRAttribute), typeof(NoMarkADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EqualLengthADRAttribute), typeof(EqualLengthADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(PasswordADRAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(PasswordFullADRAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(TaxCodeADRAttribute), typeof(RegularExpressionAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DenySomeCharacterADRAttribute), typeof(DenySomeCharacterADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DateCompareADRAttribute), typeof(DateCompareADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(DateInFutureADRAttribute), typeof(DateInFutureADRAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(IntNumberADRAttribute), typeof(IntNumberADRAttributeAdapter));
        }
    }
}