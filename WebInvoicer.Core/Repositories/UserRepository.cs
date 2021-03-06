using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Extensions;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IMapper mapper;

        public UserRepository(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<TaskResult<string>> CreateUser(CreateUserDto data)
        {
            var user = await GetUser(data.Email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = data.Email,
                    Email = data.Email,
                    FullName = data.FullName
                };

                var result = await userManager.CreateAsync(user, data.Password);

                if (!result.Succeeded)
                {
                    return new TaskResult<string>(result.GetErrorDescriptions());
                }

                var token = await userManager.GenerateEmailConfirmationToken(user);
                return new TaskResult<string>(token);
            }
            else
            {
                return new TaskResult<string>(new[] { "Account already created" });
            }
        }

        public async Task<TaskResult> ConfirmUser(ConfirmUserDto data)
        {
            var result = await PerformUserAction(data.Email, user =>
                userManager.ConfirmRegistration(user, data.Token), IdentityResult.Failed());

            return result.GetTaskResult();
        }

        public async Task<TaskResult<ApplicationUser>> VerifyPassword(VerifyPasswordDto data)
        {
            var user = await GetUser(data.Email);
            var result = await PerformUserAction(user, () =>
                userManager.Authenticate(user, data.Password), false);

            return result
                ? new TaskResult<ApplicationUser>(user)
                : new TaskResult<ApplicationUser>(new[] { "Incorrect email or password" });
        }

        public async Task<TaskResult<string>> ResetPassword(string email)
        {
            var token = await PerformUserAction(email, user =>
                userManager.GeneratePasswordResetToken(user), "");

            return token != ""
                ? new TaskResult<string>(token)
                : new TaskResult<string>(new[] { "Password could not be reset" });
        }

        public async Task<TaskResult> ChangePassword(PasswordDto data)
        {
            var result = await PerformUserAction(data.Email, user =>
                userManager.ChangePasswordAsync(user, data.Password, data.NewPassword),
                IdentityResult.Failed());

            return result.GetTaskResult();
        }

        public async Task<TaskResult> ChangePassword(PasswordResetDto data)
        {
            var result = await PerformUserAction(data.Email, user =>
                userManager.ResetPasswordAsync(user, data.ResetToken, data.NewPassword),
                IdentityResult.Failed());

            return result.GetTaskResult();
        }

        public async Task<TaskResult> SetCompanyDetails(SetCompanyDetailsDto data, string email)
        {
            var result = await PerformUserAction(email, user =>
            {
                mapper.Map(data, user);
                return userManager.UpdateAsync(user);
            },
            IdentityResult.Failed());

            return result.GetTaskResult();
        }

        private async Task<T> PerformUserAction<T>(
            ApplicationUser user, Func<Task<T>> action, T resultIfNull)
        {
            return user != null ? await action() : resultIfNull;
        }

        private async Task<T> PerformUserAction<T>(
            string email, Func<ApplicationUser, Task<T>> action, T resultIfNull)
        {
            var user = await GetUser(email);
            return user != null ? await action(user) : resultIfNull;
        }

        private async Task<ApplicationUser> GetUser(string email) =>
            await userManager.FindByEmailAsync(email);
    }
}
