using ApplicantsProcessor.Core.Models;
using Newtonsoft.Json;

namespace ApplicantsProcessor.Core.Services
{
    public class SpecialitySevice
    {
        private static readonly string undergraduateFilePath = "./Data/SpecialitiesUndergraduate.json";
        private static readonly string specialtyFilePath = "./Data/SpecialitiesSpecialty.json";

        private readonly IEnumerable<Speciality> specialities;

        public SpecialitySevice()
        {
            Console.WriteLine("Undergraduate file exist: " + File.Exists(undergraduateFilePath));
            Console.WriteLine("Specialty file exist: " + File.Exists(specialtyFilePath));

            var specialitiesUndergraduate = JsonConvert
                .DeserializeObject<IEnumerable<Speciality>>(File.ReadAllText(undergraduateFilePath)) ?? Enumerable.Empty<Speciality>();

            var specialitiesSpecialty = JsonConvert
                .DeserializeObject<IEnumerable<Speciality>>(File.ReadAllText(specialtyFilePath)) ?? Enumerable.Empty<Speciality>();

            specialities = specialitiesUndergraduate
                .Concat(specialitiesSpecialty
                    .Select(x =>
                    {
                        x.Undergraduate = true;
                        return x;
                    }))
                .Where(x => x.Links.Any());

            Console.WriteLine("Count of specialities: " + specialities.Count());
        }

        public IEnumerable<Speciality> GetSpecialities()
        {
            return specialities;
        }
    }
}
