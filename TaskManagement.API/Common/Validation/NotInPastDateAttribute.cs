using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Common.Validation
{
    public class NotInPastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is DateTime dueDate)
            {
                var utcDueDate = dueDate.Kind == DateTimeKind.Utc
                    ? dueDate
                    : dueDate.ToUniversalTime();

                if (utcDueDate <= DateTime.UtcNow)
                {
                    return new ValidationResult(
                        $"{validationContext.MemberName} cannot be in the past");
                }
            }

            return ValidationResult.Success;
        }
    }
}
