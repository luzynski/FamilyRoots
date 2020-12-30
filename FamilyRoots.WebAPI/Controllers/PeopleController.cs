using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using FamilyRoots.WebAPI.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FamilyRoots.WebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IGraphDatabase _database;
        
        public PeopleController(ILogger<PeopleController> logger, IGraphDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        [Route("v1/people")]
        public async Task<IEnumerable<Person>> GetAsync()
        {
            return await _database.GetPeopleAsync();
        }
        
        [HttpGet]
        [Route("v1/people/{id}")]
        public async Task<Person> GetAsync(string id)
        {
            var uuid = Guid.Parse(id);
            return await _database.GetPersonAsync(uuid);
        }
        
        [HttpPost]
        [Route("v1/people")]
        public async Task<Guid> Create(CreatePersonRequest newPerson)
        {
            return await _database.CreatePersonAsync(newPerson);
        }
        
        [HttpPut]
        [Route("v1/people/{id}")]
        public Person Update(string id, UpdatePersonRequest updatedPerson)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [Route("v1/people/{father-id}/is-father-of/{child-id}")]
        public Person CreatePaternityRelation(string fatherId, string childId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [Route("v1/people/{mother-uuid}/is-mother-of/{child-id}")]
        public Person CreateMaternityRelation(string motherId, string childId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut]
        [Route("v1/people/{first-party-id}/married/{second-party-id}")]
        public Person CreateMarriageRelation(string firstPartyId, string secondPartyId, DateTime? date)
        {
            throw new NotImplementedException();
        }
        
        //[HttpPut]
        //[Route("v1/people/{first-party-id}/divorced/{second-party-id}")]
        //public Person CreateMarriageRelation(string firstPartyId, string secondPartyId, DateTime? date)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
