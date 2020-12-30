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
        Task<IEnumerable<Person>> GetPeopleAsync();
        Task<Person> GetPersonAsync(Guid uuid);
        Task<Guid> CreatePersonAsync(CreatePersonRequest newPerson);
    }
    
    public class GraphDatabase : IGraphDatabase
    {
        private readonly IDriver _driver;

        public GraphDatabase(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            var session = _driver.AsyncSession();
            try
            {
                var cursor = await session.RunAsync("MATCH (p:Person) RETURN p");
                return await cursor.ToListAsync(record => record["p"].As<INode>().ToPerson());
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<Person> GetPersonAsync(Guid uuid)
        {
            var session = _driver.AsyncSession();
            try
            {
                var cursor = await session.RunAsync($"MATCH (p:Person) WHERE p.id = '{uuid}' RETURN p");
                var people = await cursor.ToListAsync(record => record["p"].As<INode>().ToPerson());
                return people.Single();
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<Guid> CreatePersonAsync(CreatePersonRequest newPerson)
        {
            var session = _driver.AsyncSession();
            try
            {
                var cursor = await session.RunAsync(newPerson.ToCypherCreateQuery());
                var guids = await cursor.ToListAsync(record => Guid.Parse(record["id"].As<string>()));
                return guids.Single();
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}