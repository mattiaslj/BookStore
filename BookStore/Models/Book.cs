namespace BookStore.Models
{
    public class Book : IBook
    {
        public Book(string Author, decimal Price, string Title)
        {
            this.Author = Author;
            this.Price = Price;
            this.Title = Title;
        }

        public string Author
        {
            get;
        }

        public decimal Price
        {
            get;
        }

        public string Title
        {
            get;
        }
    }
}