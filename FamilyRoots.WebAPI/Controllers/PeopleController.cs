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
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IGraphDatabase _database;
        
        public PeopleController(ILogger<PeopleController> logger, IGraphDatabase database)
        {
            _logger = logger;
            _database = database;
        }
        
        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Person>> GetAsync([FromQuery(Name="guids:guid")] IReadOnlyList<Guid> guids)
        {
            return await _database.GetPeopleAsync(guids);
        }
        
        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("")]
        public async Task<IEnumerable<Person>> Create([FromBody] IReadOnlyList<CreatePersonRequest> newPeople)
        {
            return await _database.CreatePeopleAsync(newPeople);
        }
        
        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("")]
        public async Task<IEnumerable<Person>> Update([FromBody] IReadOnlyList<UpdatePersonRequest> updatedPeople)
        {
            return await _database.UpdatePeopleAsync(updatedPeople);
        }
        
        [MapToApiVersion("1.0")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete]
        [Route("")]
        public async Task DeleteAsync([FromQuery(Name="guids:guid")] IReadOnlyList<Guid> guids)
        {
            await _database.DeletePeopleAsync(guids);
        }
        
        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("{father-id:guid}/is-father-of/{child-id:guid}")]
        public Person CreatePaternityRelation(Guid fatherId, Guid childId)
        {
            throw new NotImplementedException();
        }
        
        [MapToApiVersion("1.0")]
        [HttpPost]
        [Route("{mother-id:guid}/is-mother-of/{child-id:guid}")]
        public Person CreateMaternityRelation(Guid motherId, Guid childId)
        {
            throw new NotImplementedException();
        }
        
        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("{first-party-id:guid}/married/{second-party-id:guid}")]
        public Person CreateMarriageRelation(Guid firstPartyId, Guid secondPartyId, DateTime? date)
        {
            throw new NotImplementedException();
        }
        
        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("{first-party-id:guid}/divorced/{second-party-id:guid}")]
        public Person CreateDivorceRelation(Guid firstPartyId, Guid secondPartyId, DateTime? date)
        {
            throw new NotImplementedException();
        }
    }
}
