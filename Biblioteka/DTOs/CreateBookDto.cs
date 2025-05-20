namespace LibraryApi.DTOs
{
    public class CreateBookDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public int AuthorId { get; set; }
    }
}
