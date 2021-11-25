using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ICharacterRepository Characters { get; }
        IFilmRepository Films { get; }
        IRepository<CharacterFilm> CharacterFilms { get; }
        IGenreRepository Genres { get; }
        Task CommitAsync();
        void DiscardChanges();
    }
}
