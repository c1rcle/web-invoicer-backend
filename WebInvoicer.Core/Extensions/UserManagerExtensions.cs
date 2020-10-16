using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebInvoicer.Core.Models;

namespace WebInvoicer.Core.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<bool> Authenticate(
            this UserManager<ApplicationUser> userManager,
            ApplicationUser user,
            string password)
        {
            var passwordValid = await userManager.CheckPasswordAsync(user, password);
            return user.EmailConfirmed && passwordValid;
        }

        public static async Task<IdentityResult> ConfirmRegistration(
            this UserManager<ApplicationUser> userManager,
            ApplicationUser user,
            string token)
        {
            return user.EmailConfirmed
                ? IdentityResult.Failed(new[] { new IdentityError { Description = "Invalid token." } })
                : await userManager.ConfirmEmailAsync(user, token);
        }
    }
}
