namespace ApplicantsProcessor.Core.Models
{
    public class Speciality
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool Undergraduate { get; set; }

        public IEnumerable<SpecialityLink> Links { get; set; }
    }
}
