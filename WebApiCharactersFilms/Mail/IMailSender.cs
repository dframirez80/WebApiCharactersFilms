using System.Threading.Tasks;
using WebApiCharactersFilms.Models;

namespace WebApiCharactersFilms.Mail
{
    public interface IMailSender
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task EmailChangePassword(MailRequest mailRequest);
        Task EmailConfirmRegister(MailRequest mailRequest);

    }

}
