using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Handlers;
using WebApiCharactersFilms.Models.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiCharactersFilms.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.User)]

    public class MoviesController : ControllerBase
    {
        private readonly IFilmsHandler _handler;
        public MoviesController(IFilmsHandler handler) {
            _handler = handler;
        }

        // GET: api/v1/<MoviesController>
        // GET: api/v1/<MoviesController>?title=title
        // GET: api/v1/<MoviesController>?genre=idGenero
        // GET: api/v1/<MoviesController>?order=ASC | DESC
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetFilmsAsync([FromQuery] string title,
                                                      [FromQuery] int? genre,
                                                      [FromQuery] string order) {
            if (!string.IsNullOrEmpty(title))
                return Ok(await _handler.GetFilmsContainTitleAsync(title));
            if (genre.HasValue)
                return Ok(await _handler.GetFilmsByIdGenreAsync((int)genre));
            if (!string.IsNullOrEmpty(order)) {
                if (order == Constraints.OrderASC)
                    return Ok(await _handler.GetFilmsOrderAsync(true));
                return Ok(await _handler.GetFilmsOrderAsync(false));
            }
            return Ok(await _handler.GetFilmsAsync());
        }

        // GET api/v1/<MoviesController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FilmDetailDto>> GetFilmAsync(int id) {
            var ch = await _handler.GetFilmWithCharactersAsync(id);
            if (ch == null)
                return NotFound();
            return Ok(ch);
        }

        // POST api/v1/<MoviesController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostFilmAsync([FromForm] FilmCreateDto createDto) {
            if (createDto == null)
                BadRequest();
            if (createDto.File != null) {
                if (createDto.File.Length > Constraints.MaxLengthFile)
                    BadRequest(ErrorMessage.MaxLengthFile);
                var ext = createDto.File.ContentType;
                if (!ext.Contains(Constraints.Jpg) && !ext.Contains(Constraints.Jpge) && !ext.Contains(Constraints.Png))
                    BadRequest(ErrorMessage.Extensions);
            }
            var resp = await _handler.CreateFilmAsync(createDto);
            if (resp == 0)
                return BadRequest(new { message = ErrorMessage.ExistsFilm});
            return Created("GetFilm", new { Id = resp });
        }

        // PUT api/v1/<MoviesController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PutFilmAsync(int id, [FromForm] FilmUpdateDto updateDto) {
            if (updateDto == null)
                BadRequest();
            if (updateDto.File != null) { 
                if (updateDto.File.Length > Constraints.MaxLengthFile)
                    BadRequest(ErrorMessage.MaxLengthFile);
                var ext = updateDto.File.ContentType;
                if (ext.Contains(Constraints.Jpg) || ext.Contains(Constraints.Jpge) || ext.Contains(Constraints.Png))
                    BadRequest(ErrorMessage.Extensions);
            }
            var ch = await _handler.GetFilmAsync(id);
            if (ch == null)
                return BadRequest();
 
            await _handler.UpdateFilmAsync(updateDto);
            return NoContent();
        }


        // DELETE api/v1/<MoviesController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteFilmAsync(int id) {
            await _handler.DeleteFilmAsync(id);
            return NoContent();
        }

    }
}
