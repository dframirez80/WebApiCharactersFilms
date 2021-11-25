using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;

namespace WebApiCharactersFilms.Models
{
    public class ResetPassword
    {
        [Required(ErrorMessage = ErrorMessage.Email)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
