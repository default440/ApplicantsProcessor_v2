using Microsoft.AspNetCore.Mvc;
using ApplicantsProcessor.Core.Models;
using ApplicantsProcessor.Core.Services;

namespace ApplicantsProcessor.Core.Controllers
{
    [ApiController]
    [Route("applicants")]
    public class ApplicantsController : Controller
    {
        [HttpGet("get-by-code/{university}/{code}")]
        public async Task<IEnumerable<IEnumerable<Applicant>>> GetApplicantsByCodeAsync([FromRoute] string code, [FromRoute] University university)
        {
            ApplicantService applicantService = new();

            var applicants = await applicantService.GetApplicantsByCode(code, university);

            return applicants;
        }

        [HttpGet("get-by-link/{university}")]
        public async Task<IEnumerable<Applicant>> GetApplicantsByLinkAsync([FromQuery] string link, [FromRoute] University university)
        {
            ApplicantService applicantService = new();

            var applicants = await applicantService.GetApplicantsByLinkAsync(link, university);

            return applicants;
        }
    }
}
