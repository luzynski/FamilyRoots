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
    
        public bool IsValid(out ImmutableArray<string> errors)
        {
            var errorList = new List<string>();
            if (string.IsNullOrWhiteSpace(Surname) && (Names == null || !Names.Any() || !Names.All(string.IsNullOrWhiteSpace)))
            {
                errorList.Add("Either person surname or at least one name must be set.");
            }
            
            if (!Sex.HasValue)
            {
                errorList.Add("Person sex has to be set on creation.");
            }

            errors = errorList.ToImmutableArray();
            return errors.IsEmpty;
        }
    }
}