using System.ComponentModel.DataAnnotations;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Dtos.User
{
    public class CreateUserDto : UserData
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
