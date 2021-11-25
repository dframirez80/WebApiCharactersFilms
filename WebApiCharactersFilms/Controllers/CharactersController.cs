using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Handlers;
using WebApiCharactersFilms.Models;
using WebApiCharactersFilms.Models.Dtos;
using WebApiCharactersFilms.Repository.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiCharactersFilms.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize(Roles = Roles.User)]
    public class CharactersController : ControllerBase {
        private readonly ICharactersHandler _handler;
        public CharactersController(ICharactersHandler handler) {
            _handler = handler;
        }
        
        // GET: api/v1/<CharactersController>
        // GET: api/v1/<CharactersController>?name=nombre
        // GET: api/v1/<CharactersController>?age=edad
        // GET: api/v1/<CharactersController>?weight=peso
        // GET: api/v1/<CharactersController>?movies=movie
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CharacterResponseDto>>> GetCharactersAsync(
                                                                    [FromQuery] string name,
                                                                    [FromQuery] int? age,
                                                                    [FromQuery] double? weight,
                                                                    [FromQuery] string movies) {
            if (!string.IsNullOrEmpty(name)) 
                return Ok(await _handler.GetCharactersContainNameAsync(name));
            if (age.HasValue && age > Constraints.MinYearsOld && age < Constraints.MaxYearsOld)
                return Ok(await _handler.GetCharactersContainAgeAsync((int)age));
            if (weight.HasValue && weight > Constraints.MinWeight && weight < Constraints.MaxWeight)
                return Ok(await _handler.GetCharactersContainWeightAsync((double)weight));
            if (!string.IsNullOrEmpty(movies))
                return Ok(await _handler.GetCharactersContainFilmAsync(movies));
            return Ok(await _handler.GetCharactersAsync());
        }

        // GET api/v1/<CharactersController>/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CharacterDetailDto>> GetCharacterAsync(int id) {
            var ch = await _handler.GetCharacterWithFilmsAsync(id);
            if (ch == null)
                return NotFound();
            return Ok(ch);
        }

        // POST api/v1/<CharactersController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostCharacterAsync([FromForm] CharacterCreateDto createDto) {
            if (createDto == null) 
                BadRequest();
            if (createDto.File != null) { 
                if (createDto.File.Length > Constraints.MaxLengthFile) 
                    BadRequest(new { message = ErrorMessage.MaxLengthFile });
                var ext = createDto.File.ContentType;
                if (!ext.Contains(Constraints.Jpg) && !ext.Contains(Constraints.Jpge) && !ext.Contains(Constraints.Png))
                    BadRequest(new { message = ErrorMessage.Extensions });
            }

            var resp = await _handler.CreateCharacterAsync(createDto);
            if (resp == 0)
                return BadRequest(new { message = ErrorMessage.ExistsCharacter });
            return Created("GetCharacter", new { Id = resp });
        }

        // PUT api/v1/<CharactersController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult> PutCharacterAsync(int id, [FromForm] CharacterUpdateDto updateDto) {
            if (updateDto == null)
                BadRequest();
            if (updateDto.File != null) { 
                if (updateDto.File.Length > Constraints.MaxLengthFile)
                    BadRequest(new { message = ErrorMessage.MaxLengthFile } );
                var ext = updateDto.File.ContentType;
                if (ext.Contains(Constraints.Jpg) || ext.Contains(Constraints.Jpge) || ext.Contains(Constraints.Png))
                    BadRequest(new { message = ErrorMessage.Extensions });
            }
            var ch = await _handler.GetCharacterAsync(id);
            if(ch == null) 
                return BadRequest();
            await _handler.UpdateCharacterAsync(updateDto);
            return NoContent();
        }

        // DELETE api/v1/<CharactersController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult> DeleteCharacterAsync(int id) {
            await _handler.DeleteCharacterAsync(id);
            return NoContent();
        }

    }
}
