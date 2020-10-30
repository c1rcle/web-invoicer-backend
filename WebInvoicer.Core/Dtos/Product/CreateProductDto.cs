using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Models;

namespace WebInvoicer.Core.Dtos.Product
{
    public class CreateProductDto
    {
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
