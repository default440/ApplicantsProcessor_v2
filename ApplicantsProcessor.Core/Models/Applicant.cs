namespace ApplicantsProcessor.Core.Models
{
    public class Applicant
    {
        public string Id { get; set; }

        public int Score { get; set; }

        public List<ApplicantPlan> Plans { get; set; } = new();
    }
}
