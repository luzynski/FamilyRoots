using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FamilyRoots.Data.Requests
{
    public class UpdateDeathEventRequest : IValidatableRequest
    {
        public Guid Id { get; set; }
        
        public Guid PersonId { get; set; }
        
        public DateTime? DeathDate { get; set; }
     
        public string BurialPlace { get; set; }
        
        public bool IsValid(out ImmutableArray<string> errors)
        {
            var errorList = new List<string>();
            if (Guid.Empty.Equals(Id))
            {
                errorList.Add("Updated event id cannot be empty.");
            }
            if (Guid.Empty.Equals(PersonId))
            {
                errorList.Add("Cannot update person id to blank.");
            }

            errors = errorList.ToImmutableArray();
            return errors.IsEmpty;
        }
    }
}