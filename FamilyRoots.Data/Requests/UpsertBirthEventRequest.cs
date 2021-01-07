using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyRoots.Data.Requests
{
    public class UpsertBirthEventRequest : IValidatableRequest
    {
        public Guid? Id { get; set; }
        
        public Guid ChildId { get; set; }
        
        public Guid? FatherId { get; set; }
        
        public Guid? MotherId { get; set; }
        
        public DateTime? BirthDate { get; set; }

        public string BirthPlace { get; set; }
        
        public bool IsValid(out IReadOnlyList<string> errors)
        {
            var errorList = new List<string>();
            if (Id.HasValue && Guid.Empty.Equals(Id.Value))
            {
                errorList.Add("Event id cannot be empty uuid.");
            }
            if (Guid.Empty.Equals(ChildId))
            {
                errorList.Add("Cannot update child id to empty uuid.");
            }

            errors = errorList;
            return !errors.Any();
        }
    }
}