namespace ApplicantsProcessor.Core.Models
{
    public class Speciality
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool Undergraduate { get; set; }

        public List<SpecialityLink> Links { get; set; } = new();
    }
}
