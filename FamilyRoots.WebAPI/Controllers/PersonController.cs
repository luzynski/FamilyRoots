using System;
using System.Collections.Generic;
using System.Linq;
using FamilyRoots.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FamilyRoots.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<Person> GetAll()
        {
            var rng = new Random();
            return Enumerable
                .Range(1, 5)
                .Select(index => new Person($"surname{index}"))
                .ToArray();
        }
    }
}
