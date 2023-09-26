using System.Net.Mail;
using System.Net;

namespace charity_website_backend.Common.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailSendDTO emailSendDTO);
    }
    public class EmailSendDTO
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration congf)
        {
            _configuration = congf;
        }
        public void SendEmail(EmailSendDTO emailSendDTO)
        {

            var SenderEmail = _configuration["MailService:EmailFrom"];
            var SenderPassword = _configuration["MailService:EmailPassword"];
            var EmailHost = _configuration["MailService:EmailHost"];
            var Port = int.Parse(_configuration["MailService:Port"]);
            var message = new MailMessage();
            message.From = new MailAddress(SenderEmail, "GC");
            message.To.Add(new MailAddress(emailSendDTO.To));
            message.Subject = emailSendDTO.Subject;
            message.Body = emailSendDTO.Body;
            message.IsBodyHtml = true; // Set this to indicate that the body is HTML

            using (var client = new SmtpClient(EmailHost, Port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                client.EnableSsl = true;
                try
                {

                    client.Send(message);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
    }
}
