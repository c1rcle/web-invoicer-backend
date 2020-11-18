using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebInvoicer.Core.Models;

namespace WebInvoicer.Core.Dtos.Invoice
{
    public class InvoiceDto
    {
        [JsonPropertyName("id")]
        public int InvoiceId { get; set; }

        [Required]
        public InvoiceType? Type { get; set; }

        public string Number { get; set; }

        public DateTime? Date { get; set; }

        public int? EmployeeId { get; set; }

        public int? CounterpartyId { get; set; }

        public PaymentType? PaymentType { get; set; }

        public DateTime? PaymentDeadline { get; set; }

        [DataType(DataType.Currency)]
        public decimal? NetTotal { get; set; }

        [DataType(DataType.Currency)]
        public decimal? GrossTotal { get; set; }
    }
}
