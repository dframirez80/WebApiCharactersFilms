using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Models.Dtos;

namespace WebApiCharactersFilms.Handlers
{
    public interface IGenresHandler
    {
        Task<int> CreateGenreAsync(GenreCreateDto createDto);
        Task UpdateGenreAsync(GenreUpdateDto createDto);
        Task DeleteGenreAsync(int id);
        Task<IEnumerable<GenreResponseDto>> GetGenresAsync();
        Task<int> GetFirstGenreAsync();
        Task<GenreResponseDto> GetGenreAsync(int id);
    }
}
