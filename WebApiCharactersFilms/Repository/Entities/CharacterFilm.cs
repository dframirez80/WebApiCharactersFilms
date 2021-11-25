using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCharactersFilms.Repository.Entities
{
    public class CharacterFilm
    {
        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public int FilmId { get; set; }
        public Film Film { get; set; }
    }
}
