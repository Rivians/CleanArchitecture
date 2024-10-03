using CleanArchitecture.Application.Services;
using FluentEmail.Core;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public sealed class EmailService : IEmailService
    {
        public EmailService()
        {
            var sender = new SmtpSender(() => new System.Net.Mail.SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new System.Net.NetworkCredential("nexorevolutionn@gmail.com", "cppe pxyy nrkx lfsq"),
                EnableSsl = true                
            });

            Email.DefaultSender = sender; // FluentEmail ile e-posta göndermek istediğinizde, her seferinde SMTP ayarlarını (sunucu adı, port, kimlik doğrulama bilgileri) yeniden ayarlamak zorunda kalmamak için bu satırı kullanarak, tüm uygulama genelinde geçerli olacak bir varsayılan gönderici tanımlarsınız.
        }


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var response = Email
                .From("nexorevolutionn@gmail.com")
                .To(email)
                .Subject(subject)
                .Body(message)
                .SendAsync();

            if (!response.Result.Successful)
            {
                throw new Exception("Email gönderilemedi : " + response.Result.ErrorMessages.ToString());
            }
        }
    }
}
