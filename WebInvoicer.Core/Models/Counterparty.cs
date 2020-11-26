using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebInvoicer.Core.Models
{
    public class Counterparty
    {
        [Key]
        public int CounterpartyId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Nip { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(6)]
        [DataType(DataType.PostalCode)]
        public string PostalCode { get; set; }

        [Required]
        public string City { get; set; }
            
        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Counterparties")]
        public virtual ApplicationUser User { get; set; }

        [InverseProperty("Counterparty")]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
