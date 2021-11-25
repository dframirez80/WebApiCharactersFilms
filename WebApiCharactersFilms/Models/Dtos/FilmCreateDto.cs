using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Constants;

namespace WebApiCharactersFilms.Models.Dtos
{
    public class FilmCreateDto
    {
        [Required(ErrorMessage = ErrorMessage.Title)]
        public string Title { get; set; }
        
        [Required]
        public string CreationDate { get; set; }

        [Required(ErrorMessage = ErrorMessage.Qualification)]
        [Range(Constraints.MinQualification, Constraints.MaxQualification)]
        public int Qualification { get; set; }

        public IFormFile File { get; set; }

        public IList<string> Characters { get; set; }
        public int GenreId { get; set; }
    }
}
