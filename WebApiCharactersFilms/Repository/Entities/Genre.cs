using System;
using System.Collections.Generic;

namespace WebApiCharactersFilms.Repository.Entities
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string PathFile { get; set; }
        public ICollection<Film> Films { get; set; }
    }
}
