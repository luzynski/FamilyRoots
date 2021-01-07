using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyRoots.Data.Requests;
using RestEase;

namespace FamilyRoots.Data.Api
{
    public interface IPeopleApi
    {
        public Task<IEnumerable<Person>> GetAsync([Query(Name = "ids:guid")] IReadOnlyList<Guid> ids);

        public Task<IEnumerable<Person>> Update([Body] IReadOnlyList<UpsertPersonRequest> peopleToUpsert);

        public Task DeleteAsync([Query(Name = "ids:guid")] IReadOnlyList<Guid> ids);
    }
}