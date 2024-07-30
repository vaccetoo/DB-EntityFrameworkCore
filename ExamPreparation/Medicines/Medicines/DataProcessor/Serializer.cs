namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.XmlHelpers;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            DateTime givenDate;
            bool isGivenDateValid =
                DateTime.TryParseExact(date,
                "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out givenDate);

            if (!isGivenDateValid)
            {
                throw new Exception("Invalid date!");
            }

            var patients = context.Patients
                .Where(p => p.PatientsMedicines.Any(m => m.Medicine.ProductionDate > givenDate))
                .Select(p => new ExportPatientDto()
                {
                    Name = p.FullName,
                    AgeGroup = p.AgeGroup.ToString(),
                    Gender = p.Gender.ToString().ToLower(),
                    Medicines = p.PatientsMedicines
                    .Where(pm => pm.Medicine.ProductionDate > givenDate)
                    .Select(m => m.Medicine)
                    .OrderByDescending(m => m.ExpiryDate)
                    .ThenBy(m => m.Price)
                    .Select(m => new MedicineExportDto()
                    {
                        Name = m.Name,
                        Price = m.Price.ToString("f2"),
                        Category = m.Category.ToString().ToLower(),
                        Producer = m.Producer,
                        BestBefore = m.ExpiryDate.ToString("yyyy-MM-dd")
                    })
                    .ToList()
                })
                .OrderByDescending(p => p.Medicines.Count)
                .ThenBy(p => p.Name)
                .ToList();


            return XmlSerializationHelper.Serialize(patients, "Patients");
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                //NullValueHandling = NullValueHandling.Ignore,
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                //DateFormatString = "dd-MM-yyyy"
            };

            var medicines = context.Medicines.AsNoTracking()
                .Where(m => m.Category == (Category)medicineCategory && m.Pharmacy.IsNonStop)
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(m => new
                {
                    Name = m.Name,
                    Price = m.Price.ToString("f2"),
                    Pharmacy = new
                    {
                        Name = m.Pharmacy.Name,
                        PhoneNumber = m.Pharmacy.PhoneNumber
                    }
                })
                .ToList();

            return JsonConvert.SerializeObject(medicines, jsonSettings);
        }
    }
}
