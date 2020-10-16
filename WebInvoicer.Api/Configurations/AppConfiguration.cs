using WebInvoicer.Core.Email;
using WebInvoicer.Core.Token;

namespace WebInvoicer.Api.Configurations
{
    public class AppConfiguration
    {
        public EmailConfiguration EmailConfig { get; set; }

        public TokenConfiguration TokenConfig { get; set; }

        public string ConnectionString { get; set; }
    }
}
