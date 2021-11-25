using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Models.Dtos;

namespace WebApiCharactersFilms.Handlers
{
    public interface ICharactersHandler
    {
        Task<int> CreateCharacterAsync(CharacterCreateDto createDto);
        Task UpdateCharacterAsync(CharacterUpdateDto createDto);
        Task DeleteCharacterAsync(int id);
        Task<IEnumerable<CharacterResponseDto>> GetCharactersAsync();
        Task<CharacterResponseDto> GetCharacterAsync(int id);
        Task<CharacterDetailDto> GetCharacterWithFilmsAsync(int id);
        Task<CharacterDetailDto> GetCharacterByNameAsync(string name);

        Task<IEnumerable<CharacterResponseDto>> GetCharactersContainNameAsync(string name);
        Task<IEnumerable<CharacterResponseDto>> GetCharactersContainWeightAsync(double weight);
        Task<IEnumerable<CharacterResponseDto>> GetCharactersContainAgeAsync(int age);
        Task<IEnumerable<CharacterResponseDto>> GetCharactersContainFilmAsync(string film);
    }
}
