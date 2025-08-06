using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace GoDecola.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendForgotPasswordEmailAsync(string toEmail, string userName, string resetLink)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = "GoDecola - Redefinição de Senha";

            string emailTemplate = await File.ReadAllTextAsync("Templates/ForgotPasswordEmail.html");
            string emailBody = emailTemplate
                .Replace("{UserName}", userName)
                .Replace("{ResetLink}", resetLink);

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = emailBody
            };

            using var smtp = new SmtpClient();
            int port = int.Parse(_config["EmailSettings:Port"]);

            if (_config["EmailSettings:SmtpServer"] == "localhost")
            {
                // conecta sem seguranca para o smtp4dev
                await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], port, SecureSocketOptions.None);
            }
            else
            {
                // conecta com TlS para servidores de producao
                await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
            }

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:From"]));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = htmlContent
            };

            using var smtp = new SmtpClient();
            int port = int.Parse(_config["EmailSettings:Port"]);

            if (_config["EmailSettings:SmtpServer"] == "localhost")
            {
               
                await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], port, SecureSocketOptions.None);
            }
            else
            {
                
                await smtp.ConnectAsync(_config["EmailSettings:SmtpServer"], port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
            }

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendPaymentVoucherAsync(string to, string guestName, string voucherUrl, long? amountPaid)
        {
            var subject = "Comprovante de Pagamento - GoDecola";
            var htmlContent = $@"
            <html>
                <body>
                    <h2>Olá {guestName},</h2>
                    <p>Seu pagamento de <strong>R$ {amountPaid / 100.0:F2}</strong> foi confirmado!</p>
                    <p>Você pode acessar seu comprovante clicando no link abaixo:</p>
                    <a href='{voucherUrl}'>Ver Comprovante</a>
                    <br/><br/>
                    <p>Obrigado por viajar com a GoDecola!</p>
                </body>
            </html>";

            await SendEmailAsync(to, subject, htmlContent);
        }
    }

}

