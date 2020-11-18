using System.Threading.Tasks;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Models;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories
{
    public interface IUserRepository
    {
        Task<TaskResult<string>> CreateUser(CreateUserDto data);

        Task<TaskResult> ConfirmUser(ConfirmUserDto data);

        Task<TaskResult<ApplicationUser>> VerifyPassword(VerifyPasswordDto data);

        Task<TaskResult<string>> ResetPassword(string email);

        Task<TaskResult> ChangePassword(PasswordDto data);

        Task<TaskResult> ChangePassword(PasswordResetDto data);

        Task<TaskResult> SetCompanyDetails(SetCompanyDetailsDto data, string email);
    }
}
