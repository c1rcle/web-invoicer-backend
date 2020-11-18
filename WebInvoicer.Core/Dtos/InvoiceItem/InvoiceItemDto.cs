using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebInvoicer.Core.Dtos.InvoiceItem
{
    public class InvoiceItemDto
    {
        [JsonPropertyName("id")]
        public int InvoiceItemId { get; set; }

        public int? ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int? Count { get; set; }

        public string Unit { get; set; }
    }
}
