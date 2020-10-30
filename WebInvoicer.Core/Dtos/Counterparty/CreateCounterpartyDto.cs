using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Attributes;

namespace WebInvoicer.Core.Dtos.Counterparty
{
    public class CreateCounterpartyDto
    {
        [Required]
        public string Name { get; set; }

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

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
