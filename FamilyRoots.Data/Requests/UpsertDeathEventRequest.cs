using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyRoots.Data.Requests
{
    public class UpsertDeathEventRequest : IValidatableRequest
    {
        public Guid? Id { get; set; }
        
        public Guid DecedentId { get; set; }
        
        public DateTime? DeathDate { get; set; }
     
        public string BurialPlace { get; set; }
        
        public bool IsValid(out IReadOnlyList<string> errors)
        {
            var errorList = new List<string>();
            if (Id.HasValue && Guid.Empty.Equals(Id.Value))
            {
                errorList.Add("Event id cannot be empty uuid.");
            }
            if (Guid.Empty.Equals(DecedentId))
            {
                errorList.Add("Cannot set decedent id to empty uuid.");
            }

            errors = errorList;
            return !errors.Any();
        }
    }
}