using WebApiCharactersFilms.Repository.EntityDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.IRepositories;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.Repositories
{
    public class CharacterFilmRepository : ICharacterFilmRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<CharacterFilm> _dbSet;

        public CharacterFilmRepository(AppDbContext context) {
            _context = context;
            _dbSet = context.Set<CharacterFilm>();
        }
        public async Task CreateAsync(CharacterFilm entity) {
            _dbSet.Add(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id) {
            var entity = await GetAsync(id);
            await DeleteAsync(entity);
        }

        public async Task DeleteAsync(CharacterFilm entity) {
            _context.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<CharacterFilm>> GetAllAsync() {
            return await _dbSet.ToListAsync();
        }

        public async Task<CharacterFilm> GetAsync(int id) {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task UpdateAsync(CharacterFilm entity) {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        // Dispose
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing) {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
