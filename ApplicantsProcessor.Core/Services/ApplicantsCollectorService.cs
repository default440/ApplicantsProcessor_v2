using AngleSharp.Html.Parser;
using ApplicantsProcessor.Core.Models;

namespace ApplicantsProcessor.Core.Services
{
    public class ApplicantsCollectorService
    {
        public async Task<List<Applicant>> GetApplicantsFromTsu(SpecialityLink link, bool needPlan = true)
        {
            var client = new HttpClient();

            link.Link = "https://api.codetabs.com/v1/proxy?quest=" + link.Link;

            var responseBody = await client.GetStringAsync(link.Link);
            var applicants = new List<Applicant>();

            var htmlParser = new HtmlParser();
            var html = htmlParser.ParseDocument(responseBody);

            var tbody = html.GetElementsByTagName("tbody")[0];
            var trs = tbody.GetElementsByClassName("rating-tr");

            int i = 0;

            var places = "1000000";
            var places_content = html.GetElementsByClassName("places");
            if (places_content != null && places_content.Count() > 0)
            {
                places = places_content[0]
                    .TextContent
                    .Replace("Бюджет: ", "")
                    .Split(" ")[0];
            }

            if (Int32.TryParse(places, out var countOfVacancies))
            {
                foreach (var tr in trs)
                {
                    var tds = tr.GetElementsByTagName("td");

                    if (tds.Length < 11)
                    {
                        continue;
                    }

                    var id = tds[2].TextContent.Trim();
                    if (id == "")
                    {
                        id = tds[1].TextContent.Trim();
                    }
                    id = id.Replace("-", "").Replace(" ", "");

                    var docs = tds[3].TextContent.Trim() == "Оригинал";
                    var agree = tds[5].TextContent.Trim() == "Да";

                    var score_text = tds[tds.Length - 2].TextContent.Trim();

                    if (Int32.TryParse(score_text, out var score))
                    {
                        var applicant = new Applicant
                        {
                            Id = id,
                            Score = score
                        };

                        if (needPlan)
                        {
                            applicant.Plans = new List<ApplicantPlan> {
                                new ApplicantPlan()
                                {
                                    Name = link.Name,
                                    Priority = -1,
                                    State = i < countOfVacancies,
                                    University = University.Tsu,
                                    HasAgreement = agree,
                                    HasOriginal = docs
                                }
                            };
                        }

                        applicants.Add(applicant);
                    }

                    i++;
                }
            }

            return applicants;
        }

        public async Task<List<Applicant>> GetApplicantsFromNstu(string link, bool needPlan = true)
        {
            var client = new HttpClient();
            var responseBody = await client.GetStringAsync(link);
            var applicants = new List<Applicant>();

            var htmlParser = new HtmlParser();
            var html = htmlParser.ParseDocument(responseBody);

            var specialityName = "";
            var specialityName_content = html.GetElementsByTagName("div")
                .Where(x => x.TextContent.Trim().StartsWith("Направление: ")).ToList();
            if (specialityName_content.Count == 0)
            {
                specialityName_content = html.GetElementsByTagName("div")
                    .Where(x => x.TextContent.Trim().StartsWith("Cпециальность: ")).ToList();

                if (specialityName_content.Count == 0)
                {
                    return applicants;
                }

                specialityName = specialityName_content[0]
                    .TextContent.Replace("Cпециальность: ", "").Trim();
            }
            else
            {
                specialityName = specialityName_content[0]
                    .TextContent.Replace("Направление: ", "").Trim();
            }

            var vac_contenet = html.GetElementsByTagName("div")
                .Where(x => x.TextContent.Trim().StartsWith("Количество бюджетных мест")).ToList();

            if (vac_contenet.Count == 0)
            {
                return applicants;
            }

            Int32.TryParse(vac_contenet[0]
                .GetElementsByTagName("b")[0].TextContent.Trim(), out var countOfVacancies);

            var table = html.GetElementsByClassName("tablelike__table-body")[0];
            var ratingCards = table.GetElementsByClassName("rating__card");

            int i = 0;

            foreach (var card in ratingCards)
            {
                var content = card.GetElementsByClassName("rating__card__content")[0];

                var id = content.GetElementsByClassName("rating__table-cell__snils")[0].TextContent
                    .Replace("-", "").Replace("\n", "").Replace("\t", "")
                    .Replace("СНИЛС / Номер дела: ", "");
                var docs = content.GetElementsByClassName("rating__table-cell__doc")[0].TextContent
                    .Replace("-", "").Replace("\n", "").Replace("\t", "")
                    .Replace("Наличие оригинала документа об образовании: ", "")
                    .Split(";");

                var original = docs[0].Trim() == "оригинал";
                var agree = false;
                if (docs.Length > 1)
                {
                    agree = docs[1].Replace(",", "").Replace(".", "")
                        .Split(" ").Contains("бюджет");
                }

                if (Int32.TryParse(content.GetElementsByClassName("rating__table-cell__total")[0].TextContent
                    .Replace("-", "").Replace("\n", "").Replace("\t", "")
                    .Replace("Общий балл: ", ""),
                    out var score) &&
                    Int32.TryParse(content.GetElementsByClassName("rating__table-cell__priority")[0].TextContent
                    .Replace("-", "").Replace("\n", "").Replace("\t", "")
                    .Replace("Номер в заявлении: ", ""),
                    out var priority))
                {
                    var applicant = new Applicant
                    {
                        Id = id,
                        Score = score
                    };

                    if (needPlan)
                    {
                        applicant.Plans = new List<ApplicantPlan> {
                            new ApplicantPlan()
                            {
                                Name = specialityName,
                                Priority = priority,
                                State = i < countOfVacancies,
                                University = University.Nstu,
                                HasAgreement = agree,
                                HasOriginal = original
                            }
                        };
                    }

                    applicants.Add(applicant);
                };

                i++;
            }

            return applicants;
        }

        public async Task<List<Applicant>> GetApplicantsPlans(List<Applicant> applicants, Speciality speciality)
        {
            Console.WriteLine(speciality.Name);

            foreach (var link in speciality.Links)
            {
                Console.WriteLine(link.Link);

                var specialityApplicants = new List<Applicant>();

                if (link.University == University.Nstu)
                {
                    try
                    {
                        specialityApplicants = await GetApplicantsFromNstu(link.Link, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (link.University == University.Tsu)
                {
                    try
                    {
                        specialityApplicants = await GetApplicantsFromTsu(link, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                foreach (var applicant in specialityApplicants)
                {
                    var specialityApplicant = applicants.Find(x => x.Id == applicant.Id);
                    if (specialityApplicant != null)
                    {
                        if (specialityApplicant.Plans.Find(x => x.Name == link.Name) != null)
                        {
                            continue;
                        }
                        specialityApplicant.Plans.Add(new ApplicantPlan
                        {
                            Name = link.Name,
                            Priority = applicant.Plans[0].Priority,
                            State = applicant.Plans[0].State,
                            University = applicant.Plans[0].University
                        });
                    }
                }
            }

            return applicants;
        }
    }
}
