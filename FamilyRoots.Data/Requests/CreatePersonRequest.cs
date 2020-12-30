using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FamilyRoots.Data.Requests
{
    public class CreatePersonRequest : IValidatableRequest
    {
        public string Surname { get; set; }
        
        public string[] Names { get; set; }
        
        public Sex? Sex { get; set; }
        
        public DateTime? BirthDate { get; set; }

        public string BirthPlace { get; set; }
        
        public DateTime? DeathDate { get; set; }
     
        public string BurialPlace { get; set; }
    
        public bool IsValid(out ImmutableArray<string> errors)
        {
            var errorList = new List<string>();
            if (string.IsNullOrWhiteSpace(Surname) && (Names == null || !Names.Any() || !Names.All(string.IsNullOrWhiteSpace)))
            {
                errorList.Add("Either person's surname or at least one name must be set.");
            }
            
            if (!Sex.HasValue)
            {
                errorList.Add("Person's sex has to be set on creation.");
            }

            errors = errorList.ToImmutableArray();
            return errors.IsEmpty;
        }
    }
}