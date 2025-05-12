using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace TourismWeb.Helpers
{
    public class EmailSendingHelper
    {
        private static readonly Lazy<EmailSendingHelper> _instance =
           new Lazy<EmailSendingHelper>(() => new EmailSendingHelper());

        public static EmailSendingHelper Instance => _instance.Value;

        private EmailSendingHelper()
        {
        }
        private IConfiguration _configuration;

        public void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendConfirmationEmailAsync(string toEmail, string otp)
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration is not set for EmailSendingHelper.");

            try
            {
                var smtpHost = _configuration["EmailSettings:Host"];
                var smtpPort = int.Parse(_configuration["EmailSettings:Port"]);
                var smtpUser = _configuration["EmailSettings:UserName"];
                var smtpPass = _configuration["EmailSettings:Password"];
                var fromEmail = _configuration["EmailSettings:From"];
                var frontendUrl = _configuration["Frontend:ConfirmUrl"];

                var confirmationLink = $"{frontendUrl}?email={WebUtility.UrlEncode(toEmail)}&otp={WebUtility.UrlEncode(otp)}";

                var subject = "Xác nhận tài khoản của bạn";
                var body = $@"
                    <p>Xin chào,</p>
                    <p>Bạn đã đăng ký tài khoản tại TourismWeb. Vui lòng nhấn vào liên kết dưới đây để xác nhận tài khoản của bạn:</p>
                    <p><a href='{confirmationLink}'>Xác nhận tài khoản</a></p>
                    <p>Nếu bạn không đăng ký tài khoản, vui lòng bỏ qua email này.</p>
                    <p>Trân trọng,</p>
                    <p>TourismWeb Team</p>";

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                var message = new MailMessage(fromEmail, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendOTP(string toEmail, string otp)
        {
            if (_configuration == null)
                throw new InvalidOperationException("Configuration is not set for EmailSendingHelper.");
            try
            {
                var smtpHost = _configuration["EmailSettings:Host"];
                var smtpPort = int.Parse(_configuration["EmailSettings:Port"]);
                var smtpUser = _configuration["EmailSettings:UserName"];
                var smtpPass = _configuration["EmailSettings:Password"];
                var fromEmail = _configuration["EmailSettings:From"];


                var subject = "Lấy lại mật khẩu";
                var body = $@"
                    <p>Xin chào,</p>
                    <p>Bạn đã gửi yêu cầu lấy lại mật khẩu tại TourismWeb. Mã Otp để lấy lại mật khẩu là:</p>
                    <p><b>{otp}</b></p>
                    <p>Vui lòng không chia sẻ mã này cho người khác</p>
                    <p>Trân trọng,</p>
                    <p>TourismWeb Team</p>";

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                    EnableSsl = true
                };

                var message = new MailMessage(fromEmail, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
