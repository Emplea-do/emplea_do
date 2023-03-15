using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfTrueAttribute : RequiredAttribute
    {
        private string PropertyName { get; }

        public RequiredIfTrueAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();

            bool.TryParse(type.GetProperty(PropertyName).GetValue(instance)?.ToString(), out bool propertyValue);

            if (propertyValue && string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}