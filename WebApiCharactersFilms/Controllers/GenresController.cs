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

    public class GenresController : ControllerBase
    {
        private readonly IGenresHandler _handler;

        public GenresController(IGenresHandler handler) {
            _handler = handler;
        }

        // GET: api/v1/<GenresController>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetGenresAsync() {
            return Ok(await _handler.GetGenresAsync());
        }

        // GET api/v1/<GenresController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GenreResponseDto>> GetGenreAsync(int id) {
            var ch = await _handler.GetGenreAsync(id);
            if (ch == null)
                return NotFound();
            return Ok(ch);
        }

        // POST api/v1/<GenresController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostGenreAsync([FromForm] GenreCreateDto createDto) {
            if (createDto == null)
                BadRequest();
            if (createDto.File != null) {
                if (createDto.File.Length > Constraints.MaxLengthFile)
                    BadRequest(ErrorMessage.MaxLengthFile);
                var ext = createDto.File.ContentType;
                if (!ext.Contains(Constraints.Jpg) && !ext.Contains(Constraints.Jpge) && !ext.Contains(Constraints.Png))
                    BadRequest(ErrorMessage.Extensions);
            }

            var resp = await _handler.CreateGenreAsync(createDto);
            if (resp == 0)
                return BadRequest(new { message = ErrorMessage.ExistsGenre});
            return Created("GetGenre", new { Id = resp });
        }

        // PUT api/v1/<GenresController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PutGenreAsync(int id, [FromForm] GenreUpdateDto updateDto) {
            if (updateDto == null)
                BadRequest();
            if (updateDto.File != null) {
                if (updateDto.File.Length > Constraints.MaxLengthFile)
                    BadRequest(ErrorMessage.MaxLengthFile);
                var ext = updateDto.File.ContentType;
                if (ext.Contains(Constraints.Jpg) || ext.Contains(Constraints.Jpge) || ext.Contains(Constraints.Png))
                    BadRequest(ErrorMessage.Extensions);
            }
            var ch = await _handler.GetGenreAsync(id);
            if (ch == null)
                return BadRequest();
 
            await _handler.UpdateGenreAsync(updateDto);
            return NoContent();
        }

        // DELETE api/v1/<GenresController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteGenreAsync(int id) {
            await _handler.DeleteGenreAsync(id);
            return NoContent();
        }

    }
}
