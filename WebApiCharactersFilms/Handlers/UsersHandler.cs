using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Mail;
using WebApiCharactersFilms.Models;
using WebApiCharactersFilms.Models.Dtos;
using WebApiCharactersFilms.Repository.Entities;
using WebApiCharactersFilms.Repository.IRepositories;
using WebApiCharactersFilms.Security;

namespace WebApiCharactersFilms.Handlers
{
    public class UsersHandler : IUsersHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IMailSender _mailSender;
        private readonly ITokenJwt _tokenJwt;

        public UsersHandler(IUnitOfWork uow, IMapper mapper, IMailSender mailSender, ITokenJwt tokenJwt) {
            _uow = uow;
            _mapper = mapper;
            _mailSender = mailSender;
            _tokenJwt = tokenJwt;
        }

        public async Task<string> ChangeUserPasswordAsync(ChangePassword changePassword) {
            var user = await _uow.Users.GetUserByEmailAsync(changePassword.Email);              // verifica si existe usuario x email
            if (user == null)
                return ErrorMessage.EmailOrPassword;
            if (BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);    // cifrar contraseña
                user.ResetPassword = false;
                await _uow.CommitAsync();
                return Constants.Mail.ChangePassword;
            }
            return ErrorMessage.EmailOrPassword;
        }

        public async Task<string> ConfirmUserAsync(int id, string token) {
            var result = _tokenJwt.ValidateToken(token);
            if (id != result)
                return Constants.Mail.ConfirmFail;
            var user = await _uow.Users.GetAsync(id);
            if (user == null)
                return Constants.Mail.ConfirmFail;
            user.StatusUser = (int)Constraints.StatusUser.Active;
            await _uow.CommitAsync();
            return Constants.Mail.Confirm;
        }

        public async Task<string> LoginUserAsync(Login login) {
            //---------------- solo para pruebas
            if (login.Email == Constants.Mail.EmailInvited && login.Password == Constants.Mail.PassInvited) {
                var moqUser = new User() {
                    Names = Constants.Mail.EmailInvited, Email = Constants.Mail.EmailInvited, UserId = 99999,
                    Role = Roles.User
                };
                var tokenUser = _tokenJwt.GenerateToken(moqUser, TokenItems.ExpireLogin);          // generar token
                return tokenUser;
            }
            //---------------

            var user = await _uow.Users.GetUserByEmailAsync(login.Email);                              // verifica password y correo
            if (user == null)
                return ErrorMessage.UserNotLogin;
            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                return ErrorMessage.UserNotLogin;
            if (user.StatusUser == (int)Constraints.StatusUser.Active)
            {
                if (!user.ResetPassword)
                {
                    var tokenUser = _tokenJwt.GenerateToken(user, TokenItems.ExpireLogin);          // generar token
                    return tokenUser;
                }
                return ErrorMessage.ResetPassword;
            } 
            return ErrorMessage.UserPending;
        }

        public async Task<string> RegisterUserAsync(UserDto userDto, string host) {
            if (userDto == null)
                return string.Empty;
            var user = await _uow.Users.GetUserByEmailAsync(userDto.Email);             // verifica si existe usuario x email
            if (user != null)
            {
                if (user.StatusUser == (int)Constraints.StatusUser.Active)              // verifica si usuario esta activo
                    return ErrorMessage.UserExists;
                else {                                                                  // statusUser= Pending
                    userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);// cifrar contraseña
                    int userId = user.UserId;
                    user = _mapper.Map<User>(userDto);                                  // actualizo DB
                    user.UserId = userId;
                }
            } else{
                userDto.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);// cifrar contraseña
                user = _mapper.Map<User>(userDto);
                await _uow.Users.CreateAsync(user);                                     // crea usuario con estado pendiente
            }
            user.Role = Roles.User;
            user.StatusUser = (int)Constraints.StatusUser.Pending;
            user.Created = DateTime.UtcNow.AddHours(UTC.GmtBuenosAires);
            await _uow.CommitAsync();

            // envio de correo
            var token = _tokenJwt.GenerateToken(user, TokenItems.ExpireRegister);
            string path = $"{host}{user.UserId}/{token}";
            MailRequest mailRequest = new() { ToEmail = user.Email, Subject = Constants.Mail.Subject, Body = path };
            await _mailSender.EmailConfirmRegister(mailRequest);
            return Constants.Mail.Sent;
        }

        public async Task<string> ResetUserPasswordAsync(ResetPassword email) {
            var user = await _uow.Users.GetUserByEmailAsync(email.Email);               // verifica correo
            if (user == null)
                return ErrorMessage.UserNotLogin;
            Random r = new();                                                           // password aleatorio
            var newPass = r.Next(10000000, 90000000);
            string pass = newPass.ToString();
            user.Password = BCrypt.Net.BCrypt.HashPassword(pass);                       // cifrar contraseña
            user.ResetPassword = true;
            await _uow.CommitAsync();

            var token = _tokenJwt.GenerateToken(user, TokenItems.ExpireRegister);       // envio correo
            MailRequest emailRequest = new() { ToEmail = user.Email, Subject = Constants.Mail.Subject, Body = newPass.ToString() };
            await _mailSender.EmailChangePassword(emailRequest);
            return Constants.Mail.NewPassword;
        }
    }
}
