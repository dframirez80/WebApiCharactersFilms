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
    public class CharacterRepository : Repository<Character>, ICharacterRepository
    {
        private readonly DbSet<Character> _dbSet;
        public CharacterRepository(AppDbContext context) : base(context) {
            _dbSet = context.Set<Character>();
        }
        public async Task<Character> GetCharacterByNameAsync(string name) {
            return await _dbSet.Where(w => w.Name == name).FirstOrDefaultAsync();
        }
        public async Task<Character> GetCharacterAllDataAsync(int id) {
            return await _dbSet.Include(i=>i.CharacterFilms).FirstOrDefaultAsync(f => f.CharacterId == id);
        }

        public async Task<IEnumerable<Character>> GetCharactersByWeightAsync(double weight) {
            var resp =  await _dbSet.Where(f => f.Weight == weight).OrderBy(o => o.CharacterId).ToListAsync();
            return resp;
        }
        public async Task<IEnumerable<Character>> GetCharactersByAgeAsync(int age) {
            var resp = await _dbSet.Where(f => f.YearsOld == age).OrderBy(o => o.CharacterId).ToListAsync();
            return resp;
        }
        public async Task<IEnumerable<Character>> GetCharactersByContainNameAsync(string name) {
            var characters = await _dbSet.Where(w => w.Name.Contains(name))
                                            .OrderBy(o => o.CharacterId)
                                            .ToListAsync();
            return characters;
        }
    }
}
