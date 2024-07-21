using CarDealer.Data;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();

            // 01.
            //string inputJson = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, inputJson));

            // 02.
            string inputJson = File.ReadAllText("../../../Datasets/parts.json");
            Console.WriteLine(ImportParts(context, inputJson));
        }

        private static string JsonSerializeObject(object obj)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                //NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(obj, jsonSettings);
        }

        // 01.
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        // 02.
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var vallidIds = context.Suppliers
                .Select(s => s.Id)
                .ToList();

            var vallidParts = parts
                .Where(p => vallidIds.Contains(p.SupplierId))
                .ToList();

            context.Parts.AddRange(vallidParts);
            context.SaveChanges();

            return $"Successfully imported {vallidParts.Count}.";

        }
    }
}