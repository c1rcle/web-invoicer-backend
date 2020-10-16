using System.IO;

namespace WebInvoicer.Core.Email
{
    public class EmailGenerator
    {
        public static EmailMessage GenerateMessage(string recipient, MessageType type, string url)
        {
            var messageData = type switch
            {
                MessageType.Confirmation =>
                    new
                    {
                        FileName = "EmailTemplates/ConfirmTemplate.html",
                        Subject = "Confirm your email address"
                    },
                MessageType.PasswordReset =>
                    new
                    {
                        FileName = "EmailTemplates/PasswordResetTemplate.html",
                        Subject = "Reset your password"
                    },
                _ =>
                    new
                    {
                        FileName = "EmailTemplates/PasswordChangeTemplate.html",
                        Subject = "Password change notification"
                    }
            };

            var messageBody = File.ReadAllText(messageData.FileName).Replace("{url}", url);

            return new EmailMessage()
            {
                Recipient = recipient,
                Subject = messageData.Subject,
                Content = messageBody
            };
        }
    }
}
