using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class BookModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        [Key]
        public string? ISBN { get; set; }
        [DisplayName("Available")]
        public Boolean AvailabilityStatus { get; set; }

        public BookModel(int Id, string Title, Boolean Available)
        {
            this.Id = Id;
            this.Title = Title;
            this.AvailabilityStatus = Available;
        }
        public BookModel(string Title, string Author, string Genre, string ISBN, Boolean Available)
        {
            this.Title = Title;
            this.Author = Author;
            this.Genre = Genre;
            this.ISBN = ISBN;
            this.AvailabilityStatus = Available;
        }
        public BookModel()
        {

        }
    }
}
