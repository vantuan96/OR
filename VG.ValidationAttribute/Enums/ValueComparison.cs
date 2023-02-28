using System.ComponentModel.DataAnnotations;

namespace VG.ValidAttribute.Enums
{
    public enum ValueComparison : int
    {
        /// <summary> 
        /// Values are compared to be equal. 
        /// </summary> 
        [Display(Name = "bằng")]
        IsEqual = 0,

        /// <summary> 
        /// Values are compared to be inequal. 
        /// </summary> 
        [Display(Name = "khác")]
        IsNotEqual = 1,

        /// <summary> 
        /// Value is compared to be greater than other value. 
        /// </summary> 
        [Display(Name = "lớn hơn")]
        IsGreaterThan = 2,

        /// <summary> 
        /// Value is compared to be greater than or equal to other value. 
        /// </summary> 
        [Display(Name = "lớn hơn hoặc bằng")]
        IsGreaterThanOrEqual = 3,

        /// <summary> 
        /// Value is compared to be less than other value. 
        /// </summary> 
        [Display(Name = "nhỏ hơn")]
        IsLessThan = 4,

        /// <summary> 
        /// Value is compared to be less than or equal to other value. 
        /// </summary> 
        [Display(Name = "nhỏ hơn hoặc bằng")]
        IsLessThanOrEqual = 5
    }
}
