using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyRoots.Data.Requests;
using RestEase;

namespace FamilyRoots.Data.Api
{
    [AllowAnyStatusCode]
    public interface IPeopleApi
    {
        [Get("api/v1/people")]
        public Task<IEnumerable<Person>> GetAsync([Query(Name = "ids:guid")] IReadOnlyList<Guid> ids);

        [Put("api/v1/people")]
        public Task<IEnumerable<Person>> Update([Body] IReadOnlyList<UpsertPersonRequest> peopleToUpsert);

        [Delete("api/v1/people")]
        public Task DeleteAsync([Query(Name = "ids:guid")] IReadOnlyList<Guid> ids);
    }
}