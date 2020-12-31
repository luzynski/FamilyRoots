using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using Neo4j.Driver;

namespace FamilyRoots.WebAPI.Persistence
{
    public interface IGraphDatabase
    {
        Task<IEnumerable<Person>> GetPeopleAsync(IReadOnlyList<Guid> guids);
        Task<IEnumerable<Person>> CreatePeopleAsync(IReadOnlyList<CreatePersonRequest> newPeople);
        Task<IEnumerable<Person>> UpdatePeopleAsync(IReadOnlyList<UpdatePersonRequest> updatedPeople);
        Task DeletePeopleAsync(IReadOnlyList<Guid> guids);
        Task<Person> GetFatherAsync(Guid childId);
        Task<Person> GetMotherAsync(Guid childId);
        Task CreatePaternityRelationAsync(Guid fatherId, Guid childId);
        Task CreateMaternityRelationAsync(Guid motherId, Guid childId);
        Task DeletePaternityRelationAsync(Guid fatherId, Guid childId);
        Task DeleteMaternityRelationAsync(Guid motherId, Guid childId);
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
        
        public async Task<IEnumerable<Person>> GetPeopleAsync(IReadOnlyList<Guid> ids)
        {
            var queryCondition = ids.Any()
                ? $"WHERE p.id IN ['{string.Join("','", ids)}'] "
                : "";
            var query = $"MATCH (p:Person) {queryCondition}RETURN p";
            return await QueryPeople(query);
        }

        public async Task<IEnumerable<Person>> CreatePeopleAsync(IReadOnlyList<CreatePersonRequest> newPeople)
        {
            return await QueryPeople(newPeople.Select(x => x.ToCypherCreateQuery()).ToArray());
        }
        
        public async Task<IEnumerable<Person>> UpdatePeopleAsync(IReadOnlyList<UpdatePersonRequest> updatedPeople)
        {
            return await QueryPeople(updatedPeople.Select(x => x.ToCypherUpdateQuery()).ToArray());
        }
        
        public async Task DeletePeopleAsync(IReadOnlyList<Guid> ids)
        {
            var queryCondition = ids.Any()
                ? $"WHERE p.id IN ['{string.Join("','", ids)}'] "
                : "";
            var query = $"MATCH (p:Person) {queryCondition}DETACH DELETE p";
            await QueryPeople(query);
        }

        public async Task<Person> GetFatherAsync(Guid childId)
        {
            var query = $"MATCH (c:Person)<-[r:IS_FATHER_OF]-(p) WHERE c.id = '{childId}' RETURN p";
            var result = await QueryPeople(query);
            return result.DefaultIfEmpty(null).Single();
        }

        public async Task<Person> GetMotherAsync(Guid childId)
        {
            var query = $"MATCH (c:Person)<-[r:IS_MOTHER_OF]-(p) WHERE c.id = '{childId}' RETURN p";
            var result = await QueryPeople(query);
            return result.DefaultIfEmpty(null).Single();
        }

        public async Task CreatePaternityRelationAsync(Guid fatherId, Guid childId)
        {
            var query = $"MATCH (f:Person),(c:Person) WHERE f.id = '{fatherId}' AND c.id = '{childId}' " +
                $"CREATE (f)-[r:IS_FATHER_OF]->(c)";
            await QueryPeople(query);
        }

        public async Task CreateMaternityRelationAsync(Guid motherId, Guid childId)
        {
            var query = $"MATCH (m:Person),(c:Person) WHERE m.id = '{motherId}' AND c.id = '{childId}' " +
                        $"CREATE (m)-[r:IS_MOTHER_OF]->(c)";
            await QueryPeople(query);
        }

        public Task DeletePaternityRelationAsync(Guid fatherId, Guid childId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMaternityRelationAsync(Guid motherId, Guid childId)
        {
            throw new NotImplementedException();
        }
    }
}