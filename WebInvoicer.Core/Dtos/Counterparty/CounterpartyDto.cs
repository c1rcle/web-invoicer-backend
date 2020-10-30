using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Attributes;

namespace WebInvoicer.Core.Dtos.Counterparty
{
    public class CounterpartyDto
    {
        public int CounterpartyId { get; set; }

        public string Name { get; set; }

        [ValidNip]
        public string Nip { get; set; }

        public string Address { get; set; }

        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        public string City { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
