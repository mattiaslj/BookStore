namespace BookStore.Models
{
    public interface IBook
    {
        string Title { get; }
        string Author { get; }
        decimal Price { get; }
    }
}
