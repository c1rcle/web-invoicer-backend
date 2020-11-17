using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebInvoicer.Core.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal? NetPrice { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal? GrossPrice { get; set; }

        [Required]
        public VatRate? VatRate { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Products")]
        public virtual ApplicationUser User { get; set; }

        [InverseProperty("Product")]
        public virtual ICollection<InvoiceItem> Items { get; set; }
    }
}
