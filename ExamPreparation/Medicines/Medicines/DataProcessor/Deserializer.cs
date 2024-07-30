namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.XmlHelpers;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            var patientsDto = JsonConvert.DeserializeObject<List<PatientImportDto>>(jsonString);

            var result = new StringBuilder();

            List<Patient> patients = new List<Patient>();

            foreach (var patientDto in patientsDto)
            {
                if (!IsValid(patientDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new Patient()
                {
                    FullName = patientDto.FullName,
                    AgeGroup = (AgeGroup)patientDto.AgeGroup,
                    Gender = (Gender)patientDto.Gender,
                };

                foreach (var mdeicineDtoId in patientDto.Medicines)
                {
                    if (patient.PatientsMedicines.Any(m => m.MedicineId == mdeicineDtoId))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    patient.PatientsMedicines.Add(new PatientMedicine()
                    {
                        Patient = patient,
                        MedicineId = mdeicineDtoId
                    });
                }

                patients.Add(patient);
                result.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count));
            }

            context.Patients.AddRange(patients);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            var pharmaciesDto = XmlSerializationHelper.Deserialize<List<PharmacyImportDto>>(xmlString, "Pharmacies");

            var result = new StringBuilder();

            List<Pharmacy> pharmacies = new List<Pharmacy>();

            foreach (var pharmacyDto in pharmaciesDto)
            {
                if (!IsValid(pharmacyDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Pharmacy pharmacy = new Pharmacy()
                {
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber,
                    IsNonStop = bool.Parse(pharmacyDto.IsNonStop)
                };

                foreach (var medicineDto in pharmacyDto.Medicines)
                {
                    if (!IsValid(medicineDto))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime productionDate;
                    bool isProductionDateValid = 
                        DateTime.TryParseExact(medicineDto.ProductionDate, 
                        "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out productionDate);

                    if (!isProductionDateValid)
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime expiryDate;
                    bool isExpiryDateValid =
                        DateTime.TryParseExact(medicineDto.ExpiryDate, 
                        "yyyy-MM-dd", CultureInfo.InvariantCulture, 
                        DateTimeStyles.None, out expiryDate);

                    if (!isExpiryDateValid)
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (productionDate >= expiryDate)
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (pharmacy.Medicines.Any(m => m.Name == medicineDto.Name && m.Producer == medicineDto.Producer))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    pharmacy.Medicines.Add(new Medicine()
                    {
                        Name = medicineDto.Name,
                        Producer = medicineDto.Producer,
                        ExpiryDate = expiryDate,
                        ProductionDate = productionDate,
                        Category = (Category)medicineDto.Category,
                        Price = medicineDto.Price
                    });
                }

                pharmacies.Add(pharmacy);
                result.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));
            }

            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();

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
