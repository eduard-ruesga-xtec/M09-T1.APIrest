﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using T1_APIREST.Context;
using T1_APIREST.DTO;
using T1_APIREST.Models;

namespace T1_APIREST.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DirectorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DirectorsController> _logger;

        public DirectorsController(AppDbContext context, ILogger<DirectorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        ///     Prova per a comprovar claims del Token. Only development enviromment
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Editor")]
        [HttpGet("prova")]
        public IActionResult ProvaToken()
        {
            return Ok(new
            {
                Usuari = User.Identity?.Name,
                Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Rol = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        // GET: api/Directors
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Director>>> GetDirectors()
        {
            return await _context.Directors.ToListAsync();
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Director>> GetDirector(int id)
        {
            var director = await _context.Directors.FindAsync(id);

            if (director == null)
            {
                return NotFound();
            }

            return director;
        }

        // PUT: api/Directors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirector(int id, Director director)
        {
            if (id != director.Id)
            {
                return BadRequest();
            }

            _context.Entry(director).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorExists(id))
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

        // POST: api/Directors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Director>> PostDirector(DirectorInsertDTO directorDTO)
        {
            var director = new Models.Director
            {
                Name = directorDTO.Name,
                Surname = directorDTO.Surname
            };

            try
            {
                var result = await _context.Directors.AddAsync(director);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un director");
                return BadRequest("Error d'inserció");
            }

            return CreatedAtAction("GetDirector", new { id = director.Id }, director);
        }

        // DELETE: api/Directors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DirectorExists(int id)
        {
            return _context.Directors.Any(e => e.Id == id);
        }
    }
}
