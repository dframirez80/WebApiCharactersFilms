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
    public class FilmsHandler : IFilmsHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public FilmsHandler(IUnitOfWork uow, IMapper mapper, IWebHostEnvironment env) {
            _uow = uow;
            _mapper = mapper;
            _env = env;
        }

        public async Task<int> CreateFilmAsync(FilmCreateDto createDto) {
            if (createDto == null) return 0;
            var film = await _uow.Films.GetFilmByTitleAsync(createDto.Title);       // verifico si ya existe
            if(film != null) return 0;
            film = _mapper.Map<Film>(createDto);

            if (createDto.File != null) {
                if (createDto.File.Length > 0)
                {
                    var pathFile = $"{_env.WebRootPath}\\Films\\{createDto.File.FileName}";    // copia imagen
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await createDto.File.CopyToAsync(stream);
                        film.PathFile = pathFile;
                    }
                }
            }

            await _uow.Films.CreateAsync(film);                                     // creo film
            //await _uow.CommitAsync();

            foreach (var name in createDto.Characters)
            {
                var ch = await _uow.Characters.GetCharacterByNameAsync(name);       // verifica si existe personaje
                if (ch == null)
                {
                    ch = new Character() { Name = name };                           // creo personaje
                    await _uow.Characters.CreateAsync(ch);
                }
                await _uow.CommitAsync();
                var cf = new CharacterFilm();
                cf.CharacterId = ch.CharacterId;
                cf.FilmId = film.FilmId;
                await _uow.CharacterFilms.CreateAsync(cf);
                await _uow.CommitAsync();
            }
            return film.FilmId;
        }

        public async Task DeleteFilmAsync(int id) {
            var film = await _uow.Films.GetFilmAllDataAsync(id);
            if (film == null) return;
            foreach (var item in film.CharacterFilms)
            {
                await _uow.CharacterFilms.DeleteAsync(item);
            }
            await _uow.Films.DeleteAsync(id);
            await _uow.CommitAsync();
        }

        public async Task<IEnumerable<FilmResponseDto>> GetFilmsAsync() {
            var films = await _uow.Films.GetAllAsync();
            var responseDto = _mapper.Map<List<FilmResponseDto>>(films);
            return responseDto;
        }

        public async Task<FilmResponseDto> GetFilmAsync(int id) {
            var films = await _uow.Characters.GetAsync(id);
            return _mapper.Map<FilmResponseDto>(films);
        }

        public async Task<FilmDetailDto> GetFilmWithCharactersAsync(int id) {
            var film = await _uow.Films.GetAsync(id);
            var filmDetailDto = _mapper.Map<FilmDetailDto>(film);
            filmDetailDto.Genre = film.Genre.Name;
            var characters = film.CharacterFilms;
            foreach (var item in characters)
            {
                filmDetailDto.Characters.Add(item.Character.Name);
            }
            return filmDetailDto;
        }

        public async Task UpdateFilmAsync(FilmUpdateDto createDto) {
            if (createDto == null) return;
            var film = await _uow.Films.GetFilmAllDataAsync(createDto.FilmId);

            try{
                System.IO.File.Delete(film.PathFile);              // elimina archivo si existe
            } catch (Exception) { }
            _uow.DiscardChanges();

            film = _mapper.Map<Film>(createDto);
            if (createDto.File != null) {
                if (createDto.File.Length > 0)
                {
                    var pathFile = $"{_env.WebRootPath}\\Films\\{createDto.File.FileName}";    // copia imagen
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await createDto.File.CopyToAsync(stream);
                        film.PathFile = pathFile;
                    }
                }
            }

            await _uow.Films.UpdateAsync(film);                                     // actualizo film

            foreach (var cf in film.CharacterFilms)
            {
                await _uow.CharacterFilms.DeleteAsync(cf);
            }
            await _uow.CommitAsync();
            foreach (var name in createDto.Characters)
            {
                var ch = await _uow.Characters.GetCharacterByNameAsync(name);       // verifica si existe personaje
                if (ch == null) {
                    ch = new Character() { Name = name };                           // creo personaje
                    await _uow.Characters.CreateAsync(ch);
                    await _uow.CommitAsync();
                }
                var cf = new CharacterFilm();
                cf.CharacterId = ch.CharacterId;
                cf.FilmId = film.FilmId;
                await _uow.CharacterFilms.CreateAsync(cf);
                await _uow.CommitAsync();
            }
        }

        public async Task<IEnumerable<FilmResponseDto>> GetFilmsContainTitleAsync(string title) {
            var films = await _uow.Films.GetFilmsByContainTitleAsync(title);
            var responseDto = _mapper.Map<List<FilmResponseDto>>(films);
            return responseDto; 
        }

        public async Task<IEnumerable<FilmResponseDto>> GetFilmsByIdGenreAsync(int id) {
            List<FilmResponseDto> responseDto = new();

            var g = await _uow.Genres.GetGenreAllDataAsync(id);
            if (g == null) return responseDto;

            var films = g.Films;
            if (films.Count() > 0) { 
                foreach (var item in films)
                {
                    FilmResponseDto resp = new();
                    resp.FilmId = item.FilmId;
                    resp.Title = item.Title;
                    resp.PathFile = item.PathFile;
                    responseDto.Add(resp);
                }
            }
            return responseDto;
        }

        public async Task<IEnumerable<FilmResponseDto>> GetFilmsOrderAsync(bool order) {
            var films = await _uow.Films.GetFilmsOrderAsync(order);
            var responseDto = _mapper.Map<List<FilmResponseDto>>(films);
            return responseDto;
        }
    }
}
