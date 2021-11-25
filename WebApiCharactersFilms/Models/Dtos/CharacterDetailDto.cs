using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Models.Dtos
{
    public class CharacterDetailDto
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public int YearsOld { get; set; }
        public double Weight { get; set; }
        public string Biography { get; set; }
        public string PathFile { get; set; }
        public IList<string> Films { get; set; }
    }
}
