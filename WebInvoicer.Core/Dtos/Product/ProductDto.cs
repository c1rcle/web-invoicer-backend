using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebInvoicer.Core.Models;

namespace WebInvoicer.Core.Dtos.Product
{
    public class ProductDto
    {
        [JsonPropertyName("id")]
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal? NetPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal? GrossPrice { get; set; }

        public VatRate? VatRate { get; set; }
    }
}
