using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebInvoicer.Core.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Employees")]
        public virtual ApplicationUser User { get; set; }

        [InverseProperty("Employee")]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
