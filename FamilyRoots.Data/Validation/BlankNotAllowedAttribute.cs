using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FamilyRoots.Data.Validation
{
    public class BlankNotAllowedAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return new ValidationResult("Value cannot be null, empty or white spaces only.");
                }
            }
            if (value is string[] array)
            {
                if (array.Any(string.IsNullOrWhiteSpace))
                {
                    return new ValidationResult("Array cannot have values that are null, empty or white spaces only.");
                }
            }
            return ValidationResult.Success;
        }
    }
}