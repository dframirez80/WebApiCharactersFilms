using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Models.Dtos
{
    public class CharacterCreateDto
    {
        [Required(ErrorMessage = ErrorMessage.Names)]
        public string Name { get; set; }

        [Required(ErrorMessage = ErrorMessage.YearsOld)]
        [Range(Constraints.MinYearsOld, Constraints.MaxYearsOld)]
        public int YearsOld { get; set; }

        [Required(ErrorMessage = ErrorMessage.Weight)]
        [Range(Constraints.MinWeight, Constraints.MaxWeight)]
        public double Weight { get; set; }

        [Required]
        [MaxLength(Constraints.MaxLengthBiography)]
        public string Biography { get; set; }

        public IFormFile File { get; set; }

        public IEnumerable<string> Films { get; set; }

    }
}
