using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Models;

namespace WebInvoicer.Core.Repositories
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUser(CreateUserDto data);

        Task<IdentityResult> ConfirmUser(ConfirmUserDto data);

        Task<ApplicationUser> VerifyPassword(VerifyPasswordDto data);

        Task<string> ResetPassword(string email);

        Task<IdentityResult> ChangePassword(PasswordDto data);

        Task<IdentityResult> ChangePassword(PasswordResetDto data);
    }
}
