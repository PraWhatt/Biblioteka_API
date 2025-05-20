using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("books")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;
        public BooksController(LibraryContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] int? authorId = null)
        {
            var booksQuery = _context.Books.Include(b => b.Author).AsQueryable();
            if (authorId.HasValue)
                booksQuery = booksQuery.Where(b => b.AuthorId == authorId);

            var books = await booksQuery
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Author = new AuthorDto
                    {
                        Id = b.Author.Id,
                        FirstName = b.Author.FirstName,
                        LastName = b.Author.LastName
                    }
                })
                .ToListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _context.Books.Include(b => b.Author)
                .Where(b => b.Id == id)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Author = new AuthorDto
                    {
                        Id = b.Author.Id,
                        FirstName = b.Author.FirstName,
                        LastName = b.Author.LastName
                    }
                })
                .FirstOrDefaultAsync();

            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title) || dto.AuthorId == 0)
                return BadRequest("Title and AuthorId are required.");
            if (dto.Year < 0)
                return BadRequest("Year cannot be negative.");

            var author = await _context.Authors.FindAsync(dto.AuthorId);
            if (author == null) return BadRequest("Author not found.");

            var book = new Book { Title = dto.Title, Year = dto.Year, AuthorId = dto.AuthorId };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                Author = new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                }
            };
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, bookDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title) || dto.AuthorId == 0)
                return BadRequest("Title and AuthorId are required.");
            if (dto.Year < 0)
                return BadRequest("Year cannot be negative.");

            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            var author = await _context.Authors.FindAsync(dto.AuthorId);
            if (author == null) return BadRequest("Author not found.");

            book.Title = dto.Title;
            book.Year = dto.Year;
            book.AuthorId = dto.AuthorId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
