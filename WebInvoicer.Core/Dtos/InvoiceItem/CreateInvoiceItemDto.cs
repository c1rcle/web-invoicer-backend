using System.ComponentModel.DataAnnotations;

namespace WebInvoicer.Core.Dtos.InvoiceItem
{
    public class CreateInvoiceItemDto
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Index { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Count { get; set; }

        [Required]
        public string Unit { get; set; }
    }
}
