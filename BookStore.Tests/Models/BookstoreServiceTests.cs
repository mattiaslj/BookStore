using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BookStore.Models.Tests
{
    [TestClass()]
    public class BookstoreServiceTests
    {
        [TestMethod()]
        public void GetBooksAsyncTest()
        {
            // arrange

            BookstoreService bs = new BookstoreService();
            string empty = "";
            // act
            var result = bs.GetBooksAsync(empty);

            // assert

            //  Assert.IsInstanceOfType(result, Task<IEnumerable<IBook>>);
        }
    }
}
