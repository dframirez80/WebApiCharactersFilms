using System.ComponentModel.DataAnnotations;
using WebApiCharactersFilms.Constants;

namespace WebApiCharactersFilms.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = ErrorMessage.Email)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(Constraints.MaxLengthPassword,
              MinimumLength = Constraints.MinLengthPassword,
              ErrorMessage = "Debe ingresar {0} y {1} caracteres")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(Constraints.MaxLengthPassword,
              MinimumLength = Constraints.MinLengthPassword,
              ErrorMessage = "Debe ingresar {0} y {1} caracteres")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
