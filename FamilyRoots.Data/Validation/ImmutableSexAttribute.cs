using System.ComponentModel.DataAnnotations;
using FamilyRoots.Data.Requests;

namespace FamilyRoots.Data.Validation
{
    public class ImmutableSexAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is UpsertPersonRequest request)
            {
                if (!request.Id.HasValue && !request.Sex.HasValue)
                {
                    return new ValidationResult("Person sex has to be set on creation.");
                }
                if (request.Id.HasValue && request.Sex.HasValue)
                {
                    return new ValidationResult("Cannot change person sex.");
                }
            }
            return ValidationResult.Success;
        }
    }
}