using BookStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private IBookstoreService bookstoreService;

        public HomeController(IBookstoreService bookstoreService)
        {
            this.bookstoreService = bookstoreService;
        }

        public HomeController()
        {
        }

        public async Task<ActionResult> Index(string searchString = "")
        {
            // Get all books
            IEnumerable<IBook> books = await bookstoreService.GetBooksAsync(searchString);
            if (books != null)
            {
                return View(books);
            }

            ViewBag.error = "No books found";
            return View();
        }

        public ActionResult BackToStore()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}