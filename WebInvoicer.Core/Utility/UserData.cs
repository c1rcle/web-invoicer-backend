using System.ComponentModel.DataAnnotations;

namespace WebInvoicer.Core.Utility
{
    public class UserData
    {
        [Required]
        public string Email { get; set; }
    }
}
