using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Dtos.User
{
    public class VerifyPasswordDto : UserData
    {
        [Required]
        public string Password { get; set; }
    }
}
