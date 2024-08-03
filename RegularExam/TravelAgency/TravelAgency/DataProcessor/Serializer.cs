using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TravelAgency.Data;
using TravelAgency.Data.Models.Enums;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Helpers;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            var guides = context.Guides
                .Where(guide => guide.Language == Language.Spanish)
                .Select(guide => new GuideExportDto
                {
                    FullName = guide.FullName,
                    TourPackagesExportDto = guide.TourPackagesGuides
                        .Select(tpg => tpg.TourPackage)
                        .OrderByDescending(tp => tp.Price)
                        .ThenBy(tp => tp.PackageName)
                        .Select(tp => new TourPackagesExportDto
                        {
                            PackageName = tp.PackageName,
                            Description = tp.Description,
                            Price = tp.Price.ToString("F2")
                        }).ToList()
                })
                .OrderByDescending(guide => guide.TourPackagesExportDto.Count)
                .ThenBy(guide => guide.FullName)
                .ToList();

            return XmlSerializationHelper.Serialize<List<GuideExportDto>>(guides, "Guides");
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                DateFormatString = "yyyy-MM-dd"
            };

            var customers = context.Customers.AsNoTracking()
              .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
              .Select(c => new
              {
                  FullName = c.FullName,
                  PhoneNumber = c.PhoneNumber,
                  Bookings = c.Bookings
                  .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                  .Select(b => new
                  {
                      TourPackageName = b.TourPackage.PackageName,
                      Date = b.BookingDate
                  })
                  .OrderBy(b => b.Date)
                  .ToList()
              })
              .OrderByDescending(c => c.Bookings.Count)
              .ThenBy(c => c.FullName)
              .ToList();

            return JsonConvert.SerializeObject(customers, jsonSettings);
        }
    }
}
