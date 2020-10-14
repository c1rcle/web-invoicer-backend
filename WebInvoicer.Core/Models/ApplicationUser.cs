using Microsoft.AspNetCore.Identity;

namespace WebInvoicer.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public string Language { get; set; }
    }
}
