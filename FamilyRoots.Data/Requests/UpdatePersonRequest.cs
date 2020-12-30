using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FamilyRoots.Data.Requests
{
    public class UpdatePersonRequest : IValidatableRequest
    {
        public Guid Id { get; set; }

        public string Surname { get; set; }

        public string[] Names { get; set; }

        public Sex? Sex { get; set; }

        public DateTime? BirthDate { get; set; }

        public string BirthPlace { get; set; }

        public DateTime? DeathDate { get; set; }

        public string BurialPlace { get; set; }

        public bool IsValid(out ImmutableArray<string> errors)
        {
            var errorList = new List<string>();
            if (Guid.Empty.Equals(Id))
            {
                errorList.Add("Updated person's id cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(Surname) && (Names == null || !Names.Any() || !Names.All(string.IsNullOrWhiteSpace)))
            {
                errorList.Add("Cannot update person's surname and all names to blanks.");
            }
            if (!Sex.HasValue)
            {
                errorList.Add("Person's sex cannot be updated to null.");
            }

            errors = errorList.ToImmutableArray();
            return errors.IsEmpty;
        }
    }
}