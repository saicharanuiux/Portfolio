using Microsoft.AspNetCore.Mvc;
using Portfolio.Modals;

namespace Portfolio.Services
{
    public interface IEmailService
    {
        void SendEmail(Email email);

        Status SendConfirmationEmail(Email email);
    }
}
