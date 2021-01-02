using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FamilyRoots.Data.Requests
{
    public class UpdateBirthEventRequest : IValidatableRequest
    {
        public Guid Id { get; set; }
        
        public Guid ChildId { get; set; }
        
        public Guid? FatherId { get; set; }
        
        public Guid? MotherId { get; set; }
        
        public DateTime? BirthDate { get; set; }

        public string BirthPlace { get; set; }
        
        public bool IsValid(out ImmutableArray<string> errors)
        {
            var errorList = new List<string>();
            if (Guid.Empty.Equals(Id))
            {
                errorList.Add("Updated event id cannot be empty.");
            }
            if (Guid.Empty.Equals(ChildId))
            {
                errorList.Add("Cannot update child id to blank.");
            }

            errors = errorList.ToImmutableArray();
            return errors.IsEmpty;
        }
    }
}