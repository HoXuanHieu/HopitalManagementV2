using HospitalManagement.API.DTOs;
using HospitalManagement.API.Repositories.User;
using HospitalManagement.API.Services.Token;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace HospitalManagement.API.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public EmailService(IConfiguration configuration, IUserRepository userRepository, ITokenService tokenService)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public APIResponse SendEmail(string to, string subject, string body)
        {
            try
            {
                //Create Email 
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_configuration.GetSection("Email:From").Value));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();

                //Config SMTP to send email 
                smtp.Connect(_configuration.GetSection("Email:Host").Value, int.Parse(_configuration.GetSection("Email:Port").Value), SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration.GetSection("Email:From").Value, _configuration.GetSection("Email:Password").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                return new APIResponse() { StatusCode = 200, Message = "Success! Please check your email" };
            }
            catch (Exception e)
            {
                return new APIResponse() { StatusCode = 400, Message = "Faile. " + e.Message, };
            }
        }
    }
}
