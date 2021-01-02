using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FamilyRoots.Data.Requests
{
    public class UpdateMarriageEventRequest : IValidatableRequest
    {
        public Guid Id { get; set; }
        
        public Guid FirstSpouseId { get; set; }
        
        public Guid SecondSpouseId { get; set; }
        
        public DateTime? MarriageDate { get; set; }
        
        public DateTime? DivorceDate { get; set; }
        
        public bool IsValid(out ImmutableArray<string> errors)
        {
            var errorList = new List<string>();
            if (Guid.Empty.Equals(Id))
            {
                errorList.Add("Updated event id cannot be empty.");
            }
            if (Guid.Empty.Equals(FirstSpouseId) || Guid.Empty.Equals(SecondSpouseId))
            {
                errorList.Add("Cannot update spouse ids to blanks.");
            }

            errors = errorList.ToImmutableArray();
            return errors.IsEmpty;
        }
    }
}