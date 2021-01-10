using System;
using FamilyRoots.Data.Validation;

namespace FamilyRoots.Data.Requests
{
    public class UpsertBirthEventRequest
    {
        
        [GuidNotEmpty]
        public Guid? Id { get; set; }
        
        [GuidNotEmpty]
        public Guid ChildId { get; set; }
        
        [GuidNotEmpty]
        public Guid? FatherId { get; set; }
        
        [GuidNotEmpty]
        public Guid? MotherId { get; set; }
        
        public DateTime? BirthDate { get; set; }

        [BlankNotAllowed]
        public string BirthPlace { get; set; }
    }
}