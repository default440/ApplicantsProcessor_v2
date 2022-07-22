using ApplicantsProcessor.Core.Models;
using ApplicantsProcessor.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApplicantsProcessor.Core.Controllers
{
    [ApiController]
    [Route("specialities")]
    public class SpecialitiesController : Controller
    {
        [HttpGet("get")]
        public IEnumerable<Speciality> GetSpecialities()
        {
            SpecialitySevice specialitySevice = new();

            var specialities = specialitySevice.GetSpecialities();

            return specialities;
        }
    }
}
