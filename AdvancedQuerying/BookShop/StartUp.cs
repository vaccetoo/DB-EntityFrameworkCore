namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics;
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
            //string input = Console.ReadLine();
            //Console.WriteLine(GetAuthorNamesEndingIn(context, input));

            // 08.
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBookTitlesContaining(context, input));

            // 09.
            //string input = Console.ReadLine();
            //Console.WriteLine(GetBooksByAuthor(context, input));

            // 10.
            //int lengthCheck = int.Parse(Console.ReadLine());
            //Console.WriteLine(CountBooks(context, lengthCheck));

            // 11.
            //Console.WriteLine(CountCopiesByAuthor(context));

            // 12.
            //Console.WriteLine(GetTotalProfitByCategory(context));

            // 13.
            //Console.WriteLine(GetMostRecentBooks(context));

            // 14.
            //IncreasePrices(context);

            // 15.
            //Console.WriteLine(RemoveBooks(context)); 

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
            
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .OrderBy(fn => fn)
                .ToList();

            return string.Join(Environment.NewLine, authors);
        }

        // 08.
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var bookTitles = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(bt => bt)
                .ToList();

            return string.Join(Environment.NewLine, bookTitles);
        }

        // 09.
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorName = $"{b.Author.FirstName} {b.Author.LastName}"
                })
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var book in books)
            {
                result.AppendLine($"{book.Title} ({book.AuthorName})");
            }

            return result.ToString().TrimEnd();
        }

        // 10.
        public static int CountBooks(BookShopContext context, int lengthCheck)
            => context.Books.Count(b => b.Title.Length > lengthCheck);

        // 11.
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorWithBooks = context.Authors
                .Select(a => new
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    BookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.BookCopies)
                .ToList();

            StringBuilder result = new StringBuilder();

            foreach (var book in authorWithBooks)
            {
                result.AppendLine($"{book.AuthorName} - {book.BookCopies}");
            }

            return result.ToString().TrimEnd();
        }

        // 12.
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoryProfits = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks.Sum(cb => cb.Book.Price * cb.Book.Copies)
                })
                .OrderByDescending(c => c.Profit)
                .ThenBy(c => c.Name)
                .ToArray();

                return string.Join(Environment.NewLine, categoryProfits.Select(cbp => $"{cbp.Name} ${cbp.Profit:f2}"));
        }

        // 13.
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var mostrecentBooks = context.Categories
                .Select(c => new
                {
                    c.Name,
                    MostRecentBooks = c.CategoryBooks
                    .OrderByDescending(b => b.Book.ReleaseDate)
                    .Take(3)
                    .Select(b => $"{b.Book.Title} ({b.Book.ReleaseDate.Value.Year})")
                })
                .ToList()
                .OrderBy(c => c.Name)
                .ToList();

            var result = new StringBuilder();

            foreach (var book in mostrecentBooks)
            {
                result.AppendLine($"--{book.Name}");

                foreach (var b in book.MostRecentBooks)
                {
                    result.AppendLine(b);
                }
            }

            return result.ToString().TrimEnd();
        }

        // 14.
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        // 15.
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.RemoveRange(books);
            context.SaveChanges();

            return books.Count();
        }
    }
}


