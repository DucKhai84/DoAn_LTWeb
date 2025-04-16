using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DoAn_LTWeb.Repositories
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["EmailSettings:SmtpUsername"]; 
                var smtpPassword = _configuration["EmailSettings:SmtpPassword"]; 
                var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"] ?? "true");

                Console.WriteLine("📨 Preparing to send email...");
                Console.WriteLine($"SMTP Server: {smtpServer}");
                Console.WriteLine($"SMTP Port: {smtpPort}");
                Console.WriteLine($"SMTP User: {smtpUsername}");

                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = enableSsl,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("✅ Email sent successfully to " + email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Failed to send email:");
                Console.WriteLine($"Exception: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException.Message}");
                }
                throw; // để Identity cũng biết bị lỗi nếu cần
            }
        }

        public class EmailSettings
        {
            public string SmtpServer { get; set; }
            public int Port { get; set; }
            public string SmtpUsername { get; set; } // Sửa tên property cho đồng bộ
            public string SmtpPassword { get; set; }
            public bool EnableSsl { get; set; }
        }
    }
}
