using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Models;
using WebApiCharactersFilms.Models.Dtos;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Handlers
{
    public interface IUsersHandler
    {
        Task<string> ConfirmUserAsync(int id, string token);
        Task<string> RegisterUserAsync(UserDto userDto, string host);
        Task<string> ChangeUserPasswordAsync(ChangePassword changePassword);
        Task<string> ResetUserPasswordAsync(ResetPassword email);
        Task<string> LoginUserAsync(Login login);
    }
}
