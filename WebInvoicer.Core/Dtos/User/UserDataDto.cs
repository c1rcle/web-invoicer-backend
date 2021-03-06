using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Dtos.User
{
    public class UserDataDto : UserData
    {
        public string FullName { get; set; }

        public string CompanyName { get; set; }

        public string Nip { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }
}
