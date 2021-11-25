using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;
using WebApiCharactersFilms.Repository.EntityDbContext;
using WebApiCharactersFilms.Repository.IRepositories;

namespace WebApiCharactersFilms.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; }
        public ICharacterRepository Characters { get; }
        public IFilmRepository Films { get; }
        public IRepository<CharacterFilm> CharacterFilms { get; }
        public IGenreRepository Genres { get; }

        public UnitOfWork(AppDbContext context) {
            _context = context;
            Users = new UserRepository(_context);
            Characters = new CharacterRepository(_context);
            Films = new FilmRepository(_context);
            Genres = new GenreRepository(_context);
            CharacterFilms = new Repository<CharacterFilm>(_context);
        }
        public async Task CommitAsync() {
            await _context.SaveChangesAsync();
        }

        public void DiscardChanges() {
            _context.ChangeTracker.Clear();
        }

        public void Dispose() {
            _context.Dispose();
        }
    }
}
