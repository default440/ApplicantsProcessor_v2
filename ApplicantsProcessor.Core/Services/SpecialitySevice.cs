using ApplicantsProcessor.Core.Models;
using Newtonsoft.Json;

namespace ApplicantsProcessor.Core.Services
{
    public class SpecialitySevice
    {
        private static readonly string undergraduateFilePath = @".\Data\SpecialitiesUndergraduate.json";
        private static readonly string specialtyFilePath = @".\Data\SpecialitiesSpecialty.json";

        private static readonly IEnumerable<Speciality> specialities;

        static SpecialitySevice()
        {
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
        }

        public IEnumerable<Speciality> GetSpecialities()
        {
            return specialities;
        }
    }
}
