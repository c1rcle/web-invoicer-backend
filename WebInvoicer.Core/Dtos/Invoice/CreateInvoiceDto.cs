using System;
using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Models;

namespace WebInvoicer.Core.Dtos.Invoice
{
    public class CreateInvoiceDto
    {
        [Required]
        public InvoiceType? Type { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public int? EmployeeId { get; set; }

        public int? CounterpartyId { get; set; }

        public PaymentType? PaymentType { get; set; }

        public DateTime? PaymentDeadline { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal? NetTotal { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal? GrossTotal { get; set; }
    }
}
