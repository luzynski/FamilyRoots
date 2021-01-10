using System;
using System.ComponentModel.DataAnnotations;

namespace FamilyRoots.Data.Validation
{
    public class GuidNotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return value is Guid guid && Guid.Empty.Equals(guid)
                ? new ValidationResult("Id cannot be empty uuid.")
                : ValidationResult.Success;
        }
    }
}