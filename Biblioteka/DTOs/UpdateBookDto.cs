namespace LibraryApi.DTOs
{
    public class UpdateBookDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public int AuthorId { get; set; }
    }
}
