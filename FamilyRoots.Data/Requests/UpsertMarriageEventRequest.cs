using System;
using FamilyRoots.Data.Validation;

namespace FamilyRoots.Data.Requests
{
    public class UpsertMarriageEventRequest
    {
        [GuidNotEmpty]
        public Guid? Id { get; set; }
        
        [GuidNotEmpty]
        public Guid FirstSpouseId { get; set; }
        
        [GuidNotEmpty]
        public Guid SecondSpouseId { get; set; }
        
        public DateTime? MarriageDate { get; set; }
        
        public DateTime? DivorceDate { get; set; }
    }
}