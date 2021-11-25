using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.IRepositories
{
    public interface ICharacterFilmRepository
    {
        Task CreateAsync(CharacterFilm entity);
        Task<IEnumerable<CharacterFilm>> GetAllAsync();
        Task<CharacterFilm> GetAsync(int id);
        Task UpdateAsync(CharacterFilm entity);
        Task DeleteAsync(int id);
        Task DeleteAsync(CharacterFilm entity);
    }
}
