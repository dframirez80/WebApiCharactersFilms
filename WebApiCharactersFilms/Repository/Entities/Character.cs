using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCharactersFilms.Repository.Entities
{
    public class Character
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public int YearsOld { get; set; }
        public double Weight { get; set; }
        public string Biography { get; set; }
        public string PathFile { get; set; }
        public IEnumerable<CharacterFilm> CharacterFilms { get; set; }
    }
}
