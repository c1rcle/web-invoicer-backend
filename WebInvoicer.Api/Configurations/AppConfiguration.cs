using WebInvoicer.Core.Email;
using WebInvoicer.Core.Gus;
using WebInvoicer.Core.Token;

namespace WebInvoicer.Api.Configurations
{
    public class AppConfiguration
    {
        public EmailConfiguration EmailConfig { get; set; }

        public TokenConfiguration TokenConfig { get; set; }

        public GusConfiguration GusConfig { get; set; }

        public string ConnectionString { get; set; }
    }
}
