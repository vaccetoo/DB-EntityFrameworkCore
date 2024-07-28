using Cadastre.Data;
using Cadastre.Data.Enumerations;
using Cadastre.Data.Models;
using Cadastre.DataProcessor.ImportDtos;
using Cadastre.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Cadastre.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            var districtsDto = XmlSerializationHelper.Deserialize<List<ImportDistrictDto>>(xmlDocument, "Districts");

            List<District> districts = new List<District>();

            var result = new StringBuilder();

            foreach (var districtDto in districtsDto)
            {
                if (!IsValid(districtDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Region region;
                if (!Enum.TryParse<Region>(districtDto.Region, out region))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                if (districts.Any(d => d.Name == districtDto.Name))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                District district = new District()
                {
                    Name = districtDto.Name,
                    PostalCode = districtDto.PostalCode,
                    Region = region
                };

                foreach (var propDto in districtDto.Properties)
                {
                    if (!IsValid(propDto))
                    {
                        result.AppendLine(ErrorMessage);
                    }

                    DateTime dateOfAcquisition;
                    if (!DateTime.TryParseExact(propDto.DateOfAcquisition, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfAcquisition))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (dbContext.Properties.Any(p => p.PropertyIdentifier == propDto.PropertyIdentifier) ||
                        (district.Properties.Any(p => p.PropertyIdentifier == propDto.PropertyIdentifier) ||
                        (dbContext.Properties.Any(p => p.Address == propDto.Address) || 
                        (district.Properties.Any(p => p.Address == propDto.Address)))))
                    {
                        districts.Add(district);
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    district.Properties.Add(new Property()
                    {
                        PropertyIdentifier = propDto.PropertyIdentifier,
                        Area = propDto.Area,
                        Details = propDto.Details,
                        Address = propDto.Address,
                        DateOfAcquisition = dateOfAcquisition
                    });
                }

                districts.Add(district);
                result.AppendLine(string.Format(SuccessfullyImportedDistrict, district.Name, district.Properties.Count));
            }

            dbContext.Districts.AddRange(districts);
            dbContext.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                DateFormatString = "dd-MM-yyyy"
            };

            var citizensDto = JsonConvert.DeserializeObject<List<ImportCitizenDto>>(jsonDocument, jsonSettings);

            var result = new StringBuilder();
            List<Citizen> citizens = new List<Citizen>();

            foreach (var citizenDto in citizensDto)
            {
                if (!IsValid(citizenDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime birthDate;
                if (!DateTime.TryParseExact(citizenDto.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthDate))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                MaritalStatus maritalStatus;
                if (!Enum.TryParse(citizenDto.MaritalStatus, true, out maritalStatus))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Citizen citizen = new Citizen()
                {
                    FirstName = citizenDto.FirstName,
                    LastName = citizenDto.LastName,
                    BirthDate = birthDate,
                    MaritalStatus = maritalStatus
                };

                var validPropertiesId = dbContext.Properties.Select(p => p.Id).ToList();

                foreach (var propId in citizenDto.Properties)
                {
                    if (!validPropertiesId.Contains(propId))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    PropertyCitizen propertyCitizen = new PropertyCitizen()
                    {
                        Citizen = citizen,
                        PropertyId = propId
                    };

                    citizen.PropertiesCitizens.Add(propertyCitizen);
                }

                result.AppendLine(string.Format(SuccessfullyImportedCitizen, citizen.FirstName, citizen.LastName, citizen.PropertiesCitizens.Count));
                citizens.Add(citizen);
            }

            dbContext.Citizens.AddRange(citizens);
            dbContext.SaveChanges();

            Console.WriteLine(dbContext.PropertiesCitizens.Count());
            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
