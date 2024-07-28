using Cadastre.Data;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Cadastre.Data.Enumerations;
using Cadastre.DataProcessor.ExportDtos;
using Cadastre.Helpers;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                //NullValueHandling = NullValueHandling.Ignore,
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                DateFormatString = "dd/MM/yyyy"
            };

            DateTime dateTime = DateTime.Parse("01/01/2000");

            var properties = dbContext.Properties
                .Where(p => p.DateOfAcquisition > dateTime)
                .Select(p => new
                {
                    p.PropertyIdentifier,
                    p.Area,
                    p.Address,
                    p.DateOfAcquisition,
                    Owners = p.PropertiesCitizens.Select(pc => new
                    {
                        LastName = pc.Citizen.LastName,
                        MaritalStatus = pc.Citizen.MaritalStatus.ToString(),
                    })
                    .OrderBy(p => p.LastName) 
                    .ToList()
                })
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .ToList();

            return JsonConvert.SerializeObject(properties, jsonSettings);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            var properties = dbContext.Properties
                .Where(p => p.Area >= 100)
                .OrderBy(p => p.DateOfAcquisition)
                .Select(p => new ExportpropertyDto()
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy"),
                    PostalCode = p.District.PostalCode
                })
                .OrderByDescending(p => p.Area)
                .ToList();

            return XmlSerializationHelper.Serialize(properties, "Properties", false);
        }
    }
}
