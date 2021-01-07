using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using Neo4j.Driver;
using Type = FamilyRoots.Data.Type;

namespace FamilyRoots.WebAPI.Persistence
{
    public interface IGraphDatabase
    {
        Task<IEnumerable<Person>> GetPeopleAsync();
        Task<IEnumerable<Person>> GetPeopleAsync(IReadOnlyList<Guid> guids);
        Task<IEnumerable<Person>> CreatePeopleAsync(IReadOnlyList<UpsertPersonRequest> newPeople);
        Task<IEnumerable<Person>> UpdatePeopleAsync(IReadOnlyList<UpsertPersonRequest> updatedPeople);
        Task DeletePeopleAsync(IReadOnlyList<Guid> guids);
        Task DeleteAllAsync();
        Task<IEnumerable<Event>> GetEventsAsync(Type birth, IReadOnlyList<Guid> ids);
        Task<IEnumerable<BirthEvent>> CreateBirthEventsAsync(IReadOnlyList<UpsertBirthEventRequest> newBirthEvents);
        Task<IEnumerable<BirthEvent>> UpdateBirthEventsAsync(IReadOnlyList<UpsertBirthEventRequest> updatedBirthEvents);
        Task<IEnumerable<DeathEvent>> CreateDeathEventsAsync(IReadOnlyList<UpsertBirthEventRequest> newDeathEvents);
        Task<IEnumerable<DeathEvent>> UpdateDeathEventsAsync(IReadOnlyList<UpsertDeathEventRequest> updatedDeathEvents);
        Task<IEnumerable<MarriageEvent>> CreateMarriageEventsAsync(IReadOnlyList<UpsertBirthEventRequest> newMarriageEvents);
        Task<IEnumerable<MarriageEvent>> UpdateMarriageEventsAsync(IReadOnlyList<UpsertMarriageEventRequest> updatedMarriageEvents);
        Task DeleteEventsAsync(Type birth, IReadOnlyList<Guid> ids);
    }
    
    public class GraphDatabase : IGraphDatabase
    {
        private readonly IDriver _driver;

        public GraphDatabase(IDriver driver)
        {
            _driver = driver;
        }

        private async Task<IEnumerable<Person>> QueryPeople(params string[] queries)
        {
            var session = _driver.AsyncSession();
            try
            {
                var people = new List<Person>();
                foreach (var query in queries)
                {
                    var cursor = await session.RunAsync(query);
                    var partial = await cursor.ToListAsync(record => record["p"].As<INode>().ToPerson());
                    people.AddRange(partial);
                }

                return people;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
        
        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            var query = "MATCH (p:Person) RETURN p";
            return await QueryPeople(query);
        }
        
        public async Task<IEnumerable<Person>> GetPeopleAsync(IReadOnlyList<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return await Task.FromResult(Enumerable.Empty<Person>());
            }
            var query = $"MATCH (p:Person) WHERE p.id IN ['{string.Join("','", ids)}'] RETURN p";
            return await QueryPeople(query);
        }

        public async Task<IEnumerable<Person>> CreatePeopleAsync(IReadOnlyList<UpsertPersonRequest> newPeople)
        {
            return await QueryPeople(newPeople.Select(x => x.ToCypherCreateQuery()).ToArray());
        }
        
        public async Task<IEnumerable<Person>> UpdatePeopleAsync(IReadOnlyList<UpsertPersonRequest> updatedPeople)
        {
            return await QueryPeople(updatedPeople.Select(x => x.ToCypherUpdateQuery()).ToArray());
        }


        public async Task DeleteAllAsync()
        {
            var query = "MATCH (n) DETACH DELETE n";
            await QueryPeople(query);
        }

        public Task<IEnumerable<Event>> GetEventsAsync(Type birth, IReadOnlyList<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BirthEvent>> CreateBirthEventsAsync(IReadOnlyList<UpsertBirthEventRequest> newBirthEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BirthEvent>> UpdateBirthEventsAsync(IReadOnlyList<UpsertBirthEventRequest> updatedBirthEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeathEvent>> CreateDeathEventsAsync(IReadOnlyList<UpsertBirthEventRequest> newDeathEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeathEvent>> UpdateDeathEventsAsync(IReadOnlyList<UpsertDeathEventRequest> updatedDeathEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarriageEvent>> CreateMarriageEventsAsync(IReadOnlyList<UpsertBirthEventRequest> newMarriageEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarriageEvent>> UpdateMarriageEventsAsync(IReadOnlyList<UpsertMarriageEventRequest> updatedMarriageEvents)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEventsAsync(Type birth, IReadOnlyList<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public async Task DeletePeopleAsync(IReadOnlyList<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return;
            }

            var idsQueryList = $"['{string.Join("','", ids)}']";
            var deletePersonQuery = $"MATCH (p:Person) WHERE p.id IN {idsQueryList} " +
                $"MATCH (be:Event)-(:CHILD)->(c:Person) WHERE be.type = 'birth' AND c.id IN {idsQueryList} " +
                $"MATCH (de:Event)-(:DECEDENT)->(d:Person) WHERE de.type = ''death' AND d.id IN {idsQueryList} " +
                "DETACH DELETE p, be, de";
            var deleteOrphanedEventsQuery = "MATCH (me:Event) " +
                "WHERE me.type = 'marriage' AND size((me)-(r:SPOUSE)->(:Person)) = 0" +
                "DETACH DELETE me";
            await QueryPeople(deletePersonQuery, deleteOrphanedEventsQuery);
        }
    }
}