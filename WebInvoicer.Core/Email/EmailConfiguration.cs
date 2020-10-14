namespace WebInvoicer.Core.Email
{
    public class EmailConfiguration
    {
        public string UrlBase { get; set; }

        public string SmtpServer { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }
    }
}
