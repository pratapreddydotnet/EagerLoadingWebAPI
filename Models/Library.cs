namespace EagerLoadingWebAPI.Models
{
    public class Library
    {
        public int LibraryId { get; set; }
        public string Name { get; set; }

        // Navigation property
        public ICollection<Book> Books { get; set; }
    }
}
