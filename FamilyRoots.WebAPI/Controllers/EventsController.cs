using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using FamilyRoots.WebAPI.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Type = FamilyRoots.Data.Type;

namespace FamilyRoots.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;
        private readonly IGraphDatabase _database;

        public EventsController(ILogger<PeopleController> logger, IGraphDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        [Route("birth")]
        public async Task<IEnumerable<BirthEvent>> GetBirthEventsAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
            return (IEnumerable<BirthEvent>) await _database.GetEventsAsync(Type.Birth, ids);
        }

        [HttpPut]
        [Route("birth")]
        public async Task<IEnumerable<BirthEvent>> UpsertBirthEventsAsync([FromBody] IReadOnlyList<UpsertBirthEventRequest> updatedBirthEvents)
        {
            return await _database.UpdateBirthEventsAsync(updatedBirthEvents);
        }
        
        [HttpDelete]
        [Route("birth")]
        public async Task DeleteBirthEventAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
            await _database.DeleteEventsAsync(Type.Birth, ids);
        }
        
        [HttpGet]
        [Route("death")]
        public async Task<IEnumerable<DeathEvent>> GetDeathEventsAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
             return (IEnumerable<DeathEvent>) await _database.GetEventsAsync(Type.Death, ids);
        }

        [HttpPut]
        [Route("death")]
        public async Task<IEnumerable<DeathEvent>> UpsertDeathEventsAsync([FromBody] IReadOnlyList<UpsertDeathEventRequest> updatedDeathEvents)
        {
            return await _database.UpdateDeathEventsAsync(updatedDeathEvents);
        }
        
        [HttpDelete]
        [Route("death")]
        public async Task DeleteDeathEventAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
            await _database.DeleteEventsAsync(Type.Death, ids);
        }
        
        [HttpGet]
        [Route("marriage")]
        public async Task<IEnumerable<MarriageEvent>> GetMarriageEventsAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
            return (IEnumerable<MarriageEvent>) await _database.GetEventsAsync(Type.Marriage, ids);
        }

        [HttpPut]
        [Route("marriage")]
        public async Task<IEnumerable<MarriageEvent>> UpsertMarriageEventAsync([FromBody] IReadOnlyList<UpsertMarriageEventRequest> updatedMarriageEvents)
        {
            return await _database.UpdateMarriageEventsAsync(updatedMarriageEvents);
        }
        
        [HttpDelete]
        [Route("marriage")]
        public async Task DeleteMarriageEventAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
            await _database.DeleteEventsAsync(Type.Marriage, ids);
        }
    }
}