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
    public class FilmRepository : Repository<Film>, IFilmRepository
    {
        private readonly DbSet<Film> _dbSet;
        public FilmRepository(AppDbContext context) : base(context) {
            _dbSet = context.Set<Film>();
        }

        public async Task<Film> GetFilmAllDataAsync(int id) {
            return await _dbSet.Include(i => i.CharacterFilms).FirstOrDefaultAsync(f => f.FilmId == id);
        }

        public async Task<Film> GetFilmByTitleAsync(string title) {
            return await _dbSet.Where(w => w.Title == title).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Film>> GetFilmsOrderAsync(bool order) {
            if (order)
                return await _dbSet.OrderBy(a => a.CreationDate).ToListAsync();
            return await _dbSet.OrderByDescending(a => a.CreationDate).ToListAsync();
        }

        public async Task<IEnumerable<Film>> GetFilmsByContainTitleAsync(string title) {
            return await _dbSet.Where(w => w.Title.Contains(title)).ToListAsync();
        }
    }
}
