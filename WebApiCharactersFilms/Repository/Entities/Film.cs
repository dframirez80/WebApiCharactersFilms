using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCharactersFilms.Repository.Entities
{
    public class Film 
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string CreationDate { get; set; }
        public int Qualification { get; set; }
        public string PathFile { get; set; }
        public IList<CharacterFilm> CharacterFilms { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
