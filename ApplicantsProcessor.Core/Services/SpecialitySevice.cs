using ApplicantsProcessor.Core.Models;
using Newtonsoft.Json;

namespace ApplicantsProcessor.Core.Services
{
    public class SpecialitySevice
    {
        private static readonly string undergraduateFilePath =
            @"C:\Users\Default_Laptop\Source\Repos\ApplicantsProcessor_v2\ApplicantsProcessor.Core\Data\SpecialitiesUndergraduate.json";
        private static readonly string specialtyFilePath =
            @"C:\Users\Default_Laptop\Source\Repos\ApplicantsProcessor_v2\ApplicantsProcessor.Core\Data\SpecialitiesSpecialty.json";

        private static readonly IEnumerable<Speciality> specialities;

        static SpecialitySevice()
        {
            var specialitiesUndergraduate = JsonConvert
                .DeserializeObject<IEnumerable<Speciality>>(File.ReadAllText(undergraduateFilePath)) ?? new List<Speciality>();

            var specialitiesSpecialty = JsonConvert
                .DeserializeObject<IEnumerable<Speciality>>(File.ReadAllText(specialtyFilePath)) ?? new List<Speciality>();

            specialities = specialitiesUndergraduate
                .Concat(specialitiesSpecialty
                    .Select(x =>
                    {
                        x.Undergraduate = true;
                        return x;
                    }))
                .Where(x => x.Links.Any());
        }

        public IEnumerable<Speciality> GetSpecialities()
        {
            return specialities;
        }
    }
}
