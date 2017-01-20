using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BookStore.Models
{
    public class BookstoreService : IBookstoreService
    {
        private string URI = "http://www.contribe.se/arbetsprov-net/books.json";
        public Task<IEnumerable<IBook>> GetBooksAsync(string searchString)
        {
            return Task<IEnumerable<IBook>>.Factory.StartNew(() =>
            {
                string booksJson = GetBooksFromUrl(URI);
                return JsonToBooks(booksJson, searchString);
            });

        }

        public Task<IEnumerable<IBook>> GetBoughtBooksAsync(List<IBook> orderedBooks)
        {
            return Task<IEnumerable<IBook>>.Factory.StartNew(() =>
            {
                string booksAsJson = GetBooksFromUrl(URI);
                // Deserialize json string
                JavaScriptSerializer js = new JavaScriptSerializer();
                dynamic books = js.DeserializeObject(booksAsJson);

                // check that each book is in stock and correct order to not excede number in stock
                for (int i = 0; i < orderedBooks.Count; i++)
                {
                    IBook book = orderedBooks[i];
                    int inStock = 0;
                    int inCart = 0;
                    foreach (var item in books["books"])
                    {
                        if (item["author"] == book.Author
                        && item["title"] == book.Title
                        && item["price"] == book.Price)
                        {
                            inStock = item["inStock"];
                            inCart = orderedBooks.Where(x => x.Author == book.Author)
                                                .Where(x => x.Price == book.Price)
                                                .Where(x => x.Title == book.Title).Count();

                            if (inCart > inStock)
                            {
                                int difference = inCart - inStock;
                                for (int j = 0; j < difference; j++)
                                {
                                    var bookToBeRemoved = orderedBooks.Where(x => x.Author == book.Author)
                                                    .Where(x => x.Price == book.Price)
                                                    .Where(x => x.Title == book.Title)
                                                    .FirstOrDefault();
                                    orderedBooks.Remove(bookToBeRemoved);
                                }
                                break;
                            }
                        }
                    }
                }
                return orderedBooks;
            });
        }

        // Returns a list of books in json
        private string GetBooksFromUrl(string URI)
        {
            using (WebClient wc = new WebClient())
            {
                // Get booklist from url
                wc.Encoding = Encoding.UTF8;
                var booksJson = wc.DownloadString(URI);
                return booksJson;
            }
        }

        // Takes json string and converts it into a c# object that can implement IBook
        private IEnumerable<IBook> JsonToBooks(string booksJson, string searchString)
        {
            // Deserialize json string
            JavaScriptSerializer js = new JavaScriptSerializer();
            dynamic books = js.DeserializeObject(booksJson);

            // loop over all books and store them in a list
            List<IBook> bookstore = new List<IBook>();
            foreach (var item in books["books"])
            {
                // check if books contain chars from searchString
                if (item["author"].ToLower().Contains(searchString.ToLower())
                    || item["title"].ToLower().Contains(searchString.ToLower()))
                {
                    IBook book = new Book(item["author"], item["price"], item["title"]);
                    bookstore.Add(book);
                }
            }
            return bookstore;
        }
    }
}