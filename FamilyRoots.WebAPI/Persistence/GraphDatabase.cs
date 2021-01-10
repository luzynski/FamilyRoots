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
        Task<IEnumerable<Person>> GetPeopleAsync(IList<Guid> guids);
        Task<IEnumerable<Person>> CreatePeopleAsync(IList<UpsertPersonRequest> newPeople);
        Task<IEnumerable<Person>> UpdatePeopleAsync(IList<UpsertPersonRequest> updatedPeople);
        Task DeletePeopleAsync(IList<Guid> guids);
        Task DeleteAllAsync();
        Task<IEnumerable<Event>> GetEventsAsync(Type birth, IList<Guid> ids);
        Task<IEnumerable<BirthEvent>> CreateBirthEventsAsync(IList<UpsertBirthEventRequest> newBirthEvents);
        Task<IEnumerable<BirthEvent>> UpdateBirthEventsAsync(IList<UpsertBirthEventRequest> updatedBirthEvents);
        Task<IEnumerable<DeathEvent>> CreateDeathEventsAsync(IList<UpsertBirthEventRequest> newDeathEvents);
        Task<IEnumerable<DeathEvent>> UpdateDeathEventsAsync(IList<UpsertDeathEventRequest> updatedDeathEvents);
        Task<IEnumerable<MarriageEvent>> CreateMarriageEventsAsync(IList<UpsertBirthEventRequest> newMarriageEvents);
        Task<IEnumerable<MarriageEvent>> UpdateMarriageEventsAsync(IList<UpsertMarriageEventRequest> updatedMarriageEvents);
        Task DeleteEventsAsync(Type birth, IList<Guid> ids);
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
        
        public async Task<IEnumerable<Person>> GetPeopleAsync(IList<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return await Task.FromResult(Enumerable.Empty<Person>());
            }
            var query = $"MATCH (p:Person) WHERE p.id IN ['{string.Join("','", ids)}'] RETURN p";
            return await QueryPeople(query);
        }

        public async Task<IEnumerable<Person>> CreatePeopleAsync(IList<UpsertPersonRequest> newPeople)
        {
            return await QueryPeople(newPeople.Select(x => x.ToCypherCreateQuery()).ToArray());
        }
        
        public async Task<IEnumerable<Person>> UpdatePeopleAsync(IList<UpsertPersonRequest> updatedPeople)
        {
            return await QueryPeople(updatedPeople.Select(x => x.ToCypherUpdateQuery()).ToArray());
        }


        public async Task DeleteAllAsync()
        {
            var query = "MATCH (n) DETACH DELETE n";
            await QueryPeople(query);
        }

        public Task<IEnumerable<Event>> GetEventsAsync(Type birth, IList<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BirthEvent>> CreateBirthEventsAsync(IList<UpsertBirthEventRequest> newBirthEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BirthEvent>> UpdateBirthEventsAsync(IList<UpsertBirthEventRequest> updatedBirthEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeathEvent>> CreateDeathEventsAsync(IList<UpsertBirthEventRequest> newDeathEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeathEvent>> UpdateDeathEventsAsync(IList<UpsertDeathEventRequest> updatedDeathEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarriageEvent>> CreateMarriageEventsAsync(IList<UpsertBirthEventRequest> newMarriageEvents)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MarriageEvent>> UpdateMarriageEventsAsync(IList<UpsertMarriageEventRequest> updatedMarriageEvents)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEventsAsync(Type birth, IList<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public async Task DeletePeopleAsync(IList<Guid> ids)
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