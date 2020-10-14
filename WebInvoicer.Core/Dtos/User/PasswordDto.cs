using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Dtos.User
{
    public class PasswordDto : UserData
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
