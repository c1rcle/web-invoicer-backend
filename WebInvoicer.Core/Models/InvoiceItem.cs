using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebInvoicer.Core.Models
{
    public class InvoiceItem
    {
        [Key]
        public int InvoiceItemId { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int? ProductId { get; set; }

        [Required]
        public int Index { get; set; }

        [Required]
        public int? Count { get; set; }

        [Required]
        public string Unit { get; set; }

        [ForeignKey("InvoiceId")]
        [InverseProperty("Items")]
        public virtual Invoice Invoice { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("Items")]
        public virtual Product Product { get; set; }
    }
}
