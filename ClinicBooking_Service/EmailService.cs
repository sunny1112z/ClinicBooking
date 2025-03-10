using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ClinicBooking.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Đọc cấu hình SMTP
            var smtpServer = _configuration["SmtpSettings:Server"];
            if (!int.TryParse(_configuration["SmtpSettings:Port"], out int smtpPort))
            {
                throw new Exception("SMTP Port is not configured properly in appsettings.json.");
            }
            var senderEmail = _configuration["SmtpSettings:SenderEmail"];
            var senderPassword = _configuration["SmtpSettings:SenderPassword"];
            var enableSSL = bool.Parse(_configuration["SmtpSettings:EnableSSL"] ?? "true");

            using (var client = new SmtpClient(smtpServer))
            {
                client.Port = smtpPort;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.EnableSsl = enableSSL;

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(senderEmail);
                    mailMessage.To.Add(toEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    try
                    {
                        await client.SendMailAsync(mailMessage);
                        Console.WriteLine($"✅ Email sent to {toEmail}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Failed to send email: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task SendResetPasswordEmail(string toEmail, string resetLink)
        {
            string subject = "🔒 Reset Your Password";
            string body = $@"
                <p>Hello,</p>
                <p>You requested to reset your password. Click the link below to reset it:</p>
                <p><a href='{resetLink}'>🔗 Reset Password</a></p>
                <p><b>This link will expire in 1 hour.</b></p>
                <p>If you did not request this, please ignore this email.</p>";

            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
