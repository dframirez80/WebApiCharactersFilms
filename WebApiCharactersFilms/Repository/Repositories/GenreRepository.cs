using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;
using WebApiCharactersFilms.Repository.EntityDbContext;
using WebApiCharactersFilms.Repository.IRepositories;

namespace WebApiCharactersFilms.Repository.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        private readonly DbSet<Genre> _dbSet;
        public GenreRepository(AppDbContext context) : base(context) {
            _dbSet = context.Set<Genre>();
        }

        public async Task<Genre> GetGenreAllDataAsync(int id) {
            return await _dbSet.Include(i => i.Films).FirstOrDefaultAsync(f => f.GenreId == id);
        }

        public async Task<Genre> GetGenreByNameAsync(string name) {
            return await _dbSet.Where(w => w.Name == name).FirstOrDefaultAsync();
        }
        public async Task<int> GetFirstGenreAsync() {
            var genre = await _dbSet.FirstOrDefaultAsync();
            return genre.GenreId;
        }
    }
}
