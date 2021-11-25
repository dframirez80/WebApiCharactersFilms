using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCharactersFilms.Constants
{
    public static class ErrorMessage
    {
        public const string UserId = "Debe ingresar Id de usuario";
        public const string Names = "Debe ingresar nombre";
        public const string Surnames = "Debe ingresar Apellido";
        public const string Email = "Debe ingresar un correo valido";
        public const string Title = "Debe ingresar titulo";
        public const string Content = "Debe ingresar contenido";

        public const string EmailOrPassword = "Debe ingresar un correo/contraseña valido";
        public const string UserExists = "El correo ya existe.";
        public const string UserBlocked = "El usuario se encuentra bloquedao, contacte al Administrador.";
        public const string UserNotLogin = "El correo o la contraseña no es valida.";
        public const string UserPending = "El correo todavia no fue verificado, revise su correo en la carpeta de spam.";
        public const string ResetPassword = "Debe cambiar la contraseña";
        public const string InfoInvalid = "La informacion es invalida";
        public const string PasswordInvalid = "La contraseña debe tener una mayuscula, un numero y un caracter especial";
        public const string ExistsCharacter = "El personaje ya existe.";
        public const string ExistsGenre = "El genero ya existe.";
        public const string ExistsFilm = "la pelicula/series ya existe.";

        public const string Qualification = "La calificacion debe ser del 1 al 5";
        public const string YearsOld = "La edad debe ser del 1 al 100";
        public const string Weight = "El peso debe ser del 1 al 200";
        
        public const string Image = "Debe colocar una Imagen";
        public const string MaxLengthFile = "la imagen debe ser menor a 2MB";
        public const string Extensions = "Solo acepta extensiones jpg, jpeg o png";


    }
}
