using System;
using FamilyRoots.Data.Validation;

namespace FamilyRoots.Data.Requests
{
    public class UpsertDeathEventRequest
    {
        [GuidNotEmpty]
        public Guid? Id { get; set; }
        
        [GuidNotEmpty]
        public Guid DecedentId { get; set; }
        
        public DateTime? DeathDate { get; set; }
     
        [BlankNotAllowed]
        public string BurialPlace { get; set; }
    }
}