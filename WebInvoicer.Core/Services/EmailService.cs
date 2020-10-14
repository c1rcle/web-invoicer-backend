using System;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using WebInvoicer.Core.Email;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration emailConfiguration;

        private readonly IHttpContextAccessor contextAccessor;

        public EmailService(IOptions<EmailConfiguration> emailConfiguration,
            IHttpContextAccessor contextAccessor)
        {
            this.emailConfiguration = emailConfiguration.Value;
            this.contextAccessor = contextAccessor;
        }

        public async Task<TaskResult> SendForTaskResult(TaskResult result,
            MessageData data)
        {
            if (!result.Success)
            {
                return result;
            }

            var url = data.GetEndpointUrl(emailConfiguration.UrlBase, result.Payload as string);
            var message = EmailGenerator.GenerateMessage(data.Recipient, data.Type, url);

            try
            {
                await SendEmail(message);
                return new TaskResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new TaskResult(false);
            }
        }

        public async Task SendEmail(EmailMessage message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Web Invoicer", emailConfiguration.SmtpUsername));
            email.To.Add(MailboxAddress.Parse(message.Recipient));
            email.Subject = message.Subject;

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = message.Content
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback =
                    (sender, certificate, certChainType, errors) => true;
                await client.ConnectAsync(emailConfiguration.SmtpServer,
                    emailConfiguration.SmtpPort, true, GetCancellationToken());
                await client.AuthenticateAsync(emailConfiguration.SmtpUsername,
                    emailConfiguration.SmtpPassword, GetCancellationToken());

                await client.SendAsync(email, GetCancellationToken());
                await client.DisconnectAsync(true, GetCancellationToken());
            }
        }

        private CancellationToken GetCancellationToken()
        {
            return contextAccessor.HttpContext.RequestAborted;
        }
    }
}
