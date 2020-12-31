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
    }
    
    public class GraphDatabase : IGraphDatabase
    {
        private readonly IDriver _driver;

        public GraphDatabase(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<IEnumerable<Person>> GetPeopleAsync(IReadOnlyList<Guid> guids)
        {
            var session = _driver.AsyncSession();
            try
            {
                var queryCondition = guids.Any()
                    ? $"WHERE p.id IN ['{string.Join("','", guids)}'] "
                    : "";
                var cursor = await session.RunAsync($"MATCH (p:Person) {queryCondition}RETURN p");
                return await cursor.ToListAsync(record => record["p"].As<INode>().ToPerson());
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<IEnumerable<Person>> CreatePeopleAsync(IReadOnlyList<CreatePersonRequest> newPeople)
        {
            var session = _driver.AsyncSession();
            try
            {
                var createdPeople = new List<Person>();
                foreach (var newPerson in newPeople)
                {
                    var cursor = await session.RunAsync(newPerson.ToCypherCreateQuery());
                    var people = await cursor.ToListAsync(record => record["p"].As<INode>().ToPerson());
                    createdPeople.Add(people.Single());
                }

                return createdPeople;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
        
        public async Task<IEnumerable<Person>> UpdatePeopleAsync(IReadOnlyList<UpdatePersonRequest> updatedPeople)
        {
            var session = _driver.AsyncSession();
            try
            {
                var createdPeople = new List<Person>();
                foreach (var updatedPerson in updatedPeople)
                {
                    var cursor = await session.RunAsync(updatedPerson.ToCypherUpdateQuery());
                    var people = await cursor.ToListAsync(record => record["p"].As<INode>().ToPerson());
                    createdPeople.Add(people.Single());
                }

                return createdPeople;
            }
            finally
            {
                await session.CloseAsync();
            }
        }
        
        public async Task DeletePeopleAsync(IReadOnlyList<Guid> guids)
        {
            var session = _driver.AsyncSession();
            try
            {
                var queryCondition = guids.Any()
                    ? $"WHERE p.id IN ['{string.Join("','", guids)}'] "
                    : "";
                await session.RunAsync($"MATCH (p:Person) {queryCondition}DETACH DELETE p");
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}