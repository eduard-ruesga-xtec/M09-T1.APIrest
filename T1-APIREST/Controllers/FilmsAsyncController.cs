using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using T1_APIREST.Context;
using T1_APIREST.DTO;
using T1_APIREST.Models;

namespace T1_APIREST.Controllers
{
    [Route("/api/Films")]
    [ApiController]
    public class FilmsAsyncController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmsAsyncController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Films
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
            return await _context.Films.ToListAsync();
        }

        // GET: api/Films/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var film = await _context.Films.FindAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            return film;
        }
        // POST: api/Films
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(FilmInsertDTO filmDTO)
        {
            var film = new Film{
                Name = filmDTO.Name,
                Description = filmDTO.Description,
                DirectorId = filmDTO.DirectorId
            };

            try
            {
                await _context.Films.AddAsync(film);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }


            return CreatedAtAction(nameof(GetFilm), new { id = film.ID }, film);
        }

        // DELETE: api/Films/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // PUT: api/Films/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutFilm(int id, Film film)
        {
            if (id != film.ID)
            {
                return BadRequest();
            }

            _context.Entry(film).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

     

        private bool FilmExists(int id)
        {
            return _context.Films.Any(e => e.ID == id);
        }
    }
}
