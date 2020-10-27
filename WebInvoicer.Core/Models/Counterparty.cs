using System.ComponentModel.DataAnnotations;

namespace WebInvoicer.Core.Models
{
    public class Counterparty
    {
        [Key]
        public int CounterpartyId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
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
