using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Models.Dtos;

namespace WebApiCharactersFilms.Handlers
{
    public interface IFilmsHandler
    {
        Task<int> CreateFilmAsync(FilmCreateDto createDto);
        Task UpdateFilmAsync(FilmUpdateDto createDto);
        Task DeleteFilmAsync(int id);
        Task<IEnumerable<FilmResponseDto>> GetFilmsAsync();
        Task<FilmResponseDto> GetFilmAsync(int id);
        Task<FilmDetailDto> GetFilmWithCharactersAsync(int id);

        Task<IEnumerable<FilmResponseDto>> GetFilmsContainTitleAsync(string title);
        Task<IEnumerable<FilmResponseDto>> GetFilmsByIdGenreAsync(int id);
        Task<IEnumerable<FilmResponseDto>> GetFilmsOrderAsync(bool order);
    }
}
