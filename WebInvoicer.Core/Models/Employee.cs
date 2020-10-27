using System;
using System.ComponentModel.DataAnnotations;

namespace WebInvoicer.Core.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
    }
}
