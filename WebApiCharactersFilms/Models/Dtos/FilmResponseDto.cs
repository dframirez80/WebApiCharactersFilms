using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;

namespace WebApiCharactersFilms.Models.Dtos
{
    public class FilmResponseDto
    {
        public int FilmId { get; set; }

        public string Title { get; set; }
        
        public string CreationDate { get; set; }

        public string PathFile { get; set; }
    }
}
