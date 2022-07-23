using Microsoft.AspNetCore.Mvc;
using ApplicantsProcessor.Core.Models;
using ApplicantsProcessor.Core.Services;
using Microsoft.AspNetCore.Cors;

namespace ApplicantsProcessor.Core.Controllers
{
    [EnableCors("Policy")]
    [ApiController]
    [Route("applicants")]
    public class ApplicantsController : Controller
    {
        [HttpGet("get-by-code/{university}/{code}")]
        public async Task<IEnumerable<IEnumerable<Applicant>>> GetApplicantsByCodeAsync([FromRoute] University university, [FromRoute] string code)
        {
            ApplicantService applicantService = new();

            var applicants = await applicantService.GetApplicantsByCode(code, university);

            return applicants;
        }

        [HttpGet("get-by-link/{university}")]
        public async Task<IEnumerable<Applicant>> GetApplicantsByLinkAsync([FromRoute] University university, [FromQuery] string link)
        {
            ApplicantService applicantService = new();

            var applicants = await applicantService.GetApplicantsByLinkAsync(link, university);

            return applicants;
        }
    }
}
