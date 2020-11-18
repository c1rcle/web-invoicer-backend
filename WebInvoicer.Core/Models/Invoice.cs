using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebInvoicer.Core.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int? EmployeeId { get; set; }

        public int? CounterpartyId { get; set; }

        [Required]
        public InvoiceType Type { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        public PaymentType? PaymentType { get; set; }

        public DateTime? PaymentDeadline { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal? NetTotal { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal? GrossTotal { get; set; }

        [InverseProperty("Invoice")]
        public virtual ICollection<InvoiceItem> Items { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Invoices")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("EmployeeId")]
        [InverseProperty("Invoices")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("CounterpartyId")]
        [InverseProperty("Invoices")]
        public virtual Counterparty Counterparty { get; set; }
    }
}
