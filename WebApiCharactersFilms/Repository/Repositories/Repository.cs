using WebApiCharactersFilms.Repository.EntityDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.IRepositories;

namespace WebApiCharactersFilms.Repository.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context) {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task CreateAsync(T entity) {
            _dbSet.Add(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id) {
            var entity = await GetAsync(id);
            await DeleteAsync(entity);
        }

        public async Task DeleteAsync(T entity) {
            _context.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync() {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(int id) {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task UpdateAsync(T entity) {
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
