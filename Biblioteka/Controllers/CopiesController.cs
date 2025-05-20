using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("copies")]
    public class CopiesController : ControllerBase
    {
        private readonly LibraryContext _context;
        public CopiesController(LibraryContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CopyDto>>> GetCopies()
        {
            var copies = await _context.Copies
                .Select(c => new CopyDto { Id = c.Id, BookId = c.BookId })
                .ToListAsync();
            return Ok(copies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CopyDto>> GetCopy(int id)
        {
            var copy = await _context.Copies.FindAsync(id);
            if (copy == null) return NotFound();
            return Ok(new CopyDto { Id = copy.Id, BookId = copy.BookId });
        }

        [HttpPost]
        public async Task<ActionResult<CopyDto>> CreateCopy([FromBody] CreateCopyDto dto)
        {
            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null)
                return BadRequest("Book not found.");

            var copy = new Copy { BookId = dto.BookId };
            _context.Copies.Add(copy);
            await _context.SaveChangesAsync();

            var copyDto = new CopyDto { Id = copy.Id, BookId = copy.BookId };
            return CreatedAtAction(nameof(GetCopy), new { id = copy.Id }, copyDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCopy(int id, [FromBody] CreateCopyDto dto)
        {
            var copy = await _context.Copies.FindAsync(id);
            if (copy == null) return NotFound();
            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null) return BadRequest("Book not found.");

            copy.BookId = dto.BookId;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCopy(int id)
        {
            var copy = await _context.Copies.FindAsync(id);
            if (copy == null) return NotFound();
            _context.Copies.Remove(copy);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
