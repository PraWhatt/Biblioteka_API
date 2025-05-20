namespace LibraryApi.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public AuthorDto Author { get; set; }
    }
}
