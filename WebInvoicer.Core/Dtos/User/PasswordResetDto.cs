using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Dtos.User
{
    public class PasswordResetDto : UserData
    {
        [Required]
        public string ResetToken { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
