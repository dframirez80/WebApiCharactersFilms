using Microsoft.AspNetCore.Authorization;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiCharactersFilms.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersHandler _handler;
        public AuthController(IUsersHandler handler) {
            _handler = handler;
        }

        // GET: api/v1/<AuthController>
        [HttpGet("confirm/{id}/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetConfirmAsync(int id, string token) {
            var resp = await _handler.ConfirmUserAsync(id, token);
            if (resp == Constants.Mail.ConfirmFail)
                return BadRequest(new { message = resp});
            return Ok(resp);
        }

         // POST api/v1/<AuthController>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PostRegisterAsync([FromBody] UserDto userDto) {
            if (userDto == null)
                return BadRequest();
            var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Constants.Mail.AddressConfirm}";
            var resp = await _handler.RegisterUserAsync(userDto, host);
            if (resp == string.Empty)
                return BadRequest();
            if(resp == ErrorMessage.UserExists)
                return BadRequest(new { message = resp });
            return Created("Create", new { message = resp });
        }

        // POST api/v1/<AuthController>
        [HttpPost("forgot")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> PostResetPasswordAsync([FromBody] ResetPassword mail) {
            if (string.IsNullOrEmpty(mail.Email) || mail == null)
                return BadRequest();

            var resp = await _handler.ResetUserPasswordAsync(mail);

            if (resp == Constants.Mail.NewPassword)
                return Ok(new { message = resp });
            return BadRequest(new { message = resp});
        }

        // POST api/v1/<AuthController>
        [HttpPost("changePassword")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> PostChangePasswordAsync([FromBody] ChangePassword change) {
            if (change == null)
                return BadRequest();
            var resp = await _handler.ChangeUserPasswordAsync(change);
            if (resp == ErrorMessage.EmailOrPassword)
                return BadRequest(new{ message = resp});
            return Ok(new { message = resp });
        }

        // POST: api/v1/Login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostUserLoginAsync(Login login) {
            if (login == null)
                return BadRequest();
            var response = await _handler.LoginUserAsync(login);
            if (response == ErrorMessage.UserNotLogin || response == ErrorMessage.ResetPassword || response == ErrorMessage.UserPending)
                return BadRequest(new { message = response });
            return Ok(new { token = response });
        }
    }
}
