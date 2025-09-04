using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EagerLoadingWebAPI.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }

        // Foreign key
        public int LibraryId { get; set; }

        // Navigation property
        //[ForeignKey(nameof(Library))]
        public Library? Library { get; set; }
    }
}
