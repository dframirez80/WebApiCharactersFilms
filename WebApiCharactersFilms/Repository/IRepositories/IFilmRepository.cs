using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.IRepositories
{
    public interface IFilmRepository : IRepository<Film>
    {
        Task<Film> GetFilmByTitleAsync(string title);
        Task<Film> GetFilmAllDataAsync(int id);
        Task<IEnumerable<Film>> GetFilmsOrderAsync(bool order);
        Task<IEnumerable<Film>> GetFilmsByContainTitleAsync(string title);
    }
}
