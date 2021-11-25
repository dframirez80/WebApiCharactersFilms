using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCharactersFilms.Models.Dtos
{
    public class FilmDetailDto
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string CreationDate { get; set; }
        public int Qualification { get; set; }
        public string PathFile { get; set; }
        public int GenreId { get; set; }
        public string Genre { get; set; }
        public IList<string> Characters { get; set; }
    }
}
