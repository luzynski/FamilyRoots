using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FamilyRoots.WebAPI.Controllers
{
    public abstract class Controller : ControllerBase
    {
        protected IActionResult CreateMissingObjectsResponse<T>(IOptions<ApiBehaviorOptions> apiBehaviorOptions, 
            IList<T> missing, IList<T> all, string errorMessage = "Unknown object.")
        {
            foreach (var item in missing)
            {
                ModelState.AddModelError($"[{all.IndexOf(item)}]", errorMessage);
            }
            return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}