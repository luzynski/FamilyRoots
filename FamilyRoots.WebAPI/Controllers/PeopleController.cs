using System;
using System.Collections.Generic;
using FamilyRoots.Data;
using FamilyRoots.Data.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FamilyRoots.WebAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class PeopleController : ControllerBase
    {
        private readonly ILogger<PeopleController> _logger;

        public PeopleController(ILogger<PeopleController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("/v1/people")]
        public IEnumerable<Person> Get()
        {
            throw new NotImplementedException();
        }
        
        [HttpGet]
        [Route("v1/people/{id}")]
        public Person Get(long id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [Route("/v1/people")]
        public Person Create(CreatePersonRequest newPerson)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut]
        [Route("v1/people/{id}")]
        public Person Update(long id, UpdatePersonRequest updatedPerson)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [Route("v1/people/{father-id}/is-father-of/{child-id}")]
        public Person CreatePaternityRelation(long fatherId, long childId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        [Route("v1/people/{mother-id}/is-mother-of/{child-id}")]
        public Person CreateMaternityRelation(long motherId, long childId)
        {
            throw new NotImplementedException();
        }
        
        [HttpPut]
        [Route("v1/people/{first-party-id}/married/{second-party-id}")]
        public Person CreateMarriageRelation(long firstPartyId, long secondPartyId, DateTime? date)
        {
            throw new NotImplementedException();
        }
        
        //[HttpPut]
        //[Route("v1/people/{first-party-id}/divorced/{second-party-id}")]
        //public Person CreateMarriageRelation(long firstPartyId, long secondPartyId, DateTime? date)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
