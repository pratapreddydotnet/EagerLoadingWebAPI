using EagerLoadingWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EagerLoadingWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LibraryController(AppDbContext context) => _context = context;

        // GET: api/libraries (EAGER LOAD Books)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
        {
            var libraries = await _context.Libraries
                .Include(l => l.Books)// Eager loading
                .AsNoTracking()
                .ToListAsync();

            return Ok(libraries);
        }

        // GET: api/libraries/5 (EAGER LOAD Books)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Library>> GetLibrary(int id)
        {
            var library = await _context.Libraries
                .Include(l => l.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.LibraryId == id);

            return library is null ? NotFound() : Ok(library);
        }

        // POST: api/libraries
        [HttpPost]
        public async Task<ActionResult<Library>> CreateLibrary([FromBody] Library library)
        {
            _context.Libraries.Add(library);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLibrary), new { id = library.LibraryId }, library);
        }

        // PUT: api/libraries/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLibrary(int id, [FromBody] Library library)
        {
            if (id != library.LibraryId) return BadRequest("ID mismatch.");

            _context.Entry(library).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Libraries.AnyAsync(l => l.LibraryId == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/libraries/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLibrary(int id)
        {
            var library = await _context.Libraries.FindAsync(id);
            if (library is null) return NotFound();

            _context.Libraries.Remove(library);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
