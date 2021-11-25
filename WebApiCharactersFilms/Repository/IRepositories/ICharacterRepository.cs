using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.IRepositories
{
    public interface ICharacterRepository : IRepository<Character>
    {
        Task<Character> GetCharacterAllDataAsync(int id);
        Task<Character> GetCharacterByNameAsync(string name);
        Task<IEnumerable<Character>> GetCharactersByWeightAsync(double weight);
        Task<IEnumerable<Character>> GetCharactersByAgeAsync(int age);
        Task<IEnumerable<Character>> GetCharactersByContainNameAsync(string name);
    }
}
