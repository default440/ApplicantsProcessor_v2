using Microsoft.AspNetCore.Mvc;
using ApplicantsProcessor.Core.Models;

namespace ApplicantsProcessor.Core.Controllers
{
    [ApiController]
    [Route("applicants")]
    public class ApplicantsController : Controller
    {
        [HttpGet("get-by-code/{code}/{university}")]
        public IEnumerable<Applicant> GetApplicantsByCode([FromRoute] string code, [FromRoute] University university)
        {
            throw new NotImplementedException();
        }
    }
}
