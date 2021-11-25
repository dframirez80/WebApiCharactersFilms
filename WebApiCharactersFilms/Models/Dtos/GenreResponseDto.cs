using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Models.Dtos
{
    public class GenreResponseDto
    {
        public int GenreId { get; set; }

        public string Name { get; set; }
        
        public string PathFile { get; set; }

        public IEnumerable<Film> Films { get; set; }
    }
}
