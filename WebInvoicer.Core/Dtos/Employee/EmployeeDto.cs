using System;

namespace WebInvoicer.Core.Dtos.Employee
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
