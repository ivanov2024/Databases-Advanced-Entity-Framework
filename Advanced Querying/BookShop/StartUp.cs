namespace BookShop
{
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            Console.WriteLine(GetMostRecentBooks(db));
        }

        //Task 02
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            bool isEnumValid = Enum
                .TryParse(command, true, out AgeRestriction ageRestriction);

            if (!isEnumValid) return String.Empty;

            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        //Task 03
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Copies < 5000 &&
                    b.EditionType == EditionType.Gold)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        //Task 04
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.Price > 40m)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookPrice = b.Price,
                })
                .OrderByDescending(b => b.BookPrice)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.BookTitle} - ${book.BookPrice:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 05
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.ReleaseDate.HasValue &&
                    b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        //Task 06
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            var books = context.Books
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .Where(b => b.BookCategories
                    .Any(bc => categories
                        .Contains(bc.Category.Name.ToLower())))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        //Task 07
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            DateTime targetDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            var books = context.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value < targetDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookEdition = b.EditionType,
                    BookPrice = b.Price,
                    BookReleaseDate = b.ReleaseDate.Value.ToString("dd-MM-yyyy")
                })
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.BookTitle} - {book.BookEdition} - ${book.BookPrice:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 08
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    authorFirstName = a.FirstName,
                    authorLastName = a.LastName,
                })
                .OrderBy(a => a.authorFirstName)
                .ThenBy(a => a.authorLastName)
                .ToArray();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.authorFirstName} {author.authorLastName}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 09
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            string pattern = $"%{input.ToLower()}%";

            var books = context.Books
                .Where(b => EF.Functions.Like(b.Title, pattern))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        //Task 10
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Include(b => b.Author)
                .Where(a => a.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})")
                .ToArray();

            foreach (var book in books)
            {
                sb.AppendLine(book);
            }

            return sb.ToString().TrimEnd();
        }

        //Task 11
        public static int CountBooks(BookShopContext context, int lengthCheck)
            => context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

        //Task 12
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var copies = context.Authors
                .Include(a => a.Books)
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    TotalCopies = a.Books
                        .Sum(b => b.Copies),
                })
                .OrderByDescending(a => a.TotalCopies)
                .ToArray();

            foreach (var copy in copies)
            {
                sb.AppendLine($"{copy.FirstName} {copy.LastName} - {copy.TotalCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 13
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var totalProfit = context.Categories
                .Include(c => c.CategoryBooks)
                .ThenInclude(b => b.Book)
                .Select(c => new
                {
                    Category = c.Name,
                    TotalProfit = c.CategoryBooks
                        .Sum(cb => cb.Book.Price * cb.Book.Copies)
                })
                .ToArray()
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Category)
                .ToArray();

            foreach (var item in totalProfit)
            {
                sb.AppendLine($"{item.Category} ${item.TotalProfit.ToString("F2")}");
            }

            return sb.ToString().TrimEnd();
        }

        //Task 14
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var categories = context.Categories
            .Include(c => c.CategoryBooks)
            .ThenInclude(cb => cb.Book)
            .OrderBy(c => c.Name)
            .Select(c => new
            {
                CategoryName = c.Name,
                Books = c.CategoryBooks
                .Where(cb => cb.Book.ReleaseDate.HasValue)
                .OrderByDescending(cb => cb.Book.ReleaseDate)
                .Take(3)
                .Select(cb => new
                {
                    cb.Book.Title,
                    ReleaseYear = cb.Book.ReleaseDate.Value.Year
                })
                .ToArray()
            })
            .ToArray();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Task 15
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.HasValue &&
                    b.ReleaseDate.Value.Year < 2010)
                .ToArray();
            
            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChangesAsync();
        }

        //Task 16
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            foreach (var book in books)
            {
                context.Books.Remove(book);
            }

            context.SaveChanges();

            return books.Length;
        }
    }
}


