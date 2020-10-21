using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WebInvoicer.Core.Email
{
    public class MessageData
    {
        public string Recipient { get; }

        public MessageType Type { get; }

        public MessageData(string recipient, MessageType type)
        {
            Recipient = recipient;
            Type = type;
        }

        public string GetEndpointUrl(string baseUrl, string token = null)
        {
            if (token == null)
            {
                return $"{baseUrl}/login";
            }

            var endpoint = Type switch
            {
                MessageType.Confirmation => "confirmEmail",
                _ => "changePassword"
            };

            var queryParams = new Dictionary<string, string>
            {
                { "token", token },
                { "email", Recipient }
            };

            return $"{baseUrl}/{endpoint}{QueryString.Create(queryParams)}";
        }
    }
}
