using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        Task<IEnumerable<Person>> CreatePeopleAsync(ImmutableList<CreatePersonRequest> newPeople);
        Task<IEnumerable<Person>> UpdatePeopleAsync(ImmutableList<UpdatePersonRequest> updatedPeople);
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

        public async Task<IEnumerable<Person>> CreatePeopleAsync(ImmutableList<CreatePersonRequest> newPeople)
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
        
        public async Task<IEnumerable<Person>> UpdatePeopleAsync(ImmutableList<UpdatePersonRequest> updatedPeople)
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
        
        public async Task DeletePeopleAsync()
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.RunAsync("MATCH (p:Person) DETACH DELETE p");
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task DeletePersonAsync(Guid uuid)
        {
            var session = _driver.AsyncSession();
            try
            {
                await session.RunAsync($"MATCH (p:Person) WHERE p.id = '{uuid}' DETACH DELETE p");
            }
            finally
            {
                await session.CloseAsync();
            }
        }
    }
}