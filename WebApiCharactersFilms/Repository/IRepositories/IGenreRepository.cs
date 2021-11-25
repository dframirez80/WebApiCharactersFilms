using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.IRepositories
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<Genre> GetGenreByNameAsync(string name);
        Task<Genre> GetGenreAllDataAsync(int id);
        Task<int> GetFirstGenreAsync();
    }
}
