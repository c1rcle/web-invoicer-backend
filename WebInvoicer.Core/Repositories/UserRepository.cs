using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(UserManager<ApplicationUser> userManager) =>
            this.userManager = userManager;

        public async Task<TaskResult> CreateUser(CreateUserDto data)
        {
            var user = new ApplicationUser
            {
                UserName = data.Email,
                Email = data.Email,
                FullName = data.FullName
            };

            var result = await userManager.CreateAsync(user, data.Password);

            if (!result.Succeeded)
            {
                return new TaskResult(false);
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            return new TaskResult(true, token);
        }

        public async Task<TaskResult> ConfirmUser(ConfirmUserDto data)
        {
            var user = await GetUser(data.Email);
            var result = await userManager.ConfirmEmailAsync(user, data.Token);

            return new TaskResult(result.Succeeded);
        }

        public async Task<TaskResult> VerifyPassword(VerifyPasswordDto data)
        {
            var user = await GetUser(data.Email);
            var result = await userManager.CheckPasswordAsync(user, data.Password);

            return new TaskResult(result, user);
        }

        public async Task<TaskResult> ResetPassword(string email)
        {
            var user = await GetUser(email);

            if (user == null)
            {
                return new TaskResult(false);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            return new TaskResult(true, token);
        }

        public async Task<TaskResult> ChangePassword(PasswordDto data)
        {
            var user = await GetUser(data.Email);
            var result = await userManager
                .ChangePasswordAsync(user, data.Password, data.NewPassword);

            return new TaskResult(result.Succeeded);
        }

        public async Task<TaskResult> ChangePassword(PasswordResetDto data)
        {
            var user = await GetUser(data.Email);
            var result = await userManager
                .ResetPasswordAsync(user, data.ResetToken, data.NewPassword);

            return new TaskResult(result.Succeeded);
        }

        private async Task<ApplicationUser> GetUser(string email) =>
            await userManager.FindByEmailAsync(email);
    }
}
