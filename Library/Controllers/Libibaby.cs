using Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class Libibaby : Controller
    {
        private static List<BookModel> books = [];
        public async Task<IActionResult> Catalogue()
        {
            Database db = await Database.Create();
            books = await db.GetAllBooks();
            return View(books);
        }

        public async Task<IActionResult> Search(string searchQuery)
        {
            Database db = await Database.Create();
            books = await db.SearchByAll(searchQuery);
            return View("Catalogue", books);
        }

        public IActionResult Book(BookModel book)
        {
            return View(book);
        }

        [Route("~/Catalogue")]
        public async Task<IActionResult> CheckOrReturnBook(BookModel book)
        {
            Database db = await Database.Create();
            book.AvailabilityStatus = !book.AvailabilityStatus;
            await db.UpdateBook(book);
            books = await db.GetAllBooks();
            return View("Catalogue", books);
        }

        public IActionResult Add()
        {
            return View();
        }

        [Route("~/Catalogue")]
        public async Task<IActionResult> AddBook(BookModel book)
        {
            book.AvailabilityStatus = true;
            Database db = await Database.Create();
            await db.AddBook(book);
            books = await db.GetAllBooks();
            return View("Catalogue", books);
        }
    }
}
