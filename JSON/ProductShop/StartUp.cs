using ProductShop.Data;
using Newtonsoft.Json;
using ProductShop.Models;
using Newtonsoft.Json.Serialization;
using ProductShop.DTOs.Export;
using Microsoft.EntityFrameworkCore;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            // 01.
            //string inputJson = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, inputJson));

            // 02.
            //string inputJson = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, inputJson));

            // 03.
            //string inputJson = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, inputJson));

            // 04.
            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, inputJson));

            // 05.
            //Console.WriteLine(GetProductsInRange(context));

            // 06.
            //Console.WriteLine(GetSoldProducts(context));

            // 07.
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            // 08.
            //Console.WriteLine(GetUsersWithProducts(context));
        }

        private static string JsonSerializeObject(object obj)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(obj, jsonSettings);
        }

        // 01.
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        // 02.
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }

        // 03.
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

            categories.RemoveAll(c => c.Name == null);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        // 04.
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoriesProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        // 05.
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .OrderBy(p => p.Price)
                .ToList();

            return JsonSerializeObject(products);
        }

        // 06.
        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new SellerWithProductDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new SoldProductDto
                    {
                        Name = p.Name,
                        Price = p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    })
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            return JsonSerializeObject(products);
        }

        // 07.
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categoriesByProducts = context.Categories
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = c.CategoriesProducts
                                    .Average(cp => cp.Product.Price)
                                    .ToString("f2"),
                    TotalRevenue = c.CategoriesProducts
                                    .Sum(cp => cp.Product.Price)
                                    .ToString("f2")
                })
                .OrderByDescending(pc => pc.ProductsCount)
                .ToList();

            return JsonSerializeObject(categoriesByProducts);
        }

        // 08.
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersWithProducts = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null && p.Price != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = u.ProductsSold
                                    .Where(p => p.BuyerId != null && p.Price != null)
                                    .Select(p => new
                                    {
                                        p.Name,
                                        p.Price
                                    })
                                    .ToList()
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToList();

            var output = new
            {
                UsersCount = usersWithProducts.Count,
                Users = usersWithProducts.Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.SoldProducts.Count,
                        Products = u.SoldProducts
                    }
                })
            };

            return JsonSerializeObject(output);
        }
    }
}