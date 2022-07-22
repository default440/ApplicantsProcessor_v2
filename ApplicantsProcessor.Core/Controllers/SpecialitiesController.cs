using ApplicantsProcessor.Core.Models;
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
            throw new NotImplementedException();
        }
    }
}
