using System.Collections.Immutable;

namespace FamilyRoots.Data.Requests
{
    public interface IValidatableRequest
    {
        bool IsValid(out ImmutableArray<string> errors);
    }
}