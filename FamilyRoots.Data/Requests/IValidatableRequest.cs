using System.Collections.Generic;

namespace FamilyRoots.Data.Requests
{
    public interface IValidatableRequest
    {
        bool IsValid(out IReadOnlyList<string> errors);
    }
}