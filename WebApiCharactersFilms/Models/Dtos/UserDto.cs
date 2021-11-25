using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;

namespace WebApiCharactersFilms.Models.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = ErrorMessage.Names)]
        public string Names { get; set; }

        [Required(ErrorMessage = ErrorMessage.Surnames)]
        public string Surnames { get; set; }

        [Required(ErrorMessage = ErrorMessage.Email)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(Constraints.MaxLengthPassword,
                      MinimumLength = Constraints.MinLengthPassword,
                      ErrorMessage = "Debe ingresar {0} y {1} caracteres")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = ErrorMessage.PasswordInvalid)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
