using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using FamilyRoots.WebAPI.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FamilyRoots.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IGraphDatabase _database;
        
        public PeopleController(IGraphDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
            if (!ids.Any())
            {
                return Ok(Enumerable.Empty<Person>());
            }
            var storedPeople = await _database.GetPeopleAsync(ids);
            var missingIds = ids.Except(storedPeople.Select(x => x.Id)).ToList();
            return missingIds.Any() ? (IActionResult) NotFound(missingIds) : Ok(storedPeople);
        }

        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] IReadOnlyList<UpsertPersonRequest> peopleToUpsert)
        {
            var peopleToInsert = peopleToUpsert.Where(x => !x.Id.HasValue).ToList();
            var peopleToUpdate = peopleToUpsert.Where(x => x.Id.HasValue).ToList();
            /*var storedPeople = await _database.GetPeopleAsync(peopleToUpdate.Select(x => x.Id.Value).ToList());
            var missingIds = ids.Except(storedPeople.Select(x => x.Id)).ToList();
            if (missingIds.Any())
            {
                return BadRequest(missingIds);
            }*/
            //TODO: add validation - nodes to update has to exist, update node only once
            var createdPeople = await _database.UpdatePeopleAsync(peopleToInsert);
            var updatedPeople = await _database.UpdatePeopleAsync(peopleToUpdate);
            return Ok(createdPeople.Concat(updatedPeople));
        }
        
        [HttpDelete]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromQuery(Name="ids:guid")] IReadOnlyList<Guid> ids)
        {
            if (!ids.Any())
            {
                return Ok();
            }
            var storedPeople = await _database.GetPeopleAsync(ids);
            var missingIds = ids.Except(storedPeople.Select(x => x.Id)).ToList();
            if (missingIds.Any())
            {
                return BadRequest(missingIds);
            }
            await _database.DeletePeopleAsync(ids);
            return Ok();
        }
        
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAsync()
        {
            await _database.DeleteAllAsync();
            return Ok();
        }
    }
}
