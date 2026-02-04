using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Common.Validation
{
    public class ValidEnumAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public ValidEnumAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (!Enum.IsDefined(_enumType, value))
            {
                return new ValidationResult(
                    $"Invalid value for {_enumType.Name}");
            }

            return ValidationResult.Success;
        }
    }
}
