using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Dtos.User
{
    public class ConfirmUserDto : UserData
    {
        [Required]
        public string Token { get; set; }
    }
}
