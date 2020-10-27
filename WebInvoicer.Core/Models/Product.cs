using System.ComponentModel.DataAnnotations;

namespace WebInvoicer.Core.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal NetPrice { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal GrossPrice { get; set; }

        [Required]
        public VatRate VatRate { get; set; }
    }
}
