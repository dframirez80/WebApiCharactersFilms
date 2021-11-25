using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCharactersFilms.Models.Dtos
{
    public class CharacterResponseDto
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public string PathFile { get; set; }
    }
}
