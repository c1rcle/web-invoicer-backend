using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Attributes;

namespace WebInvoicer.Core.Dtos.User
{
    public class SetCompanyDetailsDto
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        [ValidNip]
        public string Nip { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }
    }
}
