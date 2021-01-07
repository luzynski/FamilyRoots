using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyRoots.Data.Requests
{
    public class UpsertMarriageEventRequest : IValidatableRequest
    {
        public Guid? Id { get; set; }
        
        public Guid FirstSpouseId { get; set; }
        
        public Guid SecondSpouseId { get; set; }
        
        public DateTime? MarriageDate { get; set; }
        
        public DateTime? DivorceDate { get; set; }
        
        public bool IsValid(out IReadOnlyList<string> errors)
        {
            var errorList = new List<string>();
            if (Id.HasValue && Guid.Empty.Equals(Id.Value))
            {
                errorList.Add("Event id cannot be empty uuid.");
            }
            if (Guid.Empty.Equals(FirstSpouseId) || Guid.Empty.Equals(SecondSpouseId))
            {
                errorList.Add("Cannot set spouse ids to empty uuids.");
            }

            errors = errorList;
            return !errors.Any();
        }
    }
}