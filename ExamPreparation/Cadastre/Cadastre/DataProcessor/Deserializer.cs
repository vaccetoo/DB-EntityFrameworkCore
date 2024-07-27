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

            return "";
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
