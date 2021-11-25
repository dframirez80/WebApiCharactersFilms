using AutoMapper;
using System.Collections.Generic;
using WebApiCharactersFilms.Models.Dtos;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Profiles
{
    public class AppProfiles : Profile
    {
        public AppProfiles() {
            //CreateMap<OriginModel, DestinationModel>();
            CreateMap<UserDto, User>();

            CreateMap<CharacterDetailDto, Character>();
            CreateMap<CharacterCreateDto, Character>();
            CreateMap<Character, CharacterResponseDto>();
            CreateMap<Character, CharacterUpdateDto>();

            CreateMap<FilmDetailDto, Film>();
            CreateMap<FilmCreateDto, Film>();
            CreateMap<Film, FilmResponseDto>();
            CreateMap<Film, FilmUpdateDto>();

            CreateMap<GenreUpdateDto, Genre>();
            CreateMap<GenreCreateDto, Genre>();
            CreateMap<Genre, GenreResponseDto>();
        }
    }
}
