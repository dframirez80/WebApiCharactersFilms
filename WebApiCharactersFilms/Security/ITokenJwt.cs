using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Security
{
    public interface ITokenJwt
    {
        string GenerateToken(User user, int expiresMinutes);
        int ValidateToken(string token);

    }
}
