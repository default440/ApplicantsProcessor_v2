namespace ApplicantsProcessor.Core.Models
{
    public class ApplicantPlan
    {
        public string Name { get; set; }

        public string University { get; set; }

        public int Priority { get; set; }

        public bool State { get; set; }

        public bool HasOriginal { get; set; }

        public bool HasAgreement { get; set; }
    }
}
