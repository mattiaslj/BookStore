using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public interface IBookstoreService
    {
        Task<IEnumerable<IBook>> GetBooksAsync(string searchString);
        Task<IEnumerable<IBook>> GetBoughtBooksAsync(List<IBook> orderedBooks);
    }
}
