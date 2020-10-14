using System.ComponentModel.DataAnnotations;

namespace WebInvoicer.Core.Utility
{
    public abstract class UserData
    {
        [Required]
        public string Email { get; set; }
    }
}
