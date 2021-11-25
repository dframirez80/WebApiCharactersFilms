using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;

namespace WebApiCharactersFilms.Models.Dtos
{
    public class GenreCreateDto
    {
        [Required(ErrorMessage = ErrorMessage.Names)]
        public string Name { get; set; }
        public IFormFile File { get; set; }

    }
}
