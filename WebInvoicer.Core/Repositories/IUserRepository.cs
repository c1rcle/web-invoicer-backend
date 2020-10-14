using System.Threading.Tasks;
using WebInvoicer.Core.Dtos.User;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Repositories
{
    public interface IUserRepository
    {
        Task<TaskResult> CreateUser(CreateUserDto data);

        Task<TaskResult> ConfirmUser(ConfirmUserDto data);

        Task<TaskResult> VerifyPassword(VerifyPasswordDto data);

        Task<TaskResult> ResetPassword(string email);

        Task<TaskResult> ChangePassword(PasswordDto data);

        Task<TaskResult> ChangePassword(PasswordResetDto data);
    }
}
