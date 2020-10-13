using Microsoft.AspNetCore.Identity;

namespace WebInvoicer.Core.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        public string FullName;
    }
}
