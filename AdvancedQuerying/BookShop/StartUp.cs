namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            // 01.
            //string command = Console.ReadLine();
            //Console.WriteLine(GetBooksByAgeRestriction(context, command));

            // 02.
            //Console.WriteLine(GetGoldenBooks(context));

            // 03.
            //Console.WriteLine(GetBooksByPrice(context));

            // 04.
            //int year = int.Parse(Console.ReadLine());
            //Console.WriteLine(GetBooksNotReleasedIn(context, year));

            // 05.
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByCategory(context, input));

            // 06.
            //string date = Console.ReadLine();
            //Console.WriteLine(GetBooksReleasedBefore(context, date));

            // 07.
            string input = Console.ReadLine();
            Console.WriteLine(GetAuthorNamesEndingIn(context, input));
        }

        // 01.
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if(!Enum.TryParse(command, true, out AgeRestriction ageRestriction))
            {
                return $"Wrong command !";
            }

            var booksTitles = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            return string.Join(Environment.NewLine, booksTitles);
        }

        // 02.
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var goldenEditionBooks = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var book in goldenEditionBooks)
            {
                stringBuilder.AppendLine(book.Title);
            }

            return stringBuilder.ToString().TrimEnd();
        }

        // 03.
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var book in books)
            {
                result.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return result.ToString().TrimEnd();
        }

        // 04.
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }

        // 05.
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .ToLower()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var booksByCategory = context.BooksCategories
                .Where(bc => categories.Contains(bc.Category.Name.ToLower()))
                .Select(bc => bc.Book.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, booksByCategory);
        }

        // 06.
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dt = DateTime
                .ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < dt)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        // 07.
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            //TODO: ...

            return "";
        }
    }
}


