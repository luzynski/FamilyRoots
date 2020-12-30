using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        [Route("v1/people/{id:guid}")]
        public async Task<Person> GetAsync(Guid id)
        {
            return await _database.GetPersonAsync(id);
        }
        
        [HttpPost]
        [Route("v1/people")]
        public async Task<IEnumerable<Person>> Create(ImmutableList<CreatePersonRequest> newPeople)
        {
            return await _database.CreatePeopleAsync(newPeople);
        }
        
        [HttpPut]
        [Route("v1/people")]
        public async Task<IEnumerable<Person>> Update(ImmutableList<UpdatePersonRequest> updatedPeople)
        {
            return await _database.UpdatePeopleAsync(updatedPeople);
        }
        
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete]
        [Route("v1/people")]
        public async Task DeleteAsync()
        {
            await _database.DeletePeopleAsync();
        }
        
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete]
        [Route("v1/people/{id:guid}")]
        public async Task DeleteAsync(Guid id)
        {
            await _database.DeletePersonAsync(id);
        }
        
        [HttpPost]
        [Route("v1/people/{father-id:guid}/is-father-of/{child-id:guid}")]
        public Person CreatePaternityRelation(Guid fatherId, Guid childId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [Route("v1/people/{mother-uuid}/is-mother-of/{child-id}")]
        public Person CreateMaternityRelation(Guid motherId, Guid childId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut]
        [Route("v1/people/{first-party-id}/married/{second-party-id}")]
        public Person CreateMarriageRelation(Guid firstPartyId, Guid secondPartyId, DateTime? date)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut]
        [Route("v1/people/{first-party-id}/divorced/{second-party-id}")]
        public Person CreateDivorceRelation(Guid firstPartyId, Guid secondPartyId, DateTime? date)
        {
            throw new NotImplementedException();
        }
    }
}
