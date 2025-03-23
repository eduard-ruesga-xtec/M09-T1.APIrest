using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using T1_APIREST.Context;
using T1_APIREST.Models;
using T1_APIREST.DTO;

namespace T1_APIREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsSyncController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public FilmsSyncController(AppDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("get/{Id}")]       //El seu URI sera /api/FilmsSync/1
        public ActionResult<Film> Get(int Id)
        {
            var film = _dbContext.Films.FirstOrDefault(f => f.ID == Id);

            if (film == null)
            {
                return NotFound("La pelicula solicitada no esta en el cataleg");
            }
            return Ok(film);
        }

        [HttpGet("")]           //URI = /api/FilmsSync/
        public ActionResult<IEnumerable<Film>> GetAll()
        {
            var films = _dbContext.Films.ToList();
            if (films.Count == 0)
            {
                return NotFound("El cataleg esta buid!");
            }
            return Ok(films);
        }

        [HttpPost("insert")]
        public ActionResult<Film> PostFilm([FromBody]FilmInsertDTO film) //Al ser una dada composta no l'envia al header, l'envia pel body del response
        {
            //En aquest cas, no volem que ens omplin el ID ni Genre, hem creat un DTO expresament sense aquests camps
            //D'aquesta forma no els demanara. Tot i que no cal omplir-los
            var newFilm = new Film
            {
                Name = film.Name,
                Description = film.Description
            };
            var filmAdded = _dbContext.Films.Add(newFilm);
            _dbContext.SaveChanges();

            //Li pasem la resposta d'insercio (CreateAtAction()) i la URI que enviara la nova pelicula
            return CreatedAtAction(nameof (Get), new {id = newFilm.ID}, newFilm);

            //En ek header retornara -> location: https://localhost:7215/api/FilmsSync/8 
        }

        [HttpPut("put/{Id}")]
        public ActionResult<Film> PutFilm([FromRoute] int Id, [FromBody] FilmInsertDTO filmInsert)  //FromRoute: extreu la dada de la URI
        {
            var film = _dbContext.Films.FirstOrDefault(f => f.ID == Id);

            if (film == null)
            {
                return NotFound("La pelicula solicitada no esta en el cataleg");
            }

            film.Name = filmInsert.Name;
            film.Description = filmInsert.Description; 
            _dbContext.SaveChanges();

            return NoContent();
        }
        
        [HttpDelete("delete/{Id}")]
        public ActionResult<Film> DeleteFilm(int Id)
        {
            var film = _dbContext.Films.FirstOrDefault(f => f.ID == Id);

            if (film == null)
            {
                return NotFound("La pelicula solicitada no esta en el cataleg");
            }

            _dbContext.Films.Remove(film);
            _dbContext.SaveChanges();

            return NoContent();
        }
        

    }
}
