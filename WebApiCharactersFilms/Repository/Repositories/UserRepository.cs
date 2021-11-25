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
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DbSet<User> _dbSet;
        public UserRepository(AppDbContext context) : base(context) {
            _dbSet = context.Set<User>();
        }
        public async Task<User> GetUserByEmailAsync(string email) {
            return await _dbSet.Where(w => w.Email == email).FirstOrDefaultAsync();
        }
    }
}
