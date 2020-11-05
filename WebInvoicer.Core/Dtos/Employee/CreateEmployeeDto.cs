using System.ComponentModel.DataAnnotations;

namespace WebInvoicer.Core.Dtos.Employee
{
    public class CreateEmployeeDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
