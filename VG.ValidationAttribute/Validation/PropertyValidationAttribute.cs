using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace VG.ValidAttribute.Validation
{
    /// <summary> 
    /// Base class for performing validation between two properties. 
    /// </summary> 
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public abstract class PropertyValidationAttribute : ValidationAttribute
    {
        #region Fields

        private readonly string otherPropertyName;
        private string otherPropertyDisplayName;
        private object value;

        #endregion

        #region Ctor

        /// <summary> 
        /// Initializes new instance of the <see cref="PropertyValidationAttribute"/> class. 
        /// </summary> 
        /// <param name="propertyName">The name of the other property.</param> 
        /// <exception cref="ArgumentNullException">If <paramref name="propertyName"/> is <c>null</c>, empty or whitespace.</exception> 
        protected PropertyValidationAttribute(string otherPropertyName, Type type = null)
            : base()
        {
            if (String.IsNullOrWhiteSpace(otherPropertyName))
                throw new ArgumentNullException("otherPropertyName");

            this.otherPropertyName = otherPropertyName;

            if (type != null)
            {
                PropertyInfo property = type.GetProperty(OtherPropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                var displayName = property.GetCustomAttributes()
                    .Where(p => p.GetType() == typeof(DisplayAttribute))
                    .Select(p => ((DisplayAttribute)p).Name)
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(displayName))
                {
                    this.otherPropertyDisplayName = otherPropertyName;
                }
                else
                {
                    this.otherPropertyDisplayName = displayName;
                }
            }
        }

        #endregion

        #region Properties

        /// <summary> 
        /// Gets whether or not <see cref="ValidationContext"/> is required. 
        /// </summary> 
        public override bool RequiresValidationContext
        {
            get { return true; }
        }

        /// <summary> 
        /// Gets the name of the other property. 
        /// </summary> 
        public string OtherPropertyName
        {
            get { return this.otherPropertyName; }
        }

        public string OtherPropertyDisplayName
        {
            get
            {
                return this.otherPropertyDisplayName;
            }
        }

        #endregion

        #region Methods

        /// <summary> 
        /// Gets the value of the other property. 
        /// </summary> 
        /// <param name="validationContext">The context of the validation.</param> 
        /// <returns>A value of the other property.</returns> 
        /// <exception cref="InvalidOperationException">If object type of the validation context does not contain <see cref="PropertyName"/> property.</exception> 
        /// <exception cref="NotSupportedException">If property requires indexer parameters.</exception> 
        protected object GetValue(ValidationContext validationContext)
        {
            Type type = validationContext.ObjectType;
            PropertyInfo property = type.GetProperty(OtherPropertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

            if (property == null)
                throw new InvalidOperationException(String.Format("Type {0} does not contains public instance property {1}.", type.FullName, OtherPropertyName));

            if (IsIndexerProperty(property))
                throw new NotSupportedException("Property with indexer parameters is not supported.");

            value = property.GetValue(validationContext.ObjectInstance);

            return value;
        }

        private bool IsIndexerProperty(PropertyInfo property)
        {
            var parameters = property.GetIndexParameters();

            return (parameters != null && parameters.Length > 0);
        }

        #endregion
    }
}
