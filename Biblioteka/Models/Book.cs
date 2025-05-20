using System.Collections.Generic;
using Biblioteka.Models;

namespace LibraryApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<Copy> Copies { get; set; } = new List<Copy>();
    }
}
