using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Models;

namespace WebInvoicer.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(UserManager<ApplicationUser> userManager) =>
            this.userManager = userManager;

        public async Task<IdentityResult> CreateUser(CreateUserDto data)
        {
            var user = new ApplicationUser
            {
                UserName = data.Email,
                Language = data.Language,
                Email = data.Email,
                FullName = data.FullName
            };

            return await userManager.CreateAsync(user, data.Password);
        }

        public async Task<IdentityResult> ConfirmUser(ConfirmUserDto data)
        {
            var user = await GetUser(data.Email);
            return await userManager.ConfirmEmailAsync(user, data.Token);
        }

        public async Task<ApplicationUser> VerifyPassword(VerifyPasswordDto data)
        {
            var user = await GetUser(data.Email);
            return await userManager.CheckPasswordAsync(user, data.Password) ? user : null;
        }

        public async Task<string> ResetPassword(string email)
        {
            var user = await GetUser(email);

            if (user == null)
            {
                return null;
            }

            return await userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ChangePassword(PasswordDto data)
        {
            var user = await GetUser(data.Email);
            return await userManager.ChangePasswordAsync(user, data.Password, data.NewPassword);
        }

        public async Task<IdentityResult> ChangePassword(PasswordResetDto data)
        {
            var user = await GetUser(data.Email);
            return await userManager.ResetPasswordAsync(user, data.ResetToken, data.NewPassword);
        }

        private async Task<ApplicationUser> GetUser(string email) =>
            await userManager.FindByEmailAsync(email);
    }
}
