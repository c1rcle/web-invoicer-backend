using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebInvoicer.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        public string FullName { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Counterparty> Counterparties { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Employee> Employees { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
