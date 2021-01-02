using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FamilyRoots.Data.Requests
{
    public class CreateMarriageEventRequest : IValidatableRequest
    {
        public Guid FirstSpouseId { get; set; }
        
        public Guid SecondSpouseId { get; set; }
        
        public DateTime? MarriageDate { get; set; }
        
        public DateTime? DivorceDate { get; set; }
        
        public bool IsValid(out ImmutableArray<string> errors)
        {
            var errorList = new List<string>();
            if (Guid.Empty.Equals(FirstSpouseId) || Guid.Empty.Equals(SecondSpouseId))
            {
                errorList.Add("Both spouse ids has to be set on creation.");
            }

            errors = errorList.ToImmutableArray();
            return errors.IsEmpty;
        }
    }
}