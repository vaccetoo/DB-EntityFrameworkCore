using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Helpers;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            var customersDto = XmlSerializationHelper.Deserialize<List<ImportCustomerDto>>(xmlString, "Customers");

            var result = new StringBuilder();

            List<Customer> customers = new List<Customer>();

            foreach (var customerDto in customersDto)
            {
                if (!IsValid(customerDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                if (context.Customers.Any(c => (customerDto.FullName == c.FullName) || 
                (customerDto.PhoneNumber == c.PhoneNumber) ||
                (customerDto.Email == c.Email)))
                {
                    result.AppendLine(DuplicationDataMessage);
                    continue;
                }

                if (customers.Any(c => (customerDto.FullName == c.FullName) ||
                (customerDto.PhoneNumber == c.PhoneNumber) ||
                (customerDto.Email == c.Email)))
                {
                    result.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Customer customer = new Customer()
                {
                    FullName = customerDto.FullName,
                    Email = customerDto.Email,
                    PhoneNumber = customerDto.PhoneNumber
                };

                customers.Add(customer);
                result.AppendLine(string.Format(SuccessfullyImportedCustomer, customer.FullName));
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                //NullValueHandling = NullValueHandling.Ignore,
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                DateFormatString = "yyyy-MM-dd"
            };

            var bookingsDto = JsonConvert.DeserializeObject<List<ImportBookingsDto>>(jsonString, jsonSettings);

            var result = new StringBuilder();

            List<Booking> bookings = new List<Booking>();

            foreach (var bookingDto in bookingsDto)
            {
                if (!IsValid(bookingDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime currentDate;
                bool isCurrentDateValid =
                    DateTime.TryParseExact(bookingDto.BookingDate,
                    "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out currentDate);

                if (!isCurrentDateValid)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var customer = context.Customers.FirstOrDefault(c => c.FullName == bookingDto.CustomerName);
                var tourPackage = context.TourPackages.FirstOrDefault(tp => tp.PackageName == bookingDto.TourPackageName);

                if (tourPackage == null || customer == null)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Booking booking = new Booking()
                {
                    BookingDate = currentDate,
                    Customer = customer,
                    TourPackage = tourPackage,
                };

                bookings.Add(booking);
                result.AppendLine(string.Format(SuccessfullyImportedBooking, booking.TourPackage.PackageName, booking.BookingDate.ToString("yyyy-MM-dd")));
            }

            context.Bookings.AddRange(bookings);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
