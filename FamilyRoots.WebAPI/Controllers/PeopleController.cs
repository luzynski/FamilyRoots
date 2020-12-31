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
        public async Task<IEnumerable<Person>> GetAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
            return await _database.GetPeopleAsync(ids);
        }

        [HttpGet]
        [Route("v1/people/{child-id:guid}/father")]
        public async Task<Person> GetFatherAsync([FromRoute(Name = "child-id")] Guid childId)
        {
            return await _database.GetFatherAsync(childId);
        }

        [HttpGet]
        [Route("v1/people/{child-id:guid}/mother")]
        public async Task<Person> GetMotherAsync([FromRoute(Name = "child-id")] Guid childId)
        {
            return await _database.GetMotherAsync(childId);
        }
        
        [HttpPost]
        [Route("v1/people")]
        public async Task<IEnumerable<Person>> Create([FromBody] IReadOnlyList<CreatePersonRequest> newPeople)
        {
            return await _database.CreatePeopleAsync(newPeople);
        }
        
        [HttpPut]
        [Route("v1/people")]
        public async Task<IEnumerable<Person>> Update([FromBody] IReadOnlyList<UpdatePersonRequest> updatedPeople)
        {
            return await _database.UpdatePeopleAsync(updatedPeople);
        }
        
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete]
        [Route("v1/people")]
        public async Task DeleteAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
            await _database.DeletePeopleAsync(ids);
        }
        
        [HttpPost]
        [Route("v1/people/{father-id:guid}/is-father-of/{child-id:guid}")]
        public async Task CreatePaternityRelationAsync([FromRoute(Name = "father-id")] Guid fatherId, [FromRoute(Name = "child-id")] Guid childId)
        {
            await _database.CreatePaternityRelationAsync(fatherId, childId);
        }
        
        [HttpPost]
        [Route("v1/people/{mother-id:guid}/is-mother-of/{child-id:guid}")]
        public async Task CreateMaternityRelationAsync([FromRoute(Name = "mother-id")] Guid motherId, [FromRoute(Name = "child-id")] Guid childId)
        {
            await _database.CreateMaternityRelationAsync(motherId, childId);
        }
        
        [HttpDelete]
        [Route("v1/people/{father-id:guid}/is-father-of/{child-id:guid}")]
        public async Task DeletePaternityRelationAsync([FromRoute(Name = "father-id")] Guid fatherId, [FromRoute(Name = "child-id")] Guid childId)
        {
            await _database.DeletePaternityRelationAsync(fatherId, childId);
        }
        
        [HttpDelete]
        [Route("v1/people/{mother-id:guid}/is-mother-of/{child-id:guid}")]
        public async Task DeleteMaternityRelationAsync([FromRoute(Name = "mother-id")] Guid motherId, [FromRoute(Name = "child-id")] Guid childId)
        {
            await _database.DeleteMaternityRelationAsync(motherId, childId);
        }
        
        /*[HttpPut]
        [Route("v1/people/{first-party-id:guid}/married/{second-party-id:guid}")]
        public Person CreateMarriageRelation(Guid firstPartyId, Guid secondPartyId, DateTime? date)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut]
        [Route("v1/people/{first-party-id:guid}/divorced/{second-party-id:guid}")]
        public Person CreateDivorceRelation(Guid firstPartyId, Guid secondPartyId, DateTime? date)
        {
            throw new NotImplementedException();
        }*/
    }
}
