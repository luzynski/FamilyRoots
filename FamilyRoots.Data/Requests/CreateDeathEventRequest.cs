using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FamilyRoots.Data.Requests
{
    public class CreateDeathEventRequest : IValidatableRequest
    {
        public Guid PersonId { get; set; }
        
        public DateTime? DeathDate { get; set; }
     
        public string BurialPlace { get; set; }
        
        public bool IsValid(out ImmutableArray<string> errors)
        {
            var errorList = new List<string>();
            if (Guid.Empty.Equals(PersonId))
            {
                errorList.Add("Person id has to be set on creation.");
            }

            errors = errorList.ToImmutableArray();
            return errors.IsEmpty;
        }
    }
}