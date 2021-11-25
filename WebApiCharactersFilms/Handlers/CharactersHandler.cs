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
    public class CharactersHandler : ICharactersHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CharactersHandler(IUnitOfWork uow, IMapper mapper, IWebHostEnvironment env) {
            _uow = uow;
            _mapper = mapper;
            _env = env;
        }

        public async Task<int> CreateCharacterAsync(CharacterCreateDto createDto) {
            if (createDto == null) return 0;
            var character = await _uow.Characters.GetCharacterByNameAsync(createDto.Name); // verifico si ya existe
            if(character != null) return 0;
            character = _mapper.Map<Character>(createDto);
            if (createDto.File != null) {
                if (createDto.File.Length > 0)
                {
                    var pathFile = $"{_env.WebRootPath}\\Characters\\{createDto.File.FileName}";    // copia imagen
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await createDto.File.CopyToAsync(stream);
                        character.PathFile = pathFile;
                    }
                }
            }

            await _uow.Characters.CreateAsync(character);                                   // creo personaje
            //await _uow.CommitAsync();
            var genreId = await _uow.Genres.GetFirstGenreAsync();
            foreach (var title in createDto.Films)
            {
                var f = await _uow.Films.GetFilmByTitleAsync(title);                        // verifica si existe film
                if (f == null){
                    f = new Film() { Title = title, GenreId = genreId };                    // creo film
                    await _uow.Films.CreateAsync(f);
                }
                await _uow.CommitAsync();
                var cf = new CharacterFilm();
                cf.CharacterId = character.CharacterId;
                cf.FilmId = f.FilmId;
                await _uow.CharacterFilms.CreateAsync(cf);
                await _uow.CommitAsync();
            }
            return character.CharacterId;
        }

        public async Task DeleteCharacterAsync(int id) {
            var character = await _uow.Characters.GetCharacterAllDataAsync(id);
            if (character == null) return;
            foreach (var item in character.CharacterFilms)
            {
                await _uow.CharacterFilms.DeleteAsync(item);
            }
            try{
                System.IO.File.Delete(character.PathFile);  // elimina archivo si existe
            } catch (Exception) { }
            await _uow.Characters.DeleteAsync(id);
            await _uow.CommitAsync();
        }

        public async Task<IEnumerable<CharacterResponseDto>> GetCharactersAsync() {
            var characters = await _uow.Characters.GetAllAsync();
            var responseDto = _mapper.Map<List<CharacterResponseDto>>(characters);
            return responseDto;
        }

        public async Task<CharacterResponseDto> GetCharacterAsync(int id) {
            var characters = await _uow.Characters.GetAsync(id);
            if (characters == null)
                return null;
            return _mapper.Map<CharacterResponseDto>(characters);
        }

        public async Task<CharacterDetailDto> GetCharacterWithFilmsAsync(int id) {
            var characters = await _uow.Characters.GetAsync(id);
            if (characters == null)
                return null; 
            var characterDetailDto = _mapper.Map<CharacterDetailDto>(characters);
            var film = characters.CharacterFilms;
            foreach (var item in film)
            {
                characterDetailDto.Films.Add(item.Film.Title);
            }
            return characterDetailDto;
        }

        public async Task<CharacterDetailDto> GetCharacterByNameAsync(string name) {
            var characters = await _uow.Characters.GetCharacterByNameAsync(name);
            if (characters == null) 
                return null;
            characters = await _uow.Characters.GetCharacterAllDataAsync(characters.CharacterId);
            var characterDetailDto = _mapper.Map<CharacterDetailDto>(characters);
            var film = characters.CharacterFilms;
            foreach (var item in film)
            {
                characterDetailDto.Films.Add(item.Film.Title);
            }
            return characterDetailDto;
        }
        public async Task UpdateCharacterAsync(CharacterUpdateDto createDto) {
            if (createDto == null) return;
            var character = await _uow.Characters.GetCharacterAllDataAsync(createDto.Id);
            
            try{
                System.IO.File.Delete(character.PathFile);              // elimina archivo si existe
            } catch (Exception) { }
            _uow.DiscardChanges();

            character = _mapper.Map<Character>(createDto);

            if (createDto.File != null) {
                if (createDto.File.Length > 0)
                {
                    var pathFile = $"{_env.WebRootPath}\\Characters\\{createDto.File.FileName}";    // copia imagen
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await createDto.File.CopyToAsync(stream);
                        character.PathFile = pathFile;
                    }
                }
            }

            await _uow.Characters.UpdateAsync(character);               // actualizo personaje

            foreach (var cf in character.CharacterFilms)
            {
                await _uow.CharacterFilms.DeleteAsync(cf);
            }
            await _uow.CommitAsync();
            foreach (var title in createDto.Films)
            {
                var f = await _uow.Films.GetFilmByTitleAsync(title);                        // verifica si existe film
                if (f == null) {
                    f = new Film() { Title = title };                                       // creo film
                    await _uow.Films.CreateAsync(f);
                    await _uow.CommitAsync();
                }
                var cf = new CharacterFilm();
                cf.CharacterId = character.CharacterId;
                cf.FilmId = f.FilmId;
                await _uow.CharacterFilms.CreateAsync(cf);
                await _uow.CommitAsync();
            }
        }

        public async Task<IEnumerable<CharacterResponseDto>> GetCharactersContainNameAsync(string name) {
            var characters = await _uow.Characters.GetCharactersByContainNameAsync(name);
            var responseDto = _mapper.Map<List<CharacterResponseDto>>(characters);
            return responseDto;
        }
        public async Task<IEnumerable<CharacterResponseDto>> GetCharactersContainWeightAsync(double weight) {
            var characters = await _uow.Characters.GetCharactersByWeightAsync(weight);
            var responseDto = _mapper.Map<List<CharacterResponseDto>>(characters);
            return responseDto;
        }
        public async Task<IEnumerable<CharacterResponseDto>> GetCharactersContainAgeAsync(int age) {
            var characters = await _uow.Characters.GetCharactersByAgeAsync(age);
            var responseDto = _mapper.Map<List<CharacterResponseDto>>(characters);
            return responseDto;
        }
        public async Task<IEnumerable<CharacterResponseDto>> GetCharactersContainFilmAsync(string film) {
            List<CharacterResponseDto> responseDto = new ();

            var f = await _uow.Films.GetFilmByTitleAsync(film);
            if (f == null) return responseDto;
            var films = await _uow.Films.GetAsync(f.FilmId);                // informacion completa con personajes
            if (films == null) return responseDto;

            var characters = films.CharacterFilms;
            if (characters.Count() > 0) { 
                foreach (var item in characters)
                {
                    CharacterResponseDto resp = new();
                    resp.CharacterId = item.Character.CharacterId;
                    resp.Name = item.Character.Name;
                    resp.PathFile = item.Character.PathFile;
                    responseDto.Add(resp);
                }
            }
            return responseDto;
        }
    }
}
