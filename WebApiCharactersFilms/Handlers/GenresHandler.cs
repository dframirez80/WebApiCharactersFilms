using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Models.Dtos;
using WebApiCharactersFilms.Repository.Entities;
using WebApiCharactersFilms.Repository.IRepositories;

namespace WebApiCharactersFilms.Handlers
{
    public class GenresHandler : IGenresHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public GenresHandler(IUnitOfWork uow, IMapper mapper, IWebHostEnvironment env) {
            _uow = uow;
            _mapper = mapper;
            _env = env;
        }
        public async Task<int> CreateGenreAsync(GenreCreateDto createDto) {
            if (createDto == null) return 0;
            var genre = await _uow.Genres.GetGenreByNameAsync(createDto.Name);       // verifico si ya existe
            if (genre != null) return 0;
            genre = _mapper.Map<Genre>(createDto);
  
            if (createDto.File.Length > 0)
            {
                var pathFile = $"{_env.WebRootPath}\\Genres\\{createDto.File.FileName}";    // copia imagen
                using (var stream = new FileStream(pathFile, FileMode.Create))
                {
                    await createDto.File.CopyToAsync(stream);
                    genre.PathFile = pathFile;
                }
            }
            await _uow.Genres.CreateAsync(genre);                                     // creo genero
            await _uow.CommitAsync();
            return genre.GenreId;
        }

        public async Task DeleteGenreAsync(int id) {
            var genre = await _uow.Genres.GetAsync(id);
            if (genre == null) return;
            await _uow.Genres.DeleteAsync(id);
            await _uow.CommitAsync();
        }

        public async Task<GenreResponseDto> GetGenreAsync(int id) {
            var genre = await _uow.Genres.GetGenreAllDataAsync(id);
            return _mapper.Map<GenreResponseDto>(genre);
        }

        public async Task<int> GetFirstGenreAsync() {
            return await _uow.Genres.GetFirstGenreAsync();
        }

        public async Task<IEnumerable<GenreResponseDto>> GetGenresAsync() {
            var genre = await _uow.Genres.GetAllAsync();
            var responseDto = _mapper.Map<List<GenreResponseDto>>(genre);
            return responseDto;
        }

        public async Task UpdateGenreAsync(GenreUpdateDto createDto) {
            if (createDto == null) return;
            var genre = await _uow.Genres.GetAsync(createDto.GenreId);
            
            try
            {
                System.IO.File.Delete(genre.PathFile);              // elimina archivo si existe
            } catch (Exception) { }
            _uow.DiscardChanges();
            genre = _mapper.Map<Genre>(createDto);

            if (createDto.File.Length > 0)
            {
                var pathFile = $"{_env.WebRootPath}\\Genres\\{createDto.File.FileName}";    // copia imagen
                using (var stream = new FileStream(pathFile, FileMode.Create))
                {
                    await createDto.File.CopyToAsync(stream);
                    genre.PathFile = pathFile;
                }
            }

            await _uow.Genres.UpdateAsync(genre);                                     // actualizo
            await _uow.CommitAsync();
        }
    }
}
