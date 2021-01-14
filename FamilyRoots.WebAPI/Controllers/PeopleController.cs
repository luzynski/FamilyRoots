using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using FamilyRoots.WebAPI.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FamilyRoots.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PeopleController : Controller
    {
        private readonly IGraphDatabase _database;
        
        public PeopleController(IGraphDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAsync([FromQuery(Name="ids:guid")] IList<Guid> ids,
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            if (!ids.Any())
            {
                return NoContent();
            }
            var storedPeople = await _database.GetPeopleAsync(ids);
            return Ok(storedPeople);
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] ICollection<UpsertPersonRequest> peopleToUpsert,
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            if (!peopleToUpsert.Any())
            {
                return NoContent();
            }
            var peopleToInsert = peopleToUpsert.Where(x => !x.Id.HasValue).ToList();
            var peopleToUpdate = peopleToUpsert.Where(x => x.Id.HasValue).ToList();
            var peopleToUpdateIds = peopleToUpdate.Select(x => x.Id.Value).ToList();
            var storedPeople = await _database.GetPeopleAsync(peopleToUpdateIds);
            var missingIds = peopleToUpdateIds.Except(storedPeople.Select(x => x.Id)).ToList();
            if (missingIds.Any())
            {
                return CreateMissingObjectsResponse(apiBehaviorOptions, missingIds,
                    peopleToUpsert.Select(x => x.Id ?? Guid.Empty).ToList(), "Updated person id does not exist.");
            }
            var createdPeople = await _database.CreatePeopleAsync(peopleToInsert);
            var updatedPeople = await _database.UpdatePeopleAsync(peopleToUpdate);
            return Ok(createdPeople.Concat(updatedPeople));
        }
        
        [HttpDelete]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromQuery(Name="ids:guid")] IList<Guid> ids,
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            if (!ids.Any())
            {
                return NoContent();
            }
            var storedPeople = await _database.GetPeopleAsync(ids);
            var missingIds = ids.Except(storedPeople.Select(x => x.Id)).ToList();
            if (missingIds.Any())
            {
                return CreateMissingObjectsResponse(apiBehaviorOptions, missingIds, ids, "Deleted person id does not exist.");
            }
            await _database.DeletePeopleAsync(ids);
            return NoContent();
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync()
        {
            await _database.DeleteAllAsync();
            return NoContent();
        }
    }
}
