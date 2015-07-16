using System;
namespace HomeWork3Common.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailModel email);
    }
}
