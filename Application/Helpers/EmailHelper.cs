using Application.AppSettingConfigurations;
using System.Net;
using System.Net.Mail;

namespace Application.Helpers
{
    public class EmailHelper
    {
        public static async Task SendEmailAsync(EmailSettings _emailSettings, string toEmail, string subject, string body)
        {

            var mail = new MailMessage()
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(toEmail);
            //using ở đây sẽ biến smtp thành try finally luôn gọi hàm dispose để giải phóng tài nguyên dù có chuyện gì xảy ra
            using var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
