using BookStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private IBookstoreService bookstoreService;

        public CartController(IBookstoreService bookstoreService)
        {
            this.bookstoreService = bookstoreService;
        }

        public ActionResult ShowCart()
        {
            if (Session["books"] != null)
            {
                List<IBook> books = (List<IBook>)Session["books"];
                return PartialView("_Cart", books);
            }
            List<IBook> booksEmpty = new List<IBook>();
            return PartialView("_Cart", booksEmpty);
        }

        [HttpGet]
        public ActionResult AddToCart(string Author = "", string Title = "", decimal Price = 0)
        {
            if (!string.IsNullOrEmpty(Author) && !string.IsNullOrEmpty(Title) && Price != 0)
            {
                IBook book = new Book(Author, Price, Title);
                List<IBook> books;
                // if books are in session copy that list
                if (Session["books"] != null)
                {
                    books = (List<IBook>)Session["books"];
                }
                else
                {   // no books in session, make new list
                    books = new List<IBook>();
                }
                books.Add(book);
                Session["books"] = books;
                return PartialView("_Cart", books);
            }

            ViewBag.error = "Something went wrong";
            List<IBook> booksEmpty = new List<IBook>();
            return PartialView("_Cart", booksEmpty);
        }

        public ActionResult DeleteFromCart(string Author = "", string Title = "", decimal Price = 0)
        {
            List<IBook> booksEmpty;
            List<IBook> books;
            // if books are in session copy that list
            if (Session["books"] != null)
            {
                books = (List<IBook>)Session["books"];

                if (!string.IsNullOrEmpty(Author) && !string.IsNullOrEmpty(Title) && Price != 0)
                {
                    var bookToBeRemoved = books.Where(x => x.Author == Author)
                                                .Where(x => x.Price == Price)
                                                .Where(x => x.Title == Title)
                                                .FirstOrDefault();
                    // check to see if book is in cart
                    if (books.Contains(bookToBeRemoved))
                    {
                        // remove book, update session
                        books.Remove(bookToBeRemoved);
                        Session["books"] = books;
                        return PartialView("_Cart", books);
                    }
                }
                ViewBag.error = "Couldnt find book";
                return PartialView("_Cart", books);
            }
            else
            {   // no books in session, cant remove book
                ViewBag.error = "Something went wrong";
                booksEmpty = new List<IBook>();
                return PartialView("_Cart", booksEmpty);
            }
        }

        public ActionResult GetTotalPrice()
        {
            if (Session["books"] == null)
            {
                decimal zero = 0;
                return PartialView("_Price", zero);
            }

            List<IBook> books = (List<IBook>)Session["books"];
            decimal total = 0;

            foreach (var book in books)
            {
                total += book.Price;
            }
            return PartialView("_Price", total);
        }

        public async Task<ActionResult> BuyBooks()
        {
            if (Session["books"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            List<IBook> list = (List<IBook>)Session["books"];
            IEnumerable<IBook> boughtBooks = await bookstoreService.GetBoughtBooksAsync(list);

            return View(boughtBooks);
        }
    }
}