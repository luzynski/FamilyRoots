using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyRoots.Data.Requests
{
    public class UpsertPersonRequest : IValidatableRequest
    {
        public Guid? Id { get; set; }

        public string Surname { get; set; }

        public string[] Names { get; set; }
        
        public Sex? Sex { get; set; }

        public bool IsValid(out IReadOnlyList<string> errors)
        {
            var errorList = new List<string>();
            if (Id.HasValue && Guid.Empty.Equals(Id.Value))
            {
                errorList.Add("Person id cannot be empty uuid.");
            }
            if (string.IsNullOrWhiteSpace(Surname) && (Names == null || !Names.Any() || !Names.All(string.IsNullOrWhiteSpace)))
            {
                errorList.Add("Cannot set both person surname and all names to blanks.");
            }
            if (!Id.HasValue && !Sex.HasValue)
            {
                errorList.Add("Person sex has to be set on creation.");
            }
            if (Id.HasValue && Sex.HasValue)
            {
                errorList.Add("Cannot change person sex.");
            }

            errors = errorList;
            return errors.Any();
        }

        protected bool Equals(UpsertPersonRequest other)
        {
            return Nullable.Equals(Id, other.Id) && Surname == other.Surname && Names.Equals(other.Names) && Sex == other.Sex;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((UpsertPersonRequest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ Surname.GetHashCode();
                hashCode = (hashCode * 397) ^ Names.GetHashCode();
                hashCode = (hashCode * 397) ^ Sex.GetHashCode();
                return hashCode;
            }
        }
    }
}