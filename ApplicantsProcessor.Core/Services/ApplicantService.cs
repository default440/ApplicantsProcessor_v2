using ApplicantsProcessor.Core.Models;

namespace ApplicantsProcessor.Core.Services
{
    public class ApplicantService
    {
        public async Task<IEnumerable<IEnumerable<Applicant>>> GetApplicantsByCode(string code, University university)
        {
            SpecialitySevice specialityService = new();
            var links = specialityService.GetSpecialities()
                .Select(x =>
                {
                    x.Links = x.Links.Where(y => y.University == university);
                    return x;
                })
                .Where(x => x.Links.Any() && x.Code == code)
                .SelectMany(x => x.Links);

            if (!links.Any())
            {
                return Enumerable.Empty<IEnumerable<Applicant>>();
            }

            return university switch
            {
                University.Nstu => await GetNstuApplicantsAsync(links),
                University.Tsu => await GetTsuApplicantsAsync(links),
                _ => throw new NotSupportedException()
            };
        }

        public async Task<IEnumerable<Applicant>> GetApplicantsByLinkAsync(string link, University university)
        {
            SpecialitySevice specialityService = new();
            var specialityLink = specialityService.GetSpecialities()
                .SelectMany(x => x.Links
                    .Where(y => y.University == university && y.Link == link))
                .FirstOrDefault();

            if (specialityLink == default)
            {
                return Enumerable.Empty<Applicant>();
            }

            return university switch
            {
                University.Nstu => await GetNstuApplicantsAsync(specialityLink),
                University.Tsu => await GetTsuApplicantsAsync(specialityLink),
                _ => throw new NotSupportedException()
            };
        }


        private async Task<IEnumerable<IEnumerable<Applicant>>> GetNstuApplicantsAsync(IEnumerable<SpecialityLink> links)
        {
            List<List<Applicant>> applicantsBySections = new();

            var tasks = links.ToList().Select(link => GetNstuApplicantsAsync(link));
            var results = await Task.WhenAll(tasks);

            results.ToList().ForEach(x => applicantsBySections.Add(x.ToList()));

            return applicantsBySections;
        }

        private async Task<IEnumerable<IEnumerable<Applicant>>> GetTsuApplicantsAsync(IEnumerable<SpecialityLink> links)
        {
            List<List<Applicant>> applicantsBySections = new();

            var tasks = links.ToList().Select(link => GetTsuApplicantsAsync(link));
            var results = await Task.WhenAll(tasks);

            results.ToList().ForEach(x => applicantsBySections.Add(x.ToList()));

            return applicantsBySections;

        }

        private async Task<IEnumerable<Applicant>> GetNstuApplicantsAsync(SpecialityLink link)
        {
            ApplicantsCollectorService applicantsCollectorService = new();
            var applicants = await applicantsCollectorService.GetApplicantsFromNstu(link.Link);

            return applicants;
        }

        private async Task<IEnumerable<Applicant>> GetTsuApplicantsAsync(SpecialityLink link)
        {
            ApplicantsCollectorService applicantsCollectorService = new();
            var applicants = await applicantsCollectorService.GetApplicantsFromTsu(link);

            return applicants;
        }
    }
}
