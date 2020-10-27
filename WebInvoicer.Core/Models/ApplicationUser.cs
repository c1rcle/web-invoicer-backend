using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebInvoicer.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        public string FullName { get; set; }
    }
}
