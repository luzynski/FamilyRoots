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

        public bool IsValid(out ImmutableArray<string> errors)
        {
            var errorList = new List<string>();
            if (Guid.Empty.Equals(Id))
            {
                errorList.Add("Updated person id cannot be empty.");
            }
            if (string.IsNullOrWhiteSpace(Surname) && (Names == null || !Names.Any() || !Names.All(string.IsNullOrWhiteSpace)))
            {
                errorList.Add("Cannot update surname and all names to blanks.");
            }

            errors = errorList.ToImmutableArray();
            return errors.IsEmpty;
        }
    }
}